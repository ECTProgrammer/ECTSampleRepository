using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class Department : T_Department
    {
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
                            Position = d.Position
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

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
                            Position = d.Position
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<Department> GetJobOverviewDepartment() 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from d in db.T_Department
                        where d.M_JobTypes.FirstOrDefault(j => j.ShowInJobOverview == true) != null
                        orderby d.Position ascending
                        select new Department()
                        {
                            Id = d.Id,
                            Description = d.Description,
                            CreateDate = d.CreateDate,
                            LastUpdateDate = d.LastUpdateDate,
                            CreatedBy = d.CreatedBy,
                            LastUpdatedBy = d.LastUpdatedBy,
                            Position = d.Position
                        }).ToList();

            db.Dispose();

            return data;
        }

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

        private T_Department InsertParse(Department department)
        {
            T_Department t_department = new T_Department();
            t_department.Description = department.Description;
            t_department.CreateDate = department.CreateDate;
            t_department.LastUpdateDate = department.LastUpdateDate;
            t_department.CreatedBy = department.CreatedBy;
            t_department.LastUpdatedBy = department.LastUpdatedBy;
            t_department.Position = department.Position;
            return t_department;
        }

        private void UpdateParse(T_Department t_department, Department department) 
        {
            t_department.Description = department.Description;
            t_department.LastUpdateDate = department.LastUpdateDate;
            t_department.LastUpdatedBy = department.LastUpdatedBy;
            t_department.Position = department.Position;
        }
    }
}