using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class Module:T_Modules
    {
        public Module GetModule(int moduleid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from m in db.T_Modules
                        where m.Id == moduleid
                        select new Module()
                        {
                            Id = m.Id,
                            Filename = m.Filename,
                            Description = m.Description,
                            ModuleType = m.ModuleType,
                            ArrangementOrder = m.ArrangementOrder
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public Module GetModule(string filename)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from m in db.T_Modules
                        where m.Filename == filename
                        select new Module()
                        {
                            Id = m.Id,
                            Filename = m.Filename,
                            Description = m.Description,
                            ModuleType = m.ModuleType,
                            ArrangementOrder = m.ArrangementOrder
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public List<Module> GetModuleList(string moduletype)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from m in db.T_Modules
                        where m.ModuleType == moduletype
                        orderby m.ArrangementOrder
                        select new Module()
                        {
                            Id = m.Id,
                            Filename = m.Filename,
                            Description = m.Description,
                            ModuleType = m.ModuleType,
                            ArrangementOrder = m.ArrangementOrder
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<Module> GetModuleList()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from m in db.T_Modules
                        orderby m.ModuleType,m.ArrangementOrder
                        select new Module()
                        {
                            Id = m.Id,
                            Filename = m.Filename,
                            Description = m.Description,
                            ModuleType = m.ModuleType,
                            ArrangementOrder = m.ArrangementOrder
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<Module> GetModuleList(int roleId,string moduletype)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from m in db.T_Modules
                        where m.ModuleType == moduletype
                        && m.M_RolesModuleAccesses.FirstOrDefault(r => r.RoleId == roleId) != null
                        orderby m.ModuleType, m.ArrangementOrder
                        select new Module()
                        {
                            Id = m.Id,
                            Filename = m.Filename,
                            Description = m.Description,
                            ModuleType = m.ModuleType,
                            ArrangementOrder = m.ArrangementOrder
                        }).ToList();

            db.Dispose();

            return data;
        }
    }
}