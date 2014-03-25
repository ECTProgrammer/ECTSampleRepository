using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TimeTracker.Model;
using System.Configuration;

namespace TimeTracker
{
    public partial class SetupCAPStageMapping : System.Web.UI.Page
    {
        RolesModuleAccess myAccessRights = new RolesModuleAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            JobTracker jobtracker = new JobTracker();
            if (!isValidUser() || (!jobtracker.CanConnectToCAP()))
                Response.Redirect("Login.aspx");
            GetMyAccessRights();
            if (myAccessRights == null)
                Response.Redirect("Dashboard.aspx");
            HttpContext.Current.Session["siteSubHeader"] = "Setup";
            HttpContext.Current.Session["selectedTab"] = "Setup";

            if (!IsPostBack)
            {
                InitializeDropDownDepartment();
                InitializeMainGrid();
            }
        }

        protected void InitializeDropDownDepartment()
        {
            Department department = new Department();
            var departmentList = department.GetDepartmentList();
            if (departmentList.Count > 0)
            {
                if (departmentList[0].Description == "All")
                    departmentList.RemoveAt(0);
            }
            department.Id = 0;
            department.Description = "All";
            departmentList.Insert(0, department);
            dropDownListDepartment.DataSource = departmentList;
            dropDownListDepartment.DataTextField = "Description";
            dropDownListDepartment.DataValueField = "Id";
            dropDownListDepartment.DataBind();
        }

        private void InitializeMainGrid()
        {
            GetMyAccessRights();

            CAPStageMapping capStageMapping = new CAPStageMapping();
            if (myAccessRights.CanAdd == true)
            {
                linkBtnAdd.Visible = true;
            }
            else
                linkBtnAdd.Visible = false;

            List<CAPStageMapping> stagemappings = new List<CAPStageMapping>();
            if (dropDownListDepartment.SelectedItem.Text == "All")
            {
                stagemappings = capStageMapping.GetCapStageMappingList();
            }
            else
            {
                stagemappings = capStageMapping.GetCapStageMappingListByDepartment(Convert.ToInt32(dropDownListDepartment.SelectedItem.Value));
            }

            gridViewStageMapping.DataSource = stagemappings;
            gridViewStageMapping.DataBind();
        }

        protected void dropDownDepartment_Changed(Object sender, EventArgs e)
        {
            InitializeMainGrid();
        }

        protected void linkBtnAdd_Click(object sender, EventArgs e)
        {
            modalLabelError.Text = "";
            modalLabelStageMappingId.Text = "";
            modalLabelError.Visible = false;
            string departmentId = dropDownListDepartment.SelectedItem.Text == "All" ? "" : dropDownListDepartment.SelectedItem.Value.ToString();
            InitializeModalDropDownDepartment(departmentId);
            InitializeModalDropDownJobType(Convert.ToInt32(modalDropDownDepartment.SelectedItem.Value));
            InitializeModalDatabase();
            InitializeModalCAPStages(modalDropDownDatabase.SelectedItem.Value.Trim());
            modalBtnSubmit.Visible = true;
            modalBtnSubmit.CommandArgument = "Add";
            modalBtnDelete.Visible = false;
            programmaticModalPopup.Show();
        }

