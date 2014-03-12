using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class UserRateSchedule : T_UserRateSchedule
    {
        //gets UserRateSchedule by Id
        public UserRateSchedule GetUserScheduleRate(int id)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_UserRateSchedule
                        where u.Id == id
                        select new UserRateSchedule()
                        {
                            Id = u.Id,
                            UserId = u.UserId,
                            StartTime = u.StartTime,
                            EndTime = u.EndTime,
                            StartDate = u.StartDate,
                            EndDate = u.EndDate,
                            Salary = u.Salary,
                            IsCurrentRate = u.IsCurrentRate,
                            OffDay = u.OffDay,
                            SpecialOffDay = u.SpecialOffDay,
                            NoOTPay = u.NoOTPay,
                            MinsBreak = u.MinsBreak,
                            IsOfficeWorker = u.IsOfficeWorker,
                            OptionalOffDay1 = u.OptionalOffDay1,
                            OptionalOffDay2 = u.OptionalOffDay2,
                            OptionalOffDay3 = u.OptionalOffDay3,
                            OptionalOffDay4 = u.OptionalOffDay4
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }
       
        //gets all UserScheduleRates data of a specific user
        public List<UserRateSchedule> GetUserScheduleRatesByUserId(int userid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_UserRateSchedule
                        where u.UserId == userid
                        select new UserRateSchedule()
                        {
                            Id = u.Id,
                            UserId = u.UserId,
                            StartTime = u.StartTime,
                            EndTime = u.EndTime,
                            StartDate = u.StartDate,
                            EndDate = u.EndDate,
                            Salary = u.Salary,
                            IsCurrentRate = u.IsCurrentRate,
                            OffDay = u.OffDay,
                            SpecialOffDay = u.SpecialOffDay,
                            NoOTPay = u.NoOTPay,
                            MinsBreak = u.MinsBreak,
                            IsOfficeWorker = u.IsOfficeWorker,
                            OptionalOffDay1 = u.OptionalOffDay1,
                            OptionalOffDay2 = u.OptionalOffDay2,
                            OptionalOffDay3 = u.OptionalOffDay3,
                            OptionalOffDay4 = u.OptionalOffDay4
                        }).ToList();

            db.Dispose();

            return data;
        }

        //get all UserScheduleRates data for a specific user where IsCurrentRate is set to true
        public List<UserRateSchedule> GetCurrentUserScheduleRatesByUserId(int userid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_UserRateSchedule
                        where u.UserId == userid
                        select new UserRateSchedule()
                        {
                            Id = u.Id,
                            UserId = u.UserId,
                            StartTime = u.StartTime,
                            EndTime = u.EndTime,
                            StartDate = u.StartDate,
                            EndDate = u.EndDate,
                            Salary = u.Salary,
                            IsCurrentRate = u.IsCurrentRate,
                            OffDay = u.OffDay,
                            SpecialOffDay = u.SpecialOffDay,
                            NoOTPay = u.NoOTPay,
                            MinsBreak = u.MinsBreak,
                            IsOfficeWorker = u.IsOfficeWorker,
                            OptionalOffDay1 = u.OptionalOffDay1,
                            OptionalOffDay2 = u.OptionalOffDay2,
                            OptionalOffDay3 = u.OptionalOffDay3,
                            OptionalOffDay4 = u.OptionalOffDay4
                        }).ToList();

            db.Dispose();

            return data;
        }

        //gets the UserScheduleRate Data base on the date provided for a specific user
        public UserRateSchedule GetUserScheduleRateByUserIdDate(int userid,DateTime date)
        {
            DateTime datetomorrow = new DateTime();
            datetomorrow = DateTime.Now.AddDays(1);
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_UserRateSchedule
                        where u.UserId == userid
                        && u.StartDate <= date
                        && (u.EndDate == null ? datetomorrow : u.EndDate) > date
                        select new UserRateSchedule()
                        {
                            Id = u.Id,
                            UserId = u.UserId,
                            StartTime = u.StartTime,
                            EndTime = u.EndTime,
                            StartDate = u.StartDate,
                            EndDate = u.EndDate,
                            Salary = u.Salary,
                            IsCurrentRate = u.IsCurrentRate,
                            OffDay = u.OffDay,
                            SpecialOffDay = u.SpecialOffDay,
                            NoOTPay = u.NoOTPay,
                            MinsBreak = u.MinsBreak,
                            IsOfficeWorker = u.IsOfficeWorker,
                            OptionalOffDay1 = u.OptionalOffDay1,
                            OptionalOffDay2 = u.OptionalOffDay2,
                            OptionalOffDay3 = u.OptionalOffDay3,
                            OptionalOffDay4 = u.OptionalOffDay4
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        //Get specific users current rate
        public UserRateSchedule GetUserScheduleRateCurrentRate(int userid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_UserRateSchedule
                        where u.UserId == userid
                        && u.IsCurrentRate == true
                        select new UserRateSchedule()
                        {
                            Id = u.Id,
                            UserId = u.UserId,
                            StartTime = u.StartTime,
                            EndTime = u.EndTime,
                            StartDate = u.StartDate,
                            EndDate = u.EndDate,
                            Salary = u.Salary,
                            IsCurrentRate = u.IsCurrentRate,
                            OffDay = u.OffDay,
                            SpecialOffDay = u.SpecialOffDay,
                            NoOTPay = u.NoOTPay,
                            MinsBreak = u.MinsBreak,
                            IsOfficeWorker = u.IsOfficeWorker,
                            OptionalOffDay1 = u.OptionalOffDay1,
                            OptionalOffDay2 = u.OptionalOffDay2,
                            OptionalOffDay3 = u.OptionalOffDay3,
                            OptionalOffDay4 = u.OptionalOffDay4
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        //Get specific users schedule where startdate equals provided parameter
        public UserRateSchedule GetUserScheduleRateByUserIdStartDate(int userid, DateTime startdate)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_UserRateSchedule
                        where u.UserId == userid
                        && u.StartDate == startdate
                        select new UserRateSchedule()
                        {
                            Id = u.Id,
                            UserId = u.UserId,
                            StartTime = u.StartTime,
                            EndTime = u.EndTime,
                            StartDate = u.StartDate,
                            EndDate = u.EndDate,
                            Salary = u.Salary,
                            IsCurrentRate = u.IsCurrentRate,
                            OffDay = u.OffDay,
                            SpecialOffDay = u.SpecialOffDay,
                            NoOTPay = u.NoOTPay,
                            MinsBreak = u.MinsBreak,
                            IsOfficeWorker = u.IsOfficeWorker,
                            OptionalOffDay1 = u.OptionalOffDay1,
                            OptionalOffDay2 = u.OptionalOffDay2,
                            OptionalOffDay3 = u.OptionalOffDay3,
                            OptionalOffDay4 = u.OptionalOffDay4
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public void Insert(UserRateSchedule userRateSchedule)
        {
            T_UserRateSchedule t_userRateSchedule = new T_UserRateSchedule();
            Parse(t_userRateSchedule, userRateSchedule);

            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    db.T_UserRateSchedule.Add(t_userRateSchedule);
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
                    T_UserRateSchedule t_userRateSchedule = new T_UserRateSchedule();
                    t_userRateSchedule = db.T_UserRateSchedule.FirstOrDefault(u => u.Id == id);
                    db.T_UserRateSchedule.Remove(t_userRateSchedule);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        //Update User record in the database
        public void Update(UserRateSchedule userRateSchedule)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_UserRateSchedule t_userRateSchedule = db.T_UserRateSchedule.FirstOrDefault(u => u.Id == userRateSchedule.Id);
                    Parse(t_userRateSchedule, userRateSchedule);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        private void Parse(T_UserRateSchedule t_userRateSchedule, UserRateSchedule userRateSchedule) 
        {
            t_userRateSchedule.Id = userRateSchedule.Id;
            t_userRateSchedule.UserId = userRateSchedule.UserId;
            t_userRateSchedule.StartTime = userRateSchedule.StartTime;
            t_userRateSchedule.EndTime = userRateSchedule.EndTime;
            t_userRateSchedule.StartDate = userRateSchedule.StartDate;
            t_userRateSchedule.EndDate = userRateSchedule.EndDate;
            t_userRateSchedule.Salary = userRateSchedule.Salary;
            t_userRateSchedule.IsCurrentRate = userRateSchedule.IsCurrentRate;
            t_userRateSchedule.OffDay = userRateSchedule.OffDay;
            t_userRateSchedule.SpecialOffDay = userRateSchedule.SpecialOffDay;
            t_userRateSchedule.NoOTPay = userRateSchedule.NoOTPay;
            t_userRateSchedule.MinsBreak = userRateSchedule.MinsBreak;
            t_userRateSchedule.IsOfficeWorker = userRateSchedule.IsOfficeWorker;
            t_userRateSchedule.OptionalOffDay1 = userRateSchedule.OptionalOffDay1;
            t_userRateSchedule.OptionalOffDay2 = userRateSchedule.OptionalOffDay2;
            t_userRateSchedule.OptionalOffDay3 = userRateSchedule.OptionalOffDay3;
            t_userRateSchedule.OptionalOffDay4 = userRateSchedule.OptionalOffDay4;
        }

    }
}