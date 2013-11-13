using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TimeTracker.Model
{
    public class JobTrackerHistory : T_JobTrackerHistory
    {
        public string jobtype { get; set; }
        public string pcbdesc { get; set; }
        public string customer { get; set; }
        public string totalhours { get; set; }
        public string fullname { get; set; }

        public void Insert(JobTrackerHistory jtHist)
        {
            T_JobTrackerHistory j = new T_JobTrackerHistory();
            ParseJobTracker(j, jtHist);
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    db.T_JobTrackerHistory.Add(j);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Delete(int id)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_JobTrackerHistory t_jthist = new T_JobTrackerHistory();
                    t_jthist = db.T_JobTrackerHistory.FirstOrDefault(j => j.Id == id);
                    db.T_JobTrackerHistory.Remove(t_jthist);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Update(JobTrackerHistory jthist)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_JobTrackerHistory t_jthist= db.T_JobTrackerHistory.FirstOrDefault(j => j.Id == jthist.Id);
                    UpdateParse(t_jthist, jthist);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        private void ParseJobTracker(T_JobTrackerHistory data, JobTrackerHistory jtHist)
        {
            data.Id = jtHist.Id;
            data.JobTrackerId = jtHist.JobTrackerId;
            data.JobIdNumber = jtHist.JobIdNumber;
            data.JobTypeId = jtHist.JobTypeId;
            data.Remarks = jtHist.Remarks;
            data.Description = jtHist.Description;
            data.StartTime = jtHist.StartTime;
            data.EndTime = jtHist.EndTime;
            data.ApprovedBy = jtHist.ApprovedBy;
            data.CreatedBy = jtHist.CreatedBy;
            data.LastUpdateDate = jtHist.LastUpdateDate;
            data.LastUpdatedBy = jtHist.LastUpdatedBy;
            data.CreateDate = jtHist.CreateDate;
            data.Status = jtHist.Status;
            data.SupervisorRemarks = jtHist.SupervisorRemarks;
            data.UserId = jtHist.UserId;
            data.ScheduleDate = jtHist.ScheduleDate;
            data.Action = jtHist.Action;
            data.JobStatus = jtHist.JobStatus;
            data.HWNo = jtHist.HWNo;
            data.SWNo = jtHist.SWNo;
        }

        private void UpdateParse(T_JobTrackerHistory t_jthist, JobTrackerHistory jthist)
        {
            t_jthist.JobIdNumber = jthist.JobIdNumber;
            t_jthist.JobTrackerId = jthist.JobTrackerId;
            t_jthist.JobTypeId = jthist.JobTypeId;
            t_jthist.Remarks = jthist.Remarks;
            t_jthist.Description = jthist.Description;
            t_jthist.StartTime = jthist.StartTime;
            t_jthist.EndTime = jthist.EndTime;
            t_jthist.ApprovedBy = jthist.ApprovedBy;
            t_jthist.LastUpdateDate = jthist.LastUpdateDate;
            t_jthist.LastUpdatedBy = jthist.LastUpdatedBy;
            t_jthist.Status = jthist.Status;
            t_jthist.SupervisorRemarks = jthist.SupervisorRemarks;
            t_jthist.UserId = jthist.UserId;
            t_jthist.ScheduleDate = jthist.ScheduleDate;
            t_jthist.Action = jthist.Action;
            t_jthist.JobStatus = jthist.JobStatus;
            t_jthist.HWNo = jthist.HWNo;
            t_jthist.SWNo = jthist.SWNo;
        }

        public JobTrackerHistory ConvertToHistory(JobTracker jobtracker) 
        {
            JobTrackerHistory jthist = new JobTrackerHistory();
            jthist.JobTrackerId = jobtracker.Id;
            jthist.JobIdNumber = jobtracker.JobIdNumber;
            jthist.JobTypeId = jobtracker.JobTypeId;
            jthist.Remarks = jobtracker.Remarks;
            jthist.Description = jobtracker.Description;
            jthist.StartTime = jobtracker.StartTime;
            jthist.EndTime = jobtracker.EndTime;
            jthist.ApprovedBy = jobtracker.ApprovedBy;
            jthist.CreatedBy = jobtracker.CreatedBy;
            jthist.LastUpdateDate = jobtracker.LastUpdateDate;
            jthist.LastUpdatedBy = jobtracker.LastUpdatedBy;
            jthist.CreateDate = jobtracker.CreateDate;
            jthist.Status = jobtracker.Status;
            jthist.SupervisorRemarks = jobtracker.SupervisorRemarks;
            jthist.UserId = jobtracker.UserId;
            jthist.ScheduleDate = jobtracker.ScheduleDate;
            jthist.Action = jobtracker.ActionRequest;
            jthist.JobStatus = jobtracker.JobStatus;
            jthist.HWNo = jobtracker.HWNo;
            jthist.SWNo = jobtracker.SWNo;

            return jthist;
        }

    }
}