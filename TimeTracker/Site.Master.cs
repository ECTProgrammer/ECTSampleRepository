using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeTracker.Model;

namespace TimeTracker
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string selectedTab = "";
            if (HttpContext.Current.Session["UserId"] == null || HttpContext.Current.Session["UserId"].ToString() == "0")
            {
                labelUserName.Visible = false;
            }
            else 
            {
                User user = new User();
                user = user.GetUser(Convert.ToInt32(HttpContext.Current.Session["UserId"]));
                labelUserName.Text = "Welcome " + user.Firstname + " " + user.Lastname;
                labelUserName.Text += " | ";
                labelUserName.Visible = true;

                InitializeRepError();
            }

            if (HttpContext.Current.Session["selectedTab"] != null) 
            {
                selectedTab = HttpContext.Current.Session["selectedTab"].ToString();
            }

            if (HttpContext.Current.Session["siteSubHeader"] != null) 
            {
                subHeader.Text = HttpContext.Current.Session["siteSubHeader"].ToString();
            }

            SetSelectedNavLink(selectedTab);
        }

        protected void SetSelectedNavLink(string selectedTab) 
        {
            Panel panel = (Panel)this.FindControl(selectedTab);
            if (panel != null)
                panel.BackColor = System.Drawing.ColorTranslator.FromHtml("#0086DD");
        }

        protected void linkBtnLogoutClick(object sender, EventArgs e) 
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }

        protected void InitializeRepError() 
        {
            JobTracker jobtracker = new JobTracker();
            var data = jobtracker.GetUnclosedJobs(Convert.ToInt32(Session["UserId"]));
            List<string> openjobs = new List<string>();
            foreach (JobTracker j in data) 
            {
                openjobs.Add("Unclosed job: "+j.jobtype+", opened last "+Convert.ToDateTime(j.StartTime).ToString("dd-MMM-yyyy hh:mm"));
            }

            RepError.DataSource = openjobs;
            RepError.DataBind();
        }

    }
}