using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class Roles : T_Role
    {
        public Roles GetRole(int rolesid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from d in db.T_Roles
                        where d.Id == rolesid
                        select new Roles()
                        {
                            Id = d.Id,
                            Description = d.Description,
                            Rank = d.Rank,
                            CreateDate = d.CreateDate,
                            LastUpdateDate = d.LastUpdateDate,
                            CreatedBy = d.CreatedBy,
                            LastUpdatedBy = d.LastUpdatedBy,
                            IsSupervisor = d.IsSupervisor
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public List<Roles> GetRoleList()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from d in db.T_Roles
                        orderby d.Id
                        select new Roles()
                        {
                            Id = d.Id,
                            Description = d.Description,
                            Rank = d.Rank,
                            CreateDate = d.CreateDate,
                            LastUpdateDate = d.LastUpdateDate,
                            CreatedBy = d.CreatedBy,
                            LastUpdatedBy = d.LastUpdatedBy,
                            IsSupervisor = d.IsSupervisor
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<Roles> GetRolesWithModuleAccess()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from d in db.T_Roles
                        join m in db.T_RolesModuleAccess
                        on d.Id equals m.RoleId
                        select new Roles()
                        {
                            Id = d.Id,
                            Description = d.Description,
                            Rank = d.Rank,
                            CreateDate = d.CreateDate,
                            LastUpdateDate = d.LastUpdateDate,
                            CreatedBy = d.CreatedBy,
                            LastUpdatedBy = d.LastUpdatedBy,
                            IsSupervisor = d.IsSupervisor
                        }).Distinct().ToList();

            db.Dispose();

            return data;
        }

        public List<Roles> GetRolesWithoutModuleAccess()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from d in db.T_Roles
                        join m in db.T_RolesModuleAccess
                        on d.Id equals m.RoleId
                        orderby d.Id
                        select new Roles()
                        {
                            Id = d.Id,
                            Description = d.Description,
                            Rank = d.Rank,
                            CreateDate = d.CreateDate,
                            LastUpdateDate = d.LastUpdateDate,
                            CreatedBy = d.CreatedBy,
                            LastUpdatedBy = d.LastUpdatedBy,
                            IsSupervisor = d.IsSupervisor
                        }).Distinct().ToList();

            db.Dispose();

            var list = GetRoleList();

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (data[i].Id == list[j].Id) 
                    {
                        list.RemoveAt(j);
                        break;
                    }
                }
            }

            return list;
        }

        public void Insert(Roles role)
        {
            T_Role t_role = InsertParse(role);

            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    db.T_Roles.Add(t_role);
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
                    T_Role t_role = new T_Role();
                    t_role = db.T_Roles.FirstOrDefault(p => p.Id == id);
                    db.T_Roles.Remove(t_role);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Update(Roles role)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_Role t_role = db.T_Roles.FirstOrDefault(p => p.Id == role.Id);
                    UpdateParse(t_role, role);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        private T_Role InsertParse(Roles role)
        {
            T_Role t_role = new T_Role();
            t_role.Description = role.Description;
            t_role.Rank = role.Rank;
            t_role.CreateDate = role.CreateDate;
            t_role.LastUpdateDate = role.LastUpdateDate;
            t_role.CreatedBy = role.CreatedBy;
            t_role.LastUpdatedBy = role.LastUpdatedBy;
            t_role.IsSupervisor = role.IsSupervisor;
            return t_role;
        }

        private void UpdateParse(T_Role t_role, Roles role)
        {
            t_role.Description = role.Description;
            t_role.Rank = role.Rank;
            t_role.LastUpdateDate = role.LastUpdateDate;
            t_role.LastUpdatedBy = role.LastUpdatedBy;
            t_role.IsSupervisor = role.IsSupervisor;
        }
    }
}