using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class User : T_Users
    {
        public string fullname { get; set; }
        public string department { get; set; }
        public string role { get; set; }
        public string startTime {get;set;}
        public string endTime {get;set;}
        public decimal currentSalary { get; set; }
        public int currentOffDay { get; set; }
        public int currentSpecialOffDay { get; set; }
        public int currentOptOffDay1 { get; set; }
        public int currentOptOffDay2 { get; set; }
        public int currentOptOffDay3 { get; set; }
        public int currentOptOffDay4 { get; set; }
        public bool noOTpay { get; set; }
        public bool shifting { get; set; }
        public int minsWorkPerDay {get;set;}
        public int currentMinBreak { get; set; }
        public bool isOfficeWorker { get; set; }

        //Gets User data base on userid
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
                            Phone = u.Phone,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            RoleId = u.RoleId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname+" "+u.Lastname,
                            role = u.M_Role.Description,
                            department = u.M_Department.Description,
                            EmployeeNumber = u.EmployeeNumber,
                            Shift = u.Shift,
                            currentSalary = 0,
                            currentOffDay = 0,
                            currentSpecialOffDay = 0,
                            currentOptOffDay1 = 0,
                            currentOptOffDay2 = 0,
                            currentOptOffDay3 = 0,
                            currentOptOffDay4 = 0,
                            noOTpay = false,
                            startTime = "",
                            endTime = ""
                        }).FirstOrDefault();

            db.Dispose();

            if (data != null)
            {
                data.GetMyRate();
            }
            return data;
        }

        //Gets User and rate on the datetime provided
        public User GetUser(int userid,DateTime seleteddate)
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
                            Phone = u.Phone,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            RoleId = u.RoleId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname,
                            role = u.M_Role.Description,
                            department = u.M_Department.Description,
                            EmployeeNumber = u.EmployeeNumber,
                            Shift = u.Shift,
                            currentSalary = 0,
                            currentOffDay = 0,
                            currentSpecialOffDay = 0,
                            currentOptOffDay1 = 0,
                            currentOptOffDay2 = 0,
                            currentOptOffDay3 = 0,
                            currentOptOffDay4 = 0,
                            noOTpay = false,
                            startTime = "",
                            endTime = "",
                            shifting = false
                        }).FirstOrDefault();

            db.Dispose();

            if (data != null)
            {
                data.GetMyRate(seleteddate);
            }
            return data;
        }

        //Gets User data base on username
        public User GetUser(string username)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_Users
                        where u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                        select new User()
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Phone,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            RoleId = u.RoleId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname,
                            role = u.M_Role.Description,
                            department = u.M_Department.Description,
                            EmployeeNumber = u.EmployeeNumber,
                            Shift = u.Shift,
                            currentSalary = 0,
                            currentOffDay = 0,
                            currentSpecialOffDay = 0,
                            currentOptOffDay1 = 0,
                            currentOptOffDay2 = 0,
                            currentOptOffDay3 = 0,
                            currentOptOffDay4 = 0,
                            noOTpay = false,
                            startTime = "",
                            endTime = "",
                            shifting = false
                        }).FirstOrDefault();

            db.Dispose();
            if (data != null)
            {
                data.GetMyRate();
            }
            return data;
        }

        //Gets User data base on username and password
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
                            Phone = u.Phone,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            RoleId = u.RoleId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname,
                            role = u.M_Role.Description,
                            department = u.M_Department.Description,
                            EmployeeNumber = u.EmployeeNumber,
                            Shift = u.Shift,
                            startTime = "",
                            endTime = "",
                            currentSalary = 0,
                            currentOffDay = 0,
                            currentSpecialOffDay = 0,
                            currentOptOffDay1 = 0,
                            currentOptOffDay2 = 0,
                            currentOptOffDay3 = 0,
                            currentOptOffDay4 = 0,
                            noOTpay = false,
                            shifting = false
                        }).FirstOrDefault();

            db.Dispose();
            if (data != null)
            {
                data.GetMyRate();
            }
            return  data;
        }

        //Get Last Inserted User
        public User GetLastInsertedUser()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_Users
                        orderby u.Id descending
                        select new User()
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Phone,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            RoleId = u.RoleId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname,
                            role = u.M_Role.Description,
                            department = u.M_Department.Description,
                            EmployeeNumber = u.EmployeeNumber,
                            Shift = u.Shift,
                            currentSalary = 0,
                            currentOffDay = 0,
                            currentSpecialOffDay = 0,
                            currentOptOffDay1 = 0,
                            currentOptOffDay2 = 0,
                            currentOptOffDay3 = 0,
                            currentOptOffDay4 = 0,
                            noOTpay = false,
                            startTime = "",
                            endTime = "",
                            shifting = false
                        }).FirstOrDefault();

            db.Dispose();
            if (data != null)
            {
                data.GetMyRate();
            }
            return data;
        }

        // Gets active user base on username and password
        public User GetActiveUser(string username, string password)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_Users
                        where u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                        && u.Password == password
                        && u.Status == "Active"
                        select new User()
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Phone,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            RoleId = u.RoleId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname,
                            role = u.M_Role.Description,
                            department = u.M_Department.Description,
                            EmployeeNumber = u.EmployeeNumber,
                            Shift = u.Shift,
                            currentSalary = 0,
                            currentOffDay = 0,
                            currentSpecialOffDay = 0,
                            currentOptOffDay1 = 0,
                            currentOptOffDay2 = 0,
                            currentOptOffDay3 = 0,
                            currentOptOffDay4 = 0,
                            noOTpay = false,
                            startTime = "",
                            endTime = "",
                            shifting = false
                        }).FirstOrDefault();

            db.Dispose();
            if (data != null)
            {
                data.GetMyRate();
            }
            return data;
        }

        //Gets all Users
        public List<User> GetUserList()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_Users
                        orderby u.Firstname
                        select new User()
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Phone,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            RoleId = u.RoleId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname,
                            role = u.M_Role.Description,
                            department = u.M_Department.Description,
                            EmployeeNumber = u.EmployeeNumber,
                            Shift = u.Shift,
                            currentSalary = 0,
                            currentOffDay = 0,
                            currentSpecialOffDay = 0,
                            currentOptOffDay1 = 0,
                            currentOptOffDay2 = 0,
                            currentOptOffDay3 = 0,
                            currentOptOffDay4 = 0,
                            noOTpay = false,
                            startTime = "",
                            endTime = "",
                            shifting = false
                        }).ToList();

            db.Dispose();
            foreach (User u in data) 
            {
                u.GetMyRate();
            }

            return data;
        }

        //Gets all users of a department
        public List<User> GetUserList(int departmentId)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_Users
                        where u.DepartmentId == departmentId
                        orderby u.Firstname
                        select new User()
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Phone,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            RoleId = u.RoleId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname,
                            role = u.M_Role.Description,
                            department = u.M_Department.Description,
                            EmployeeNumber = u.EmployeeNumber,
                            Shift = u.Shift,
                            currentSalary = 0,
                            currentOffDay = 0,
                            currentSpecialOffDay = 0,
                            currentOptOffDay1 = 0,
                            currentOptOffDay2 = 0,
                            currentOptOffDay3 = 0,
                            currentOptOffDay4 = 0,
                            noOTpay = false,
                            startTime = "",
                            endTime = "",
                            shifting = false
                        }).ToList();

            db.Dispose();
            foreach (User u in data)
            {
                u.GetMyRate();
            }
            return data;
        }

        //Gets all active users in a department
        public List<User> GetActiveUserList(int departmentId)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_Users
                        where u.DepartmentId == departmentId
                        && u.Status == "Active"
                        orderby u.Firstname
                        select new User()
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Phone,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            RoleId = u.RoleId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname,
                            role = u.M_Role.Description,
                            department = u.M_Department.Description,
                            EmployeeNumber = u.EmployeeNumber,
                            Shift = u.Shift,
                            currentSalary = 0,
                            currentOffDay = 0,
                            currentSpecialOffDay = 0,
                            currentOptOffDay1 = 0,
                            currentOptOffDay2 = 0,
                            currentOptOffDay3 = 0,
                            currentOptOffDay4 = 0,
                            noOTpay = false,
                        }).ToList();

            db.Dispose();
            foreach (User u in data)
            {
                u.GetMyRate();
            }
            return data;
        }

        //Gets all Active users with supervisors
        public List<User> GetActiveUsersWithSupervisor()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_Users
                        where u.M_SupervisorMappingUsers.Count > 0
                        && u.Status == "Active"
                        orderby u.Firstname
                        select new User()
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Phone,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            RoleId = u.RoleId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname,
                            role = u.M_Role.Description,
                            department = u.M_Department.Description,
                            EmployeeNumber = u.EmployeeNumber,
                            Shift = u.Shift,
                            currentSalary = 0,
                            currentOffDay = 0,
                            currentSpecialOffDay = 0,
                            currentOptOffDay1 = 0,
                            currentOptOffDay2 = 0,
                            currentOptOffDay3 = 0,
                            currentOptOffDay4 = 0,
                            noOTpay = false,
                            startTime = "",
                            endTime = "",
                            shifting = false
                        }).ToList();

            db.Dispose();
            foreach (User u in data)
            {
                u.GetMyRate();
            }
            return data;
        }

        //Gets all active users in a specific department with supervisor
        public List<User> GetActiveUsersWithSupervisor(int departmentid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_Users
                        where u.M_SupervisorMappingUsers.Count > 0
                        && u.DepartmentId == departmentid
                        && u.Status == "Active"
                        orderby u.Firstname
                        select new User()
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Phone,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            RoleId = u.RoleId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname,
                            role = u.M_Role.Description,
                            department = u.M_Department.Description,
                            EmployeeNumber = u.EmployeeNumber,
                            currentSalary = 0,
                            currentOffDay = 0,
                            currentSpecialOffDay = 0,
                            currentOptOffDay1 = 0,
                            currentOptOffDay2 = 0,
                            currentOptOffDay3 = 0,
                            currentOptOffDay4 = 0,
                            noOTpay = false,
                            startTime = "",
                            endTime = "",
                            shifting = false
                        }).ToList();

            db.Dispose();
            foreach (User u in data)
            {
                u.GetMyRate();
            }
            return data;
        }

        //Gets All active users without supervisors
        public List<User> GetActiveUsersWithoutSupervisor()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_Users
                        where u.M_SupervisorMappingUsers.Count == 0
                        && u.Status == "Active"
                        orderby u.Firstname
                        select new User()
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Phone,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            RoleId = u.RoleId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname,
                            role = u.M_Role.Description,
                            department = u.M_Department.Description,
                            EmployeeNumber = u.EmployeeNumber,
                            Shift = u.Shift,
                            currentSalary = 0,
                            currentOffDay = 0,
                            currentSpecialOffDay = 0,
                            currentOptOffDay1 = 0,
                            currentOptOffDay2 = 0,
                            currentOptOffDay3 = 0,
                            currentOptOffDay4 = 0,
                            noOTpay = false,
                            startTime = "",
                            endTime = "",
                            shifting = false
                        }).ToList();

            db.Dispose();
            foreach (User u in data)
            {
                u.GetMyRate();
            }
            return data;
        }

        //Gets all active users in a specific department without a supervisor
        public List<User> GetActiveUsersWithoutSupervisor(int departmentid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_Users
                        where u.M_SupervisorMappingUsers.Count == 0
                        && u.DepartmentId == departmentid
                        && u.Status == "Active"
                        orderby u.Firstname
                        select new User()
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Phone,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            RoleId = u.RoleId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname,
                            role = u.M_Role.Description,
                            department = u.M_Department.Description,
                            EmployeeNumber = u.EmployeeNumber,
                            Shift = u.Shift,
                            currentSalary = 0,
                            currentOffDay = 0,
                            currentSpecialOffDay = 0,
                            currentOptOffDay1 = 0,
                            currentOptOffDay2 = 0,
                            currentOptOffDay3 = 0,
                            currentOptOffDay4 = 0,
                            noOTpay = false,
                            startTime = "",
                            endTime = "",
                            shifting = false
                        }).ToList();

            db.Dispose();
            foreach (User u in data)
            {
                u.GetMyRate();
            }
            return data;
        }

        //Gets all users by status
        public List<User> GetUserListByStatus(string status)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_Users
                        where u.Status == status
                        orderby u.Firstname
                        select new User()
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Phone,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            RoleId = u.RoleId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname,
                            role = u.M_Role.Description,
                            department = u.M_Department.Description,
                            EmployeeNumber = u.EmployeeNumber,
                            Shift = u.Shift,
                            currentSalary = 0,
                            currentOffDay = 0,
                            currentSpecialOffDay = 0,
                            currentOptOffDay1 = 0,
                            currentOptOffDay2 = 0,
                            currentOptOffDay3 = 0,
                            currentOptOffDay4 = 0,
                            noOTpay = false,
                            startTime = "",
                            endTime = "",
                            shifting = false
                        }).ToList();

            db.Dispose();
            foreach (User u in data)
            {
                u.GetMyRate();
            }
            return data;
        }

        //Gets all users in a specific department by status
        public List<User> GetUserListByDepartmentAndStatus(int departmentId,string status)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_Users
                        where u.DepartmentId == departmentId
                        && u.Status == status
                        orderby u.Firstname
                        select new User()
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Phone,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            RoleId = u.RoleId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname,
                            role = u.M_Role.Description,
                            department = u.M_Department.Description,
                            EmployeeNumber = u.EmployeeNumber,
                            Shift = u.Shift,
                            currentSalary = 0,
                            currentOffDay = 0,
                            currentSpecialOffDay = 0,
                            currentOptOffDay1 = 0,
                            currentOptOffDay2 = 0,
                            currentOptOffDay3 = 0,
                            currentOptOffDay4 = 0,
                            noOTpay = false,
                            startTime = "",
                            endTime = "",
                            shifting = false
                        }).ToList();

            db.Dispose();
            foreach (User u in data)
            {
                u.GetMyRate();
            }
            return data;
        }

        //Old Code not in use
        public List<User> GetSupervisors(int departmentId,int userid = 0) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_Users
                        where u.DepartmentId == departmentId
                        && u.M_Role.IsSupervisor == true
                        select new User()
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Phone,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            RoleId = u.RoleId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname,
                            role = u.M_Role.Description,
                            department = u.M_Department.Description,
                            EmployeeNumber = u.EmployeeNumber,
                            currentSalary = 0,
                            currentOffDay = 0,
                            currentSpecialOffDay = 0,
                            currentOptOffDay1 = 0,
                            currentOptOffDay2 = 0,
                            currentOptOffDay3 = 0,
                            currentOptOffDay4 = 0,
                            noOTpay = false,
                            Shift = u.Shift,
                            startTime = "",
                            endTime = "",
                            shifting = false
                        }).ToList();

            db.Dispose();
            foreach (User u in data)
            {
                u.GetMyRate();
            }
            return data;
        }

        //Get all available users that is not yet selected as a supervisor by a specific user
        public List<User> GetAvailableSupervisors(int userid,int departmentId) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            SupervisorMapping supmap = new SupervisorMapping();
            var data = (from u in db.T_Users
                        where u.DepartmentId == departmentId
                        && u.Status == "Active"
                        select new User()
                        {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Phone,
                            Mobile = u.Mobile,
                            Fax = u.Fax,
                            Email = u.Email,
                            DepartmentId = u.DepartmentId,
                            RoleId = u.RoleId,
                            CreateDate = u.CreateDate,
                            CreatedBy = u.CreatedBy,
                            LastUpdateDate = u.LastUpdateDate,
                            LastUpdatedBy = u.LastUpdatedBy,
                            Status = u.Status,
                            fullname = u.Firstname + " " + u.Lastname,
                            role = u.M_Role.Description,
                            department = u.M_Department.Description,
                            EmployeeNumber = u.EmployeeNumber,
                            currentSalary = 0,
                            currentOffDay = 0,
                            currentSpecialOffDay = 0,
                            currentOptOffDay1 = 0,
                            currentOptOffDay2 = 0,
                            currentOptOffDay3 = 0,
                            currentOptOffDay4 = 0,
                            noOTpay = false,
                            startTime = "",
                            endTime = "",
                            shifting = false
                        }).ToList();
            db.Dispose();
            
            var suplist = supmap.GetActiveSupervisors(userid, departmentId);

            foreach (SupervisorMapping s in suplist) 
            {
                for (int i = 0; i < data.Count; i++) 
                {
                    if (data[i].Id == s.SupervisorId) 
                    {
                        data.RemoveAt(i);
                        break;
                    }
                }
            }

            return data;
        }

        //Insert User in the database
        public void Insert(User user)
        {
            T_Users t_user = InsertParse(user);

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

        //Delete user in the database
        public void Delete(int id)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_Users t_user = new T_Users();
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

        //Update User record in the database
        public void Update(User user)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_Users t_user = db.T_Users.FirstOrDefault(u => u.Id == user.Id);
                    UpdateParse(t_user, user);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        //Parsing done before inserting in the database
        private T_Users InsertParse(User user)
        {
            T_Users t_user = new T_Users();
            t_user.Firstname = user.Firstname;
            t_user.Lastname = user.Lastname;
            t_user.RoleId = user.RoleId;
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
            t_user.EmployeeNumber = user.EmployeeNumber;
            t_user.Shift = user.Shift;
            return t_user;
        }

        //Parsing done before updating record in the database
        private void UpdateParse(T_Users t_user, User user)
        {
            t_user.Firstname = user.Firstname;
            t_user.Lastname = user.Lastname;
            t_user.RoleId = user.RoleId;
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
            t_user.EmployeeNumber = user.EmployeeNumber;
            t_user.Shift = user.Shift;
        }

        private void GetMyRate() 
        {
            UserRateSchedule URS = new UserRateSchedule();

            URS = URS.GetUserScheduleRateCurrentRate(Id);
            if (URS != null) 
            {
                startTime = URS.StartTime == "" ? "08:00" : URS.StartTime;
                endTime = URS.EndTime == "" ? "17:00" : URS.EndTime;
                currentSalary = URS.Salary;
                currentOffDay = URS.OffDay;
                currentSpecialOffDay = URS.SpecialOffDay;
                currentOptOffDay1 = URS.OptionalOffDay1 == null ? 0 : Convert.ToInt32(URS.OptionalOffDay1);
                currentOptOffDay2 = URS.OptionalOffDay2 == null ? 0 : Convert.ToInt32(URS.OptionalOffDay2);
                currentOptOffDay3 = URS.OptionalOffDay3 == null ? 0 : Convert.ToInt32(URS.OptionalOffDay3);
                currentOptOffDay4 = URS.OptionalOffDay4 == null ? 0 : Convert.ToInt32(URS.OptionalOffDay4);
                noOTpay = URS.NoOTPay;
                currentMinBreak = URS.MinsBreak == null ? 0 : Convert.ToInt32(URS.MinsBreak);
                isOfficeWorker = URS.IsOfficeWorker == null ? false : Convert.ToBoolean(URS.IsOfficeWorker);
            }
            else
            {
                startTime = "08:00";
                endTime = "17:00";
                currentSalary = 0;
                currentOffDay = 0;
                currentSpecialOffDay = 0;
                currentOptOffDay1 = 0;
                currentOptOffDay2 = 0;
                currentOptOffDay3 = 0;
                currentOptOffDay4 = 0;
                noOTpay = false;
                currentMinBreak = 0;
                isOfficeWorker = false;
            }

            if (TimeSpan.Parse(startTime) > TimeSpan.Parse(endTime))
            {
                shifting = true;
            }
            else 
            {
                shifting = false;
            }
        }

        private void GetMyRate(DateTime date) 
        {
            UserRateSchedule URS = new UserRateSchedule();

            URS = URS.GetUserScheduleRateByUserIdDate(Id, date);
            if (URS != null)
            {
                startTime = URS.StartTime == "" ? "08:00" : URS.StartTime;
                endTime = URS.EndTime == "" ? "17:00" : URS.EndTime;
                currentSalary = URS.Salary;
                currentOffDay = URS.OffDay;
                currentSpecialOffDay = URS.SpecialOffDay;
                currentOptOffDay1 = URS.OptionalOffDay1 == null ? 0 : Convert.ToInt32(URS.OptionalOffDay1);
                currentOptOffDay2 = URS.OptionalOffDay2 == null ? 0 : Convert.ToInt32(URS.OptionalOffDay2);
                currentOptOffDay3 = URS.OptionalOffDay3 == null ? 0 : Convert.ToInt32(URS.OptionalOffDay3);
                currentOptOffDay4 = URS.OptionalOffDay4 == null ? 0 : Convert.ToInt32(URS.OptionalOffDay4);
                noOTpay = URS.NoOTPay;
                currentMinBreak = URS.MinsBreak == null ? 0 : Convert.ToInt32(URS.MinsBreak);
                isOfficeWorker = URS.IsOfficeWorker == null ? false : Convert.ToBoolean(URS.IsOfficeWorker);
            }
            else 
            {
                startTime = "08:00";
                endTime = "17:00";
                currentSalary = 0;
                currentOffDay = 0;
                currentSpecialOffDay = 0;
                currentOptOffDay1 = 0;
                currentOptOffDay2 = 0;
                currentOptOffDay3 = 0;
                currentOptOffDay4 = 0;
                noOTpay = false;
                currentMinBreak = 0;
                isOfficeWorker = false;
            }
            if (TimeSpan.Parse(startTime) > TimeSpan.Parse(endTime))
            {
                shifting = true;
            }
            else
            {
                shifting = false;
            }
            TimeSpan stime = TimeSpan.Parse(startTime == "" ? "08:00" : startTime);
            TimeSpan etime = TimeSpan.Parse(endTime == "" ? "17:00" : endTime);
            if (stime > etime)
            {
                minsWorkPerDay = (int)Math.Floor(new TimeSpan(1,0,0,0).TotalMinutes - stime.TotalMinutes);
                minsWorkPerDay += (int)Math.Floor(etime.TotalMinutes);
            }
            else 
            {
                minsWorkPerDay = (int)Math.Floor(etime.TotalMinutes - stime.TotalMinutes);
            }
            minsWorkPerDay -= currentMinBreak;
        }

        public TimeSpan GetMyCutOfTime() 
        {
            TimeSpan cutOfTime = new TimeSpan(00,00,00);
            TimeSpan stime = TimeSpan.Parse(startTime == "" ? "08:00" : startTime);
            TimeSpan etime = TimeSpan.Parse(endTime == "" ? "17:00" : endTime);
            if(stime > etime)
            {
                double result = stime.TotalMinutes - etime.TotalMinutes;
                cutOfTime = etime.Add(TimeSpan.FromMinutes(result/2));
            }
            return cutOfTime;
        }
    }
}