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
    public partial class SetupDepartmentAccess : System.Web.UI.Page
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
                InitializeGridRoles();
            }
        }

        protected void InitializeGridRoles()
        {
            //int userid = Convert.ToInt32(Session["UserId"]);
            GetMyAccessRights();
            Roles role = new Roles();
            if (role.GetRolesWithoutDepartmentAccess().Count > 0 && myAccessRights.CanAdd == true)
                linkBtnAdd.Visible = true;
            else
                linkBtnAdd.Visible = false;

            var roleList = role.GetRolesWithDepartmentAccess();

            //Converter model = new Converter();
            //DataTable table = model.ConvertToDataTable(roleList);

            gridViewRoles.DataSource = roleList;
            gridViewRoles.DataBind();
        }

        protected void InitializeModalDropDownRoles(int roleid = 0)
        {
            Roles role = new Roles();
            List<Roles> datalist = new List<Roles>();
            if (roleid == 0) //Add
            {
                datalist = role.GetRolesWithoutDepartmentAccess();
                modalDropDownRoles.Enabled = true;
            }
            else //Update
            {
                role = role.GetRole(roleid);
                datalist.Add(role);
                modalDropDownRoles.Enabled = false;
            }
            modalDropDownRoles.DataSource = datalist;
            modalDropDownRoles.DataTextField = "Description";
            modalDropDownRoles.DataValueField = "Id";
            modalDropDownRoles.DataBind();
        }

        private void InitializeModalGridViewDepartment(int roleid = 0)
        {
            Department department = new Department();
            var departmentList = department.GetDepartmentList();
            modalGridViewDepartment.DataSource = departmentList;
            modalGridViewDepartment.DataBind();

            if (roleid > 0)
            {
                InitializeModalDepartmentCheckBox(roleid);
                if (modalGridViewDepartment.Rows.Count > 1)
                    modalChkboxAll.Visible = true;
                else
                    modalChkboxAll.Visible = false;
            }
            else 
            {
                for (int i = 0; i < modalGridViewDepartment.Rows.Count; i++)
                {
                    CheckBox cb = (CheckBox)modalGridViewDepartment.Rows[i].FindControl("modalChkBoxSelect");
                    cb.Checked = false;
                }
            }
        }

        private void InitializeModalDepartmentCheckBox(int roleid)
        {
            RoleDepartmentAccess deptAccess = new RoleDepartmentAccess();
            var roledeptlist = deptAccess.GetRoleDepartmentList(roleid);
            for (int i = 0; i < modalGridViewDepartment.Rows.Count; i++)
            {
                Label labelDeptId = (Label)modalGridViewDepartment.Rows[i].FindControl("modalLabelDepartmentId");
                CheckBox cbSelect = (CheckBox)modalGridViewDepartment.Rows[i].FindControl("modalChkBoxSelect");
                for (int j = 0; j < roledeptlist.Count; j++)
                {
                    if (labelDeptId.Text.Trim() == roledeptlist[j].DepartmentId.ToString())
                    {
                        cbSelect.Checked = true;
                        labelDeptId.ToolTip = roledeptlist[j].Id.ToString();
                        roledeptlist.RemoveAt(j);
                        break;
                    }
                }
            }
        }

        protected void linkBtnAdd_Command(object sender, CommandEventArgs e)
        {
            GetMyAccessRights();
            InitializeModalGridViewDepartment();
            InitializeModalDropDownRoles();
            modalBtnSubmit.Visible = true;
            modalBtnSubmit.CommandArgument = "Add";
            modalChkboxAll.Checked = false;
            this.programmaticModalPopup.Show();
        }

        protected void gridViewRoles_Command(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GetMyAccessRights();
                int index = Convert.ToInt32(e.CommandArgument);
                int roleId = Convert.ToInt32(((Label)gridViewRoles.Rows[index].FindControl("labelRoleId")).Text);
                InitializeModalGridViewDepartment(roleId);
                InitializeModalDropDownRoles(roleId);
                modalBtnSubmit.CommandArgument = "Update";
                modalBtnSubmit.Visible = Convert.ToBoolean(myAccessRights.CanUpdate);
                modalChkboxAll.Checked = areAllDepartmentSelected();
                this.programmaticModalPopup.Show();
            }
        }

        protected void modalBtnSubmit_Command(object sender, CommandEventArgs e)
        {
            int roleId = Convert.ToInt32(modalDropDownRoles.SelectedItem.Value);
            RoleDepartmentAccess departmentAccess = new RoleDepartmentAccess();
            for (int i = 0; i < modalGridViewDepartment.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)modalGridViewDepartment.Rows[i].FindControl("modalChkBoxSelect");
                Label labelDepartmentId = (Label)modalGridViewDepartment.Rows[i].FindControl("modalLabelDepartmentId");
                departmentAccess.DepartmentId = Convert.ToInt32(labelDepartmentId.Text);
                departmentAccess.RoleId = roleId;
                if (cb.Checked != true && labelDepartmentId.ToolTip != "ToolTip") //Delete
                {
                    departmentAccess.Delete(Convert.ToInt32(labelDepartmentId.ToolTip));
                }
                else if (cb.Checked == true && labelDepartmentId.ToolTip == "ToolTip") //Add
                {
                    departmentAccess.Insert(departmentAccess);
                }
                else if (cb.Checked == true && labelDepartmentId.ToolTip != "ToolTip") //Update
                {
                    departmentAccess.Id = Convert.ToInt32(labelDepartmentId.ToolTip);
                    departmentAccess.Update(departmentAccess);
                }
            }
            InitializeGridRoles();
        }

        protected void modalChkboxAll_Changed(object sender, EventArgs e)
        {
            for (int i = 0; i < modalGridViewDepartment.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)modalGridViewDepartment.Rows[i].FindControl("modalChkBoxSelect");
                cb.Checked = modalChkboxAll.Checked;
            }
            this.programmaticModalPopup.Show();
        }

        #region OTHERS

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            foreach (GridViewRow row in gridViewRoles.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    row.Attributes["onmouseover"] = "this.style.cursor = 'pointer';this.style.backgroundColor = '#e5f2fc';";
                    row.Attributes["onmouseout"] = "this.style.backgroundColor='#ffffff';";
                    row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gridViewRoles, "Select$" + row.DataItemIndex, true);
                }
            }
            base.Render(writer);
        }

        private bool IsDeptCheckBoxEmpty()
        {
            bool isempty = true;
            for (int i = 0; i < modalGridViewDepartment.Rows.Count; i++)
            {
                CheckBox cbSelect = (CheckBox)modalGridViewDepartment.Rows[i].FindControl("modalChkBoxSelect");
                if (cbSelect.Checked == true)
                {
                    isempty = false;
                    break;
                }
            }

            return isempty;
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
            module = module.GetModule("SetupDepartmentAccess.aspx");
            myAccessRights = myAccessRights.GetRolesModuleAccess(Convert.ToInt32(user.RoleId), module.Id);
        }

        protected bool areAllDepartmentSelected() 
        {
            bool result = true;
            for (int i = 0; i < modalGridViewDepartment.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)modalGridViewDepartment.Rows[i].FindControl("modalChkBoxSelect");
                if (cb.Checked == false) 
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        #endregion
    }
}