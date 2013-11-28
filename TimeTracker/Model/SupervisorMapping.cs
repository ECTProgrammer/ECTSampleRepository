using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class SupervisorMapping : T_SupervisorMapping
    {
        public string userfullname {get;set;}
        public string supervisorname { get; set; }
        public string subdepartment { get; set; }
        public string supdepartment { get; set; }

        public SupervisorMapping GetSupervisorMapping(int id) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from s in db.T_SupervisorMapping
                        where s.Id == id
                        select new SupervisorMapping()
                        {
                            Id = s.Id,
                            UserId = s.UserId,
                            SupervisorId = s.SupervisorId,
                            userfullname = s.M_User.Firstname+" "+s.M_User.Lastname,
                            supervisorname = s.M_Supervisor.Firstname+" "+s.M_Supervisor.Lastname,
                            subdepartment = s.M_User.M_Department.Description,
                            supdepartment = s.M_Supervisor.M_Department.Description
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public SupervisorMapping GetSupervisorMapping(int userid,int supervisorid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from s in db.T_SupervisorMapping
                        where s.UserId == userid
                        && s.SupervisorId == supervisorid
                        select new SupervisorMapping()
                        {
                            Id = s.Id,
                            UserId = s.UserId,
                            SupervisorId = s.SupervisorId,
                            userfullname = s.M_User.Firstname + " " + s.M_User.Lastname,
                            supervisorname = s.M_Supervisor.Firstname + " " + s.M_Supervisor.Lastname,
                            subdepartment = s.M_User.M_Department.Description,
                            supdepartment = s.M_Supervisor.M_Department.Description
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }
        
        public List<SupervisorMapping> GetSupervisors(int userid) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from s in db.T_SupervisorMapping
                        where s.UserId == userid
                        select new SupervisorMapping()
                        {
                            Id = s.Id,
                            UserId = s.UserId,
                            SupervisorId = s.SupervisorId,
                            userfullname = s.M_User.Firstname + " " + s.M_User.Lastname,
                            supervisorname = s.M_Supervisor.Firstname + " " + s.M_Supervisor.Lastname,
                            subdepartment = s.M_User.M_Department.Description,
                            supdepartment = s.M_Supervisor.M_Department.Description
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<SupervisorMapping> GetActiveSupervisors(int userid,int departmentId)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from s in db.T_SupervisorMapping
                        where s.UserId == userid
                        && s.M_Supervisor.Status == "Active"
                        && s.M_Supervisor.DepartmentId == departmentId
                        select new SupervisorMapping()
                        {
                            Id = s.Id,
                            UserId = s.UserId,
                            SupervisorId = s.SupervisorId,
                            userfullname = s.M_User.Firstname + " " + s.M_User.Lastname,
                            supervisorname = s.M_Supervisor.Firstname + " " + s.M_Supervisor.Lastname,
                            subdepartment = s.M_User.M_Department.Description,
                            supdepartment = s.M_Supervisor.M_Department.Description
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<SupervisorMapping> GetActiveSupervisors(int userid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from s in db.T_SupervisorMapping
                        where s.UserId == userid
                        && s.M_Supervisor.Status == "Active"
                        select new SupervisorMapping()
                        {
                            Id = s.Id,
                            UserId = s.UserId,
                            SupervisorId = s.SupervisorId,
                            userfullname = s.M_User.Firstname + " " + s.M_User.Lastname,
                            supervisorname = s.M_Supervisor.Firstname + " " + s.M_Supervisor.Lastname,
                            subdepartment = s.M_User.M_Department.Description,
                            supdepartment = s.M_Supervisor.M_Department.Description
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<SupervisorMapping> GetSubordinates(int supervisorid) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from s in db.T_SupervisorMapping
                        where s.SupervisorId == supervisorid
                        && s.M_User.Status == "Active"
                        select new SupervisorMapping()
                        {
                            Id = s.Id,
                            UserId = s.UserId,
                            SupervisorId = s.SupervisorId,
                            userfullname = s.M_User.Firstname + " " + s.M_User.Lastname,
                            supervisorname = s.M_Supervisor.Firstname + " " + s.M_Supervisor.Lastname,
                            subdepartment = s.M_User.M_Department.Description,
                            supdepartment = s.M_Supervisor.M_Department.Description
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<SupervisorMapping> GetActiveSubordinates(int supervisorid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from s in db.T_SupervisorMapping
                        where s.SupervisorId == supervisorid
                        select new SupervisorMapping()
                        {
                            Id = s.Id,
                            UserId = s.UserId,
                            SupervisorId = s.SupervisorId,
                            userfullname = s.M_User.Firstname + " " + s.M_User.Lastname,
                            supervisorname = s.M_Supervisor.Firstname + " " + s.M_Supervisor.Lastname,
                            subdepartment = s.M_User.M_Department.Description,
                            supdepartment = s.M_Supervisor.M_Department.Description
                        }).ToList();

            db.Dispose();

            return data;
        }


        public void Insert(SupervisorMapping rm)
        {
            T_SupervisorMapping t_rm = new T_SupervisorMapping();
            t_rm.UserId = rm.UserId;
            t_rm.SupervisorId = rm.SupervisorId;

            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    db.T_SupervisorMapping.Add(t_rm);
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
                    T_SupervisorMapping t_rs = new T_SupervisorMapping();
                    t_rs = db.T_SupervisorMapping.FirstOrDefault(p => p.Id == id);
                    db.T_SupervisorMapping.Remove(t_rs);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Update(SupervisorMapping rs)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_SupervisorMapping t_rs = db.T_SupervisorMapping.FirstOrDefault(p => p.Id == rs.Id);
                    t_rs.UserId = rs.UserId;
                    t_rs.SupervisorId = rs.SupervisorId;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public double getAngle(int x1, int y1, int x2, int y2, int x3, int y3) 
        {
            double result = 0;
            double a;
            double b;
            double c;

            a = Math.Sqrt(Math.Pow(x2 - x1,2) + Math.Pow(y2-y1,2));
            c = Math.Sqrt(Math.Pow(x3 - x2, 2) + Math.Pow(y3 - y2, 2));
            b = Math.Sqrt(Math.Pow(x1 - x3, 2) + Math.Pow(y1 - y3, 2));

            result = Math.Acos((Math.Pow(a,2)+Math.Pow(b,2)-Math.Pow(c,2))/(2*a*b));
            return result;
        }
    }
}