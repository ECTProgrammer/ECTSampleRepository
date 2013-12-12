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
    public partial class SetupRole : System.Web.UI.Page
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
                InitializeGridViewRole();
            }
        }

        protected void InitializeGridViewRole()
        {
            GetMyAccessRights();
            if (myAccessRights.CanAdd == true)
                linkBtnAdd.Visible = true;
            else
                linkBtnAdd.Visible = false;

            Roles role = new Roles();
            List<Roles> rolelist = new List<Roles>();
            rolelist = role.GetRoleList();
            if (rolelist.Count > 0)
            {
                if (rolelist[0].Description == "All")
                    rolelist.RemoveAt(0);
            }
            gridViewRole.DataSource = rolelist;
            gridViewRole.DataBind();
        }

        protected void linkBtnAdd_Click(object sender, EventArgs e)
        {
            modalLabelRoleId.Text = "";
            modalLabelError.Text = "";
            modalLabelError.Visible = false;
            modalTxtBoxDescription.Text = "";
            modalBtnSubmit.CommandArgument = "Add";
            this.programmaticModalPopup.Show();
        }

        protected void gridViewRole_Command(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GetMyAccessRights();
                modalLabelError.Text = "";
                modalLabelError.Visible = false;
                modalBtnSubmit.Visible = Convert.ToBoolean(myAccessRights.CanUpdate);
                int index = Convert.ToInt32(e.CommandArgument);
                int roleid = Convert.ToInt32(((Label)gridViewRole.Rows[index].FindControl("labelRoleId")).Text);
                modalBtnSubmit.CommandArgument = "Update";
                Roles role = new Roles();
                role = role.GetRole(roleid);
                modalLabelRoleId.Text = role.Id.ToString();
                modalTxtBoxDescription.Text = role.Description;
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
                Roles role = new Roles();
                if (modalLabelRoleId.Text.Trim() != "")
                {
                    role = role.GetRole(Convert.ToInt32(modalLabelRoleId.Text));
                }
                role.Description = modalTxtBoxDescription.Text.Trim();
                role.LastUpdateDate = DateTime.Now;
                role.LastUpdatedBy = userid;
                if (e.CommandArgument.ToString() == "Add")
                {
                    role.CreateDate = DateTime.Now;
                    role.CreatedBy = userid;
                    role.Insert(role);
                }
                else if (e.CommandArgument.ToString() == "Update")
                {
                    role.Update(role);
                }
                InitializeGridViewRole();
                this.programmaticModalPopup.Hide();
            }
        }

        #region OTHERS

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            foreach (GridViewRow row in gridViewRole.Rows)
            {

                if (row.RowType == DataControlRowType.DataRow)
                {
                    row.Attributes["onmouseover"] = "this.style.cursor = 'pointer';this.style.backgroundColor = '#e5f2fc';";
                    row.Attributes["onmouseout"] = "this.style.backgroundColor='#ffffff';";
                    row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gridViewRole, "Select$" + row.DataItemIndex, true);
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
            module = module.GetModule("SetupRole.aspx");
            myAccessRights = myAccessRights.GetRolesModuleAccess(Convert.ToInt32(user.RoleId), module.Id);
        }
        #endregion
    }
}