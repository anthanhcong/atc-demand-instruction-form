using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.IO.Ports;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using MasterDatabase;

namespace Job_Assignment
{
    public partial class Form1 : SQL_APPL
    {
        DataTable Load_PO_Plan(DateTime date, string part)
        {
            string cmd = @"SELECT * FROM [P_001_InputFromPlanner]";
            cmd += @" WHERE Date = '" + date.ToString("dd MMM yyyy") + "'";
            cmd += @" AND PartNumber = '" + part + "'";
            DataTable po_plan = GetSqlData(cmd);
            return po_plan;
        }

        private bool DeletePlanDetail(DateTime date)
        {
            bool result;
            string cmd = @"Delete FROM [P_004_KeHoachSanXuatTheoTram] 
                            WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            result = Update_Data_Info(MasterDatabase_Connection_Str, cmd);
            return result;
        }

        DataTable Load_KeHoachSX_Theo_Line(DateTime date)
        {
            string sql_cmd = String.Format("SELECT * FROM [P_003_KeHoachSanXuatTheoLine] WHERE [Date] = '{0}' order by LineId, From_Time", date.ToString("yyyy-MMM-dd"));
            DataTable line_plan = GetSqlData(sql_cmd);
            return line_plan.Copy() ;
        }

        DataTable Load_Line_List_By_Date(DateTime date)
        {
            string sql_cmd = String.Format(@"SELECT DISTINCT [Date]
                                              ,[LineID]
                                              ,[LineName]
                                              ,[SubLine_ID]
                                              ,[SubLine_Name]
                                          FROM [P_002_PlanForProductionByDate] WHERE [Date] = '{0}'", date.ToString("yyyy-MMM-dd"));
            DataTable line_plan = GetSqlData(sql_cmd);
            return line_plan.Copy();
        }

        DataTable Load_All_PartOfLine(DateTime date, string subline)
        {
            string sql_cmd = String.Format(@"SELECT * FROM [P_002_PlanForProductionByDate] 
                                             WHERE [Date] = '{0}' AND [SubLine_ID] = '{1}'", date.ToString("yyyy-MMM-dd"), subline);
            DataTable line_plan = GetSqlData(sql_cmd);
            return line_plan.Copy();
        }

        bool Create_Update_PlanDetail(DateTime date, string shift, string po, string part, string line_id, string line_name, string subline, string subline_name,
                                        string wst, string wst_name, int qty, TimeSpan from_time, TimeSpan to_time)
        {
            DateTime cur_date;
            string cur_date_str, cur_po, cur_line, cur_shift, cur_wst, cur_part, cur_subline;
            bool exist = false;

            foreach (DataRow row in ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows)
            {
                cur_date_str = row["Date"].ToString().Trim();
                cur_date = DateTime.Parse(cur_date_str);
                cur_line = row["LineID"].ToString().Trim();
                cur_shift = row["ShiftName"].ToString().Trim();
                cur_wst = row["WST_ID"].ToString().Trim();
                cur_part = row["PartNumber"].ToString().Trim();
                cur_subline = row["SubLine_ID"].ToString().Trim();
                cur_po = row["PO"].ToString().Trim();

                if ((cur_date.Date == date.Date) && (cur_line == line_id) && (cur_po == po)
                    && (cur_shift == shift) && (cur_wst == wst) && (subline == cur_subline) && (cur_part == part))
                {
                    row["LineName"] = line_name;
                    row["WST_Name"] = wst_name;
                    row["SubLine_Name"] = subline_name;
                    row["From_Time"] = from_time;
                    row["To_Time"] = to_time;
                    exist = true;
                    break;
                }
            }

            if (exist == false)
            {
                DataRow new_row = ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.NewRow();
                new_row["Date"] = date.ToString("dd MMM yyyy");
                new_row["LineID"] = line_id;
                new_row["LineName"] = line_name;
                new_row["ShiftName"] = shift;
                new_row["SubLine_ID"] = subline;
                new_row["SubLine_Name"] = subline_name;
                new_row["WST_ID"] = wst;
                new_row["WST_Name"] = wst_name;
                new_row["From_Time"] = from_time;
                new_row["To_Time"] = to_time;
                new_row["PartNumber"] = part;
                new_row["PO"] = po;
                ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Add(new_row);
            }
            // P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Save_Data();

            Update_SQL_Data(ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.Data_da, ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb);
            return true;
        }

        string[] Get_Empl_Line_Plan(DataTable table , string shift, string line, string subline, string wst)
        {
            string cur_line, cur_subline, cur_wst;
            string cur_shift;
            string[] empl_info = { "", "" };
#if false 
            string sql_cmd = String.Format("SELECT * FROM [P_003_KeHoachSanXuatTheoLine] WHERE [Date] = '{0}' AND LineID = '{1}' AND SubLine_ID = '{2}' AND WST_ID = '{3}'", date.ToString("yyyy-MMM-dd"), line, subline, wst);
            DataTable line_plan = GetSqlData(sql_cmd);

            if ((line_plan != null) && (line_plan.Rows.Count > 0))
            {
                empl_info[0] = line_plan.Rows[0]["Empl_ID"].ToString().Trim();
                empl_info[1] = line_plan.Rows[0]["Empl_Name"].ToString().Trim();
            }
#else
            foreach (DataRow row in table.Rows)
            {
                cur_line = row["LineID"].ToString().Trim();
                cur_subline = row["SubLine_ID"].ToString().Trim();
                cur_wst = row["WST_ID"].ToString().Trim();
                cur_shift = row["ShiftName"].ToString().Trim();
                if ((cur_wst == wst) && (cur_shift == shift))
                {
                    empl_info[0] = row["Empl_ID"].ToString().Trim();
                    empl_info[1] = row["Empl_Name"].ToString().Trim();
                    break;
                }
            }
#endif
            return empl_info;
        }
    }
}