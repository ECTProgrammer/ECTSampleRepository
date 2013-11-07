using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class RolesModuleAccess : T_RolesModuleAccess
    {
        public RolesModuleAccess GetRolesModuleAccess(int id)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from p in db.T_RolesModuleAccess
                        where p.Id == id
                        select new RolesModuleAccess()
                        {
                            Id = p.Id,
                            RoleId = p.RoleId,
                            ModuleId = p.ModuleId,
                            CanView = p.CanView,
                            CanAdd = p.CanAdd,
                            CanUpdate = p.CanUpdate,
                            CanDelete = p.CanDelete
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public RolesModuleAccess GetRolesModuleAccess(int roleid, int moduleid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from p in db.T_RolesModuleAccess
                        where p.RoleId == roleid
                        && p.ModuleId == moduleid
                        select new RolesModuleAccess()
                        {
                            Id = p.Id,
                            RoleId = p.RoleId,
                            ModuleId = p.ModuleId,
                            CanView = p.CanView,
                            CanAdd = p.CanAdd,
                            CanUpdate = p.CanUpdate,
                            CanDelete = p.CanDelete
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public List<RolesModuleAccess> GetRoleModuleList()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from p in db.T_RolesModuleAccess
                        select new RolesModuleAccess()
                        {
                            Id = p.Id,
                            RoleId = p.RoleId,
                            ModuleId = p.ModuleId,
                            CanView = p.CanView,
                            CanAdd = p.CanAdd,
                            CanUpdate = p.CanUpdate,
                            CanDelete = p.CanDelete
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<RolesModuleAccess> GetRoleModuleList(int roleId)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from p in db.T_RolesModuleAccess
                        where p.RoleId == roleId
                        select new RolesModuleAccess()
                        {
                            Id = p.Id,
                            RoleId = p.RoleId,
                            ModuleId = p.ModuleId,
                            CanView = p.CanView,
                            CanAdd = p.CanAdd,
                            CanUpdate = p.CanUpdate,
                            CanDelete = p.CanDelete
                        }).ToList();

            db.Dispose();

            return data;
        }

        public void Insert(RolesModuleAccess rolemoduleaccess)
        {
            T_RolesModuleAccess t_rolemoduleaccess = new T_RolesModuleAccess();
            Parse(t_rolemoduleaccess, rolemoduleaccess);

            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    db.T_RolesModuleAccess.Add(t_rolemoduleaccess);
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
                    T_RolesModuleAccess t_rolemoduleaccess = new T_RolesModuleAccess();
                    t_rolemoduleaccess = db.T_RolesModuleAccess.FirstOrDefault(p => p.Id == id);
                    db.T_RolesModuleAccess.Remove(t_rolemoduleaccess);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Update(RolesModuleAccess rolemoduleaccess)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_RolesModuleAccess t_rolemoduleaccess = db.T_RolesModuleAccess.FirstOrDefault(p => p.Id == rolemoduleaccess.Id);
                    Parse(t_rolemoduleaccess, rolemoduleaccess);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        private void Parse(T_RolesModuleAccess t_rolemoduleaccess, RolesModuleAccess rolemoduleaccess)
        {
            t_rolemoduleaccess.RoleId = rolemoduleaccess.RoleId;
            t_rolemoduleaccess.ModuleId = rolemoduleaccess.ModuleId;
            t_rolemoduleaccess.CanAdd = rolemoduleaccess.CanAdd;
            t_rolemoduleaccess.CanView = rolemoduleaccess.CanView;
            t_rolemoduleaccess.CanUpdate = rolemoduleaccess.CanUpdate;
            t_rolemoduleaccess.CanDelete = rolemoduleaccess.CanDelete;
        }
    }
}