﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class JobType : T_JobType
    {

        public JobType GetJobType(int jobtypeid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobType
                        where j.Id == jobtypeid
                        select new JobType()
                        {
                            Id = j.Id,
                            Description = j.Description,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            RequiredJobId = j.RequiredJobId,
                            ComputeTime = j.ComputeTime,
                            Position = j.Position,
                            ShowInJobOverview = j.ShowInJobOverview,
                            Acronym = j.Acronym
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public JobType GetJobTypeByDescription(string description)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobType
                        where j.Description == description
                        select new JobType()
                        {
                            Id = j.Id,
                            Description = j.Description,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            RequiredJobId = j.RequiredJobId,
                            ComputeTime = j.ComputeTime,
                            Position = j.Position,
                            ShowInJobOverview = j.ShowInJobOverview,
                            Acronym = j.Acronym
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public JobType GetJobTypeByAcronym(string acronym)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobType
                        where j.Acronym == acronym
                        select new JobType()
                        {
                            Id = j.Id,
                            Description = j.Description,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            RequiredJobId = j.RequiredJobId,
                            ComputeTime = j.ComputeTime,
                            Position = j.Position,
                            ShowInJobOverview = j.ShowInJobOverview,
                            Acronym = j.Acronym
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        //public List<JobType> GetJobTypeList(int departmentid) 
        //{
        //    TimeTrackerEntities db = new TimeTrackerEntities();

        //    var data = (from j in db.T_JobType
        //                where j.DepartmentId == 1
        //                || j.DepartmentId == departmentid
        //                orderby j.M_Department.Position, j.Position
        //                select new JobType()
        //                {
        //                    Id = j.Id,
        //                    Description = j.Description,
        //                    CreatedBy = j.CreatedBy,
        //                    LastUpdatedBy = j.LastUpdatedBy,
        //                    CreateDate = j.CreateDate,
        //                    LastUpdateDate = j.LastUpdateDate,
        //                    RequiredJobId = j.RequiredJobId,
        //                    ComputeTime = j.ComputeTime,
        //                    Position = j.Position,
        //                    ShowInJobOverview = j.ShowInJobOverview,
        //                    Acronym = j.Acronym,
        //                }).ToList();

        //    db.Dispose();

        //    return data;
        //}

        public List<JobType> GetJobTypeList()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from j in db.T_JobType
                        orderby j.Position
                        select new JobType()
                        {
                            Id = j.Id,
                            Description = j.Description,
                            CreatedBy = j.CreatedBy,
                            LastUpdatedBy = j.LastUpdatedBy,
                            CreateDate = j.CreateDate,
                            LastUpdateDate = j.LastUpdateDate,
                            RequiredJobId = j.RequiredJobId,
                            ComputeTime = j.ComputeTime,
                            Position = j.Position,
                            ShowInJobOverview = j.ShowInJobOverview,
                            Acronym = j.Acronym,
                        }).ToList();

            db.Dispose();

            return data;
        }

        //public List<JobType> GetJobOverviewJobType(int departmentid)
        //{
        //    TimeTrackerEntities db = new TimeTrackerEntities();

        //    var data = (from j in db.T_JobType
        //                where j.DepartmentId == departmentid
        //                && j.ShowInJobOverview == true
        //                orderby j.M_Department.Position, j.Position
        //                select new JobType()
        //                {
        //                    Id = j.Id,
        //                    Description = j.Description,
        //                    CreatedBy = j.CreatedBy,
        //                    LastUpdatedBy = j.LastUpdatedBy,
        //                    CreateDate = j.CreateDate,
        //                    LastUpdateDate = j.LastUpdateDate,
        //                    RequiredJobId = j.RequiredJobId,
        //                    ComputeTime = j.ComputeTime,
        //                    Position = j.Position,
        //                    ShowInJobOverview = j.ShowInJobOverview,
        //                    Acronym = j.Acronym,
        //                }).ToList();

        //    db.Dispose();

        //    return data;
        //}

        //public List<JobType> GetExclusiveJobTypeList(int departmentid)
        //{
        //    TimeTrackerEntities db = new TimeTrackerEntities();

        //    var data = (from j in db.T_JobType
        //                where j.DepartmentId == departmentid
        //                orderby j.M_Department.Position, j.Position
        //                select new JobType()
        //                {
        //                    Id = j.Id,
        //                    Description = j.Description,
        //                    CreatedBy = j.CreatedBy,
        //                    LastUpdatedBy = j.LastUpdatedBy,
        //                    CreateDate = j.CreateDate,
        //                    LastUpdateDate = j.LastUpdateDate,
        //                    RequiredJobId = j.RequiredJobId,
        //                    ComputeTime = j.ComputeTime,
        //                    Position = j.Position,
        //                    Acronym = j.Acronym,
        //                    ShowInJobOverview = j.ShowInJobOverview
        //                }).ToList();

        //    db.Dispose();

        //    return data;
        //}

        public void Insert(JobType jobtype)
        {
            T_JobType t_jobtype = InsertParse(jobtype);

            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    db.T_JobType.Add(t_jobtype);
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
                    T_JobType t_jobtype = new T_JobType();
                    t_jobtype = db.T_JobType.FirstOrDefault(j => j.Id == id);
                    db.T_JobType.Remove(t_jobtype);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Update(JobType jobtype)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_JobType t_jobtype = db.T_JobType.FirstOrDefault(d => d.Id == jobtype.Id);
                    UpdateParse(t_jobtype, jobtype);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        private T_JobType InsertParse(JobType jobtype)
        {
            T_JobType t_jobtype = new T_JobType();
            t_jobtype.Description = jobtype.Description;
            t_jobtype.CreateDate = jobtype.CreateDate;
            t_jobtype.LastUpdateDate = jobtype.LastUpdateDate;
            t_jobtype.CreatedBy = jobtype.CreatedBy;
            t_jobtype.LastUpdatedBy = jobtype.LastUpdatedBy;
            t_jobtype.RequiredJobId = jobtype.RequiredJobId;
            t_jobtype.ComputeTime = jobtype.ComputeTime;
            t_jobtype.Position = jobtype.Position;
            t_jobtype.ShowInJobOverview = jobtype.ShowInJobOverview;
            t_jobtype.Acronym = jobtype.Acronym;
            return t_jobtype;
        }

        private void UpdateParse(T_JobType t_jobtype, JobType jobtype)
        {
            t_jobtype.Description = jobtype.Description;
            t_jobtype.CreateDate = jobtype.CreateDate;
            t_jobtype.LastUpdateDate = jobtype.LastUpdateDate;
            t_jobtype.CreatedBy = jobtype.CreatedBy;
            t_jobtype.LastUpdatedBy = jobtype.LastUpdatedBy;
            t_jobtype.RequiredJobId = jobtype.RequiredJobId;
            t_jobtype.ComputeTime = jobtype.ComputeTime;
            t_jobtype.Position = jobtype.Position;
            t_jobtype.ShowInJobOverview = jobtype.ShowInJobOverview;
            t_jobtype.Acronym = jobtype.Acronym;
        }

    }
}