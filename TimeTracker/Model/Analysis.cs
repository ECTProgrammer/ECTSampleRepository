using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class Analysis
    {
        public string jobtype { get; set; }
        public string totalworktime { get; set; }
        public string totalforapproval { get; set; }
        public string totalrejectedtime { get; set; }
        public string totalunclosedjobs { get; set; }

        public List<Analysis> GetAnalysis(int departmentid, DateTime startdate, DateTime enddate) 
        {
            List<Analysis> data = new List<Analysis>();
            JobTracker jobtracker = new JobTracker();
            List<JobTracker> joblist = new List<JobTracker>();
            JobTypeDepartment jobtypeDepartment = new JobTypeDepartment();
            List<JobType> jobtypelist = new List<JobType>();
            if (departmentid == 0)
            {
                //joblist = jobtracker.GetJobTrackerForAnalysis(startdate, enddate);
                JobType jobtype = new JobType();
                jobtypelist = jobtype.GetJobTypeList();
            }
            else
            {
                //joblist = jobtracker.GetJobTrackerForAnalysis(departmentid, startdate, enddate);
                jobtypelist = jobtypeDepartment.GetJobTypeList(departmentid);
            }
            for (int i = 0; i < jobtypelist.Count; i++) 
            {
                Analysis newAnalysis = new Analysis();
                newAnalysis.jobtype = jobtypelist[i].Description;
                newAnalysis.totalworktime = jobtracker.GetTotalHours(jobtypelist[i].Id, startdate, enddate, "Approved",departmentid);
                newAnalysis.totalforapproval = jobtracker.GetTotalHours(jobtypelist[i].Id, startdate, enddate, "For Approval", departmentid);
                newAnalysis.totalrejectedtime = jobtracker.GetTotalHours(jobtypelist[i].Id, startdate, enddate, "Rejected", departmentid);
                newAnalysis.totalunclosedjobs = jobtracker.GetTotalUnclosedJobs(jobtypelist[i].Id, startdate, enddate, "Pending", departmentid).ToString();
                data.Add(newAnalysis);
            }

            return data;
        }

        public List<Analysis> GetAnalysis(int departmentid, DateTime startdate, DateTime enddate,int userid)
        {
            List<Analysis> data = new List<Analysis>();
            JobTracker jobtracker = new JobTracker();
            List<JobTracker> joblist = new List<JobTracker>();
            JobTypeDepartment jobtypeDepartment = new JobTypeDepartment();
            List<JobType> jobtypelist = new List<JobType>();
            jobtypelist = jobtypeDepartment.GetJobTypeList(departmentid);

            for (int i = 0; i < jobtypelist.Count; i++)
            {
                Analysis newAnalysis = new Analysis();
                newAnalysis.jobtype = jobtypelist[i].Description;
                newAnalysis.totalworktime = jobtracker.GetTotalHours(jobtypelist[i].Id,userid, startdate, enddate, "Approved", departmentid);
                newAnalysis.totalforapproval = jobtracker.GetTotalHours(jobtypelist[i].Id, userid, startdate, enddate, "For Approval", departmentid);
                newAnalysis.totalrejectedtime = jobtracker.GetTotalHours(jobtypelist[i].Id, userid, startdate, enddate, "Rejected", departmentid);
                newAnalysis.totalunclosedjobs = jobtracker.GetTotalUnclosedJobs(jobtypelist[i].Id, userid, startdate, enddate, "Pending", departmentid).ToString();
                data.Add(newAnalysis);
            }

            return data;

        }

    }
}