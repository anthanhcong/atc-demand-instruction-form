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
using DataGridViewAutoFilter;

namespace Job_Assignment
{
    public partial class Form1 : SQL_APPL
    {
        private bool DeleteEmpl_plan(DateTime date)
        {
            bool result;
            SQL_API.SQL_ATC sql_api = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string cmd = @"Delete FROM [P_005_EmplWorkingPlan] 
                            WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            result = sql_api.Execute_SQL_CMD(cmd);
            return result;
        }

        private bool GetEmplPlan(ref DataRow row)
        {
            string empl_id, cmd;
            DateTime date;
            string shiftName;
            string empl_id_leave;
            string cmd_sql;

            SQL_API.SQL_ATC line_plan = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            try
            {
                empl_id = row["Empl_ID"].ToString().Trim();
                date = (DateTime)row["Date"];

                // Load Line Plan 
                cmd = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[P_003_KeHoachSanXuatTheoLine]";
                cmd += @" WHERE Date = '" + date.ToString("dd MMM yyyy") + "'";
                cmd += @" AND Empl_ID = '" + empl_id + "'";
                line_plan.GET_SQL_DATA(cmd);
                DataTable leave_register = Load_Leave_Register(date);

                if ((line_plan.DaTable != null) && (line_plan.DaTable.Rows.Count > 0))
                {
                    shiftName = line_plan.DaTable.Rows[0]["ShiftName"].ToString().Trim();
                    row["ShiftName"] = line_plan.DaTable.Rows[0]["ShiftName"];
                    row["LineID"] = line_plan.DaTable.Rows[0]["LineID"];
                    row["LineName"] = line_plan.DaTable.Rows[0]["LineName"];
                    row["SubLine_ID"] = line_plan.DaTable.Rows[0]["SubLine_ID"];
                    row["SubLine_Name"] = line_plan.DaTable.Rows[0]["SubLine_Name"];
                    row["WST_ID"] = line_plan.DaTable.Rows[0]["WST_ID"];
                    row["WST_Name"] = line_plan.DaTable.Rows[0]["WST_Name"];

                    cmd_sql = @"SELECT ShiftName, LineID,  WST_ID FROM [P_003_KeHoachSanXuatTheoLine] ";
                    // kiem tra dieu kien neu thu 2 tru 2 ngay, cacs ngay khac tru` 1
                    if (date.DayOfWeek == DayOfWeek.Monday)
                    {
                        cmd_sql += @"WHERE [date] = '" + date.AddDays(-2).ToString("dd MMM yyyy") + "' and Empl_ID ='" + empl_id + "' ";
                    }
                    else
                    {
                        cmd_sql += @"WHERE [date] = '" + date.AddDays(-1).ToString("dd MMM yyyy") + "' and Empl_ID ='" + empl_id + "' ";
                    }

                    line_plan.GET_SQL_DATA(cmd_sql);
                    foreach (DataRow his in line_plan.DaTable.Rows)
                    {
                        row["Shift_His"] = his["ShiftName"].ToString().Trim();
                        row["LineID_His"] = his["LineID"].ToString().Trim();
                        row["WST_ID_His"] = his["WST_ID"].ToString().Trim();
                    }
                }
                else
                {
                    foreach (DataRow leave in leave_register.Rows)
                    {
                        empl_id_leave = leave["Empl_ID"].ToString().Trim();
                        if (empl_id == empl_id_leave)
                        {
                            row["LeaveCode"] = leave["LeaveCode"].ToString().Trim();
                        }
                    }

                    cmd_sql = @"SELECT ShiftName, LineID,  WST_ID FROM [P_003_KeHoachSanXuatTheoLine] ";
                    // kiem tra dieu kien neu thu 2 tru 2 ngay, cacs ngay khac tru` 1
                    if (date.DayOfWeek == DayOfWeek.Monday)
                    {
                        cmd_sql += @"WHERE [date] = '" + date.AddDays(-2).ToString("dd MMM yyyy") + "' and Empl_ID ='" + empl_id + "' ";
                    }
                    else
                    {
                        cmd_sql += @"WHERE [date] = '" + date.AddDays(-1).ToString("dd MMM yyyy") + "' and Empl_ID ='" + empl_id + "' ";
                    }

                    line_plan.GET_SQL_DATA(cmd_sql);
                    foreach (DataRow his in line_plan.DaTable.Rows)
                    {
                        row["Shift_His"] = his["ShiftName"].ToString().Trim();
                        row["LineID_His"] = his["LineID"].ToString().Trim();
                        row["WST_ID_His"] = his["WST_ID"].ToString().Trim();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
