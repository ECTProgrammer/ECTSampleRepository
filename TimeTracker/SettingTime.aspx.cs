using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeTracker.Model;
namespace TimeTracker
{
    public partial class SettingTime : System.Web.UI.Page
    {
        RolesModuleAccess myAccessRights = new RolesModuleAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsValidUser())
                Response.Redirect("Login.aspx");
            GetMyAccessRights();
            if (myAccessRights == null)
                Response.Redirect("Dashboard.aspx");
            HttpContext.Current.Session["siteSubHeader"] = "Setting";
            HttpContext.Current.Session["selectedTab"] = "Setting";

            if (!IsPostBack)
            {
                InitializeGridViewTime();
            }
        }
        protected void InitializeGridViewTime()
        {
            TimeSetting timesetting = new TimeSetting();
            List<TimeSetting> times= new List<TimeSetting>();
            times = timesetting.GetTimeSettingList();
            gridViewTime.DataSource = times;
            gridViewTime.DataBind();
        }

        protected void gridViewTime_Command(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GetMyAccessRights();
                modalLabelError.Text = "";
                modalLabelError.Visible = false;
                modalBtnSubmit.Visible = Convert.ToBoolean(myAccessRights.CanUpdate);
                modalBtnSubmit.CommandArgument = "Update";
                TimeSetting timesetting = new TimeSetting();
                timesetting = timesetting.GetTimeSetting();
                modalTxtBoxInterval.Text = timesetting.Interval.ToString();
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
                TimeSetting timesetting = new TimeSetting();
                timesetting.Interval = Convert.ToInt32(modalTxtBoxInterval.Text);
                timesetting.LastUpdateDate = DateTime.Now;
                timesetting.LastUpdatedBy = userid;
                timesetting.Update(timesetting);
                InitializeGridViewTime();
                this.programmaticModalPopup.Hide();
            }
        }

        #region OTHERS

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            foreach (GridViewRow row in gridViewTime.Rows)
            {

                if (row.RowType == DataControlRowType.DataRow)
                {
                    row.Attributes["onmouseover"] = "this.style.cursor = 'pointer';this.style.backgroundColor = '#e5f2fc';";
                    row.Attributes["onmouseout"] = "this.style.backgroundColor='#ffffff';";
                    row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gridViewTime, "Select$" + row.DataItemIndex, true);
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
            module = module.GetModule("SettingTime.aspx");
            myAccessRights = myAccessRights.GetRolesModuleAccess(Convert.ToInt32(user.RoleId), module.Id);
        }
        #endregion
    }
}