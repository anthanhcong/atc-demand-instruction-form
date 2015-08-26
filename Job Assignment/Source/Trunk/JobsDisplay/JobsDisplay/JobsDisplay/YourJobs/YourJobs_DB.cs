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
using System.Collections;

namespace JobsDisplay
{
    public partial class Form1 : SQL_APPL
    {
        public DataTable JobsPlan_dtb = new DataTable();
        public DataSet JobsPlan_ds = new DataSet();
        public SqlDataAdapter JobsPlan_da;

        public DataTable WST_Status_dtb = new DataTable();
        public DataSet WST_Status_ds = new DataSet();
        public SqlDataAdapter WST_Status_da;

        public DataTable Cur_JobsPlan_dtb = new DataTable();
        public DataSet Cur_JobsPlan_ds = new DataSet();
        public SqlDataAdapter Cur_JobsPlan_da;

        public DataTable Load_Job_Plan(string empl_id, DateTime date)
        {
            string from_date_str = date.ToString("dd MMM yyyy");
            string to_date_str = date.AddDays(7).ToString("dd MMM yyyy");

            string sql_cmd = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[P_003_KeHoachSanXuatTheoLine] ";
            sql_cmd += " WHERE Empl_ID = '" + empl_id + "'";
            sql_cmd += " AND Date Between + '" + from_date_str + "' AND '" + to_date_str + "'";

            if (JobsPlan_dtb != null)
            {
                JobsPlan_dtb.Clear();
            }
            JobsPlan_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref JobsPlan_da, ref JobsPlan_ds);
            return JobsPlan_dtb;
        }

        public DataTable Load_Cur_JobsPlan_Details(string empl_id, DateTime date)
        {
            if (empl_id == "")
            {
                return null;
            }
            string date_str = date.ToString("dd MMM yyyy");
            string time_str = date.ToString("HH:mm");
            string sql_cmd = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[P_003_KeHoachSanXuatTheoLine] ";
            sql_cmd += " WHERE Empl_ID = '" + empl_id + "'";
            sql_cmd += " AND Date = '" + date_str + "'";
            sql_cmd += " AND ('" + time_str + "' BETWEEN From_Time AND To_Time";
            sql_cmd += " OR ('" + time_str + "' BETWEEN '18:00:00' AND '23:59:59' AND ShiftName = 'SHIFT_3')";
            sql_cmd += " OR ('" + time_str + "' BETWEEN '00:00:00' AND '06:00:00' AND ShiftName = 'SHIFT_3'))"; // edit Thuy: them so 0 truoc so 6 (6:00:00)

            if (Cur_JobsPlan_dtb != null)
            {
                Cur_JobsPlan_dtb.Clear();
            }
            Cur_JobsPlan_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref Cur_JobsPlan_da, ref Cur_JobsPlan_ds);
            return JobsPlan_dtb;
        }

        public DataTable Load_Cur_JobsPlan(string empl_id, DateTime date)
        {
            SQL_API.SQL_ATC job_plan_table = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            if (empl_id == "")
            {
                return null;
            }
            string date_str = date.ToString("dd MMM yyyy");
            string time_str = date.ToString("HH:mm");
            string sql_cmd = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[P_003_KeHoachSanXuatTheoLine] ";
            sql_cmd += " WHERE Empl_ID = '" + empl_id + "'";
            sql_cmd += " AND Date = '" + date_str + "'";
            sql_cmd += " AND ('" + time_str + "' BETWEEN From_Time AND To_Time";
            sql_cmd += " OR ('" + time_str + "' BETWEEN '18:00:00' AND '23:59:59' AND ShiftName = 'SHIFT_3')";
            sql_cmd += " OR ('" + time_str + "' BETWEEN '00:00:00' AND '06:00:00' AND ShiftName = 'SHIFT_3'))"; // edit Thuy: them so 0 truoc so 6 (6:00:00)

            job_plan_table.GET_SQL_DATA(sql_cmd);
            return job_plan_table.DaTable;
        }

        public DataTable Load_Production_Plan(string line_id, string wst_id, DateTime current)
        {
            if ((line_id == "") || (wst_id == ""))
            {
                return null;
            }
            string date_str = current.ToString("dd MMM yyyy");
            string time_str = current.ToString("HH:mm");
            string sql_cmd = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[P_004_KeHoachSanXuatTheoTram] ";
            sql_cmd += " WHERE LineID = '" + line_id + "'";
            if (wst_id != "")
            {
                sql_cmd += " AND WST_ID = '" + wst_id + "'";
            }
            sql_cmd += " AND Date = '" + date_str + "'";
            sql_cmd += " AND ('" + time_str + "' BETWEEN From_Time AND To_Time";
            sql_cmd += " OR ('" + time_str + "' BETWEEN '18:00:00' AND '23:59:59' AND ShiftName = 'SHIFT_3')";
            sql_cmd += " OR ('" + time_str + "' BETWEEN '00:00:00' AND '06:00:00' AND ShiftName = 'SHIFT_3'))"; // edit Thuy: them so 0 truoc so 6 (6:00:00)

            if (WST_Status_dtb != null)
            {
                WST_Status_dtb.Clear();
            }
            WST_Status_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref WST_Status_da, ref WST_Status_ds);
            return WST_Status_dtb;
        }
    }
}