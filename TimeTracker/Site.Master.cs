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
                linkBtnUserName.Visible = false;
            }
            else 
            {
                User user = new User();
                user = user.GetUser(Convert.ToInt32(HttpContext.Current.Session["UserId"]));
                linkBtnUserName.Text = "Welcome " + user.Firstname + " " + user.Lastname;
                linkBtnUserName.Text += " | ";
                linkBtnUserName.Visible = true;
                InitializeRepError();
            }
            Setup.Visible = hasAccess("Setup");
            Settings.Visible = hasAccess("Settings");
            Report.Visible = hasAccess("Report");

            //if (modalUserlinkChkBoxUpdate.Checked == false)
            //{
            //    ToggleReqField(false);
            //    cpeUserlink.ClientState = "true"; //Collapse 
            //}
            //else 
            //{
            //    ToggleReqField(true);
            //    cpeUserlink.ClientState = "false"; //Expand
            //}

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

        protected void linkBtnUserlink_Click(object sender, EventArgs e) 
        {
            modalUserlinkChkBoxUpdate.Checked = false;
            cpeUserlink.ClientState = "true"; //Collapse the panel
            ToggleReqField(false);

            modalUserlinkLabelError.Text = "";
            modalUserlinkLabelError.Visible = false;
            int userid = Convert.ToInt32(Session["UserId"]);
            User user = new User();
            user = user.GetUser(userid);
            modalUserlinkTxtBoxFirstname.Text = user.Firstname.Trim();
            modalUserlinkTxtBoxLastname.Text = user.Lastname.Trim();
            modalUserlinkTxtBoxUsername.Text = user.Username.Trim();
            modalUserlinkTxtBoxEmail.Text = user.Email.Trim();
            modalUserlinkTxtBoxPhone.Text = user.Phone.Trim();
            modalUserlinkTxtBoxMobile.Text = user.Mobile.Trim();
            modalUserlinkTxtBoxFax.Text = user.Fax.Trim();
            this.programmaticUserlinkModalPopup.Show();
        }

        protected void modalUserlinkTxtBoxUser_Changed(object sender, EventArgs e) 
        {
            if (modalUserlinkTxtBoxUsername.Text != "")
            {
                if (!IsValidUserName(modalUserlinkTxtBoxUsername.Text.Trim()))
                {
                    modalUserlinkLabelError.Text = "Username \"" + modalUserlinkTxtBoxUsername.Text.Trim() + "\" is already in use.";
                    modalUserlinkLabelError.Visible = true;
                }
                else
                {
                    modalUserlinkLabelError.Text = "";
                    modalUserlinkLabelError.Visible = false;
                }
            }
            else
            {
                modalUserlinkLabelError.Text = "";
                modalUserlinkLabelError.Visible = false;
            }
            this.programmaticUserlinkModalPopup.Show();
        }

        protected void modalUserlinkChkBoxUpdate_Changed(object sender, EventArgs e) 
        {
            if (modalUserlinkChkBoxUpdate.Checked == true)
            {
                ToggleReqField(true);
                cpeUserlink.ClientState = "false";
            }
            else 
            {
                ToggleReqField(false);
                cpeUserlink.ClientState = "true";
            }
            this.programmaticUserlinkModalPopup.Show();
        }

        protected void modalUserlinkBtnSubmit_Command(object sender, CommandEventArgs e) 
        {
            bool haserror = false;
            int userid = Convert.ToInt32(Session["UserId"]);
            User user = new User();
            user = user.GetUser(userid);

            if (modalUserlinkChkBoxUpdate.Checked == true)
            {
                if (modalUserlinkTxtBoxOldPassword.Text.Trim() != user.Password.Trim())
                {
                    modalUserlinkLabelError.Text = "Invalid Old Password.";
                    modalUserlinkLabelError.Visible = true;
                    haserror = true;
                }
                else if (modalUserlinkTxtBoxPassword.Text.Trim() != modalUserlinkTxtBoxRetypePassword.Text.Trim()) 
                {
                    modalUserlinkLabelError.Text = "New password and retype-password does not match.";
                    modalUserlinkLabelError.Visible = true;
                    haserror = true;
                }
                if (haserror == false)
                {
                    user.Username = modalUserlinkTxtBoxUsername.Text.Trim();
                    user.Password = modalUserlinkTxtBoxPassword.Text.Trim();
                }
            }
            if (haserror == false)
            {
                user.Firstname = modalUserlinkTxtBoxFirstname.Text.Trim();
                user.Lastname = modalUserlinkTxtBoxLastname.Text.Trim();
                user.Email = modalUserlinkTxtBoxEmail.Text.Trim();
                user.Phone = modalUserlinkTxtBoxPhone.Text.Trim();
                user.Fax = modalUserlinkTxtBoxFax.Text.Trim();
                user.Mobile = modalUserlinkTxtBoxMobile.Text.Trim();
                user.LastUpdatedBy = userid;
                user.LastUpdateDate = DateTime.Now;
                user.Update(user);
            }
            else 
            {
                this.programmaticUserlinkModalPopup.Show();
            }
        }

        private void ToggleReqField(bool value) 
        {
            modalUserlinkReqUsername.Enabled = value;
            modalUserlinkRegValUsername.Enabled = value;
            modalUserlinkReqOldPassword.Enabled = value;
            modalUserlinkRegValOldPassword.Enabled = value;
            modalUserlinkReqPassword.Enabled = value;
            modalUserlinkRegValPassword.Enabled = value;
            modalUserlinkReqRetypePassword.Enabled = value;
            modalUserlinkRegValRetypePassword.Enabled = value;
        }

        private bool IsValidUserName(string username)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            bool result = true;
            User user = new User();

            user = user.GetUser(username);
            if (user != null)
            {
                var curUser = user.GetUser(userid);
                if (curUser.Username != user.Username)
                    result = false;
            }
            return result;
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

        protected bool hasAccess(string modulename) 
        {
            bool result = false;
            User user = new User();
            user = user.GetUser(Convert.ToInt32(HttpContext.Current.Session["UserId"]));   
            Module module = new Module();
            var list = module.GetModuleList(Convert.ToInt32(user.RoleId),modulename);
            RolesModuleAccess moduleAccess = new RolesModuleAccess();
            if (list.Count > 0)
                result = true;
            return result;
        }

    }
}