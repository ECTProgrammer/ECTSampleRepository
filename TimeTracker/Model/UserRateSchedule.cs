﻿using System;
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
                            BaseRate = u.BaseRate,
                            OTRate = u.OTRate,
                            SpecialRate = u.SpecialRate,
                            IsCurrentRate = u.IsCurrentRate
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
                            BaseRate = u.BaseRate,
                            OTRate = u.OTRate,
                            SpecialRate = u.SpecialRate,
                            IsCurrentRate = u.IsCurrentRate
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
                            BaseRate = u.BaseRate,
                            OTRate = u.OTRate,
                            SpecialRate = u.SpecialRate,
                            IsCurrentRate = u.IsCurrentRate
                        }).ToList();

            db.Dispose();

            return data;
        }

        //gets the UserScheduleRate Data base on the date provided for a specific user
        public UserRateSchedule GetUserScheduleRateByUserIdDate(int userid,DateTime date)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_UserRateSchedule
                        where u.UserId == userid
                        && u.StartDate <= date
                        && (u.EndDate == null ? DateTime.Now.AddDays(1) : u.EndDate) > date
                        select new UserRateSchedule()
                        {
                            Id = u.Id,
                            UserId = u.UserId,
                            StartTime = u.StartTime,
                            EndTime = u.EndTime,
                            StartDate = u.StartDate,
                            EndDate = u.EndDate,
                            BaseRate = u.BaseRate,
                            OTRate = u.OTRate,
                            SpecialRate = u.SpecialRate,
                            IsCurrentRate = u.IsCurrentRate
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
                            BaseRate = u.BaseRate,
                            OTRate = u.OTRate,
                            SpecialRate = u.SpecialRate,
                            IsCurrentRate = u.IsCurrentRate
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
                            BaseRate = u.BaseRate,
                            OTRate = u.OTRate,
                            SpecialRate = u.SpecialRate,
                            IsCurrentRate = u.IsCurrentRate
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
            t_userRateSchedule.BaseRate = userRateSchedule.BaseRate;
            t_userRateSchedule.OTRate = userRateSchedule.OTRate;
            t_userRateSchedule.SpecialRate = userRateSchedule.SpecialRate;
            t_userRateSchedule.IsCurrentRate = userRateSchedule.IsCurrentRate;
        }

    }
}