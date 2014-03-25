using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

namespace TimeTracker.Model
{
    public class MaterialCost
    {
        public string BookedDate { get; set; }
        public string ActualDate { get; set; }
        public string PartNo { get; set; }
        public string PartDescription { get; set; }
        public int BookedQuantity { get; set; }
        public double BookedUnitCost { get; set; }
        public int ActualQuantity { get; set; }
        public double ActualUnitCost { get; set; }
        public string BookedQuoteVersion { get; set; }
        public string ActualQuoteVersion { get; set; }

        public List<MaterialCost> GetMaterialCost(string hwso, string swso, string evalno) 
        {
            List<MaterialCost> data = new List<MaterialCost>();
            if (hwso.Trim() != "")
            {
                List<MaterialCost> hwMCost = ComputeMaterialCost("CAPHWConnection",hwso.Trim());
                data.AddRange(hwMCost);
            }
            if (swso.Trim() != "") 
            {
                List<MaterialCost> swMCost = ComputeMaterialCost("CAPSWConnection", swso.Trim());
                data.AddRange(swMCost);
            }
            
            if (evalno.Trim() != "") 
            {
                string[] evalnos = evalno.Split(',');
                foreach (string e in evalnos) 
                {
                    string nSoNum = Regex.Replace(e, "[^0-9]", "");
                    if (nSoNum.Trim() != "")
                    {
                        string connection = GetConnectionString(nSoNum.Trim());
                        if (connection.Trim() != "")
                        {
                            List<MaterialCost> eMCost = ComputeMaterialCost("CAPSWConnection", nSoNum.Trim());
                            data.AddRange(eMCost);
                        }
                    }
                }
            }
            return data;
        }

        private string GenerateMaterialCostQueryScript(string sonum,string bookedQuoteVersion,string actualQuoteVersion)
        {
            return "select Ver1=QD1.QV_Version, QD1.QD_Part_num, QD1.QD_Descript, Qty1=QD1.QD_QTY, Ucost1=QD1.QD_Ucost," +
                        " Uprice1=QD1.QD_Uprice, Setup1=QD1.QD_Setup,Ver2=QD2.QV_Version, Qty2=QD2.QD_QTY, Ucost2=QD2.QD_Ucost," +
                        " Uprice2=QD2.QD_Uprice, Setup2=QD2.QD_Setup" +
                        " from Quote_Details QD1, Quote_Details QD2" +
                        " where QD1.SO_Num = '" + sonum.Trim() + "'" +
                        " and QD1.SO_num = QD2.SO_num and (QD1.QD_Ucost + QD2.QD_Ucost) <> 0" +
                        " and QD1.QV_Version = '" + bookedQuoteVersion.Trim() + "'" +
                        " and QD2.QV_Version = '" + actualQuoteVersion + "'" +
                        " and QD1.QD_Part_Num = QD2.QD_Part_Num and QD1.QD_Part_Num not Like '-%'" +
                        " Union" +
                        " select Ver1=QV_Version, QD_Part_num, QD_Descript, Qty1=QD_QTY, UCost1=QD_Ucost, Uprice1=QD_Uprice," +
                        " Setup1=QD_Setup, Ver2='" + actualQuoteVersion + "', Qty2=0, Ucost2=0, Uprice2=0, Setup2=0" +
                        " from Quote_Details" +
                        " where SO_Num = '" + sonum.Trim() + "' and QD_Ucost <> 0" +
                        " and QV_Version = '" + bookedQuoteVersion.Trim() + "' and QD_Part_Num not Like '-%'" +
                        " and QD_Part_num not in (select QD_Part_num from Quote_details" +
                                                    " where SO_Num = '" + sonum.Trim() + "'" +
                                                    " and QV_Version = '" + actualQuoteVersion + "')" +
                        " Union" +
                        " select Ver1='" + bookedQuoteVersion.Trim() + "', QD_Part_num, QD_Descript, Qty1=0, UCost1=0, Uprice1=0, Setup1=0," +
                        " Ver2=QV_Version, Qty2=QD_Qty, Ucost2=QD_Ucost, Uprice2=QD_Uprice, Setup2=QD_Setup" +
                        " from Quote_Details" +
                        " where SO_Num = '" + sonum.Trim() + "' and QD_Ucost <> 0 and QD_Part_Num not Like '-%'" +
                        " and QV_Version = '" + actualQuoteVersion + "' and QD_Part_num not in" +
                            " (select QD_Part_num from Quote_details where SO_Num = '" + sonum.Trim() + "' and QV_Version = '" + bookedQuoteVersion.Trim() + "')";
        }

