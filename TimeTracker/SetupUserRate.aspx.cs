using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeTracker.Model;
using System.Data;

namespace TimeTracker
{
    public partial class SetupUserRate : System.Web.UI.Page
    {
        RolesModuleAccess myAccessRights = new RolesModuleAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            JobTracker jobtracker = new JobTracker();
            if (!isValidUser() || (!jobtracker.CanConnectToCAP()))
                Response.Redirect("Login.aspx");
            GetMyAccessRights();
            if (myAccessRights == null)
                Response.Redirect("Dashboard.aspx");
            HttpContext.Current.Session["siteSubHeader"] = "Setup";
            HttpContext.Current.Session["selectedTab"] = "Setup";

            if (!IsPostBack)
            {
                modalTxtBoxStartDate.Attributes.Add("readonly", "readonly"); //prevents user from typing on calendar textbox
                InitializeDropDownDepartment();
                InitializeGridUser();
                calendarExtenderStartDate.EndDate = DateTime.Today;
                calendarExtenderPatternStartDate.EndDate = DateTime.Today;
            }
        }

        #region INITIALIZE
        protected void InitializeDropDownDepartment()
        {
            Department department = new Department();
            var departmentList = department.GetDepartmentList();
            if (departmentList.Count > 0)
            {
                if (departmentList[0].Description == "All")
                    departmentList.RemoveAt(0);
            }
            department.Id = 0;
            department.Description = "All";
            departmentList.Insert(0, department);
            dropDownListDepartment.DataSource = departmentList;
            dropDownListDepartment.DataTextField = "Description";
            dropDownListDepartment.DataValueField = "Id";
            dropDownListDepartment.DataBind();
        }

        protected void InitializeGridUser()
        {
            //int userid = Convert.ToInt32(Session["UserId"]);
            GetMyAccessRights();
            User user = new User();

            List<User> userlist = new List<User>();
            if (dropDownListDepartment.SelectedItem.Text == "All")
            {
                userlist = user.GetUserListByStatus("Active");
            }
            else
            {
                userlist = user.GetUserListByDepartmentAndStatus(Convert.ToInt32(dropDownListDepartment.SelectedItem.Value),"Active");
            }
            gridViewUser.DataSource = userlist;
            gridViewUser.DataBind();
        }
        #endregion

        #region COMMAND
        protected void dropDownDepartment_Changed(object sender, EventArgs e)
        {
            InitializeGridUser();
        }

