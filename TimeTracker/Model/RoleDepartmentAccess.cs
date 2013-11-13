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
    }
}