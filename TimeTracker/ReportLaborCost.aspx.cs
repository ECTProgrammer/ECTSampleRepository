using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeTracker.Model;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
//using System.Threading;
//using System.Reflection;

namespace TimeTracker
{
    public partial class ReportLaborCost : System.Web.UI.Page
    {
        RolesModuleAccess myAccessRights = new RolesModuleAccess();
        protected bool isFinish;

        protected void Page_Load(object sender, EventArgs e)
        {
            JobTracker jobtracker = new JobTracker();
            if (!isValidUser() || (!jobtracker.CanConnectToCAP()))
                Response.Redirect("Login.aspx");
            GetMyAccessRights();
            if (myAccessRights == null)
                Response.Redirect("Dashboard.aspx");
            HttpContext.Current.Session["siteSubHeader"] = "Report";
            HttpContext.Current.Session["selectedTab"] = "Report";
            //ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            //scriptManager.RegisterPostBackControl(this.hiddenButton);
            if (!IsPostBack)
            {
                txtBoxFrom.Attributes.Add("readonly", "readonly"); //prevents user from typing on calendar textbox
                txtBoxTo.Attributes.Add("readonly", "readonly"); //prevents user from typing on calendar textbox
                txtBoxFrom.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtBoxTo.Text = DateTime.Today.ToString("dd MMM yyyy");
                calendarExtenderFrom.SelectedDate = DateTime.Today;
                calendarExtenderFrom.EndDate = DateTime.Today;
                calendarExtenderTo.SelectedDate = DateTime.Today;
                calendarExtenderTo.StartDate = DateTime.Today;
                calendarExtenderTo.EndDate = DateTime.Today;
                if (Session["LaborCostReport"] == null)
                {
                    btnDownload.Visible = false;
                    lblReadyForDownload.Visible = false;
                }
                else
                {
                    lblReadyForDownload.Text = "Report Ready For Download : " + (string)Session["LaborCostReportName"];
                }
            }
        }

        #region COMMAND
        protected void txtBoxFrom_Changed(object sender, EventArgs e)
        {
            DateTime sdate = Convert.ToDateTime(txtBoxFrom.Text);
            calendarExtenderFrom.SelectedDate = sdate;
            DateTime edate = Convert.ToDateTime(txtBoxTo.Text);
            if (sdate > edate)
            {
                txtBoxTo.Text = txtBoxFrom.Text;
                calendarExtenderTo.SelectedDate = sdate;
            }
            calendarExtenderTo.StartDate = sdate;
        }

        protected void txtBoxTo_Changed(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(txtBoxTo.Text);
            calendarExtenderTo.SelectedDate = date;
        }

        protected void btnGenerate_Click(object sender, EventArgs e) 
        {
            Session["LaborCostReport"] = null;
            Session["LaborCostReportName"] = null;
            isFinish = false;
            //ThreadStart ts = new ThreadStart(NewTread);
            //Thread th = new Thread(ts);
            //th.Start();
            //while (isFinish == false)
            //{
            //    UpdatePanelMain.Update();
            //}
            IWorkbook workbook = GenerateHeader();
            GenerateReport(workbook);

            //using (var ms = new MemoryStream())
            //{
            //    workbook.Write(ms);

            //    Response.Clear();
            //    Response.ContentType = "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    Response.AppendHeader("Content-Disposition", "inline;filename=" + lblReadyForDownload.Text.Trim().Replace("Report Ready For Download : ", ""));
            //    Response.BinaryWrite(ms.ToArray());
            //    Response.End();
            //}
            Session["LaborCostReport"] = workbook;
            Session["LaborCostReportName"] = "LaborCostReport" + DateTime.Now.ToString("yyyyMMMdd") + ".xlsx";
            lblReadyForDownload.Text = "Report Ready For Download : " + (string)Session["LaborCostReportName"];
            lblReadyForDownload.Visible = true;
            btnDownload.Visible = true;

            //Response.Redirect("ReportLaborCost.aspx");
            //hiddenButton_Click(hiddenButton, new EventArgs());
            //MethodInfo clickMethod = typeof(Button).GetMethod("OnClick", BindingFlags.NonPublic | BindingFlags.Instance);
            //clickMethod.Invoke(hiddenButton, new object[] { EventArgs.Empty });
        }

