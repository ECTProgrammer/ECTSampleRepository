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
            if (!isValidUser())
                Response.Redirect("Login.aspx");
            HttpContext.Current.Session["siteSubHeader"] = "JobTrack";
            HttpContext.Current.Session["selectedTab"] = "JobTrack";

            if (!IsPostBack)
            {
                string script = "<script type=\"text/javascript\"> //<![CDATA[Sys.Application.add_load(function(){setTimeout(function(){ var popup = $find(\"<%=programmaticAlertModalPopup.ClientID %>\"); popup._backgroundElement.style.zIndex = 10010; popup._foregroundElement.style.zIndex = 10011;},0);}0;//]]></script>";
                ClientScript.RegisterStartupScript(Page.GetType(), "OnLoad", script, false);
                txtBoxDate.Attributes.Add("readonly", "readonly");
                calendarExtenderDate.SelectedDate = DateTime.Now;
                calendarExtenderDate.EndDate = DateTime.Now;
                txtBoxDate.Text = DateTime.Now.ToString("dd MMM yyyy");
                labelDay.Text = DateTime.Now.DayOfWeek.ToString()+", "+DateTime.Today.ToString("dd MMM yyyy");

                InitializeWorkingHours();
                InitializeGrid();
            }
        }

        #region COMMAND
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
            InitializeWorkingHours();
            InitializeGrid();
        }

        protected void linkBtnAddJobTrack_Click(object sender, EventArgs e) 
        {
            JobTracker jobtracker = new JobTracker();
            DateTime selectedDate = DateTime.Parse(txtBoxDate.Text);

            if (jobtracker.HasUnclosedJobs(Convert.ToInt32(Session["UserId"]), selectedDate))
            {
                panelAlertHeader2.CssClass = "modalAlertHeader";
                alertModalBtnOK2.CssClass = "buttonalert";
                labelAlertHeader2.Text = "Error";
                labelAlertMessage2.Text = "Please close all jobs before adding a new one.";
                programmaticAlertModalPopup2.Show();
            }
            else
            {
                InitializeModalJobType();
                InitializeModalStartTime();
                InitializeModalEndTime();

                modalBtnSubmit.CommandArgument = "Add";
                if (selectedDate.CompareTo(DateTime.Today) < 0)
                {
                    InitializeModalSupervisor();
                    modalLabelSupervisor.Visible = true;
                    modalLabelSupColon.Visible = true;
                    modalDropDownSupervisor.Visible = true;
                }
                else
                {
                    modalLabelSupervisor.Visible = false;
                    modalLabelSupColon.Visible = false;
                    modalDropDownSupervisor.Visible = false;
                }
                modalBtnDelete.Visible = false;
                modalTxtBoxRemarks.Text = "";
                modalLabelError.Text = "";
                modalLabelError.Visible = false;
                modallabelBoxJobDescription.Text = "";
                modallabelCustomer.Text = "";
                modalTxtBoxJobId.Text = "";
                modalTxtBoxJobId.Enabled = false;
                Page.Validate();

                this.programmaticModalPopup.Show();
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
                JobTracker jobtracker = new JobTracker();
                List<JobTracker> datalist = new List<JobTracker>();
                datalist = jobtracker.GetJobTrackerList(userid,date);

                modalLabelError.Text = "";
                modalLabelError.Visible = false;
                modalBtnSubmit.CommandArgument = datalist[i].Id.ToString();
                modalBtnDelete.CommandArgument = datalist[i].Id.ToString();
                InitializeModalJobType(datalist[i].JobTypeId.ToString());
                InitializeModalStartTime(datalist[i].StartTime.ToString());
                string endtime = "";
                if (datalist[i].EndTime != null)
                    endtime = datalist[i].EndTime.ToString();
                InitializeModalEndTime(endtime);
                if (datalist[i].ApprovedBy != null && datalist[i].ApprovedBy != userid && datalist[i].ApprovedBy != 0) 
                {
                    InitializeModalSupervisor(datalist[i].ApprovedBy.ToString());
                    modalLabelSupervisor.Visible = true;
                    modalLabelSupColon.Visible = true;
                    modalDropDownSupervisor.Visible = true;
                }
                if (datalist[i].JobIdNumber != null && datalist[i].JobIdNumber.Trim() != "") 
                {
                    modalTxtBoxJobId.Text = datalist[i].JobIdNumber;
                    JobTracker jobTracker = new JobTracker();
                    jobTracker = jobTracker.GetCustomer(modalTxtBoxJobId.Text.Trim());
                    modallabelBoxJobDescription.Text = jobTracker.pcbdesc;
                    modallabelCustomer.Text = jobTracker.customer;
                }

                modalTxtBoxRemarks.Text = datalist[i].Remarks.Trim();
                
                Page.Validate();
                this.programmaticModalPopup.Show();
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
            data = jobTracker.GetJobTrackerList(userid,date);
            Converter model = new Converter();

            //DataTable table = model.ConvertToDataTable(data);

            gridJobTrack.DataSource = data;
            gridJobTrack.DataBind();
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
        #endregion

        #region MODAL
        #region COMMAND
        protected void modalBtnSubmit_Command(object sender, CommandEventArgs e) 
        {
            bool haserror = false;
            string errormessage = "";
            if (modalTxtBoxJobId.Enabled == true)
            {
                
                if (modalTxtBoxJobId.Text.Trim() == "") 
                {
                    panelAlertHeader.CssClass = "modalAlertHeader";
                    alertModalBtnOK.CssClass = "buttonalert";
                    modalLabelError.Text = "Job Id is required for this job.";
                    modalLabelError.Visible = true;
                }
                if (modalLabelError.Visible == true)
                {
                    errormessage = modalLabelError.Text;
                    haserror = true;
                }
            }
            
            if (haserror) 
            {
                //System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='JavaScript'>alert('Error: "+errormessage+"')</SCRIPT>");
                labelAlertHeader.Text = "Error";
                labelAlertMessage.Text = errormessage;
                this.programmaticModalPopup.Show();
                programmaticAlertModalPopup.Show();

            }
            else
            {
                DateTime selectedDate = DateTime.Parse(txtBoxDate.Text);
                int userid = Convert.ToInt32(Session["UserId"]);
                JobTracker jobTracker = new JobTracker();
                DateTime startTime = DateTime.Parse(selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " " + modalDropDownStartTime.SelectedValue + ":00");
                DateTime endTime = DateTime.Parse(selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " " + modalDropDownEndTime.SelectedValue + ":00");
                jobTracker.StartTime = startTime;
                jobTracker.ScheduleDate = selectedDate;
                if (modalDropDownEndTime.SelectedItem.Text != "Select End Time")
                {
                    jobTracker.EndTime = endTime;
                }
                jobTracker.JobTypeId = Convert.ToInt32(modalDropDownJobType.SelectedValue);
                jobTracker.JobIdNumber = modalTxtBoxJobId.Text.Trim();
                jobTracker.Remarks = modalTxtBoxRemarks.Text.Trim();
                jobTracker.LastUpdateDate = DateTime.Now;
                jobTracker.LastUpdatedBy = userid;
                jobTracker.UserId = userid;
                if (selectedDate.CompareTo(DateTime.Today) == 0)
                    if (modalDropDownEndTime.SelectedItem.Text != "Select End Time")
                        jobTracker.Status = "Approved";
                    else
                        jobTracker.Status = "Pending";
                else
                    jobTracker.Status = "For Approval";
                if (modalDropDownSupervisor.Visible)
                {
                    jobTracker.ApprovedBy = Convert.ToInt32(modalDropDownSupervisor.SelectedItem.Value);
                }
                else 
                {
                    jobTracker.ApprovedBy = userid;
                }
                if (e.CommandArgument.ToString() == "Add")
                {

                    jobTracker.CreateDate = DateTime.Now;
                    jobTracker.SupervisorRemarks = "";
                    jobTracker.CreatedBy = userid;
                    jobTracker.ActionRequest = "Add";
                    jobTracker.Insert(jobTracker);
                }
                else 
                {
                    jobTracker.Id = Convert.ToInt32(e.CommandArgument);
                    jobTracker.ActionRequest = "Update";
                    jobTracker.Update(jobTracker);
                }
                InitializeWorkingHours();
                InitializeGrid();
            }
            
        }

        protected void modalBtnDelete_Command(object sender, CommandEventArgs e) 
        {
            DateTime selectedDate = DateTime.Parse(txtBoxDate.Text);
            int userid = Convert.ToInt32(Session["UserId"]);
            JobTracker jobTracker = new JobTracker();
            jobTracker.Id = Convert.ToInt32(e.CommandArgument);
            jobTracker = jobTracker.GetJobTracker(jobTracker.Id);
            if (selectedDate.CompareTo(DateTime.Today) == 0 || jobTracker.Status == "Rejected")
            {
                jobTracker.Delete(jobTracker.Id);
            }
            else 
            {
                jobTracker.ActionRequest = "Delete";
                jobTracker.Status = "For Approval";
                jobTracker.LastUpdateDate = DateTime.Now;
                jobTracker.LastUpdatedBy = userid;
                jobTracker.Update(jobTracker);
            }
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
                modalTxtBoxJobId.Enabled = Convert.ToBoolean(jobType.RequiredJobId);
            }
            if (modalTxtBoxJobId.Enabled == false) 
            {
                modallabelCustomer.Text = "";
                modallabelBoxJobDescription.Text = "";
                modalTxtBoxJobId.Text = "";
                modalLabelError.Visible = false;
            }
            this.programmaticModalPopup.Show();

        }

        protected void modalDropDownStartTime_IndexChanged(object sender, EventArgs e) 
        {
            Time time = new Time();
            Time stime = time.GetTime(modalDropDownStartTime.SelectedValue);
            Time etime = time.GetTime(modalDropDownEndTime.SelectedItem.Value);

            TimeSpan selectedTime = TimeSpan.Parse(modalDropDownStartTime.SelectedValue);
            TimeSpan curtime = TimeSpan.Parse(DateTime.Now.Hour + ":" + DateTime.Now.Minute);

            InitializeModalEndTime(modalDropDownStartTime.SelectedValue);
            //var endtime = time.GetEndTimeList(modalDropDownStartTime.SelectedValue);
            //modalDropDownEndTime.DataSource = endtime;
            //modalDropDownEndTime.DataTextField = "Description";
            //modalDropDownEndTime.DataValueField = "C24hrConversion";
            //modalDropDownEndTime.DataBind();

            if (etime != null)
            {
                if (etime.Position > stime.Position)
                {
                    foreach (ListItem i in modalDropDownEndTime.Items)
                    {
                        if (i.Value == etime.C24hrConversion)
                        {
                            i.Selected = true;
                            break;
                        }
                    }
                }
                else
                {
                    modalDropDownEndTime.Items[0].Selected = true;
                }
            }
            DateTime date = Convert.ToDateTime(txtBoxDate.Text);
            if (date.CompareTo(DateTime.Today) == 0)
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

        protected void modalTxtBoxJobId_TextChanged(object sender, EventArgs e) 
        {
            if (modalTxtBoxJobId.Text.Trim() != "")
            {
                JobTracker jobTracker = new JobTracker();
                jobTracker = jobTracker.GetCustomer(modalTxtBoxJobId.Text.Trim());
                if (jobTracker.customer == null || jobTracker.customer == "")
                {
                    modalLabelError.Text = "Job Id not found in CAP.";
                    modalLabelError.Visible = true;
                    modallabelBoxJobDescription.Text = "";
                    modallabelCustomer.Text = "";
                }
                else 
                {
                    modalLabelError.Text = "";
                    modalLabelError.Visible = false;
                    modallabelBoxJobDescription.Text = jobTracker.pcbdesc;
                    modallabelCustomer.Text =  jobTracker.customer;
                }
            }
            else 
            {
                modallabelBoxJobDescription.Text = "";
                modallabelCustomer.Text = "";
            }
            this.programmaticModalPopup.Show();
        }

        #endregion

        #region INITIALIZED
        protected void InitializeModal() 
        {
            JobType jobtype = new JobType();
            var data = jobtype.GetJobTypeList(Convert.ToInt32(Session["DepartmentId"]));
            modalDropDownJobType.DataSource = data;
            modalDropDownJobType.DataTextField = "Description";
            modalDropDownJobType.DataValueField = "Id";
            modalDropDownJobType.DataBind();

            Time time = new Time();
            var starttime = time.GetStartTimeList();

            modalDropDownStartTime.DataSource = starttime;
            modalDropDownStartTime.DataTextField = "Description";
            modalDropDownStartTime.DataValueField = "C24hrConversion";
            modalDropDownStartTime.DataBind();

            bool hasSelected = false;
            foreach (ListItem i in modalDropDownStartTime.Items) 
            {
                
                TimeSpan selectedTime = TimeSpan.Parse(i.Value);
                TimeSpan curtime = TimeSpan.Parse(DateTime.Now.Hour + ":" + DateTime.Now.Minute);
                if (selectedTime >= curtime && hasSelected == false) 
                {
                    i.Selected = true;
                    hasSelected = true;
                }
                else
                    i.Selected = false;
            }

            var endtime = time.GetEndTimeList(modalDropDownStartTime.SelectedValue);
            modalDropDownEndTime.DataSource = endtime;
            modalDropDownEndTime.DataTextField = "Description";
            modalDropDownEndTime.DataValueField = "C24hrConversion";
            modalDropDownEndTime.DataBind();
        }

        private void InitializeModalJobType(string value = "") 
        {
            JobType jobtype = new JobType();
            var data = jobtype.GetJobTypeList(Convert.ToInt32(Session["DepartmentId"]));
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
                        JobType jobType = new JobType();
                        jobType = jobType.GetJobType(Convert.ToInt32(i.Value));
                        if (jobType != null)
                        {
                            modalTxtBoxJobId.Enabled = Convert.ToBoolean(jobType.RequiredJobId);
                        }
                        if (modalTxtBoxJobId.Enabled == false)
                        {
                            modallabelCustomer.Text = "";
                            modallabelBoxJobDescription.Text = "";
                            modalTxtBoxJobId.Text = "";
                            modalLabelError.Visible = false;
                        }
                        break;
                    }
                }
            }
        }

        private void InitializeModalStartTime(string value = "") 
        {
            Time time = new Time();
            var timelist = time.GetTimeList();
            DateTime date = Convert.ToDateTime(txtBoxDate.Text);
            List<Time> starttime = new List<Time>();
            TimeSpan curtime = TimeSpan.Parse(DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            if (date.CompareTo(DateTime.Today) == 0)
            {
                foreach (Time t in timelist)
                {
                    TimeSpan selectedTime = TimeSpan.Parse(t.C24hrConversion);
                    if (selectedTime >= curtime)
                    {
                        time.Position = t.Position > 1 ? (t.Position-1) : t.Position;
                        break;
                    }
                }
                starttime = time.GetStartTimeList(time.Position);
            }
            else
            {
                starttime = time.GetStartTimeList();
            }

            //remove used time
            JobTracker jobtracker = new JobTracker();
            var joblist = jobtracker.GetJobTrackerList(Convert.ToInt32(Session["UserId"]), date);
            for (int i = 0; i < joblist.Count; i++)
            {
                if (value.Trim() != "") 
                {
                    if (joblist[i].StartTime == DateTime.Parse(value))
                        continue;
                }
                if (joblist[i].EndTime != null)
                {
                    Time stime = time.GetTime(Convert.ToDateTime(joblist[i].StartTime));
                    Time etime = time.GetTime(Convert.ToDateTime(joblist[i].EndTime));
                    RemoveUsedStartTime(starttime, stime.Position, etime.Position);
                }
            }              
            //
            modalDropDownStartTime.DataSource = starttime;
            modalDropDownStartTime.DataTextField = "Description";
            modalDropDownStartTime.DataValueField = "C24hrConversion";
            modalDropDownStartTime.DataBind();
            if (value.Trim() == "")
            {
                bool hasSelected = false;
                int maxindex = 0;
                TimeSpan maxTime = new TimeSpan(24, 59, 0);
                for (int i = 0; i < modalDropDownStartTime.Items.Count;i++)
                {
                    TimeSpan selectedTime = TimeSpan.Parse(modalDropDownStartTime.Items[i].Value);
                    if (selectedTime < curtime && maxTime < selectedTime) 
                    {
                        maxindex = i;
                        maxTime = selectedTime;
                    }
                    if (selectedTime >= curtime && hasSelected == false)
                    {
                        modalDropDownStartTime.Items[i].Selected = true;
                        hasSelected = true;
                    }
                    else
                        modalDropDownStartTime.Items[i].Selected = false;
                }
                if (hasSelected == false && modalDropDownStartTime.Items.Count > 0) 
                {
                    if (maxindex > 0)
                        modalDropDownStartTime.Items[maxindex].Selected = true;
                    else
                        modalDropDownStartTime.Items[modalDropDownStartTime.Items.Count - 1].Selected = true;
                }
            }
            else 
            {
                DateTime stime = DateTime.Parse(value);
                string selectedTime = stime.Hour + ":" + stime.Minute;
                foreach (ListItem i in modalDropDownStartTime.Items)
                {
                    if (TimeSpan.Parse(i.Value.Trim()) == TimeSpan.Parse(selectedTime.Trim()))
                    {
                        i.Selected = true;
                        break;
                    }
                }
            }
        }

        private void InitializeModalEndTime(string value = "") 
        {
            Time time = new Time();
            TimeSpan curtime = TimeSpan.Parse(DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            var timelist = time.GetTimeList();
            List<Time> endtime = new List<Time>();
            DateTime date = Convert.ToDateTime(txtBoxDate.Text);

            if (date.CompareTo(DateTime.Today) == 0)
            {
                foreach (Time t in timelist)
                {
                    TimeSpan selectedTime = TimeSpan.Parse(t.C24hrConversion);
                    if (selectedTime >= curtime)
                    {
                        time.Position = t.Position;
                        break;
                    }
                }
                endtime = time.GetEndTimeList(modalDropDownStartTime.SelectedItem.Value,time.Position);
                time.Description = "Select End Time";
                time.C24hrConversion = "00";
                endtime.Insert(0, time);
            }
            else
                endtime = time.GetEndTimeList(modalDropDownStartTime.SelectedItem.Value);
            //Remove Used Time
            RemoveUsedTime(endtime, DateTime.Parse(date.Year + "-" + date.Month + "-" + date.Day + " " + modalDropDownStartTime.SelectedValue + ":00"));
            //
            modalDropDownEndTime.DataSource = endtime;
            modalDropDownEndTime.DataTextField = "Description";
            modalDropDownEndTime.DataValueField = "C24hrConversion";
            modalDropDownEndTime.DataBind();
            if (value.Trim() != "")
            {
                DateTime stime = DateTime.Parse(value);
                string selectedTime = stime.Hour + ":" + stime.Minute;
                foreach (ListItem i in modalDropDownEndTime.Items)
                {
                    if (TimeSpan.Parse(i.Value) == TimeSpan.Parse(selectedTime.Trim()))
                    {
                        i.Selected = true;
                        break;
                    }
                }
            }


        }

        private void InitializeModalSupervisor(string value = "") 
        {
            int departmentId = Convert.ToInt32(Session["DepartmentId"]);
            User user = new User();
            var supervisors = user.GetSupervisors(departmentId);
            modalDropDownSupervisor.DataSource = supervisors;
            modalDropDownSupervisor.DataTextField = "fullname";
            modalDropDownSupervisor.DataValueField = "Id";
            modalDropDownSupervisor.DataBind();

            if (value.Trim() != "") 
            {
                foreach (ListItem i in modalDropDownSupervisor.Items)
                {
                    if (i.Value.Trim() == value.Trim())
                    {
                        i.Selected = true;
                        break;
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

        protected void RemoveUsedStartTime(List<Time> datalist,int stimepos, int etimepos) 
        {
            for (int i = 0; i < datalist.Count; i++) 
            {
                if (datalist[i].Position >= stimepos && datalist[i].Position < etimepos)
                {
                    //if (stimepos == datalist[i].Position && i > 0) 
                    //{
                    //    datalist.RemoveAt(i - 1);
                    //    --i;
                    //}
                    datalist.RemoveAt(i);
                    i--;
                }
                else if (datalist[i].Position >= etimepos)
                    break;
            }
        }

        protected void RemoveUsedTime(List<Time> datalist, DateTime startTime) 
        {
            JobTracker jobtracker = new JobTracker();
            int userid = Convert.ToInt32(Session["UserId"]);
            DateTime selectedDate = DateTime.Parse(txtBoxDate.Text);
            jobtracker = jobtracker.GetNextUsedTime(userid, startTime, selectedDate);
            if (jobtracker != null) 
            {
                Time jobTime = new Time();
                jobTime = jobTime.GetTime(Convert.ToDateTime(jobtracker.StartTime));
                for (int i = 0; i < datalist.Count; i++) 
                {
                    if (datalist[i].Position > jobTime.Position) 
                    {
                        datalist.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
        #endregion
    }
}