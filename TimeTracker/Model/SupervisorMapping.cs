using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class SupervisorMapping : T_SupervisorMapping
    {
        string userfullname {get;set;}
        string supervisorname {get;set;}

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
                            supervisorname = s.M_Supervisor.Firstname+" "+s.M_Supervisor.Lastname
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
                            supervisorname = s.M_Supervisor.Firstname + " " + s.M_Supervisor.Lastname
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
                            supervisorname = s.M_Supervisor.Firstname + " " + s.M_Supervisor.Lastname
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<SupervisorMapping> GetSubordinates(int supervisorid) 
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
                            supervisorname = s.M_Supervisor.Firstname + " " + s.M_Supervisor.Lastname
                        }).ToList();

            db.Dispose();

            return data;
        }

        public void Insert(SupervisorMapping rm)
        {
            SupervisorMapping t_rm = new SupervisorMapping();
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
    }
}