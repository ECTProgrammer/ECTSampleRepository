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
                        join d in db.T_Departments
                        on p.DepartmentId equals d.Id
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
                        join d in db.T_Departments
                        on p.DepartmentId equals d.Id
                        where p.RoleId == roleid
                        select new RoleDepartmentAccess()
                        {
                            Id = p.Id,
                            RoleId = p.RoleId,
                            DepartmentId = p.DepartmentId,
                            department = d.Description
                        }).ToList();

            db.Dispose();

            return data;
        }
    }
}