        protected void btnDownload_Click(object sender, EventArgs e) 
        {
            if (Session["LaborCostReport"] != null)
            {
                IWorkbook workbook = (IWorkbook)Session["LaborCostReport"];
                using (var ms = new MemoryStream())
                {
                    workbook.Write(ms);

                    Response.Clear();
                    Response.ContentType = "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AppendHeader("Content-Disposition", "inline;filename="+lblReadyForDownload.Text.Trim().Replace("Report Ready For Download : ",""));
                    Response.BinaryWrite(ms.ToArray());
                    

                    Session["LaborCostReport"] = null;
                    Session["LaborCostReportName"] = null;
                    btnDownload.Visible = false;
                    lblReadyForDownload.Visible = false;
                    //Response.Redirect("ReportLaborCost.aspx");
                    Response.End();
                }
                
            }
            btnDownload.Visible = false;
            lblReadyForDownload.Visible = false;
            
        }
        private void NewTread() 
        {
            IWorkbook workbook = GenerateHeader();
            GenerateReport(workbook);
            Session["LaborCostReport"] = workbook;
            isFinish = true;
            //using (var ms = new MemoryStream())
            //{
            //    workbook.Write(ms);

            //    Response.Clear();
            //    Response.ContentType = "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    Response.AppendHeader("Content-Disposition", "inline;filename=Sample" + DateTime.Now.ToString("yyyyMMMdd") + ".xlsx");
            //    Response.BinaryWrite(ms.ToArray());
            //    Response.End();
            //}
        }

        private IWorkbook GenerateHeader() 
        {
            
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheetReport = workbook.CreateSheet("Report");
            IFont fontBold = workbook.CreateFont();
            fontBold.Boldweight = (short)FontBoldWeight.Bold;
            ICellStyle styleFontBold = workbook.CreateCellStyle();
            styleFontBold.SetFont(fontBold);
            ICell cell1 = sheetReport.CreateRow(0).CreateCell(0);
            cell1.CellStyle = styleFontBold;
            cell1.SetCellValue("From " + txtBoxFrom.Text + " to " + txtBoxTo.Text);
            sheetReport.CreateRow(1).CreateCell(0).SetCellValue("Labor Cost Report");
            sheetReport.GetRow(1).GetCell(0).CellStyle = styleFontBold;
           
            return workbook;
        }

