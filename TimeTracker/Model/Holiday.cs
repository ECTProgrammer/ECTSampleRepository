using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class Holiday : T_Holiday
    {
        public Holiday GetHoliday(DateTime date)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from d in db.T_Holiday
                        where d.HolidayDate == date
                        select new Holiday()
                        {
                            Id = d.Id,
                            HolidayYear = d.HolidayYear,
                            HolidayDate = d.HolidayDate,
                            Description = d.Description,
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public List<Holiday> GetHolidays(int year)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from d in db.T_Holiday
                        where d.HolidayYear == year
                        select new Holiday()
                        {
                            Id = d.Id,
                            HolidayYear = d.HolidayYear,
                            HolidayDate = d.HolidayDate,
                            Description = d.Description,
                        }).ToList();

            db.Dispose();

            return data;
        }

        public bool IsHoliday(DateTime date) 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();
            DateTime selDate = date.Date;
            var data = (from d in db.T_Holiday
                        where d.HolidayDate == selDate
                        select new Holiday()
                        {
                            Id = d.Id,
                            HolidayYear = d.HolidayYear,
                            HolidayDate = d.HolidayDate,
                            Description = d.Description,
                        }).FirstOrDefault();

            db.Dispose();

            bool result = false;
            if (data != null)
                result = true;
            return result;
        }

        public int GetWorkingDaysInMonth(int userid,DateTime curdate) 
        {
            User user = new User();
            user = user.GetUser(userid, curdate);
            int days = DateTime.DaysInMonth(curdate.Year, curdate.Month);
            DateTime firstDay = Convert.ToDateTime("01 "+curdate.ToString("MMM")+" "+curdate.ToString("yyyy")+" 00:00:00");
            DateTime lastDay = Convert.ToDateTime(days + " " + curdate.ToString("MMM") + " " + curdate.ToString("yyyy") + " 23:00:00");
            
            TimeTrackerEntities db = new TimeTrackerEntities();
            var data = (from d in db.T_Holiday
                        where d.HolidayDate >= firstDay
                        && d.HolidayDate <= lastDay
                        select new Holiday()
                        {
                            Id = d.Id,
                            HolidayYear = d.HolidayYear,
                            HolidayDate = d.HolidayDate,
                            Description = d.Description,
                        }).ToList();
            db.Dispose();
            int offdays = 0;
            int specialdays = 0;
            int optionaloffday1 = 0;
            int optionaloffday2 = 0;
            int optionaloffday3 = 0;
            int optionaloffday4 = 0;
            int count = days/7;
            int remainder = days % 7;
            if (user.currentOffDay != 0) 
            {
                offdays = count;
                int sincelastday = (int)(lastDay.DayOfWeek - user.currentOffDay);
                if (sincelastday < 0) sincelastday += 7;
                if (remainder >= sincelastday)
                    offdays++;
            }
            if (user.currentSpecialOffDay != 0) 
            {
                specialdays = count;
                int sincelastday = (int)(lastDay.DayOfWeek - user.currentOffDay);
                if (sincelastday < 0) sincelastday += 7;
                if (remainder >= sincelastday)
                    specialdays++;
            }
            if (user.currentOffDay != 0)
            {
                optionaloffday1 = count;
                int sincelastday = (int)(lastDay.DayOfWeek - user.currentOffDay);
                if (sincelastday < 0) sincelastday += 7;
                if (remainder >= sincelastday)
                    optionaloffday1++;
            }
            if (user.currentOffDay != 0)
            {
                optionaloffday2 = count;
                int sincelastday = (int)(lastDay.DayOfWeek - user.currentOffDay);
                if (sincelastday < 0) sincelastday += 7;
                if (remainder >= sincelastday)
                    optionaloffday2++;
            }
            if (user.currentOffDay != 0)
            {
                optionaloffday3 = count;
                int sincelastday = (int)(lastDay.DayOfWeek - user.currentOffDay);
                if (sincelastday < 0) sincelastday += 7;
                if (remainder >= sincelastday)
                    optionaloffday3++;
            }
            if (user.currentOffDay != 0)
            {
                optionaloffday4 = count;
                int sincelastday = (int)(lastDay.DayOfWeek - user.currentOffDay);
                if (sincelastday < 0) sincelastday += 7;
                if (remainder >= sincelastday)
                    optionaloffday4++;
            }

            days = days - offdays - specialdays - optionaloffday1 - optionaloffday2 - optionaloffday3 - optionaloffday4;
            foreach (Holiday d in data) 
            {
                if (d.HolidayDate.Day != user.currentOffDay && d.HolidayDate.Day != user.currentSpecialOffDay)
                    days--;
            }
            return days;
        }
    }
}