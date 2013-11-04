using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class User : T_User
    {
        public string fullname { get; set; }

        public User GetUser(int userid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_Users
                        where u.Id == userid
                        select new User()
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Password,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            PositionId = u.PositionId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname+" "+u.Lastname
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public User GetUser(string username, string password) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_Users
                        where u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                        && u.Password == password
                        select new User() 
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Password,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            PositionId = u.PositionId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname
                        }).FirstOrDefault();

            db.Dispose();

            return  data;
        }

        public List<User> GetSupervisors(int departmentId,int userid = 0) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_Users
                        join p in db.T_Positions
                        on u.PositionId equals p.Id
                        where u.DepartmentId == departmentId
                        && p.Rank == 1
                        select new User()
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Password,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            PositionId = u.PositionId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname
                        }).ToList();

            db.Dispose();

            return data;
        }

        public void Insert(User user)
        {
            T_User t_user = InsertParse(user);

            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    db.T_Users.Add(t_user);
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
                    T_User t_user = new T_User();
                    t_user = db.T_Users.FirstOrDefault(u => u.Id == id);
                    db.T_Users.Remove(t_user);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Update(User user)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_User t_user = db.T_Users.FirstOrDefault(u => u.Id == user.Id);
                    UpdateParse(t_user, user);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        private T_User InsertParse(User user)
        {
            T_User t_user = new T_User();
            t_user.Firstname = user.Firstname;
            t_user.Lastname = user.Lastname;
            t_user.PositionId = user.PositionId;
            t_user.DepartmentId = user.DepartmentId;
            t_user.Username = user.Username;
            t_user.Password = user.Password;
            t_user.Email = user.Email;
            t_user.Phone = user.Phone;
            t_user.Fax = user.Fax;
            t_user.Mobile = user.Mobile;
            t_user.Status = user.Status;
            t_user.CreateDate = user.CreateDate;
            t_user.LastUpdateDate = user.LastUpdateDate;
            t_user.CreatedBy = user.CreatedBy;
            t_user.LastUpdatedBy = user.LastUpdatedBy;

            return t_user;
        }

        private void UpdateParse(T_User t_user, User user)
        {
            t_user.Firstname = user.Firstname;
            t_user.Lastname = user.Lastname;
            t_user.PositionId = user.PositionId;
            t_user.DepartmentId = user.DepartmentId;
            t_user.Username = user.Username;
            t_user.Password = user.Password;
            t_user.Email = user.Email;
            t_user.Phone = user.Phone;
            t_user.Fax = user.Fax;
            t_user.Mobile = user.Mobile;
            t_user.Status = user.Status;
            t_user.LastUpdateDate = user.LastUpdateDate;
            t_user.LastUpdatedBy = user.LastUpdatedBy;
        }
    }
}