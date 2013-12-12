using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class RoleDepartmentAccess :T_RoleDepartmentAccess
    {
        public string department { get; set; }
        public List<RoleDepartmentAccess> GetRoleDepartmentList()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from p in db.T_RoleDepartmentAccess
                        select new RoleDepartmentAccess()
                        {
                            Id = p.Id,
                            RoleId = p.RoleId,
                            DepartmentId = p.DepartmentId
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<RoleDepartmentAccess> GetRoleDepartmentList(int roleid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from p in db.T_RoleDepartmentAccess
                        where p.RoleId == roleid
                        select new RoleDepartmentAccess()
                        {
                            Id = p.Id,
                            RoleId = p.RoleId,
                            DepartmentId = p.DepartmentId,
                            department = p.M_Department.Description
                        }).ToList();

            db.Dispose();

            return data;
        }

        public void Insert(RoleDepartmentAccess roledepartmentaccess)
        {
            T_RoleDepartmentAccess t_roledepartmentaccess = new T_RoleDepartmentAccess();
            Parse(t_roledepartmentaccess, roledepartmentaccess);

            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    db.T_RoleDepartmentAccess.Add(t_roledepartmentaccess);
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
                    T_RoleDepartmentAccess t_roledepartmentaccess = new T_RoleDepartmentAccess();
                    t_roledepartmentaccess = db.T_RoleDepartmentAccess.FirstOrDefault(p => p.Id == id);
                    db.T_RoleDepartmentAccess.Remove(t_roledepartmentaccess);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Update(RoleDepartmentAccess roledepartmentaccess)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_RoleDepartmentAccess t_roledepartmentaccess = db.T_RoleDepartmentAccess.FirstOrDefault(p => p.Id == roledepartmentaccess.Id);
                    Parse(t_roledepartmentaccess, roledepartmentaccess);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        private void Parse(T_RoleDepartmentAccess t_roledepartmentaccess, RoleDepartmentAccess roledepartmentaccess)
        {
            t_roledepartmentaccess.RoleId = roledepartmentaccess.RoleId;
            t_roledepartmentaccess.DepartmentId = roledepartmentaccess.DepartmentId;
        }
    }
}