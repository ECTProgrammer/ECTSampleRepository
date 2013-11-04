using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeTracker.Model;

namespace TimeTracker
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!isValidUser())
                Response.Redirect("Login.aspx");
            HttpContext.Current.Session["siteSubHeader"] = "Default";
            HttpContext.Current.Session["selectedTab"] = "Default";
            if (!IsPostBack) 
            {
                
            }
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
    }
}