        protected void gridViewStageMapping_Command(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GetMyAccessRights();

                modalBtnDelete.Visible = myAccessRights.CanDelete == null ? false : Convert.ToBoolean(myAccessRights.CanDelete);
                
                modalLabelError.Text = "";
                modalLabelError.Visible = false;
                int index = Convert.ToInt32(e.CommandArgument);
                int stageMappingId = Convert.ToInt32(((Label)gridViewStageMapping.Rows[index].FindControl("labelStageMappingId")).Text);
                modalLabelStageMappingId.Text = stageMappingId.ToString();

                CAPStageMapping capStageMapping = new CAPStageMapping();
                capStageMapping = capStageMapping.GetCapStageMapping(stageMappingId);

                InitializeModalDropDownDepartment(capStageMapping.DepartmentId.ToString());
                InitializeModalDropDownJobType(Convert.ToInt32(modalDropDownDepartment.SelectedItem.Value),capStageMapping.JobTypeId.ToString().Trim());
                InitializeModalDatabase(capStageMapping.DatabaseMap.Trim());
                InitializeModalCAPStages(modalDropDownDatabase.SelectedItem.Value.Trim(),capStageMapping.SD_Stage_No.ToString().Trim());
                modalBtnSubmit.Visible = Convert.ToBoolean(myAccessRights.CanUpdate);
                modalBtnSubmit.CommandArgument = "Update";
                programmaticModalPopup.Show();
            }
        }

        #region MODAL

        protected void InitializeModalDropDownDepartment(string departmentId = "")
        {
            Department department = new Department();
            var departmentList = department.GetDepartmentList();
            modalDropDownDepartment.DataSource = departmentList;
            modalDropDownDepartment.DataTextField = "Description";
            modalDropDownDepartment.DataValueField = "Id";
            modalDropDownDepartment.DataBind();

            if (departmentId != null && departmentId.Trim() != "")
            {
                foreach (ListItem i in modalDropDownDepartment.Items)
                {
                    if (departmentId.Trim() == i.Value.Trim())
                    {
                        i.Selected = true;
                        break;
                    }
                }
            }
        }

        protected void InitializeModalDropDownJobType(int departmentId,string selectedvalue = "") 
        {
            JobTypeDepartment jobtypeDepartment = new JobTypeDepartment();
            List<JobType> jobTypelist = new List<JobType>();
            jobTypelist = jobtypeDepartment.GetJobTypeList(departmentId);
            modalDropDownJobType.DataSource = jobTypelist;
            modalDropDownJobType.DataTextField = "Description";
            modalDropDownJobType.DataValueField = "Id";
            modalDropDownJobType.DataBind();
            if (selectedvalue != null && selectedvalue.Trim() != "") 
            {
                foreach (ListItem i in modalDropDownJobType.Items)
                {
                    if (i.Value.Trim() == selectedvalue.Trim())
                    {
                        i.Selected = true;
                        break;
                    }
                }
            }
        }

        protected void InitializeModalDatabase(string database = "") 
        {
            //reads the string connection in webconfig file
            string hwConnectionString = ConfigurationManager.ConnectionStrings["CAPHWConnection"].ConnectionString;
            string swConnectionString = ConfigurationManager.ConnectionStrings["CAPSWConnection"].ConnectionString;

            //split the string connection separate each database configuration element
            string[] hwString = hwConnectionString.Split(';');
            string[] swString = swConnectionString.Split(';');

            //replace the word Database with blank to get the Databasename
            string hwDatabase = hwString[1].Replace("Database=", "").Trim();
            string swDatabase = swString[1].Replace("Database=", "").Trim();

            //create the dictionary to store the Software and hardware database
            Dictionary<string, string> dbList = new Dictionary<string, string>();
            dbList.Add("CAP Hardware - "+hwDatabase, "CAPHWConnection");
            dbList.Add("CAP Software - " + swDatabase, "CAPSWConnection");

            //Bind the database list to the database drop down
            modalDropDownDatabase.DataSource = dbList;
            modalDropDownDatabase.DataTextField = "Key";
            modalDropDownDatabase.DataValueField = "Value";
            modalDropDownDatabase.DataBind();

            if (database != null && database.Trim() != "") 
            {
                foreach (ListItem i in modalDropDownDatabase.Items) 
                {
                    if (i.Value.Trim() == database.Trim()) 
                    {
                        i.Selected = true;
                        break;
                    }
                }
            }
        }

        protected void InitializeModalCAPStages(string databasemap,string selectedStage = "") 
        {
            CAPStageMapping capstagemapping = new CAPStageMapping();
            List<CAPStageMapping> capStageList = new List<CAPStageMapping>();
            capStageList = capstagemapping.GetCAPStagesByCAPDatabase(databasemap);

            modalDropDownCapStage.DataSource = capStageList;
            modalDropDownCapStage.DataTextField = "StageDescription";
            modalDropDownCapStage.DataValueField = "SD_Stage_No";
            modalDropDownCapStage.DataBind();

            if (selectedStage != null && selectedStage.Trim() != "") 
            {
                foreach (ListItem i in modalDropDownCapStage.Items) 
                {
                    if (i.Value.Trim() == selectedStage.Trim()) 
                    {
                        i.Selected = true;
                        break;
                    }
                }
            }
        }
        #endregion

        #region COMMAND
        protected void modalDropDownDepartment_Changed(object sender, EventArgs e)
        {
            InitializeModalDropDownJobType(Convert.ToInt32(modalDropDownDepartment.SelectedItem.Value));
            programmaticModalPopup.Show();
        }

        protected void modalDropDownDatabase_Changed(object sender, EventArgs e)
        {
            InitializeModalCAPStages(modalDropDownDatabase.SelectedItem.Value.Trim());
            programmaticModalPopup.Show();
        }

        protected void modalBtnSubmit_Command(object sender, CommandEventArgs e)
        {
            CAPStageMapping capstageMapping = new CAPStageMapping();
            
            bool haserror = false;
            if (e.CommandArgument.ToString().Trim() == "Add")
            {
                capstageMapping = capstageMapping.GetCapStageMapping(capstageMapping.DepartmentId, capstageMapping.JobTypeId);
                if (capstageMapping != null)
                {
                    modalLabelError.Text = "Error: JobType already been map.";
                    modalLabelError.Visible = true;
                    haserror = true;
                }
                capstageMapping = new CAPStageMapping();
            }
            else 
            {
                int mappingId = Convert.ToInt32(modalLabelStageMappingId.Text);
                capstageMapping = capstageMapping.GetCapStageMapping(capstageMapping.DepartmentId, capstageMapping.JobTypeId);
                if (capstageMapping != null && capstageMapping.Id != mappingId)
                {
                    modalLabelError.Text = "Error: JobType already been map.";
                    modalLabelError.Visible = true;
                    haserror = true;
                }
                capstageMapping = new CAPStageMapping();
                capstageMapping.Id = mappingId;
            }
            if (haserror)
            {
                this.programmaticModalPopup.Show();
            }
            else
            {
                capstageMapping.DepartmentId = Convert.ToInt32(modalDropDownDepartment.SelectedItem.Value);
                capstageMapping.JobTypeId = Convert.ToInt32(modalDropDownJobType.SelectedItem.Value);
                capstageMapping.DatabaseMap = modalDropDownDatabase.SelectedItem.Value.Trim();
                capstageMapping.SD_Stage_No = Convert.ToInt32(modalDropDownCapStage.SelectedItem.Value);
                capstageMapping.StageDescription = modalDropDownCapStage.SelectedItem.Text.Trim();
                if (e.CommandArgument.ToString().Trim() == "Add")
                {
                    capstageMapping.Insert(capstageMapping);
                }
                else 
                {
                    capstageMapping.Update(capstageMapping);
                }
                modalLabelError.Text = "";
                modalLabelError.Visible = false;
                this.programmaticModalPopup.Hide();
                InitializeMainGrid();
            }
        }

        protected void modalBtnDelete_Command(object sender, CommandEventArgs e) 
        {
            CAPStageMapping capStageMapping = new CAPStageMapping();
            int mappingId = Convert.ToInt32(modalLabelStageMappingId.Text);

            capStageMapping.Delete(mappingId);
            this.programmaticModalPopup.Hide();
            InitializeMainGrid();
        }
        #endregion

        #region OTHERS
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            foreach (GridViewRow row in gridViewStageMapping.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    row.Attributes["onmouseover"] = "this.style.cursor = 'pointer';this.style.backgroundColor = '#e5f2fc';";
                    row.Attributes["onmouseout"] = "this.style.backgroundColor='#ffffff';";
                    row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gridViewStageMapping, "Select$" + row.DataItemIndex, true);
                }
            }
            base.Render(writer);
        }

        protected bool isValidUser()
        {
            bool isvalid = false;
            if (Session["UserId"] != null)
            {
                int userid = Convert.ToInt32(Session["UserId"]);
                User user = new User();
                user = user.GetUser(userid);
                if (user != null)
                {
                    isvalid = true;
                }
            }
            return isvalid;
        }

        protected void GetMyAccessRights()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            User user = new User();
            user = user.GetUser(userid);
            Module module = new Module();
            module = module.GetModule("SetupCapStageMapping.aspx");
            myAccessRights = myAccessRights.GetRolesModuleAccess(Convert.ToInt32(user.RoleId), module.Id);
        }
        #endregion
    }
}