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
    public partial class DashBoard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!isValidUser())
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
                InitializeGridViewBottom();
            }
        }

        #region PANEL LEFT

        #region INITIALIZED
        protected void InitializeGridViewLeftPanel1()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            JobTracker jobTracker = new JobTracker();
            List<JobTracker> data = new List<JobTracker>();
            data = jobTracker.GetRequestNeededApproval(userid);
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
            data = jobTracker.GetPendingRequest(userid);
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
            data = jobTracker.GetRejectedRequest(userid);
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
            var data = jobtracker.GetRequestNeededApproval(userid);
            data[index].LastUpdateDate = DateTime.Now;
            data[index].LastUpdatedBy = userid;
            if(e.CommandName == "AcceptRequest")
            {
                if (data[index].ActionRequest == "Delete")
                {
                    jobtracker.Delete(data[index].Id);
                }
                else
                {
                    data[index].Status = "Approved";
                    jobtracker.Update(data[index]);
                }
                InitializeGridViewLeftPanel1();
                InitializeGridViewLeftPanel2();
                InitializeGridViewLeftPanel3();
            }
            else if (e.CommandName == "RejectRequest") 
            {
                //data[index].Status = "Rejected";
                modalBtnConfirm.CommandArgument = data[index].Id.ToString();
                programmaticModalPopup.Show();
                //jobtracker.Update(data[index]);
                //InitializeGridViewLeft();
            }
        }

        protected void modalBtnConfirm_Command(object sender, CommandEventArgs e) 
        {
            JobTracker jobtracker = new JobTracker();
            int id = Convert.ToInt32(e.CommandArgument);
            int userid = Convert.ToInt32(Session["UserId"]);
            jobtracker = jobtracker.GetJobTracker(id);
            jobtracker.LastUpdatedBy = userid;
            jobtracker.LastUpdateDate = DateTime.Now;
            jobtracker.SupervisorRemarks = modalTxtBoxRemarks.Text;
            jobtracker.Status = "Rejected";
            jobtracker.Update(jobtracker);
            InitializeGridViewLeftPanel1();
            InitializeGridViewLeftPanel2();
            InitializeGridViewLeftPanel3();
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

        protected void InitializeBottomDropDownJobType() 
        {
            JobType jobtype = new JobType();
            var joblist = jobtype.GetJobTypeList(Convert.ToInt32(ddlBottomDepartment.SelectedValue));
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
                data = analysis.GetAnalysis(Convert.ToInt32(ddlBottomDepartment.SelectedItem.Value), Convert.ToDateTime(txtBoxBottomFromDate.Text), Convert.ToDateTime(txtBoxBottomToDate.Text),userid);
            else
                data = analysis.GetAnalysis(Convert.ToInt32(ddlBottomDepartment.SelectedItem.Value), Convert.ToDateTime(txtBoxBottomFromDate.Text), Convert.ToDateTime(txtBoxBottomToDate.Text));
            
            gridViewBottom.DataSource = data;
            gridViewBottom.DataBind();
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
        }

        protected void txtBoxBottomToDate_Changed(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(txtBoxBottomToDate.Text);
            calExtBottomToDate.SelectedDate = date;
            InitializeGridViewBottom();
        }

        protected void ddlBottomDepartment_Changed(object sender, EventArgs e) 
        {
            InitializeGridViewBottom();
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
            int userid = Convert.ToInt32(Session["UserId"]);
            User user = new User();
            user = user.GetUser(userid);
            Roles role = new Roles();
            role = role.GetRole(Convert.ToInt32(user.RoleId));

            if (role.IsSupervisor == true)
                return true;
            else
                return false;
        }
    }
}