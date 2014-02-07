using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeTracker.Model;
using System.Data;

namespace TimeTracker
{
    public partial class SetupJobType : System.Web.UI.Page
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
                InitializeGridViewJobType();
            }
        }

        #region Initialize
        protected void InitializeGridViewJobType()
        {
            GetMyAccessRights();
            if (myAccessRights.CanAdd == true)
                linkBtnAdd.Visible = true;
            else
                linkBtnAdd.Visible = false;
            JobTypeDepartment jobtypeDepartment = new JobTypeDepartment();
            JobType jobtype = new JobType();
            List<JobType> jobtypeList = new List<JobType>();

            if (dropDownListDepartment.SelectedItem.Text == "All")
                jobtypeList = jobtype.GetJobTypeList();
            else
                jobtypeList = jobtypeDepartment.GetJobTypeList(Convert.ToInt32(dropDownListDepartment.SelectedItem.Value));

            gridViewJobType.DataSource = jobtypeList;
            gridViewJobType.DataBind();
        }

        protected void InitializeDropDownDepartment()
        {
            Department department = new Department();
            var departmentList = department.GetDepartmentList();
            if (departmentList.Count > 1) 
            {
                department.Description = "All";
                department.Id = 0;
                departmentList.Insert(0, department);
            }

            dropDownListDepartment.DataSource = departmentList;
            dropDownListDepartment.DataTextField = "Description";
            dropDownListDepartment.DataValueField = "Id";
            dropDownListDepartment.DataBind();
        }
        #endregion

        #region COMMAND
        protected void dropDownDepartment_Changed(object sender, EventArgs e)
        {
            InitializeGridViewJobType();
        }

        protected void linkBtnAdd_Click(object sender, EventArgs e)
        {
            //InitializeModalDropDownDepartment(dropDownListDepartment.SelectedItem.Value);
            InitializeModalGridViewDepartment();
            SetStatus(true);
            modalLabelJobTypeId.Text = "";
            modalChkboxAll.Checked = false;
            modalLabelError.Text = "";
            modalLabelError.Visible = false;
            modalTxtBoxDescription.Text = "";
            modalTxtBoxPosition.Text = "";
            modalTxtBoxAcronym.Text = "";
            modalBtnSubmit.CommandArgument = "Add";
            modalChkBoxComputeTime.Checked = false;
            modalChkBoxRequiredJobId.Checked = false;
            modalChkBoxShowJobOverview.Checked = false;

            this.programmaticModalPopup.Show();
        }

        protected void gridViewJobType_Command(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GetMyAccessRights();
                
                modalLabelError.Text = "";
                modalLabelError.Visible = false;
                modalChkboxAll.Checked = false;
                int index = Convert.ToInt32(e.CommandArgument);
                int jobtypeId = Convert.ToInt32(((Label)gridViewJobType.Rows[index].FindControl("labelJobTypeId")).Text);
                modalBtnSubmit.CommandArgument = "Update";
                JobType jobtype = new JobType();
                jobtype = jobtype.GetJobType(jobtypeId);
                modalLabelJobTypeId.Text = jobtype.Id.ToString();
                modalTxtBoxDescription.Text = jobtype.Description;
                modalTxtBoxAcronym.Text = jobtype.Acronym;
                modalTxtBoxPosition.Text = jobtype.Position.ToString();
                modalChkBoxComputeTime.Checked = Convert.ToBoolean(jobtype.ComputeTime);
                modalChkBoxRequiredJobId.Checked = Convert.ToBoolean(jobtype.RequiredJobId);
                modalChkBoxShowJobOverview.Checked = Convert.ToBoolean(jobtype.ShowInJobOverview);
                InitializeModalGridViewDepartment(jobtype.Id);
                //InitializeModalDropDownDepartment(jobtype.DepartmentId.ToString());
                SetStatus(Convert.ToBoolean(myAccessRights.CanUpdate));
                this.programmaticModalPopup.Show();
            }
        }
        #endregion

        #region MODAL

        protected void modalDescription_Changed(object sender, EventArgs e) 
        {
            string errorMsg = "Description already used. ";
            if (modalTxtBoxDescription.Text.Trim() != "")
            {
                JobType jobtype = new JobType();
                jobtype = jobtype.GetJobTypeByDescription(modalTxtBoxDescription.Text.Trim());
                
                if (jobtype != null)
                {
                    if (modalLabelJobTypeId.Text.Trim() == "")
                    {
                        modalLabelError.Text += errorMsg;
                        modalLabelError.Visible = true;
                    }
                    else
                    {
                        JobType j = jobtype.GetJobType(Convert.ToInt32(modalLabelJobTypeId.Text));
                        if (j.Description.Trim().Equals(modalTxtBoxDescription.Text.Trim(), StringComparison.OrdinalIgnoreCase) == true)
                        {
                            modalLabelError.Text += errorMsg;
                            modalLabelError.Visible = true;
                        }
                        else
                        {
                            modalLabelError.Text = modalLabelError.Text.Replace(errorMsg, "").Trim();
                        }
                    }
                }
                else
                {
                    modalLabelError.Text = modalLabelError.Text.Replace(errorMsg, "").Trim();
                }
            }
            else 
            {
                modalLabelError.Text = modalLabelError.Text.Replace(errorMsg, "").Trim();
            }
            if (modalLabelError.Text.Trim().Length == 0) 
            {
                modalLabelError.Visible = false;
            }
            this.programmaticModalPopup.Show();
        }

        protected void modalAcronym_Changed(object sender, EventArgs e)
        {
            string errorMsg = "Acronym already used.";
            if (modalTxtBoxAcronym.Text.Trim() != "")
            {
                JobType jobtype = new JobType();
                jobtype = jobtype.GetJobTypeByAcronym(modalTxtBoxAcronym.Text.Trim());

                if (jobtype != null)
                {
                    if (modalLabelJobTypeId.Text.Trim() == "")
                    {
                        modalLabelError.Text += errorMsg;
                        modalLabelError.Visible = true;
                    }
                    else
                    {
                        JobType j = jobtype.GetJobType(Convert.ToInt32(modalLabelJobTypeId.Text));
                        if (j.Acronym.Trim().Equals(modalTxtBoxAcronym.Text.Trim(), StringComparison.OrdinalIgnoreCase) == true)
                        {
                            modalLabelError.Text += errorMsg;
                            modalLabelError.Visible = true;
                        }
                        else
                        {
                            modalLabelError.Text = modalLabelError.Text.Replace(errorMsg, "").Trim();
                        }
                    }
                }
                else
                {
                    modalLabelError.Text = modalLabelError.Text.Replace(errorMsg, "").Trim();
                }
            }
            else
            {
                modalLabelError.Text = modalLabelError.Text.Replace(errorMsg, "").Trim();
            }
            if (modalLabelError.Text.Trim().Length == 0)
            {
                modalLabelError.Visible = false;
            }
            this.programmaticModalPopup.Show();
        }

        private void InitializeModalGridViewDepartment(int jobtypeid = 0) 
        {
            Department department = new Department();
            var departmentList = department.GetDepartmentList();
            modalGridViewDepartment.DataSource = departmentList;
            modalGridViewDepartment.DataBind();

            if (jobtypeid > 0) 
            {
                InitializeModalDepartmentCheckBox(jobtypeid);
                if (modalGridViewDepartment.Rows.Count > 1)
                    modalChkboxAll.Visible = true;
                else
                    modalChkboxAll.Visible = false;
            }
        }

        private void InitializeModalDepartmentCheckBox(int jobtypeid) 
        {
            JobTypeDepartment jobtypeDepartment = new JobTypeDepartment();
            var jobdeptlist = jobtypeDepartment.GetJobTypeDepartmentListByJobType(jobtypeid);
            for (int i = 0; i < modalGridViewDepartment.Rows.Count; i++)
            {
                Label labelDeptId = (Label)modalGridViewDepartment.Rows[i].FindControl("modalLabelDepartmentId");
                CheckBox cbSelect = (CheckBox)modalGridViewDepartment.Rows[i].FindControl("modalChkBoxSelect");
                TextBox tbPosition = (TextBox)modalGridViewDepartment.Rows[i].FindControl("modalTxtBoxDeptPosition");
                for (int j = 0; j < jobdeptlist.Count; j++) 
                {
                    if (labelDeptId.Text.Trim() == jobdeptlist[j].DepartmentId.ToString()) 
                    {
                        cbSelect.Checked = true;
                        tbPosition.Text = jobdeptlist[j].Position == null ? "" : jobdeptlist[j].Position.ToString();
                        labelDeptId.ToolTip = jobdeptlist[j].Id.ToString();
                        jobdeptlist.RemoveAt(j);
                        break;
                    }
                }
            }

            
        }

        protected void modalChkboxAll_Changed(object sender, EventArgs e) 
        {
            for (int i = 0; i < modalGridViewDepartment.Rows.Count; i++)
            {
                CheckBox cbSelect = (CheckBox)modalGridViewDepartment.Rows[i].FindControl("modalChkBoxSelect");
                cbSelect.Checked = modalChkboxAll.Checked;
            }
            this.programmaticModalPopup.Show();
        }

        protected void modalBtnSubmit_Command(object sender, CommandEventArgs e)
        {
            if (modalLabelError.Visible == true)
                this.programmaticModalPopup.Show();
            else
            {
                bool ischkBoxEmpty = IsDeptCheckBoxEmpty();
                int userid = Convert.ToInt32(Session["UserId"]);
                JobType jobtype = new JobType();
                if (modalLabelJobTypeId.Text.Trim() != "")
                {
                    jobtype = jobtype.GetJobType(Convert.ToInt32(modalLabelJobTypeId.Text));
                }
                jobtype.Description = modalTxtBoxDescription.Text.Trim();
                jobtype.Acronym = modalTxtBoxAcronym.Text.Trim();
                jobtype.Position = Convert.ToInt32(modalTxtBoxPosition.Text);
                jobtype.LastUpdateDate = DateTime.Now;
                jobtype.LastUpdatedBy = userid;
                //jobtype.DepartmentId = Convert.ToInt32(modalDropDownDepartment.SelectedItem.Value);
                if (modalChkBoxRequiredJobId.Checked == true)
                    jobtype.RequiredJobId = true;
                else
                    jobtype.RequiredJobId = false;
                if (modalChkBoxComputeTime.Checked == true)
                    jobtype.ComputeTime = true;
                else
                    jobtype.ComputeTime = false;
                if (modalChkBoxShowJobOverview.Checked == true)
                    jobtype.ShowInJobOverview = true;
                else
                    jobtype.ShowInJobOverview = false;
                if (e.CommandArgument.ToString() == "Add" && ischkBoxEmpty == false)
                {
                    jobtype.CreateDate = DateTime.Now;
                    jobtype.CreatedBy = userid;
                    jobtype.Insert(jobtype);
                    jobtype = jobtype.GetJobTypeByAcronym(modalTxtBoxAcronym.Text.Trim());
                    JobTypeDepartmentAction(jobtype.Id);
                    this.programmaticModalPopup.Hide();
                }
                else if (e.CommandArgument.ToString() == "Update" && ischkBoxEmpty == false)
                {
                    jobtype.Update(jobtype);
                    JobTypeDepartmentAction(jobtype.Id);
                    this.programmaticModalPopup.Hide();
                }
                else if (e.CommandArgument.ToString() == "Update" && ischkBoxEmpty == true) 
                {
                    JobTypeDepartmentAction(jobtype.Id);
                    jobtype.Delete(jobtype.Id);
                    this.programmaticModalPopup.Hide();
                } 

                InitializeGridViewJobType();
            }
        }

        private void SetStatus(bool value)
        {
            modalTxtBoxDescription.Enabled = value;
            modalTxtBoxAcronym.Enabled = value;
            modalTxtBoxPosition.Enabled = value;
            modalChkBoxComputeTime.Enabled = value;
            modalChkBoxRequiredJobId.Enabled = value;
            modalChkBoxShowJobOverview.Enabled = value;
            modalBtnSubmit.Visible = value;
            modalBtnSubmit.Enabled = value;
            for (int i = 0; i < modalGridViewDepartment.Rows.Count; i++)
            {
                CheckBox cbSelect = (CheckBox)modalGridViewDepartment.Rows[i].FindControl("modalChkBoxSelect");
                TextBox tbPosition = (TextBox)modalGridViewDepartment.Rows[i].FindControl("modalTxtBoxDeptPosition");
                cbSelect.Enabled = value;
                tbPosition.Enabled = value;
            }
            modalChkboxAll.Enabled = value;
        }

        private void JobTypeDepartmentAction(int jobtypeid) 
        {
            JobTypeDepartment jobtypedepartment = new JobTypeDepartment();
            for (int i = 0; i < modalGridViewDepartment.Rows.Count; i++)
            {
                Label labelDeptId = (Label)modalGridViewDepartment.Rows[i].FindControl("modalLabelDepartmentId");
                CheckBox cbSelect = (CheckBox)modalGridViewDepartment.Rows[i].FindControl("modalChkBoxSelect");
                TextBox tbPosition = (TextBox)modalGridViewDepartment.Rows[i].FindControl("modalTxtBoxDeptPosition");
                
                if (cbSelect.Checked == false && labelDeptId.ToolTip != "ToolTip") //Delete
                {
                    jobtypedepartment.Delete(Convert.ToInt32(labelDeptId.ToolTip));
                }
                else if (cbSelect.Checked == true && labelDeptId.ToolTip == "ToolTip") //Add
                {
                    jobtypedepartment.DepartmentId = Convert.ToInt32(labelDeptId.Text);
                    jobtypedepartment.Position = Convert.ToInt32(tbPosition.Text.Trim() == "" ? modalTxtBoxPosition.Text : tbPosition.Text);
                    jobtypedepartment.JobTypeId = jobtypeid;
                    jobtypedepartment.Insert(jobtypedepartment);
                }
                else if (cbSelect.Checked == true && labelDeptId.ToolTip != "ToolTip") //Update
                {
                    jobtypedepartment.Id = Convert.ToInt32(labelDeptId.ToolTip);
                    jobtypedepartment.DepartmentId = Convert.ToInt32(labelDeptId.Text);
                    jobtypedepartment.Position = Convert.ToInt32(tbPosition.Text.Trim() == "" ? modalTxtBoxPosition.Text : tbPosition.Text);
                    jobtypedepartment.JobTypeId = jobtypeid;
                    jobtypedepartment.Update(jobtypedepartment);
                }
            }
        }

        #endregion

        #region OTHERS
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            foreach (GridViewRow row in gridViewJobType.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    row.Attributes["onmouseover"] = "this.style.cursor = 'pointer';this.style.backgroundColor = '#e5f2fc';";
                    row.Attributes["onmouseout"] = "this.style.backgroundColor='#ffffff';";
                    row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gridViewJobType, "Select$" + row.DataItemIndex, true);
                }
            }
            base.Render(writer);
        }

        private bool IsDeptCheckBoxEmpty()
        {
            bool isempty = true;
            for (int i = 0; i < modalGridViewDepartment.Rows.Count; i++)
            {
                CheckBox cbSelect = (CheckBox)modalGridViewDepartment.Rows[i].FindControl("modalChkBoxSelect");
                if (cbSelect.Checked == true) 
                {
                    isempty = false;
                    break;
                }
            }

            return isempty;
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
            module = module.GetModule("SetupJobType.aspx");
            myAccessRights = myAccessRights.GetRolesModuleAccess(Convert.ToInt32(user.RoleId), module.Id);
        }
        #endregion
    }
}