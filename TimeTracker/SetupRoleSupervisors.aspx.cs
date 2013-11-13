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
    public partial class SetupRoleSupervisors : System.Web.UI.Page
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
                InitializeGridRoles();
            }
        }

        protected void InitializeGridRoles()
        {
            //int userid = Convert.ToInt32(Session["UserId"]);
            GetMyAccessRights();
            Roles role = new Roles();
            if (role.GetRolesWithoutSupervisor().Count > 0 && myAccessRights.CanAdd == true)
                linkBtnAdd.Visible = true;
            else
                linkBtnAdd.Visible = false;

            var roleList = role.GetRolesWithSupervisors();

            //Converter model = new Converter();
            //DataTable table = model.ConvertToDataTable(roleList);

            gridViewRoles.DataSource = roleList;
            gridViewRoles.DataBind();
        }

        protected void InitializeModuleGrid(int roleId = 0)
        {

            Roles role = new Roles();
            var datalist = role.GetRoleList();

            gridViewModal.DataSource = datalist;
            gridViewModal.DataBind();
            if (roleId > 0)
                InitializeCheckBox(roleId);
            if (gridViewModal.Rows.Count > 1)
                modalChkboxAll.Visible = true;
            else
                modalChkboxAll.Visible = false;
        }

        protected void InitializeCheckBox(int roleId)
        {
            RolesSupervisor roleSupervisor = new RolesSupervisor();
            modalChkboxAll.Checked = false;
            for (int i = 0; i < gridViewModal.Rows.Count; i++)
            {
                CheckBox cbSupervisor = (CheckBox)gridViewModal.Rows[i].FindControl("chkboxSupervisor");
                Label modalLabelRoleId = (Label)gridViewModal.Rows[i].FindControl("modalLabelRoleId");
                var data = roleSupervisor.GetRoleSupervisor(roleId, Convert.ToInt32(modalLabelRoleId.Text));
                if (data != null)
                {
                    cbSupervisor.Checked = true;
                    modalLabelRoleId.ToolTip = data.Id.ToString();
                }
            }
        }

        protected void InitializeModalDropDownRoles(int roleid = 0)
        {
            Roles role = new Roles();
            List<Roles> datalist = new List<Roles>();
            if (roleid == 0) //Add
            {
                datalist = role.GetRolesWithoutSupervisor();
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


        protected void linkBtnAdd_Command(object sender, CommandEventArgs e)
        {
            GetMyAccessRights();
            InitializeModuleGrid();
            InitializeModalDropDownRoles();
            modalBtnSubmit.Visible = true;
            modalBtnSubmit.CommandArgument = "Add";
            SetCheckBoxStatus(true);
            this.programmaticModalPopup.Show();
        }

        protected void SetCheckBoxStatus(bool value)
        {
            for (int i = 0; i < gridViewModal.Rows.Count; i++)
            {
                CheckBox cbSupervisor = (CheckBox)gridViewModal.Rows[i].FindControl("chkboxSupervisor");
                cbSupervisor.Enabled = value;
            }
            modalChkboxAll.Enabled = value;
        }

        protected void gridViewRoles_Command(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GetMyAccessRights();
                int index = Convert.ToInt32(e.CommandArgument);
                int roleId = Convert.ToInt32(((Label)gridViewRoles.Rows[index].FindControl("labelRoleId")).Text);
                InitializeModuleGrid(roleId);
                InitializeModalDropDownRoles(roleId);
                modalBtnSubmit.CommandArgument = "Update";
                modalBtnSubmit.Visible = Convert.ToBoolean(myAccessRights.CanUpdate);
                SetCheckBoxStatus(Convert.ToBoolean(myAccessRights.CanUpdate));
                this.programmaticModalPopup.Show();
            }
        }

        protected void modalBtnSubmit_Command(object sender, CommandEventArgs e)
        {
            int roleId = Convert.ToInt32(modalDropDownRoles.SelectedItem.Value);
            RolesSupervisor roleSupervisor = new RolesSupervisor();
            for (int i = 0; i < gridViewModal.Rows.Count; i++)
            {
                CheckBox cbSupervisor = (CheckBox)gridViewModal.Rows[i].FindControl("chkboxSupervisor");
                Label modalLabelRoleId = (Label)gridViewModal.Rows[i].FindControl("modalLabelRoleId");

                roleSupervisor.SupervisorRoleId = Convert.ToInt32(modalLabelRoleId.Text);
                roleSupervisor.RoleId = roleId;

                if (cbSupervisor.Checked == false && modalLabelRoleId.ToolTip != "ToolTip") //Delete
                {
                    roleSupervisor.Delete(Convert.ToInt32(modalLabelRoleId.ToolTip));
                }
                else if (cbSupervisor.Checked == true && modalLabelRoleId.ToolTip == "ToolTip") //Add
                {
                    roleSupervisor.Insert(roleSupervisor);
                }
                else if (cbSupervisor.Checked == true && modalLabelRoleId.ToolTip != "ToolTip") //Update
                {
                    roleSupervisor.Id = Convert.ToInt32(modalLabelRoleId.ToolTip);
                    roleSupervisor.Update(roleSupervisor);
                }
            }
            InitializeGridRoles();
        }

        protected void modalChkboxAll_Changed(object sender, EventArgs e)
        {
            for (int i = 0; i < gridViewModal.Rows.Count; i++)
            {
                CheckBox cbSupervisor = (CheckBox)gridViewModal.Rows[i].FindControl("chkboxSupervisor");
                cbSupervisor.Checked = modalChkboxAll.Checked;
            }
            this.programmaticModalPopup.Show();
        }


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
            module = module.GetModule("SetupRoleSupervisors.aspx");
            myAccessRights = myAccessRights.GetRolesModuleAccess(Convert.ToInt32(user.RoleId), module.Id);
        }
    }
}