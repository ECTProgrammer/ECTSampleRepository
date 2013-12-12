using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TimeTracker.Model;

namespace TimeTracker
{
    public partial class SetupSupervisorMapping : System.Web.UI.Page
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
                InitializeMainGrid();
            }
        }

        private void InitializeMainGrid() 
        {
            GetMyAccessRights();

            User user = new User();
            if (myAccessRights.CanAdd == true)
            {
                if (dropDownListDepartment.SelectedItem.Text == "All" && user.GetActiveUsersWithoutSupervisor().Count > 0)
                {
                    linkBtnAdd.Visible = true;
                }
                else if (user.GetActiveUsersWithoutSupervisor(Convert.ToInt32(dropDownListDepartment.SelectedItem.Value)).Count > 0)
                {
                    linkBtnAdd.Visible = true;
                }
                else
                    linkBtnAdd.Visible = false;
            }
            else
                linkBtnAdd.Visible = false;

            List<User> userlist = new List<User>();
            if (dropDownListDepartment.SelectedItem.Text == "All")
            {
                userlist = user.GetActiveUsersWithSupervisor();
            }
            else
            {
                userlist = user.GetActiveUsersWithSupervisor(Convert.ToInt32(dropDownListDepartment.SelectedItem.Value));
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
            departmentList.Insert(0, department);
            dropDownListDepartment.DataSource = departmentList;
            dropDownListDepartment.DataTextField = "Description";
            dropDownListDepartment.DataValueField = "Id";
            dropDownListDepartment.DataBind();
        }

        protected void dropDownDepartment_Changed(Object sender, EventArgs e) 
        {
            InitializeMainGrid();
        }

        protected void linkBtnAdd_Click(object sender, EventArgs e) 
        {
            modalLabelError.Text = "";
            modalLabelError.Visible = false;
            InitializeModalDropDownUsers();
            InitializeModalDropDownDepartment();
            InitializeModalGridView();
            InitializeModalDropDownSupervisor();
            modalDropDownUsers.Enabled = true;
            modalBtnAdd.Visible = true;
            modalBtnSubmit.Visible = true;
            programmaticModalPopup.Show();
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
                InitializeModalDropDownUsers(userid);
                InitializeModalDropDownDepartment();
                InitializeModalGridView();
                InitializeModalDropDownSupervisor();
                modalDropDownUsers.Enabled = false;
                modalBtnSubmit.Visible = Convert.ToBoolean(myAccessRights.CanUpdate);
                modalBtnAdd.Visible = Convert.ToBoolean(myAccessRights.CanUpdate);
                programmaticModalPopup.Show();
            }
        }

        #region MODAL

        #region INITIALIZED
        protected void InitializeModalDropDownUsers(int userid = 0) 
        {
            User user = new User();
            List<User> userlist = new List<User>();
            if (userid == 0)
            {
                if (dropDownListDepartment.SelectedItem.Text == "All")
                {
                    userlist = user.GetActiveUsersWithoutSupervisor();
                }
                else 
                {
                    userlist = user.GetActiveUsersWithoutSupervisor(Convert.ToInt32(dropDownListDepartment.SelectedItem.Value));
                }
            }
            else 
            {
                user = user.GetUser(userid);
                userlist.Add(user);
            }
            modalDropDownUsers.DataSource = userlist;
            modalDropDownUsers.DataTextField = "fullname";
            modalDropDownUsers.DataValueField = "Id";
            modalDropDownUsers.DataBind();
        }

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

        protected void InitializeModalDropDownSupervisor() 
        {
            int userid = Convert.ToInt32(modalDropDownUsers.SelectedItem.Value);
            int deptid = Convert.ToInt32(modalDropDownDepartment.SelectedItem.Value);
            User user = new User();
            var data = user.GetAvailableSupervisors(userid, deptid);

            modalDropDownSupervisor.DataSource = data;
            for (int i = 0; i < gridViewModal.Rows.Count; i++)
            {
                Label labelSupId = (Label)gridViewModal.Rows[i].FindControl("modalLabelSupId");
                int lid = Convert.ToInt32(labelSupId.Text);
                for (int j = 0; j < data.Count; j++) 
                {
                    if (data[j].Id == lid) 
                    {
                        data.RemoveAt(j);
                        break;
                    }
                }
            }
            modalDropDownSupervisor.DataTextField = "fullname";
            modalDropDownSupervisor.DataValueField = "Id";
            modalDropDownSupervisor.DataBind();
            if (modalDropDownSupervisor.Items.Count < 1)
                modalBtnAdd.Visible = false;
            else
                modalBtnAdd.Visible = true;
        }

        protected void InitializeModalGridView() 
        {
            int userid = Convert.ToInt32(modalDropDownUsers.SelectedItem.Value);
            SupervisorMapping supmap = new SupervisorMapping();
            List<SupervisorMapping> supervisors = supmap.GetActiveSupervisors(userid);
            gridViewModal.DataSource = supervisors;
            gridViewModal.DataBind();

            for (int i = 0; i < gridViewModal.Rows.Count; i++) 
            {
                CheckBox cb = (CheckBox)gridViewModal.Rows[i].FindControl("modalChkSupervisor");
                Label labelSupId = (Label)gridViewModal.Rows[i].FindControl("modalLabelSupId");
                //cb.Checked = true;
                labelSupId.ToolTip = supervisors[i].Id.ToString();
            }
            Session["SupMap"] = supervisors;
        }
        #endregion

        #region COMMAND
        protected void modalDropDownDepartment_Changed(object sender, EventArgs e) 
        {
            InitializeModalDropDownSupervisor();
            programmaticModalPopup.Show();
        }

        protected void modalBtnAdd_Click(object sender, EventArgs e) 
        {
            List<SupervisorMapping> supervisors = new List<SupervisorMapping>();
            if (Session["SupMap"] != null) 
            {
                supervisors = (List<SupervisorMapping>)Session["SupMap"];
            }
            SupervisorMapping supmap = new SupervisorMapping();
            supmap.SupervisorId = Convert.ToInt32(modalDropDownSupervisor.SelectedItem.Value);
            supmap.supervisorname = modalDropDownSupervisor.SelectedItem.Text;
            supmap.supdepartment = modalDropDownDepartment.SelectedItem.Text;
            supmap.UserId = Convert.ToInt32(modalDropDownUsers.SelectedItem.Value);
            supervisors.Add(supmap);
            gridViewModal.DataSource = supervisors;
            gridViewModal.DataBind();
            InitializeModalDropDownSupervisor();
            Session["SupMap"] = supervisors;
            programmaticModalPopup.Show();
        }

        protected void modalBtnSubmit_Command(object sender, CommandEventArgs e) 
        {
            int userid = Convert.ToInt32(modalDropDownUsers.SelectedItem.Value);
            for (int i = 0; i < gridViewModal.Rows.Count; i++) 
            {
                SupervisorMapping supmap = new SupervisorMapping();
                CheckBox cb = (CheckBox)gridViewModal.Rows[i].FindControl("modalChkSupervisor");
                Label labelSupId = (Label)gridViewModal.Rows[i].FindControl("modalLabelSupId");
                int supid = Convert.ToInt32(labelSupId.Text);
                supmap = supmap.GetSupervisorMapping(userid, supid);
                if (cb.Checked)
                {
                    if (supmap == null)
                    {
                        supmap = new SupervisorMapping();
                        supmap.UserId = userid;
                        supmap.SupervisorId = supid;
                        supmap.Insert(supmap);
                    }
                }
                else 
                {
                    if (supmap != null) 
                    {
                        supmap.Delete(supmap.Id);
                    }
                }
            }
            Session["SupMap"] = null;
            this.programmaticModalPopup.Hide();
            InitializeMainGrid();
        }

        protected void modalBtnCancel_Click(object sender, EventArgs e) 
        {
            Session["SupMap"] = null;
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