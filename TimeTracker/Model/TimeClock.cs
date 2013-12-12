using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.IO;

namespace TimeTracker.Model
{
    public class TimeClock
    {
        public void GetTime() 
        {
            string timeClockConnectionString = ConfigurationManager.ConnectionStrings["TimeClockConnection"].ConnectionString;
            string[] tcStrign = timeClockConnectionString.Split(';');
            string path = tcStrign[0].Replace("Server=", "").Trim();
            string filename = tcStrign[1].Replace("Database=", "").Trim();
            string fileExt = tcStrign[2].Replace("User Id=", "").Trim();
            filename += "_"+DateTime.Today.ToString("ddMMyy")+"*."+fileExt;
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo[] files = directory.GetFiles(filename);
            foreach (FileInfo file in files) 
            {
                string line = "";
                using(StreamReader sr = new StreamReader(file.FullName))
                {
                    while(!String.IsNullOrEmpty(line = sr.ReadLine()))
                    {
                        
                    }
                }
            }
        }
    }
}