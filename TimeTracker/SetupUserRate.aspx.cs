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
                InitializeDropDownDepartment();
                InitializeGridUser();
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