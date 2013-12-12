using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TimeTracker.Model;

namespace TimeTracker
{
    public partial class SetupJobFlow : System.Web.UI.Page
    {
        RolesModuleAccess myAccessRights = new RolesModuleAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsValidUser())
                Response.Redirect("Login.aspx");
            GetMyAccessRights();
            if (myAccessRights == null)
                Response.Redirect("Dashboard.aspx");
            HttpContext.Current.Session["siteSubHeader"] = "Setup";
            HttpContext.Current.Session["selectedTab"] = "Setup";

            if (!IsPostBack)
            {
                //InitializeDropDownDepartment();
                InitializeMainGrid();
            }
        }

        private void InitializeMainGrid()
        {
            GetMyAccessRights();

            JobFlow jobflow = new JobFlow();
            if (myAccessRights.CanAdd == true)
            {
                linkBtnAdd.Visible = true;
            }
            else
                linkBtnAdd.Visible = false;

            List<JobFlow> jobflowList = new List<JobFlow>();
            jobflowList = jobflow.GetJobFlowList();
            gridViewJobFlow.DataSource = jobflowList;
            gridViewJobFlow.DataBind();
        }

        protected void linkBtnAdd_Click(object sender, EventArgs e)
        {
            modalLabelJobFlowId.Text = "";
            modalLabelError.Text = "";
            modalLabelError.Visible = false;
            modalTxtBoxDescription.Text = "";
            modalTxtBoxAcronym.Text = "";
            modalTxtBoxPosition.Text = "";
            InitializeModalJobType();
            InitializeModalGridView();
            InitializeModalDepartment();
            modalBtnSubmit.Visible = true;
            programmaticModalPopup.Show();
        }

        protected void gridViewJobFlow_Command(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GetMyAccessRights();
                JobFlow jobflow = new JobFlow();
                modalLabelError.Text = "";
                modalLabelError.Visible = false;
                int index = Convert.ToInt32(e.CommandArgument);
                int jobflowId = Convert.ToInt32(((Label)gridViewJobFlow.Rows[index].FindControl("labelJobFlowId")).Text);
                modalLabelJobFlowId.Text = jobflowId.ToString();
                jobflow = jobflow.GetJobFlow(jobflowId);
                modalTxtBoxDescription.Text = jobflow.Description;
                modalTxtBoxAcronym.Text = jobflow.Acronym;
                modalTxtBoxPosition.Text = jobflow.Position.ToString();
                InitializeModalJobType();
                InitializeModalGridView();
                InitializeModalDepartment();
                modalBtnSubmit.Visible = Convert.ToBoolean(myAccessRights.CanUpdate);
                programmaticModalPopup.Show();
            }
        }

        #region MODAL

        #region INITIALIZE
        private void InitializeModalJobType() 
        {
            JobType jobType = new JobType();
            List<JobType> jobtypeList = new List<JobType>();
            jobtypeList = jobType.GetJobTypeList();
           
            modalDropDownJobType.DataSource = jobtypeList;
            modalDropDownJobType.DataValueField = "Id";
            modalDropDownJobType.DataTextField = "Description";
            modalDropDownJobType.DataBind();
        }

        private void InitializeModalGridView() 
        {
            JobTypeFlow jobtypeflow = new JobTypeFlow();
            int jobflowid = 0;
            if (modalLabelJobFlowId.Text.Trim() != "") 
            {
                jobflowid = Convert.ToInt32(modalLabelJobFlowId.Text);
            }
            var data = jobtypeflow.GetJobTypeFlowListByJobFlow(jobflowid);
            gridViewModal.DataSource = data;
            gridViewModal.DataBind();

            for (int i = 0; i < gridViewModal.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)gridViewModal.Rows[i].FindControl("modalChkJobType");
                Label labelJobTypeId = (Label)gridViewModal.Rows[i].FindControl("modalLabelJobTypeId");
                Label labelDeptId = (Label)gridViewModal.Rows[i].FindControl("modalLabelDepartmentId");
                if (labelDeptId.Text.Trim() == "") 
                {
                    Label labelDept = (Label)gridViewModal.Rows[i].FindControl("modalLabelDepartment");
                    labelDept.Text = "All";
                    labelDeptId.Text = "0";
                }
                labelJobTypeId.ToolTip = data[i].Id.ToString();
                labelDeptId.ToolTip = data[i].Id.ToString();
            }
            Session["JobFlows"] = data;
        }

        private void InitializeModalDepartment()
        {
            modalBtnAdd.Visible = true;
            Department department = new Department();
            List<Department> deptlist = new List<Department>();

            int selectedjobTypeId = Convert.ToInt32(modalDropDownJobType.SelectedValue);
            deptlist = department.GetDepartmentList();

            if (deptlist.Count > 0)
            {
                department.Id = 0;
                department.Description = "All";
                department.Position = 0;
                department.Acronym = "All";
                deptlist.Insert(0, department);
            }

            for (int i = 0; i < gridViewModal.Rows.Count; i++)
            {
                Label labelJobTypeId = (Label)gridViewModal.Rows[i].FindControl("modalLabelJobTypeId");
                Label labelDeptId = (Label)gridViewModal.Rows[i].FindControl("modalLabelDepartmentId");
                int deptId = labelDeptId.Text.Trim() == "" ? 0 : Convert.ToInt32(labelDeptId.Text);
                int jobtypeId = labelJobTypeId.Text.Trim() == "" ? 0 : Convert.ToInt32(labelJobTypeId.Text);
                for (int j = 0; j < deptlist.Count; j++)
                {
                    if (deptlist[j].Id == deptId && selectedjobTypeId == jobtypeId)
                    {
                        deptlist.RemoveAt(j);
                        break;
                    }
                }
            }

            if (deptlist.Count == 0)
            {
                modalBtnAdd.Visible = false;
            }

            modalDropDownDepartment.DataSource = deptlist;
            modalDropDownDepartment.DataTextField = "Description";
            modalDropDownDepartment.DataValueField = "Id";
            modalDropDownDepartment.DataBind();
        }
        #endregion

        #region COMMAND
        #endregion
        protected void modalDropDownJobType_Changed(object sender, EventArgs e) 
        {
            InitializeModalDepartment();
            this.programmaticModalPopup.Show();
        }

        protected void modalDescription_Changed(object sender, EventArgs e)
        {
            string errorMsg = "Description already used. ";
            if (modalTxtBoxDescription.Text.Trim() != "")
            {
                JobFlow jobflow = new JobFlow();
                jobflow = jobflow.GetJobFlowByDescription(modalTxtBoxDescription.Text.Trim());

                if (jobflow != null)
                {
                    if (modalLabelJobFlowId.Text.Trim() == "")
                    {
                        modalLabelError.Text += errorMsg;
                        modalLabelError.Visible = true;
                    }
                    else
                    {
                        JobFlow j = jobflow.GetJobFlow(Convert.ToInt32(modalLabelJobFlowId.Text));
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
                JobFlow jobflow = new JobFlow();
                jobflow = jobflow.GetJobFlowByAcronym(modalTxtBoxAcronym.Text.Trim());

                if (jobflow != null)
                {
                    if (modalLabelJobFlowId.Text.Trim() == "")
                    {
                        modalLabelError.Text += errorMsg;
                        modalLabelError.Visible = true;
                    }
                    else
                    {
                        JobFlow j = jobflow.GetJobFlow(Convert.ToInt32(modalLabelJobFlowId.Text));
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

        protected void modalBtnAdd_Click(object sender, EventArgs e)
        {
            List<JobTypeFlow> jobflows = new List<JobTypeFlow>();
            if (Session["JobFlows"] != null)
            {
                jobflows = (List<JobTypeFlow>)Session["JobFlows"];
            }
            JobTypeFlow jobflow = new JobTypeFlow();
            jobflow.JobTypeId = Convert.ToInt32(modalDropDownJobType.SelectedItem.Value);
            jobflow.jobtype = modalDropDownJobType.SelectedItem.Text.Trim();
            jobflow.DepartmentId = Convert.ToInt32(modalDropDownDepartment.SelectedItem.Value);
            jobflow.department = modalDropDownDepartment.SelectedItem.Text.Trim();
            jobflows.Add(jobflow);
            gridViewModal.DataSource = jobflows;
            gridViewModal.DataBind();
            InitializeModalDepartment();
            Session["JobFlows"] = jobflows;
            modalLabelError.Text = modalLabelError.Text.Replace("Please add atleast one department.", "").Trim();
            if (modalLabelError.Text.Trim().Length == 0)
            {
                modalLabelError.Visible = false;
            }

            programmaticModalPopup.Show();
        }

        protected void modalBtnSubmit_Command(object sender, CommandEventArgs e)
        {
            if (gridViewModal.Rows.Count == 0) 
            {
                modalLabelError.Visible = true;
                modalLabelError.Text += " Please add atleast one department.";
                this.programmaticModalPopup.Show();
            }
            else if (modalLabelError.Visible == true)
                this.programmaticModalPopup.Show();
            else
            {
                JobFlow jobflow = new JobFlow();

                if (modalLabelJobFlowId.Text.Trim() != "")
                {
                    jobflow = jobflow.GetJobFlow(Convert.ToInt32(modalLabelJobFlowId.Text));
                }
                jobflow.Description = modalTxtBoxDescription.Text.Trim();
                jobflow.Acronym = modalTxtBoxAcronym.Text.Trim();
                jobflow.Position = modalTxtBoxPosition.Text.Trim() == "" ? 0 : Convert.ToInt32(modalTxtBoxPosition.Text);

                if (modalLabelJobFlowId.Text.Trim() == "")
                {
                    jobflow.Insert(jobflow);
                    jobflow = jobflow.GetJobFlowByDescription(jobflow.Description);
                }
                else 
                {
                    jobflow.Update(jobflow);
                }

                for (int i = 0; i < gridViewModal.Rows.Count; i++)
                {
                    JobTypeFlow jobTypeFlow = new JobTypeFlow();
                    CheckBox cb = (CheckBox)gridViewModal.Rows[i].FindControl("modalChkJobType");
                    Label labelJobTypeId = (Label)gridViewModal.Rows[i].FindControl("modalLabelJobTypeId");
                    Label labelDeptId = (Label)gridViewModal.Rows[i].FindControl("modalLabelDepartmentId");
                    TextBox txtBoxPosition = (TextBox)gridViewModal.Rows[i].FindControl("modalInnerTxtBoxPostion");
                    
                    int jobtypeid = Convert.ToInt32(labelJobTypeId.Text);
                    int? deptid = Convert.ToInt32(labelDeptId.Text.Trim() == "" ? "0" : labelDeptId.Text);
                    if(deptid == 0)
                        deptid = null;
                    jobTypeFlow = jobTypeFlow.GetJobTypeFlow(jobflow.Id,jobtypeid,deptid);
                    if (cb.Checked)
                    {
                        if (jobTypeFlow == null)
                        {
                            jobTypeFlow = new JobTypeFlow();
                            jobTypeFlow.JobFlowId = jobflow.Id;
                            jobTypeFlow.JobTypeId = jobtypeid;
                            jobTypeFlow.DepartmentId = deptid;
                            jobTypeFlow.Position = txtBoxPosition.Text.Trim() == "" ? 0 : Convert.ToInt32(txtBoxPosition.Text.Trim());
                            jobTypeFlow.Insert(jobTypeFlow);
                        }
                        else 
                        {
                            jobTypeFlow.Position = txtBoxPosition.Text.Trim() == "" ? 0 : Convert.ToInt32(txtBoxPosition.Text.Trim());
                            jobTypeFlow.Update(jobTypeFlow);
                        }
                    }
                    else
                    {
                        if (jobTypeFlow != null)
                        {
                            jobTypeFlow.Delete(jobTypeFlow.Id);
                        }
                    }
                }
                if(IsCheckBoxEmpty())
                {
                    jobflow.Delete(jobflow.Id);
                }
                Session["JobFlows"] = null;
                InitializeMainGrid();
            }
        }

        protected void modalBtnCancel_Click(object sender, EventArgs e)
        {
            Session["JobFlows"] = null;
        }
        #endregion

        #region OTHERS

        private bool IsCheckBoxEmpty()
        {
            bool isempty = true;
            for (int i = 0; i < gridViewModal.Rows.Count; i++)
            {
                CheckBox cbSelect = (CheckBox)gridViewModal.Rows[i].FindControl("modalChkJobType");
                if (cbSelect.Checked == true)
                {
                    isempty = false;
                    break;
                }
            }

            return isempty;
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            foreach (GridViewRow row in gridViewJobFlow.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    row.Attributes["onmouseover"] = "this.style.cursor = 'pointer';this.style.backgroundColor = '#e5f2fc';";
                    row.Attributes["onmouseout"] = "this.style.backgroundColor='#ffffff';";
                    row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gridViewJobFlow, "Select$" + row.DataItemIndex, true);
                }
            }
            base.Render(writer);
        }

        protected bool IsValidUser()
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
            module = module.GetModule("SetupJobFlow.aspx");
            myAccessRights = myAccessRights.GetRolesModuleAccess(Convert.ToInt32(user.RoleId), module.Id);
        }
        #endregion
    }
}