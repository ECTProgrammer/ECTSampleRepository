using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeTracker.Model;

namespace TimeTracker
{
    public partial class Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JobTracker jobtracker = new JobTracker();
            if (!isValidUser() || (!jobtracker.CanConnectToCAP()))
                Response.Redirect("Login.aspx");
            if (!HasAccess())
                Response.Redirect("Dashboard.aspx");
            HttpContext.Current.Session["siteSubHeader"] = "Report";
            HttpContext.Current.Session["selectedTab"] = "Report";

            if (!IsPostBack)
            {
                InitializeRepeater();
            }
        }

        protected void InitializeRepeater()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            User user = new User();
            user = user.GetUser(userid);
            Module module = new Module();
            var list = module.GetModuleList(Convert.ToInt32(user.RoleId), "Report");
            repeaterReport.DataSource = list;
            repeaterReport.DataBind();
        }

        protected void lnkBtnReport_Command(object sender, CommandEventArgs e)
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
            var list = module.GetModuleList(Convert.ToInt32(user.RoleId), "Report");
            if (list.Count > 0)
                result = true;
            return result;
        }
    }
}