using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class Time: T_Time
    {
        public Time GetTime(int timeid) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from t in db.T_Time
                        where t.Id == timeid
                        select new Time() {
                        Id = t.Id,
                        Description = t.Description,
                        C24hrConversion = t.C24hrConversion,
                        Position = t.Position
                        }).FirstOrDefault();
            
            db.Dispose();

            return data;
        }

        public Time GetTime(string c24hrval)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from t in db.T_Time
                        where t.C24hrConversion == c24hrval
                        select new Time()
                        {
                            Id = t.Id,
                            Description = t.Description,
                            C24hrConversion = t.C24hrConversion,
                            Position = t.Position
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public Time GetTime(DateTime time)
        {
            string c24hrval = (time.Hour > 9 ? time.Hour.ToString() : "0"+time.Hour) + ":" + (time.Minute > 9 ? time.Minute.ToString() : "0"+time.Minute);
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from t in db.T_Time
                        where t.C24hrConversion == c24hrval
                        select new Time()
                        {
                            Id = t.Id,
                            Description = t.Description,
                            C24hrConversion = t.C24hrConversion,
                            Position = t.Position
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public Time GetTimeByPosition(int timepos)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from t in db.T_Time
                        where t.Position == timepos
                        select new Time()
                        {
                            Id = t.Id,
                            Description = t.Description,
                            C24hrConversion = t.C24hrConversion,
                            Position = t.Position
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public List<Time> GetTimeList()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from t in db.T_Time
                        orderby t.Position ascending
                        select new Time()
                        {
                            Id = t.Id,
                            Description = t.Description,
                            C24hrConversion = t.C24hrConversion,
                            Position = t.Position
                        }).ToList();

            db.Dispose();

            return data;
        }
        public List<Time> GetStartTimeList(int timepos = 999)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from t in db.T_Time
                        where t.Position <= timepos
                        orderby t.Position ascending
                        select new Time()
                        {
                            Id = t.Id,
                            Description = t.Description,
                            C24hrConversion = t.C24hrConversion,
                            Position = t.Position
                        }).ToList();

            db.Dispose();
            if(timepos == 999)
                data.RemoveAt(data.Count - 1); // remove last time element to prevent user from keying last time of the day as start time
            return data;
        }

        public List<Time> GetEndTimeList(string c24hrval,int timepos = 999)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            Time time = new Time();
            time = time.GetTime(c24hrval);

            if (timepos <= time.Position)
                timepos = time.Position + 1;

            var data = (from t in db.T_Time
                        where t.Position > time.Position
                        && t.Position <= timepos
                        orderby t.Position ascending
                        select new Time()
                        {
                            Id = t.Id,
                            Description = t.Description,
                            C24hrConversion = t.C24hrConversion,
                            Position = t.Position
                        }).ToList();

            db.Dispose();

            return data;
        }

        
    }
}