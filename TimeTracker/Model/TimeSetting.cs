using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class TimeSetting : T_TimeSetting
    {
        public TimeSetting GetTimeSetting()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from t in db.T_TimeSetting
                        select new TimeSetting()
                        {
                            Id = t.Id,
                            Interval = t.Interval,
                            LastUpdateDate = t.LastUpdateDate,
                            LastUpdatedBy = t.LastUpdatedBy
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public TimeSetting GetTimeSetting(int id)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from t in db.T_TimeSetting
                        where t.Id == id
                        select new TimeSetting()
                        {
                            Id = t.Id,
                            Interval = t.Interval,
                            LastUpdateDate = t.LastUpdateDate,
                            LastUpdatedBy = t.LastUpdatedBy
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }
    }
}