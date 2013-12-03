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
            gridViewMain.Columns.Clear();
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

            gridViewMain.Columns.Add(bfHw);
            gridViewMain.Columns.Add(bfSw);
            gridViewMain.Columns.Add(bfCus);
            gridViewMain.Columns.Add(bfDes);

            foreach (Department d in departments) //Creates the columns 
            {
                var jobtypes = jobtype.GetJobOverviewJobType(d.Id);
                for (int i = 0; i < jobtypes.Count; i++)
                {
                    DataColumn col = new DataColumn(jobtypes[i].Acronym+""+jobtypes[i].DepartmentId, typeof(System.String));
                    table.Columns.Add(col);

                    TemplateField tfield = new TemplateField();
                    tfield.HeaderText = jobtypes[i].Acronym;
                    gridViewMain.Columns.Add(tfield);
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
                            row[jobtypes[i].Acronym+""+jobtypes[i].DepartmentId] = "";
                        }
                        else
                        {
                            row[jobtypes[i].Acronym + "" + jobtypes[i].DepartmentId] = j.Id+"|"+j.JobStatus + " " + Convert.ToDateTime(j.EndTime).ToString("dd-MMM-yyyy");
                        }
                    }
                }
                table.Rows.Add(row);
            }
        }

        protected void gridViewMain_RowCreated(object sender, GridViewRowEventArgs e)
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
                JobType jobtype = new JobType();
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
                    var jobtypes = jobtype.GetJobOverviewJobType(departments[i].Id);
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
                gridViewMain.Controls[0].Controls.AddAt(0, gvr);
            }
            
        }

        protected void gridViewMain_RowDataBound(object sender, GridViewRowEventArgs e) 
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
                        lnkBtn.ID = "modalLinkStatus";
                        lnkBtn.Text = d[1];
                        lnkBtn.Click += new EventHandler(LinkButtonAction);
                        lnkBtn.CommandArgument = d[0];
                        e.Row.Cells[i].Controls.Add(lnkBtn);
                        //lnkBtn.Command += lnkBtn_Command;
                        e.Row.Cells[i].BackColor = System.Drawing.Color.LightGreen;
                    }
                    else if (s.IndexOf("In Progress") > -1)
                    {
                        string[] d = s.Split('|');
                        LinkButton lnkBtn = new LinkButton();
                        lnkBtn.ID = "modalLinkStatus";
                        lnkBtn.Text = d[1];
                        lnkBtn.Click += new EventHandler(LinkButtonAction);
                        lnkBtn.CommandArgument = d[0];
                        //lnkBtn.Command += lnkBtn_Command;
                        e.Row.Cells[i].Controls.Add(lnkBtn);
                        e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                    }
                }
            }
        }


        protected void lnkBtn_Command(object sender, CommandEventArgs e) 
        {
            GenerateDynamicGrid();
        }

        protected void LinkButtonAction(object sender, EventArgs e) 
        {
            LinkButton lnkBtn = (sender as LinkButton);
            GridViewRow row = (lnkBtn.NamingContainer as GridViewRow);
            GenerateDynamicGrid();
        }


        #region OTHERS

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