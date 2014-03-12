using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeTracker.Model;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace TimeTracker
{
    public partial class ReportCostBurden : System.Web.UI.Page
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
            }
        }

        protected void btn_click(object sender, EventArgs e) 
        {
            //FileStream fs = new FileStream(Server.MapPath(@"\Content\Sample.xlsx"), FileMode.Open, FileAccess.Read);
            //XSSFWorkbook temWorkBook = new XSSFWorkbook(fs);
            //ISheet nsheet = temWorkBook.GetSheet("Sheet1");
            //IRow datarow = nsheet.GetRow(4);

            //datarow.GetCell(0).SetCellValue(77);
            //nsheet.ForceFormulaRecalculation = true;

            //using (var ms = new MemoryStream())
            //{
            //    temWorkBook.Write(ms);

            //    Response.Clear();
            //    Response.ContentType = "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    Response.AppendHeader("Content-Disposition","inline;filename=Sample"+DateTime.Now.ToString("yyyyMMMdd")+".xlsx");
            //    Response.BinaryWrite(ms.ToArray());
            //    Response.End();
            //}

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet1 = workbook.CreateSheet("New Sheet");
            ISheet sheet2 = workbook.CreateSheet("Second Sheet");
            ICell cell1 = sheet1.CreateRow(0).CreateCell(0);
            IFont fontBold = workbook.CreateFont();
            fontBold.Boldweight = (short)FontBoldWeight.Bold;
            ICellStyle style1 = workbook.CreateCellStyle();
            style1.SetFont(fontBold);
            cell1.CellStyle = style1;
            cell1.SetCellValue("sample value");
            int x = 1;
            for (int i = 1; i <= 15; i++) 
            {
                IRow row = sheet1.CreateRow(i);
                for(int j = 0;j < 15;j++)
                {
                    row.CreateCell(j).SetCellValue(x++);
                }
            }
            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);

                Response.Clear();
                Response.ContentType = "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AppendHeader("Content-Disposition", "inline;filename=Sample" + DateTime.Now.ToString("yyyyMMMdd") + ".xlsx");
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
            
        }

        #region OTHERS
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
            module = module.GetModule("ReportCostBurden.aspx");
            myAccessRights = myAccessRights.GetRolesModuleAccess(Convert.ToInt32(user.RoleId), module.Id);
        }
        #endregion
    }
}