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
            if (!IsValidUser())
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

            JobType jobtype = new JobType();
            List<JobType> jobtypeList = new List<JobType>();

            if (dropDownListDepartment.SelectedItem.Text == "All")
                jobtypeList = jobtype.GetJobTypeList();
            else
                jobtypeList = jobtype.GetJobTypeList(Convert.ToInt32(dropDownListDepartment.SelectedItem.Value));

            gridViewJobType.DataSource = jobtypeList;
            gridViewJobType.DataBind();
        }

        protected void InitializeDropDownDepartment()
        {
            Department department = new Department();
            var departmentList = department.GetDepartmentList();
            //if (departmentList.Count > 0)
            //{
            //    if (departmentList[0].Description == "All")
            //        departmentList.RemoveAt(0);
            //}
            //department.Id = 0;
            //department.Description = "All";
            //departmentList.Insert(0, department);
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
            InitializeModalDropDownDepartment(dropDownListDepartment.SelectedItem.Value);
            modalLabelJobTypeId.Text = "";
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
                InitializeModalDropDownDepartment(jobtype.DepartmentId.ToString());
                this.programmaticModalPopup.Show();
            }
        }
        #endregion

        #region MODAL
        private void InitializeModalDropDownDepartment(string value = "") 
        {
            Department department = new Department();
            List<Department> departmentList = new List<Department>();
            departmentList = department.GetDepartmentList();
            modalDropDownDepartment.DataSource = departmentList;
            modalDropDownDepartment.DataTextField = "Description";
            modalDropDownDepartment.DataValueField = "Id";
            modalDropDownDepartment.DataBind();

            if (value.Trim() != "") 
            {
                foreach (ListItem i in modalDropDownDepartment.Items) 
                {
                    if (value.Trim() == i.Value.Trim()) 
                    {
                        i.Selected = true;
                        break;
                    }
                }
            }
        }

        protected void modalBtnSubmit_Command(object sender, CommandEventArgs e)
        {
            if (modalLabelError.Visible == true)
                this.programmaticModalPopup.Show();
            else
            {
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
                jobtype.DepartmentId = Convert.ToInt32(modalDropDownDepartment.SelectedItem.Value);
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
                if (e.CommandArgument.ToString() == "Add")
                {
                    jobtype.CreateDate = DateTime.Now;
                    jobtype.CreatedBy = userid;
                    jobtype.Insert(jobtype);
                }
                else if (e.CommandArgument.ToString() == "Update")
                {
                    jobtype.Update(jobtype);
                }
                InitializeGridViewJobType();
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
            module = module.GetModule("SetupJobType.aspx");
            myAccessRights = myAccessRights.GetRolesModuleAccess(Convert.ToInt32(user.RoleId), module.Id);
        }
        #endregion
    }
}