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
    public partial class DashBoard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!isValidUser())
                Response.Redirect("Login.aspx");
            HttpContext.Current.Session["siteSubHeader"] = "Dashboard";
            HttpContext.Current.Session["selectedTab"] = "Dashboard";

            if (!IsPostBack)
            {
                InitializeGridViewLeft();
            }
        }

        #region INITIALIZED
        protected void InitializeGridViewLeft()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            JobTracker jobTracker = new JobTracker();
            List<JobTracker> data = new List<JobTracker>();
            data = jobTracker.GetRequestNeededApproval(userid);
            Converter model = new Converter();

            DataTable table = model.ConvertToDataTable(data);

            gridViewLeftPanel1.DataSource = table;
            gridViewLeftPanel1.DataBind();
        }
        #endregion

        #region COMMAND
        protected void gridViewLeftPanel1_RowCommand(object sender, GridViewCommandEventArgs e) 
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int index = Convert.ToInt32(e.CommandArgument);
            JobTracker jobtracker = new JobTracker();
            var data = jobtracker.GetRequestNeededApproval(userid);
            data[index].LastUpdateDate = DateTime.Now;
            data[index].LastUpdatedBy = userid;
            if(e.CommandName == "AcceptRequest")
            {
                if (data[index].ActionRequest == "Delete")
                {
                }
                else
                {
                    data[index].Status = "Approved";
                    jobtracker.Update(data[index]);
                }
                InitializeGridViewLeft();
            }
            else if (e.CommandName == "RejectRequest") 
            {
                data[index].Status = "Rejected";
                jobtracker.Update(data[index]);
                InitializeGridViewLeft();
            }
        }
        #endregion

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