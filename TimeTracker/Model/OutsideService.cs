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
    
    public class OutsideService
    {
        public string PartType { get; set; }
        public string PartDescription { get; set; }
        public string Vendor { get; set; }
        public int BookedQuantity { get; set; }
        public double BookedUnitCost { get; set; }
        public int ActualQuantity { get; set; }
        public double ActualUnitCost { get; set; }
        public string PartNo { get; set; }
        public double BookedSetupCost { get; set; }
        public double ActualSetupCost { get; set; }

        public List<OutsideService> GetOutsideServiceCost(string hwso, string swso, string evalno)
        {
            List<OutsideService> data = new List<OutsideService>();
            if (hwso.Trim() != "")
            {
                List<OutsideService> hwMCost = ComputeOutsideServiceCost("CAPHWConnection", hwso.Trim());
                data.AddRange(hwMCost);
            }
            if (swso.Trim() != "")
            {
                List<OutsideService> swMCost = ComputeOutsideServiceCost("CAPSWConnection", swso.Trim());
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
                            List<OutsideService> eMCost = ComputeOutsideServiceCost("CAPSWConnection", nSoNum.Trim());
                            data.AddRange(eMCost);
                        }
                    }
                }
            }
            return data;
        }

        private string GenerateOutsideServiceQueryString(string sonum, string bookedQuoteVersion, string actualQuoteVersion)
        {
            return "select Ver1=QD1.QV_Version, QD1.QD_Part_num, QD1.QD_Descript, Qty1=QD1.QD_QTY, Ucost1=QD1.QD_Ucost," +
                 " Uprice1=QD1.QD_Uprice, Setup1=QD1.QD_Setup,Ver2=QD2.QV_Version, Qty2=QD2.QD_QTY, Ucost2=QD2.QD_Ucost," +
                 " Uprice2=QD2.QD_Uprice, Setup2=QD2.QD_Setup, QD1.SP_PartType, QD1.VE_Name" +
                 " from Quote_Details QD1, Quote_Details QD2" +
                 " where QD1.SO_Num = '" + sonum + "'" +
                 " and QD1.SO_num = QD2.SO_num and (QD1.QD_Ucost + QD2.QD_Ucost) <> 0" +
                 " and QD1.QV_Version = '" + bookedQuoteVersion + "'" +
                 " and QD2.QV_Version = '" + actualQuoteVersion + "'" +
                 " and QD1.QD_Part_Num = QD2.QD_Part_Num and QD1.QD_Part_Num Like '-%'" +
                 " Union" +
                 " select Ver1=QV_Version, QD_Part_num, QD_Descript, Qty1=QD_QTY, UCost1=QD_Ucost, Uprice1=QD_Uprice," +
                 " Setup1=QD_Setup, Ver2='" + actualQuoteVersion + "', Qty2=0, Ucost2=0, Uprice2=0, Setup2=0, SP_partType, VE_Name" +
                 " from Quote_Details" +
                 " where SO_Num = '" + sonum + "' and QD_Ucost <> 0" +
                 " and QV_Version = '" + bookedQuoteVersion + "' and QD_Part_Num Like '-%'" +
                 " and QD_Part_num not in (select QD_Part_num from Quote_details" +
                                             " where SO_Num = '" + sonum + "'" +
                                             " and QV_Version = '" + actualQuoteVersion + "')" +
                 " Union" +
                 " select Ver1='" + bookedQuoteVersion + "', QD_Part_num, QD_Descript, Qty1=0, UCost1=0, Uprice1=0, Setup1=0," +
                 " Ver2=QV_Version, Qty2=QD_Qty, Ucost2=QD_Ucost, Uprice2=QD_Uprice, Setup2=QD_Setup, SP_PartType, VE_Name" +
                 " from Quote_Details" +
                 " where SO_Num = '" + sonum + "' and QD_Ucost <> 0 and QD_Part_Num Like '-%'" +
                 " and QV_Version = '" + actualQuoteVersion + "' and QD_Part_num not in" +
                             " (select QD_Part_num from Quote_details where SO_Num = '" + sonum + "' and QV_Version = '" + bookedQuoteVersion + "')";
        }

        private void GetBookedQuoteVersion(string connection, string sonum, ref string bookedQuoteVersion)
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
                }
            }
        }

        private void GetActualQuoteVersion(string connection, string sonum, ref string actualQuoteVersion)
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

        private List<OutsideService> ComputeOutsideServiceCost(string connection, string sonum)
        {
            List<OutsideService> data = new List<OutsideService>();
            string bookedQuoteVersion = "0";
            string actualQuoteVersion = "0";

            GetBookedQuoteVersion(connection, sonum, ref bookedQuoteVersion);
            GetActualQuoteVersion(connection, sonum, ref actualQuoteVersion);

            if (actualQuoteVersion != "0")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[connection].ToString()))
                {
                    string sqlScript = GenerateOutsideServiceQueryString(sonum, bookedQuoteVersion, actualQuoteVersion);
                    SqlCommand cmd = new SqlCommand(sqlScript, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        OutsideService oscost = new OutsideService();
                        oscost.PartNo = reader["QD_Part_num"].ToString();
                        oscost.PartDescription = reader["QD_Descript"].ToString();
                        oscost.BookedQuantity = Convert.ToInt32(reader["Qty1"].ToString());
                        oscost.BookedUnitCost = Convert.ToDouble(reader["Ucost1"].ToString());
                        oscost.ActualQuantity = Convert.ToInt32(reader["Qty2"].ToString());
                        oscost.ActualUnitCost = Convert.ToDouble(reader["Ucost2"].ToString());
                        oscost.Vendor = reader["VE_Name"].ToString();
                        oscost.BookedSetupCost = Convert.ToDouble(reader["Setup1"].ToString());
                        oscost.ActualSetupCost = Convert.ToDouble(reader["Setup2"].ToString());
                        int partype = Convert.ToInt32(reader["SP_PartType"].ToString());

                        switch (partype) 
                        {
                            case 0:
                                oscost.PartType = "Customer Purchased Part";
                                break;
                            case 1:
                                oscost.PartType = "Programming";
                                break;
                            case 2:
                                oscost.PartType = "Standard Purchase Part";
                                break;
                            case 3:
                                oscost.PartType = "Customer Price Adjustment";
                                break;
                            case 4:
                                oscost.PartType = "Labor Offload";
                                break;
                            case 5:
                                oscost.PartType = "Internal Cost Adjustment";
                                break;
                            case 6:
                                oscost.PartType = "Finished Fixture";
                                break;
                            default:
                                oscost.PartType = "Other";
                                break;
                        }

                        data.Add(oscost);
                    }
                }
            }

            return data;
        }
    }
}