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

        public List<TimeSetting> GetTimeSettingList()
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from t in db.T_TimeSetting
                        select new TimeSetting()
                        {
                            Id = t.Id,
                            Interval = t.Interval,
                            LastUpdateDate = t.LastUpdateDate,
                            LastUpdatedBy = t.LastUpdatedBy
                        }).ToList();

            db.Dispose();

            return data;
        }

        public void Update(TimeSetting timesetting)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_TimeSetting t_timesetting = db.T_TimeSetting.FirstOrDefault();
                    Parse(t_timesetting, timesetting);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        private void Parse(T_TimeSetting t_timesetting, TimeSetting timesetting)
        {
            t_timesetting.Interval = timesetting.Interval;
            t_timesetting.LastUpdateDate = timesetting.LastUpdateDate;
            t_timesetting.LastUpdatedBy = timesetting.LastUpdatedBy;

        }
    }
}