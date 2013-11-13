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
            if (!IsValidUser())
                Response.Redirect("Login.aspx");
            GetMyAccessRights();
            if (myAccessRights == null)
                Response.Redirect("Dashboard.aspx");
            HttpContext.Current.Session["siteSubHeader"] = "Setup";
            HttpContext.Current.Session["selectedTab"] = "Setup";

            if (!IsPostBack)
            {
                InitializeGridUser();
            }
        }

        protected void InitializeGridUser()
        {
            //int userid = Convert.ToInt32(Session["UserId"]);
            GetMyAccessRights();
            Roles role = new Roles();
            User user = new User();
            if (myAccessRights.CanAdd == true)
                linkBtnAddUser.Visible = true;
            else
                linkBtnAddUser.Visible = false;

            var userlist = user.GetUserList();

            gridViewUser.DataSource = userlist;
            gridViewUser.DataBind();
        }

        protected void InitializeDropDownDepartment(string value = "") 
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

        protected void InitializeDropDownRole(string value = "") 
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
            modalTxtBoxPassword.Attributes.Add("value","");
            modalTxtBoxPhone.Text = "";
            modalTxtBoxMobile.Text = "";
            modalTxtBoxEmail.Text = "";
            modalTxtBoxFax.Text = "";
            InitializeDropDownDepartment();
            InitializeDropDownRole();
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
                InitializeDropDownDepartment(user.DepartmentId.ToString());
                InitializeDropDownRole(user.RoleId.ToString());
                modalBtnSubmit.Visible = Convert.ToBoolean(myAccessRights.CanUpdate);
                modalLabelUserId.Text =userid.ToString();
                modalBtnSubmit.CommandArgument = "Update";
                modalTxtBoxEmployeeNo.Text = user.EmployeeNumber.ToString();
                modalTxtBoxFirstname.Text = user.Firstname.Trim();
                modalTxtBoxLastname.Text = user.Lastname;
                modalTxtBoxUsername.Text = user.Username;
                modalTxtBoxPassword.Attributes.Add("value",user.Password);
                modalTxtBoxPhone.Text = user.Phone;
                modalTxtBoxMobile.Text = user.Mobile;
                modalTxtBoxEmail.Text = user.Email;
                modalTxtBoxFax.Text = user.Fax;
                this.programmaticModalPopup.Show();
            }
        }

        protected void modalBtnSubmit_Command(object sender, CommandEventArgs e)
        {
            if (modalLabelError.Visible == true)
                this.programmaticModalPopup.Show();
            else 
            {
               
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

        protected bool IsValidUser()
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
    }
}