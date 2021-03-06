﻿using System;
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
        List<Department> joboverviewDepartment = new List<Department>();
        List<JobTracker> distincProjectList = new List<JobTracker>();
        List<List<JobType>> joboverviewJobType = new List<List<JobType>>();
        List<JobFlow> jobflowList = new List<JobFlow>();
        List<List<JobTypeFlow>> jobtypeflowlist = new List<List<JobTypeFlow>>();
        List<List<List<JobTracker>>> joboverviewRow = new List<List<List<JobTracker>>>();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            JobTracker jobtracker = new JobTracker();
            if (!isValidUser() || (!jobtracker.CanConnectToCAP()))
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
            joboverviewDepartment = dept.GetJobOverviewDepartment();
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
            joboverviewJobType = new List<List<JobType>>();
            foreach (Department d in joboverviewDepartment) //Creates the columns 
            {
                var jobtypes = jobtypeDepartment.GetJobOverviewJobType(d.Id);
                joboverviewJobType.Add(jobtypes);
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
            //joboverviewDepartment = dept.GetJobOverviewDepartment();
            distincProjectList = jobtracker.GetDistinctProjectList(Convert.ToDateTime(txtBoxStartDate.Text + " 00:00:00"), Convert.ToDateTime(txtBoxEndDate.Text + " 23:59:59"), txtBoxJobId.Text.Trim());
            joboverviewRow = new List<List<List<JobTracker>>>();
            for (int y = 0; y < distincProjectList.Count;y++ )
            {
                joboverviewRow.Add(new List<List<JobTracker>>());
                DataRow row = table.NewRow();
                row["HW No"] = distincProjectList[y].HWNo == null ? "" : distincProjectList[y].HWNo.Trim();
                row["SW No"] = distincProjectList[y].SWNo == null ? "" : distincProjectList[y].SWNo.Trim();
                row["Customer"] = distincProjectList[y].Customer == null ? "" : distincProjectList[y].Customer.Trim();
                row["Description"] = distincProjectList[y].Description == null ? "" : distincProjectList[y].Description.Trim();
                for (int x = 0; x < joboverviewDepartment.Count; x++)
                {
                    List<JobTracker> jtlist = new List<JobTracker>();
                    for (int i = 0; i < joboverviewJobType[x].Count; i++)
                    {
                        JobTracker l = jobtracker.GetJobTrackerJobOverview(joboverviewJobType[x][i].Id, distincProjectList[y].SWNo, distincProjectList[y].HWNo, Convert.ToDateTime(txtBoxStartDate.Text + " 00:00:00"), Convert.ToDateTime(txtBoxEndDate.Text + " 23:59:59"), joboverviewDepartment[x].Id);
                        jtlist.Add(l);
                    }
                    joboverviewRow[y].Add(jtlist);
                    JobTracker j = new JobTracker();
                    string curstatus = "";
                    int curindex = -1;
                    for (int i = 0; i < jtlist.Count; i++)
                    {
                        if (jtlist[i] == null)
                            continue;
                        else if (jtlist[i].JobStatus.IndexOf("On Hold") > -1)
                        {
                            curstatus = "On Hold";
                            curindex = i;

                            break;
                        }
                        else if (jtlist[i].JobStatus.IndexOf("In Progress") > -1)
                        {
                            if (curstatus != "In Progress")
                            {
                                curstatus = "In Progress";
                                curindex = i;
                            }
                        }
                        else if (jtlist[i].JobStatus.IndexOf("Completed") > -1 && curindex < 0)
                        {
                            if (curstatus == "")
                            {
                                curstatus = "Completed";
                                curindex = i;
                            }
                        }
                    }
                    if (curindex != -1)
                    {
                        j = jtlist[curindex];
                    }
                    //JobTracker j = jobtracker.GetJobTrackerJobOverview(p.SWNo, p.HWNo, Convert.ToDateTime(txtBoxStartDate.Text + " 00:00:00"), Convert.ToDateTime(txtBoxEndDate.Text + " 23:59:59"), d.Id);
                    if (j == null)
                    {
                        row[joboverviewDepartment[x].Acronym + "" + joboverviewDepartment[x].Id] = "";
                    }
                    else
                    {
                        row[joboverviewDepartment[x].Acronym + "" + joboverviewDepartment[x].Id] = joboverviewDepartment[x].Id + "|" + j.JobStatus + " " + Convert.ToDateTime(j.EndTime).ToString("dd-MMM-yyyy") + "|" + distincProjectList[y].HWNo + "|" + distincProjectList[y].SWNo;
                    }
                }
                table.Rows.Add(row);
            }
        }

        protected void gridViewSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow) //Check if RowType is DataRow
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
            //var departments = dept.GetJobOverviewDepartment();
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

            //joboverviewJobType = new List<List<JobType>>();
            for (int x = 0; x < joboverviewDepartment.Count;x++ ) //Creates the columns 
            {
                //var jobtypes = jobtypeDepartment.GetJobOverviewJobType(joboverviewDepartment[x].Id);
                //joboverviewJobType.Add(jobtypes);
                for (int i = 0; i < joboverviewJobType[x].Count; i++)
                {
                    DataColumn col = new DataColumn(joboverviewJobType[x][i].Acronym + "" + joboverviewDepartment[x].Id, typeof(System.String));
                    table.Columns.Add(col);

                    TemplateField tfield = new TemplateField();
                    tfield.HeaderText = joboverviewJobType[x][i].Acronym;
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
            //var departments = dept.GetJobOverviewDepartment();
            //var distinctProjectList = jobtracker.GetDistinctProjectList(Convert.ToDateTime(txtBoxStartDate.Text + " 00:00:00"), Convert.ToDateTime(txtBoxEndDate.Text + " 23:59:59"), txtBoxJobId.Text.Trim());

            for (int y = 0; y < distincProjectList.Count;y++ )
            {
                DataRow row = table.NewRow();
                row["HW No"] = distincProjectList[y].HWNo == null ? "" : distincProjectList[y].HWNo.Trim();
                row["SW No"] = distincProjectList[y].SWNo == null ? "" : distincProjectList[y].SWNo.Trim();
                row["Customer"] = distincProjectList[y].Customer == null ? "" : distincProjectList[y].Customer.Trim();
                row["Description"] = distincProjectList[y].Description == null ? "" : distincProjectList[y].Description.Trim();
                for (int x = 0; x < joboverviewDepartment.Count; x++)
                {
                    //var jobtypes = jobtypeDepartment.GetJobOverviewJobType(d.Id);
                    for (int i = 0; i < joboverviewJobType[x].Count; i++)
                    {
                        //JobTracker j = jobtracker.GetJobTrackerJobOverview(joboverviewJobType[x][i].Id, p.SWNo, p.HWNo, Convert.ToDateTime(txtBoxStartDate.Text + " 00:00:00"), Convert.ToDateTime(txtBoxEndDate.Text + " 23:59:59"), joboverviewDepartment[x].Id);
                        if (joboverviewRow[y][x][i] == null)
                        {
                            row[joboverviewJobType[x][i].Acronym + "" + joboverviewDepartment[x].Id] = "";
                        }
                        else
                        {
                            row[joboverviewJobType[x][i].Acronym + "" + joboverviewDepartment[x].Id] = joboverviewRow[y][x][i].Id + "|" + joboverviewRow[y][x][i].JobStatus + " " + Convert.ToDateTime(joboverviewRow[y][x][i].EndTime).ToString("dd-MMM-yyyy");
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
                //var departments = dept.GetJobOverviewDepartment();
                int rowIndex = 4;
                for (int i = 0; i < joboverviewDepartment.Count; i++)
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
                    //var jobtypes = jobtypeDepartment.GetJobOverviewJobType(joboverviewDepartment[i].Id);
                    if (joboverviewJobType[i].Count > 0)
                    {
                        thc = new TableHeaderCell();
                        thc.BackColor = backcolor;
                        thc.ForeColor = forecolor;
                        thc.ColumnSpan = joboverviewJobType[i].Count;
                        thc.Text = joboverviewDepartment[i].Description;
                        gvr.Cells.Add(thc);
                    }
                    for (int x = 0; x < joboverviewJobType[i].Count; x++) 
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

        #region JOB FLOW
        private DataTable GenerateFlowColumns()
        {
            DataTable table = new DataTable();

            JobFlow jobflow = new JobFlow();
            JobTypeFlow jobtypeflow = new JobTypeFlow();
            jobflowList = jobflow.GetJobOverviewJobFlow();
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

            gridViewJobFlow.Columns.Add(bfHw);
            gridViewJobFlow.Columns.Add(bfSw);
            gridViewJobFlow.Columns.Add(bfCus);
            gridViewJobFlow.Columns.Add(bfDes);
            jobtypeflowlist = new List<List<JobTypeFlow>>();
            for (int x = 0; x < jobflowList.Count;x++ ) //Creates the columns 
            {
                var jobtypes = jobtypeflow.GetJobTypeFlowListByJobFlow(jobflowList[x].Id);
                jobtypeflowlist.Add(jobtypes);
                for (int i = 0; i < jobtypes.Count; i++)
                {
                    string deptacro = "";
                    if (jobtypes[i].DepartmentId != null)
                        deptacro = jobtypes[i].departmentAcronym;
                    DataColumn col = new DataColumn(jobtypes[i].jobtypeAcronym + deptacro + "" + jobflowList[x].Id, typeof(System.String));
                    table.Columns.Add(col);

                    TemplateField tfield = new TemplateField();
                    string sdept = "";
                    if (jobtypes[i].DepartmentId != null)
                        sdept = jobtypes[i].departmentAcronym + "-";
                    tfield.HeaderText = sdept + jobtypes[i].jobtypeAcronym;
                    gridViewJobFlow.Columns.Add(tfield);
                }
            }

            return table;
        }

        private void GenerateFlowRows(ref DataTable table)
        {
            JobFlow jobflow = new JobFlow();
            JobTypeFlow jobtypeFlow = new JobTypeFlow();
            JobTracker jobtracker = new JobTracker();
            //var jobflows = jobflow.GetJobOverviewJobFlow();
            //var distinctProjectList = jobtracker.GetDistinctProjectList(Convert.ToDateTime(txtBoxStartDate.Text + " 00:00:00"), Convert.ToDateTime(txtBoxEndDate.Text + " 23:59:59"), txtBoxJobId.Text.Trim());

            foreach (JobTracker p in distincProjectList)
            {
                DataRow row = table.NewRow();
                row["HW No"] = p.HWNo == null ? "" : p.HWNo.Trim();
                row["SW No"] = p.SWNo == null ? "" : p.SWNo.Trim();
                row["Customer"] = p.Customer == null ? "" : p.Customer.Trim();
                row["Description"] = p.Description == null ? "" : p.Description.Trim();
                for (int x = 0; x < jobflowList.Count; x++) 
                {
                    //var jobtypes = jobtypeFlow.GetJobTypeFlowListByJobFlow(jobflowList[x].Id);
                    for (int i = 0; i < jobtypeflowlist[x].Count; i++)
                    {
                        JobTracker j = new JobTracker();
                        string deptacro = "";
                        if (jobtypeflowlist[x][i].DepartmentId != null)
                            deptacro = jobtypeflowlist[x][i].departmentAcronym;

                        if (jobtypeflowlist[x][i].DepartmentId != null)
                        {
                            j = jobtracker.GetJobTrackerJobOverview(jobtypeflowlist[x][i].JobTypeId, p.SWNo, p.HWNo, Convert.ToDateTime(txtBoxStartDate.Text + " 00:00:00"), Convert.ToDateTime(txtBoxEndDate.Text + " 23:59:59"), Convert.ToInt32(jobtypeflowlist[x][i].DepartmentId));
                        }
                        else 
                        {
                            j = jobtracker.GetJobTrackerJobOverview(jobtypeflowlist[x][i].JobTypeId, p.SWNo, p.HWNo, Convert.ToDateTime(txtBoxStartDate.Text + " 00:00:00"), Convert.ToDateTime(txtBoxEndDate.Text + " 23:59:59"));
                        }
                        if (j == null)
                        {
                            row[jobtypeflowlist[x][i].jobtypeAcronym + deptacro + "" + jobflowList[x].Id] = "";
                        }
                        else
                        {
                            row[jobtypeflowlist[x][i].jobtypeAcronym + deptacro + "" + jobflowList[x].Id] = j.Id + "|" + j.JobStatus + " " + Convert.ToDateTime(j.EndTime).ToString("dd-MMM-yyyy");
                        }
                    }
                }
                table.Rows.Add(row);
            }
        }

        protected void gridViewJobFlow_RowCreated(object sender, GridViewRowEventArgs e) 
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
                JobFlow jobflow = new JobFlow();
                JobTypeFlow jobtypeflow = new JobTypeFlow();
                //var jobflows = jobflow.GetJobOverviewJobFlow();
                int rowIndex = 4;
                for (int i = 0; i < jobflowList.Count; i++)
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
                    //var jobtypes = jobtypeflow.GetJobTypeFlowListByJobFlow(jobflowList[i].Id);
                    if (jobtypeflowlist[i].Count > 0)
                    {
                        thc = new TableHeaderCell();
                        thc.BackColor = backcolor;
                        thc.ForeColor = forecolor;
                        thc.ColumnSpan = jobtypeflowlist[i].Count;
                        thc.Text = jobflowList[i].Description;
                        gvr.Cells.Add(thc);
                    }
                    for (int x = 0; x < jobtypeflowlist[i].Count; x++)
                    {
                        e.Row.Cells[rowIndex].BackColor = backcolor;
                        e.Row.Cells[rowIndex].ForeColor = forecolor;
                        rowIndex++;
                    }
                }
                gridViewJobFlow.Controls[0].Controls.AddAt(0, gvr);
            }
        }

        protected void gridViewJobFlow_RowDataBound(object sender, GridViewRowEventArgs e) 
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
                        lnkBtn.ID = "modalLinkJobFlowStatus" + d[0];
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
                        lnkBtn.ID = "modalLinkJobFlowStatus" + d[0];
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
                        lnkBtn.ID = "modalLinkJobFlowStatus" + d[0];
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
        #region INITIALIZE
        private void GenerateDynamicGrid()
        {
            DataTable summarytable = new DataTable();
            DataTable detailtable = new DataTable();
            DataTable jobflowtable = new DataTable();

            gridViewSummary.Columns.Clear();
            gridViewDetail.Columns.Clear();
            gridViewJobFlow.Columns.Clear();

            summarytable = GenerateSummaryColumns();
            detailtable = GenerateDetailsColumns();
            jobflowtable = GenerateFlowColumns();

            GenerateSummaryRows(ref summarytable);
            GenerateDetailsRows(ref detailtable);
            GenerateFlowRows(ref jobflowtable);

            gridViewSummary.DataSource = summarytable;
            gridViewDetail.DataSource = detailtable;
            gridViewJobFlow.DataSource = jobflowtable;

            gridViewSummary.DataBind();
            gridViewDetail.DataBind();
            gridViewJobFlow.DataBind();
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
        #endregion

        #region COMMAND
        protected void lnkBtn_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "JobOverviewDetails")
            {
                JobTracker j = new JobTracker();
                j = j.GetJobTracker(Convert.ToInt32(e.CommandArgument),false);
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

        protected void txtBoxJobId_Changed(object sender, EventArgs e) 
        {
        
        }
        #endregion

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