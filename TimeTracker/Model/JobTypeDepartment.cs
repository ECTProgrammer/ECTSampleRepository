using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class JobTypeDepartment : T_JobTypeDepartment
    {
        public string jobtype { get; set; }
        public bool? requiredJobId { get; set; }
        public bool? computeTime { get; set; }
        public bool? showInJobOverview { get; set; }
        public string jobtypeAcronym { get; set; }
        public string department { get; set; }
        public string departmentAcronym { get; set; }

        public JobTypeDepartment GetJobTypeDepartment(int id)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTypeDepartment
                        where j.Id == id
                        select new JobTypeDepartment()
                        {
                            Id = j.Id,
                            JobTypeId = j.JobTypeId,
                            DepartmentId = j.DepartmentId,
                            Position = j.Position,
                            jobtype = j.M_JobType.Description,
                            requiredJobId = j.M_JobType.RequiredJobId,
                            computeTime = j.M_JobType.ComputeTime,
                            showInJobOverview = j.M_JobType.ShowInJobOverview,
                            jobtypeAcronym = j.M_JobType.Acronym,
                            department = j.M_Department.Description,
                            departmentAcronym = j.M_Department.Acronym
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public List<JobTypeDepartment> GetJobTypeDepartmentListByJobType(int jobtypeid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTypeDepartment
                        where j.JobTypeId == jobtypeid
                        orderby j.Position
                        select new JobTypeDepartment()
                        {
                            Id = j.Id,
                            JobTypeId = j.JobTypeId,
                            DepartmentId = j.DepartmentId,
                            Position = j.Position,
                            jobtype = j.M_JobType.Description,
                            requiredJobId = j.M_JobType.RequiredJobId,
                            computeTime = j.M_JobType.ComputeTime,
                            showInJobOverview = j.M_JobType.ShowInJobOverview,
                            jobtypeAcronym = j.M_JobType.Acronym,
                            department = j.M_Department.Description,
                            departmentAcronym = j.M_Department.Acronym
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<JobTypeDepartment> GetJobTypeDepartmentListByDepartment(int departmentid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTypeDepartment
                        where j.JobTypeId == departmentid
                        orderby j.Position
                        select new JobTypeDepartment()
                        {
                            Id = j.Id,
                            JobTypeId = j.JobTypeId,
                            DepartmentId = j.DepartmentId,
                            jobtype = j.M_JobType.Description,
                            requiredJobId = j.M_JobType.RequiredJobId,
                            computeTime = j.M_JobType.ComputeTime,
                            showInJobOverview = j.M_JobType.ShowInJobOverview,
                            jobtypeAcronym = j.M_JobType.Acronym,
                            department = j.M_Department.Description,
                            departmentAcronym = j.M_Department.Acronym
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<JobType> GetJobTypeList(int departmentid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTypeDepartment
                        where j.DepartmentId == departmentid
                        orderby j.M_Department.Position, j.Position
                        select new JobType()
                        {
                            Id = j.JobTypeId,
                            Description = j.M_JobType.Description,
                            CreatedBy = j.M_JobType.CreatedBy,
                            LastUpdatedBy = j.M_JobType.LastUpdatedBy,
                            CreateDate = j.M_JobType.CreateDate,
                            LastUpdateDate = j.M_JobType.LastUpdateDate,
                            RequiredJobId = j.M_JobType.RequiredJobId,
                            ComputeTime = j.M_JobType.ComputeTime,
                            Position = j.Position,
                            ShowInJobOverview = j.M_JobType.ShowInJobOverview,
                            Acronym = j.M_JobType.Acronym
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<JobType> GetJobOverviewJobType(int departmentid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTypeDepartment
                        where j.DepartmentId == departmentid
                        orderby j.M_Department.Position, j.Position
                        select new JobType()
                        {
                            Id = j.JobTypeId,
                            Description = j.M_JobType.Description,
                            CreatedBy = j.M_JobType.CreatedBy,
                            LastUpdatedBy = j.M_JobType.LastUpdatedBy,
                            CreateDate = j.M_JobType.CreateDate,
                            LastUpdateDate = j.M_JobType.LastUpdateDate,
                            RequiredJobId = j.M_JobType.RequiredJobId,
                            ComputeTime = j.M_JobType.ComputeTime,
                            Position = j.Position,
                            ShowInJobOverview = j.M_JobType.ShowInJobOverview,
                            Acronym = j.M_JobType.Acronym
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<JobType> GetExclusiveJobTypeList(int departmentid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTypeDepartment
                        where j.DepartmentId == departmentid
                        orderby j.M_Department.Position, j.Position
                        select new JobType()
                        {
                            Id = j.JobTypeId,
                            Description = j.M_JobType.Description,
                            CreatedBy = j.M_JobType.CreatedBy,
                            LastUpdatedBy = j.M_JobType.LastUpdatedBy,
                            CreateDate = j.M_JobType.CreateDate,
                            LastUpdateDate = j.M_JobType.LastUpdateDate,
                            RequiredJobId = j.M_JobType.RequiredJobId,
                            ComputeTime = j.M_JobType.ComputeTime,
                            Position = j.Position,
                            ShowInJobOverview = j.M_JobType.ShowInJobOverview,
                            Acronym = j.M_JobType.Acronym
                        }).ToList();

            db.Dispose();

            return data;
        }

        public void Insert(JobTypeDepartment jobtypedepartment)
        {
            T_JobTypeDepartment t_jobtypedepartment = new T_JobTypeDepartment();
            t_jobtypedepartment.JobTypeId = jobtypedepartment.JobTypeId;
            t_jobtypedepartment.DepartmentId = jobtypedepartment.DepartmentId;
            t_jobtypedepartment.Position = jobtypedepartment.Position;

            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    db.T_JobTypeDepartment.Add(t_jobtypedepartment);
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
                    T_JobTypeDepartment t_jobtypedepartment = new T_JobTypeDepartment();
                    t_jobtypedepartment = db.T_JobTypeDepartment.FirstOrDefault(j => j.Id == id);
                    db.T_JobTypeDepartment.Remove(t_jobtypedepartment);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Update(JobTypeDepartment jobtypedepartment)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_JobTypeDepartment t_jobtypedepartment = db.T_JobTypeDepartment.FirstOrDefault(d => d.Id == jobtypedepartment.Id);
                    t_jobtypedepartment.JobTypeId = jobtypedepartment.JobTypeId;
                    t_jobtypedepartment.DepartmentId = jobtypedepartment.DepartmentId;
                    t_jobtypedepartment.Position = jobtypedepartment.Position;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }
    }
}