        private void GenerateReport(IWorkbook workbook) 
        {
            JobTracker jobtracker = new JobTracker();
            List<JobTracker> projectList = jobtracker.GetDistinctProjectListIncludingForApproval(Convert.ToDateTime(txtBoxFrom.Text+ " 00:00:00"), Convert.ToDateTime(txtBoxTo.Text+" 23:59:59"));
            int currentrow = 1;

            IDataFormat format = workbook.CreateDataFormat();
            ICellStyle fontBoldNoBorder = CreateSheetStyle(workbook, false, false, false, false, true, false, false, false,false);
            ICellStyle fontBoldAllBorder = CreateSheetStyle(workbook, true, true, true, true, true, false, false, false);
            ICellStyle fontCurrencyBoldRigthBottom = CreateSheetStyle(workbook, false, false, true, true, true, false, false, false);
            fontCurrencyBoldRigthBottom.DataFormat = format.GetFormat("$#,##0.00_);[Red]($#,##0.00);\"-\"");
            ICellStyle fontBoldTopBottom = CreateSheetStyle(workbook, false, true, false, true, true, false, false, false);
            fontCurrencyBoldRigthBottom.Alignment = HorizontalAlignment.Center;
            fontCurrencyBoldRigthBottom.VerticalAlignment = VerticalAlignment.Center;
            ICellStyle fontCurrencyBoldAllBorder = CreateSheetStyle(workbook, true, true, true, true, true, false, false, false);
            fontBoldAllBorder.Alignment = HorizontalAlignment.Center;
            fontCurrencyBoldAllBorder.DataFormat = format.GetFormat("$#,##0.00_);[Red]($#,##0.00);\"-\"");
            ICellStyle fontNormalBorderLeftRight = CreateSheetStyle(workbook, true, false, true, false, false, false, false, false);
            fontBoldAllBorder.VerticalAlignment = VerticalAlignment.Center;
            ICellStyle fontNormalBorderLeftRightBottom = CreateSheetStyle(workbook, true, false, true,true, false, false, false, false);
            ICellStyle fontCurrencyBorderLeftRight = CreateSheetStyle(workbook, true, false, true, false, false, false, false, false);
            fontCurrencyBorderLeftRight.DataFormat = format.GetFormat("$#,##0.00_);[Red]($#,##0.00);\"-\"");
            ICellStyle fontCurrencyBorderLeftRightBottom = CreateSheetStyle(workbook, true, false, true, true, false, false, false, false);
            fontCurrencyBorderLeftRightBottom.DataFormat = format.GetFormat("$#,##0.00_);[Red]($#,##0.00);\"-\"");
            //fontNormalBorderLeftRightBottom.BorderTop = NPOI.SS.UserModel.BorderStyle.None;
            //fontNormalBorderLeftRightBottom.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            //fontNormalBorderLeftRightBottom.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            //fontCurrencyBorderLeftRightBottom.BorderTop = NPOI.SS.UserModel.BorderStyle.None;
            //fontCurrencyBorderLeftRightBottom.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            //fontCurrencyBorderLeftRightBottom.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            
            //fontCurrencyBoldRigthBottom.BorderLeft = NPOI.SS.UserModel.BorderStyle.None;

            ISheet sheetReport = workbook.GetSheet("Report");
            foreach (JobTracker project in projectList) 
            {  
                //Running Total Variable
                double runningNTApproved = 0;
                double runningNTForApproval = 0;
                double runningOTApproved = 0;
                double runningOTForApproval = 0;

                double runningNTApprovedCost = 0;
                double runningNTForApprovalCost = 0;
                double runningOTApprovedCost = 0;
                double runningOTForApprovalCost = 0;


                string jobheader = project.HWNo == null ? "" : project.HWNo.Trim() == "" ? "" : "HW SO: " + project.HWNo;
                jobheader += project.SWNo == null ? "" : project.SWNo.Trim() == "" ? "" : jobheader == "" ? "SW SO: " + project.SWNo : "; SW SO: " + project.SWNo;
                jobheader += project.EvalNo == null ? "" : project.EvalNo.Trim() == "" ? "" : jobheader == "" ? "EVAL NO: " + project.EvalNo : "; EVAL NO: " + project.EvalNo;
                currentrow += 2;
                IRow row = sheetReport.CreateRow(currentrow++);
                ICell cell = row.CreateCell(0);
                cell.CellStyle = fontBoldAllBorder;
                cell.SetCellValue(jobheader);
                for (int i = 1; i < 11; i++)
                {
                    cell = row.CreateCell(i);
                    cell.CellStyle = fontBoldAllBorder;
                }

                #region Header Upper Row
                row = sheetReport.CreateRow(currentrow);
                cell = row.CreateCell(0);
                cell.CellStyle = fontBoldAllBorder;
                cell.SetCellValue("JobStage");
                cell = row.CreateCell(1);
                cell.CellStyle = fontBoldAllBorder;
                cell.SetCellValue("Time");
                for (int i = 2; i < 5; i++) 
                {
                    cell = row.CreateCell(i);
                    cell.CellStyle = fontBoldAllBorder;
                }

                cell = row.CreateCell(5);
                cell.CellStyle = fontBoldAllBorder;
                cell.SetCellValue("Labor Cost");
                for (int i = 6; i < 9; i++)
                {
                    cell = row.CreateCell(i);
                    cell.CellStyle = fontBoldAllBorder;
                }
                cell = row.CreateCell(9);
                cell.CellStyle = fontBoldAllBorder;
                cell.SetCellValue("Total");
                cell = row.CreateCell(10);
                cell.CellStyle = fontBoldAllBorder;
                

                //Merging
                sheetReport.AddMergedRegion(new CellRangeAddress(currentrow -1, currentrow-1, 0, 10));
                sheetReport.AddMergedRegion(new CellRangeAddress(currentrow,currentrow,1,4));
                sheetReport.AddMergedRegion(new CellRangeAddress(currentrow,currentrow,5,8));
                sheetReport.AddMergedRegion(new CellRangeAddress(currentrow,currentrow,9,10));

                #endregion

                #region Header Lower Row
                row = sheetReport.CreateRow(++currentrow);

                cell = row.CreateCell(0);
                cell.CellStyle = fontBoldAllBorder;
                for (int i = 0; i < 5; i+=4)
                {
                    cell = row.CreateCell(1 + i);
                    cell.CellStyle = fontBoldAllBorder;
                    cell.SetCellValue("Normal Time(Approved)");

                    cell = row.CreateCell(2 + i);
                    cell.CellStyle = fontBoldAllBorder;
                    cell.SetCellValue("Normal Time(For Approval)");

                    cell = row.CreateCell(3 + i);
                    cell.CellStyle = fontBoldAllBorder;
                    cell.SetCellValue("Overtime(Approved)");

                    cell = row.CreateCell(4 + i);
                    cell.CellStyle = fontBoldAllBorder;
                    cell.SetCellValue("Overtime(For Approved)");
                }
                cell = row.CreateCell(9);
                cell.CellStyle = fontBoldAllBorder;
                cell.SetCellValue("Time");
                cell = row.CreateCell(10);
                cell.CellStyle = fontBoldAllBorder;
                cell.SetCellValue("Cost");

                //Merging
                sheetReport.AddMergedRegion(new CellRangeAddress(currentrow - 1, currentrow, 0, 0));
                #endregion

                #region Data
                List<JobTracker> uniqueJobType = jobtracker.GetUniqueComputedJobType(project.HWNo == null ? "" : project.HWNo.Trim(), project.HWNo == null ? "" : project.SWNo.Trim());
                
                #region DETAIL Page
                ISheet detailSheet = workbook.CreateSheet(jobheader.Replace(":",""));
                int detailRowCount = 0;
                IRow detailRow = detailSheet.CreateRow(detailRowCount++);
                detailRow.CreateCell(0).SetCellValue(jobheader);
                detailRow.GetCell(0).CellStyle = fontBoldNoBorder;
                detailRowCount++;
                detailRow = detailSheet.CreateRow(detailRowCount++);
                detailRow.CreateCell(0).SetCellValue("Labor Cost");
                detailRow.GetCell(0).CellStyle = fontBoldNoBorder;
                detailRow = detailSheet.CreateRow(detailRowCount++);
                string[] detailHeader = { "Department","Name", "Job Task", "Start Time", "End Time","Date", "Remarks","Status", "Normal Time","Overtime", "Normal Time Cost","Overtime Cost" };
                for (int i = 0; i < 12; i++)
                {
                    ICell detailCell = detailRow.CreateCell(i);
                    detailCell.CellStyle = fontBoldAllBorder;
                    detailCell.SetCellValue(detailHeader[i]);
                }

                for (int i = 0; i < uniqueJobType.Count; i++) 
                {
                    row = sheetReport.CreateRow(++currentrow);
                    cell = row.CreateCell(0);
                    cell.SetCellValue(uniqueJobType[i].jobtype);
                    if (i < uniqueJobType.Count - 1)
                    {
                        cell.CellStyle = fontNormalBorderLeftRight;
                    }
                    else
                    {
                        cell.CellStyle = fontNormalBorderLeftRightBottom;
                    }
                    List<JobTracker> data = jobtracker.GetJobTrackerListWithJobTypeIdHWSO(Convert.ToInt32(uniqueJobType[i].JobTypeId), project.HWNo == null ? "" : project.HWNo.Trim(), project.SWNo == null ? "" : project.SWNo.Trim(), true);
                    double ntApproved = 0;
                    double ntForApproval = 0;
                    double otApproved = 0;
                    double otForApproval = 0;

                    double ntApprovedCost = 0;
                    double ntForApprovalCost = 0;
                    double otApprovedCost = 0;
                    double otForApprovalCost = 0;
                    for (int x = 0; x < data.Count;x++ )
                    {
                        detailRow = detailSheet.CreateRow(detailRowCount++);
                        if (data[x].Status == "Approved")
                        {
                            ntApproved += data[x].normalmins;
                            otApproved += data[x].otmins;
                            ntApprovedCost += data[x].normalcost;
                            otApprovedCost += data[x].otcost;
                        }
                        else if (data[x].Status == "For Approval")
                        {
                            ntForApproval += data[x].normalmins;
                            otForApproval += data[x].otmins;
                            ntForApprovalCost += data[x].normalcost;
                            otForApprovalCost += data[x].otcost;
                        }
                        for (int y = 0; y < 12; y++)
                        {
                            ICell detailCell = detailRow.CreateCell(y);
                            if (y == 0)
                                detailCell.SetCellValue(data[x].department);
                            else if (y == 1)
                                detailCell.SetCellValue(data[x].fullname);
                            else if (y == 2)
                                detailCell.SetCellValue(data[x].jobtype);
                            else if (y == 3)
                                detailCell.SetCellValue(Convert.ToDateTime(data[x].StartTime).ToString("hh:mm tt"));
                            else if (y == 4)
                                detailCell.SetCellValue(Convert.ToDateTime(data[x].EndTime).ToString("hh:mm tt"));
                            else if (y == 5)
                                detailCell.SetCellValue(Convert.ToDateTime(data[x].ScheduleDate).ToString("dd-MMM-yyyy"));
                            else if (y == 6)
                                detailCell.SetCellValue(data[x].Remarks);
                            else if (y == 7)
                                detailCell.SetCellValue(data[x].Status);
                            else if (y == 8)
                                detailCell.SetCellValue(data[x].normalhours.Trim() == "0 min" ? "-" : data[x].normalhours);
                            else if (y == 9)
                                detailCell.SetCellValue(data[x].othours.Trim() == "0 min" ? "-" : data[x].othours);
                            else if (y == 10)
                                detailCell.SetCellValue(data[x].normalcost);
                            else if (y == 11)
                                detailCell.SetCellValue(data[x].otcost);

                            if (x == data.Count - 1 && i == uniqueJobType.Count -1)
                            {
                                if (y < 10)
                                    detailCell.CellStyle = fontNormalBorderLeftRightBottom;
                                else
                                    detailCell.CellStyle = fontCurrencyBorderLeftRightBottom;
                            }
                            else
                            {
                                if (y < 10)
                                    detailCell.CellStyle = fontNormalBorderLeftRight;
                                else
                                    detailCell.CellStyle = fontCurrencyBorderLeftRight;
                            }
                        }
                    }
                #endregion

                    #region GETTING THE TIME
                    //Running Time
                    runningNTApproved += ntApproved;
                    runningNTForApproval += ntForApproval;
                    runningOTApproved += otApproved;
                    runningOTForApproval += otForApproval;

                    runningNTApprovedCost += ntApprovedCost;
                    runningNTForApprovalCost += ntForApprovalCost;
                    runningOTApprovedCost += otApprovedCost;
                    runningOTForApprovalCost += otForApprovalCost;

                    string NTApproved = GenerateTimeConsumed(ntApproved);
                    string NTForApproval = GenerateTimeConsumed(ntForApproval);
                    string OTApproved = GenerateTimeConsumed(otApproved);
                    string OTForApproval = GenerateTimeConsumed(otForApproval);

                    string StotalTime = GenerateTimeConsumed(ntApproved + otApproved + ntForApproval + otForApproval);
                    #endregion

                    for (int j = 1; j < 11; j++) 
                    {
                        cell = row.CreateCell(j);
                        if (j == 1)
                            cell.SetCellValue(NTApproved.Trim() == "0 min" ? "-" : NTApproved.Trim());
                        else if (j == 2)
                            cell.SetCellValue(NTForApproval.Trim() == "0 min" ? "-" : NTForApproval.Trim());
                        else if (j == 3)
                            cell.SetCellValue(OTApproved.Trim() == "0 min" ? "-" : OTApproved.Trim());
                        else if (j == 4)
                            cell.SetCellValue(OTForApproval.Trim() == "0 min" ? "-" : OTForApproval.Trim());
                        else if (j == 5)
                            cell.SetCellValue(ntApprovedCost);
                        else if (j == 6)
                            cell.SetCellValue(ntForApprovalCost);
                        else if (j == 7)
                            cell.SetCellValue(otApprovedCost);
                        else if (j == 8)
                            cell.SetCellValue(otForApprovalCost);
                        else if (j == 9)
                            cell.SetCellValue(StotalTime.Trim() == "0 min" ? "-" : StotalTime.Trim());
                        else if (j == 10)
                            cell.SetCellValue(ntForApprovalCost + ntApprovedCost + otApprovedCost + otForApprovalCost);

                        if (i < uniqueJobType.Count - 1)
                        {
                            if(j < 5 || j == 9)
                                cell.CellStyle = fontNormalBorderLeftRight;
                            else
                                cell.CellStyle = fontCurrencyBorderLeftRight;
                        }
                        else
                        {
                            if (j < 5 || j == 9)
                                cell.CellStyle = fontNormalBorderLeftRightBottom;
                            else
                                cell.CellStyle = fontCurrencyBorderLeftRightBottom;
                        }
                    }
                }
                #endregion

               
                #region TOTAL, MATERIAL COST, OUTSIDE SERVICE 

                string projNTApproved = GenerateTimeConsumed(runningNTApproved);
                string projNTForApproval = GenerateTimeConsumed(runningNTForApproval);
                string projOTApproved = GenerateTimeConsumed(runningOTApproved);
                string projOTForApproval = GenerateTimeConsumed(runningOTForApproval);
                string projTotalTime = GenerateTimeConsumed(runningNTApproved + runningNTForApproval + runningOTApproved + runningOTForApproval);
                string projNTTotal = GenerateTimeConsumed(runningNTApproved + runningNTForApproval);
                string projOTTotal = GenerateTimeConsumed(runningOTApproved + runningOTForApproval);
                
                #region DETAIL TOTAL
                detailRow = detailSheet.CreateRow(detailRowCount++);
                for (int y = 0; y < 12; y++)
                {
                    ICell detailCell = detailRow.CreateCell(y);
                    if (y == 0)
                        detailCell.SetCellValue("TOTAL");
                    else if (y == 8)
                        detailCell.SetCellValue(projNTTotal.Trim() == "0 min" ? "-" : projNTTotal.Trim());
                    else if (y == 9)
                        detailCell.SetCellValue(projOTTotal.Trim() == "0 min" ? "-" : projOTTotal.Trim());
                    else if (y == 10)
                        detailCell.SetCellValue(runningNTApprovedCost + runningNTForApprovalCost);
                    else if (y == 11)
                        detailCell.SetCellValue(runningOTApprovedCost + runningOTForApprovalCost);
                    if (y < 10)
                        detailCell.CellStyle = fontBoldAllBorder;
                    else
                        detailCell.CellStyle = fontCurrencyBoldAllBorder;
                }
                #endregion

                #region MATERIAL COST
                MaterialCost materialCost = new MaterialCost();
                List<MaterialCost> projMaterialCost = new List<MaterialCost>();
                projMaterialCost = materialCost.GetMaterialCost(project.HWNo == null ? "" : project.HWNo.Trim() == "" ? "" : project.HWNo, project.SWNo == null ? "" : project.SWNo.Trim() == "" ? "" : project.SWNo, project.EvalNo == null ? "" : project.EvalNo.Trim() == "" ? "" : project.EvalNo);
                double totalmaterialBookedCost = 0;
                double totalmaterialActualCost = 0;
                if (projMaterialCost.Count > 0) 
                {
                    detailRowCount++; //Blank Row
                    #region HEADER
                    detailRow = detailSheet.CreateRow(detailRowCount++);
                    detailRow.CreateCell(0).SetCellValue("Material Cost");
                    detailRow.GetCell(0).CellStyle = fontBoldNoBorder;
                    string[] materialHeader = { "Item", "Description", "Part Number", "Booked Quantity", "Booked Unit Cost", "Booked Extended Cost", "Actual Quantity", "Actual Unit Cost", "Actual Extended Cost","Cost Overrun (Underrun)" };
                    detailRow = detailSheet.CreateRow(detailRowCount++);
                    for (int i = 0; i < 10; i++) 
                    {
                        ICell detailCell = detailRow.CreateCell(i);
                        detailCell.CellStyle = fontBoldAllBorder;
                        detailCell.SetCellValue(materialHeader[i]);
                    }
                    #endregion

                    #region DETAIL
                    for (int i = 0; i < projMaterialCost.Count; i++)
                    {
                        detailRow = detailSheet.CreateRow(detailRowCount++);
                        double bookcost = projMaterialCost[i].BookedQuantity * projMaterialCost[i].BookedUnitCost;
                        double actualcost = projMaterialCost[i].ActualUnitCost * projMaterialCost[i].ActualQuantity;
                        for (int y = 0; y < 10; y++)
                        {
                            ICell detailCell = detailRow.CreateCell(y);
                            if (y == 0)
                                detailCell.SetCellValue(i + 1);
                            else if (y == 1)
                                detailCell.SetCellValue(projMaterialCost[i].PartDescription);
                            else if (y == 2)
                                detailCell.SetCellValue(projMaterialCost[i].PartNo);
                            else if (y == 3)
                                detailCell.SetCellValue(projMaterialCost[i].BookedQuantity);
                            else if (y == 4)
                                detailCell.SetCellValue(projMaterialCost[i].BookedUnitCost);
                            else if (y == 5)
                                detailCell.SetCellValue(bookcost);
                            else if (y == 6)
                                detailCell.SetCellValue(projMaterialCost[i].ActualQuantity);
                            else if (y == 7)
                                detailCell.SetCellValue(projMaterialCost[i].ActualUnitCost);
                            else if (y == 8)
                                detailCell.SetCellValue(actualcost);
                            else if (y == 9)
                                detailCell.SetCellValue(actualcost - bookcost);
                            if (i < projMaterialCost.Count - 1)
                            {
                                if (y < 4 || y == 6)
                                    detailCell.CellStyle = fontNormalBorderLeftRight;
                                else
                                    detailCell.CellStyle = fontCurrencyBorderLeftRight;
                            }
                            else
                            {
                                if (y < 4 || y == 6)
                                    detailCell.CellStyle = fontNormalBorderLeftRightBottom;
                                else
                                    detailCell.CellStyle = fontCurrencyBorderLeftRightBottom;
                            }
                        }
                        totalmaterialBookedCost += bookcost;
                        totalmaterialActualCost += actualcost;
                    }
                    #endregion

                    #region TOTAL
                    detailRow = detailSheet.CreateRow(detailRowCount++);
                    for (int y = 0; y < 10; y++)
                    {
                        ICell detailCell = detailRow.CreateCell(y);
                        if (y == 0)
                            detailCell.SetCellValue("TOTAL");
                        else if (y == 5)
                            detailCell.SetCellValue(totalmaterialBookedCost);
                        else if (y == 8)
                            detailCell.SetCellValue(totalmaterialActualCost);
                        else if (y == 9)
                            detailCell.SetCellValue(totalmaterialActualCost - totalmaterialBookedCost);
                        if (y == 5 || y == 8 || y == 9)
                            detailCell.CellStyle = fontCurrencyBoldAllBorder;
                        else
                            detailCell.CellStyle = fontBoldAllBorder;
                    }
                    #endregion
                }
                #endregion

                #region OUTSIDE SERVICE
                OutsideService outsideServiceCost = new OutsideService();
                List<OutsideService> projOutServCost = new List<OutsideService>();
                projOutServCost = outsideServiceCost.GetOutsideServiceCost(project.HWNo == null ? "" : project.HWNo.Trim() == "" ? "" : project.HWNo, project.SWNo == null ? "" : project.SWNo.Trim() == "" ? "" : project.SWNo, project.EvalNo == null ? "" : project.EvalNo.Trim() == "" ? "" : project.EvalNo);
                double totalOutServBookedCost = 0;
                double totalOutServActualCost = 0;
                if (projOutServCost.Count > 0)
                {
                    detailRowCount++; //Blank Row
                    #region HEADER
                    detailRow = detailSheet.CreateRow(detailRowCount++);
                    detailRow.CreateCell(0).SetCellValue("Outside Service Cost");
                    detailRow.GetCell(0).CellStyle = fontBoldNoBorder;
                    string[] outsideServiceHeader = { "Item","Type", "Description", "Vendor", "Booked Quantity", "Booked Unit Cost", "Booked Extended Cost", "Actual Quantity", "Actual Unit Cost", "Actual Extended Cost", "Cost Overrun (Underrun)" };
                    detailRow = detailSheet.CreateRow(detailRowCount++);
                    for (int i = 0; i < 11; i++)
                    {
                        ICell detailCell = detailRow.CreateCell(i);
                        detailCell.CellStyle = fontBoldAllBorder;
                        detailCell.SetCellValue(outsideServiceHeader[i]);
                    }
                    #endregion

                    #region DETAIL
                    for (int i = 0; i < projOutServCost.Count; i++)
                    {
                        detailRow = detailSheet.CreateRow(detailRowCount++);
                        double bookcost = projOutServCost[i].BookedQuantity * projOutServCost[i].BookedUnitCost;
                        double actualcost = projOutServCost[i].ActualUnitCost * projOutServCost[i].ActualQuantity;
                        for (int y = 0; y < 11; y++)
                        {
                            ICell detailCell = detailRow.CreateCell(y);
                            if (y == 0)
                                detailCell.SetCellValue(i + 1);
                            else if (y == 1)
                                detailCell.SetCellValue(projOutServCost[i].PartType);
                            else if (y == 2)
                                detailCell.SetCellValue(projOutServCost[i].PartDescription);
                            else if (y == 3)
                                detailCell.SetCellValue(projOutServCost[i].Vendor);
                            else if (y == 4)
                                detailCell.SetCellValue(projOutServCost[i].BookedQuantity);
                            else if (y == 5)
                                detailCell.SetCellValue(projOutServCost[i].BookedUnitCost);
                            else if (y == 6)
                                detailCell.SetCellValue(bookcost);
                            else if (y == 7)
                                detailCell.SetCellValue(projOutServCost[i].ActualQuantity);
                            else if (y == 8)
                                detailCell.SetCellValue(projOutServCost[i].ActualUnitCost);
                            else if (y == 9)
                                detailCell.SetCellValue(actualcost);
                            else if (y == 10)
                                detailCell.SetCellValue(actualcost - bookcost);
                            if (i < projMaterialCost.Count - 1)
                            {
                                if (y < 5 || y == 7)
                                    detailCell.CellStyle = fontNormalBorderLeftRight;
                                else
                                    detailCell.CellStyle = fontCurrencyBorderLeftRight;
                            }
                            else
                            {
                                if (y < 5 || y == 7)
                                    detailCell.CellStyle = fontNormalBorderLeftRightBottom;
                                else
                                    detailCell.CellStyle = fontCurrencyBorderLeftRightBottom;
                            }
                        }
                        totalOutServBookedCost += bookcost;
                        totalOutServActualCost += actualcost;
                    }
                    #endregion

                    #region TOTAL
                    detailRow = detailSheet.CreateRow(detailRowCount++);
                    for (int y = 0; y < 11; y++)
                    {
                        ICell detailCell = detailRow.CreateCell(y);
                        if (y == 0)
                            detailCell.SetCellValue("TOTAL");
                        else if (y == 6)
                            detailCell.SetCellValue(totalOutServBookedCost);
                        else if (y == 9)
                            detailCell.SetCellValue(totalOutServActualCost);
                        else if (y == 10)
                            detailCell.SetCellValue(totalOutServActualCost - totalOutServBookedCost);
                        if (y == 6 || y == 9 || y == 10)
                            detailCell.CellStyle = fontCurrencyBoldAllBorder;
                        else
                            detailCell.CellStyle = fontBoldAllBorder;
                    }
                    #endregion
                }

                #endregion

                row = sheetReport.CreateRow(++currentrow);
                for (int j = 0; j < 11; j++)
                {
                    cell = row.CreateCell(j);
                    if (j == 0)
                        cell.SetCellValue("Total Labor Cost");
                    else if (j == 1)
                        cell.SetCellValue(projNTApproved.Trim() == "0 min" ? "-" : projNTApproved.Trim());
                    else if (j == 2)
                        cell.SetCellValue(projNTForApproval.Trim() == "0 min" ? "-" : projNTForApproval.Trim());
                    else if (j == 3)
                        cell.SetCellValue(projOTApproved.Trim() == "0 min" ? "-" : projOTApproved.Trim());
                    else if (j == 4)
                        cell.SetCellValue(projOTForApproval.Trim() == "0 min" ? "-" : projOTForApproval.Trim());
                    else if (j == 5)
                        cell.SetCellValue(runningNTApprovedCost);
                    else if (j == 6)
                        cell.SetCellValue(runningNTForApprovalCost);
                    else if (j == 7)
                        cell.SetCellValue(runningOTApprovedCost);
                    else if (j == 8)
                        cell.SetCellValue(runningOTForApprovalCost);
                    else if (j == 9)
                        cell.SetCellValue(projTotalTime.Trim() == "0 min" ? "-" : projTotalTime.Trim());
                    else if (j == 10)
                        cell.SetCellValue(runningNTApprovedCost + runningNTForApprovalCost + runningOTApprovedCost + runningOTForApprovalCost);
                    if (j < 5 || j == 9)
                        cell.CellStyle = fontBoldAllBorder;
                    else
                        cell.CellStyle = fontCurrencyBoldAllBorder;
                }
                row = sheetReport.CreateRow(++currentrow);
                row.CreateCell(0).SetCellValue("Material Cost");
                row.GetCell(0).CellStyle = fontBoldAllBorder;
                for (int j = 1; j < 11; j++)
                { 
                    cell = row.CreateCell(j);
                    if (j == 1)
                        cell.SetCellValue("Booked Cost:");
                    else if (j == 2)
                        cell.SetCellValue(totalmaterialBookedCost);
                    else if (j == 4)
                        cell.SetCellValue("Actual Cost:");
                    else if (j == 5)
                        cell.SetCellValue(totalmaterialActualCost);
                    else if (j == 7)
                        cell.SetCellValue("Cost Overrun (Underrun)");
                    else if (j == 8)
                        cell.SetCellValue(totalmaterialActualCost - totalmaterialBookedCost);
                    if (j == 1 || j == 4 || j == 7)
                        cell.CellStyle = fontBoldTopBottom;
                    else
                        cell.CellStyle = fontCurrencyBoldRigthBottom;
                }
                sheetReport.AddMergedRegion(new CellRangeAddress(currentrow, currentrow, 2, 3));
                sheetReport.AddMergedRegion(new CellRangeAddress(currentrow, currentrow, 5, 6));
                sheetReport.AddMergedRegion(new CellRangeAddress(currentrow, currentrow, 8, 10));

                row = sheetReport.CreateRow(++currentrow);
                row.CreateCell(0).SetCellValue("Outside Service Cost");
                row.GetCell(0).CellStyle = fontBoldAllBorder;
                for (int j = 1; j < 11; j++)
                {
                    cell = row.CreateCell(j);
                    if (j == 1)
                        cell.SetCellValue("Booked Cost:");
                    else if (j == 2)
                        cell.SetCellValue(totalOutServBookedCost);
                    else if (j == 4)
                        cell.SetCellValue("Actual Cost:");
                    else if (j == 5)
                        cell.SetCellValue(totalOutServActualCost);
                    else if (j == 7)
                        cell.SetCellValue("Cost Overrun (Underrun)");
                    else if (j == 8)
                        cell.SetCellValue(totalOutServActualCost - totalOutServBookedCost);
                    if (j == 1 || j == 4 || j == 7)
                        cell.CellStyle = fontBoldTopBottom;
                    else
                        cell.CellStyle = fontCurrencyBoldRigthBottom;
                }
                sheetReport.AddMergedRegion(new CellRangeAddress(currentrow, currentrow, 2, 3));
                sheetReport.AddMergedRegion(new CellRangeAddress(currentrow, currentrow, 5, 6));
                sheetReport.AddMergedRegion(new CellRangeAddress(currentrow, currentrow, 8, 10));

                #endregion

            }
        }

