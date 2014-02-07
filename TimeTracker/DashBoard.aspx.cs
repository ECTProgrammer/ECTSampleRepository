using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeTracker.Model;
using System.Data;
using System.Text.RegularExpressions;
namespace TimeTracker
{
    public partial class DashBoard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JobTracker jobtracker = new JobTracker();
            if (!isValidUser() || (!jobtracker.CanConnectToCAP()))
                Response.Redirect("Login.aspx");
            HttpContext.Current.Session["siteSubHeader"] = "Dashboard";
            HttpContext.Current.Session["selectedTab"] = "Dashboard";

            if (!IsPostBack)
            {
                txtBoxBottomFromDate.Attributes.Add("readonly", "readonly");
                txtBoxBottomToDate.Attributes.Add("readonly", "readonly");
                calExtBottomFromDate.SelectedDate = DateTime.Now;
                calExtBottomFromDate.EndDate = DateTime.Now;
                txtBoxBottomFromDate.Text = DateTime.Now.ToString("dd MMM yyyy");
                calExtBottomToDate.SelectedDate = DateTime.Now;
                calExtBottomToDate.StartDate = DateTime.Now;
                calExtBottomToDate.EndDate = DateTime.Now;
                txtBoxBottomToDate.Text = DateTime.Now.ToString("dd MMM yyyy");

                if (!isSupervisor())
                    tabPanelLeft1.Visible = false;
                InitializeGridViewLeftPanel1();
                InitializeGridViewLeftPanel2();
                InitializeGridViewLeftPanel3();
                InitializeBottomDropDownDepartment();
                InitializeBottomDropDownPersonel();
                InitializeGridViewBottom();
                InitializeGridViewBottom2();
            }
        }

        #region PANEL LEFT

        #region INITIALIZED
        protected void InitializeGridViewLeftPanel1()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            JobTracker jobTracker = new JobTracker();
            List<JobTracker> data = new List<JobTracker>();
            data = jobTracker.GetRequestNeededApproval(userid,false);
            Converter model = new Converter();

            DataTable table = model.ConvertToDataTable(data);

            gridViewLeftPanel1.DataSource = table;
            gridViewLeftPanel1.DataBind();
        }

        protected void InitializeGridViewLeftPanel2()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            JobTracker jobTracker = new JobTracker();
            List<JobTracker> data = new List<JobTracker>();
            data = jobTracker.GetPendingRequest(userid,false);
            Converter model = new Converter();

            DataTable table = model.ConvertToDataTable(data);

            gridViewLeftPanel2.DataSource = table;
            gridViewLeftPanel2.DataBind();
        }

        protected void InitializeGridViewLeftPanel3()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            JobTracker jobTracker = new JobTracker();
            List<JobTracker> data = new List<JobTracker>();
            data = jobTracker.GetRejectedRequest(userid,false);
            Converter model = new Converter();

            DataTable table = model.ConvertToDataTable(data);

            gridViewLeftPanel3.DataSource = table;
            gridViewLeftPanel3.DataBind();
        }
        #endregion

        #region COMMAND
        protected void gridViewLeftPanel1_RowCommand(object sender, GridViewCommandEventArgs e) 
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int index = Convert.ToInt32(e.CommandArgument);
            JobTracker jobtracker = new JobTracker();
            JobTrackerHistory jtHist = new JobTrackerHistory();
            //var data = jobtracker.GetRequestNeededApproval(userid);
            Label jobtrackId = (Label)gridViewLeftPanel1.Rows[index].FindControl("gridLeftlblJobTrackId");
            jobtracker = jobtracker.GetJobTracker(Convert.ToInt32(jobtrackId.Text),false);
            //data[index].LastUpdateDate = DateTime.Now;
            //data[index].LastUpdatedBy = userid;
            jobtracker.ApprovedBy = userid;
            jobtracker.LastUpdateDate = DateTime.Now;
            jobtracker.LastUpdatedBy = userid;
            if(e.CommandName == "AcceptRequest")
            {
                if (jobtracker.ActionRequest == "Delete")
                {
                    jobtracker.Status = "Approved";
                    jtHist = jtHist.ConvertToHistory(jobtracker);
                    jobtracker.Delete(jobtracker.Id);
                }
                else
                {
                    jobtracker.Status = "Approved";
                    jobtracker.Update(jobtracker);
                    jtHist = jtHist.ConvertToHistory(jobtracker);
                }
                jtHist.Insert(jtHist);
                InitializeGridViewLeftPanel1();
                InitializeGridViewLeftPanel2();
                InitializeGridViewLeftPanel3();
            }
            else if (e.CommandName == "RejectRequest") 
            {
                //data[index].Status = "Rejected";
                modalBottomLabelError.Visible = false;
                modalBottomLabelError.Text = "";
                modalBtnConfirm.CommandArgument = jobtracker.Id.ToString();
                modalTxtBoxRemarks.Text = "";
                programmaticModalPopup.Show();
                //jobtracker.Update(data[index]);
                //InitializeGridViewLeft();
            }
        }

        protected void modalBtnConfirm_Command(object sender, CommandEventArgs e) 
        {
            string[] remarks = modalTxtBoxRemarks.Text.Replace("\n"," ").Trim().Split(' ');
            int counter = 0;
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            for (int i = 0; i < remarks.Length; i++) 
            {  
                if (rgx.Replace(remarks[i],"").Trim() != "")
                {
                    counter++;
                }
            }
            if (counter > 2)
            {
                JobTracker jobtracker = new JobTracker();
                JobTrackerHistory jtHist = new JobTrackerHistory();
                int id = Convert.ToInt32(e.CommandArgument);
                int userid = Convert.ToInt32(Session["UserId"]);
                jobtracker = jobtracker.GetJobTracker(id,false);
                jobtracker.ApprovedBy = userid;
                jobtracker.LastUpdatedBy = userid;
                jobtracker.LastUpdateDate = DateTime.Now;
                jobtracker.SupervisorRemarks = modalTxtBoxRemarks.Text;
                jobtracker.Status = "Rejected";
                jobtracker.Update(jobtracker);

                jtHist = jtHist.ConvertToHistory(jobtracker);
                jtHist.Insert(jtHist);

                InitializeGridViewLeftPanel1();
                InitializeGridViewLeftPanel2();
                InitializeGridViewLeftPanel3();
            }
            else 
            {
                modalBottomLabelError.Text = "Minimum Three(3) Words";
                modalBottomLabelError.Visible = true;
                programmaticModalPopup.Show();
            }
        }
        #endregion

        #endregion

        #region PANEL BOTTOM
        protected void InitializeBottomDropDownDepartment() 
        {
            ddlBottomDepartment.Enabled = true;
            int roleid = Convert.ToInt32(Session["RoleId"]);
            RoleDepartmentAccess departmentAccess = new RoleDepartmentAccess();
            var departmentlist = departmentAccess.GetRoleDepartmentList(roleid);
            if (departmentlist.Count > 1) 
            {
                departmentAccess.DepartmentId = 0;
                departmentAccess.department = "All";
                departmentlist.Insert(0, departmentAccess);
            }
            else if (departmentlist.Count < 1) 
            {
                int userid = Convert.ToInt32(Session["UserId"]);
                Department department = new Department();
                User user = new User();
                user = user.GetUser(userid);
                department = department.GetDepartment(Convert.ToInt32(user.DepartmentId));
                departmentAccess.DepartmentId = department.Id;
                departmentAccess.department = department.Description;
                departmentlist.Insert(0, departmentAccess);
                ddlBottomDepartment.Enabled = false;
            }
            ddlBottomDepartment.DataSource = departmentlist;
            ddlBottomDepartment.DataTextField = "department";
            ddlBottomDepartment.DataValueField = "DepartmentId";
            ddlBottomDepartment.DataBind();

        }
        
        protected void InitializeBottomDropDownPersonel()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int roleid = Convert.ToInt32(Session["RoleId"]);
            RoleDepartmentAccess departmentAccess = new RoleDepartmentAccess();
            var departmentlist = departmentAccess.GetRoleDepartmentList(roleid);
            User user = new User();
            List<User> userlist = new List<User>();
            user = user.GetUser(userid);
            if (departmentlist.Count < 1)
            {
                userlist.Add(user);
                ddlBottomPersonel.Enabled = false;
            }
            else 
            {
                int departmentid = Convert.ToInt32(ddlBottomDepartment.SelectedItem.Value);
                if (departmentid == 0)
                {
                    foreach (RoleDepartmentAccess r in departmentlist) 
                    {
                        var ulist = user.GetUserList(r.DepartmentId);
                        userlist.AddRange(ulist);
                    }
                    userlist = userlist.Distinct().ToList();
                }
                else
                {
                    userlist = user.GetUserList(departmentid);
                }
                if (userlist.Count > 1) 
                {
                    User alluser = new User();
                    alluser.fullname = "All";
                    alluser.Id = 0;
                    userlist.Insert(0, alluser);
                }
                ddlBottomPersonel.Enabled = true;
            }

            ddlBottomPersonel.DataSource = userlist;
            ddlBottomPersonel.DataTextField = "fullname";
            ddlBottomPersonel.DataValueField = "Id";
            ddlBottomPersonel.DataBind();
        }

        protected void InitializeGridViewBottom() 
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            bool isSingle = false;
            int roleid = Convert.ToInt32(Session["RoleId"]);
            RoleDepartmentAccess departmentAccess = new RoleDepartmentAccess();
            var departmentlist = departmentAccess.GetRoleDepartmentList(roleid);
            if (departmentlist.Count < 1) 
            {
                isSingle = true;
            }
            Analysis analysis = new Analysis();
            List<Analysis> data = new List<Analysis>();
            if (isSingle)
                data = analysis.GetAnalysis(Convert.ToDateTime(txtBoxBottomFromDate.Text), Convert.ToDateTime(txtBoxBottomToDate.Text), userid, txtBoxBottomJobId.Text.Trim(), txtBoxBottomCustomer.Text.Trim());
            else
            {
                if (ddlBottomPersonel.Items.Count > 0)
                {
                    if (ddlBottomPersonel.SelectedItem.Text.Trim() == "All")
                    {
                        data = analysis.GetAnalysis(Convert.ToInt32(ddlBottomDepartment.SelectedItem.Value), Convert.ToDateTime(txtBoxBottomFromDate.Text), Convert.ToDateTime(txtBoxBottomToDate.Text), txtBoxBottomJobId.Text.Trim(),txtBoxBottomCustomer.Text.Trim(), roleid);
                    }
                    else
                    {
                        data = analysis.GetAnalysis(Convert.ToDateTime(txtBoxBottomFromDate.Text), Convert.ToDateTime(txtBoxBottomToDate.Text), Convert.ToInt32(ddlBottomPersonel.SelectedItem.Value), txtBoxBottomJobId.Text.Trim(), txtBoxBottomCustomer.Text.Trim());
                    }

                }
            }
            
            gridViewBottom.DataSource = data;
            gridViewBottom.DataBind();
        }

        protected void InitializeGridViewBottom2() 
        {       
            JobTracker jobTracker = new JobTracker();
            List<JobTracker> data = new List<JobTracker>();
            gridViewBottom2.EmptyDataText = "No data found";

            if (ddlBottomPersonel.Items.Count > 0)
            {
                //if (ddlBottomPersonel.SelectedItem.Text.Trim() != "All")
                //{
                //    gridViewBottom2.EmptyDataText = "No Data Found";
                //    int personelid = Convert.ToInt32(ddlBottomPersonel.SelectedItem.Value);
                //    DateTime startdate = Convert.ToDateTime(txtBoxBottomFromDate.Text.Trim() + " 00:00:00");
                //    DateTime enddate = Convert.ToDateTime(txtBoxBottomToDate.Text.Trim() + " 23:59:59");
                //    data = jobTracker.GetJobTrackerListExcludeRejected(personelid, startdate, enddate, false);
                //}
                //else 
                //{
                DateTime startdate = Convert.ToDateTime(txtBoxBottomFromDate.Text.Trim() + " 00:00:00");
                DateTime enddate = Convert.ToDateTime(txtBoxBottomToDate.Text.Trim() + " 23:59:59");
                int deptid = Convert.ToInt32(ddlBottomDepartment.SelectedValue);
                string jobid = txtBoxBottomJobId.Text.Trim();
                string customer = txtBoxBottomCustomer.Text.Trim();
                int userid = Convert.ToInt32(ddlBottomPersonel.SelectedItem.Value);
                data = jobTracker.GetJobTrackerListExcludeRejected(startdate, enddate, userid, deptid, jobid, customer,true);
                //}
            }
            
            gridViewBottom2.DataSource = data;
            gridViewBottom2.DataBind();
        }

        protected void gridViewBottom_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton approvedwork = (LinkButton)e.Row.FindControl("linkBtnBottomApprovedWork");
                if (approvedwork.Text.Trim() == "0 min") 
                {
                    approvedwork.Enabled = false;
                    approvedwork.Font.Underline = false;
                }
                LinkButton waitingwork = (LinkButton)e.Row.FindControl("linkBtnBottomWaitingWork");
                if (waitingwork.Text.Trim() == "0 min")
                {
                    waitingwork.Enabled = false;
                    waitingwork.Font.Underline = false;
                }
                LinkButton rejectedwork = (LinkButton)e.Row.FindControl("linkBtnBottomRejectedWork");
                if (rejectedwork.Text.Trim() == "0 min" || rejectedwork.Text.Trim() == "")
                {
                    rejectedwork.Enabled = false;
                    rejectedwork.Font.Underline = false;
                }
            }
        }

        protected void txtBoxBottomFromDate_Changed(object sender, EventArgs e)
        {
            DateTime sdate = Convert.ToDateTime(txtBoxBottomFromDate.Text);
            calExtBottomFromDate.SelectedDate = sdate;
            DateTime edate = Convert.ToDateTime(txtBoxBottomToDate.Text);
            if (sdate > edate) 
            {
                txtBoxBottomToDate.Text = txtBoxBottomFromDate.Text;
                calExtBottomToDate.SelectedDate = sdate;
            }
            calExtBottomToDate.StartDate = sdate;
            InitializeGridViewBottom();
            InitializeGridViewBottom2();
        }

        protected void txtBoxBottomToDate_Changed(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(txtBoxBottomToDate.Text);
            calExtBottomToDate.SelectedDate = date;
            InitializeGridViewBottom();
            InitializeGridViewBottom2();
        }

        protected void txtBoxBottomJobId_Changed(object sender, EventArgs e) 
        {
            InitializeGridViewBottom();
            InitializeGridViewBottom2();
        }

        protected void txtBoxBottomCustomer_Changed(object sender, EventArgs e) 
        {
            InitializeGridViewBottom();
            InitializeGridViewBottom2();
        }

        protected void ddlBottomDepartment_Changed(object sender, EventArgs e) 
        {
            InitializeBottomDropDownPersonel();
            InitializeGridViewBottom();
            InitializeGridViewBottom2();
        }

        protected void ddlBottomPersonel_Changed(object sender, EventArgs e) 
        {
            InitializeGridViewBottom();
            InitializeGridViewBottom2();
        }

        protected void linkButtonBottom_Command(object sender, CommandEventArgs e) 
        {
            if (ddlBottomPersonel.Items.Count > 0)
            {
                JobTracker jobTracker = new JobTracker();
                string jobtypeid = e.CommandArgument.ToString();
                List<JobTracker> data = new List<JobTracker>();
                string[] argument = jobtypeid.Split('|');
                DateTime startdate = Convert.ToDateTime(txtBoxBottomFromDate.Text.Trim() + " 00:00:00");
                DateTime enddate = Convert.ToDateTime(txtBoxBottomToDate.Text.Trim() + " 23:59:59");
                if (argument.Count() > 3) 
                {
                    startdate = Convert.ToDateTime("1900-01-01");
                    enddate = DateTime.Now;
                }
                LinkButton lb = (LinkButton)sender;
                
                int deptid = Convert.ToInt32(ddlBottomDepartment.SelectedValue);
                string jobid = txtBoxBottomJobId.Text.Trim();
                string customer = txtBoxBottomCustomer.Text.Trim();
                int userid = Convert.ToInt32(ddlBottomPersonel.SelectedItem.Value);
                data = jobTracker.GetJobTrackerList(startdate, enddate,Convert.ToInt32(argument[0]),argument[2].Trim(), userid, deptid, jobid, customer,true);
                labelmodalBottom.Text = argument[1] + " - " + lb.Text+" - "+argument[2];
                gridViewModalBottom.DataSource = data;
                gridViewModalBottom.DataBind();
                programmaticModalPopupBottom.Show();
            }
        }

        #endregion

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

        protected bool isSupervisor() 
        {
            SupervisorMapping supmap = new SupervisorMapping();

            if(supmap.GetActiveSubordinates(Convert.ToInt32(Session["UserId"])).Count > 0)
                return true;
            else
                return false;
        }
    }
}