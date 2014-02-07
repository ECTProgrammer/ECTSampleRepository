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
    public partial class SetupModuleAccess : System.Web.UI.Page
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
                InitializeGridRoles();
            }
        }

        #region INITIALIZE
        protected void InitializeGridRoles() 
        {
            //int userid = Convert.ToInt32(Session["UserId"]);
            GetMyAccessRights();
            Roles role = new Roles();
            if (role.GetRolesWithoutModuleAccess().Count > 0 && myAccessRights.CanAdd == true)
                linkBtnAdd.Visible = true;
            else
                linkBtnAdd.Visible = false;

            var roleList = role.GetRolesWithModuleAccess();

            //Converter model = new Converter();
            //DataTable table = model.ConvertToDataTable(roleList);

            gridViewRoles.DataSource = roleList;
            gridViewRoles.DataBind();
        }

        protected void InitializeModuleGrid(int roleId = 0) 
        {
            
            Module module = new Module();
            var datalist = module.GetModuleList();

            gridViewModuleAccess.DataSource = datalist;
            gridViewModuleAccess.DataBind();
            if(roleId > 0)
                InitializeCheckBox(roleId);
            if (gridViewModuleAccess.Rows.Count > 1)
                modalChkboxAll.Visible = true;
            else
                modalChkboxAll.Visible = false;
        }

        protected void InitializeCheckBox(int roleId) 
        {
            RolesModuleAccess moduleAccess = new RolesModuleAccess();
            modalChkboxAll.Checked = false;
            for (int i = 0; i < gridViewModuleAccess.Rows.Count; i++) 
            {
                CheckBox cbView = (CheckBox)gridViewModuleAccess.Rows[i].FindControl("chkBoxView");
                CheckBox cbAdd = (CheckBox)gridViewModuleAccess.Rows[i].FindControl("chkBoxAdd");
                CheckBox cbUpdate = (CheckBox)gridViewModuleAccess.Rows[i].FindControl("chkBoxUpdate");
                CheckBox cbDelete = (CheckBox)gridViewModuleAccess.Rows[i].FindControl("chkBoxDelete");
                Label labelModuleId = (Label)gridViewModuleAccess.Rows[i].FindControl("modalLabelModuleId");
                var data = moduleAccess.GetRolesModuleAccess(roleId, Convert.ToInt32(labelModuleId.Text));
                if (data != null) 
                {
                    if (data.CanAdd == true)
                        cbAdd.Checked = true;
                    else
                        cbAdd.Checked = false;
                    if (data.CanDelete == true)
                        cbDelete.Checked = true;
                    else
                        cbDelete.Checked = false;
                    if (data.CanUpdate == true)
                        cbUpdate.Checked = true;
                    else
                        cbUpdate.Checked = false;
                    if (data.CanView == true)
                        cbView.Checked = true;
                    else
                        cbView.Checked = false;
                    labelModuleId.ToolTip = data.Id.ToString();
                }
            }
        }

        protected void InitializeModalDropDownRoles(int roleid = 0) 
        {
            Roles role = new Roles();
            List<Roles> datalist = new List<Roles>();
            if (roleid == 0) //Add
            {
                datalist = role.GetRolesWithoutModuleAccess();
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

        protected void SetCheckBoxStatus(bool value)
        {
            for (int i = 0; i < gridViewModuleAccess.Rows.Count; i++)
            {
                CheckBox cbAdd = (CheckBox)gridViewModuleAccess.Rows[i].FindControl("chkBoxAdd");
                CheckBox cbView = (CheckBox)gridViewModuleAccess.Rows[i].FindControl("chkBoxView");
                CheckBox cbUpdate = (CheckBox)gridViewModuleAccess.Rows[i].FindControl("chkBoxUpdate");
                CheckBox cbDelete = (CheckBox)gridViewModuleAccess.Rows[i].FindControl("chkBoxDelete");

                cbAdd.Enabled = value;
                cbView.Enabled = value;
                cbUpdate.Enabled = value;
                cbDelete.Enabled = value;
            }
            modalChkboxAll.Enabled = value;
        }
        #endregion

        #region COMMAND
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
            RolesModuleAccess moduleAccess = new RolesModuleAccess();
            for (int i = 0; i < gridViewModuleAccess.Rows.Count; i++) 
            {
                CheckBox cbAdd = (CheckBox)gridViewModuleAccess.Rows[i].FindControl("chkBoxAdd");
                CheckBox cbView = (CheckBox)gridViewModuleAccess.Rows[i].FindControl("chkBoxView");
                CheckBox cbUpdate = (CheckBox)gridViewModuleAccess.Rows[i].FindControl("chkBoxUpdate");
                CheckBox cbDelete = (CheckBox)gridViewModuleAccess.Rows[i].FindControl("chkBoxDelete");
                Label labelModuleId = (Label)gridViewModuleAccess.Rows[i].FindControl("modalLabelModuleId");

                moduleAccess.CanAdd = cbAdd.Checked;
                moduleAccess.CanDelete = cbDelete.Checked;
                moduleAccess.CanUpdate = cbUpdate.Checked;
                moduleAccess.CanView = cbView.Checked;
                moduleAccess.ModuleId = Convert.ToInt32(labelModuleId.Text);
                moduleAccess.RoleId = roleId;

                if (IsCheckBoxEmpty(gridViewModuleAccess.Rows[i]) && labelModuleId.ToolTip != "ToolTip") //Delete
                {
                    moduleAccess.Delete(Convert.ToInt32(labelModuleId.ToolTip));
                }
                else if ((!IsCheckBoxEmpty(gridViewModuleAccess.Rows[i])) && labelModuleId.ToolTip == "ToolTip") //Add
                {
                    
                    moduleAccess.Insert(moduleAccess);
                }
                else if ((!IsCheckBoxEmpty(gridViewModuleAccess.Rows[i])) && labelModuleId.ToolTip != "ToolTip") //Update
                {
                    moduleAccess.Id = Convert.ToInt32(labelModuleId.ToolTip);
                    moduleAccess.Update(moduleAccess);
                }
            }
            this.programmaticModalPopup.Hide();
            InitializeGridRoles();
        }

        protected void modalChkboxAll_Changed(object sender, EventArgs e) 
        {
            for (int i = 0; i < gridViewModuleAccess.Rows.Count; i++)
            {
                CheckBox cbAdd = (CheckBox)gridViewModuleAccess.Rows[i].FindControl("chkBoxAdd");
                CheckBox cbView = (CheckBox)gridViewModuleAccess.Rows[i].FindControl("chkBoxView");
                CheckBox cbUpdate = (CheckBox)gridViewModuleAccess.Rows[i].FindControl("chkBoxUpdate");
                CheckBox cbDelete = (CheckBox)gridViewModuleAccess.Rows[i].FindControl("chkBoxDelete");

                cbAdd.Checked = modalChkboxAll.Checked;
                cbView.Checked = modalChkboxAll.Checked;
                cbUpdate.Checked = modalChkboxAll.Checked;
                cbDelete.Checked = modalChkboxAll.Checked;
            }
            this.programmaticModalPopup.Show();
        }

        #endregion

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
     
        protected bool IsCheckBoxEmpty(GridViewRow row) 
        {
            bool isempty = true;
            CheckBox cbView = (CheckBox)row.FindControl("chkBoxView");
            CheckBox cbAdd = (CheckBox)row.FindControl("chkBoxAdd");
            CheckBox cbUpdate = (CheckBox)row.FindControl("chkBoxUpdate");
            CheckBox cbDelete = (CheckBox)row.FindControl("chkBoxDelete");
            if (cbAdd.Checked || cbView.Checked || cbUpdate.Checked || cbDelete.Checked)
                isempty = false;
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
            module = module.GetModule("SetupModuleAccess.aspx");
            myAccessRights = myAccessRights.GetRolesModuleAccess(Convert.ToInt32(user.RoleId), module.Id);
        }

        #endregion
    }
}