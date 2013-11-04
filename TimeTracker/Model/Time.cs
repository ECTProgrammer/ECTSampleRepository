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

            var data = (from t in db.T_Times
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

            var data = (from t in db.T_Times
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

        public List<Time> GetTimeList()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from t in db.T_Times
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
        public List<Time> GetStartTimeList()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from t in db.T_Times
                        orderby t.Position ascending
                        select new Time()
                        {
                            Id = t.Id,
                            Description = t.Description,
                            C24hrConversion = t.C24hrConversion,
                            Position = t.Position
                        }).ToList();

            db.Dispose();

            data.RemoveAt(data.Count - 1); // remove last time element to prevent user from keying last time of the day as start time
            return data;
        }

        public List<Time> GetEndTimeList(string c24hrval)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            Time time = new Time();
            time = time.GetTime(c24hrval);

            var data = (from t in db.T_Times
                        where t.Position > time.Position
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