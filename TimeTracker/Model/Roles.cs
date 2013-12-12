using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class Roles : T_Roles
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
                        where d.M_RolesModuleAccesses.Count > 0
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

        public List<Roles> GetRolesWithDepartmentAccess()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from d in db.T_Roles
                        where d.M_RoleDepartmentAccesses.Count > 0
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
                        where d.M_RolesModuleAccesses.Count > 0
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

        public List<Roles> GetRolesWithoutDepartmentAccess()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from d in db.T_Roles
                        where d.M_RoleDepartmentAccesses.Count > 0
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

        //public List<Roles> GetRolesWithSupervisors() 
        //{
        //    TimeTrackerEntities db = new TimeTrackerEntities();

        //    var data = (from r in db.T_Roles
        //                join s in db.T_RolesSupervisor
        //                on r.Id equals s.RoleId
        //                select new Roles()
        //                {
        //                    Id = r.Id,
        //                    Description = r.Description,
        //                    Rank = r.Rank,
        //                    CreateDate = r.CreateDate,
        //                    LastUpdateDate = r.LastUpdateDate,
        //                    CreatedBy = r.CreatedBy,
        //                    LastUpdatedBy = r.LastUpdatedBy,
        //                    IsSupervisor = r.IsSupervisor
        //                }).Distinct().ToList();

        //    db.Dispose();

        //    return data;
        //}

        //public List<Roles> GetRolesWithoutSupervisor()
        //{
        //    TimeTrackerEntities db = new TimeTrackerEntities();

        //    var data = (from r in db.T_Roles
        //                join s in db.T_RolesSupervisor
        //                on r.Id equals s.RoleId
        //                orderby r.Id
        //                select new Roles()
        //                {
        //                    Id = r.Id,
        //                    Description = r.Description,
        //                    Rank = r.Rank,
        //                    CreateDate = r.CreateDate,
        //                    LastUpdateDate = r.LastUpdateDate,
        //                    CreatedBy = r.CreatedBy,
        //                    LastUpdatedBy = r.LastUpdatedBy,
        //                    IsSupervisor = r.IsSupervisor
        //                }).Distinct().ToList();

        //    db.Dispose();

        //    var list = GetRoleList();

        //    for (int i = 0; i < data.Count; i++)
        //    {
        //        for (int j = 0; j < list.Count; j++)
        //        {
        //            if (data[i].Id == list[j].Id)
        //            {
        //                list.RemoveAt(j);
        //                break;
        //            }
        //        }
        //    }

        //    return list;
        //}

        public void Insert(Roles role)
        {
            T_Roles t_role = InsertParse(role);

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
                    T_Roles t_role = new T_Roles();
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
                    T_Roles t_role = db.T_Roles.FirstOrDefault(p => p.Id == role.Id);
                    UpdateParse(t_role, role);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        private T_Roles InsertParse(Roles role)
        {
            T_Roles t_role = new T_Roles();
            t_role.Description = role.Description;
            t_role.Rank = role.Rank;
            t_role.CreateDate = role.CreateDate;
            t_role.LastUpdateDate = role.LastUpdateDate;
            t_role.CreatedBy = role.CreatedBy;
            t_role.LastUpdatedBy = role.LastUpdatedBy;
            t_role.IsSupervisor = role.IsSupervisor;
            return t_role;
        }

        private void UpdateParse(T_Roles t_role, Roles role)
        {
            t_role.Description = role.Description;
            t_role.Rank = role.Rank;
            t_role.LastUpdateDate = role.LastUpdateDate;
            t_role.LastUpdatedBy = role.LastUpdatedBy;
            t_role.IsSupervisor = role.IsSupervisor;
        }
    }
}