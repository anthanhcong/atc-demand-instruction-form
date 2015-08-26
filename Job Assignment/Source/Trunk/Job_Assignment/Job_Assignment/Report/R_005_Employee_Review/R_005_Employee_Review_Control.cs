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
        private bool R_005_Employee_Review_Get_Plan_Empl(DateTime date)
        {
            string sql_cmd;
            bool b;
            DataRow newrow;
            string mess;
            string empl_id, empl_name, shift, line, subline, wst;
            string[] working_history;
            int i = 0;
            int countT = 0;
            int total = 0;


            sql_cmd = String.Format("SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R005_Employee_Review_Report] WHERE [Date] = '{0}'", date.ToString("yyyy-MMM-dd"));
            b = R_005_Employee_Review_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);

            if (b == false)
            {
                return false;
            }

            StatusLabel1.Visible = true;
            ProgressBar1.Visible = true;
            StatusLabel1.Text = "create Employee_Review";

            // Neu da co du lieu: hoi co Xoa de taoj lai khong. neu yes thi xoa taoj lai. No thi reload len thoi
            int count = R_005_Employee_Review_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Count;
            if (count > 0)
            {
                mess = "Plan for date:" + date.ToString("dd MMM yyyy") + "was existing\n";
                mess += "Do you want to delete and create the new one?";

                if (MessageBox.Show(mess, "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }

                DeleteReport_Employee_Review(date);
                R_005_Employee_Review_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            }

            // Load All DSNV
            DataTable all_empl = Get_All_Empl();
            if (all_empl != null)
            {
                countT = all_empl.Rows.Count;
            }
            total = countT;
            foreach (DataRow row in all_empl.Rows)
            {
                empl_id = row["Empl_ID"].ToString().Trim();
                empl_name = row["Empl_Name"].ToString().Trim();
                working_history = Get_working_History(empl_id, date);
                shift = working_history[0];
                line = working_history[1];
                subline = working_history[2]; // bang thieu cot nay
                wst = working_history[3];
                newrow = R_005_Employee_Review_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.NewRow();
                newrow["Date"] = date.ToString("MM/dd/yyyy");
                newrow["Empl_ID"] = empl_id;
                newrow["Empl_Name"] = empl_name;
                newrow["ShiftName"] = shift;
                newrow["LineID"] = line;
                newrow["WST_ID"] = wst;
                R_005_Employee_Review_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Add(newrow);

                i++;
                ProgressBar1.Value = i * 100 / total;
            }

            Update_SQL_Data(R_005_Employee_Review_MasterDatabase.MasterDatabase_GridviewTBL.Data_da, R_005_Employee_Review_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb);

            StatusLabel1.Visible = false;
            ProgressBar1.Visible = false;
            return true;
        }

        private bool DeleteReport_Employee_Review(DateTime date)
        {
            bool result;
            string cmd = @"Delete FROM [R005_Employee_Review_Report]
                            WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            result = Update_Data_Info(MasterDatabase_Connection_Str, cmd);
            return result;
        }

        private DataTable Get_All_Empl()
        {
            string cmd = @"select distinct Empl_ID, Empl_Name FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_002_Empl_Skill]";
            SQL_API.SQL_ATC sqlobj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            sqlobj.GET_SQL_DATA(cmd);
            return sqlobj.DaTable;
        }

        string[] Get_working_History(string Empl_ID, DateTime date)
        {
            string[] empl_info = { "", "", "", "" };

            SQL_API.SQL_ATC spl_obj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string sqlcmd = @"SELECT distinct
                                  [Date]
                                  ,[ShiftName]
                                  ,[LineID]
                                  ,[WST_ID]
                                  ,[SubLine_ID]
                                  ,[Empl_ID]
                                  ,[Empl_Name]
                                  ,[From_Time]
                              FROM [JOB_ASSIGNMENT_DB].[dbo].[P007_P008_Tracking]";
            sqlcmd += @" WHERE Date = '" + date.ToString("dd MMM yyyy") + "' and Empl_ID = '" + Empl_ID + "'";
            sqlcmd += @" AND  (([ShiftName] = 'Shift_1' AND [From_Time] BETWEEN '06:00:00' and '14:00:00' or [To_Time] is null)";
            sqlcmd += @" OR    ([ShiftName] = 'Shift_2' AND [From_Time] BETWEEN '14:00:00' and '22:00:00' or [To_Time] is null)";
            sqlcmd += @" OR    ([ShiftName] = 'Shift_3' AND [From_Time] BETWEEN '22:00:00' and '23:59:59' or [To_Time] is null)";
            sqlcmd += @" OR    ([ShiftName] = 'Shift_3' AND [From_Time] BETWEEN '00:00:00' and '06:00:00' or [To_Time] is null))";
            sqlcmd += @" ORDER by [From_Time] DESC";

            spl_obj.GET_SQL_DATA(sqlcmd);
            // cái này chưa đúng
            if ((spl_obj.DaTable != null) && (spl_obj.DaTable.Rows.Count > 0))
            {
                empl_info[0] = spl_obj.DaTable.Rows[0]["ShiftName"].ToString().Trim();
                empl_info[1] = spl_obj.DaTable.Rows[0]["LineID"].ToString().Trim();
                empl_info[2] = spl_obj.DaTable.Rows[0]["SubLine_ID"].ToString().Trim();
                empl_info[3] = spl_obj.DaTable.Rows[0]["WST_ID"].ToString().Trim();
            }

            return empl_info;
        }
    }
}
