using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{

    public class JobTypeFlow : T_JobTypeFlow
    {
        public string jobtype { get; set; }
        public bool? requiredJobId { get; set; }
        public bool? computeTime { get; set; }
        public bool? showInJobOverview { get; set; }
        public string jobtypeAcronym { get; set; }
        public string jobflow { get; set; }
        public string jobflowAcronym { get; set; }
        public string department { get; set; }
        public string departmentAcronym { get; set; }

        public JobTypeFlow GetJobTypeFlow(int id)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTypeFlow
                        where j.Id == id
                        select new JobTypeFlow()
                        {
                            Id = j.Id,
                            JobTypeId = j.JobTypeId,
                            JobFlowId = j.JobFlowId,
                            Position = j.Position,
                            DepartmentId = j.DepartmentId,
                            jobtype = j.M_JobType.Description,
                            requiredJobId = j.M_JobType.RequiredJobId,
                            computeTime = j.M_JobType.ComputeTime,
                            showInJobOverview = j.M_JobType.ShowInJobOverview,
                            jobtypeAcronym = j.M_JobType.Acronym,
                            jobflow = j.M_JobFlow.Description,
                            jobflowAcronym = j.M_JobFlow.Acronym,
                            department = j.M_Department.Description,
                            departmentAcronym = j.M_Department.Acronym
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public JobTypeFlow GetJobTypeFlow(int jobflowid,int jobtypeid,int? departmentid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTypeFlow
                        where j.JobFlowId == jobflowid
                        && j.JobTypeId == jobtypeid
                        && j.DepartmentId == departmentid
                        select new JobTypeFlow()
                        {
                            Id = j.Id,
                            JobTypeId = j.JobTypeId,
                            JobFlowId = j.JobFlowId,
                            Position = j.Position,
                            DepartmentId = j.DepartmentId,
                            jobtype = j.M_JobType.Description,
                            requiredJobId = j.M_JobType.RequiredJobId,
                            computeTime = j.M_JobType.ComputeTime,
                            showInJobOverview = j.M_JobType.ShowInJobOverview,
                            jobtypeAcronym = j.M_JobType.Acronym,
                            jobflow = j.M_JobFlow.Description,
                            jobflowAcronym = j.M_JobFlow.Acronym,
                            department = j.M_Department.Description,
                            departmentAcronym = j.M_Department.Acronym
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public List<JobTypeFlow> GetJobTypeFlowListByJobType(int jobtypeid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTypeFlow
                        where j.JobTypeId == jobtypeid
                        orderby j.Position
                        select new JobTypeFlow()
                        {
                            Id = j.Id,
                            JobTypeId = j.JobTypeId,
                            JobFlowId = j.JobFlowId,
                            Position = j.Position,
                            DepartmentId = j.DepartmentId,
                            jobtype = j.M_JobType.Description,
                            requiredJobId = j.M_JobType.RequiredJobId,
                            computeTime = j.M_JobType.ComputeTime,
                            showInJobOverview = j.M_JobType.ShowInJobOverview,
                            jobtypeAcronym = j.M_JobType.Acronym,
                            jobflow = j.M_JobFlow.Description,
                            jobflowAcronym = j.M_JobFlow.Acronym,
                            department = j.M_Department.Description,
                            departmentAcronym = j.M_Department.Acronym
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<JobTypeFlow> GetJobTypeFlowListByJobFlow(int jobflowId)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTypeFlow
                        where j.JobFlowId == jobflowId
                        orderby j.Position
                        select new JobTypeFlow()
                        {
                            Id = j.Id,
                            JobTypeId = j.JobTypeId,
                            JobFlowId = j.JobFlowId,
                            Position = j.Position,
                            DepartmentId = j.DepartmentId,
                            jobtype = j.M_JobType.Description,
                            requiredJobId = j.M_JobType.RequiredJobId,
                            computeTime = j.M_JobType.ComputeTime,
                            showInJobOverview = j.M_JobType.ShowInJobOverview,
                            jobtypeAcronym = j.M_JobType.Acronym,
                            jobflow = j.M_JobFlow.Description,
                            jobflowAcronym = j.M_JobFlow.Acronym,
                            department = j.M_Department.Description,
                            departmentAcronym = j.M_Department.Acronym
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<JobTypeFlow> GetDepartmentListByJobTypeId(int jobtypeid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTypeFlow
                        where j.JobTypeId == jobtypeid
                        && j.DepartmentId != null
                        orderby j.Position
                        select new JobTypeFlow()
                        {
                            Id = j.Id,
                            JobTypeId = j.JobTypeId,
                            JobFlowId = j.JobFlowId,
                            Position = j.Position,
                            DepartmentId = j.DepartmentId,
                            jobtype = j.M_JobType.Description,
                            requiredJobId = j.M_JobType.RequiredJobId,
                            computeTime = j.M_JobType.ComputeTime,
                            showInJobOverview = j.M_JobType.ShowInJobOverview,
                            jobtypeAcronym = j.M_JobType.Acronym,
                            jobflow = j.M_JobFlow.Description,
                            jobflowAcronym = j.M_JobFlow.Acronym,
                            department = j.M_Department.Description,
                            departmentAcronym = j.M_Department.Acronym
                                
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<JobType> GetJobTypeList(int jobflowid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTypeFlow
                        where j.JobFlowId == jobflowid
                        orderby j.M_JobFlow.Position, j.Position
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

        public List<JobType> GetJobOverviewJobType(int jobflowid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTypeFlow
                        where j.JobFlowId == jobflowid
                        orderby j.M_JobFlow.Position, j.Position
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

        public List<JobType> GetExclusiveJobTypeList(int jobflowid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobTypeFlow
                        where j.JobFlowId == jobflowid
                        orderby j.M_JobFlow.Position, j.Position
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

        public void Insert(JobTypeFlow jobtypeflow)
        {
            T_JobTypeFlow t_jobtypeflow = new T_JobTypeFlow();
            Parse(t_jobtypeflow, jobtypeflow);

            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    db.T_JobTypeFlow.Add(t_jobtypeflow);
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
                    T_JobTypeFlow t_jobtypeflow = new T_JobTypeFlow();
                    t_jobtypeflow = db.T_JobTypeFlow.FirstOrDefault(j => j.Id == id);
                    db.T_JobTypeFlow.Remove(t_jobtypeflow);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Update(JobTypeFlow jobtypeflow)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_JobTypeFlow t_jobtypeflow = db.T_JobTypeFlow.FirstOrDefault(f => f.Id == jobtypeflow.Id);
                    Parse(t_jobtypeflow, jobtypeflow);

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        private void Parse(T_JobTypeFlow t_jobtypeflow, JobTypeFlow jobtypeflow)
        {
            t_jobtypeflow.JobTypeId = jobtypeflow.JobTypeId;
            t_jobtypeflow.JobFlowId = jobtypeflow.JobFlowId;
            t_jobtypeflow.Position = jobtypeflow.Position;
            t_jobtypeflow.DepartmentId = jobtypeflow.DepartmentId;
        }
    }
}