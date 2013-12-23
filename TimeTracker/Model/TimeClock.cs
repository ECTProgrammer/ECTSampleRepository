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
    }
}