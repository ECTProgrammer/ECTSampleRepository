using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace TimeTracker.Model
{
    public class UserRateSchedule : T_UserRateSchedule
    {
        //gets UserRateSchedule by Id
        static readonly string PasswordHash = "s@$sw0rD";
        static readonly string SaltKey = "R@&suT02";
        static readonly string VIKey = "@23B84lt23Hs9dG2";

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
                            IsCurrentRate = u.IsCurrentRate,
                            OffDay = u.OffDay,
                            SpecialOffDay = u.SpecialOffDay,
                            NoOTPay = u.NoOTPay,
                            MinsBreak = u.MinsBreak,
                            IsOfficeWorker = u.IsOfficeWorker,
                            OptionalOffDay1 = u.OptionalOffDay1,
                            OptionalOffDay2 = u.OptionalOffDay2,
                            OptionalOffDay3 = u.OptionalOffDay3,
                            OptionalOffDay4 = u.OptionalOffDay4,
                            CreatedDate = u.CreatedDate,
                            LastUpdatedDate = u.LastUpdatedDate,
                            UsePattern = u.UsePattern,
                            OffPattern = u.OffPattern,
                            PatternStartDate = u.PatternStartDate,
                            MonthlySalary = u.MonthlySalary
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
                            IsCurrentRate = u.IsCurrentRate,
                            OffDay = u.OffDay,
                            SpecialOffDay = u.SpecialOffDay,
                            NoOTPay = u.NoOTPay,
                            MinsBreak = u.MinsBreak,
                            IsOfficeWorker = u.IsOfficeWorker,
                            OptionalOffDay1 = u.OptionalOffDay1,
                            OptionalOffDay2 = u.OptionalOffDay2,
                            OptionalOffDay3 = u.OptionalOffDay3,
                            OptionalOffDay4 = u.OptionalOffDay4,
                            CreatedDate = u.CreatedDate,
                            LastUpdatedDate = u.LastUpdatedDate,
                            UsePattern = u.UsePattern,
                            OffPattern = u.OffPattern,
                            PatternStartDate = u.PatternStartDate,
                            MonthlySalary = u.MonthlySalary
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
                            IsCurrentRate = u.IsCurrentRate,
                            OffDay = u.OffDay,
                            SpecialOffDay = u.SpecialOffDay,
                            NoOTPay = u.NoOTPay,
                            MinsBreak = u.MinsBreak,
                            IsOfficeWorker = u.IsOfficeWorker,
                            OptionalOffDay1 = u.OptionalOffDay1,
                            OptionalOffDay2 = u.OptionalOffDay2,
                            OptionalOffDay3 = u.OptionalOffDay3,
                            OptionalOffDay4 = u.OptionalOffDay4,
                            CreatedDate = u.CreatedDate,
                            LastUpdatedDate = u.LastUpdatedDate,
                            UsePattern = u.UsePattern,
                            OffPattern = u.OffPattern,
                            PatternStartDate = u.PatternStartDate,
                            MonthlySalary = u.MonthlySalary
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        //Get specific users current rate
        public UserRateSchedule GetCurrentUserScheduleRateByUserId(int userid)
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
                            IsCurrentRate = u.IsCurrentRate,
                            OffDay = u.OffDay,
                            SpecialOffDay = u.SpecialOffDay,
                            NoOTPay = u.NoOTPay,
                            MinsBreak = u.MinsBreak,
                            IsOfficeWorker = u.IsOfficeWorker,
                            OptionalOffDay1 = u.OptionalOffDay1,
                            OptionalOffDay2 = u.OptionalOffDay2,
                            OptionalOffDay3 = u.OptionalOffDay3,
                            OptionalOffDay4 = u.OptionalOffDay4,
                            CreatedDate = u.CreatedDate,
                            LastUpdatedDate = u.LastUpdatedDate,
                            UsePattern = u.UsePattern,
                            OffPattern = u.OffPattern,
                            PatternStartDate = u.PatternStartDate,
                            MonthlySalary = u.MonthlySalary
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
                            IsCurrentRate = u.IsCurrentRate,
                            OffDay = u.OffDay,
                            SpecialOffDay = u.SpecialOffDay,
                            NoOTPay = u.NoOTPay,
                            MinsBreak = u.MinsBreak,
                            IsOfficeWorker = u.IsOfficeWorker,
                            OptionalOffDay1 = u.OptionalOffDay1,
                            OptionalOffDay2 = u.OptionalOffDay2,
                            OptionalOffDay3 = u.OptionalOffDay3,
                            OptionalOffDay4 = u.OptionalOffDay4,
                            CreatedDate = u.CreatedDate,
                            LastUpdatedDate = u.LastUpdatedDate,
                            UsePattern = u.UsePattern,
                            OffPattern = u.OffPattern,
                            PatternStartDate = u.PatternStartDate,
                            MonthlySalary = u.MonthlySalary
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public UserRateSchedule GetUserScheduleRateByUserIdCreateDate(int userid, DateTime createddate)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_UserRateSchedule
                        where u.UserId == userid
                        && u.CreatedDate == createddate
                        select new UserRateSchedule()
                        {
                            Id = u.Id,
                            UserId = u.UserId,
                            StartTime = u.StartTime,
                            EndTime = u.EndTime,
                            StartDate = u.StartDate,
                            EndDate = u.EndDate,
                            IsCurrentRate = u.IsCurrentRate,
                            OffDay = u.OffDay,
                            SpecialOffDay = u.SpecialOffDay,
                            NoOTPay = u.NoOTPay,
                            MinsBreak = u.MinsBreak,
                            IsOfficeWorker = u.IsOfficeWorker,
                            OptionalOffDay1 = u.OptionalOffDay1,
                            OptionalOffDay2 = u.OptionalOffDay2,
                            OptionalOffDay3 = u.OptionalOffDay3,
                            OptionalOffDay4 = u.OptionalOffDay4,
                            CreatedDate = u.CreatedDate,
                            LastUpdatedDate = u.LastUpdatedDate,
                            UsePattern = u.UsePattern,
                            OffPattern = u.OffPattern,
                            PatternStartDate = u.PatternStartDate,
                            MonthlySalary = u.MonthlySalary
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public UserRateSchedule GetPreviousUserScheduleRateByUserIdLastUpdateDate(int userid, DateTime lastupdatedate)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from u in db.T_UserRateSchedule
                        where u.UserId == userid
                        && u.LastUpdatedDate  <= lastupdatedate
                        && u.CreatedDate < lastupdatedate
                        orderby u.LastUpdatedDate descending,u.CreatedDate descending
                        select new UserRateSchedule()
                        {
                            Id = u.Id,
                            UserId = u.UserId,
                            StartTime = u.StartTime,
                            EndTime = u.EndTime,
                            StartDate = u.StartDate,
                            EndDate = u.EndDate,
                            IsCurrentRate = u.IsCurrentRate,
                            OffDay = u.OffDay,
                            SpecialOffDay = u.SpecialOffDay,
                            NoOTPay = u.NoOTPay,
                            MinsBreak = u.MinsBreak,
                            IsOfficeWorker = u.IsOfficeWorker,
                            OptionalOffDay1 = u.OptionalOffDay1,
                            OptionalOffDay2 = u.OptionalOffDay2,
                            OptionalOffDay3 = u.OptionalOffDay3,
                            OptionalOffDay4 = u.OptionalOffDay4,
                            CreatedDate = u.CreatedDate,
                            LastUpdatedDate = u.LastUpdatedDate,
                            UsePattern = u.UsePattern,
                            OffPattern = u.OffPattern,
                            PatternStartDate = u.PatternStartDate,
                            MonthlySalary = u.MonthlySalary
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public string EncryptSalary(string salaryInText) 
        {
            string data = "";
            if (salaryInText.Trim() != "") 
            {
                data = Encrypt(salaryInText);
            }
            return data;
        }

        public decimal GetMySalary() 
        {
            decimal data = 0;
            if (MonthlySalary != null && MonthlySalary.Trim() != "") 
            {
                data = Convert.ToDecimal(DecryptSalary(MonthlySalary));
            }
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
            t_userRateSchedule.MonthlySalary = userRateSchedule.MonthlySalary;
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
            t_userRateSchedule.CreatedDate = userRateSchedule.CreatedDate;
            t_userRateSchedule.LastUpdatedDate = userRateSchedule.LastUpdatedDate;
            t_userRateSchedule.UsePattern = userRateSchedule.UsePattern;
            t_userRateSchedule.OffPattern = userRateSchedule.OffPattern;
            t_userRateSchedule.PatternStartDate = userRateSchedule.PatternStartDate;
        }

        private static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string DecryptSalary(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

    }
}