        protected void gridViewUser_Command(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GetMyAccessRights();
                modalLabelError.Text = "";
                modalLabelError.Visible = false;
                int index = Convert.ToInt32(e.CommandArgument);
                int userid = Convert.ToInt32(((Label)gridViewUser.Rows[index].FindControl("labelUserId")).Text);
                User user = new User();
                user = user.GetUser(userid);
                modalBtnSubmit.Visible = Convert.ToBoolean(myAccessRights.CanUpdate);
                modalLabelUserId.Text = userid.ToString();
                modalLabelEmployeeNo.Text = user.EmployeeNumber.ToString();
                modalLabelFirstName.Text = user.Firstname.Trim();
                modalLableLastName.Text = user.Lastname;
                modalTxtBoxStartTime.Text = user.startTime;
                modalTxtBoxEndTime.Text = user.endTime;
                modalTxtBoxSalary.Text = user.currentSalary.ToString();
                modalTxtBoxBreakTimeMin.Text = user.currentMinBreak.ToString();

                InitializeOffDayDropDown(modalDropDownOffDay,user.currentOffDay.ToString());
                InitializeOffDayDropDown(modalDropDownSpecialOffDay, user.currentSpecialOffDay.ToString());
                InitializeOffDayDropDown(modalDropDownOptionalOffDay1,user.currentOptOffDay1.ToString());
                InitializeOffDayDropDown(modalDropDownOptionalOffDay2,user.currentOptOffDay2.ToString());
                InitializeOffDayDropDown(modalDropDownOptionalOffDay3,user.currentOptOffDay3.ToString());
                InitializeOffDayDropDown(modalDropDownOptionalOffDay4,user.currentOptOffDay4.ToString());
                modalChckBoxNoOTPay.Checked = user.noOTpay;
                modalChckBoxOfficeWorker.Checked = user.isOfficeWorker;
                if (user.currentRateCreateDate == DateTime.Today)
                {
                    modalTxtBoxStartDate.Text = user.currentRateStartDate.ToString("dd MMM yyyy");
                    calendarExtenderStartDate.SelectedDate = user.currentRateStartDate;
                }
                else 
                {
                    modalTxtBoxStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                    calendarExtenderStartDate.SelectedDate = DateTime.Today;
                }
                UserRateSchedule prevUserRateSchedule= new UserRateSchedule();
                prevUserRateSchedule = prevUserRateSchedule.GetPreviousUserScheduleRateByUserIdLastUpdateDate(user.Id, DateTime.Today);
                if (prevUserRateSchedule != null)
                {
                    calendarExtenderStartDate.StartDate = prevUserRateSchedule.StartDate.AddDays(1);
                }
                else 
                {
                    calendarExtenderStartDate.StartDate = null;
                }

                modalChckBoxUsePattern.Checked = user.usePattern;
                ToggleReqField(user.usePattern);
                modalTxtBoxPattern.Text = user.offPattern.Trim();
                modalTxtBoxPatternStartDate.Text = user.patternStartDate.ToString("dd MMM yyyy");
                calendarExtenderPatternStartDate.SelectedDate = user.patternStartDate;
                if (modalChckBoxUsePattern.Checked == true)
                {
                    cpeNormalUse.ClientState = "true";
                    cpePatternUse.ClientState = "false";
                }
                else
                {
                    cpePatternUse.ClientState = "true";
                    cpeNormalUse.ClientState = "false";
                }

                this.programmaticModalPopup.Show();
            }
        }
        #endregion

        #region MODAL
        #region INITIALIZE
        private void InitializeOffDayDropDown(DropDownList dropdown, string value = "")
        {
            Dictionary<string, string> days = new Dictionary<string, string>();
            days.Add("0", "Not Applicable");
            days.Add("1", "Monday");
            days.Add("2", "Tuesday");
            days.Add("3", "Wednesday");
            days.Add("4", "Thursday");
            days.Add("5", "Friday");
            days.Add("6", "Saturday");
            days.Add("7", "Sunday");
            dropdown.DataSource = days;
            dropdown.DataTextField = "Value";
            dropdown.DataValueField = "Key";
            dropdown.DataBind();
            if (value.Trim() != "")
            {
                foreach (ListItem item in dropdown.Items)
                {
                    if (item.Value.Trim() == value.Trim())
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }
        #endregion

        #region COMMAND
        protected void modalBtnSubmit_Command(object sender, CommandEventArgs e)
        {
            if (modalLabelError.Visible == true)
                this.programmaticModalPopup.Show();
            else
            {
                int userid = Convert.ToInt32(Session["UserId"]);
                User user = new User();
                if (modalLabelUserId.Text.Trim() != "") //Update
                {
                    user = user.GetUser(Convert.ToInt32(modalLabelUserId.Text));
                }

                UserRateSchedule userRateSchedule = new UserRateSchedule();
                UserRateSchedule prevUserRateSchedule = userRateSchedule.GetPreviousUserScheduleRateByUserIdLastUpdateDate(user.Id, DateTime.Today);
                userRateSchedule = userRateSchedule.GetUserScheduleRateByUserIdCreateDate(user.Id, DateTime.Today); //Checks if there is already a schedule date with the same start date
                
                if (userRateSchedule != null)
                {
                    userRateSchedule.StartTime = modalTxtBoxStartTime.Text.Trim();
                    userRateSchedule.EndTime = modalTxtBoxEndTime.Text.Trim();
                    userRateSchedule.Salary = Convert.ToDecimal(modalTxtBoxSalary.Text);
                    userRateSchedule.OffDay = Convert.ToInt32(modalDropDownOffDay.SelectedItem.Value);
                    userRateSchedule.SpecialOffDay = Convert.ToInt32(modalDropDownSpecialOffDay.SelectedItem.Value);
                    userRateSchedule.OptionalOffDay1 = Convert.ToInt32(modalDropDownOptionalOffDay1.SelectedItem.Value);
                    userRateSchedule.OptionalOffDay2 = Convert.ToInt32(modalDropDownOptionalOffDay2.SelectedItem.Value);
                    userRateSchedule.OptionalOffDay3 = Convert.ToInt32(modalDropDownOptionalOffDay3.SelectedItem.Value);
                    userRateSchedule.OptionalOffDay4 = Convert.ToInt32(modalDropDownOptionalOffDay4.SelectedItem.Value);
                    userRateSchedule.NoOTPay = modalChckBoxNoOTPay.Checked;
                    userRateSchedule.IsOfficeWorker = modalChckBoxOfficeWorker.Checked;
                    userRateSchedule.MinsBreak = Convert.ToInt32(modalTxtBoxBreakTimeMin.Text);
                    userRateSchedule.StartDate = Convert.ToDateTime(modalTxtBoxStartDate.Text.Trim());
                    userRateSchedule.LastUpdatedDate = DateTime.Today;
                    userRateSchedule.IsCurrentRate = true;
                    userRateSchedule.UsePattern = modalChckBoxUsePattern.Checked;
                    userRateSchedule.OffPattern = modalTxtBoxPattern.Text.Trim();
                    userRateSchedule.PatternStartDate = Convert.ToDateTime(modalTxtBoxPatternStartDate.Text.Trim());
                    userRateSchedule.Update(userRateSchedule);
                }
                else
                {
                    userRateSchedule = new UserRateSchedule();
                    userRateSchedule.StartTime = modalTxtBoxStartTime.Text.Trim();
                    userRateSchedule.EndTime = modalTxtBoxEndTime.Text.Trim();
                    userRateSchedule.UserId = user.Id;
                    userRateSchedule.Salary = Convert.ToDecimal(modalTxtBoxSalary.Text);
                    userRateSchedule.OffDay = Convert.ToInt32(modalDropDownOffDay.SelectedItem.Value);
                    userRateSchedule.SpecialOffDay = Convert.ToInt32(modalDropDownSpecialOffDay.SelectedItem.Value);
                    userRateSchedule.OptionalOffDay1 = Convert.ToInt32(modalDropDownOptionalOffDay1.SelectedItem.Value);
                    userRateSchedule.OptionalOffDay2 = Convert.ToInt32(modalDropDownOptionalOffDay2.SelectedItem.Value);
                    userRateSchedule.OptionalOffDay3 = Convert.ToInt32(modalDropDownOptionalOffDay3.SelectedItem.Value);
                    userRateSchedule.OptionalOffDay4 = Convert.ToInt32(modalDropDownOptionalOffDay4.SelectedItem.Value);
                    userRateSchedule.NoOTPay = modalChckBoxNoOTPay.Checked;
                    userRateSchedule.IsOfficeWorker = modalChckBoxOfficeWorker.Checked;
                    userRateSchedule.MinsBreak = Convert.ToInt32(modalTxtBoxBreakTimeMin.Text);
                    userRateSchedule.StartDate = Convert.ToDateTime(modalTxtBoxStartDate.Text.Trim());
                    userRateSchedule.IsCurrentRate = true;
                    userRateSchedule.CreatedDate = DateTime.Today;
                    userRateSchedule.LastUpdatedDate = DateTime.Today;
                    userRateSchedule.UsePattern = modalChckBoxUsePattern.Checked;
                    userRateSchedule.OffPattern = modalTxtBoxPattern.Text.Trim();
                    userRateSchedule.PatternStartDate = Convert.ToDateTime(modalTxtBoxPatternStartDate.Text.Trim());
                    userRateSchedule.Insert(userRateSchedule);
                }
                if (prevUserRateSchedule != null)
                {
                    prevUserRateSchedule.EndDate = Convert.ToDateTime(modalTxtBoxStartDate.Text.Trim()+" 23:59:59");
                    prevUserRateSchedule.IsCurrentRate = false;
                    prevUserRateSchedule.LastUpdatedDate = DateTime.Today;

                    prevUserRateSchedule.Update(prevUserRateSchedule);
                }
            }
        }

        protected void modalChckBoxUsePattern_Changed(object sender, EventArgs e) 
        {
            if (modalChckBoxUsePattern.Checked == true)
            {
                ToggleReqField(true);
                cpeNormalUse.ClientState = "true";
                cpePatternUse.ClientState = "false";
            }
            else
            {
                ToggleReqField(false);
                cpePatternUse.ClientState = "true";
                cpeNormalUse.ClientState = "false";
            }
            this.programmaticModalPopup.Show();
        }

        #endregion
        #endregion

        #region OTHERS
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            foreach (GridViewRow row in gridViewUser.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    row.Attributes["onmouseover"] = "this.style.cursor = 'pointer';this.style.backgroundColor = '#e5f2fc';";
                    row.Attributes["onmouseout"] = "this.style.backgroundColor='#ffffff';";
                    row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gridViewUser, "Select$" + row.DataItemIndex, true);
                }
            }
            base.Render(writer);
        }

        private void ToggleReqField(bool value)
        {
            modalReqPattern.Enabled = value;
            modalRegValPattern.Enabled = value;
        }

        protected bool isValidUser()
        {
            bool isvalid = false;
            if (Session["UserId"] != null)
            {
                int userid = Convert.ToInt32(Session["UserId"]);
                User user = new User();
                user = user.GetUser(userid);
                if (user != null)
                {
                    isvalid = true;
                }
            }
            return isvalid;
        }

        protected void GetMyAccessRights()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            User user = new User();
            user = user.GetUser(userid);
            Module module = new Module();
            module = module.GetModule("SetupUserRate.aspx");
            myAccessRights = myAccessRights.GetRolesModuleAccess(Convert.ToInt32(user.RoleId), module.Id);
        }

        #endregion
    }
}