        #region Style Create

        private ICellStyle CreateSheetStyle(IWorkbook workbook, bool BorderLeft, bool BorderTop, bool BorderRight, bool BorderBottom, bool IsFontBold, bool IsFontItalize, bool IsFontUnderline, bool IsFontStrikeOut,bool applyColor = true)
        {
            IFont newfont = workbook.CreateFont();
            if(IsFontBold)
                newfont.Boldweight = (short)FontBoldWeight.Bold;
            newfont.IsItalic = IsFontItalize;
            newfont.IsStrikeout = IsFontStrikeOut;
            if(IsFontUnderline)
                newfont.Underline = FontUnderlineType.Single;
            ICellStyle newstyle = workbook.CreateCellStyle();
            newstyle.SetFont(newfont);

            if (BorderLeft)
                newstyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            else
                newstyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.None;
            if (BorderTop)
                newstyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            else
                newstyle.BorderTop = NPOI.SS.UserModel.BorderStyle.None;
            if (BorderRight)
                newstyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            else
                newstyle.BorderRight = NPOI.SS.UserModel.BorderStyle.None;
            if (BorderBottom)
                newstyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            else
                newstyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.None;
            if (applyColor == true)
            {
                newstyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightYellow.Index;
                newstyle.FillPattern = FillPattern.SolidForeground;
            }
            return newstyle;
        }
        #endregion



        #endregion

        #region OTHERS
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
            TimeTracker.Model.Module module = new TimeTracker.Model.Module();
            module = module.GetModule("ReportLaborCost.aspx");
            myAccessRights = myAccessRights.GetRolesModuleAccess(Convert.ToInt32(user.RoleId), module.Id);
        }

        private string GenerateTimeConsumed(double time) 
        {
            double hour = Math.Truncate(time / 60);
            double min = time % 60;
            string stringtime = hour == 0 && min == 0 ? "0 min" : (hour > 0 ? hour > 1 ? hour + " hrs" : hour + " hr" : "") + (hour > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
            return stringtime;
        }
        #endregion
    }
}