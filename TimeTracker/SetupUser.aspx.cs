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
    public partial class SetupUser : System.Web.UI.Page
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
                InitializeDropDownDepartment();
                InitializeGridUser();
            }
        }

        #region INITIALIZE

        protected void InitializeGridUser()
        {
            //int userid = Convert.ToInt32(Session["UserId"]);
            GetMyAccessRights();
            User user = new User();
            if (myAccessRights.CanAdd == true)
                linkBtnAddUser.Visible = true;
            else
                linkBtnAddUser.Visible = false;

            List<User> userlist = new List<User>();
            if (dropDownListDepartment.SelectedItem.Text == "All")
            {
                if (radioBtnListStatus.SelectedItem.Text == "All")
                    userlist = user.GetUserList();
                else
                    userlist = user.GetUserListByStatus(radioBtnListStatus.SelectedItem.Value.Trim());
            }
            else 
            {
                if (radioBtnListStatus.SelectedItem.Text == "All")
                    userlist = user.GetUserList(Convert.ToInt32(dropDownListDepartment.SelectedItem.Value));
                else
                    userlist = user.GetUserListByDepartmentAndStatus(Convert.ToInt32(dropDownListDepartment.SelectedItem.Value), radioBtnListStatus.SelectedItem.Value.Trim());
            }
            gridViewUser.DataSource = userlist;
            gridViewUser.DataBind();
        }

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
            departmentList.Insert(0,department);
            dropDownListDepartment.DataSource = departmentList;
            dropDownListDepartment.DataTextField = "Description";
            dropDownListDepartment.DataValueField = "Id";
            dropDownListDepartment.DataBind();
        }

        #endregion

        #region COMMAND

        protected void linkBtnAddUser_Click(object sender, EventArgs e)
        {
            modalLabelError.Text = "";
            modalLabelError.Visible = false;
            modalLabelUserId.Text = "";
            modalBtnSubmit.CommandArgument = "Add";
            modalTxtBoxEmployeeNo.Text = "";
            modalTxtBoxFirstname.Text = "";
            modalTxtBoxLastname.Text = "";
            modalTxtBoxUsername.Text = "";
            modalTxtBoxPassword.Attributes.Add("value", "");
            modalTxtBoxPhone.Text = "";
            modalTxtBoxMobile.Text = "";
            modalTxtBoxEmail.Text = "";
            modalTxtBoxFax.Text = "";
            InitializeModalDropDownDepartment();
            InitializeModalDropDownRole();
            InitializeModalDropDownStatus();
            //InitializeModalRadBtnShift();

            modalTxtBoxStartTime.Text = "08:00";
            modalTxtBoxEndTime.Text = "05:00";
            modalTxtBoxSalary.Text = "0.00";
            modalTxtBoxBreakTimeMin.Text = "0";
            InitializeModalDropDownOffDay();
            InitializeModalDropDownSpecialOffDay();
            modalChckBoxNoOTPay.Checked = false;
            modalChkBoxUpdateRate.Checked = false;
            modalChckBoxOfficeWorker.Checked = false;
            cpeSalaryRate.ClientState = "true";
            ToggleReqField(false);

            this.programmaticModalPopup.Show();
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
                InitializeModalDropDownDepartment(user.DepartmentId.ToString());
                InitializeModalDropDownRole(user.RoleId.ToString());
                InitializeModalDropDownStatus(user.Status);
                modalBtnSubmit.Visible = Convert.ToBoolean(myAccessRights.CanUpdate);
                modalLabelUserId.Text = userid.ToString();
                modalBtnSubmit.CommandArgument = "Update";
                modalTxtBoxEmployeeNo.Text = user.EmployeeNumber.ToString();
                modalTxtBoxFirstname.Text = user.Firstname.Trim();
                modalTxtBoxLastname.Text = user.Lastname;
                modalTxtBoxUsername.Text = user.Username;
                modalTxtBoxPassword.Attributes.Add("value", user.Password);
                modalTxtBoxPhone.Text = user.Phone;
                modalTxtBoxMobile.Text = user.Mobile;
                modalTxtBoxEmail.Text = user.Email;
                modalTxtBoxFax.Text = user.Fax;
                //InitializeModalRadBtnShift(user.Shift);

                modalTxtBoxStartTime.Text = user.startTime;
                modalTxtBoxEndTime.Text = user.endTime;
                modalTxtBoxSalary.Text = user.currentSalary.ToString();
                modalTxtBoxBreakTimeMin.Text = user.currentMinBreak.ToString();

                InitializeModalDropDownOffDay(user.currentOffDay.ToString());
                InitializeModalDropDownSpecialOffDay(user.currentSpecialOffDay.ToString());
                modalChckBoxNoOTPay.Checked = user.noOTpay;
                modalChckBoxOfficeWorker.Checked =  user.isOfficeWorker;

                TimeSpan cutOfTime = user.GetMyCutOfTime();

                modalChkBoxUpdateRate.Checked = false;
                cpeSalaryRate.ClientState = "true";
                ToggleReqField(false);

                this.programmaticModalPopup.Show();
            }
        }

        protected void dropDownDepartment_Changed(object sender, EventArgs e) 
        {
            InitializeGridUser();
        }

        protected void radioBtnStatus_Changed(object sender, EventArgs e) 
        {
            InitializeGridUser();
        }

        #endregion

        #region MODAL
        #region INITIALIZE
        protected void InitializeModalDropDownDepartment(string value = "") 
        {
            Department department = new Department();
            var departmentList = department.GetDepartmentList();
            if (departmentList.Count > 0) 
            {
                if (departmentList[0].Description == "All")
                    departmentList.RemoveAt(0);
            }
            modalDropDownDepartment.DataSource = departmentList;
            modalDropDownDepartment.DataTextField = "Description";
            modalDropDownDepartment.DataValueField = "Id";
            modalDropDownDepartment.DataBind();

            if (value != null && value.Trim() != "") 
            {
                foreach (ListItem i in modalDropDownDepartment.Items) 
                {
                    if (value.Trim() == i.Value.ToString().Trim()) 
                    {
                        i.Selected = true;
                        break;
                    }
                }
            }
        }

        protected void InitializeModalDropDownRole(string value = "") 
        {
            Roles role = new Roles();
            var roles = role.GetRoleList();
            modalDropDownRole.DataSource = roles;
            modalDropDownRole.DataTextField = "Description";
            modalDropDownRole.DataValueField = "Id";
            modalDropDownRole.DataBind();

            if (value != null && value.Trim() != "")
            {
                foreach (ListItem i in modalDropDownRole.Items)
                {
                    if (value.Trim() == i.Value.ToString().Trim())
                    {
                        i.Selected = true;
                        break;
                    }
                }
            }
        }

        protected void InitializeModalDropDownStatus(string value = "") 
        {
            foreach (ListItem i in modalDropDownStatus.Items) 
            {
                if (value.Trim() == i.Value.Trim()) 
                    i.Selected = true;
                else
                    i.Selected = false;
            }
        }

        protected void InitializeModalDropDownOffDay(string value = "") 
        {
            Dictionary<string, string> days = new Dictionary<string, string>();
            days.Add("0", "No Off Day");
            days.Add("1", "Monday");
            days.Add("2", "Tuesday");
            days.Add("3", "Wednesday");
            days.Add("4", "Thursday");
            days.Add("5", "Friday");
            days.Add("6", "Saturday");
            days.Add("7", "Sunday");
            modalDropDownOffDay.DataSource = days;
            modalDropDownOffDay.DataTextField = "Value";
            modalDropDownOffDay.DataValueField = "Key";
            modalDropDownOffDay.DataBind();
            if (value.Trim() != "") 
            {
                foreach (ListItem item in modalDropDownOffDay.Items) 
                {
                    if (item.Value.Trim() == value.Trim()) 
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }

        protected void InitializeModalDropDownSpecialOffDay(string value = "")
        {
            Dictionary<string, string> days = new Dictionary<string, string>();
            days.Add("0", "No Special Off Day");
            days.Add("1", "Monday");
            days.Add("2", "Tuesday");
            days.Add("3", "Wednesday");
            days.Add("4", "Thursday");
            days.Add("5", "Friday");
            days.Add("6", "Saturday");
            days.Add("7", "Sunday");
            modalDropDownSpecialOffDay.DataSource = days;
            modalDropDownSpecialOffDay.DataTextField = "Value";
            modalDropDownSpecialOffDay.DataValueField = "Key";
            modalDropDownSpecialOffDay.DataBind();
            if (value.Trim() != "")
            {
                foreach (ListItem item in modalDropDownSpecialOffDay.Items)
                {
                    if (item.Value.Trim() == value.Trim())
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }

        //private void InitializeModalRadBtnShift(string value = "AM") 
        //{
        //    for (int i = 0; i < modalRadBtnListShift.Items.Count; i++) 
        //    {
        //        if (modalRadBtnListShift.Items[i].Value.Trim() == value)
        //            modalRadBtnListShift.Items[i].Selected = true;
        //        else
        //            modalRadBtnListShift.Items[i].Selected = false;
        //    }
        //}
        #endregion

        #region COMMAND
        protected void modalBtnSubmit_Command(object sender, CommandEventArgs e)
        {
            if (modalChkBoxUpdateRate.Checked == true) //Checks if starttime and endtime is in correct format
            { 
                TimeSpan starttime = new TimeSpan();
                TimeSpan endtime = new TimeSpan();
                if (TimeSpan.TryParse(modalTxtBoxStartTime.Text, out starttime))
                {
                    
                }
                else 
                {
                    modalLabelError.Text = "Start Time not in valid format.";
                    modalLabelError.Visible = true;
                }
                if (TimeSpan.TryParse(modalTxtBoxEndTime.Text, out endtime))
                {

                }
                else
                {
                    modalLabelError.Text = "End Time not in valid format.";
                    modalLabelError.Visible = true;
                }
            }
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
                user.Firstname = modalTxtBoxFirstname.Text.Trim();
                user.Lastname = modalTxtBoxLastname.Text.Trim();
                user.RoleId = Convert.ToInt32(modalDropDownRole.SelectedItem.Value);
                user.DepartmentId = Convert.ToInt32(modalDropDownDepartment.SelectedItem.Value);
                user.Username = modalTxtBoxUsername.Text.Trim();
                user.Password = modalTxtBoxPassword.Text.Trim();
                user.Email = modalTxtBoxEmail.Text.Trim();
                user.Phone = modalTxtBoxPhone.Text.Trim();
                user.Fax = modalTxtBoxFax.Text.Trim();
                user.Mobile = modalTxtBoxMobile.Text.Trim();
                user.LastUpdatedBy = userid;
                user.LastUpdateDate = DateTime.Now;
                user.EmployeeNumber = Convert.ToInt32(modalTxtBoxEmployeeNo.Text);
                user.Status = modalDropDownStatus.SelectedItem.Value;
                //user.Shift = modalRadBtnListShift.SelectedItem.Value.Trim();
                if (e.CommandArgument.ToString().Trim() == "Add") 
                {
                    user.CreateDate = DateTime.Now;
                    user.CreatedBy = userid;
                    user.Insert(user);
                    user = user.GetLastInsertedUser(); //assign newly created user to variable user
                }
                else if (e.CommandArgument.ToString().Trim() == "Update") 
                {
                    user.Update(user);
                }
                if (modalChkBoxUpdateRate.Checked == true) //User intends to update salary rate
                {
                    UserRateSchedule userRateSchedule = new UserRateSchedule();
                    userRateSchedule = userRateSchedule.GetUserScheduleRateByUserIdStartDate(user.Id, DateTime.Today); //Checks if there is already a schedule date with the same start date
                    if (userRateSchedule != null)
                    {
                        userRateSchedule.StartTime = modalTxtBoxStartTime.Text.Trim(); 
                        userRateSchedule.EndTime = modalTxtBoxEndTime.Text.Trim(); 
                        userRateSchedule.Salary = Convert.ToDecimal(modalTxtBoxSalary.Text);
                        userRateSchedule.OffDay = Convert.ToInt32(modalDropDownOffDay.SelectedItem.Value);
                        userRateSchedule.SpecialOffDay = Convert.ToInt32(modalDropDownSpecialOffDay.SelectedItem.Value);
                        userRateSchedule.NoOTPay = modalChckBoxNoOTPay.Checked;
                        userRateSchedule.Update(userRateSchedule);
                        userRateSchedule.IsOfficeWorker = modalChckBoxOfficeWorker.Checked;
                        userRateSchedule.MinsBreak = Convert.ToInt32(modalTxtBoxBreakTimeMin.Text);
                    }
                    else 
                    {
                        userRateSchedule = new UserRateSchedule();
                        List<UserRateSchedule> userRateScheduleList = userRateSchedule.GetCurrentUserScheduleRatesByUserId(user.Id);
                        userRateSchedule.StartTime = modalTxtBoxStartTime.Text.Trim();
                        userRateSchedule.EndTime = modalTxtBoxEndTime.Text.Trim(); 
                        userRateSchedule.UserId = user.Id;
                        userRateSchedule.Salary = Convert.ToDecimal(modalTxtBoxSalary.Text);
                        userRateSchedule.OffDay = Convert.ToInt32(modalDropDownOffDay.SelectedItem.Value);
                        userRateSchedule.SpecialOffDay = Convert.ToInt32(modalDropDownSpecialOffDay.SelectedItem.Value);
                        userRateSchedule.NoOTPay = modalChckBoxNoOTPay.Checked;
                        userRateSchedule.IsOfficeWorker = modalChckBoxOfficeWorker.Checked;
                        userRateSchedule.MinsBreak = Convert.ToInt32(modalTxtBoxBreakTimeMin.Text);
                        userRateSchedule.StartDate = DateTime.Today;
                        userRateSchedule.IsCurrentRate = true;

                        foreach (UserRateSchedule u in userRateScheduleList) 
                        {
                            if (u.EndDate == null) 
                            {

                                u.EndDate = Convert.ToDateTime(DateTime.Today.AddDays(-1).ToString("dd MMM yyyy")+" 23:59:59");
                            }
                            u.IsCurrentRate = false;
                            u.Update(u);
                        }

                        userRateSchedule.Insert(userRateSchedule);
                    }
                }
                InitializeGridUser();
                this.programmaticModalPopup.Hide();
            }
        }

        protected void modalTxtBoxUser_Changed(object sender, EventArgs e) 
        {
            if (modalTxtBoxUsername.Text != "")
            {
                if (!IsValidUserName(modalTxtBoxUsername.Text.Trim()))
                {
                    modalLabelError.Text = "Username \"" + modalTxtBoxUsername.Text.Trim() + "\" is already in use.";
                    modalLabelError.Visible = true;
                }
                else
                {
                    modalLabelError.Text = "";
                    modalLabelError.Visible = false;
                }
            }
            else 
            {
                modalLabelError.Text = "";
                modalLabelError.Visible = false;
            }
            this.programmaticModalPopup.Show();
        }

        protected void modalChkBoxUpdateRate_Changed(object sender, EventArgs e)
        {
            if (modalChkBoxUpdateRate.Checked == true)
            {
                ToggleReqField(true);
                cpeSalaryRate.ClientState = "false";
            }
            else
            {
                ToggleReqField(false);
                cpeSalaryRate.ClientState = "true";
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

        private bool IsValidUserName(string username) 
        {
            int userid = 0;
            if (modalLabelUserId.Text.Trim() != "")
                userid = Convert.ToInt32(modalLabelUserId.Text);
            bool result = true;
            User user = new User();
            
            user = user.GetUser(username);
            if (user != null)
            {
                if (userid == 0)
                {
                    result = false;
                }
                else 
                {
                    var curUser = user.GetUser(userid);
                    if (curUser.Username != user.Username)
                        result = false;
                }
            }
            return result;
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
            module = module.GetModule("SetupUser.aspx");
            myAccessRights = myAccessRights.GetRolesModuleAccess(Convert.ToInt32(user.RoleId), module.Id);
        }

        private void ToggleReqField(bool value)
        {
            modalReqStartTime.Enabled = value;
            modalRegValStartTime.Enabled = value;
            modalReqEndTime.Enabled = value;
            modalRegValEndTime.Enabled = value;
            modalReqSalary.Enabled = value;
            modalRegValSalary.Enabled = value;
            modalReqBreakTimeMin.Enabled = value;
            modalRegValBreakTimeMin.Enabled = value;
        }
        #endregion
    }
}