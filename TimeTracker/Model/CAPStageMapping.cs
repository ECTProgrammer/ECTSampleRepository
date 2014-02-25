using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TimeTracker.Model
{
    public class CAPStageMapping : T_CAPStageMapping
    {
        public string department { get; set; }
        public string jobtype { get; set; }

        public CAPStageMapping GetCapStageMapping(int id)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from c in db.T_CAPStageMapping
                        where c.Id == id
                        select new CAPStageMapping()
                        {
                            Id = c.Id,
                            DepartmentId = c.DepartmentId,
                            JobTypeId = c.JobTypeId,
                            SD_Stage_No = c.SD_Stage_No,
                            DatabaseMap = c.DatabaseMap,
                            StageDescription = c.StageDescription,
                            department = c.M_Department.Description,
                            jobtype = c.M_JobType.Description
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public CAPStageMapping GetCapStageMapping(int departmentId,int jobtypeId,int sd_stage_no,string databasemap)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from c in db.T_CAPStageMapping
                        where c.DepartmentId == departmentId
                        && c.JobTypeId == jobtypeId
                        && c.SD_Stage_No == sd_stage_no
                        && c.DatabaseMap == databasemap
                        select new CAPStageMapping()
                        {
                            Id = c.Id,
                            DepartmentId = c.DepartmentId,
                            JobTypeId = c.JobTypeId,
                            SD_Stage_No = c.SD_Stage_No,
                            DatabaseMap = c.DatabaseMap,
                            StageDescription = c.StageDescription,
                            department = c.M_Department.Description,
                            jobtype = c.M_JobType.Description
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public CAPStageMapping GetCapStageMapping(int departmentId, int jobtypeId)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from c in db.T_CAPStageMapping
                        where c.DepartmentId == departmentId
                        && c.JobTypeId == jobtypeId
                        select new CAPStageMapping()
                        {
                            Id = c.Id,
                            DepartmentId = c.DepartmentId,
                            JobTypeId = c.JobTypeId,
                            SD_Stage_No = c.SD_Stage_No,
                            DatabaseMap = c.DatabaseMap,
                            StageDescription = c.StageDescription,
                            department = c.M_Department.Description,
                            jobtype = c.M_JobType.Description
                        }).FirstOrDefault();

            db.Dispose();

            return data;
        }

        public List<CAPStageMapping> GetCapStageMappingList() 
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from c in db.T_CAPStageMapping
                        orderby c.M_JobType.Description ascending
                        select new CAPStageMapping()
                        {
                            Id = c.Id,
                            DepartmentId = c.DepartmentId,
                            JobTypeId = c.JobTypeId,
                            SD_Stage_No = c.SD_Stage_No,
                            DatabaseMap = c.DatabaseMap,
                            StageDescription = c.StageDescription,
                            department = c.M_Department.Description,
                            jobtype = c.M_JobType.Description
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<CAPStageMapping> GetCapStageMappingListByDepartment(int departmentid)
        {
            TimeTrackerEntities db = new TimeTrackerEntities();

            var data = (from c in db.T_CAPStageMapping
                        where c.DepartmentId == departmentid
                        orderby c.M_JobType.Description ascending
                        select new CAPStageMapping()
                        {
                            Id = c.Id,
                            DepartmentId = c.DepartmentId,
                            JobTypeId = c.JobTypeId,
                            SD_Stage_No = c.SD_Stage_No,
                            DatabaseMap = c.DatabaseMap,
                            StageDescription = c.StageDescription,
                            department = c.M_Department.Description,
                            jobtype = c.M_JobType.Description
                        }).ToList();

            db.Dispose();

            return data;
        }

        public List<CAPStageMapping> GetCAPStagesByCAPDatabase(string databaselocation) 
        {
            //Gets all stages from STAGE_DEFINITION table in CAP by databaselocation
            //databaselocation is set in WebConfig. (CAPHWConnection or CAPSWConnection)
            List<CAPStageMapping> capStageList = new List<CAPStageMapping>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[databaselocation].ToString()))
            {
                SqlCommand cmd = new SqlCommand("Select  SD_Stage_No,SD_Stage_Desc from STAGE_DEFINITION ", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CAPStageMapping capStage = new CAPStageMapping();
                    capStage.SD_Stage_No = Convert.ToInt32(reader["SD_Stage_No"].ToString());
                    capStage.StageDescription = reader["SD_Stage_Desc"].ToString();
                    capStage.DatabaseMap = databaselocation;
                    capStageList.Add(capStage);
                }
            }
            return capStageList;
        }


        public void Insert(CAPStageMapping capstagemapping)
        {
            T_CAPStageMapping t_capstagemapping = new T_CAPStageMapping();
            ParseMapping(t_capstagemapping, capstagemapping);

            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    db.T_CAPStageMapping.Add(t_capstagemapping);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Delete(int id)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_CAPStageMapping t_capstagemapping = new T_CAPStageMapping();
                    t_capstagemapping = db.T_CAPStageMapping.FirstOrDefault(p => p.Id == id);
                    db.T_CAPStageMapping.Remove(t_capstagemapping);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void Update(CAPStageMapping capstagemapping)
        {
            using (TimeTrackerEntities db = new TimeTrackerEntities())
            {
                try
                {
                    T_CAPStageMapping t_capstagemapping = db.T_CAPStageMapping.FirstOrDefault(p => p.Id == capstagemapping.Id);
                    ParseMapping(t_capstagemapping, capstagemapping);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        private void ParseMapping(T_CAPStageMapping t_capstagemapping, CAPStageMapping capstagemapping) 
        {
            t_capstagemapping.Id = capstagemapping.Id;
            t_capstagemapping.DepartmentId = capstagemapping.DepartmentId;
            t_capstagemapping.JobTypeId = capstagemapping.JobTypeId;
            t_capstagemapping.SD_Stage_No = capstagemapping.SD_Stage_No;
            t_capstagemapping.DatabaseMap = capstagemapping.DatabaseMap;
            t_capstagemapping.StageDescription = capstagemapping.StageDescription;
        }
    }
}