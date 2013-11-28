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
    public partial class JobOverview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!isValidUser())
                Response.Redirect("Login.aspx");
            HttpContext.Current.Session["siteSubHeader"] = "JobOverview";
            HttpContext.Current.Session["selectedTab"] = "JobOverview";

            if (!IsPostBack)
            {
                txtBoxStartDate.Attributes.Add("readonly", "readonly");
                txtBoxEndDate.Attributes.Add("readonly", "readonly");
                InitializeStartDate();
                InitializeEndDate();
                GenerateDynamicGrid();
            }
        }

        private void GenerateDynamicGrid() 
        {
            DataTable table = new DataTable();

            table = GenerateColumns();
            GenerateRows(ref table);
            gridViewMain.DataSource = table;
            gridViewMain.DataBind();
        }

        private DataTable GenerateColumns() 
        {
            DataTable table = new DataTable();

            Department dept = new Department();
            JobType jobtype = new JobType();
            var departments = dept.GetJobOverviewDepartment();
            DataColumn hwCol = new DataColumn("HW No", typeof(System.String));
            DataColumn swCol = new DataColumn("SW No", typeof(System.String));
            DataColumn cusCol = new DataColumn("Customer", typeof(System.String));
            DataColumn jdCol = new DataColumn("Description", typeof(System.String));
            table.Columns.Add(hwCol);
            table.Columns.Add(swCol);
            table.Columns.Add(cusCol);
            table.Columns.Add(jdCol);
            foreach (Department d in departments) //Creates the columns 
            {
                var jobtypes = jobtype.GetJobOverviewJobType(d.Id);
                for (int i = 0; i < jobtypes.Count; i++)
                {
                    DataColumn col = new DataColumn(jobtypes[i].Acronym, typeof(System.String));
                    table.Columns.Add(col);
                }
            }

            return table;
        }

        private void GenerateRows(ref DataTable table) 
        {
            Department dept = new Department();
            JobType jobtype = new JobType();
            JobTracker jobtracker = new JobTracker();
            var departments = dept.GetJobOverviewDepartment();
            var distinctProjectList = jobtracker.GetDistinctProjectList(Convert.ToDateTime(txtBoxStartDate.Text+" 00:00:00"),Convert.ToDateTime(txtBoxEndDate.Text+" 23:59:59"));
         
            foreach (JobTracker p in distinctProjectList)
            {
                DataRow row = table.NewRow();
                row["HW No"] = p.HWNo == null ? "" : p.HWNo.Trim();
                row["SW No"] = p.SWNo == null ? "" : p.SWNo.Trim();
                row["Customer"] = p.customer == null ? "" : p.customer.Trim();
                row["Description"] = p.pcbdesc == null ? "" : p.pcbdesc.Trim();
                foreach (Department d in departments)
                {
                    var jobtypes = jobtype.GetJobOverviewJobType(d.Id);
                    for (int i = 0; i < jobtypes.Count; i++)
                    {
                        JobTracker j = jobtracker.GetJobTrackerJobOverview(jobtypes[i].Id, p.SWNo, p.HWNo, Convert.ToDateTime(txtBoxStartDate.Text + " 00:00:00"), Convert.ToDateTime(txtBoxEndDate.Text + " 23:59:59"));
                        if (j == null)
                        {
                            row[jobtypes[i].Acronym] = "";
                        }
                        else
                        {
                            row[jobtypes[i].Acronym] = j.JobStatus + " " + Convert.ToDateTime(j.EndTime).ToString("dd-MMM-yyyy");
                        }
                    }
                }
                table.Rows.Add(row);
            }
        }

        private void InitializeStartDate(string value = "") 
        {
            if (value == "")
            {
                calendarExtenderStartDate.SelectedDate = DateTime.Today;
                txtBoxStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            }
            else
            {
                calendarExtenderStartDate.SelectedDate = Convert.ToDateTime(txtBoxStartDate.Text);
                if (Convert.ToDateTime(calendarExtenderStartDate.SelectedDate).CompareTo(Convert.ToDateTime(value)) > 0)
                    calendarExtenderStartDate.SelectedDate = Convert.ToDateTime(value);
            }
            calendarExtenderStartDate.EndDate = DateTime.Now;
        }

        private void InitializeEndDate(String value = "") 
        {
            calendarExtenderEndDate.EndDate = DateTime.Now;
            if (value == "")
            {
                calendarExtenderEndDate.StartDate = DateTime.Now;
                calendarExtenderEndDate.SelectedDate = DateTime.Today;
                txtBoxEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            }
            else
            {
                calendarExtenderEndDate.SelectedDate = Convert.ToDateTime(txtBoxEndDate.Text);
                if (Convert.ToDateTime(calendarExtenderEndDate.SelectedDate).CompareTo(Convert.ToDateTime(value)) < 0)
                {
                    calendarExtenderEndDate.SelectedDate = Convert.ToDateTime(value);
                }
                calendarExtenderEndDate.StartDate = Convert.ToDateTime(calendarExtenderStartDate.SelectedDate);
            }  
        }

        protected void txtBoxStartDate_Changed(object sender, EventArgs e) 
        {
            calendarExtenderStartDate.SelectedDate = Convert.ToDateTime(txtBoxStartDate.Text);
            InitializeEndDate(txtBoxStartDate.Text);
            GenerateDynamicGrid();
        }
        protected void txtBoxEndDate_Changed(object sender, EventArgs e) 
        {
            calendarExtenderEndDate.SelectedDate = Convert.ToDateTime(txtBoxEndDate.Text);
            InitializeStartDate(txtBoxEndDate.Text);
            GenerateDynamicGrid();
        }

        #region OTHERS
        protected void gridViewMain_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvr = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                TableHeaderCell thc = new TableHeaderCell();
                thc.ColumnSpan = 4;
                thc.Text = "General Info";
                gvr.Cells.Add(thc);

                Department dept = new Department(); 
                JobType jobtype = new JobType();
                var departments = dept.GetJobOverviewDepartment();
                foreach (Department d in departments)
                {
                    var jobtypes = jobtype.GetJobOverviewJobType(d.Id);
                    if (jobtypes.Count > 0)
                    {
                        thc = new TableHeaderCell();
                        thc.ColumnSpan = jobtypes.Count;
                        thc.Text = d.Description;
                        gvr.Cells.Add(thc);
                    }
                }

                gridViewMain.Controls[0].Controls.AddAt(0, gvr);
            }
        }

        //protected override void Render(System.Web.UI.HtmlTextWriter writer)
        //{
        //    foreach (GridViewRow row in gridViewMain.Rows)
        //    {
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            row.Attributes["onmouseover"] = "this.style.cursor = 'pointer';this.style.backgroundColor = '#e5f2fc';";
        //            row.Attributes["onmouseout"] = "this.style.backgroundColor='#ffffff';";
        //            row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gridViewMain, "Select$" + row.DataItemIndex, true);
        //        }
        //    }
        //    base.Render(writer);
        //}

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
        #endregion
    }
}