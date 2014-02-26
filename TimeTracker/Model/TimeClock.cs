using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.IO;

namespace TimeTracker.Model
{
    public class TimeClock : T_TimeClock
    {
        public DateTime starttime { get; set; }
        public DateTime endtime { get; set; }


        public TimeClock GetTimeClock(int id) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from t in db.T_TimeClock
                        where t.Id == id
                        select new TimeClock()
                        {
                            Id = t.Id,
                            EmployeeNumber = t.EmployeeNumber,
                            ScanTime = t.ScanTime,
                            CreateDate = t.CreateDate,
                            Filename = t.Filename
                        }).FirstOrDefault();
            
            db.Dispose();

            return data;
        }

        public TimeClock GetStartEndTime(int employeenum, DateTime stime, DateTime etime) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();
            TimeClock result = new TimeClock();

            var data = (from t in db.T_TimeClock
                        where t.EmployeeNumber == employeenum
                        && t.ScanTime >= stime
                        && t.ScanTime <= etime
                        orderby ScanTime ascending
                        select new TimeClock()
                        {
                            Id = t.Id,
                            EmployeeNumber = t.EmployeeNumber,
                            ScanTime = t.ScanTime,
                            CreateDate = t.CreateDate,
                            Filename = t.Filename
                        }).ToList();
            
            db.Dispose();
            result.starttime = DateTime.Now.AddYears(1);

            for (int i = 0; i < data.Count; i++)
            {
                if (i % 2 == 0)
                {
                    if (result.starttime == null || result.starttime > data[i].ScanTime)
                        result.starttime = data[i].ScanTime;
                }
                else
                {
                    if (result.endtime == null || result.endtime < data[i].ScanTime)
                        result.endtime = data[i].ScanTime;
                }
            }

            return data.Count < 2 ? null : result;
        }

        public TimeClock GetStartEndTimeForShifting(int employeenum, DateTime stime, DateTime etime, bool isFirstPart) 
        {
            TimeClock result = new TimeClock();
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from t in db.T_TimeClock
                        where t.EmployeeNumber == employeenum
                        && t.ScanTime >= stime
                        && t.ScanTime <= etime
                        orderby ScanTime ascending
                        select new TimeClock()
                        {
                            Id = t.Id,
                            EmployeeNumber = t.EmployeeNumber,
                            ScanTime = t.ScanTime,
                            CreateDate = t.CreateDate,
                            Filename = t.Filename
                        }).ToList();

            db.Dispose();
            if (data.Count < 2)
            { 
                result = null; 
            }
            else
            {
                result.starttime = DateTime.Now.AddYears(1);
                DateTime midnight = new DateTime();
                midnight = Convert.ToDateTime(etime.ToString("yyyy-MM-dd") + " 00:00:00");
                for (int i = 0; i < data.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        if (result.starttime == null || result.starttime > data[i].ScanTime)
                            result.starttime = data[i].ScanTime;
                    }
                    else
                    {
                        if (result.endtime == null || result.endtime < data[i].ScanTime)
                            result.endtime = data[i].ScanTime;
                    }
                }
                if (isFirstPart) 
                {
                    if (result.starttime > midnight)
                    {
                        result = null;
                    }
                    else 
                    {
                        if (result.endtime > midnight)
                        {
                            result.endtime = midnight;
                        }
                        //else 
                        //{
                        //    result.endtime.AddMinutes(-30); //Subtract 30 min to allow 30 gap in checking
                        //}
                    }
                }
                else
                {
                    if (result.endtime < midnight)
                    {
                        result = null;
                    }
                    else 
                    {
                        if (result.starttime < midnight)
                            result.starttime = midnight;
                    }
                }
            }

            return result;
        }

        public TimeClock GetStartEndTime(int userid, DateTime selectedDate)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();
            TimeClock result = new TimeClock();
            User user = new User();

            user = user.GetUser(userid);

            //DateTime stime = Convert.ToDateTime(selectedDate.ToString("yyyy-MM-dd") + " 00:00");
            //DateTime etime = Convert.ToDateTime(selectedDate.ToString("yyyy-MM-dd") + " 23:59");
            DateTime stime = new DateTime();
            DateTime etime = new DateTime();
            TimeSpan startTime = new TimeSpan();
            TimeSpan endTime = new TimeSpan();
            TimeSpan.TryParse(user.startTime == "" ? "08:00":user.startTime, out startTime);
            TimeSpan.TryParse(user.endTime == "" ? "17:00":user.endTime, out endTime);
            if (startTime > endTime) 
            {
                
            }
            var data = (from t in db.T_TimeClock
                        where t.EmployeeNumber == user.EmployeeNumber
                        && t.ScanTime >= stime
                        && t.ScanTime <= etime
                        orderby ScanTime ascending
                        select new TimeClock()
                        {
                            Id = t.Id,
                            EmployeeNumber = t.EmployeeNumber,
                            ScanTime = t.ScanTime,
                            CreateDate = t.CreateDate,
                            Filename = t.Filename
                        }).ToList();

            db.Dispose();
            result.starttime = DateTime.Now.AddYears(1);
            for (int i = 0; i < data.Count; i++)
            {
                if (i % 2 == 0)
                {
                    if (result.starttime == null || result.starttime > data[i].ScanTime)
                        result.starttime = data[i].ScanTime;
                }
                else
                {
                    if (result.endtime == null || result.endtime < data[i].ScanTime)
                        result.endtime = data[i].ScanTime;
                }
            }

            return data.Count < 2 ? null : result;
        }
    }
}