using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace TimeTracker.Model
{
    public class RolesSupervisor : T_RolesSupervisor
    {
        public string supervisorname { get; set; }
        public int supervisorid {get;set;}
        public string subfullname {get;set;}
        public int subid {get;set;}


        public RolesSupervisor GetRoleSupervisor(int roleid, int supervisorid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from r in db.T_RolesSupervisor
                        join u in db.T_Users
                        on r.RoleId equals u.RoleId
                        join s in db.T_Users
                        on r.SupervisorRoleId equals s.RoleId
                        where r.RoleId == roleid
                        && r.SupervisorRoleId == supervisorid
                        select new RolesSupervisor()
                        {
                            RoleId = r.RoleId,
                            SupervisorRoleId = r.SupervisorRoleId,
                            supervisorname = s.Firstname + " " + s.Lastname,
                            supervisorid = s.Id,
                            subfullname = u.Firstname + " " + u.Lastname,
                            subid = u.Id
                        }).FirstOrDefault();

            return data;
        }

        public List<RolesSupervisor> GetSupervisors(int roleid,int userid = 0) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from r in db.T_RolesSupervisor
                        join u in db.T_Users
                        on r.RoleId equals u.RoleId
                        join s in db.T_Users
                        on r.SupervisorRoleId equals s.RoleId
                        where r.RoleId == roleid
                        && s.Id != userid //prevent showing self in supervisor
                        select new RolesSupervisor() 
                        {
                            RoleId = r.RoleId,
                            SupervisorRoleId = r.SupervisorRoleId,
                            supervisorname = s.Firstname + " "+s.Lastname,
                            supervisorid = s.Id,
                            subfullname = u.Firstname + " " +u.Lastname,
                            subid = u.Id
                        }).ToList();

            return data;
        }

        public List<RolesSupervisor> GetSubordinates(int supRoleId,int userid = 0) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from r in db.T_RolesSupervisor
                        join u in db.T_Users
                        on r.RoleId equals u.RoleId
                        join s in db.T_Users
                        on r.SupervisorRoleId equals s.RoleId
                        where r.SupervisorRoleId == supRoleId
                        && u.Id != userid //prevent showing self in subordinate
                        select new RolesSupervisor() 
                        {
                            RoleId = r.RoleId,
                            SupervisorRoleId = r.SupervisorRoleId,
                            supervisorname = s.Firstname + " "+s.Lastname,
                            supervisorid = s.Id,
                            subfullname = u.Firstname + " " +u.Lastname,
                            subid = u.Id
                        }).ToList();

            return data;
        }

        public void Insert(RolesSupervisor rs)
        {
            T_RolesSupervisor t_rs= new T_RolesSupervisor();
            t_rs.RoleId = rs.RoleId;
            t_rs.SupervisorRoleId = rs.SupervisorRoleId;

            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    db.T_RolesSupervisor.Add(t_rs);
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
                    T_RolesSupervisor t_rs = new T_RolesSupervisor();
                    t_rs = db.T_RolesSupervisor.FirstOrDefault(p => p.Id == id);
                    db.T_RolesSupervisor.Remove(t_rs);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Update(RolesSupervisor rs)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_RolesSupervisor t_rs = db.T_RolesSupervisor.FirstOrDefault(p => p.Id == rs.Id);
                    t_rs.RoleId = rs.RoleId;
                    t_rs.SupervisorRoleId = rs.SupervisorRoleId;
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