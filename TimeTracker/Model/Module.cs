using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class Module:T_Module
    {
        public Module GetModule(int moduleid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from m in db.T_Modules
                        where m.Id == moduleid
                        select new Module()
                        {
                            Id = m.Id,
                            Module1 = m.Module1,
                            Description = m.Description,
                            ModuleType = m.ModuleType,
                            ArrangementOrder = m.ArrangementOrder
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public Module GetModule(string modulename)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from m in db.T_Modules
                        where m.Module1 == modulename
                        select new Module()
                        {
                            Id = m.Id,
                            Module1 = m.Module1,
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
                            Module1 = m.Module1,
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
                            Module1 = m.Module1,
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
                        join p in db.T_RolesModuleAccess
                        on m.Id equals p.ModuleId
                        where m.ModuleType == moduletype
                        && p.RoleId == roleId
                        orderby m.ModuleType, m.ArrangementOrder
                        select new Module()
                        {
                            Id = m.Id,
                            Module1 = m.Module1,
                            Description = m.Description,
                            ModuleType = m.ModuleType,
                            ArrangementOrder = m.ArrangementOrder
                        }).Distinct().ToList();

            db.Dispose();

            return data;
        }
    }
}