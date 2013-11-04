using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class PositionModuleAccess : T_PositionModuleAccess
    {
        public PositionModuleAccess GetPositionModuleAccess(int id) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from p in db.T_PositionModuleAccesses
                        where p.Id == id
                        select new PositionModuleAccess()
                        {
                            Id = p.Id,
                            PositionId = p.PositionId,
                            ModuleId = p.ModuleId,
                            CanView = p.CanView,
                            CanAdd = p.CanAdd,
                            CanUpdate = p.CanUpdate,
                            CanDelete = p.CanDelete
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public List<PositionModuleAccess> GetPositionModuleList() 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from p in db.T_PositionModuleAccesses
                        select new PositionModuleAccess()
                        {
                            Id = p.Id,
                            PositionId = p.PositionId,
                            ModuleId = p.ModuleId,
                            CanView = p.CanView,
                            CanAdd = p.CanAdd,
                            CanUpdate = p.CanUpdate,
                            CanDelete = p.CanDelete
                        }).ToList();

            db.Dispose();

            return data;
        }

        public void Insert(PositionModuleAccess positionmoduleaccess)
        {
            T_PositionModuleAccess t_positionmoduleaccess = new T_PositionModuleAccess();
            Parse(t_positionmoduleaccess,positionmoduleaccess);

            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    db.T_PositionModuleAccesses.Add(t_positionmoduleaccess);
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
                    T_PositionModuleAccess t_positionmoduleaccess = new T_PositionModuleAccess();
                    t_positionmoduleaccess = db.T_PositionModuleAccesses.FirstOrDefault(p => p.Id == id);
                    db.T_PositionModuleAccesses.Remove(t_positionmoduleaccess);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Update(PositionModuleAccess positionmoduleaccess)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_PositionModuleAccess t_positionmoduleaccess = db.T_PositionModuleAccesses.FirstOrDefault(p => p.Id == positionmoduleaccess.Id);
                    Parse(t_positionmoduleaccess, positionmoduleaccess);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        private void Parse(T_PositionModuleAccess t_positionmoduleaccess, PositionModuleAccess positionmoduleaccess)
        {
            t_positionmoduleaccess.PositionId = positionmoduleaccess.PositionId;
            t_positionmoduleaccess.ModuleId = positionmoduleaccess.ModuleId;
            t_positionmoduleaccess.CanAdd = positionmoduleaccess.CanAdd;
            t_positionmoduleaccess.CanView = positionmoduleaccess.CanView;
            t_positionmoduleaccess.CanUpdate = positionmoduleaccess.CanUpdate;
            t_positionmoduleaccess.CanDelete = positionmoduleaccess.CanDelete;
        }
    }
}