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
    public partial class JobTrack : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JobTracker jobtracker = new JobTracker();
            if (!isValidUser() || (!jobtracker.CanConnectToCAP())) //Check if the user is properly login and if the system can connect to CAP database
                Response.Redirect("Login.aspx"); //Redirects the user to login page
            HttpContext.Current.Session["siteSubHeader"] = "JobTrack";
            HttpContext.Current.Session["selectedTab"] = "JobTrack";

            if (!IsPostBack)
            {
                //makesure that the alertmodalpopup is on top
                string script = "<script type=\"text/javascript\"> //<![CDATA[Sys.Application.add_load(function(){setTimeout(function(){ var popup = $find(\"<%=programmaticAlertModalPopup.ClientID %>\"); popup._backgroundElement.style.zIndex = 10010; popup._foregroundElement.style.zIndex = 10011;},0);}0;//]]></script>";
                ClientScript.RegisterStartupScript(Page.GetType(), "OnLoad", script, false); //function to register new script
                txtBoxDate.Attributes.Add("readonly", "readonly"); //prevents user to manualy key in dates
                calendarExtenderDate.SelectedDate = DateTime.Now; //Initialize the selected date to be the current date
                calendarExtenderDate.EndDate = DateTime.Now; 
                txtBoxDate.Text = DateTime.Now.ToString("dd MMM yyyy");
                labelDay.Text = DateTime.Now.DayOfWeek.ToString()+", "+DateTime.Today.ToString("dd MMM yyyy");
                InitializeLabelTimeClock(DateTime.Today);
                InitializeWorkingHours();
                InitializeGrid();
            }
        }

        #region COMMAND
            //Action trigger when a new date is selected
        protected void txtBoxDate_TextChanged(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(txtBoxDate.Text);
            calendarExtenderDate.SelectedDate = date;
            labelDay.Text = date.DayOfWeek.ToString() + ", " + date.ToString("dd MMM yyyy") ;
            if (date.CompareTo(DateTime.Today) < 0)
            {
                labelDay.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff0000");
                //LabelTotalHours.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff0000");
            }
            else 
            {
                labelDay.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0088cc");
                //LabelTotalHours.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0088cc");
            }
            InitializeLabelTimeClock(date);
            InitializeWorkingHours();
            InitializeGrid();
        }

        //Action triggered when the users click the add job track button
        protected void linkBtnAddJobTrack_Click(object sender, EventArgs e) 
        {
            Session["StartTime"] = null;
            JobTracker jobtracker = new JobTracker();
            DateTime selectedDate = DateTime.Parse(txtBoxDate.Text);
            int userid = Convert.ToInt32(Session["UserId"]);
            bool noError = true;
            
            if (jobtracker.HasUnclosedJobs(userid)) //check if user has unclosed jobs
            {
                noError = false;
                panelAlertHeader2.CssClass = "modalAlertHeader";
                alertModalBtnOK2.CssClass = "buttonalert";
                labelAlertHeader2.Text = "Error";
                labelAlertMessage2.Text = "Please close all jobs before adding a new one.";
                programmaticAlertModalPopup2.Show();
            }
            if (selectedDate.CompareTo(DateTime.Today) < 0)
            {
                SupervisorMapping supmap = new SupervisorMapping();
                if (supmap.GetActiveSupervisors(userid).Count < 1)
                {
                    noError = false;
                    panelAlertHeader2.CssClass = "modalAlertHeader";
                    alertModalBtnOK2.CssClass = "buttonalert";
                    labelAlertHeader2.Text = "Error";
                    labelAlertMessage2.Text = "Sorry you cannot add job on previous date without a direct supervisor. Please contact your system administrator.";
                    programmaticAlertModalPopup2.Show();
                }
            }
            else 
            {
                string errMsg = "";
                errMsg = jobtracker.GetError(userid, selectedDate, 30); //check if there is an error
                if (errMsg.Trim() != "")
                {
                    noError = false;
                    panelAlertHeader2.CssClass = "modalAlertHeader";
                    alertModalBtnOK2.CssClass = "buttonalert";
                    labelAlertHeader2.Text = "Error";
                    labelAlertMessage2.Text = errMsg.Trim() + " Please settle error first.";
                    programmaticAlertModalPopup2.Show();
                }
            }
            if(noError)
            {
                InitializeModalJobType();
                //InitializeModalStartTime();
                //InitializeModalEndTime();
                InitializeModalJobStatus();

                bool generateBlank = false;
                if (selectedDate.CompareTo(DateTime.Today) == 0)
                    generateBlank = true;
                GenerateStartHour();
                GenerateStartMin(Convert.ToInt32(modalDropDownStartTimeHour.SelectedItem.Value));
                GenerateEndHour(Convert.ToInt32(modalDropDownStartTimeHour.SelectedItem.Value), Convert.ToInt32(modalDropDownStartTimeMin.SelectedItem.Value), generateBlank);
                GenerateEndMin(Convert.ToInt32(modalDropDownStartTimeHour.SelectedItem.Value), Convert.ToInt32(modalDropDownStartTimeMin.SelectedItem.Value), modalDropDownEndTimeHour.SelectedItem.Text.Trim());

                modalBtnSubmit.CommandArgument = "Add";

                modalBtnDelete.Visible = false;
                modalTxtBoxRemarks.Text = "";
                modalLabelError.Text = "";
                modalLabelError.Visible = false;
                modallabelBoxJobDescription.Text = "";
                modallabelCustomer.Text = "";
                modalTxtBoxJobId.Text = "";
                modalLabelHWSW.Text = "";
                modalLabelHWSW.ToolTip = "";
                modalLabelEvalNo.Text = "";
                //modalTxtBoxJobId.Enabled = false;
                Page.Validate();

                this.programmaticModalPopup.Show();
            }
        }

        protected void BtnBreak_Click(object sender, EventArgs e) 
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            JobType jobtype = new JobType();
            jobtype = jobtype.GetDefaultBreak();
            DateTime selectedDate = DateTime.Parse(txtBoxDate.Text);
            JobTracker jobtracker = new JobTracker();
            bool noError = true;
            User user = new User();
            user = user.GetUser(userid, selectedDate);

            if (jobtracker.HasPreviousUnclosedJobs(userid,DateTime.Today)) //check if user has unclosed jobs
            {
                noError = false;
                panelAlertHeader2.CssClass = "modalAlertHeader";
                alertModalBtnOK2.CssClass = "buttonalert";
                labelAlertHeader2.Text = "Error";
                labelAlertMessage2.Text = "Please close all jobs before adding a new one.";
                programmaticAlertModalPopup2.Show();
            }
            else
            {
                string errMsg = "";
                errMsg = jobtracker.GetError(userid, selectedDate, 30); //check if there is an error
                if (errMsg.Trim() != "")
                {
                    noError = false;
                    panelAlertHeader2.CssClass = "modalAlertHeader";
                    alertModalBtnOK2.CssClass = "buttonalert";
                    labelAlertHeader2.Text = "Error";
                    labelAlertMessage2.Text = errMsg.Trim() + " Please settle error first.";
                    programmaticAlertModalPopup2.Show();
                }
            }
            if (noError)
            {
                if (TimeSpan.Parse(user.startTime) > TimeSpan.Parse(user.endTime)) //check if user has shifting hours
                {
                    RunBreakButtonActionForShifting();
                }
                else
                {
                    RunBreakButtonAction();
                }
                
            }
        }

        protected void gridViewJobTrack_Command(object sender, GridViewCommandEventArgs e) 
        {
            if (e.CommandName == "Select")
            {
                modalBtnDelete.Visible = true;
                DateTime date = Convert.ToDateTime(txtBoxDate.Text);
                int i = Convert.ToInt32(e.CommandArgument);
                int userid = Convert.ToInt32(Session["UserId"]);
                int jobtrackerId = Convert.ToInt32(((Label)gridJobTrack.Rows[i].FindControl("labelJobTrackId")).Text);
                bool noError = true;
                if (date.CompareTo(DateTime.Today) < 0)
                {
                    SupervisorMapping supmap = new SupervisorMapping();
                    if (supmap.GetActiveSupervisors(userid).Count < 1)
                    {
                        noError = false;
                        panelAlertHeader2.CssClass = "modalAlertHeader";
                        alertModalBtnOK2.CssClass = "buttonalert";
                        labelAlertHeader2.Text = "Error";
                        labelAlertMessage2.Text = "Sorry you cannot edit job on previous date without a direct supervisor. Please contact your system administrator.";
                        programmaticAlertModalPopup2.Show();
                    }
                }
                else 
                {
                    JobTracker jobtracker = new JobTracker();
                    string errMsg = "";
                    errMsg = jobtracker.GetError(userid, date, 7);
                    if (errMsg.Trim() != "")
                    {
                        noError = false;
                        panelAlertHeader2.CssClass = "modalAlertHeader";
                        alertModalBtnOK2.CssClass = "buttonalert";
                        labelAlertHeader2.Text = "Error";
                        labelAlertMessage2.Text = errMsg.Trim() + " Please settle error first.";
                        programmaticAlertModalPopup2.Show();
                    }
                }
                if(noError)
                {
                    JobTracker jobtracker = new JobTracker();
                    jobtracker = jobtracker.GetJobTracker(jobtrackerId,false);
                    //List<JobTracker> datalist = new List<JobTracker>();
                    //datalist = jobtracker.GetJobTrackerList(userid, date);
                    modalLabelError.Text = "";
                    modalLabelError.Visible = false;
                    modalBtnSubmit.CommandArgument = jobtracker.Id.ToString();
                    modalBtnDelete.CommandArgument = jobtracker.Id.ToString();
                    
                    InitializeModalJobType(jobtracker.JobTypeId.ToString());
                    //InitializeModalStartTime(jobtracker.StartTime.ToString());
                    InitializeModalJobStatus(jobtracker.JobStatus == null ? "" : jobtracker.JobStatus.Trim());

                    bool generateBlank = false;
                    if (date.CompareTo(DateTime.Today) == 0)
                        generateBlank = true;

                    Session["StartTime"] = Convert.ToDateTime(jobtracker.StartTime).TimeOfDay.ToString();
                    GenerateStartHour(Convert.ToDateTime(jobtracker.StartTime).TimeOfDay.ToString());
                    GenerateStartMin(Convert.ToInt32(modalDropDownStartTimeHour.SelectedItem.Value), Convert.ToDateTime(jobtracker.StartTime).TimeOfDay.ToString());
                    

                    string endtime = "";
                    if (jobtracker.EndTime != null)
                        endtime = jobtracker.EndTime.ToString();
                    //InitializeModalEndTime(endtime);

                    GenerateEndHour(Convert.ToInt32(modalDropDownStartTimeHour.SelectedItem.Value), Convert.ToInt32(modalDropDownStartTimeMin.SelectedItem.Value), generateBlank, endtime == "" ? "" : Convert.ToDateTime(jobtracker.EndTime).ToString());
                    GenerateEndMin(Convert.ToInt32(modalDropDownStartTimeHour.SelectedItem.Value), Convert.ToInt32(modalDropDownStartTimeMin.SelectedItem.Value), modalDropDownEndTimeHour.SelectedItem.Text.Trim(), endtime == "" ? "" : Convert.ToDateTime(jobtracker.EndTime).TimeOfDay.ToString());
                    //if (datalist[i].ApprovedBy != null && datalist[i].ApprovedBy != userid && datalist[i].ApprovedBy != 0) 
                    //{
                    //    //InitializeModalSupervisor(datalist[i].ApprovedBy.ToString());
                    //    //modalLabelSupervisor.Visible = true;
                    //    //modalLabelSupColon.Visible = true;
                    //    //modalDropDownSupervisor.Visible = true;
                    //}
                    //else if (date.CompareTo(DateTime.Today) < 0) 
                    //{
                    //InitializeModalSupervisor();
                    //modalLabelSupervisor.Visible = true;
                    //modalLabelSupColon.Visible = true;
                    //modalDropDownSupervisor.Visible = true;
                    //}
                    //else
                    //{
                    //    //modalLabelSupervisor.Visible = false;
                    //    //modalLabelSupColon.Visible = false;
                    //    //modalDropDownSupervisor.Visible = false;
                    //}
                    if (jobtracker.JobIdNumber != null && jobtracker.JobIdNumber.Trim() != "")
                    {

                        modalTxtBoxJobId.Text = jobtracker.JobIdNumber;
                        JobTracker jobTracker = new JobTracker();
                        jobTracker = jobTracker.GenerateHWAndSW(modalTxtBoxJobId.Text.Trim());
                        modallabelBoxJobDescription.Text = jobTracker.Description;
                        modallabelCustomer.Text = jobTracker.Customer;
                        modalLabelHWSW.Text = jobTracker.HWNo;
                        modalLabelHWSW.ToolTip = jobTracker.SWNo;
                        modalLabelEvalNo.Text = jobTracker.EvalNo;
                    }

                    modalTxtBoxRemarks.Text = jobtracker.Remarks == null ? "" : jobtracker.Remarks.Trim();

                    Page.Validate();
                    this.programmaticModalPopup.Show();
                }
            }
        }

        #endregion

        #region INITIALIZED
        protected void InitializeGrid()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            JobTracker jobTracker = new JobTracker();
            List<JobTracker> data = new List<JobTracker>();
            DateTime date = Convert.ToDateTime(txtBoxDate.Text);
            data = jobTracker.GetJobTrackerList(userid,date,true);
            //Converter model = new Converter();
            //DataTable table = model.ConvertToDataTable(data);
            gridJobTrack.DataSource = data;
            gridJobTrack.DataBind();
            Session["StartTime"] = null;
            GenerateStartHour();
            if (modalDropDownStartTimeHour.Items.Count < 1)
            {
                linkBtnAddJobTrack.Visible = false;
            }
            else 
            {
                linkBtnAddJobTrack.Visible = true;
            }
           
            if (date.CompareTo(DateTime.Today) == 0)//If doing job track on the current day, show take a break button
            {
                BtnBreak.Text = "Take a break";
                BtnBreak.CssClass = "buttongreen";
                BtnBreak.Visible = true;
                if (data.Count > 0)
                {
                    JobType jobtype = new JobType();
                    jobtype = jobtype.GetJobType(Convert.ToInt32(data[data.Count - 1].JobTypeId));
                    if (jobtype.IsDefaultBreak == true && data[data.Count - 1].Status == "Pending")
                    {
                        BtnBreak.Text = "End break";
                        BtnBreak.CssClass = "buttonred";
                    }
                }
            }
            else
            {
                BtnBreak.Visible = false;
            }
        }

        protected void InitializeWorkingHours() 
        {
            DateTime date = Convert.ToDateTime(txtBoxDate.Text);
            int userid = Convert.ToInt32(Session["UserId"]);
            JobTracker jobtracker = new JobTracker();
            double totalmin = 0;
            LabelTotalHours.Text = "Total Working Hours: " + jobtracker.GetTotalHours(userid, "Approved", date, ref totalmin);
            if (totalmin >= 480)
                LabelTotalHours.ForeColor = System.Drawing.ColorTranslator.FromHtml("#17990B");
            else
                LabelTotalHours.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff0000");
            
        }

        private void InitializeLabelTimeClock(DateTime seltime) 
        {
            TimeClock timeclock = new TimeClock();
            int userid = Convert.ToInt32(Session["UserId"]);
            LabelTimeClock.Text = "";
            
            User user = new User();
            user = user.GetUser(userid,seltime);
            if (user != null)
            {
                if (TimeSpan.Parse(user.startTime) > TimeSpan.Parse(user.endTime)) //user with shifting hour 
                {
                    TimeSpan cutOfTime = user.GetMyCutOfTime();
                    string cot = cutOfTime.ToString("hh\\:mm");
                    timeclock = timeclock.GetStartEndTimeForShifting(Convert.ToInt32(user.EmployeeNumber), Convert.ToDateTime(seltime.AddDays(-1).ToString("yyyy-MM-dd") + " " + cutOfTime.ToString("hh\\:mm")), Convert.ToDateTime(seltime.ToString("yyyy-MM-dd") + " " + cutOfTime.ToString("hh\\:mm")), false);
                    if (timeclock != null)
                    {
                        LabelTimeClock.Text = "Time-In: " + timeclock.starttime.ToString("hh:mm tt") + ", Time-Out: " + timeclock.endtime.ToString("hh:mm tt");
                    }
                    timeclock = new TimeClock();
                    timeclock = timeclock.GetStartEndTimeForShifting(Convert.ToInt32(user.EmployeeNumber), Convert.ToDateTime(seltime.ToString("yyyy-MM-dd") + " " + cutOfTime.ToString("hh\\:mm\\:ss")), Convert.ToDateTime(seltime.AddDays(1).ToString("yyyy-MM-dd") + " " + cutOfTime.ToString("hh\\:mm\\:ss")), true);
                    if (timeclock != null) 
                    {
                        if (LabelTimeClock.Text != "")
                            LabelTimeClock.Text += " | ";
                        LabelTimeClock.Text += "Time-In: " + timeclock.starttime.ToString("hh:mm tt") + ", Time-Out: " + timeclock.endtime.ToString("hh:mm tt");
                    }
                }
                else
                {
                    DateTime sdate = Convert.ToDateTime(seltime.ToString("yyyy-MM-dd") + " 00:00");
                    DateTime edate = Convert.ToDateTime(seltime.ToString("yyyy-MM-dd") + " 23:59");
                    timeclock = timeclock.GetStartEndTime(Convert.ToInt32(user.EmployeeNumber), sdate, edate);
                    if (timeclock != null)
                    {
                        LabelTimeClock.Text = "Time-In: " + timeclock.starttime.ToString("hh:mm tt") + ", Time-Out: " + timeclock.endtime.ToString("hh:mm tt");
                    }
                }
            }
        }

        #endregion

        #region MODAL

        #region COMMAND
        protected void modalBtnSubmit_Command(object sender, CommandEventArgs e) 
        {
            bool haserror = false;
            string errormessage = "";

            if (modalLabelError.Visible == true)
            {
                errormessage = modalLabelError.Text;
                haserror = true;
            }
            else if (modalReqJobStatus.Enabled == true && modalDropDownJobStatus.SelectedItem.Value == "Completed" && modalDropDownEndTimeHour.SelectedItem.Text == "--")
            {
                errormessage = "Cannot set job status as Completed without selecting End Time.";
                haserror = true;
            }
            
            if (haserror) 
            {
                //System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='JavaScript'>alert('Error: "+errormessage+"')</SCRIPT>");
                panelAlertHeader2.CssClass = "modalAlertHeader";
                alertModalBtnOK2.CssClass = "buttonalert";
                labelAlertHeader2.Text = "Error";
                labelAlertMessage2.Text = errormessage;
                this.programmaticModalPopup.Show();
                programmaticAlertModalPopup2.Show();

            }
            else
            {
                DateTime selectedDate = DateTime.Parse(txtBoxDate.Text);
                int userid = Convert.ToInt32(Session["UserId"]);
                JobTracker jobTracker = new JobTracker();
                JobTrackerHistory jtHist = new JobTrackerHistory();
                //DateTime startTime = DateTime.Parse(selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " " + modalDropDownStartTime.SelectedValue + ":00");
                DateTime startTime = DateTime.Parse(selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " " + modalDropDownStartTimeHour.SelectedValue+":"+modalDropDownStartTimeMin.SelectedValue + ":00");
                //DateTime endTime = DateTime.Parse(selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " " + modalDropDownEndTime.SelectedValue + ":00");
                DateTime endTime;
                if (modalDropDownEndTimeHour.SelectedValue == "24") //end date selected is 12:00 AM next day
                {
                    endTime = DateTime.Parse((selectedDate.AddDays(1)).Year + "-" + (selectedDate.AddDays(1)).Month + "-" + (selectedDate.AddDays(1)).Day + " 00:00:00");
                }
                else 
                {
                    endTime = DateTime.Parse(selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " " + modalDropDownEndTimeHour.SelectedValue + ":" + modalDropDownEndTimeMin.SelectedValue + ":00");
                }
                jobTracker.StartTime = startTime;
                jobTracker.ScheduleDate = selectedDate;
                //if (modalDropDownEndTime.SelectedItem.Text != "Select End Time")
                if (modalDropDownEndTimeHour.SelectedItem.Text != "--")
                {
                    jobTracker.EndTime = endTime;
                }
                jobTracker.JobTypeId = Convert.ToInt32(modalDropDownJobType.SelectedValue);
                jobTracker.JobIdNumber = modalTxtBoxJobId.Text.Trim();
                jobTracker.Remarks = modalTxtBoxRemarks.Text.Trim();
                jobTracker.LastUpdateDate = DateTime.Now;
                jobTracker.LastUpdatedBy = userid;
                jobTracker.UserId = userid;
                jobTracker.HWNo = modalLabelHWSW.Text;
                jobTracker.SWNo = modalLabelHWSW.ToolTip;
                jobTracker.Customer = modallabelCustomer.Text;
                jobTracker.EvalNo = modalLabelEvalNo.Text;
                jobTracker.Description = modallabelBoxJobDescription.Text;
                //if (jobTracker.JobIdNumber.Trim() != "")
                jobTracker.JobStatus = modalDropDownJobStatus.SelectedItem.Value;
                if (modalReqJobStatus.Enabled == false) // Put blank on task that does not require job status.
                    jobTracker.JobStatus = "";

                if (selectedDate.CompareTo(DateTime.Today) == 0)
                {
                    jobTracker.ApprovedBy = userid;
                    //if (modalDropDownEndTime.SelectedItem.Text != "Select End Time")
                    if (modalDropDownEndTimeHour.SelectedItem.Text != "--")
                        jobTracker.Status = "Approved";
                    else
                        jobTracker.Status = "Pending";
                }
                else
                {
                    jobTracker.Status = "For Approval";
                }

                if (e.CommandArgument.ToString() == "Add")
                {
                    if (selectedDate.CompareTo(DateTime.Today) != 0) 
                    {
                        if ((DateTime.Now.Subtract(startTime)).TotalHours < 24)
                        {
                            jobTracker.Status = "Approved";
                            jobTracker.ApprovedBy = userid;
                        }
                    }
                    jobTracker.CreateDate = DateTime.Now;
                    jobTracker.SupervisorRemarks = "";
                    jobTracker.CreatedBy = userid;
                    jobTracker.ActionRequest = "Add";
                    jobTracker.Insert(jobTracker);
                    jobTracker = jobTracker.GetJobTracker(Convert.ToInt32(jobTracker.CreatedBy), Convert.ToInt32(jobTracker.LastUpdatedBy), Convert.ToDateTime(jobTracker.StartTime), Convert.ToInt32(jobTracker.JobTypeId), jobTracker.ActionRequest, jobTracker.Status,false);
                }
                else 
                {
                    if (selectedDate.CompareTo(DateTime.Today) != 0)
                    {
                        if ((DateTime.Now.Subtract(startTime)).TotalHours < 2)
                        {
                            jobTracker.Status = "Approved";
                            jobTracker.ApprovedBy = userid;
                        }
                    }
                    jobTracker.Id = Convert.ToInt32(e.CommandArgument);
                    jobTracker.ActionRequest = "Update";
                    jobTracker.Update(jobTracker);
                }
                //jobTracker.CreateDate = DateTime.Now;
                jtHist = jtHist.ConvertToHistory(jobTracker);
                jtHist.Insert(jtHist);
                InitializeWorkingHours();
                InitializeGrid();
                this.programmaticModalPopup.Hide();
            }
            
        }

        protected void modalBtnDelete_Command(object sender, CommandEventArgs e) 
        {
            DateTime selectedDate = DateTime.Parse(txtBoxDate.Text);
            int userid = Convert.ToInt32(Session["UserId"]);
            JobTracker jobTracker = new JobTracker();
            JobTrackerHistory jtHist = new JobTrackerHistory();
            jobTracker.Id = Convert.ToInt32(e.CommandArgument);
            jobTracker = jobTracker.GetJobTracker(jobTracker.Id,false);
            if (selectedDate.CompareTo(DateTime.Today) == 0 || (jobTracker.Status == "Rejected" && jobTracker.ActionRequest != "Delete"))
            {
                jobTracker.ActionRequest = "Delete";
                jobTracker.Status = "Approved";
                jobTracker.LastUpdateDate = DateTime.Now;
                jobTracker.LastUpdatedBy = userid;
                jtHist = jtHist.ConvertToHistory(jobTracker);
                jobTracker.Delete(jobTracker.Id);
            }
            else 
            {
                jobTracker.ActionRequest = "Delete";
                jobTracker.Status = "For Approval";
                jobTracker.LastUpdateDate = DateTime.Now;
                jobTracker.LastUpdatedBy = userid;
                jobTracker.Update(jobTracker);
                jtHist = jtHist.ConvertToHistory(jobTracker);
            }
            jtHist.Insert(jtHist);
            InitializeWorkingHours();
            InitializeGrid();
        }

        protected void modalDropDownJobType_IndexChanged(object sender, EventArgs e) 
        {
            JobType jobType = new JobType();
            jobType.Id = Convert.ToInt32(modalDropDownJobType.SelectedItem.Value);
            jobType = jobType.GetJobType(jobType.Id);
            if (jobType != null) 
            {
                modalReqJobId.Enabled = Convert.ToBoolean(jobType.RequiredJobId); //Set jobid validation depending on the JobType setup.

                modalReqJobStatus.Enabled = Convert.ToBoolean(jobType.ComputeTime); //If time is computed, require Job Status, otherwise do not require it.
            }
            //if (modalTxtBoxJobId.Enabled == false) 
            //{
            //    modallabelCustomer.Text = "";
            //    modallabelBoxJobDescription.Text = "";
            //    modalTxtBoxJobId.Text = "";
            //    modalLabelError.Visible = false;
            //    InitializeModalJobStatus();
            //}
            this.programmaticModalPopup.Show();
        }

        //protected void modalDropDownStartTime_IndexChanged(object sender, EventArgs e) 
        //{
        //    Time time = new Time();
        //    Time stime = time.GetTime(modalDropDownStartTime.SelectedValue);
        //    Time etime = time.GetTime(modalDropDownEndTime.SelectedItem.Value);

        //    TimeSpan selectedTime = TimeSpan.Parse(modalDropDownStartTime.SelectedValue);
        //    TimeSpan curtime = TimeSpan.Parse(DateTime.Now.Hour + ":" + DateTime.Now.Minute);

        //    InitializeModalEndTime(modalDropDownStartTime.SelectedValue);
        //    //var endtime = time.GetEndTimeList(modalDropDownStartTime.SelectedValue);
        //    //modalDropDownEndTime.DataSource = endtime;
        //    //modalDropDownEndTime.DataTextField = "Description";
        //    //modalDropDownEndTime.DataValueField = "C24hrConversion";
        //    //modalDropDownEndTime.DataBind();

        //    if (etime != null)
        //    {
        //        if (etime.Position > stime.Position)
        //        {
        //            foreach (ListItem i in modalDropDownEndTime.Items)
        //            {
        //                if (i.Value == etime.C24hrConversion)
        //                {
        //                    i.Selected = true;
        //                    break;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            modalDropDownEndTime.Items[0].Selected = true;
        //        }
        //    }
        //    DateTime date = Convert.ToDateTime(txtBoxDate.Text);
        //    if (date.CompareTo(DateTime.Today) == 0)
        //    {
        //        if (selectedTime < curtime)
        //        {
        //            TimeSpan newTime = curtime.Subtract(selectedTime);
        //            if (newTime.TotalMinutes > 59)
        //            {
        //                panelAlertHeader.CssClass = "modalAlertHeader2";
        //                alertModalBtnOK.CssClass = "buttonalert2";
        //                labelAlertHeader.Text = "Notice";
        //                labelAlertMessage.Text = "You have selected a start time that is " + newTime.TotalMinutes + " minutes ago.";
        //                programmaticAlertModalPopup.Show();
        //            }
        //        }
        //    }
        //    this.programmaticModalPopup.Show();
        //}

        protected void modalDropDownStartTimeHour_IndexChanged(object sender, EventArgs e) 
        {
            bool generateBlank = false;
            DateTime selectedDate = DateTime.Parse(txtBoxDate.Text);
            TimeSpan curtime = TimeSpan.Parse(DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            if (selectedDate.CompareTo(DateTime.Today) == 0)
                generateBlank = true;

            GenerateStartMin(Convert.ToInt32(modalDropDownStartTimeHour.SelectedItem.Value));
            GenerateEndHour(Convert.ToInt32(modalDropDownStartTimeHour.SelectedItem.Value), Convert.ToInt32(modalDropDownStartTimeMin.SelectedItem.Value), generateBlank, modalDropDownEndTimeHour.Items.Count == 0 ? "" : modalDropDownEndTimeHour.SelectedItem.Text.Trim() + ":" + (modalDropDownEndTimeMin.Items.Count < 1 ? "00" : modalDropDownEndTimeMin.SelectedItem.Text.Trim()));
            GenerateEndMin(Convert.ToInt32(modalDropDownStartTimeHour.SelectedItem.Value), Convert.ToInt32(modalDropDownStartTimeMin.SelectedItem.Value), modalDropDownEndTimeHour.SelectedItem.Text.Trim(), modalDropDownEndTimeMin.Items.Count == 0 ? "" : modalDropDownEndTimeMin.SelectedItem.Text.Trim() + ":" + modalDropDownEndTimeMin.SelectedItem.Text.Trim());
            
            TimeSpan selectedTime = TimeSpan.Parse(modalDropDownStartTimeHour.SelectedValue + ":" + modalDropDownStartTimeMin.SelectedValue);
            if (selectedDate.CompareTo(DateTime.Today) == 0)
            {
                if (selectedTime < curtime)
                {
                    TimeSpan newTime = curtime.Subtract(selectedTime);
                    if (newTime.TotalMinutes > 59)
                    {
                        panelAlertHeader.CssClass = "modalAlertHeader2";
                        alertModalBtnOK.CssClass = "buttonalert2";
                        labelAlertHeader.Text = "Notice";
                        labelAlertMessage.Text = "You have selected a start time that is " + newTime.TotalMinutes + " minutes ago.";
                        programmaticAlertModalPopup.Show();
                    }
                }
            }
            this.programmaticModalPopup.Show();
        }

        protected void modalDropDownStartTimeMin_IndexChanged(object sender, EventArgs e)
        {
            bool generateBlank = false;
            DateTime selectedDate = DateTime.Parse(txtBoxDate.Text);
            if (selectedDate.CompareTo(DateTime.Today) == 0)
                generateBlank = true;
            GenerateEndHour(Convert.ToInt32(modalDropDownStartTimeHour.SelectedItem.Value), Convert.ToInt32(modalDropDownStartTimeMin.SelectedItem.Value), generateBlank, modalDropDownEndTimeHour.Items.Count == 0 ? "" : modalDropDownEndTimeHour.SelectedItem.Text.Trim() + ":" + (modalDropDownEndTimeMin.Items.Count == 0 ? "00" : modalDropDownEndTimeMin.SelectedItem.Text.Trim()));
            GenerateEndMin(Convert.ToInt32(modalDropDownStartTimeHour.SelectedItem.Value), Convert.ToInt32(modalDropDownStartTimeMin.SelectedItem.Value), modalDropDownEndTimeHour.SelectedItem.Text.Trim(), modalDropDownEndTimeMin.Items.Count == 0 ? "" : modalDropDownEndTimeHour.SelectedItem.Text.Trim() + ":" + modalDropDownEndTimeMin.SelectedItem.Text.Trim());
            TimeSpan curtime = TimeSpan.Parse(DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            TimeSpan selectedTime = TimeSpan.Parse(modalDropDownStartTimeHour.SelectedValue + ":" + modalDropDownStartTimeMin.SelectedValue);
            if (selectedDate.CompareTo(DateTime.Today) == 0)
            {
                if (selectedTime < curtime)
                {
                    TimeSpan newTime = curtime.Subtract(selectedTime);
                    if (newTime.TotalMinutes > 59)
                    {
                        panelAlertHeader.CssClass = "modalAlertHeader2";
                        alertModalBtnOK.CssClass = "buttonalert2";
                        labelAlertHeader.Text = "Notice";
                        labelAlertMessage.Text = "You have selected a start time that is " + newTime.TotalMinutes + " minutes ago.";
                        programmaticAlertModalPopup.Show();
                    }
                }
            }
            this.programmaticModalPopup.Show();
        }

        protected void modalDropDownEndTimeHour_IndexChanged(object sender, EventArgs e)
        {
            GenerateEndMin(Convert.ToInt32(modalDropDownStartTimeHour.SelectedItem.Value), Convert.ToInt32(modalDropDownStartTimeMin.SelectedItem.Value), modalDropDownEndTimeHour.SelectedItem.Text.Trim(),modalDropDownEndTimeMin.Items.Count == 0 ? "" : modalDropDownEndTimeHour.SelectedItem.Text.Trim() + ":" + modalDropDownEndTimeMin.SelectedItem.Text.Trim());
            this.programmaticModalPopup.Show();
        }

        protected void modalTxtBoxJobId_TextChanged(object sender, EventArgs e) 
        {
            if (modalTxtBoxJobId.Text.Trim() != "")
            {
                JobTracker jobTracker = new JobTracker();
                //jobTracker = jobTracker.GetCustomer(modalTxtBoxJobId.Text.Trim());
                jobTracker = jobTracker.GenerateHWAndSW(modalTxtBoxJobId.Text.Trim());
                if (jobTracker.Customer == null || jobTracker.Customer == "")
                {
                    modalLabelError.Text = "Job Id not found in CAP.";
                    modalLabelError.Visible = true;
                    modallabelBoxJobDescription.Text = "";
                    modallabelCustomer.Text = "";
                    modalLabelHWSW.Text = "";
                    modalLabelHWSW.ToolTip = "";
                    modalLabelEvalNo.Text = "";
                }
                else 
                {
                    modalLabelError.Text = "";
                    modalLabelError.Visible = false;
                    modallabelBoxJobDescription.Text = jobTracker.Description;
                    modallabelCustomer.Text =  jobTracker.Customer;
                    modalLabelHWSW.Text = jobTracker.HWNo == null ? "" : jobTracker.HWNo.Trim();
                    modalLabelHWSW.ToolTip = jobTracker.SWNo == null ? "" : jobTracker.SWNo.Trim();
                    modalLabelEvalNo.Text = jobTracker.EvalNo;
                }
            }
            else 
            {
                modallabelBoxJobDescription.Text = "";
                modallabelCustomer.Text = "";
                modalLabelHWSW.Text = "";
                modalLabelHWSW.ToolTip = "";
                modalLabelEvalNo.Text = "";
                modalLabelError.Text = "";
                modalLabelError.Visible = false;
            }
            this.programmaticModalPopup.Show();
        }
        #endregion

        #region INITIALIZED
        //protected void InitializeModal() 
        //{
        //    JobType jobtype = new JobType();
        //    var data = jobtype.GetJobTypeList(Convert.ToInt32(Session["DepartmentId"]));
        //    modalDropDownJobType.DataSource = data;
        //    modalDropDownJobType.DataTextField = "Description";
        //    modalDropDownJobType.DataValueField = "Id";
        //    modalDropDownJobType.DataBind();

        //    Time time = new Time();
        //    var starttime = time.GetStartTimeList();

        //    modalDropDownStartTime.DataSource = starttime;
        //    modalDropDownStartTime.DataTextField = "Description";
        //    modalDropDownStartTime.DataValueField = "C24hrConversion";
        //    modalDropDownStartTime.DataBind();

        //    bool hasSelected = false;
        //    foreach (ListItem i in modalDropDownStartTime.Items) 
        //    {
                
        //        TimeSpan selectedTime = TimeSpan.Parse(i.Value);
        //        TimeSpan curtime = TimeSpan.Parse(DateTime.Now.Hour + ":" + DateTime.Now.Minute);
        //        if (selectedTime >= curtime && hasSelected == false) 
        //        {
        //            i.Selected = true;
        //            hasSelected = true;
        //        }
        //        else
        //            i.Selected = false;
        //    }

        //    var endtime = time.GetEndTimeList(modalDropDownStartTime.SelectedValue);
        //    modalDropDownEndTime.DataSource = endtime;
        //    modalDropDownEndTime.DataTextField = "Description";
        //    modalDropDownEndTime.DataValueField = "C24hrConversion";
        //    modalDropDownEndTime.DataBind();
        //}

        private void InitializeModalJobType(string value = "") 
        {
            JobTypeDepartment jobtypeDepartment = new JobTypeDepartment();
            var data = jobtypeDepartment.GetJobTypeList(Convert.ToInt32(Session["DepartmentId"]));
            modalDropDownJobType.DataSource = data;
            modalDropDownJobType.DataTextField = "Description";
            modalDropDownJobType.DataValueField = "Id";
            modalDropDownJobType.DataBind();
            if (value.Trim() != "") 
            {
                foreach (ListItem i in modalDropDownJobType.Items) 
                {
                    if (i.Value.Trim() == value.Trim()) 
                    {
                        i.Selected = true;
                        
                        //if (jobType != null)
                        //{
                        //    //modalTxtBoxJobId.Enabled = Convert.ToBoolean(jobType.RequiredJobId);
                        //    modalReqJobId.Enabled = Convert.ToBoolean(jobType.RequiredJobId);
                        //}
                        //if (modalTxtBoxJobId.Enabled == false)
                        //{
                        //    modallabelCustomer.Text = "";
                        //    modallabelBoxJobDescription.Text = "";
                        //    modalTxtBoxJobId.Text = "";
                        //    modalLabelError.Visible = false;
                        //}
                        break;
                    }
                }
            }
            int jobtypeId = Convert.ToInt32(modalDropDownJobType.SelectedValue);
            JobType jobType = new JobType();
            jobType = jobType.GetJobType(jobtypeId);
            if (jobType != null)
            {
                modalReqJobId.Enabled = Convert.ToBoolean(jobType.RequiredJobId);
                modalReqJobStatus.Enabled = Convert.ToBoolean(jobType.ComputeTime); //If time is computed, require Job Status, otherwise do not require it.
                //modalDropDownJobStatus.Enabled = Convert.ToBoolean(jobType.RequiredJobId);
            }
        }

        //private void InitializeModalStartTime(string value = "") 
        //{
        //    //GenerateStartHour();
        //    //GenerateStartMin(Convert.ToInt32(modalDropDownStartTimeHour.SelectedItem.Value));
        //    Time time = new Time();
        //    var timelist = time.GetTimeList();
        //    DateTime date = Convert.ToDateTime(txtBoxDate.Text);
        //    List<Time> starttime = new List<Time>();
        //    TimeSpan curtime = TimeSpan.Parse(DateTime.Now.Hour + ":" + DateTime.Now.Minute);
        //    if (date.CompareTo(DateTime.Today) == 0)
        //    {
        //        foreach (Time t in timelist)
        //        {
        //            TimeSpan selectedTime = TimeSpan.Parse(t.C24hrConversion);
        //            if (selectedTime >= curtime)
        //            {
        //                time.Position = t.Position > 1 ? (t.Position-1) : t.Position;
        //                break;
        //            }
        //        }
        //        starttime = time.GetStartTimeList(time.Position);
        //    }
        //    else
        //    {
        //        starttime = time.GetStartTimeList();
        //    }

        //    //remove used time
        //    JobTracker jobtracker = new JobTracker();
        //    var joblist = jobtracker.GetJobTrackerList(Convert.ToInt32(Session["UserId"]), date);
        //    for (int i = 0; i < joblist.Count; i++)
        //    {
        //        if (value.Trim() != "") 
        //        {
        //            if (joblist[i].StartTime == DateTime.Parse(value))
        //                continue;
        //        }
        //        if (joblist[i].EndTime != null)
        //        {
        //            Time stime = time.GetTime(Convert.ToDateTime(joblist[i].StartTime));
        //            Time etime = time.GetTime(Convert.ToDateTime(joblist[i].EndTime));
        //            RemoveUsedStartTime(starttime, stime.Position, etime.Position);
        //        }
        //    }              
        //    //
        //    modalDropDownStartTime.DataSource = starttime;
        //    modalDropDownStartTime.DataTextField = "Description";
        //    modalDropDownStartTime.DataValueField = "C24hrConversion";
        //    modalDropDownStartTime.DataBind();
        //    if (value.Trim() == "")
        //    {
        //        bool hasSelected = false;
        //        int maxindex = 0;
        //        TimeSpan maxTime = new TimeSpan(23, 59, 0);
        //        for (int i = 0; i < modalDropDownStartTime.Items.Count;i++)
        //        {
        //            TimeSpan selectedTime = TimeSpan.Parse(modalDropDownStartTime.Items[i].Value);
        //            if (selectedTime < curtime && maxTime < selectedTime) 
        //            {
        //                maxindex = i;
        //                maxTime = selectedTime;
        //            }
        //            if (selectedTime >= curtime && hasSelected == false)
        //            {
        //                modalDropDownStartTime.Items[i].Selected = true;
        //                hasSelected = true;
        //            }
        //            else
        //                modalDropDownStartTime.Items[i].Selected = false;
        //        }
        //        if (hasSelected == false && modalDropDownStartTime.Items.Count > 0) 
        //        {
        //            if (maxindex > 0)
        //                modalDropDownStartTime.Items[maxindex].Selected = true;
        //            else
        //                modalDropDownStartTime.Items[modalDropDownStartTime.Items.Count - 1].Selected = true;
        //        }
        //    }
        //    else 
        //    {
        //        DateTime stime = DateTime.Parse(value);
        //        string selectedTime = stime.Hour + ":" + stime.Minute;
        //        foreach (ListItem i in modalDropDownStartTime.Items)
        //        {
        //            if (TimeSpan.Parse(i.Value.Trim()) == TimeSpan.Parse(selectedTime.Trim()))
        //            {
        //                i.Selected = true;
        //                break;
        //            }
        //        }
        //    }
        //}

        private void GenerateStartHour(string selectedTime = "") 
        {
            Session["Is2ndPartComplete"] = true;
            DateTime selectedDate = DateTime.Parse(txtBoxDate.Text); //Get selected Time
            bool isCurrentDate = false;
            int userid = Convert.ToInt32(Session["UserId"]);

            JobTracker jobtracker = new JobTracker();
            TimeSetting timeSetting = new TimeSetting();
            timeSetting = timeSetting.GetTimeSetting();
            TimeSpan startTime = new TimeSpan();
            List<TimeSpan> availableTime = new List<TimeSpan>();
            List<TimeSpan> timeMarker = new List<TimeSpan>();
            List<TimeSpan> cutOffMarker = new List<TimeSpan>();
            User user = new User();
            user = user.GetUser(userid, selectedDate);
            TimeSpan cutOffTime = user.GetMyCutOfTime();
            //generates the timelist
            if (Session["StartTime"] != null)
            {
                string[] s = Session["StartTime"].ToString().Split(':') ;
                startTime = new TimeSpan(Convert.ToInt32(s[0]), Convert.ToInt32(s[1]),0);
            }

            for (int i = 0; i < 24; i++) 
            {
                for (int j = 0; j < 60; j += timeSetting.Interval) 
                {
                    availableTime.Add(new TimeSpan(i, j, 0));
                    timeMarker.Add(new TimeSpan(i, j, 0));
                    if (cutOffTime <= new TimeSpan(i, j, 0)) 
                    {
                        cutOffMarker.Add(new TimeSpan(i,j,0));
                    }
                }
            }

            if (selectedDate.CompareTo(DateTime.Today) == 0)
            {
                isCurrentDate = true;
            }

            Dictionary<string, string> hour = new Dictionary<string, string>();
            var usedTime = jobtracker.GetJobTrackerList(userid, selectedDate,false);

            for (int i = 0; i < usedTime.Count; i++) 
            {
                for (int j = 0; j < availableTime.Count; j++)
                {
                    if (isCurrentDate == true && DateTime.Now.TimeOfDay < availableTime[j])
                    {
                        availableTime.RemoveAt(j);
                        --j;
                    }
                    else if (usedTime[i].EndTime != null)
                    {
                        TimeSpan stime = Convert.ToDateTime(usedTime[i].StartTime).TimeOfDay;
                        TimeSpan etime;
                        if (Convert.ToDateTime(usedTime[i].EndTime).Date > Convert.ToDateTime(usedTime[i].StartTime).Date) //Convert 00:00:00 next day to 1.00:00:00 Timespan
                        {
                            etime = new TimeSpan(24,0,0);
                        }
                        else 
                        {
                            etime = new TimeSpan(Convert.ToDateTime(usedTime[i].EndTime).Hour, Convert.ToDateTime(usedTime[i].EndTime).Minute, 0);
                        }
                        if (availableTime[j] >= stime && availableTime[j] < etime)
                        {
                            if (Session["StartTime"] != null && (startTime >= stime && startTime < etime))
                                continue;
                            else
                            {
                                availableTime.RemoveAt(j);
                                --j;
                            }
                        }
                    }
                } 
            }
            if (usedTime.Count < 1)
            {
                for (int j = 0; j < availableTime.Count; j++)
                {
                    if (isCurrentDate == true && DateTime.Now.TimeOfDay < availableTime[j])
                    {
                        availableTime.RemoveAt(j);
                        --j;
                    }
                }
            }

            if (TimeSpan.Parse(user.startTime) > TimeSpan.Parse(user.endTime)) //user with shifting hour 
            {
                
                TimeClock timeclock = new TimeClock();
                timeclock = timeclock.GetStartEndTimeForShifting(Convert.ToInt32(user.EmployeeNumber), Convert.ToDateTime(selectedDate.AddDays(-1).ToString("yyyy-MM-dd") + " " + cutOffTime.ToString("hh\\:mm\\:ss")), Convert.ToDateTime(selectedDate.ToString("yyyy-MM-dd") + " " + cutOffTime.ToString("hh\\:mm\\:ss")), false);
                bool notComplete = false;
                if (timeclock != null) 
                {
                    for (int i = 0; i < availableTime.Count; i++) 
                    {
                        if (availableTime[i] >= timeclock.starttime.TimeOfDay && availableTime[i] < timeclock.endtime.TimeOfDay) //Check if the 2nd part of the previous day is completed
                        {
                            notComplete = true;
                            Session["Is2ndPartComplete"] = false;
                            break;
                        }
                    }
                }
                if (notComplete)
                {
                    //make sure that there is no gap between time
                    for (int i = 0; i < availableTime.Count; i++)
                    {
                        if (availableTime[i] >= cutOffTime)
                        {
                            if (i < availableTime.Count - 1)
                            {
                                availableTime.RemoveRange(i, availableTime.Count - i);
                                break;
                            }
                        }
                    }
                    if (!isCurrentDate)
                    {
                        if (availableTime.Count > 0 && (availableTime[availableTime.Count - 1].Hours != availableTime[availableTime.Count - 2].Hours)) //remove last hour 
                            availableTime.RemoveAt(availableTime.Count - 1);
                    }

                    if (usedTime.Count > 0)
                    {
                        for (int i = 0; i < availableTime.Count; i++)
                        {
                            if (availableTime[i] != timeMarker[i])
                            {
                                if (i < availableTime.Count - 1)
                                {
                                    availableTime.RemoveRange(i + 1, availableTime.Count - i - 1);
                                    break;
                                }
                            }
                        }
                    }
                }
                else 
                {
                    if (!isCurrentDate)
                    {
                        if (availableTime.Count > 0 && (availableTime[availableTime.Count - 1].Hours != availableTime[availableTime.Count - 2].Hours)) //remove last hour 
                            availableTime.RemoveAt(availableTime.Count - 1);
                    }
                    if (usedTime.Count > 0)
                    {
                        int loop = 0;
                        for (int i = 0; i < availableTime.Count; i++)
                        {
                            if (availableTime[i] >= cutOffTime)
                            {
                                if (availableTime[i] != cutOffMarker[loop])
                                {
                                    if (i < availableTime.Count - 1)
                                    {
                                        availableTime.RemoveRange(i + 1, availableTime.Count - i - 1);
                                        break;
                                    }
                                }
                                loop++;
                            }
                        }
                    }
                }
            }
            else 
            {
                if (!isCurrentDate)
                {
                    if (availableTime.Count > 0 && (availableTime[availableTime.Count - 1].Hours != availableTime[availableTime.Count - 2].Hours)) //remove last hour 
                        availableTime.RemoveAt(availableTime.Count - 1);
                }

                //make sure that there is no gap between time
                if (usedTime.Count > 0)
                {
                    for (int i = 0; i < availableTime.Count; i++)
                    {
                        if (availableTime[i] != timeMarker[i])
                        {
                            if (i < availableTime.Count - 1)
                            {
                                availableTime.RemoveRange(i + 1, availableTime.Count - i - 1);
                                break;
                            }
                        }
                    }
                }
            }
            
            int curtime = 24;

            for (int i = 0; i < availableTime.Count; i++)
            {
                if (curtime != availableTime[i].Hours)
                {
                    curtime = availableTime[i].Hours;
                    hour.Add(curtime > 9 ? curtime.ToString() : "0" + curtime.ToString(), curtime > 9 ? curtime.ToString() : "0" + curtime.ToString());
                }
            }
            modalDropDownStartTimeHour.DataSource = hour;
            modalDropDownStartTimeHour.DataTextField = "Key";
            modalDropDownStartTimeHour.DataValueField = "Value";
            modalDropDownStartTimeHour.DataBind();

            if (selectedTime.Trim() != "") 
            {
                string[] s = selectedTime.Split(':');
                foreach (ListItem i in modalDropDownStartTimeHour.Items) 
                {
                    if (i.Text.Trim() == s[0].Trim())
                        i.Selected = true;
                }
            }
            else if (isCurrentDate == true && usedTime.Count < 1) //select the time closest to current time
            {
                int s = DateTime.Now.Hour;
                decimal gap = 1000;
                int index = 0;
                for (int i = 0; i < modalDropDownStartTimeHour.Items.Count; i++)
                {
                    decimal cgap = Math.Abs(Convert.ToInt32(modalDropDownStartTimeHour.Items[i].Value) - s);
                    if (cgap == 0)
                    {
                        gap = cgap;
                        index = i;
                        break;
                    }
                    else if (cgap < gap)
                    {
                        gap = cgap;
                        index = i;
                    }
                }
                modalDropDownStartTimeHour.Items[index].Selected = true;
            }
            else if (hour.Count < 1) 
            {
            }
            else
            {
                modalDropDownStartTimeHour.Items[modalDropDownStartTimeHour.Items.Count - 1].Selected = true;
            }
        }

        private void GenerateStartMin(int hour, string selectedTime = "") 
        {
            DateTime selectedDate = DateTime.Parse(txtBoxDate.Text);
            int userid = Convert.ToInt32(Session["UserId"]);
            bool isCurrentDate = false;

            JobTracker jobtracker = new JobTracker();
            TimeSetting timeSetting = new TimeSetting();
            timeSetting = timeSetting.GetTimeSetting();
            TimeSpan startTime = new TimeSpan();
            List<TimeSpan> availableTime = new List<TimeSpan>();
            List<String> deleteMarker = new List<string>();
            User user = new User();
            user = user.GetUser(userid, selectedDate);
            TimeSpan cutOffTime = user.GetMyCutOfTime();
            bool isComplete = Convert.ToBoolean(Session["Is2ndPartComplete"]);
            if (Session["StartTime"] != null)
            {
                string[] s = Session["StartTime"].ToString().Split(':');
                startTime = new TimeSpan(Convert.ToInt32(s[0]), Convert.ToInt32(s[1]), 0);
            }

            for (int j = 0; j < 60; j += timeSetting.Interval)
            {
                if (isComplete == false && hour == cutOffTime.Hours)
                {
                    if (new TimeSpan(hour, j, 0) < cutOffTime)
                    {
                        availableTime.Add(new TimeSpan(hour, j, 0));
                        deleteMarker.Add(availableTime.LastOrDefault().ToString());
                    }
                    else 
                    {
                        break;
                    }
                }
                else
                {
                    availableTime.Add(new TimeSpan(hour, j, 0));
                    deleteMarker.Add(availableTime.LastOrDefault().ToString());
                }
            }

            if (selectedDate.CompareTo(DateTime.Today) == 0)
            {
                isCurrentDate = true;
            }
            Dictionary<string, string> mins = new Dictionary<string, string>();
            var usedTime = jobtracker.GetJobTrackerList(userid, selectedDate,false);

            for (int i = 0; i < usedTime.Count; i++)
            {
                for (int j = 0; j < availableTime.Count; j++)
                {
                    if (isCurrentDate == true && DateTime.Now.TimeOfDay < availableTime[j])
                    {
                        deleteMarker[deleteMarker.LastIndexOf(availableTime[j].ToString())] = "deleted";
                        availableTime.RemoveAt(j);
                        --j;
                    }
                    else if (usedTime[i].EndTime != null)
                    {
                        TimeSpan stime = Convert.ToDateTime(usedTime[i].StartTime).TimeOfDay;
                        TimeSpan etime = new TimeSpan(Convert.ToDateTime(usedTime[i].EndTime).Hour, Convert.ToDateTime(usedTime[i].EndTime).Minute, 0);
                        if (availableTime[j] >= stime && availableTime[j] < etime)
                        {
                            if (Session["StartTime"] != null && (startTime >= stime && startTime < etime))
                                continue;
                            else
                            {
                                deleteMarker[deleteMarker.LastIndexOf(availableTime[j].ToString())] = "deleted";
                                availableTime.RemoveAt(j);
                                --j;
                            }
                        }
                    }
                }
            }
            
            
            if (usedTime.Count < 1)
            {
                for (int j = 0; j < availableTime.Count; j++)
                {
                    if (isCurrentDate == true && DateTime.Now.TimeOfDay < availableTime[j])
                    {
                        deleteMarker[deleteMarker.LastIndexOf(availableTime[j].ToString())] = "deleted";
                        availableTime.RemoveAt(j);
                        --j;
                    }
                }
            }

            //make sure that there is no gap between time
            if (selectedTime.Trim() == "")
            {
                if (Convert.ToInt32(modalDropDownStartTimeHour.Items[modalDropDownStartTimeHour.Items.Count - 1].Text) == hour && availableTime.Count > 1 && usedTime.Count > 0)
                {
                    TimeSpan etime = new TimeSpan(Convert.ToDateTime(usedTime[0].EndTime).Hour, Convert.ToDateTime(usedTime[0].EndTime).Minute, 0);
                    int index = -1;
                    int markerindex = -1;
                    bool deleteDetected = false;
                    for (int i = 0; i < deleteMarker.Count; i++) 
                    {
                        if (deleteDetected && deleteMarker[i] != "deleted") 
                        {
                            markerindex = i;
                            break;
                        }
                        if (deleteMarker[i] == "deleted")
                            deleteDetected = true;
                    }
                    if (markerindex > -1)
                    {
                        for (int i = 0; i < availableTime.Count; i++)
                        {
                            if (availableTime[i].ToString() == deleteMarker[markerindex])
                            {
                                index = i;
                                break;
                            }
                        }
                        if (index > -1 && (index + 1) < availableTime.Count)
                        {
                            availableTime.RemoveRange(index + 1, availableTime.Count - (index + 1));
                            //if (index > 0)
                            //    availableTime.RemoveRange(0, index);
                        }
                    }
                }
            }


            if (!isCurrentDate)
            {
                if (availableTime.Count > 0 && hour == Convert.ToInt32(modalDropDownStartTimeHour.Items[modalDropDownStartTimeHour.Items.Count - 1].Value))
                {

                    GenerateEndHour(availableTime[availableTime.Count - 1].Hours, availableTime[availableTime.Count - 1].Minutes, false);
                    if (modalDropDownEndTimeHour.Items.Count == 0)
                        availableTime.RemoveAt(availableTime.Count - 1);
                }
            }

            

            int curtime = 60;

            for (int i = 0; i < availableTime.Count; i++)
            {
                if (curtime != availableTime[i].Minutes)
                {
                    curtime = availableTime[i].Minutes;
                    mins.Add(curtime > 9 ? curtime.ToString() : "0" + curtime.ToString(), curtime > 9 ? curtime.ToString() : "0" + curtime.ToString());
                }
            }
            modalDropDownStartTimeMin.DataSource = mins;
            modalDropDownStartTimeMin.DataTextField = "Key";
            modalDropDownStartTimeMin.DataValueField = "Value";
            modalDropDownStartTimeMin.DataBind();

            if (selectedTime.Trim() != "")
            {
                string[] s = selectedTime.Split(':');
                foreach (ListItem i in modalDropDownStartTimeMin.Items)
                {
                    if (i.Text.Trim() == s[1].Trim())
                        i.Selected = true;
                }
            }
            else
            {
                TimeSpan s = DateTime.Now.TimeOfDay;
                double gap = 100000;
                int index = 0;
                for (int i = 0; i < modalDropDownStartTimeMin.Items.Count; i++)
                {

                    System.TimeSpan sp = new System.TimeSpan(Convert.ToInt32(modalDropDownStartTimeHour.SelectedItem.Value), Convert.ToInt32(modalDropDownStartTimeMin.Items[i].Value), 0);
                    double cgap = Math.Abs((sp.Subtract(s)).TotalMinutes);
                    if (cgap == 0)
                    {
                        gap = cgap;
                        index = i;
                        break;
                    }
                    else if (cgap < gap)
                    {
                        gap = cgap;
                        index = i;
                    }
                }
                modalDropDownStartTimeMin.Items[index].Selected = true;
            }  
        }

        private void GenerateEndHour(int shour, int smin,bool addBlank, string selectedTime = "") 
        {
            DateTime selectedDate = DateTime.Parse(txtBoxDate.Text);
            bool isCurrentDate = false;
            int userid = Convert.ToInt32(Session["UserId"]);

            JobTracker jobtracker = new JobTracker();
            TimeSetting timeSetting = new TimeSetting();
            timeSetting = timeSetting.GetTimeSetting();
            TimeSpan selTime = new TimeSpan(shour, smin, 0);
            User user = new User();
            user = user.GetUser(userid, selectedDate);
            TimeSpan cutOffTime = user.GetMyCutOfTime();

            List<TimeSpan> availableTime = new List<TimeSpan>();

            for (int i = shour; i < 25; i++)
            {
                if (i != 24)
                {
                    for (int j = 0; j < 60; j += timeSetting.Interval)
                    {
                        if (selTime < cutOffTime)
                        {
                            if (new TimeSpan(i, j, 0) <= cutOffTime)
                            {
                                availableTime.Add(new TimeSpan(i, j, 0));
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            availableTime.Add(new TimeSpan(i, j, 0));
                        }
                    }
                }
                else //adding 24:00
                {
                    if (selTime < cutOffTime)
                    {
                        if (new TimeSpan(i, 0, 0) <= cutOffTime)
                        {
                            availableTime.Add(new TimeSpan(i, 0, 0));
                        }
                    }
                    else 
                    {
                        availableTime.Add(new TimeSpan(i, 0, 0));
                    }
                }
            }

            if (selectedDate.CompareTo(DateTime.Today) == 0)
            {
                isCurrentDate = true;
            }

            Dictionary<string, string> hours = new Dictionary<string, string>();
            jobtracker = jobtracker.GetNextUsedTime(userid, DateTime.Parse(selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " " + selTime.ToString()), selectedDate);

            TimeSpan stime = jobtracker == null ? new TimeSpan(24,0,0) : Convert.ToDateTime(jobtracker.StartTime).TimeOfDay;
            if (Session["StartTime"] != null) 
            {
                TimeSpan ts = TimeSpan.Parse(Session["StartTime"].ToString());
                if (ts == stime)
                {
                    jobtracker = jobtracker.GetNextUsedTime(userid, DateTime.Parse(selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " " + Session["StartTime"].ToString()), selectedDate);
                    stime = jobtracker == null ? new TimeSpan(24, 0, 0) : Convert.ToDateTime(jobtracker.StartTime).TimeOfDay;
                }
            }

            for (int j = 0; j < availableTime.Count; j++)
            {
                if (availableTime[j] > stime)
                {
                    availableTime.RemoveAt(j);
                    --j;
                }
                else if (isCurrentDate == true && DateTime.Now.TimeOfDay < availableTime[j])
                {
                    availableTime.RemoveAt(j);
                    --j;
                }
                else if (selTime >= availableTime[j])
                {
                    availableTime.RemoveAt(j);
                    --j;
                }
            }

            int curtime = 24;
            if (addBlank == true)
            {
                hours.Add("--", "00");
            }

            for (int i = 0; i < availableTime.Count; i++)
            {
                if (curtime != Convert.ToInt32(Math.Floor(availableTime[i].TotalHours)))
                {
                    curtime = Convert.ToInt32(Math.Floor(availableTime[i].TotalHours));
                    hours.Add(curtime > 9 ? curtime.ToString() : "0" + curtime.ToString(), curtime > 9 ? curtime.ToString() : "0" + curtime.ToString());
                }
            }
            modalDropDownEndTimeHour.DataSource = hours;
            modalDropDownEndTimeHour.DataTextField = "Key";
            modalDropDownEndTimeHour.DataValueField = "Value";
            modalDropDownEndTimeHour.DataBind();

            if (selectedTime.Trim() != "")
            {
                string seletime = "";
                if (selectedTime.Length > 10)
                {
                    DateTime selEndDate = Convert.ToDateTime(selectedTime);
                    
                    if (selectedDate.Date < selEndDate.Date)
                        seletime = "24:00:00";
                    else
                        seletime = selEndDate.TimeOfDay.ToString();
                }
                else 
                {
                    seletime = selectedTime;
                }
                string[] s = seletime.Split(':');
                foreach (ListItem i in modalDropDownEndTimeHour.Items)
                {
                    if (i.Text.Trim() == s[0].Trim())
                        i.Selected = true;
                }
            }
        }

        private void GenerateEndMin(int shour, int smin,string ehour,string selectedTime = "")
        {
            Dictionary<string, string> mins = new Dictionary<string, string>();
            if (modalDropDownEndTimeHour.SelectedItem.Text.Trim() == "--")
            {
                mins.Add("--", "00");
            }
            if (ehour != "--")
            {
                DateTime selectedDate = DateTime.Parse(txtBoxDate.Text);
                int userid = Convert.ToInt32(Session["UserId"]);
                bool isCurrentDate = false;

                int hour = Convert.ToInt32(ehour);
                JobTracker jobtracker = new JobTracker();
                TimeSetting timeSetting = new TimeSetting();
                timeSetting = timeSetting.GetTimeSetting();
                TimeSpan selTime = new TimeSpan(shour, smin, 0);
                List<TimeSpan> availableTime = new List<TimeSpan>();
                User user = new User();
                user = user.GetUser(userid, selectedDate);
                TimeSpan cutOffTime = user.GetMyCutOfTime();

                if (hour != 24)
                {
                    for (int j = 0; j < 60; j += timeSetting.Interval)
                    {
                        if (selTime < cutOffTime)
                        {
                            if (new TimeSpan(hour, j, 0) <= cutOffTime)
                            {
                                availableTime.Add(new TimeSpan(hour, j, 0));
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            availableTime.Add(new TimeSpan(hour, j, 0));
                        }
                    }
                }
                else 
                {
                    if (selTime < cutOffTime)
                    {
                        if (new TimeSpan(hour, 0, 0) <= cutOffTime)
                        {
                            availableTime.Add(new TimeSpan(hour, 0, 0));
                        }
                    }
                    else
                    {
                        availableTime.Add(new TimeSpan(hour, 0, 0));
                    }
                }

                if (selectedDate.CompareTo(DateTime.Today) == 0)
                {
                    isCurrentDate = true;
                }

                jobtracker = jobtracker.GetNextUsedTime(userid, DateTime.Parse(selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " " + selTime.ToString()), selectedDate);
                TimeSpan stime = jobtracker == null ? new TimeSpan(23, 59, 59) : Convert.ToDateTime(jobtracker.StartTime).TimeOfDay;
                if (Session["StartTime"] != null)
                {
                    TimeSpan ts = TimeSpan.Parse(Session["StartTime"].ToString());
                    if (ts == stime)
                    {
                        jobtracker = jobtracker.GetNextUsedTime(userid, DateTime.Parse(selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " " + Session["StartTime"].ToString()), selectedDate);
                        stime = jobtracker == null ? new TimeSpan(23, 59, 59) : Convert.ToDateTime(jobtracker.StartTime).TimeOfDay;
                    }
                }

                for (int j = 0; j < availableTime.Count; j++)
                {
                    if (availableTime[j] > stime)
                    {
                        availableTime.RemoveAt(j);
                        --j;
                    }
                    else if (isCurrentDate == true && DateTime.Now.TimeOfDay < availableTime[j])
                    {
                        availableTime.RemoveAt(j);
                        --j;
                    }
                    else if (selTime >= availableTime[j])
                    {
                        availableTime.RemoveAt(j);
                        --j;
                    }
                }

                int curtime = 60;
                
                for (int i = 0; i < availableTime.Count; i++)
                {
                    if (curtime != availableTime[i].Minutes)
                    {
                        curtime = availableTime[i].Minutes;
                        mins.Add(curtime > 9 ? curtime.ToString() : "0" + curtime.ToString(), curtime > 9 ? curtime.ToString() : "0" + curtime.ToString());
                    }
                }
            }

            modalDropDownEndTimeMin.DataSource = mins;
            modalDropDownEndTimeMin.DataTextField = "Key";
            modalDropDownEndTimeMin.DataValueField = "Value";
            modalDropDownEndTimeMin.DataBind();

            if (selectedTime.Trim() != "")
            {
                string[] s = selectedTime.Split(':');
                foreach (ListItem i in modalDropDownEndTimeMin.Items)
                {
                    if (i.Text.Trim() == s[1].Trim())
                        i.Selected = true;
                }
            }
        }

        private void InitializeModalJobStatus(string value = "") 
        {
            if (value == null)
                value = "";
            foreach (ListItem i in modalDropDownJobStatus.Items) 
            {
                if (i.Value.Trim().Equals(value.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    i.Selected = true;
                }
                else
                    i.Selected = false;
            }
        }

        private void RunBreakButtonAction() 
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            JobType jobtype = new JobType();
            jobtype = jobtype.GetDefaultBreak();
            DateTime selectedDate = DateTime.Parse(txtBoxDate.Text);
            JobTracker jobtracker = new JobTracker();
            int jobtrackid = 0;
            bool noError = true;

            if (BtnBreak.Text == "Take a break")
            {
                DateTime startTime = new DateTime();
                if (gridJobTrack.Rows.Count > 0)
                {
                    jobtrackid = Convert.ToInt32(((Label)gridJobTrack.Rows[gridJobTrack.Rows.Count - 1].FindControl("labelJobTrackId")).Text);
                    jobtracker = jobtracker.GetJobTracker(jobtrackid, false);
                    JobType jtype = jobtype.GetJobType(Convert.ToInt32(jobtracker.JobTypeId));
                    if (jobtracker.EndTime == null)
                    {
                        DateTime stime = Convert.ToDateTime(jobtracker.StartTime);
                        GenerateEndHour(stime.Hour, stime.Minute, true);
                        GenerateEndMin(stime.Hour, stime.Minute, "--");
                        modalDropDownEndTimeHour.SelectedIndex = (modalDropDownEndTimeHour.Items.Count - 1);
                        GenerateEndMin(stime.Hour, stime.Minute, modalDropDownEndTimeHour.SelectedItem.Text.Trim(), modalDropDownEndTimeMin.Items.Count == 0 ? "" : modalDropDownEndTimeHour.Items[modalDropDownEndTimeHour.Items.Count - 1].Text.Trim() + ":" + modalDropDownEndTimeMin.SelectedItem.Text.Trim());
                        if (modalDropDownEndTimeHour.Items[modalDropDownEndTimeHour.Items.Count - 1].Text != "--")
                        {
                            if (modalDropDownEndTimeHour.SelectedValue == "24") //end date selected is 12:00 AM next day
                            {
                                jobtracker.EndTime = DateTime.Parse((selectedDate.AddDays(1)).Year + "-" + (selectedDate.AddDays(1)).Month + "-" + (selectedDate.AddDays(1)).Day + " 00:00:00");
                            }
                            else
                            {
                                jobtracker.EndTime = DateTime.Parse(selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " " + modalDropDownEndTimeHour.Items[modalDropDownEndTimeHour.Items.Count - 1].Value + ":" + modalDropDownEndTimeMin.Items[modalDropDownEndTimeMin.Items.Count - 1].Value + ":00");
                            }
                            jobtracker.Status = "Approved";
                            jobtracker.ApprovedBy = userid;
                            jobtracker.ActionRequest = "Update";
                            jobtracker.Update(jobtracker);
                            startTime = Convert.ToDateTime(jobtracker.EndTime);
                            InitializeGrid();
                        }
                        else
                        {
                            noError = false;
                            TimeSetting tsetting = new TimeSetting();
                            tsetting = tsetting.GetTimeSetting();
                            panelAlertHeader2.CssClass = "modalAlertHeader";
                            alertModalBtnOK2.CssClass = "buttonalert";
                            labelAlertHeader2.Text = "Error";
                            labelAlertMessage2.Text = "Cannot take a break without spending at least " + tsetting + " minutes on the last task.";
                        }
                    }
                    else
                    {
                        startTime = Convert.ToDateTime(jobtracker.EndTime);
                    }
                }
                else
                {
                    GenerateStartHour();
                    GenerateStartMin(Convert.ToInt32(modalDropDownStartTimeHour.SelectedItem.Value));
                    startTime = DateTime.Parse(selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " " + modalDropDownStartTimeHour.SelectedValue + ":" + modalDropDownStartTimeMin.SelectedValue + ":00");
                }
                if (noError)
                {
                    jobtracker = new JobTracker();
                    jobtracker.StartTime = startTime;
                    jobtracker.Status = "Pending";
                    jobtracker.JobStatus = "";
                    jobtracker.ApprovedBy = userid;
                    jobtracker.ActionRequest = "Add";
                    jobtracker.UserId = userid;
                    jobtracker.CreateDate = DateTime.Now;
                    jobtracker.CreatedBy = userid;
                    jobtracker.LastUpdateDate = DateTime.Now;
                    jobtracker.LastUpdatedBy = userid;
                    jobtracker.Description = jobtype.Description;
                    jobtracker.JobTypeId = jobtype.Id;
                    jobtracker.ScheduleDate = selectedDate;
                    jobtracker.Remarks = "";
                    jobtracker.Insert(jobtracker);
                    InitializeGrid();
                }
                else
                {
                    programmaticAlertModalPopup2.Show();
                }
            }
            else if (BtnBreak.Text == "End break")
            {
                jobtrackid = Convert.ToInt32(((Label)gridJobTrack.Rows[gridJobTrack.Rows.Count - 1].FindControl("labelJobTrackId")).Text);
                jobtracker = jobtracker.GetJobTracker(jobtrackid, false);
                jobtracker.LastUpdateDate = DateTime.Now;
                jobtracker.LastUpdatedBy = userid;
                if (jobtracker.JobTypeId == jobtype.Id)
                {
                    DateTime stime = Convert.ToDateTime(jobtracker.StartTime);
                    GenerateEndHour(stime.Hour, stime.Minute, true);
                    GenerateEndMin(stime.Hour, stime.Minute, "--");
                    modalDropDownEndTimeHour.SelectedIndex = (modalDropDownEndTimeHour.Items.Count - 1);
                    GenerateEndMin(stime.Hour, stime.Minute, modalDropDownEndTimeHour.SelectedItem.Text.Trim(), modalDropDownEndTimeMin.Items.Count == 0 ? "" : modalDropDownEndTimeHour.Items[modalDropDownEndTimeHour.Items.Count - 1].Text.Trim() + ":" + modalDropDownEndTimeMin.SelectedItem.Text.Trim());
                    if (modalDropDownEndTimeHour.Items[modalDropDownEndTimeHour.Items.Count - 1].Text != "--")
                    {
                        if (modalDropDownEndTimeHour.SelectedValue == "24") //end date selected is 12:00 AM next day
                        {
                            jobtracker.EndTime = DateTime.Parse((selectedDate.AddDays(1)).Year + "-" + (selectedDate.AddDays(1)).Month + "-" + (selectedDate.AddDays(1)).Day + " 00:00:00");
                        }
                        else
                        {
                            jobtracker.EndTime = DateTime.Parse(selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " " + modalDropDownEndTimeHour.Items[modalDropDownEndTimeHour.Items.Count - 1].Value + ":" + modalDropDownEndTimeMin.Items[modalDropDownEndTimeMin.Items.Count - 1].Value + ":00");
                        }
                        jobtracker.Status = "Approved";
                        jobtracker.ApprovedBy = userid;
                        jobtracker.ActionRequest = "Update";
                        jobtracker.Update(jobtracker);
                        InitializeGrid();
                    }
                    else
                    {
                        jobtracker.Delete(jobtracker.Id);
                        InitializeGrid();
                    }
                }
                if (gridJobTrack.Rows.Count > 1)
                {
                    for (int i = gridJobTrack.Rows.Count - 1; i >= 0; i--)
                    {
                        jobtrackid = Convert.ToInt32(((Label)gridJobTrack.Rows[i].FindControl("labelJobTrackId")).Text);
                        JobTracker jtracker = jobtracker.GetJobTracker(jobtrackid, false);
                        JobType jtype = jobtype.GetJobType(Convert.ToInt32(jtracker.JobTypeId));
                        if (jtype.ComputeTime == true)
                        {
                            jtracker.StartTime = jobtracker.EndTime;
                            jtracker.UserId = userid;
                            jtracker.CreateDate = DateTime.Now;
                            jtracker.CreatedBy = userid;
                            jtracker.LastUpdateDate = DateTime.Now;
                            jtracker.LastUpdatedBy = userid;
                            jtracker.EndTime = null;
                            jtracker.ActionRequest = "Add";
                            jtracker.Status = "Approved";
                            jtracker.Insert(jtracker);
                            InitializeGrid();
                            break;
                        }
                    }
                }
            }
        }

        private void RunBreakButtonActionForShifting()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            JobType jobtype = new JobType();
            jobtype = jobtype.GetDefaultBreak();
            DateTime selectedDate = DateTime.Parse(txtBoxDate.Text);
            JobTracker jobtracker = new JobTracker();
            int jobtrackid = 0;
            bool noError = true;
            User user = new User();
            user = user.GetUser(userid, selectedDate);
            TimeSpan cutOffTime = user.GetMyCutOfTime();

            if (BtnBreak.Text == "Take a break")
            {
                DateTime startTime = new DateTime();
                if (gridJobTrack.Rows.Count > 0)
                {
                    jobtrackid = Convert.ToInt32(((Label)gridJobTrack.Rows[gridJobTrack.Rows.Count - 1].FindControl("labelJobTrackId")).Text);
                    jobtracker = jobtracker.GetJobTracker(jobtrackid, false);
                    JobType jtype = jobtype.GetJobType(Convert.ToInt32(jobtracker.JobTypeId));
                    if (Convert.ToDateTime(jobtracker.StartTime).TimeOfDay < cutOffTime && DateTime.Now.TimeOfDay >= cutOffTime)
                    {
                        if (jobtracker.EndTime == null)
                        {
                            noError = false;
                            TimeSetting tsetting = new TimeSetting();
                            tsetting = tsetting.GetTimeSetting();
                            panelAlertHeader2.CssClass = "modalAlertHeader";
                            alertModalBtnOK2.CssClass = "buttonalert";
                            labelAlertHeader2.Text = "Error";
                            labelAlertMessage2.Text = "Please close all task first.";
                        }
                        else 
                        {
                            GenerateStartHour();
                            GenerateStartMin(Convert.ToInt32(modalDropDownStartTimeHour.SelectedItem.Value));
                            startTime = DateTime.Parse(selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " " + modalDropDownStartTimeHour.SelectedValue + ":" + modalDropDownStartTimeMin.SelectedValue + ":00");
                        }
                    }
                    else
                    {
                        if (jobtracker.EndTime == null)
                        {
                            DateTime stime = Convert.ToDateTime(jobtracker.StartTime);
                            GenerateEndHour(stime.Hour, stime.Minute, true);
                            GenerateEndMin(stime.Hour, stime.Minute, "--");
                            modalDropDownEndTimeHour.SelectedIndex = (modalDropDownEndTimeHour.Items.Count - 1);
                            GenerateEndMin(stime.Hour, stime.Minute, modalDropDownEndTimeHour.SelectedItem.Text.Trim(), modalDropDownEndTimeMin.Items.Count == 0 ? "" : modalDropDownEndTimeHour.Items[modalDropDownEndTimeHour.Items.Count - 1].Text.Trim() + ":" + modalDropDownEndTimeMin.SelectedItem.Text.Trim());
                            if (modalDropDownEndTimeHour.Items[modalDropDownEndTimeHour.Items.Count - 1].Text != "--")
                            {
                                if (modalDropDownEndTimeHour.SelectedValue == "24") //end date selected is 12:00 AM next day
                                {
                                    jobtracker.EndTime = DateTime.Parse((selectedDate.AddDays(1)).Year + "-" + (selectedDate.AddDays(1)).Month + "-" + (selectedDate.AddDays(1)).Day + " 00:00:00");
                                }
                                else
                                {
                                    jobtracker.EndTime = DateTime.Parse(selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " " + modalDropDownEndTimeHour.Items[modalDropDownEndTimeHour.Items.Count - 1].Value + ":" + modalDropDownEndTimeMin.Items[modalDropDownEndTimeMin.Items.Count - 1].Value + ":00");
                                }
                                jobtracker.Status = "Approved";
                                jobtracker.ApprovedBy = userid;
                                jobtracker.ActionRequest = "Update";
                                jobtracker.Update(jobtracker);
                                startTime = Convert.ToDateTime(jobtracker.EndTime);
                                InitializeGrid();
                            }
                            else
                            {
                                noError = false;
                                TimeSetting tsetting = new TimeSetting();
                                tsetting = tsetting.GetTimeSetting();
                                panelAlertHeader2.CssClass = "modalAlertHeader";
                                alertModalBtnOK2.CssClass = "buttonalert";
                                labelAlertHeader2.Text = "Error";
                                labelAlertMessage2.Text = "Cannot take a break without spending at least " + tsetting + " minutes on the last task.";
                            }
                        }
                        else
                        {
                            startTime = Convert.ToDateTime(jobtracker.EndTime);
                        }
                    }
                }
                else
                {
                    GenerateStartHour();
                    GenerateStartMin(Convert.ToInt32(modalDropDownStartTimeHour.SelectedItem.Value));
                    startTime = DateTime.Parse(selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " " + modalDropDownStartTimeHour.SelectedValue + ":" + modalDropDownStartTimeMin.SelectedValue + ":00");
                }
                if (noError)
                {
                    jobtracker = new JobTracker();
                    jobtracker.StartTime = startTime;
                    jobtracker.Status = "Pending";
                    jobtracker.JobStatus = "";
                    jobtracker.ApprovedBy = userid;
                    jobtracker.ActionRequest = "Add";
                    jobtracker.UserId = userid;
                    jobtracker.CreateDate = DateTime.Now;
                    jobtracker.CreatedBy = userid;
                    jobtracker.LastUpdateDate = DateTime.Now;
                    jobtracker.LastUpdatedBy = userid;
                    jobtracker.Description = jobtype.Description;
                    jobtracker.JobTypeId = jobtype.Id;
                    jobtracker.ScheduleDate = selectedDate;
                    jobtracker.Remarks = "";
                    jobtracker.Insert(jobtracker);
                    InitializeGrid();
                }
                else
                {
                    programmaticAlertModalPopup2.Show();
                }
            }
            else if (BtnBreak.Text == "End break")
            {
                jobtrackid = Convert.ToInt32(((Label)gridJobTrack.Rows[gridJobTrack.Rows.Count - 1].FindControl("labelJobTrackId")).Text);
                jobtracker = jobtracker.GetJobTracker(jobtrackid, false);
                jobtracker.LastUpdateDate = DateTime.Now;
                jobtracker.LastUpdatedBy = userid;
                if (jobtracker.JobTypeId == jobtype.Id)
                {
                    DateTime stime = Convert.ToDateTime(jobtracker.StartTime);
                    GenerateEndHour(stime.Hour, stime.Minute, true);
                    GenerateEndMin(stime.Hour, stime.Minute, "--");
                    modalDropDownEndTimeHour.SelectedIndex = (modalDropDownEndTimeHour.Items.Count - 1);
                    GenerateEndMin(stime.Hour, stime.Minute, modalDropDownEndTimeHour.SelectedItem.Text.Trim(), modalDropDownEndTimeMin.Items.Count == 0 ? "" : modalDropDownEndTimeHour.Items[modalDropDownEndTimeHour.Items.Count - 1].Text.Trim() + ":" + modalDropDownEndTimeMin.SelectedItem.Text.Trim());
                    if (modalDropDownEndTimeHour.Items[modalDropDownEndTimeHour.Items.Count - 1].Text != "--")
                    {
                        if (modalDropDownEndTimeHour.SelectedValue == "24") //end date selected is 12:00 AM next day
                        {
                            jobtracker.EndTime = DateTime.Parse((selectedDate.AddDays(1)).Year + "-" + (selectedDate.AddDays(1)).Month + "-" + (selectedDate.AddDays(1)).Day + " 00:00:00");
                        }
                        else
                        {
                            jobtracker.EndTime = DateTime.Parse(selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " " + modalDropDownEndTimeHour.Items[modalDropDownEndTimeHour.Items.Count - 1].Value + ":" + modalDropDownEndTimeMin.Items[modalDropDownEndTimeMin.Items.Count - 1].Value + ":00");
                        }
                        jobtracker.Status = "Approved";
                        jobtracker.ApprovedBy = userid;
                        jobtracker.ActionRequest = "Update";
                        jobtracker.Update(jobtracker);
                        InitializeGrid();
                    }
                    else//delete break if start time and end time are the same
                    {
                        jobtracker.Delete(jobtracker.Id);
                        InitializeGrid();
                    }
                }
                if (gridJobTrack.Rows.Count > 1) // continue the last created job
                {
                    for (int i = gridJobTrack.Rows.Count - 1; i >= 0; i--)
                    {
                        jobtrackid = Convert.ToInt32(((Label)gridJobTrack.Rows[i].FindControl("labelJobTrackId")).Text);
                        JobTracker jtracker = jobtracker.GetJobTracker(jobtrackid, false);
                        JobType jtype = jobtype.GetJobType(Convert.ToInt32(jtracker.JobTypeId));
                        if (jtype.ComputeTime == true)
                        {
                            jtracker.StartTime = jobtracker.EndTime;
                            jtracker.UserId = userid;
                            jtracker.CreateDate = DateTime.Now;
                            jtracker.CreatedBy = userid;
                            jtracker.LastUpdateDate = DateTime.Now;
                            jtracker.LastUpdatedBy = userid;
                            jtracker.EndTime = null;
                            jtracker.ActionRequest = "Add";
                            jtracker.Status = "Approved";
                            if(Convert.ToDateTime(jobtracker.EndTime).TimeOfDay != new TimeSpan(0,0,0)) //only insert if endtime is not equal to 24:00 (next day)
                                jtracker.Insert(jtracker);
                            InitializeGrid();
                            break;
                        }
                    }
                }
            }
        }

        #endregion
        #endregion

        #region OTHERS

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            foreach (GridViewRow row in gridJobTrack.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    row.Attributes["onmouseover"] = "this.style.cursor = 'pointer';this.style.backgroundColor = '#e5f2fc';";
                    row.Attributes["onmouseout"] = "this.style.backgroundColor='#ffffff';";
                    row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gridJobTrack, "Select$" + row.DataItemIndex, true);
                }
            }
            base.Render(writer);
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