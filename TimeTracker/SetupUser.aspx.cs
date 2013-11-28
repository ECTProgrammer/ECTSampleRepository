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
                if (e.CommandArgument.ToString().Trim() == "Add") 
                {
                    user.CreateDate = DateTime.Now;
                    user.CreatedBy = userid;
                    user.Insert(user);
                }
                else if (e.CommandArgument.ToString().Trim() == "Update") 
                {
                    user.Update(user);
                }
                InitializeGridUser();
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
        #endregion
    }
}