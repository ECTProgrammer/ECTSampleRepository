using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class OTRateSetting : T_OTRateSetting
    {
        public OTRateSetting GetOTRateSettingByDate(DateTime date)
        {
            DateTime datetomorrow = new DateTime();
            datetomorrow = DateTime.Now.AddDays(1);
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from o in db.T_OTRateSetting
                        where o.StartDate <= date
                        && (o.EndDate == null ? datetomorrow : o.EndDate) > date
                        select new OTRateSetting()
                        {
                            Id = o.Id,
                            OTExemptedSalary = o.OTExemptedSalary,
                            ExemptedSalaryIncentive = o.ExemptedSalaryIncentive,
                            OTNormalRate = o.OTNormalRate,
                            OTSpecialRate = o.OTSpecialRate,
                            StartDate = o.StartDate,
                            EndDate= o.EndDate
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }
    }
}