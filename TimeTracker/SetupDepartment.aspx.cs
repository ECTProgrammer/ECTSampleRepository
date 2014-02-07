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
    public partial class SetupDepartment : System.Web.UI.Page
    {
        RolesModuleAccess myAccessRights = new RolesModuleAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            JobTracker jobtracker = new JobTracker();
            if (!isValidUser() || (!jobtracker.CanConnectToCAP()))
                Response.Redirect("Login.aspx");
            GetMyAccessRights();
            if(myAccessRights == null)
                Response.Redirect("Dashboard.aspx");
            HttpContext.Current.Session["siteSubHeader"] = "Setup";
            HttpContext.Current.Session["selectedTab"] = "Setup";

            if (!IsPostBack)
            {
                InitializeGridViewDepartment();
            }
        }

        #region INITIALIZED
        protected void InitializeGridViewDepartment() 
        {
            GetMyAccessRights();
            if (myAccessRights.CanAdd == true)
                linkBtnAdd.Visible = true;
            else
                linkBtnAdd.Visible = false;

            Department department = new Department();
            List<Department> deptlist = new List<Department>();
            deptlist = department.GetDepartmentList();
            if (deptlist.Count > 0) 
            {
                if (deptlist[0].Description == "All")
                    deptlist.RemoveAt(0);
            }
            gridViewDepartment.DataSource = deptlist;
            gridViewDepartment.DataBind();
        }
        #endregion

        #region COMMAND
        protected void linkBtnAdd_Click(object sender, EventArgs e)
        {
            modalLabelDepartmentId.Text = "";
            modalLabelError.Text = "";
            modalLabelError.Visible = false;
            modalTxtBoxDescription.Text = "";
            modalTxtBoxPosition.Text = "";
            modalTxtBoxAcronym.Text = "";
            modalBtnSubmit.CommandArgument = "Add";
            this.programmaticModalPopup.Show();
        }

        protected void gridViewDepartment_Command(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GetMyAccessRights();
                modalLabelError.Text = "";
                modalLabelError.Visible = false;
                int index = Convert.ToInt32(e.CommandArgument);
                int departmentId = Convert.ToInt32(((Label)gridViewDepartment.Rows[index].FindControl("labelDepartmentId")).Text);
                modalBtnSubmit.CommandArgument = "Update";
                Department department = new Department();
                department = department.GetDepartment(departmentId);
                modalLabelDepartmentId.Text = department.Id.ToString();
                modalTxtBoxDescription.Text = department.Description;
                modalTxtBoxAcronym.Text = department.Acronym;
                modalTxtBoxPosition.Text = department.Position.ToString();
                this.programmaticModalPopup.Show();
            }
        }

        protected void modalBtnSubmit_Command(object sender, CommandEventArgs e)
        {
            if (modalLabelError.Visible == true)
                this.programmaticModalPopup.Show();
            else
            {
                int userid = Convert.ToInt32(Session["UserId"]);
                Department department = new Department();
                if (modalLabelDepartmentId.Text.Trim() != "") 
                {
                    department = department.GetDepartment(Convert.ToInt32(modalLabelDepartmentId.Text));
                }
                department.Description = modalTxtBoxDescription.Text.Trim();
                department.Acronym = modalTxtBoxAcronym.Text.Trim();
                department.Position = Convert.ToInt32(modalTxtBoxPosition.Text);
                department.LastUpdateDate = DateTime.Now;
                department.LastUpdatedBy = userid;
                if (e.CommandArgument.ToString() == "Add")
                {
                    department.CreateDate = DateTime.Now;
                    department.CreatedBy = userid;
                    department.Insert(department);
                }
                else if(e.CommandArgument.ToString() == "Update")
                {
                    department.Update(department);
                }
                InitializeGridViewDepartment();
            }
        }
        #endregion

        #region OTHERS

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            foreach (GridViewRow row in gridViewDepartment.Rows)
            {
                
                if (row.RowType == DataControlRowType.DataRow)
                {
                    row.Attributes["onmouseover"] = "this.style.cursor = 'pointer';this.style.backgroundColor = '#e5f2fc';";
                    row.Attributes["onmouseout"] = "this.style.backgroundColor='#ffffff';";
                    row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gridViewDepartment, "Select$" + row.DataItemIndex, true);
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
            module = module.GetModule("SetupDepartment.aspx");
            myAccessRights = myAccessRights.GetRolesModuleAccess(Convert.ToInt32(user.RoleId), module.Id);
        }
        #endregion
    }
}