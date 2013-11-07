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

            var data = (from j in db.T_JobTrackers
                        join s in db.T_JobTypes
                        on j.JobTypeId equals s.Id
                        join u in db.T_Users
                        on j.UserId equals u.Id
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
                            jobtype = s.Description,
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
                            fullname = u.Firstname + " " + u.Lastname
                        }).FirstOrDefault();

            db.Dispose();


            if (data.JobIdNumber != null)
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
                {
                    SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + data.JobIdNumber.Trim() + "'", con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        data.customer = reader["CO_Name"].ToString();
                        data.pcbdesc = reader["SO_PCBdesc"].ToString();
                    }
                }
                if (data.customer == null || data.customer == "")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
                    {
                        SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + data.JobIdNumber.Trim() + "'", con);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            data.customer = reader["CO_Name"].ToString();
                            data.pcbdesc = reader["SO_PCBdesc"].ToString();
                        }
                    }
                }
            }

            if (data.EndTime != null)
            {
                double time = Convert.ToDateTime(data.EndTime).Subtract(Convert.ToDateTime(data.StartTime)).TotalMinutes;
                double hr = Math.Truncate(time / 60);
                double min = time % 60;
                data.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
            }

            return data;
        }

        public JobTracker GetNextUsedTime(int userid,DateTime starttime, DateTime selecteddate)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTrackers
                        join s in db.T_JobTypes
                        on j.JobTypeId equals s.Id
                        join u in db.T_Users
                        on j.UserId equals u.Id
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
                            jobtype = s.Description,
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
                            fullname = u.Firstname + " " + u.Lastname
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public List<JobTracker> GetJobTrackerList(int userid) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTrackers
                        join s in db.T_JobTypes
                        on j.JobTypeId equals s.Id
                        join u in db.T_Users
                        on j.UserId equals u.Id
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
                            jobtype = s.Description,
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
                            fullname = u.Firstname+" "+u.Lastname
                        }).ToList();

            db.Dispose();

            foreach (JobTracker j in data)
            {
                if (j.JobIdNumber != null)
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
                    {
                        SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '"+j.JobIdNumber.Trim()+"'", con);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            j.customer = reader["CO_Name"].ToString();
                            j.pcbdesc = reader["SO_PCBdesc"].ToString();
                        }
                    }
                    if (j.customer == null || j.customer == "") 
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
                        {
                            SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + j.JobIdNumber.Trim() + "'", con);
                            con.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                j.customer = reader["CO_Name"].ToString();
                                j.pcbdesc = reader["SO_PCBdesc"].ToString();
                            }
                        }
                    }
                }

                if (j.EndTime != null)
                {
                    double time = Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
                    double hr = Math.Truncate(time / 60);
                    double min = time % 60;
                    j.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
                }

            }


            return data;
        }

        public string GetTotalHours(int jobtypeid, DateTime startdate, DateTime enddate, string jobstatus,int departmentid = 0)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            List<JobTracker> data = new List<JobTracker>();
            if (departmentid > 0)
            {
                data = (from j in db.T_JobTrackers
                        join t in db.T_JobTypes
                        on j.JobTypeId equals t.Id
                        join u in db.T_Users
                        on j.UserId equals u.Id
                        where j.ScheduleDate >= startdate
                        && j.ScheduleDate <= enddate
                        && j.JobTypeId == jobtypeid
                        && j.Status == jobstatus
                        && u.DepartmentId == departmentid
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

                        }).ToList();
            }
            else 
            {
                data = (from j in db.T_JobTrackers
                        join t in db.T_JobTypes
                        on j.JobTypeId equals t.Id
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

                        }).ToList();
            }

            db.Dispose();
            double totalTime = 0;
            foreach (JobTracker j in data)
            {
                totalTime += Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
            }
            double hr = Math.Truncate(totalTime / 60);
            double min = totalTime % 60;
            string result = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
            return result;
        }

        public string GetTotalHours(int jobtypeid,int userid, DateTime startdate, DateTime enddate, string jobstatus, int departmentid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            List<JobTracker> data = new List<JobTracker>();

            data = (from j in db.T_JobTrackers
                    join t in db.T_JobTypes
                    on j.JobTypeId equals t.Id
                    join u in db.T_Users
                    on j.UserId equals u.Id
                    where j.ScheduleDate >= startdate
                    && j.ScheduleDate <= enddate
                    && j.JobTypeId == jobtypeid
                    && j.Status == jobstatus
                    && u.DepartmentId == departmentid
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

                    }).ToList();

            db.Dispose();
            double totalTime = 0;
            foreach (JobTracker j in data)
            {
                totalTime += Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
            }
            double hr = Math.Truncate(totalTime / 60);
            double min = totalTime % 60;
            string result = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
            return result;
        }

        public List<JobTracker> GetJobTrackerList(int userid,DateTime selecteddate)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTrackers
                        join s in db.T_JobTypes
                        on j.JobTypeId equals s.Id
                        join u in db.T_Users
                        on j.UserId equals u.Id
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
                            jobtype = s.Description,
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
                            fullname = u.Firstname + " " + u.Lastname
                        }).ToList();

            db.Dispose();

            foreach (JobTracker j in data)
            {
                if (j.JobIdNumber != null)
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
                    {
                        SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + j.JobIdNumber.Trim() + "'", con);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            j.customer = reader["CO_Name"].ToString();
                            j.pcbdesc = reader["SO_PCBdesc"].ToString();
                        }
                    }
                    if (j.customer == null || j.customer == "")
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
                        {
                            SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + j.JobIdNumber.Trim() + "'", con);
                            con.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                j.customer = reader["CO_Name"].ToString();
                                j.pcbdesc = reader["SO_PCBdesc"].ToString();
                            }
                        }
                    }
                }

                if (j.EndTime != null)
                {
                    double time = Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
                    double hr = Math.Truncate(time / 60);
                    double min = time % 60;
                    j.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
                }

            }


            return data;
        }

        public string GetTotalHours(int userid, string status, DateTime date,ref double totalmin)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTrackers
                        join t in db.T_JobTypes
                        on j.JobTypeId equals t.Id
                        where j.UserId == userid
                        && j.ScheduleDate == date
                        && j.Status == status
                        && t.ComputeTime == true
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
                            ScheduleDate = j.ScheduleDate
                        }).ToList();

            db.Dispose();
            double totalTime = 0;
            foreach (JobTracker j in data) 
            {
                 totalTime += Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
            }
            double hr = Math.Truncate(totalTime / 60);
            double min = totalTime % 60;
            string result = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
            totalmin = totalTime;
            return result;
        }

        public List<JobTracker> GetRequestNeededApproval(int userid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTrackers
                        join s in db.T_JobTypes
                        on j.JobTypeId equals s.Id
                        join u in db.T_Users
                        on j.UserId equals u.Id
                        where j.ApprovedBy == userid
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
                            jobtype = s.Description,
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
                            fullname = u.Firstname+" "+u.Lastname //Requestor
                        }).ToList();

            db.Dispose();

            foreach (JobTracker j in data)
            {
                if (j.JobIdNumber != null)
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
                    {
                        SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + j.JobIdNumber.Trim() + "'", con);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            j.customer = reader["CO_Name"].ToString();
                            j.pcbdesc = reader["SO_PCBdesc"].ToString();
                        }
                    }
                    if (j.customer == null || j.customer == "")
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
                        {
                            SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + j.JobIdNumber.Trim() + "'", con);
                            con.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                j.customer = reader["CO_Name"].ToString();
                                j.pcbdesc = reader["SO_PCBdesc"].ToString();
                            }
                        }
                    }
                }

                if (j.EndTime != null)
                {
                    double time = Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
                    double hr = Math.Truncate(time / 60);
                    double min = time % 60;
                    j.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
                }

            }


            return data;
        }

        public List<JobTracker> GetPendingRequest(int userid) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTrackers
                        join s in db.T_JobTypes
                        on j.JobTypeId equals s.Id
                        join u in db.T_Users
                        on j.ApprovedBy equals u.Id
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
                            jobtype = s.Description,
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
                            fullname = u.Firstname+" "+u.Lastname //Supervisor
                        }).ToList();

            db.Dispose();

            foreach (JobTracker j in data)
            {
                if (j.JobIdNumber != null)
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
                    {
                        SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + j.JobIdNumber.Trim() + "'", con);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            j.customer = reader["CO_Name"].ToString();
                            j.pcbdesc = reader["SO_PCBdesc"].ToString();
                        }
                    }
                    if (j.customer == null || j.customer == "")
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
                        {
                            SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + j.JobIdNumber.Trim() + "'", con);
                            con.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                j.customer = reader["CO_Name"].ToString();
                                j.pcbdesc = reader["SO_PCBdesc"].ToString();
                            }
                        }
                    }
                }

                if (j.EndTime != null)
                {
                    double time = Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
                    double hr = Math.Truncate(time / 60);
                    double min = time % 60;
                    j.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
                }

            }


            return data;
        
        }

        public List<JobTracker> GetRejectedRequest(int userid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTrackers
                        join s in db.T_JobTypes
                        on j.JobTypeId equals s.Id
                        join u in db.T_Users
                        on j.ApprovedBy equals u.Id
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
                            jobtype = s.Description,
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
                            fullname = u.Firstname + " " + u.Lastname //Supervisor
                        }).Take(10).ToList();

            db.Dispose();

            foreach (JobTracker j in data)
            {
                if (j.JobIdNumber != null)
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
                    {
                        SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + j.JobIdNumber.Trim() + "'", con);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            j.customer = reader["CO_Name"].ToString();
                            j.pcbdesc = reader["SO_PCBdesc"].ToString();
                        }
                    }
                    if (j.customer == null || j.customer == "")
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
                        {
                            SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + j.JobIdNumber.Trim() + "'", con);
                            con.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                j.customer = reader["CO_Name"].ToString();
                                j.pcbdesc = reader["SO_PCBdesc"].ToString();
                            }
                        }
                    }
                }

                if (j.EndTime != null)
                {
                    double time = Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
                    double hr = Math.Truncate(time / 60);
                    double min = time % 60;
                    j.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
                }

            }


            return data;

        }

        public List<JobTracker> GetUnclosedJobs(int userid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTrackers
                        join s in db.T_JobTypes
                        on j.JobTypeId equals s.Id
                        join u in db.T_Users
                        on j.UserId equals u.Id
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
                            jobtype = s.Description,
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
                            fullname = u.Firstname + " " + u.Lastname
                        }).ToList();

            db.Dispose();

            foreach (JobTracker j in data)
            {
                if (j.JobIdNumber != null && j.JobIdNumber != "")
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
                    {
                        SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + j.JobIdNumber.Trim() + "'", con);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            j.customer = reader["CO_Name"].ToString();
                            j.pcbdesc = reader["SO_PCBdesc"].ToString();
                        }
                    }
                    if (j.customer == null || j.customer == "")
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
                        {
                            SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + j.JobIdNumber.Trim() + "'", con);
                            con.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                j.customer = reader["CO_Name"].ToString();
                                j.pcbdesc = reader["SO_PCBdesc"].ToString();
                            }
                        }
                    }
                }

                if (j.EndTime != null)
                {
                    double time = Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
                    double hr = Math.Truncate(time / 60);
                    double min = time % 60;
                    j.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
                }

            }


            return data;
        }

        public List<JobTracker> GetUnclosedJobs(int userid,DateTime selecteddate)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTrackers
                        join s in db.T_JobTypes
                        on j.JobTypeId equals s.Id
                        join u in db.T_Users
                        on j.UserId equals u.Id
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
                            jobtype = s.Description,
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
                            fullname = u.Firstname + " " + u.Lastname
                        }).ToList();

            db.Dispose();

            foreach (JobTracker j in data)
            {
                if (j.JobIdNumber != null && j.JobIdNumber != "")
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
                    {
                        SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + j.JobIdNumber.Trim() + "'", con);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            j.customer = reader["CO_Name"].ToString();
                            j.pcbdesc = reader["SO_PCBdesc"].ToString();
                        }
                    }
                    if (j.customer == null || j.customer == "")
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
                        {
                            SqlCommand cmd = new SqlCommand("Select CO_Name,SO.SO_PCBdesc from Company CO, Sales_Order SO where SO.CO_ID = CO.CO_ID and SO.SO_Num = '" + j.JobIdNumber.Trim() + "'", con);
                            con.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                j.customer = reader["CO_Name"].ToString();
                                j.pcbdesc = reader["SO_PCBdesc"].ToString();
                            }
                        }
                    }
                }

                if (j.EndTime != null)
                {
                    double time = Convert.ToDateTime(j.EndTime).Subtract(Convert.ToDateTime(j.StartTime)).TotalMinutes;
                    double hr = Math.Truncate(time / 60);
                    double min = time % 60;
                    j.totalhours = hr == 0 && min == 0 ? "0 min" : (hr > 0 ? hr > 1 ? hr + " hrs" : hr + " hr" : "") + (hr > 0 ? ", " : "") + (min > 0 ? min > 1 ? min + " mins" : min + " min" : "");
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

        public void Insert(JobTracker jobtracker) 
        {
            T_JobTracker j = new T_JobTracker();
            ParseJobTracker(j, jobtracker);
            using (TimeTrackerEntities db = new TimeTrackerEntities()) 
            {
                try
                {
                    db.T_JobTrackers.Add(j);
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
                    t_jobtracker = db.T_JobTrackers.FirstOrDefault(j => j.Id == id);
                    db.T_JobTrackers.Remove(t_jobtracker);
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
                    T_JobTracker t_jobtracker = db.T_JobTrackers.FirstOrDefault(j => j.Id == jobtracker.Id);
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
        }

        public bool HasUnclosedJobs(int userid, DateTime selecteddate) 
        {
            bool result = true;
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTrackers
                        join s in db.T_JobTypes
                        on j.JobTypeId equals s.Id
                        join u in db.T_Users
                        on j.UserId equals u.Id
                        where j.ApprovedBy == userid
                        && j.Status == "Pending"
                        && j.ScheduleDate == selecteddate
                        select new JobTracker()
                        {
                            Id = j.Id,
                            UserId = j.UserId,
                            StartTime = j.StartTime,
                            EndTime = j.EndTime,
                            Description = j.Description,
                            JobTypeId = j.JobTypeId,
                            JobIdNumber = j.JobIdNumber,
                            jobtype = s.Description,
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
                            fullname = u.Firstname + " " + u.Lastname
                        }).FirstOrDefault();

            db.Dispose();

            if (data == null)
                result = false;

            return result;
        }

    }
}