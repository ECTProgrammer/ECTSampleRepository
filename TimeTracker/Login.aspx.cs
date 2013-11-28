using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeTracker.Model;

namespace TimeTracker
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //btnLogin.Attributes.Add("onclick", "document.body.style.cursor = 'wait';"); //change mouse pointer to hourglass on button click

        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            
            User user = new User();
            user = user.GetActiveUser(txtBoxUsername.Text.Trim(), txtBoxPassword.Text.Trim());
            if (user != null)
            {
                Session["UserId"] = user.Id;
                Session["RoleId"] = user.RoleId;
                Session["DepartmentId"] = user.DepartmentId;
                labelError.Visible = false;
                //Server.Transfer("Default.aspx", false);
                Response.Redirect("Dashboard.aspx", true);
            }
            else 
            {
                labelError.Text = "Login Failed. Please check your username or/and password.";
                labelError.Visible = true;
            }
        }
    }
}