﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Model
{
    public class Analysis
    {
        public string jobtypeid { get; set; }
        public string jobtype { get; set; }
        public string totalworktime { get; set; }
        public string totalforapproval { get; set; }
        public string totalrejectedtime { get; set; }
        public string totalunclosedjobs { get; set; }
        public string totaljobTime { get; set; }

        public List<Analysis> GetAnalysis(int departmentid, DateTime startdate, DateTime enddate, string stringjobid,string customer,int roleid) 
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
                jobtypelist = jobtype.GetJobTypeListByRoleId(roleid);
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
                newAnalysis.jobtypeid = jobtypelist[i].Id.ToString();
                newAnalysis.totalworktime = jobtracker.GetTotalHours(jobtypelist[i].Id, startdate, enddate, "Approved", departmentid, stringjobid,customer);
                newAnalysis.totalforapproval = jobtracker.GetTotalHours(jobtypelist[i].Id, startdate, enddate, "For Approval", departmentid, stringjobid,customer);
                newAnalysis.totaljobTime = "";
                if (stringjobid.Trim() != "" || customer.Trim() != "") 
                {
                    newAnalysis.totaljobTime = jobtracker.GetTotalHours(jobtypelist[i].Id, Convert.ToDateTime("1900-01-01"), DateTime.Now, "Approved", departmentid, stringjobid, customer);
                }
                //newAnalysis.totalrejectedtime = jobtracker.GetTotalHours(jobtypelist[i].Id, startdate, enddate, "Rejected", departmentid, stringjobid,customer);
                //newAnalysis.totalunclosedjobs = jobtracker.GetTotalUnclosedJobs(jobtypelist[i].Id, startdate, enddate, "Pending", departmentid, stringjobid).ToString();
                data.Add(newAnalysis);
            }

            return data;
        }

        public List<Analysis> GetAnalysis(DateTime startdate, DateTime enddate,int userid,string stringjobid,string customer)
        {
            List<Analysis> data = new List<Analysis>();
            JobTracker jobtracker = new JobTracker();
            List<JobTracker> joblist = new List<JobTracker>();
            JobTypeDepartment jobtypeDepartment = new JobTypeDepartment();
            List<JobType> jobtypelist = new List<JobType>();
            User user = new User();
            user = user.GetUser(userid);
            int departmentid = Convert.ToInt32(user.DepartmentId);
            jobtypelist = jobtypeDepartment.GetJobTypeList(departmentid);

            for (int i = 0; i < jobtypelist.Count; i++)
            {
                Analysis newAnalysis = new Analysis();
                newAnalysis.jobtype = jobtypelist[i].Description;
                newAnalysis.jobtypeid = jobtypelist[i].Id.ToString();
                newAnalysis.totalworktime = jobtracker.GetTotalHours(jobtypelist[i].Id, userid, startdate, enddate, "Approved", departmentid, stringjobid,customer);
                newAnalysis.totalforapproval = jobtracker.GetTotalHours(jobtypelist[i].Id, userid, startdate, enddate, "For Approval", departmentid, stringjobid,customer);
                newAnalysis.totaljobTime = "";
                if (stringjobid.Trim() != "" || customer.Trim() != "")
                {
                    newAnalysis.totaljobTime = jobtracker.GetTotalHours(jobtypelist[i].Id, Convert.ToDateTime("1900-01-01"), DateTime.Now, "Approved", departmentid, stringjobid, customer);
                }
                //newAnalysis.totalrejectedtime = jobtracker.GetTotalHours(jobtypelist[i].Id, userid, startdate, enddate, "Rejected", departmentid, stringjobid,customer);
                //newAnalysis.totalunclosedjobs = jobtracker.GetTotalUnclosedJobs(jobtypelist[i].Id, userid, startdate, enddate, "Pending", departmentid, stringjobid).ToString();
                data.Add(newAnalysis);
            }

            return data;

        }

    }
}