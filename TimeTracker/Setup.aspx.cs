using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeTracker.Model;

namespace TimeTracker
{
    public partial class Setup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JobTracker jobtracker = new JobTracker();
            if (!isValidUser() || (!jobtracker.CanConnectToCAP()))
                Response.Redirect("Login.aspx");
            if (!HasAccess())
                Response.Redirect("Dashboard.aspx");
            HttpContext.Current.Session["siteSubHeader"] = "Setup";
            HttpContext.Current.Session["selectedTab"] = "Setup";

            if (!IsPostBack)
            {
                InitializeRepeater();
                this.programmaticModalPopup.Show();
            }
        }

        protected void InitializeRepeater() 
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            User user = new User();
            user = user.GetUser(userid);
            Module module = new Module();
            var list = module.GetModuleList(Convert.ToInt32(user.RoleId), "Setup");
            repeaterSetup.DataSource = list;
            repeaterSetup.DataBind();
        }

        protected void lnkBtnSetup_Command(object sender, CommandEventArgs e) 
        {
            string path = "~/";
            path += e.CommandArgument.ToString().Trim();
            Response.Redirect(path);
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

        protected bool HasAccess() 
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            bool result = false;
            User user = new User();
            user = user.GetUser(userid);
            Module module = new Module();
            var list = module.GetModuleList(Convert.ToInt32(user.RoleId), "Setup");
            if (list.Count > 0)
                result = true;
            return result;
        }
    }
}