using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class JobFlow : T_JobFlow
    {
        public JobFlow GetJobFlow(int jobFlowId)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from f in db.T_JobFlow
                        where f.Id == jobFlowId
                        select new JobFlow()
                        {
                            Id = f.Id,
                            Description = f.Description,
                            Position = f.Position,
                            Acronym = f.Acronym
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public JobFlow GetJobFlowByDescription(string description) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from f in db.T_JobFlow
                        where f.Description == description
                        select new JobFlow()
                        {
                            Id = f.Id,
                            Description = f.Description,
                            Position = f.Position,
                            Acronym = f.Acronym
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public JobFlow GetJobFlowByAcronym(string acronym)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from f in db.T_JobFlow
                        where f.Acronym == acronym
                        select new JobFlow()
                        {
                            Id = f.Id,
                            Description = f.Description,
                            Position = f.Position,
                            Acronym = f.Acronym
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public List<JobFlow> GetJobFlowList()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from f in db.T_JobFlow
                        orderby f.Position
                        select new JobFlow()
                        {
                            Id = f.Id,
                            Description = f.Description,
                            Position = f.Position,
                            Acronym = f.Acronym
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<JobFlow> GetJobOverviewJobFlow()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from f in db.T_JobFlow
                        where f.M_JobTypeFlows.FirstOrDefault(j => j.M_JobType.ShowInJobOverview == true) != null
                        && f.M_JobTypeFlows.Count > 0
                        orderby f.Position ascending
                        select new JobFlow()
                        {
                            Id = f.Id,
                            Description = f.Description,
                            Position = f.Position,
                            Acronym = f.Acronym
                        }).ToList();

            db.Dispose();

            return data;
        }

        public void Insert(JobFlow jobflow)
        {
            T_JobFlow t_jobflow = new T_JobFlow();
            ParseJobFlow(t_jobflow,jobflow);

            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    db.T_JobFlow.Add(t_jobflow);
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
                    T_JobFlow t_jobflow = new T_JobFlow();
                    t_jobflow = db.T_JobFlow.FirstOrDefault(f => f.Id == id);
                    db.T_JobFlow.Remove(t_jobflow);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Update(JobFlow jobflow)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_JobFlow t_jobflow = db.T_JobFlow.FirstOrDefault(f => f.Id == jobflow.Id);
                    ParseJobFlow(t_jobflow, jobflow);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }


        private void ParseJobFlow(T_JobFlow t_jobflow, JobFlow jobflow)
        {
            t_jobflow.Description = jobflow.Description;
            t_jobflow.Position = jobflow.Position;
            t_jobflow.Acronym = jobflow.Acronym;
        }

    }
}