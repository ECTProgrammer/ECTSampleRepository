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
        public string totalhours { get; set; }
        public string fullname { get; set; }
        public int? departmentid { get; set; }
        public string department { get; set; }
        public string normalhours { get; set; }
        public string othours { get; set; }
        public double normalcost { get; set; }
        public double otcost { get; set; }
        public double normalmins { get; set; }
        public double otmins { get; set; }

        //Get JobTracker by Jobtracker id (has the option to compute the time by providing true to computetime parameter)
        public JobTracker GetJobTracker(int jobtrackerid,bool computetime)
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
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname,
                            Customer = j.Customer,
                            EvalNo = j.EvalNo
                        }).FirstOrDefault();

            db.Dispose();

            //if (data.JobIdNumber != null && data.JobIdNumber != "")
            //{
            //    GetCustomer(data);
            //}
            if (computetime == true)
            {
                if (data != null)
                {
                    data.ComputeHours();
                }
            }

            return data;
        }

        //Use to get newly created jobtracker
        public JobTracker GetJobTracker(int createdby, int lastupdatedby, DateTime starttime, int jobtypeid, string actionrequest, string status, bool computetime)
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
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname,
                            Customer = j.Customer,
                            EvalNo = j.EvalNo
                        }).FirstOrDefault();

            db.Dispose();


            //if (data.JobIdNumber != null && data.JobIdNumber != "")
            //{
            //    GetCustomer(data);
            //}
            if (computetime == true)
            {
                User user = new User();
                user = user.GetUser(Convert.ToInt32(data.UserId), Convert.ToDateTime(data.StartTime));
                if (data.EndTime != null)
                {
                    TimeSpan stime = Convert.ToDateTime(data.StartTime).TimeOfDay;
                    TimeSpan etime = Convert.ToDateTime(data.EndTime).TimeOfDay;
                    double nmins = 0;
                    double omins = 0;
                    otcost = 0;
                    normalcost = 0;

                    double time = Convert.ToDateTime(data.EndTime).Subtract(Convert.ToDateTime(data.StartTime)).TotalMinutes;
                    if (user.shifting == false)
                    {
                        if (TimeSpan.Parse(user.endTime) <= stime) //entry is OT
                        {
                            omins = time;
                        }
                        else if ((etime == new TimeSpan(0, 0, 0) ? new TimeSpan(1, 0, 0, 0) : etime) > TimeSpan.Parse(user.endTime)) //entry is from normal time to OT
                        {
                            nmins = TimeSpan.Parse(user.endTime).Subtract(stime).TotalMinutes;
                            omins = (etime == new TimeSpan(0, 0, 0) ? new TimeSpan(1, 0, 0, 0) : etime).Subtract(TimeSpan.Parse(user.endTime)).TotalMinutes;
                        }
                        else //entry is within normal time
                        {
                            nmins = time;
                        }
                    }
                    else
                    {
                        TimeSpan cutoff = user.GetMyCutOfTime();
                        if (stime >= cutoff)
                        {
                            nmins = time;
                        }
                        else
                        {
                            if (TimeSpan.Parse(user.endTime) <= stime) //entry is OT
                            {
                                omins = time;
                            }
                            else if ((etime == new TimeSpan(0, 0, 0) ? new TimeSpan(1, 0, 0, 0) : etime) > TimeSpan.Parse(user.endTime))
                            {
                                nmins = TimeSpan.Parse(user.endTime).Subtract(stime).TotalMinutes;
                                omins = (etime == new TimeSpan(0, 0, 0) ? new TimeSpan(1, 0, 0, 0) : etime).Subtract(TimeSpan.Parse(user.endTime)).TotalMinutes;
                            }
                        }
                    }
                    double hr = Math.Truncate(time / 60);
                    double min = time % 60;
                    data.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");

                    hr = Math.Truncate(nmins / 60);
                    min = nmins % 60;
                    data.normalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");

                    hr = Math.Truncate(omins / 60);
                    min = omins % 60;
                    data.othours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
                }
            }

            return data;
        }

        //Get the Jobtracker data of a specific user which start time is after the provided start time on a selected date
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
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname,
                            Customer = j.Customer,
                            EvalNo = j.EvalNo
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        //Get All jobtracker data of a specifc user (has an option to compute the time spent per task by providing the value true in the computetime parameter)
        public List<JobTracker> GetJobTrackerList(int userid,bool computetime) 
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
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname,
                            Customer = j.Customer,
                            EvalNo = j.EvalNo
                        }).ToList();

            db.Dispose();

            if (computetime == true)
            {
                foreach (JobTracker j in data)
                {
                    if (j != null)
                    {
                        j.ComputeHours();
                    }
                }
            }

            return data;
        }

        //Get all the jobtracker which the user provides job id (has the option to compute the time per task by providing the value true to the computetime parameter)
        public List<JobTracker> GetJobTrackerListWithJobId(bool computetime)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.JobIdNumber != ""
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
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname,
                            Customer = j.Customer,
                            EvalNo = j.EvalNo
                        }).ToList();

            db.Dispose();

            if (computetime == true)
            {
                foreach (JobTracker j in data)
                {
                    if (j != null)
                    {
                        j.ComputeHours();
                    }

                }
            }

            return data;
        }

        //Get all jobtracker where JobTypeId, HW or SW SO is equal to user input and is either approve or for approval
        public List<JobTracker> GetJobTrackerListWithJobTypeIdHWSO(int jobtypeid,string HWSO,string SWSO,bool computetime)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();
            List<JobTracker> data = new List<JobTracker>();
            if (HWSO != null && HWSO != "")
            {
                data = (from j in db.T_JobTracker
                            where j.JobIdNumber != ""
                            && j.Status != "Rejected"
                            && j.EndTime != null
                            && j.JobTypeId == jobtypeid
                            && j.HWNo == HWSO
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
                                fullname = j.M_User.Firstname + " " + j.M_User.Lastname,
                                Customer = j.Customer,
                                EvalNo = j.EvalNo,
                                department = j.M_User.M_Department.Description
                            }).ToList();
            }
            else if (SWSO != null && SWSO != "") 
            {
                data = (from j in db.T_JobTracker
                        where j.JobIdNumber != ""
                        && j.Status != "Rejected"
                        && j.EndTime != null
                        && j.JobTypeId == jobtypeid
                        && j.SWNo == SWSO
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
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname,
                            Customer = j.Customer,
                            EvalNo = j.EvalNo,
                            department = j.M_User.M_Department.Description
                        }).ToList();
            }

            db.Dispose();

            if (computetime == true)
            {
                foreach (JobTracker j in data)
                {
                    if (j != null)
                    {
                        j.ComputeHours();
                        j.ComputeCost();
                    }

                }
            }

            return data;
        }

        //Get Jobtracker data for Overview Tab
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
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname,
                            Customer = j.Customer,
                            EvalNo = j.EvalNo
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        //Get Jobtracker data for Overview Tab
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
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname,
                            Customer = j.Customer,
                            EvalNo = j.EvalNo
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        //Get Jobtracker data for Overview Tab
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
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname,
                            Customer = j.Customer,
                            EvalNo = j.EvalNo
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
                else if (data[i].JobStatus.IndexOf("Completed") > -1 && curindex < 0)
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

        public List<JobTracker> GetUniqueComputedJobType(string HWSO, string SWSO) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            List<JobTracker> data = new List<JobTracker>();

            if (HWSO != null && HWSO != "")
            {
                data = (from j in db.T_JobTracker
                        where j.HWNo == HWSO
                        && j.M_JobType.ComputeTime == true
                        select new JobTracker()
                        {
                            JobTypeId = j.JobTypeId,
                            jobtype = j.M_JobType.Description
                        }).Distinct().ToList();
            }
            else if(SWSO != null && SWSO != "")
            {
                data = (from j in db.T_JobTracker
                        where j.SWNo == SWSO
                        && j.M_JobType.ComputeTime == true
                        select new JobTracker()
                        {
                            JobTypeId = j.JobTypeId,
                            jobtype = j.M_JobType.Description
                        }).Distinct().ToList();
            }
            return data;
        }

        public string GetTotalHours(int jobtypeid, DateTime startdate, DateTime enddate, string jobstatus,int departmentid = 0,string stringjobid="",string customer = "")
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
                            HWNo = j.HWNo,
                            Customer = j.Customer,
                            EvalNo = j.EvalNo
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
                            HWNo = j.HWNo,
                            Customer = j.Customer,
                            EvalNo = j.EvalNo
                        }).ToList();
            }

            db.Dispose();
            if (customer.Trim() != "") 
            {
                data = data.Where(d => (d.Customer == null ? "" : d.Customer).ToUpper().Contains(customer.ToUpper().Trim())).ToList();
            }
            if (stringjobid.Trim() != "") 
            {
                data = data.Where(d => (d.EvalNo == null ? "" : d.EvalNo).ToUpper().Contains(stringjobid.ToUpper().Trim()) || (d.HWNo == null ? "" : d.HWNo.Trim()) == stringjobid.Trim() || (d.SWNo == null ? "" : d.SWNo.Trim()) == stringjobid.Trim()).ToList();
            }

            double totalTime = 0;
            foreach (JobTracker j in data)
            {
                //if (j.HWNo.ToString().Contains(stringjobid) || j.SWNo.ToString().Contains(stringjobid))
                totalTime += Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
            }
            double hr = Math.Truncate(totalTime / 60);
            double min = totalTime % 60;
            string result = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
            return result;
        }

        public string GetTotalHours(int jobtypeid,int userid, DateTime startdate, DateTime enddate, string jobstatus, int departmentid,string stringjobid="",string customer = "")
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
                        HWNo = j.HWNo,
                        Customer = j.Customer,
                        EvalNo = j.EvalNo
                    }).ToList();

            db.Dispose();
            if (customer.Trim() != "")
            {
                data = data.Where(d => (d.Customer == null ? "" : d.Customer).ToUpper().Contains(customer.ToUpper().Trim())).ToList();
            }
            if (stringjobid.Trim() != "")
            {
                data = data.Where(d => (d.EvalNo == null ? "" : d.EvalNo).ToUpper().Contains(stringjobid.ToUpper().Trim()) || d.HWNo.Trim() == stringjobid.Trim() || d.SWNo.Trim() == stringjobid.Trim()).ToList();
            }
            double totalTime = 0;
            foreach (JobTracker j in data)
            {
                //if(j.HWNo.ToString().Contains(stringjobid) || j.SWNo.ToString().Contains(stringjobid))
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
                        HWNo = j.HWNo,
                        EvalNo = j.EvalNo,
                        Customer = j.Customer
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

        public List<JobTracker> GetJobTrackerList(int userid, DateTime selecteddate, bool computetime)
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
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname,
                            Customer = j.Customer,
                            EvalNo = j.EvalNo
                        }).ToList();

            db.Dispose();

            if (computetime == true)
            {
                foreach (JobTracker j in data)
                {
                    if (j != null)
                    {
                        j.ComputeHours();
                    }
                }
            }


            return data;
        }

        public List<JobTracker> GetJobTrackerListExcludeRejected(DateTime startdate, DateTime enddate, int userid = 0,int dept = 0, string jobid = "",string customer = "", bool computetime = false)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.ScheduleDate >= startdate
                        && j.EndTime <= enddate
                        && j.Status != "Rejected"
                        orderby j.StartTime ascending, j.EndTime ascending
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
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname,
                            Customer = j.Customer,
                            EvalNo = j.EvalNo,
                            departmentid = j.M_User.DepartmentId,
                            department = j.M_User.M_Department.Description
                        }).ToList();

            db.Dispose();

            if (userid != 0) 
            {
                data = data.Where(d => d.UserId == userid).ToList();
            }
            if (dept != 0) 
            {
                data = data.Where(d => d.departmentid == dept).ToList();
            }
            if (jobid.Trim() != "") 
            {
                data = data.Where(d => (d.EvalNo == null ? "" : d.EvalNo.ToUpper()).Contains(jobid.ToUpper().Trim()) || (d.HWNo == null ? "" : d.HWNo) == jobid.Trim() || (d.SWNo == null ? "" : d.SWNo) == jobid.Trim()).ToList();
            }
            if (customer.Trim() != "") 
            {
                data = data.Where(d => (d.Customer == null ? "" : d.Customer).ToUpper().Contains(customer.ToUpper().Trim())).ToList();
            }
            if (computetime == true)
            {
                foreach (JobTracker j in data)
                {

                    if (j != null)
                    {
                        j.ComputeHours();
                    }
                }
            }
            return data;
        }

        public List<JobTracker> GetJobTrackerList(DateTime startdate, DateTime enddate,int jobtypeid,string status, int userid = 0, int dept = 0, string jobid = "", string customer = "", bool computetime = false)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.ScheduleDate >= startdate
                        && j.EndTime <= enddate
                        && j.Status == status
                        && j.JobTypeId == jobtypeid
                        orderby j.StartTime ascending, j.EndTime ascending
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
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname,
                            Customer = j.Customer,
                            EvalNo = j.EvalNo,
                            departmentid = j.M_User.DepartmentId,
                            department = j.M_User.M_Department.Description
                        }).ToList();

            db.Dispose();

            if (userid != 0)
            {
                data = data.Where(d => d.UserId == userid).ToList();
            }
            if (dept != 0)
            {
                data = data.Where(d => d.departmentid == dept).ToList();
            }
            if (jobid.Trim() != "")
            {
                data = data.Where(d => d.EvalNo.ToUpper().Contains(jobid.ToUpper().Trim()) || d.HWNo == jobid.Trim() || d.SWNo == jobid.Trim()).ToList();
            }
            if (customer.Trim() != "")
            {
                data = data.Where(d => d.Customer.ToUpper().Contains(customer.ToUpper().Trim())).ToList();
            }
            if (computetime == true)
            {
                foreach (JobTracker j in data)
                {
                    if (j != null)
                    {
                        j.ComputeHours();
                    }
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

            
            for (int i = 0; i < data.Count; i++) 
            {
                if (data[i].HWNo.ToString().Contains(stringjobid) || data[i].SWNo.ToString().Contains(stringjobid))
                {
                    //data[i].GetCustomer(data[i]);
                    string hwno = data[i].HWNo;
                    string swno = data[i].SWNo;
                    var jt = db.T_JobTracker.First(j => j.HWNo == hwno && j.SWNo == swno);
                    data[i].Customer = jt.Customer;
                    data[i].Description = jt.Description;
                    data[i].EvalNo = jt.EvalNo;
                }
                else 
                {
                    data.RemoveAt(i);
                    i--;
                }
            }
            db.Dispose();
            return data;
        }

        public List<JobTracker> GetDistinctProjectListIncludingForApproval(DateTime sdate, DateTime edate, string stringjobid = "")
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where (j.HWNo != ""
                        || j.SWNo != "")
                        &&
                        ((j.StartTime >= sdate
                        && j.StartTime <= edate)
                        ||
                        (j.EndTime >= sdate && j.EndTime <= edate))
                        && (j.Status == "Approved" || j.Status == "For Approval")
                        && j.JobIdNumber != ""
                        && j.M_JobType.ComputeTime == true
                        orderby j.StartTime ascending
                        select new JobTracker()
                        {
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                        }).Distinct().ToList();


            for (int i = 0; i < data.Count; i++)
            {
                if ((data[i].HWNo != null && data[i].HWNo.ToString() != "") ||(data[i].SWNo != null && data[i].SWNo.ToString() != ""))
                {
                    //data[i].GetCustomer(data[i]);
                    string hwno = data[i].HWNo;
                    string swno = data[i].SWNo;
                    var jt = db.T_JobTracker.FirstOrDefault(j => j.HWNo == hwno && j.SWNo == swno && j.EvalNo != "" && j.EvalNo != null);
                    if (jt != null)
                    {
                        data[i].Customer = jt.Customer;
                        data[i].Description = jt.Description;
                        data[i].EvalNo = jt.EvalNo;
                    }
                }
                else
                {
                    data.RemoveAt(i);
                    i--;
                }
            }
            db.Dispose();
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

        public List<JobTracker> GetRequestNeededApproval(int userid, bool computetime)
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
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname, //Requestor
                            Customer = j.Customer,
                            EvalNo = j.EvalNo
                        }).ToList();

            db.Dispose();

            if (computetime == true)
            {
                foreach (JobTracker j in data)
                {
                    if (j != null)
                    {
                        j.ComputeHours();
                    }
                }
            }


            return data;
        }

        public List<JobTracker> GetPendingRequest(int userid, bool computetime) 
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
                            fullname = j.M_Supervisor.Firstname + " " + j.M_Supervisor.Lastname, //Supervisor
                            Customer = j.Customer,
                            EvalNo = j.EvalNo
                        }).ToList();

            db.Dispose();

            if (computetime == true)
            {
                foreach (JobTracker j in data)
                {
                    if (j != null)
                    {
                        j.ComputeHours();
                    }
                }
            }


            return data;
        
        }

        public List<JobTracker> GetRejectedRequest(int userid, bool computetime)
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
                            fullname = j.M_Supervisor.Firstname + " " + j.M_Supervisor.Lastname, //Supervisor
                            Customer = j.Customer,
                            EvalNo = j.EvalNo
                        }).Take(10).ToList();

            db.Dispose();

            if (computetime == true)
            {
                foreach (JobTracker j in data)
                {
                    if (j != null)
                    {
                        j.ComputeHours();
                    }
                }
            }


            return data;

        }

        public JobTracker GetOldestUnclosedJob(int userid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.ApprovedBy == userid
                        && j.Status == "Pending"
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
                            SWNo = j.SWNo,
                            HWNo = j.HWNo,
                            JobStatus = j.JobStatus,
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname,
                            Customer = j.Customer,
                            EvalNo = j.EvalNo
                        }).FirstOrDefault();

            db.Dispose();

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
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname,
                            Customer = j.Customer,
                            EvalNo = j.EvalNo
                        }).ToList();

            db.Dispose();

            //foreach (JobTracker j in data)
            //{
            //    if (j.JobIdNumber != null && j.JobIdNumber != "")
            //    {
            //        GetCustomer(j);
            //    }

            //}


            return data;
        }

        public List<JobTracker> GetUnclosedJobs(int userid, DateTime selecteddate, bool computetime)
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
                            fullname = j.M_User.Firstname + " " + j.M_User.Lastname,
                            Customer = j.Customer,
                            EvalNo = j.EvalNo
                        }).ToList();

            db.Dispose();

            if (computetime == true)
            {
                foreach (JobTracker j in data)
                {
                    if (j != null)
                    {
                        j.ComputeHours();
                    }
                }
            }


            return data;
        }

        public List<JobTracker> GetJobTrackerListWithEndTimeAndNotRejected(int userid, DateTime selecteddate)
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
            return data;
        }

        public List<JobTracker> GetJobTrackerListWithEndTimeAndNotRejected(int userid, DateTime startTime,DateTime endTime)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.UserId == userid
                        && j.StartTime >= startTime
                        && j.StartTime < endTime
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
            return data;
        }

        //private JobTracker GetCustomer(string jobid) 
        //{
        //    JobTracker jobTracker = new JobTracker();
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
        //    {
        //        SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + jobid.Trim() + "'", con);
        //        con.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            jobTracker.Customer = reader["CO_Name"].ToString();
        //            jobTracker.Description = reader["SO_PCBdesc"].ToString();
        //        }
        //    }
        //    if (jobTracker.Customer == null || jobTracker.Customer == "")
        //    {
        //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
        //        {
        //            SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + jobid.Trim() + "'", con);
        //            con.Open();
        //            SqlDataReader reader = cmd.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                jobTracker.Customer = reader["CO_Name"].ToString();
        //                jobTracker.Description = reader["SO_PCBdesc"].ToString();
        //            }
        //        }
        //    }

        //    return jobTracker;
        //}

        //private void GetCustomer(JobTracker jobtracker) 
        //{
        //    if (jobtracker.HWNo != null && jobtracker.HWNo.Trim() != "")
        //    {
        //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
        //        {
        //            SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + jobtracker.HWNo.Trim() + "'", con);
        //            con.Open();
        //            SqlDataReader reader = cmd.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                jobtracker.Customer = reader["CO_Name"].ToString();
        //                jobtracker.Description = reader["SO_PCBdesc"].ToString();
        //            }
        //        }
        //    }
        //    else if ((jobtracker.SWNo != null && jobtracker.SWNo.Trim() != "") && (jobtracker.Customer == null || jobtracker.Customer.Trim() == "")) 
        //    {
        //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
        //        {
        //            SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + jobtracker.SWNo.Trim() + "'", con);
        //            con.Open();
        //            SqlDataReader reader = cmd.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                jobtracker.Customer = reader["CO_Name"].ToString();
        //                jobtracker.Description = reader["SO_PCBdesc"].ToString();
        //            }
        //        }
        //    }
        //}

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
            data.Customer = jobtracker.Customer;
            data.EvalNo = jobtracker.EvalNo;
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
            t_jobtracker.Customer = jobtracker.Customer;
            t_jobtracker.EvalNo = jobtracker.EvalNo;
        }

        public bool HasUnclosedJobs(int userid) 
        {
            bool result = true;
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.UserId == userid
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

        public bool HasPreviousUnclosedJobs(int userid,DateTime? currentDate)
        {
            bool result = true;
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTracker
                        where j.UserId == userid
                        && j.Status == "Pending"
                        && j.ScheduleDate != currentDate
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

        public JobTracker GenerateHWAndSWOld2(string jobidnumber) 
        {
            JobTracker jobtracker = new JobTracker();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("Select TOP 1 SO_Num,CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + jobidnumber.Trim() + "'", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    jobtracker.Customer = reader["CO_Name"].ToString();
                    jobtracker.Description = reader["SO_PCBdesc"].ToString();
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
                        jobtracker.Customer = reader["CO_Name"].ToString();
                        jobtracker.Description = reader["SO_PCBdesc"].ToString();
                        jobtracker.SWNo = reader["SO_Num"].ToString();
                    }
                }
                if (jobtracker.SWNo != null && jobtracker.SWNo.Trim() != "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
                    {
                        SqlCommand cmd = new SqlCommand("Select TOP 1 SO_Num from Sales_Order SO where '" + jobtracker.Description.Trim() + "' Like '%'+CAST(SO_Num as varchar(20))+'%' ORDER BY SO_NUM Desc", con);
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
                    SqlCommand cmd = new SqlCommand("Select TOP 1 SO_Num from Sales_Order SO where '" + jobtracker.Description.Trim() + "' Like '%'+CAST(SO_Num as varchar(20))+'%' ORDER BY SO_NUM Desc", con);
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

        public JobTracker GenerateHWAndSWOld(string jobidnumber)
        {
            JobTracker jobtracker = new JobTracker();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("Select TOP 1 SO_Num,CO_Name,SO.SO_PCBdesc,SO_TotalSolSO,SO.SO_EvalNo from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + jobidnumber.Trim() + "'", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    jobtracker.Customer = reader["CO_Name"].ToString();
                    jobtracker.Description = reader["SO_PCBdesc"].ToString();
                    jobtracker.HWNo = reader["SO_Num"].ToString();
                    jobtracker.SWNo = reader["SO_TotalSolSO"].ToString();
                    jobtracker.EvalNo = reader["SO_EvalNo"].ToString();
                }
            }
            if (jobtracker.HWNo == null || jobtracker.HWNo.Trim() == "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
                {
                    SqlCommand cmd = new SqlCommand("Select TOP 1 SO_Num,CO_Name,SO.SO_PCBdesc,SO_TotalSolSO,SO.SO_EvalNo from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + jobidnumber.Trim() + "'", con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        jobtracker.Customer = reader["CO_Name"].ToString();
                        jobtracker.Description = reader["SO_PCBdesc"].ToString();
                        jobtracker.SWNo = reader["SO_Num"].ToString();
                        jobtracker.HWNo = reader["SO_TotalSolSO"].ToString();
                        jobtracker.EvalNo = reader["SO_EvalNo"].ToString();
                    }
                }
            }
            return jobtracker;
        }

        public JobTracker GenerateHWAndSW(string jobidnumber)
        {
            JobTracker jobtracker = new JobTracker();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("Select TOP 1 SO_Num,CO_Name,SO.SO_PCBdesc,SO_TotalSolSO,SO.SO_EvalNo from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_EvalNo Like '%" + jobidnumber.Trim() + "%'", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    jobtracker.Customer = reader["CO_Name"].ToString();
                    jobtracker.Description = reader["SO_PCBdesc"].ToString();
                    jobtracker.HWNo = reader["SO_Num"].ToString();
                    jobtracker.SWNo = reader["SO_TotalSolSO"].ToString();
                    jobtracker.EvalNo = reader["SO_EvalNo"].ToString();
                }
            }
            if (jobtracker.HWNo == null || jobtracker.HWNo.Trim() == "")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
                {
                    SqlCommand cmd = new SqlCommand("Select TOP 1 SO_Num,CO_Name,SO.SO_PCBdesc,SO_TotalSolSO,SO.SO_EvalNo from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_EvalNo Like '%" + jobidnumber.Trim() + "%'", con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        jobtracker.Customer = reader["CO_Name"].ToString();
                        jobtracker.Description = reader["SO_PCBdesc"].ToString();
                        jobtracker.SWNo = reader["SO_Num"].ToString();
                        jobtracker.HWNo = reader["SO_TotalSolSO"].ToString();
                        jobtracker.EvalNo = reader["SO_EvalNo"].ToString();
                    }
                }
            }
            if ((jobtracker.HWNo == null || jobtracker.HWNo.Trim() == "") && (jobtracker.SWNo == null || jobtracker.SWNo.Trim() == ""))
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
                {
                    SqlCommand cmd = new SqlCommand("Select TOP 1 SO_Num,CO_Name,SO.SO_PCBdesc,SO_TotalSolSO,SO.SO_EvalNo from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + jobidnumber.Trim() + "'", con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        jobtracker.Customer = reader["CO_Name"].ToString();
                        jobtracker.Description = reader["SO_PCBdesc"].ToString();
                        jobtracker.HWNo = reader["SO_Num"].ToString();
                        jobtracker.SWNo = reader["SO_TotalSolSO"].ToString();
                        jobtracker.EvalNo = reader["SO_EvalNo"].ToString();
                    }
                }
                if (jobtracker.HWNo == null || jobtracker.HWNo.Trim() == "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
                    {
                        SqlCommand cmd = new SqlCommand("Select TOP 1 SO_Num,CO_Name,SO.SO_PCBdesc,SO_TotalSolSO,SO.SO_EvalNo from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + jobidnumber.Trim() + "'", con);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            jobtracker.Customer = reader["CO_Name"].ToString();
                            jobtracker.Description = reader["SO_PCBdesc"].ToString();
                            jobtracker.SWNo = reader["SO_Num"].ToString();
                            jobtracker.HWNo = reader["SO_TotalSolSO"].ToString();
                            jobtracker.EvalNo = reader["SO_EvalNo"].ToString();
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
            for (int i = 0; i < numberofdays; i++) 
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

        public bool CanConnectToCAP() 
        {
            bool result = true;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
            {
                try
                {
                    con.Open();
                }
                catch
                {
                    result = false;
                }
                finally 
                {
                    con.Close();
                }
            }
            return result;
        }

        private bool HasTimeGap(int userid, DateTime selecteddate) 
        {
            bool result = false;

            var data = GetJobTrackerListWithEndTimeAndNotRejected(userid, selecteddate);

            User user = new User();
            user = user.GetUser(userid,selecteddate);
            if (TimeSpan.Parse(user.startTime) > TimeSpan.Parse(user.endTime)) //for shifting hours check two time (12 midnight - cutofftime and cutooftime to 12 midnight)
            {
                int loop = 0;
                TimeSpan cutOfTime = user.GetMyCutOfTime();
                DateTime stime1 = new DateTime();
                stime1 = DateTime.Parse(selecteddate.Year + "-" + selecteddate.Month + "-" + selecteddate.Day + " 00:00:00");
                DateTime stime2 = new DateTime();
                for (int i = 0; i < data.Count; i++) 
                {
                    if (Convert.ToDateTime(data[i].StartTime).TimeOfDay < cutOfTime)
                    {
                        if (stime1 != data[i].StartTime)
                        {
                            result = true;
                            break;
                        }
                        else
                        {
                            stime1 = Convert.ToDateTime(data[i].EndTime);
                        }
                    }
                    else 
                    {
                        if (loop == 0)
                        {
                            stime2 = Convert.ToDateTime(data[i].EndTime);
                        }
                        else if (stime2 != data[i].StartTime)
                        {
                            result = true;
                            break;
                        }
                        else
                        {
                            stime2 = Convert.ToDateTime(data[i].EndTime);
                        }
                    }
                }
            }
            else
            {
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
            }

            return result;
        }

        private bool HasTimeClockGap(int userid, DateTime selecteddate) 
        {
            //Checks if the user has a gap between his facetime time and the time key in, in jobtrack
            bool result = false;
            User user = new User();
            user = user.GetUser(userid,selecteddate);
            if (user.EmployeeNumber != 0) 
            {
                if (TimeSpan.Parse(user.startTime) > TimeSpan.Parse(user.endTime)) //user with shifting hour 
                {
                    TimeClock timeclock1 = new TimeClock();
                    TimeClock timeclock2 = new TimeClock();
                    TimeSpan cutOffTime = user.GetMyCutOfTime();
                    
                    timeclock2 = timeclock2.GetStartEndTimeForShifting(Convert.ToInt32(user.EmployeeNumber), Convert.ToDateTime(selecteddate.AddDays(-1).ToString("yyyy-MM-dd") + " " + cutOffTime.ToString("hh\\:mm\\:ss")), Convert.ToDateTime(selecteddate.ToString("yyyy-MM-dd") + " " + cutOffTime.ToString("hh\\:mm\\:ss")), false);
                    if (timeclock2 != null)
                    {
                        var data = GetJobTrackerListWithEndTimeAndNotRejected(userid, Convert.ToDateTime(selecteddate.ToString("yyyy-MM-dd") + " 00:00:00"), Convert.ToDateTime(selecteddate.ToString("yyyy-MM-dd") + " " + cutOffTime.ToString("hh\\:mm\\:ss")));
                        if (data.Count > 0)
                        {
                            if (timeclock2.starttime < Convert.ToDateTime(data[0].StartTime))
                                result = true;
                            if ((timeclock2.endtime.TimeOfDay < new TimeSpan(0,30,0) ? timeclock1.endtime : timeclock2.endtime.AddMinutes(-30)) > Convert.ToDateTime(data[data.Count - 1].EndTime))
                                result = true;
                        }
                        else
                        {
                            result = true;
                        }
                    }
                    if (result == false)
                    {
                        timeclock1 = timeclock1.GetStartEndTimeForShifting(Convert.ToInt32(user.EmployeeNumber), Convert.ToDateTime(selecteddate.ToString("yyyy-MM-dd") + " " + cutOffTime.ToString("hh\\:mm\\:ss")), Convert.ToDateTime(selecteddate.AddDays(1).ToString("yyyy-MM-dd") + " " + cutOffTime.ToString("hh\\:mm\\:ss")), true);
                        if (timeclock1 != null)
                        {
                            var data = GetJobTrackerListWithEndTimeAndNotRejected(userid, Convert.ToDateTime(selecteddate.ToString("yyyy-MM-dd") + " " + cutOffTime.ToString("hh\\:mm\\:ss")), Convert.ToDateTime(selecteddate.AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00"));
                            if (data.Count > 0)
                            {
                                if (timeclock1.starttime.AddMinutes(30) < Convert.ToDateTime(data[0].StartTime))
                                    result = true;
                                if ((timeclock1.endtime.TimeOfDay < new TimeSpan(0, 30, 0) ? timeclock1.endtime : timeclock1.endtime.AddMinutes(-30)) > Convert.ToDateTime(data[data.Count - 1].EndTime))
                                    result = true;
                            }
                            else
                            {
                                result = true;
                            }
                        }
                    }
                    
                }
                else
                {
                    TimeClock timeclock = new TimeClock();
                    timeclock = timeclock.GetStartEndTime(Convert.ToInt32(user.EmployeeNumber), Convert.ToDateTime(selecteddate.ToString("dd MMM yyyy") + " 00:00:00"), Convert.ToDateTime(selecteddate.ToString("dd MMM yyyy") + " 23:59:59"));

                    if (timeclock != null)
                    {
                        var data = GetJobTrackerListWithEndTimeAndNotRejected(userid,selecteddate);
                        if (data.Count > 0)
                        {
                            //TimeSetting timesetting = new TimeSetting();
                            //timesetting = timesetting.GetTimeSetting();
                            //if (timeclock.starttime.AddMinutes(timesetting.Interval) < Convert.ToDateTime(data[0].StartTime))
                            if (timeclock.starttime.AddMinutes(30) < Convert.ToDateTime(data[0].StartTime))
                                result = true;
                            //if (timeclock.endtime.AddMinutes(-1 * timesetting.Interval) > Convert.ToDateTime(data[data.Count - 1].EndTime))
                            if (timeclock.endtime.AddMinutes(-30) > Convert.ToDateTime(data[data.Count - 1].EndTime))
                                result = true;
                        }
                        else
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        private void ComputeHours() 
        {
            User user = new User();
            user = user.GetUser(Convert.ToInt32(UserId), Convert.ToDateTime(StartTime));
            if (EndTime != null)
            {
                DateTime startdatetime = Convert.ToDateTime(StartTime);
                TimeSpan stime = startdatetime.TimeOfDay;
                TimeSpan etime = Convert.ToDateTime(EndTime).TimeOfDay;
                int curday = (int)startdatetime.DayOfWeek;
                Holiday holiday = new Holiday();

                otcost = 0;
                normalcost = 0;

                double time = Convert.ToDateTime(EndTime).Subtract(Convert.ToDateTime(StartTime)).TotalMinutes;
                if (user.shifting == false)
                {
                    if (user.usePattern == true)
                    {
                        string[] pattern = user.offPattern.Split(',');
                        int patterndays = 0;
                        List<bool> isOffdays = new List<bool>();
                        for (int i = 0; i < pattern.Length; i++)
                        {
                            patterndays += Convert.ToInt32(pattern[i]);
                            for (int j = 0; j < Convert.ToInt32(pattern[i]); j++)
                            {
                                if (i % 2 == 0)
                                    isOffdays.Add(true);
                                else
                                    isOffdays.Add(false);
                            }
                        }
                        int offdayindex = 0;
                        offdayindex = Convert.ToInt32(Math.Floor((startdatetime.Date - user.patternStartDate.Date).TotalDays)) % patterndays;
                        if (TimeSpan.Parse(user.endTime) <= stime || holiday.IsHoliday(startdatetime.Date) || isOffdays[offdayindex] == true) //entry is OT
                        {
                            otmins = time;
                        }
                        else if ((etime == new TimeSpan(0, 0, 0) ? new TimeSpan(1, 0, 0, 0) : etime) > TimeSpan.Parse(user.endTime)) //entry is from normal time to OT
                        {
                            normalmins = TimeSpan.Parse(user.endTime).Subtract(stime).TotalMinutes;
                            otmins = (etime == new TimeSpan(0, 0, 0) ? new TimeSpan(1, 0, 0, 0) : etime).Subtract(TimeSpan.Parse(user.endTime)).TotalMinutes;
                        }
                        else //entry is within normal time
                        {
                            normalmins = time;
                        }
                    }
                    else
                    {
                        if (TimeSpan.Parse(user.endTime) <= stime || holiday.IsHoliday(startdatetime.Date) || curday == user.currentOffDay || curday == user.currentSpecialOffDay || curday == user.currentOptOffDay1 || curday == user.currentOptOffDay2 || curday == user.currentOptOffDay3 || curday == user.currentOptOffDay4) //entry is OT
                        {
                            otmins = time;
                        }
                        else if ((etime == new TimeSpan(0, 0, 0) ? new TimeSpan(1, 0, 0, 0) : etime) > TimeSpan.Parse(user.endTime)) //entry is from normal time to OT
                        {
                            normalmins = TimeSpan.Parse(user.endTime).Subtract(stime).TotalMinutes;
                            otmins = (etime == new TimeSpan(0, 0, 0) ? new TimeSpan(1, 0, 0, 0) : etime).Subtract(TimeSpan.Parse(user.endTime)).TotalMinutes;
                        }
                        else //entry is within normal time
                        {
                            normalmins = time;
                        }
                    }
                }
                else
                {
                    TimeSpan cutoff = user.GetMyCutOfTime();
                    if (user.usePattern == true)
                    {
                        string[] pattern = user.offPattern.Split(',');
                        int patterndays = 0;
                        List<bool> isOffdays = new List<bool>();
                        for (int i = 0; i < pattern.Length; i++)
                        {
                            patterndays += Convert.ToInt32(pattern[i]);
                            for (int j = 0; j < Convert.ToInt32(pattern[i]); j++)
                            {
                                if (i % 2 == 0)
                                    isOffdays.Add(true);
                                else
                                    isOffdays.Add(false);
                            }
                        }
                        int offdayindex = 0;

                        if (stime >= cutoff)
                        {
                            offdayindex = Convert.ToInt32(Math.Floor((startdatetime.Date - user.patternStartDate.Date).TotalDays)) % patterndays;
                            if (holiday.IsHoliday(startdatetime.Date) || isOffdays[offdayindex] == true)
                                otmins = time;
                            else
                                normalmins = time;
                        }
                        else 
                        {
                            offdayindex = Convert.ToInt32(Math.Floor((startdatetime.AddDays(-1).Date - user.patternStartDate.Date).TotalDays)) % patterndays;
                            if (TimeSpan.Parse(user.endTime) <= stime || holiday.IsHoliday(startdatetime.AddDays(-1).Date) || isOffdays[offdayindex] == true)
                            {
                                otmins = time;
                            }
                            else if ((etime == new TimeSpan(0, 0, 0) ? new TimeSpan(1, 0, 0, 0) : etime) > TimeSpan.Parse(user.endTime))
                            {
                                normalmins = TimeSpan.Parse(user.endTime).Subtract(stime).TotalMinutes;
                                otmins = (etime == new TimeSpan(0, 0, 0) ? new TimeSpan(1, 0, 0, 0) : etime).Subtract(TimeSpan.Parse(user.endTime)).TotalMinutes;
                            }
                        }
                    }
                    else
                    {
                        if (stime >= cutoff)
                        {
                            if (holiday.IsHoliday(startdatetime.Date) || curday == user.currentOffDay || curday == user.currentSpecialOffDay || curday == user.currentOptOffDay1 || curday == user.currentOptOffDay2 || curday == user.currentOptOffDay3 || curday == user.currentOptOffDay4)
                                otmins = time;
                            else
                                normalmins = time;
                        }
                        else
                        {
                            curday = (int)startdatetime.AddDays(-1).DayOfWeek;
                            if (TimeSpan.Parse(user.endTime) <= stime || holiday.IsHoliday(startdatetime.AddDays(-1).Date) || curday == user.currentOffDay || curday == user.currentSpecialOffDay || curday == user.currentOptOffDay1 || curday == user.currentOptOffDay2 || curday == user.currentOptOffDay3 || curday == user.currentOptOffDay4) //entry is OT
                            {
                                otmins = time;
                            }
                            else if ((etime == new TimeSpan(0, 0, 0) ? new TimeSpan(1, 0, 0, 0) : etime) > TimeSpan.Parse(user.endTime))
                            {
                                normalmins = TimeSpan.Parse(user.endTime).Subtract(stime).TotalMinutes;
                                otmins = (etime == new TimeSpan(0, 0, 0) ? new TimeSpan(1, 0, 0, 0) : etime).Subtract(TimeSpan.Parse(user.endTime)).TotalMinutes;
                            }
                        }
                    }
                }
                double hr = Math.Truncate(time / 60);
                double min = time % 60;
                totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");

                hr = Math.Truncate(normalmins / 60);
                min = normalmins % 60;
                normalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");

                hr = Math.Truncate(otmins / 60);
                min = otmins % 60;
                othours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 && min > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
            
            }
        }

        private void ComputeCost() 
        {
            if (EndTime != null)
            {
                User user = new User();
                user = user.GetUser(Convert.ToInt32(UserId), Convert.ToDateTime(StartTime));
                Holiday holiday = new Holiday();
                OTRateSetting otRateSetting = new OTRateSetting();
                DateTime startdatetime = Convert.ToDateTime(StartTime);
                //int workingdays = holiday.GetWorkingDaysInMonth(user.Id, startdatetime);
                double normalRatePerMin = 0;
                //double normalRatePerMin = Convert.ToDouble((user.currentSalary/workingdays) / user.minsWorkPerDay);
                if (user.minsWorkPerDay < 490)
                    normalRatePerMin = Convert.ToDouble((user.currentSalary * 12) / (2080 * 60));
                else
                    normalRatePerMin = Convert.ToDouble((user.currentSalary * 12) / (2184 * 60));
                normalcost = normalmins * normalRatePerMin;
                otRateSetting = otRateSetting.GetOTRateSettingByDate(startdatetime);

                #region OT COMPUTATION
                otcost = 0;
                if (!user.noOTpay && otRateSetting != null)
                {
                    if (user.currentSalary >= otRateSetting.OTExemptedSalary) //User is Exempted from Salary
                    {
                        otcost = otmins * Convert.ToDouble((otRateSetting.ExemptedSalaryIncentive / 60));
                    }
                    else
                    {
                        if (user.shifting == false)
                        {
                            if (user.usePattern == true)
                            {
                                if (holiday.IsHoliday(startdatetime.Date))
                                {
                                    otcost = otmins * (normalRatePerMin * otRateSetting.OTSpecialRate);
                                }
                                else
                                {
                                    otcost = otmins * (normalRatePerMin * otRateSetting.OTNormalRate);
                                }
                            }
                            else
                            {
                                if (holiday.IsHoliday(startdatetime.Date) || user.isOfficeWorker && user.currentSpecialOffDay == (int)startdatetime.DayOfWeek)
                                {
                                    otcost = otmins * (normalRatePerMin * otRateSetting.OTSpecialRate);
                                }
                                else
                                {
                                    otcost = otmins * (normalRatePerMin * otRateSetting.OTNormalRate);
                                }
                            }
                        }
                        else //For Shifting Hours
                        {
                            TimeSpan stime = startdatetime.TimeOfDay;
                            TimeSpan etime = Convert.ToDateTime(EndTime).TimeOfDay;
                            TimeSpan cutoff = user.GetMyCutOfTime();

                            if (user.usePattern == true)
                            {
                                if (user.offPattern.Trim() != "") 
                                {
                                    
                                    if (stime >= cutoff)
                                    {
                                        if (holiday.IsHoliday(startdatetime.Date))
                                        {
                                            otcost = otmins * (normalRatePerMin * otRateSetting.OTSpecialRate);
                                        }
                                        else
                                        {
                                            otcost = otmins * (normalRatePerMin * otRateSetting.OTNormalRate);
                                        }
                                    }
                                    else
                                    {
                                        if (holiday.IsHoliday(startdatetime.AddDays(-1).Date))
                                        {
                                            otcost = otmins * (normalRatePerMin * otRateSetting.OTSpecialRate);
                                        }
                                        else
                                        {
                                            otcost = otmins * (normalRatePerMin * otRateSetting.OTNormalRate);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (stime >= cutoff)
                                {
                                    if (holiday.IsHoliday(startdatetime.Date) || (user.isOfficeWorker && user.currentSpecialOffDay == (int)startdatetime.DayOfWeek))
                                    {
                                        otcost = otmins * (normalRatePerMin * otRateSetting.OTSpecialRate);
                                    }
                                    else
                                    {
                                        otcost = otmins * (normalRatePerMin * otRateSetting.OTNormalRate);
                                    }
                                }
                                else
                                {
                                    if (holiday.IsHoliday(startdatetime.AddDays(-1).Date) || user.isOfficeWorker && user.currentSpecialOffDay == (int)startdatetime.AddDays(-1).DayOfWeek)
                                    {
                                        otcost = otmins * (normalRatePerMin * otRateSetting.OTSpecialRate);
                                    }
                                    else
                                    {
                                        otcost = otmins * (normalRatePerMin * otRateSetting.OTNormalRate);
                                    }
                                }
                            }
                        }
                    }
                    normalcost = Math.Round(normalcost, 2, MidpointRounding.AwayFromZero);
                    otcost = Math.Round(otcost, 2, MidpointRounding.AwayFromZero);
                }
                #endregion
            }
        }
    }
}