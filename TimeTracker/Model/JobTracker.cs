using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TimeTracker.Model
{
    public class JobTracker : T_JobTracker
    {
        public string jobtype { get; set; }
        public string pcbdesc { get; set; }
        public string customer { get; set; }
        public string totalhours { get; set; }
        public string fullname { get; set; }

        public JobTracker GetJobTracker(int jobtrackerid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.Id == jobtrackerid
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            jobtype = j.M_JobType.Description,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                            JobStatus = j.JobStatus,
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname
                        }).FirstOrDefault();

            db.Dispose();

            if (data.JobIdNumber != null && data.JobIdNumber != "")
            {
                GetCustomer(data);
            }

            if (data.EndTime != null)
            {
                double time = Convert.ToDateTime(data.EndTime).Subtract(Convert.ToDateTime(data.StartTime)).TotalMinutes;
                double hr = Math.Truncate(time / 60);
                double min = time % 60;
                data.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
            }

            return data;
        }

        public JobTracker GetJobTracker(int createdby,int lastupdatedby,DateTime starttime,int jobtypeid,string actionrequest,string status)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.CreatedBy == createdby
                        && j.LastUpdatedBy == lastupdatedby
                        && j.StartTime == starttime
                        && j.JobTypeId == jobtypeid
                        && j.ActionRequest == actionrequest
                        && j.Status == status
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            jobtype = j.M_JobType.Description,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                            JobStatus = j.JobStatus,
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname
                        }).FirstOrDefault();

            db.Dispose();


            if (data.JobIdNumber != null && data.JobIdNumber != "")
            {
                GetCustomer(data);
            }

            if (data.EndTime != null)
            {
                double time = Convert.ToDateTime(data.EndTime).Subtract(Convert.ToDateTime(data.StartTime)).TotalMinutes;
                double hr = Math.Truncate(time / 60);
                double min = time % 60;
                data.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
            }

            return data;
        }

        public JobTracker GetNextUsedTime(int userid,DateTime starttime, DateTime selecteddate)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.UserId == userid
                        && j.ScheduleDate == selecteddate
                        && j.StartTime > starttime
                        orderby j.StartTime
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            jobtype = j.M_JobType.Description,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                            JobStatus = j.JobStatus,
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public List<JobTracker> GetJobTrackerList(int userid) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.UserId == userid
                        select new JobTracker() 
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            jobtype = j.M_JobType.Description,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                            JobStatus = j.JobStatus,
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname
                        }).ToList();

            db.Dispose();

            foreach (JobTracker j in data)
            {
                if (j.JobIdNumber != null && j.JobIdNumber != "")
                {
                    GetCustomer(j);
                }

                if (j.EndTime != null)
                {
                    double time = Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
                    double hr = Math.Truncate(time / 60);
                    double min = time % 60;
                    j.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
                }

            }


            return data;
        }

        public JobTracker GetJobTrackerJobOverview(int jobtypeid,string SW,string HW,DateTime sdate,DateTime edate,int departmentid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.JobTypeId == jobtypeid
                        && j.M_User.DepartmentId == departmentid
                        &&
                        //((j.StartTime >= sdate
                        //&& j.StartTime <= edate)
                        //||
                        //(j.EndTime >= sdate && j.EndTime <= edate))
                         j.EndTime <= edate
                        && j.Status == "Approved"
                        && j.SWNo == SW
                        && j.HWNo == HW
                        orderby j.EndTime descending
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            jobtype = j.M_JobType.Description,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                            JobStatus = j.JobStatus,
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public JobTracker GetJobTrackerJobOverview(int jobtypeid, string SW, string HW, DateTime sdate, DateTime edate)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.JobTypeId == jobtypeid
                        &&
                        //((j.StartTime >= sdate
                        //&& j.StartTime <= edate)
                        //||
                        //(j.EndTime >= sdate && j.EndTime <= edate))
                        j.EndTime <= edate
                        && j.Status == "Approved"
                        && j.SWNo == SW
                        && j.HWNo == HW
                        orderby j.EndTime descending
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            jobtype = j.M_JobType.Description,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                            JobStatus = j.JobStatus,
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public JobTracker GetJobTrackerJobOverview(string SW, string HW, DateTime sdate, DateTime edate, int departmentid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.M_User.DepartmentId == departmentid
                        &&
                        //((j.StartTime >= sdate
                        //&& j.StartTime <= edate)
                        //||
                        //(j.EndTime >= sdate && j.EndTime <= edate))
                        j.EndTime <= edate
                        && j.Status == "Approved"
                        && j.SWNo == SW
                        && j.HWNo == HW
                        
                        orderby j.EndTime descending
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            jobtype = j.M_JobType.Description,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                            JobStatus = j.JobStatus,
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname
                        }).ToList();

            db.Dispose();
            JobTracker result = new JobTracker();
            string curstatus = "";
            int curindex = -1;
            for (int i = 0; i < data.Count; i++) 
            {
                if (data[i].JobStatus.IndexOf("On Hold") > -1)
                {
                    curstatus = "On Hold";
                    curindex = i;
                    
                    break;
                }
                else if (data[i].JobStatus.IndexOf("In Progress") > -1)
                {
                    if (curstatus != "In Progress")
                    {
                        curstatus = "In Progress";
                        curindex = i;
                    }
                }
                else if (data[i].JobStatus.IndexOf("Completed") > -1)
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
                result = data[curindex];
            }
            return result;
        }

        public string GetTotalHours(int jobtypeid, DateTime startdate, DateTime enddate, string jobstatus,int departmentid = 0,string stringjobid="")
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            List<JobTracker> data = new List<JobTracker>();
            if (departmentid > 0)
            {
                data = (from j in db.T_JobTracker
                        where j.ScheduleDate >= startdate
                        && j.ScheduleDate <= enddate
                        && j.JobTypeId == jobtypeid
                        && j.Status == jobstatus
                        && j.M_User.DepartmentId == departmentid
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            JobStatus = j.JobStatus,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo
                        }).ToList();
            }
            else 
            {
                data = (from j in db.T_JobTracker
                        where j.ScheduleDate >= startdate
                        && j.ScheduleDate <= enddate
                        && j.JobTypeId == jobtypeid
                        && j.Status == jobstatus
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            JobStatus = j.JobStatus,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo
                        }).ToList();
            }

            db.Dispose();
            double totalTime = 0;
            foreach (JobTracker j in data)
            {
                if (j.HWNo.ToString().Contains(stringjobid) || j.SWNo.ToString().Contains(stringjobid))
                    totalTime += Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
            }
            double hr = Math.Truncate(totalTime / 60);
            double min = totalTime % 60;
            string result = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
            return result;
        }

        public string GetTotalHours(int jobtypeid,int userid, DateTime startdate, DateTime enddate, string jobstatus, int departmentid,string stringjobid="")
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            List<JobTracker> data = new List<JobTracker>();

            data = (from j in db.T_JobTracker
                    where j.ScheduleDate >= startdate
                    && j.ScheduleDate <= enddate
                    && j.JobTypeId == jobtypeid
                    && j.Status == jobstatus
                    && j.M_User.DepartmentId == departmentid
                    && j.UserId == userid
                    select new JobTracker()
                    {
                        Id = j.Id,
                        UserId = j.UserId,
                        StartTime = j.StartTime,
                        EndTime = j.EndTime,
                        Description = j.Description,
                        JobTypeId = j.JobTypeId,
                        JobIdNumber = j.JobIdNumber,
                        Remarks = j.Remarks,
                        ApprovedBy = j.ApprovedBy,
                        CreateDate = j.CreateDate,
                        LastUpdateDate = j.LastUpdateDate,
                        CreatedBy = j.CreatedBy,
                        LastUpdatedBy = j.LastUpdatedBy,
                        Status = j.Status,
                        SupervisorRemarks = j.SupervisorRemarks,
                        ActionRequest = j.ActionRequest,
                        ScheduleDate = j.ScheduleDate,
                        JobStatus = j.JobStatus,
                        SWNo = j.SWNo,
                        HWNo = j.HWNo
                    }).ToList();

            db.Dispose();

            double totalTime = 0;
            foreach (JobTracker j in data)
            {
                if(j.HWNo.ToString().Contains(stringjobid) || j.SWNo.ToString().Contains(stringjobid))
                    totalTime += Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
            }
            double hr = Math.Truncate(totalTime / 60);
            double min = totalTime % 60;
            string result = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
            return result;
        }

        public int GetTotalUnclosedJobs(int jobtypeid, int userid, DateTime startdate, DateTime enddate, string jobstatus, int departmentid, string stringjobid = "")
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                    where j.ScheduleDate >= startdate
                    && j.ScheduleDate <= enddate
                    && j.JobTypeId == jobtypeid
                    && j.Status == jobstatus
                    && j.M_User.DepartmentId == departmentid
                    && j.UserId == userid
                    select new JobTracker()
                    {
                        Id = j.Id,
                        UserId = j.UserId,
                        StartTime = j.StartTime,
                        EndTime = j.EndTime,
                        Description = j.Description,
                        JobTypeId = j.JobTypeId,
                        JobIdNumber = j.JobIdNumber,
                        Remarks = j.Remarks,
                        ApprovedBy = j.ApprovedBy,
                        CreateDate = j.CreateDate,
                        LastUpdateDate = j.LastUpdateDate,
                        CreatedBy = j.CreatedBy,
                        LastUpdatedBy = j.LastUpdatedBy,
                        Status = j.Status,
                        SupervisorRemarks = j.SupervisorRemarks,
                        ActionRequest = j.ActionRequest,
                        ScheduleDate = j.ScheduleDate,
                        JobStatus = j.JobStatus,
                        SWNo = j.SWNo,
                        HWNo = j.HWNo
                    }).ToList();

            db.Dispose();
            int counter = 0;
            foreach (JobTracker j in data)
            {
                if (j.HWNo.ToString().Contains(stringjobid) || j.SWNo.ToString().Contains(stringjobid))
                    counter++;
            }
            return counter;
        }

        public int GetTotalUnclosedJobs(int jobtypeid, DateTime startdate, DateTime enddate, string jobstatus, int departmentid = 0,string stringjobid = "")
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            List<JobTracker> data = new List<JobTracker>();
            if (departmentid > 0)
            {
                data = (from j in db.T_JobTracker
                        where j.ScheduleDate >= startdate
                        && j.ScheduleDate <= enddate
                        && j.JobTypeId == jobtypeid
                        && j.Status == jobstatus
                        && j.M_User.DepartmentId == departmentid
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            JobStatus = j.JobStatus,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo
                        }).ToList();
            }
            else
            {
                data = (from j in db.T_JobTracker
                        where j.ScheduleDate >= startdate
                        && j.ScheduleDate <= enddate
                        && j.JobTypeId == jobtypeid
                        && j.Status == jobstatus
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            JobStatus = j.JobStatus,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo
                        }).ToList();
            }

            db.Dispose();
            int counter = 0;
            foreach (JobTracker j in data)
            {
                if (j.HWNo.ToString().Contains(stringjobid) || j.SWNo.ToString().Contains(stringjobid))
                    counter++;
            }
            return counter;
        }

        public List<JobTracker> GetJobTrackerList(int userid,DateTime selecteddate)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.UserId == userid
                        && j.ScheduleDate == selecteddate
                        orderby j.StartTime ascending
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            jobtype = j.M_JobType.Description,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            JobStatus = j.JobStatus,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname
                        }).ToList();

            db.Dispose();

            foreach (JobTracker j in data)
            {
                if (j.JobIdNumber != null && j.JobIdNumber != "")
                {
                    GetCustomer(j);
                }

                if (j.EndTime != null)
                {
                    double time = Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
                    double hr = Math.Truncate(time / 60);
                    double min = time % 60;
                    j.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
                }

            }


            return data;
        }

        public List<JobTracker> GetJobTrackerListExcludeRejected(int userid, DateTime startdate,DateTime enddate)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.UserId == userid
                        && j.ScheduleDate >= startdate
                        && j.EndTime <= enddate
                        && j.Status != "Rejected"
                        orderby j.StartTime ascending
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            jobtype = j.M_JobType.Description,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            JobStatus = j.JobStatus,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname
                        }).ToList();

            db.Dispose();

            foreach (JobTracker j in data)
            {
                if (j.JobIdNumber != null && j.JobIdNumber != "")
                {
                    GetCustomer(j);
                }

                if (j.EndTime != null)
                {
                    double time = Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
                    double hr = Math.Truncate(time / 60);
                    double min = time % 60;
                    j.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
                }

            }


            return data;
        }

        public List<JobTracker> GetDistinctProjectList(DateTime sdate,DateTime edate,string stringjobid = "") 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where (j.HWNo != ""
                        || j.SWNo != "") 
                        &&
                        ((j.StartTime >= sdate
                        && j.StartTime <= edate) 
                        ||
                        ( j.EndTime >= sdate && j.EndTime <= edate))
                        && j.Status == "Approved"
                         && j.JobIdNumber != ""
                        orderby j.StartTime ascending
                        select new JobTracker()
                        {
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                        }).Distinct().ToList();

            db.Dispose();
            for (int i = 0; i < data.Count; i++) 
            {
                if (data[i].HWNo.ToString().Contains(stringjobid) || data[i].SWNo.ToString().Contains(stringjobid))
                {
                    data[i].GetCustomer(data[i]);
                }
                else 
                {
                    data.RemoveAt(i);
                    i--;
                }
            }

            return data;
        }

        public string GetTotalHours(int userid, string status, DateTime date,ref double totalmin)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.UserId == userid
                        && j.ScheduleDate == date
                        && j.Status == status
                        && j.M_JobType.ComputeTime == true
                        orderby j.StartTime
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            JobStatus = j.JobStatus,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo
                        }).ToList();

            db.Dispose();
            double totalTime = 0;
            foreach (JobTracker j in data) 
            {
                 totalTime += Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
            }
            double hr = Math.Truncate(totalTime / 60);
            double min = totalTime % 60;
            string result = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
            totalmin = totalTime;
            return result;
        }

        public List<JobTracker> GetRequestNeededApproval(int userid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        join s in db.T_SupervisorMapping 
                        on new {j.UserId,userid }
                        equals new { UserId = (int?)s.UserId,userid = s.SupervisorId }
                        where j.Status == "For Approval"
                        orderby j.LastUpdateDate ascending
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            jobtype = j.M_JobType.Description,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            JobStatus = j.JobStatus,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname //Requestor
                        }).ToList();

            db.Dispose();

            foreach (JobTracker j in data)
            {
                if (j.JobIdNumber != null && j.JobIdNumber != "")
                {
                    GetCustomer(j);
                }

                if (j.EndTime != null)
                {
                    double time = Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
                    double hr = Math.Truncate(time / 60);
                    double min = time % 60;
                    j.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
                }

            }


            return data;
        }

        public List<JobTracker> GetPendingRequest(int userid) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.UserId == userid
                        && j.Status == "For Approval"
                        orderby j.LastUpdateDate ascending
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            jobtype = j.M_JobType.Description,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            JobStatus = j.JobStatus,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                            fullname = j.M_Supervisor.Firstname + " " + j.M_Supervisor.Lastname //Supervisor
                        }).ToList();

            db.Dispose();

            foreach (JobTracker j in data)
            {
                if (j.JobIdNumber != null)
                {
                    GetCustomer(j);
                }
                if (j.EndTime != null)
                {
                    double time = Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
                    double hr = Math.Truncate(time / 60);
                    double min = time % 60;
                    j.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
                }

            }


            return data;
        
        }

        public List<JobTracker> GetRejectedRequest(int userid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.UserId == userid
                        && j.Status == "Rejected"
                        orderby j.LastUpdateDate descending
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            jobtype = j.M_JobType.Description,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                            JobStatus = j.JobStatus,
                            fullname = j.M_Supervisor.Firstname + " " + j.M_Supervisor.Lastname //Supervisor
                        }).Take(10).ToList();

            db.Dispose();

            foreach (JobTracker j in data)
            {
                if (j.JobIdNumber != null && j.JobIdNumber != "")
                {
                    GetCustomer(j);
                }

                if (j.EndTime != null)
                {
                    double time = Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
                    double hr = Math.Truncate(time / 60);
                    double min = time % 60;
                    j.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
                }

            }


            return data;

        }

        public List<JobTracker> GetUnclosedJobs(int userid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.ApprovedBy == userid
                        && j.Status == "Pending"
                        orderby j.StartTime
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            jobtype = j.M_JobType.Description,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                            JobStatus = j.JobStatus,
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname
                        }).ToList();

            db.Dispose();

            foreach (JobTracker j in data)
            {
                if (j.JobIdNumber != null && j.JobIdNumber != "")
                {
                    GetCustomer(j);
                }

            }


            return data;
        }

        public List<JobTracker> GetUnclosedJobs(int userid,DateTime selecteddate)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.ApprovedBy == userid
                        && j.Status == "Pending"
                        && j.ScheduleDate == selecteddate
                        orderby j.StartTime
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            jobtype = j.M_JobType.Description,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                            JobStatus = j.JobStatus,
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname
                        }).ToList();

            db.Dispose();

            foreach (JobTracker j in data)
            {
                if (j.JobIdNumber != null && j.JobIdNumber != "")
                {
                    GetCustomer(j);
                }

                if (j.EndTime != null)
                {
                    double time = Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
                    double hr = Math.Truncate(time / 60);
                    double min = time % 60;
                    j.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
                }

            }


            return data;
        }

        public JobTracker GetCustomer(string jobid) 
        {
            JobTracker jobTracker = new JobTracker();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + jobid.Trim() + "'", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    jobTracker.customer = reader["CO_Name"].ToString();
                    jobTracker.pcbdesc = reader["SO_PCBdesc"].ToString();
                }
            }
            if (jobTracker.customer == null || jobTracker.customer == "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
                {
                    SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + jobid.Trim() + "'", con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        jobTracker.customer = reader["CO_Name"].ToString();
                        jobTracker.pcbdesc = reader["SO_PCBdesc"].ToString();
                    }
                }
            }

            return jobTracker;
        }

        public void GetCustomer(JobTracker jobtracker) 
        {
            if (jobtracker.HWNo != null && jobtracker.HWNo.Trim() != "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
                {
                    SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + jobtracker.HWNo.Trim() + "'", con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        jobtracker.customer = reader["CO_Name"].ToString();
                        jobtracker.pcbdesc = reader["SO_PCBdesc"].ToString();
                    }
                }
            }
            else if ((jobtracker.SWNo != null && jobtracker.SWNo.Trim() != "") && (jobtracker.customer == null || jobtracker.customer.Trim() == "")) 
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
                {
                    SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + jobtracker.SWNo.Trim() + "'", con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        jobtracker.customer = reader["CO_Name"].ToString();
                        jobtracker.pcbdesc = reader["SO_PCBdesc"].ToString();
                    }
                }
            }
        }

        public void Insert(JobTracker jobtracker) 
        {
            T_JobTracker j = new T_JobTracker();
            ParseJobTracker(j, jobtracker);
            using (TimeTrackerEntities db = new TimeTrackerEntities()) 
            {
                try
                {
                    db.T_JobTracker.Add(j);
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
                    T_JobTracker t_jobtracker = new T_JobTracker();
                    t_jobtracker = db.T_JobTracker.FirstOrDefault(j => j.Id == id);
                    db.T_JobTracker.Remove(t_jobtracker);
                    db.SaveChanges();
                }
                catch (Exception ex) 
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Update(JobTracker jobtracker) 
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities()) 
            {
                try 
                {
                    T_JobTracker t_jobtracker = db.T_JobTracker.FirstOrDefault(j => j.Id == jobtracker.Id);
                    UpdateParse(t_jobtracker, jobtracker);
                    db.SaveChanges();
                }
                catch (Exception ex) 
                {
                    string msg = ex.Message;
                }
            }
        }

        private void ParseJobTracker(T_JobTracker data,JobTracker jobtracker) 
        {
            data.Id = jobtracker.Id;
            data.JobIdNumber = jobtracker.JobIdNumber;
            data.JobTypeId = jobtracker.JobTypeId;
            data.Remarks = jobtracker.Remarks;
            data.Description = jobtracker.Description;
            data.StartTime = jobtracker.StartTime;
            data.EndTime = jobtracker.EndTime;
            data.ApprovedBy = jobtracker.ApprovedBy;
            data.CreatedBy = jobtracker.CreatedBy;
            data.LastUpdateDate = jobtracker.LastUpdateDate;
            data.LastUpdatedBy = jobtracker.LastUpdatedBy;
            data.CreateDate = jobtracker.CreateDate;
            data.Status = jobtracker.Status;
            data.SupervisorRemarks = jobtracker.SupervisorRemarks;
            data.UserId = jobtracker.UserId;
            data.ScheduleDate = jobtracker.ScheduleDate;
            data.ActionRequest = jobtracker.ActionRequest;
            data.HWNo = jobtracker.HWNo;
            data.SWNo = jobtracker.SWNo;
            data.JobStatus = jobtracker.JobStatus;
        }

        private void UpdateParse(T_JobTracker t_jobtracker, JobTracker jobtracker) 
        {
            t_jobtracker.JobIdNumber = jobtracker.JobIdNumber;
            t_jobtracker.JobTypeId = jobtracker.JobTypeId;
            t_jobtracker.Remarks = jobtracker.Remarks;
            t_jobtracker.Description = jobtracker.Description;
            t_jobtracker.StartTime = jobtracker.StartTime;
            t_jobtracker.EndTime = jobtracker.EndTime;
            t_jobtracker.ApprovedBy = jobtracker.ApprovedBy;
            t_jobtracker.LastUpdateDate = jobtracker.LastUpdateDate;
            t_jobtracker.LastUpdatedBy = jobtracker.LastUpdatedBy;
            t_jobtracker.Status = jobtracker.Status;
            t_jobtracker.SupervisorRemarks = jobtracker.SupervisorRemarks;
            t_jobtracker.UserId = jobtracker.UserId;
            t_jobtracker.ScheduleDate = jobtracker.ScheduleDate;
            t_jobtracker.ActionRequest = jobtracker.ActionRequest;
            t_jobtracker.HWNo = jobtracker.HWNo;
            t_jobtracker.SWNo = jobtracker.SWNo;
            t_jobtracker.JobStatus = jobtracker.JobStatus;
        }

        public bool HasUnclosedJobs(int userid) 
        {
            bool result = true;
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.ApprovedBy == userid
                        && j.Status == "Pending"
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            jobtype = j.M_JobType.Description,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname
                        }).FirstOrDefault();

            db.Dispose();

            if (data == null)
                result = false;

            return result;
        }

        public JobTracker GenerateHWAndSW(string jobidnumber) 
        {
            JobTracker jobtracker = new JobTracker();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("Select TOP 1 SO_Num,CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + jobidnumber.Trim() + "'", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    jobtracker.customer = reader["CO_Name"].ToString();
                    jobtracker.pcbdesc = reader["SO_PCBdesc"].ToString();
                    jobtracker.HWNo = reader["SO_Num"].ToString();
                }
            }
            if (jobtracker.HWNo == null || jobtracker.HWNo.Trim() == "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
                {
                    SqlCommand cmd = new SqlCommand("Select TOP 1 SO_Num,CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + jobidnumber.Trim() + "'", con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        jobtracker.customer = reader["CO_Name"].ToString();
                        jobtracker.pcbdesc = reader["SO_PCBdesc"].ToString();
                        jobtracker.SWNo = reader["SO_Num"].ToString();
                    }
                }
                if (jobtracker.SWNo != null && jobtracker.SWNo.Trim() != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
                    {
                        SqlCommand cmd = new SqlCommand("Select TOP 1 SO_Num from Sales_Order SO where '" + jobtracker.pcbdesc.Trim() + "' Like '%'+CAST(SO_Num as varchar(20))+'%' ORDER BY SO_NUM Desc", con);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            jobtracker.HWNo = reader["SO_Num"].ToString();
                        }
                    }

                    if (jobtracker.HWNo == null || jobtracker.HWNo.Trim() == "")
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
                        {
                            SqlCommand cmd = new SqlCommand("Select TOP 1 SO_Num from Sales_Order SO where SO_PCBdesc Like '%" + jobtracker.SWNo.Trim() + "%' ORDER BY SO_NUM Desc", con);
                            con.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                jobtracker.HWNo = reader["SO_Num"].ToString();
                            }
                        }
                    }
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
                {
                    SqlCommand cmd = new SqlCommand("Select TOP 1 SO_Num from Sales_Order SO where '" + jobtracker.pcbdesc.Trim() + "' Like '%'+CAST(SO_Num as varchar(20))+'%' ORDER BY SO_NUM Desc", con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        jobtracker.SWNo = reader["SO_Num"].ToString();
                    }
                }
                
                
                if (jobtracker.SWNo == null || jobtracker.SWNo.Trim() == "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
                    {
                        SqlCommand cmd = new SqlCommand("Select TOP 1 SO_Num from Sales_Order SO where SO_PCBdesc Like '%" + jobtracker.HWNo.Trim() + "%' ORDER BY SO_NUM Desc", con);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            jobtracker.SWNo = reader["SO_Num"].ToString();
                        }
                    }
                }
            }
            return jobtracker;
        }

        public string GetError(int userid, DateTime selecteddate,int numberofdays) 
        {
            string result = "";
            DateTime sdate = selecteddate.AddDays(-1 * numberofdays);
            for (int i = 1; i < numberofdays; i++) 
            {
                if (HasTimeGap(userid, sdate)) 
                {
                    result = "There is a Time Gap on " + sdate.ToString("dd MMM yyyy") + ".";
                    break;
                }
                else if (HasTimeClockGap(userid, sdate)) 
                {
                    result = "There is a missing task not recorded on " + sdate.ToString("dd MMM yyyy") + ".";
                    break;
                }
                sdate = sdate.AddDays(1);
            }
            return result;
        }

        private bool HasTimeGap(int userid, DateTime selecteddate) 
        {
            bool result = false;

            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.UserId == userid
                        && j.ScheduleDate == selecteddate
                        && j.Status != "Rejected"
                        && j.EndTime != null
                        orderby j.StartTime ascending
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            jobtype = j.M_JobType.Description,
                            Remarks = j.Remarks,
                            ApprovedBy = j.ApprovedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            Status = j.Status,
                            SupervisorRemarks = j.SupervisorRemarks,
                            ActionRequest = j.ActionRequest,
                            ScheduleDate = j.ScheduleDate,
                            JobStatus = j.JobStatus,
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname
                        }).ToList();

            db.Dispose();
            DateTime sTime = new DateTime();
            for (int i = 0; i < data.Count; i++) 
            {
                if (i == 0) 
                {
                    sTime = Convert.ToDateTime(data[i].EndTime);
                }
                else if (sTime != data[i].StartTime)
                {
                    result = true;
                    break;
                }
                else 
                {
                    sTime = Convert.ToDateTime(data[i].EndTime);
                }
            }

            return result;
        }

        private bool HasTimeClockGap(int userid, DateTime selecteddate) 
        {
            bool result = false;
            User user = new User();
            user = user.GetUser(userid);
            if (user.EmployeeNumber != 0) 
            {
                TimeClock timeclock = new TimeClock();
                timeclock = timeclock.GetStartEndTime(Convert.ToInt32(user.EmployeeNumber),Convert.ToDateTime(selecteddate.ToString("dd MMM yyyy")+" 00:00:00"),Convert.ToDateTime(selecteddate.ToString("dd MMM yyyy")+" 23:59:59"));

                if (timeclock != null)
                {
                    TimeTrackerEntities db = new TimeTrackerEntities();

                    var data = (from j in db.T_JobTracker
                                where j.UserId == userid
                                && j.ScheduleDate == selecteddate
                                && j.Status != "Rejected"
                                && j.EndTime != null
                                orderby j.StartTime ascending
                                select new JobTracker()
                                {
                                    Id = j.Id,
                                    UserId = j.UserId,
                                    StartTime = j.StartTime,
                                    EndTime = j.EndTime,
                                    Description = j.Description,
                                    JobTypeId = j.JobTypeId,
                                    JobIdNumber = j.JobIdNumber,
                                    jobtype = j.M_JobType.Description,
                                    Remarks = j.Remarks,
                                    ApprovedBy = j.ApprovedBy,
                                    CreateDate = j.CreateDate,
                                    LastUpdateDate = j.LastUpdateDate,
                                    CreatedBy = j.CreatedBy,
                                    LastUpdatedBy = j.LastUpdatedBy,
                                    Status = j.Status,
                                    SupervisorRemarks = j.SupervisorRemarks,
                                    ActionRequest = j.ActionRequest,
                                    ScheduleDate = j.ScheduleDate,
                                    JobStatus = j.JobStatus,
                                    SWNo = j.SWNo,
                                    HWNo = j.HWNo,
                                    fullname = j.M_User.Firstname + " " + j.M_User.Lastname
                                }).ToList();

                    db.Dispose();
                    if (data.Count > 0)
                    {
                        TimeSetting timesetting = new TimeSetting();
                        timesetting = timesetting.GetTimeSetting();
                        if (timeclock.starttime.AddMinutes(timesetting.Interval) < Convert.ToDateTime(data[0].StartTime))
                            result = true;
                        if (timeclock.endtime.AddMinutes(-1 * timesetting.Interval) > Convert.ToDateTime(data[data.Count - 1].EndTime))
                            result = true;
                    }
                    else 
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

    }
}