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
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!isValidUser())
                Response.Redirect("Login.aspx");
            //if (!IsPostBack)
            //{
            //    InitializeStartDate();
            //    InitializeEndDate();
            //}
            //GenerateDynamicGrid();
            
        }

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
                //GenerateDynamicGrid();
            }
            GenerateDynamicGrid();
        }

        #region JOB OVERVIEW SUMMARY

        private DataTable GenerateSummaryColumns()
        {
            DataTable table = new DataTable();

            Department dept = new Department();
            JobTypeDepartment jobtypeDepartment = new JobTypeDepartment();
            var departments = dept.GetJobOverviewDepartment();
            DataColumn hwCol = new DataColumn("HW No", typeof(System.String));
            DataColumn swCol = new DataColumn("SW No", typeof(System.String));
            DataColumn cusCol = new DataColumn("Customer", typeof(System.String));
            DataColumn jdCol = new DataColumn("Description", typeof(System.String));
            table.Columns.Add(hwCol);
            table.Columns.Add(swCol);
            table.Columns.Add(cusCol);
            table.Columns.Add(jdCol);

            BoundField bfHw = new BoundField();
            bfHw.HeaderText = "HW No";
            bfHw.DataField = "HW No";

            BoundField bfSw = new BoundField();
            bfSw.HeaderText = "SW No";
            bfSw.DataField = "SW No";

            BoundField bfCus = new BoundField();
            bfCus.HeaderText = "Customer";
            bfCus.DataField = "Customer";

            BoundField bfDes = new BoundField();
            bfDes.HeaderText = "Description";
            bfDes.DataField = "Description";

            gridViewSummary.Columns.Add(bfHw);
            gridViewSummary.Columns.Add(bfSw);
            gridViewSummary.Columns.Add(bfCus);
            gridViewSummary.Columns.Add(bfDes);

            foreach (Department d in departments) //Creates the columns 
            {
                DataColumn col = new DataColumn(d.Acronym + "" + d.Id, typeof(System.String));
                table.Columns.Add(col);

                TemplateField tfield = new TemplateField();
                tfield.HeaderText = d.Description;
                gridViewSummary.Columns.Add(tfield);
            }

            return table;
        }

        private void GenerateSummaryRows(ref DataTable table)
        {
            Department dept = new Department();
            JobTypeDepartment jobtypeDepartment = new JobTypeDepartment();
            JobTracker jobtracker = new JobTracker();
            var departments = dept.GetJobOverviewDepartment();
            var distinctProjectList = jobtracker.GetDistinctProjectList(Convert.ToDateTime(txtBoxStartDate.Text + " 00:00:00"), Convert.ToDateTime(txtBoxEndDate.Text + " 23:59:59"));
            foreach (JobTracker p in distinctProjectList)
            {
                DataRow row = table.NewRow();
                row["HW No"] = p.HWNo == null ? "" : p.HWNo.Trim();
                row["SW No"] = p.SWNo == null ? "" : p.SWNo.Trim();
                row["Customer"] = p.customer == null ? "" : p.customer.Trim();
                row["Description"] = p.pcbdesc == null ? "" : p.pcbdesc.Trim();
                foreach (Department d in departments)
                {
                    JobTracker j = jobtracker.GetJobTrackerJobOverview(p.SWNo, p.HWNo, Convert.ToDateTime(txtBoxStartDate.Text + " 00:00:00"), Convert.ToDateTime(txtBoxEndDate.Text + " 23:59:59"), d.Id);
                    if (j == null)
                    {
                        row[d.Acronym + "" + d.Id] = "";
                    }
                    else
                    {
                        row[d.Acronym + "" + d.Id] = d.Id + "|" + j.JobStatus + " " + Convert.ToDateTime(j.EndTime).ToString("dd-MMM-yyyy")+"|"+p.HWNo+"|"+p.SWNo;
                    }
                }
                table.Rows.Add(row);
            }
        }

        protected void gridViewSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < (e.Row.DataItem as DataRowView).Row.ItemArray.Length; i++)
                {
                    string s = (e.Row.DataItem as DataRowView).Row[i].ToString();
                    if (s.IndexOf("Completed") > -1)
                    {
                        string[] d = s.Split('|');
                        LinkButton lnkBtn = new LinkButton();
                        lnkBtn.ID = "modalLinkSummaryStatus" + d[0];
                        lnkBtn.Text = d[1];
                        //lnkBtn.Click += new EventHandler(LinkButtonAction);
                        lnkBtn.CommandName = "JobOverviewSummary";
                        lnkBtn.Command += lnkBtn_Command;
                        lnkBtn.CommandArgument = d[0]+"|"+d[2]+"|"+d[3];
                        e.Row.Cells[i].Controls.Add(lnkBtn);
                        lnkBtn.Command += lnkBtn_Command;
                        e.Row.Cells[i].BackColor = System.Drawing.Color.LightGreen;
                    }
                    else if (s.IndexOf("In Progress") > -1)
                    {
                        string[] d = s.Split('|');
                        LinkButton lnkBtn = new LinkButton();
                        lnkBtn.ID = "modalLinkSummaryStatus" + d[0];
                        lnkBtn.Text = d[1];
                        lnkBtn.Command += lnkBtn_Command;
                        lnkBtn.CommandName = "JobOverviewSummary";
                        lnkBtn.CommandArgument = d[0] + "|" + d[2] + "|" + d[3];
                        e.Row.Cells[i].Controls.Add(lnkBtn);
                        e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                    }
                    else if (s.IndexOf("On Hold") > -1)
                    {
                        string[] d = s.Split('|');
                        LinkButton lnkBtn = new LinkButton();
                        lnkBtn.ID = "modalLinkSummaryStatus" + d[0];
                        lnkBtn.Text = d[1];
                        lnkBtn.Command += lnkBtn_Command;
                        lnkBtn.CommandName = "JobOverviewSummary";
                        lnkBtn.CommandArgument = d[0] + "|" + d[2] + "|" + d[3];
                        e.Row.Cells[i].Controls.Add(lnkBtn);
                        e.Row.Cells[i].BackColor = System.Drawing.ColorTranslator.FromHtml("#E67E9B");
                    }
                }
            }
        }
        #endregion
        #region JOB OVERVIEW DETAILS
        

        private DataTable GenerateDetailsColumns() 
        {
            DataTable table = new DataTable();

            Department dept = new Department();
            JobTypeDepartment jobtypeDepartment = new JobTypeDepartment();
            var departments = dept.GetJobOverviewDepartment();
            DataColumn hwCol = new DataColumn("HW No", typeof(System.String));
            DataColumn swCol = new DataColumn("SW No", typeof(System.String));
            DataColumn cusCol = new DataColumn("Customer", typeof(System.String));
            DataColumn jdCol = new DataColumn("Description", typeof(System.String));
            table.Columns.Add(hwCol);
            table.Columns.Add(swCol);
            table.Columns.Add(cusCol);
            table.Columns.Add(jdCol);

            BoundField bfHw = new BoundField();
            bfHw.HeaderText = "HW No";
            bfHw.DataField = "HW No";

            BoundField bfSw = new BoundField();
            bfSw.HeaderText = "SW No";
            bfSw.DataField = "SW No";
            
            BoundField bfCus = new BoundField();
            bfCus.HeaderText = "Customer";
            bfCus.DataField = "Customer";
            
            BoundField bfDes = new BoundField();
            bfDes.HeaderText = "Description";
            bfDes.DataField = "Description";

            gridViewDetail.Columns.Add(bfHw);
            gridViewDetail.Columns.Add(bfSw);
            gridViewDetail.Columns.Add(bfCus);
            gridViewDetail.Columns.Add(bfDes);

            foreach (Department d in departments) //Creates the columns 
            {
                var jobtypes = jobtypeDepartment.GetJobOverviewJobType(d.Id);
                for (int i = 0; i < jobtypes.Count; i++)
                {
                    DataColumn col = new DataColumn(jobtypes[i].Acronym+""+d.Id, typeof(System.String));
                    table.Columns.Add(col);

                    TemplateField tfield = new TemplateField();
                    tfield.HeaderText = jobtypes[i].Acronym;
                    gridViewDetail.Columns.Add(tfield);
                }
            }

            return table;
        }

        private void GenerateDetailsRows(ref DataTable table) 
        {
            Department dept = new Department();
            JobTypeDepartment jobtypeDepartment = new JobTypeDepartment();
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
                    var jobtypes = jobtypeDepartment.GetJobOverviewJobType(d.Id);
                    for (int i = 0; i < jobtypes.Count; i++)
                    {
                        JobTracker j = jobtracker.GetJobTrackerJobOverview(jobtypes[i].Id, p.SWNo, p.HWNo, Convert.ToDateTime(txtBoxStartDate.Text + " 00:00:00"), Convert.ToDateTime(txtBoxEndDate.Text + " 23:59:59"),d.Id);
                        if (j == null)
                        {
                            row[jobtypes[i].Acronym+""+d.Id] = "";
                        }
                        else
                        {
                            row[jobtypes[i].Acronym + "" + d.Id] = j.Id + "|" + j.JobStatus + " " + Convert.ToDateTime(j.EndTime).ToString("dd-MMM-yyyy");
                        }
                    }
                }
                table.Rows.Add(row);
            }
        }

        protected void gridViewDetail_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvr = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                TableHeaderCell thc = new TableHeaderCell();
                thc.ColumnSpan = 4;
                thc.Text = "General Info";
                //thc.BackColor = System.Drawing.Color.Yellow;
                gvr.Cells.Add(thc);
                System.Drawing.Color backcolor = System.Drawing.ColorTranslator.FromHtml("#164BDB");
                System.Drawing.Color forecolor = System.Drawing.Color.White;
                Department dept = new Department();
                JobTypeDepartment jobtypeDepartment = new JobTypeDepartment();
                var departments = dept.GetJobOverviewDepartment();
                int rowIndex = 4;
                for (int i = 0; i<departments.Count;i++)
                {
                    if (i % 2 != 0)
                    {
                        backcolor = System.Drawing.ColorTranslator.FromHtml("#0D72FF");
                        //forecolor = System.Drawing.ColorTranslator.FromHtml("#2e6e9e");
                        forecolor = System.Drawing.Color.White;
                    }
                    else
                    {
                        backcolor = System.Drawing.ColorTranslator.FromHtml("#164BDB");
                        forecolor = System.Drawing.Color.White;
                    }
                    var jobtypes = jobtypeDepartment.GetJobOverviewJobType(departments[i].Id);
                    if (jobtypes.Count > 0)
                    {
                        thc = new TableHeaderCell();
                        thc.BackColor = backcolor;
                        thc.ForeColor = forecolor;
                        thc.ColumnSpan = jobtypes.Count;
                        thc.Text = departments[i].Description;
                        gvr.Cells.Add(thc);
                    }
                    for (int x = 0;x<jobtypes.Count; x++) 
                    {
                        e.Row.Cells[rowIndex].BackColor = backcolor;
                        e.Row.Cells[rowIndex].ForeColor = forecolor;
                        rowIndex++;
                    }
                }
                gridViewDetail.Controls[0].Controls.AddAt(0, gvr);
            }
            
        }

        protected void gridViewDetail_RowDataBound(object sender, GridViewRowEventArgs e) 
        {
            if (e.Row.RowType == DataControlRowType.DataRow) 
            {
                for (int i = 0; i < (e.Row.DataItem as DataRowView).Row.ItemArray.Length; i++)
                {
                    string s = (e.Row.DataItem as DataRowView).Row[i].ToString();
                    if (s.IndexOf("Completed") > -1)
                    {
                        string[] d = s.Split('|');
                        LinkButton lnkBtn = new LinkButton();
                        lnkBtn.ID = "modalLinkDetailStatus"+d[0];
                        lnkBtn.Text = d[1];
                        //lnkBtn.Click += new EventHandler(LinkButtonAction);
                        lnkBtn.CommandName = "JobOverviewDetails";
                        lnkBtn.Command += lnkBtn_Command;
                        lnkBtn.CommandArgument = d[0];
                        e.Row.Cells[i].Controls.Add(lnkBtn);
                        lnkBtn.Command += lnkBtn_Command;
                        e.Row.Cells[i].BackColor = System.Drawing.Color.LightGreen;
                    }
                    else if (s.IndexOf("In Progress") > -1)
                    {
                        string[] d = s.Split('|');
                        LinkButton lnkBtn = new LinkButton();
                        lnkBtn.ID = "modalLinkDetailStatus" + d[0];
                        lnkBtn.Text = d[1];
                        lnkBtn.Command += lnkBtn_Command;
                        lnkBtn.CommandName = "JobOverviewDetails";
                        lnkBtn.CommandArgument = d[0];
                        e.Row.Cells[i].Controls.Add(lnkBtn);
                        e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                    }
                    else if (s.IndexOf("On Hold") > -1)
                    {
                        string[] d = s.Split('|');
                        LinkButton lnkBtn = new LinkButton();
                        lnkBtn.ID = "modalLinkDetailStatus" + d[0];
                        lnkBtn.Text = d[1];
                        lnkBtn.Command += lnkBtn_Command;
                        lnkBtn.CommandName = "JobOverviewDetails";
                        lnkBtn.CommandArgument = d[0];
                        e.Row.Cells[i].Controls.Add(lnkBtn);
                        e.Row.Cells[i].BackColor = System.Drawing.ColorTranslator.FromHtml("#E67E9B");
                    }
                }
            }
        }
        #endregion

        
        
        #region OTHERS
        private void GenerateDynamicGrid() 
        {
            DataTable summarytable = new DataTable();
            DataTable detailtable = new DataTable();
            
            gridViewSummary.Columns.Clear();
            gridViewDetail.Columns.Clear();

            summarytable = GenerateSummaryColumns();
            detailtable = GenerateDetailsColumns();

            GenerateSummaryRows(ref summarytable);
            GenerateDetailsRows(ref detailtable);

            gridViewSummary.DataSource = summarytable;
            gridViewDetail.DataSource = detailtable;

            gridViewSummary.DataBind();
            gridViewDetail.DataBind();
        }

        protected void lnkBtn_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "JobOverviewDetails")
            {
                JobTracker j = new JobTracker();
                j = j.GetJobTracker(Convert.ToInt32(e.CommandArgument));
                modalDetailLabelName.Text = j.fullname;
                modalDetailLabelJobType.Text = j.jobtype;
                modalDetailLabelJobStatus.Text = j.JobStatus;
                modalDetailLabelDate.Text = Convert.ToDateTime(j.ScheduleDate).ToString("dd-MMM-yyyy");
                modalDetailLabelStartTime.Text = Convert.ToDateTime(j.StartTime).ToString("hh:mm tt");
                modalDetailLabelEndTime.Text = Convert.ToDateTime(j.EndTime).ToString("hh:mm tt");
                modalDetailTxtBoxRemarks.Text = j.Remarks;
                this.programmaticModalPopupDetail.Show();
            }
            else if (e.CommandName == "JobOverviewSummary") 
            {
                JobTypeDepartment jobtypeDepartment = new JobTypeDepartment();
                JobTracker jobtracker = new JobTracker();
                string argument = e.CommandArgument.ToString();
                string[] s = argument.Split('|');
                var jobtypes = jobtypeDepartment.GetJobOverviewJobType(Convert.ToInt32(s[0]));
                Dictionary<string, string> repeaterdata = new Dictionary<string, string>();
                for (int i = 0; i < jobtypes.Count; i++)
                {
                    JobTracker j = jobtracker.GetJobTrackerJobOverview(jobtypes[i].Id, s[2], s[1], Convert.ToDateTime(txtBoxStartDate.Text + " 00:00:00"), Convert.ToDateTime(txtBoxEndDate.Text + " 23:59:59"), Convert.ToInt32(s[0]));
                    if (j == null)
                    {
                        repeaterdata.Add(jobtypes[i].Description, "");
                    }
                    else
                    {
                        repeaterdata.Add(jobtypes[i].Description, j.JobStatus + " " + Convert.ToDateTime(j.EndTime).ToString("dd-MMM-yyyy"));
                    }
                }
                modalSummaryRepeater.DataSource = repeaterdata;
                modalSummaryRepeater.DataBind();
                programmaticModalPopupSummary.Show();
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