        private void GetBookedQuoteVersionAndDate(string connection, string sonum, ref string bookedQuoteVersion, ref string bookeddate) 
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[connection].ToString()))
            {
                SqlCommand cmd = new SqlCommand("select QV_Date=Convert(char(11), QV_Date, 106), QV_Version from quote_version QV where QV_Type = 'B' and SO_Num = '" + sonum.Trim() + "' " +
                    "and QV_Version in (Select QV_Version from Quote_details where SO_Num = '" + sonum.Trim() + "' and QV_Version = QV.QV_Version)", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    bookedQuoteVersion = reader["QV_Version"].ToString();
                    bookeddate = reader["QV_Date"].ToString();
                }
            }
        }

        private void GetActualQuoteVersionAndDate(string connection, string sonum, ref string actualQuoteVersion, ref string actualdate) 
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[connection].ToString()))
            {
                SqlCommand cmd = new SqlCommand("select QV_Date=Convert(char(11), QV_Date, 106), QV_Version from quote_version QV where QV_Type = 'F' and so_num = '" + sonum.Trim() + "' " +
                    "and QV_Version in (Select Max(QV_Version) from Quote_details where SO_Num = '" + sonum.Trim() + "')", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    actualQuoteVersion = reader["QV_Version"].ToString();
                    actualdate = reader["QV_Date"].ToString();
                }
            }
        }

        private string GetConnectionString(string sonum) 
        {
            string connection = "";
            using (SqlConnection hwcon = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPHWConnection"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("select SO_Num from Sales_Order where SO_Num = '" + sonum + "'", hwcon);
                hwcon.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    connection = "CAPHWConnection";
                }
            }
            if (connection == "")
            {
                using (SqlConnection swcon = new SqlConnection(ConfigurationManager.ConnectionStrings["CAPSWConnection"].ToString()))
                {
                    SqlCommand cmd = new SqlCommand("select SO_Num from Sales_Order where SO_Num = '" + sonum + "'", swcon);
                    swcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        connection = "CAPSWConnection";
                    }
                }
            }
            return connection;
        }

        private List<MaterialCost> ComputeMaterialCost(string connection,string sonum) 
        {
            List<MaterialCost> data = new List<MaterialCost>();
            string bookedQuoteVersion = "0";
            string actualQuoteVersion = "0";
            string bookeddate = "NA";
            string actualdate = "NA";
            GetBookedQuoteVersionAndDate(connection, sonum, ref bookedQuoteVersion, ref bookeddate);
            if (bookedQuoteVersion == "0")
                bookeddate = "NA";
            GetActualQuoteVersionAndDate(connection, sonum, ref actualQuoteVersion, ref actualdate);
            if (actualQuoteVersion != "0")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[connection].ToString()))
                {
                    string sqlScript = GenerateMaterialCostQueryScript(sonum, bookedQuoteVersion, actualQuoteVersion);
                    SqlCommand cmd = new SqlCommand(sqlScript, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        MaterialCost mcost = new MaterialCost();
                        mcost.BookedDate = bookeddate;
                        mcost.ActualDate = actualdate;
                        mcost.PartNo = reader["QD_Part_num"].ToString();
                        mcost.PartDescription = reader["QD_Descript"].ToString();
                        mcost.BookedQuantity = Convert.ToInt32(reader["Qty1"].ToString());
                        mcost.BookedUnitCost = Convert.ToDouble(reader["Ucost1"].ToString());
                        mcost.ActualQuantity = Convert.ToInt32(reader["Qty2"].ToString());
                        mcost.ActualUnitCost = Convert.ToDouble(reader["Ucost2"].ToString());
                        mcost.BookedQuoteVersion = bookedQuoteVersion;
                        mcost.ActualQuoteVersion = actualQuoteVersion;
                        data.Add(mcost);
                    }
                }
            }

            return data;
        }
    }
}