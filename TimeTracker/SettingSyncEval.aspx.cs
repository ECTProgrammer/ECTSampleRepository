using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeTracker.Model;

namespace TimeTracker
{
    public partial class SettingSyncEval : System.Web.UI.Page
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
                modalTxtBoxMessage.Attributes.Add("onchange", "DefaultValue();");
            }
        }

        protected void modalBtnSync_Command(object sender, CommandEventArgs e)
        {
            modalTxtBoxMessage.Text = "Initializing....";
            modalTxtBoxMessage.Text = modalTxtBoxMessage.Text + Environment.NewLine + "Gathering Data....";
            JobTracker jobtracker = new JobTracker();
            var data = jobtracker.GetJobTrackerListWithJobId(false);
            for (int i = 0; i < data.Count; i++) 
            {
                modalTxtBoxMessage.Text = modalTxtBoxMessage.Text + Environment.NewLine + "Synchronizing "+(i + 1)+" of "+ data.Count;
                jobtracker = jobtracker.GenerateHWAndSW(data[i].JobIdNumber.ToString().Trim());
                data[i].Customer = jobtracker.Customer;
                data[i].Description = jobtracker.Description;
                data[i].HWNo = jobtracker.HWNo;
                data[i].SWNo = jobtracker.SWNo;
                data[i].EvalNo = jobtracker.EvalNo;
                jobtracker.Update(data[i]);
            }
            modalTxtBoxMessage.Text = modalTxtBoxMessage.Text + Environment.NewLine + "Synchronization Complete.";
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
            module = module.GetModule("SettingSyncEval.aspx");
            myAccessRights = myAccessRights.GetRolesModuleAccess(Convert.ToInt32(user.RoleId), module.Id);
        }
    }
}