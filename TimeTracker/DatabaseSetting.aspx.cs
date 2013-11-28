using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeTracker.Model;
using System.Data;
using System.Configuration;


namespace TimeTracker
{
    public partial class DatabaseSetting : System.Web.UI.Page
    {
        RolesModuleAccess myAccessRights = new RolesModuleAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsValidUser())
                Response.Redirect("Login.aspx");
            GetMyAccessRights();
            if (myAccessRights == null)
                Response.Redirect("Dashboard.aspx");
            HttpContext.Current.Session["siteSubHeader"] = "Settings";
            HttpContext.Current.Session["selectedTab"] = "Settings";

            if (!IsPostBack)
            {
                InitializeDatabaseSettings();
            }
        }

        private void InitializeDatabaseSettings() 
        {
            string ttConnectionString = ConfigurationManager.ConnectionStrings["TimeTrackerEntities"].ConnectionString;
            string hwConnectionString = ConfigurationManager.ConnectionStrings["CAPHWConnection"].ConnectionString;
            string swConnectionString = ConfigurationManager.ConnectionStrings["CAPSWConnection"].ConnectionString;

            string[] ttString = ttConnectionString.Split(';');
            string[] hwString = hwConnectionString.Split(';');
            string[] swString = swConnectionString.Split(';');

            string ttServer = ttString[2].Replace("provider connection string=\"data source=", "").Trim();
            string ttDatabase = ttString[3].Replace("initial catalog=", "").Trim();
            string ttUsername = ttString[4].Replace("user id=", "").Trim();
            string ttPassword = ttString[5].Replace("password=", "").Trim();

            string hwServer = hwString[0].Replace("Server=", "").Trim();
            string hwDatabase = hwString[1].Replace("Database=", "").Trim();
            string hwUsername = hwString[2].Replace("User Id=", "").Trim();
            string hwPassword = hwString[3].Replace("password = ", "").Trim();

            string swServer = swString[0].Replace("Server=", "").Trim();
            string swDatabase = swString[1].Replace("Database=", "").Trim();
            string swUsername = swString[2].Replace("User Id=", "").Trim();
            string swPassword = swString[3].Replace("password = ", "").Trim();

            ttTxtBoxServer.Text = ttServer;
            ttTxtBoxDatabase.Text = ttDatabase;
            ttTxtBoxUsername.Text = ttUsername;
            ttTxtBoxPassword.Attributes.Add("value", ttPassword);

            hwTxtBoxServer.Text = hwServer;
            hwTxtBoxDatabase.Text = hwDatabase;
            hwTxtBoxUsername.Text = hwUsername;
            hwTxtBoxPassword.Attributes.Add("value", hwPassword);

            swTxtBoxServer.Text = swServer;
            swTxtBoxDatabase.Text = swDatabase;
            swTxtBoxUsername.Text = swUsername;
            swTxtBoxPassword.Attributes.Add("value", swPassword);
            
        }

        protected void btnSubmit_Click(object sender, EventArgs e) 
        {
            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            string ttConnectionString = ConfigurationManager.ConnectionStrings["TimeTrackerEntities"].ConnectionString;
            string hwConnectionString = ConfigurationManager.ConnectionStrings["CAPHWConnection"].ConnectionString;
            string swConnectionString = ConfigurationManager.ConnectionStrings["CAPSWConnection"].ConnectionString;

            string[] ttString = ttConnectionString.Split(';');
            string[] hwString = hwConnectionString.Split(';');
            string[] swString = swConnectionString.Split(';');

            ttString[2] = "provider connection string=\"data source=" + ttTxtBoxServer.Text.Trim();
            ttString[3] = "initial catalog=" + ttTxtBoxDatabase.Text.Trim();
            ttString[4] = "user id=" + ttTxtBoxUsername.Text.Trim();
            ttString[5] = "password=" + ttTxtBoxPassword.Text.Trim();

            hwString[0] = "Server=" + hwTxtBoxServer.Text.Trim();
            hwString[1] = "Database=" + hwTxtBoxDatabase.Text.Trim();
            hwString[2] = "User Id=" + hwTxtBoxUsername.Text.Trim();
            hwString[3] = "password = " + hwTxtBoxPassword.Text.Trim();

            swString[0] = "Server=" + swTxtBoxServer.Text.Trim();
            swString[1] = "Database=" + swTxtBoxDatabase.Text.Trim();
            swString[2] = "User Id=" + swTxtBoxUsername.Text.Trim();
            swString[3] = "password = " + swTxtBoxPassword.Text.Trim();

            ttConnectionString = string.Join(";", ttString);
            hwConnectionString = string.Join(";", hwString);
            swConnectionString = string.Join(";", swString);

            config.ConnectionStrings.ConnectionStrings["TimeTrackerEntities"].ConnectionString = ttConnectionString;
            config.ConnectionStrings.ConnectionStrings["CAPHWConnection"].ConnectionString = hwConnectionString;
            config.ConnectionStrings.ConnectionStrings["CAPSWConnection"].ConnectionString = swConnectionString;

            config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("connectionStrings");
            Response.Redirect("DatabaseSetting.aspx");
            //InitializeDatabaseSettings();
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
            module = module.GetModule("DatabaseSetting.aspx");
            myAccessRights = myAccessRights.GetRolesModuleAccess(Convert.ToInt32(user.RoleId), module.Id);
        }
    }
}