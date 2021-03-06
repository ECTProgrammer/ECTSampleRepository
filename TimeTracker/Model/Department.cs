﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class Department : T_Department
    {
        //Get Department by Department Id
        public Department GetDepartment(int departmentid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from d in db.T_Department
                        where d.Id == departmentid
                        select new Department()
                        {
                            Id = d.Id,
                            Description = d.Description,
                            CreateDate = d.CreateDate,
                            LastUpdateDate = d.LastUpdateDate,
                            CreatedBy = d.CreatedBy,
                            LastUpdatedBy = d.LastUpdatedBy,
                            Position = d.Position,
                            Acronym = d.Acronym
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        //Get all departments
        public List<Department> GetDepartmentList()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from d in db.T_Department
                        orderby d.Position ascending
                        select new Department()
                        {
                            Id = d.Id,
                            Description = d.Description,
                            CreateDate = d.CreateDate,
                            LastUpdateDate = d.LastUpdateDate,
                            CreatedBy = d.CreatedBy,
                            LastUpdatedBy = d.LastUpdatedBy,
                            Position = d.Position,
                            Acronym = d.Acronym
                        }).ToList();

            db.Dispose();

            return data;
        }

        //Get List of department that will be shown in Job Overview
        public List<Department> GetJobOverviewDepartment() 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from d in db.T_Department
                        where d.M_JobTypeDepartments.FirstOrDefault(j => j.M_JobType.ShowInJobOverview == true) != null
                        && d.M_JobTypeDepartments.Count > 0
                        orderby d.Position ascending
                        select new Department()
                        {
                            Id = d.Id,
                            Description = d.Description,
                            CreateDate = d.CreateDate,
                            LastUpdateDate = d.LastUpdateDate,
                            CreatedBy = d.CreatedBy,
                            LastUpdatedBy = d.LastUpdatedBy,
                            Position = d.Position,
                            Acronym = d.Acronym
                        }).ToList();

            db.Dispose();

            return data;
        }

        //Insert new department in database
        public void Insert(Department department)
        {
            T_Department t_department = InsertParse(department);
            
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    db.T_Department.Add(t_department);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        //Delete a department in the database
        public void Delete(int id)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_Department t_department = new T_Department();
                    t_department = db.T_Department.FirstOrDefault(d => d.Id == id);
                    db.T_Department.Remove(t_department);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        //Update a department in the database
        public void Update(Department department)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_Department t_department = db.T_Department.FirstOrDefault(d => d.Id == department.Id);
                    UpdateParse(t_department, department);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        //Parsing done before Inserting record in the database
        private T_Department InsertParse(Department department)
        {
            T_Department t_department = new T_Department();
            t_department.Description = department.Description;
            t_department.CreateDate = department.CreateDate;
            t_department.LastUpdateDate = department.LastUpdateDate;
            t_department.CreatedBy = department.CreatedBy;
            t_department.LastUpdatedBy = department.LastUpdatedBy;
            t_department.Position = department.Position;
            t_department.Acronym = department.Acronym;
            return t_department;
        }

        //Parsing done before Updating record in the database
        private void UpdateParse(T_Department t_department, Department department) 
        {
            t_department.Description = department.Description;
            t_department.LastUpdateDate = department.LastUpdateDate;
            t_department.LastUpdatedBy = department.LastUpdatedBy;
            t_department.Position = department.Position;
            t_department.Acronym = department.Acronym;
        }
    }
}