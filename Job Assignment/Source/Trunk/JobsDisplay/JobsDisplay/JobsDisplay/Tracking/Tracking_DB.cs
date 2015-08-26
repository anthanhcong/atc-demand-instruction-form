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
using System.Threading;
using LayoutControl;

namespace JobsDisplay
{
    public partial class Form1 : SQL_APPL
    {
        public DataTable JobsTracking_dtb = new DataTable();
        public DataSet JobsTracking_ds = new DataSet();
        public SqlDataAdapter JobsTracking_da;

        public DataTable WST_Tracking_dtb = new DataTable();
        public DataSet WST_Tracking_ds = new DataSet();
        public SqlDataAdapter WST_Tracking_da;

        public DataTable Load_Job_Tracking(string empl_id, DateTime date)
        {
            string date_str = date.ToString("dd MMM yyyy");
            string time_str = date.ToString("HH:mm:ss");

            string sql_cmd = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[P007_P008_Tracking] ";
            sql_cmd += " WHERE Empl_ID = '" + empl_id + "'";
            sql_cmd += " AND Date = '" + date_str + "'";
            //sql_cmd += " AND ('" + time_str + "' BETWEEN From_Time AND To_Time";
            sql_cmd += " AND ('" + time_str + "' BETWEEN From_Time AND To_Time OR To_Time is null"; // edit Thuy
            sql_cmd += " OR ('" + time_str + "' BETWEEN '18:00:00' AND '23:59:59' AND ShiftName = 'SHIFT_3')";
            sql_cmd += " OR ('" + time_str + "' BETWEEN '00:00:00' AND '06:00:00' AND ShiftName = 'SHIFT_3'))"; // edit Thuy: them so 0 truoc so 6 (6:00:00)
            //sql_cmd += " OR ('" + time_str + "' BETWEEN '00:00:00' AND '6:00:00' AND ShiftName = 'SHIFT_3'))";

            if (JobsTracking_dtb != null)
            {
                JobsTracking_dtb.Clear();
            }
            JobsTracking_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref JobsTracking_da, ref JobsTracking_ds);
            return JobsPlan_dtb;
        }

        public DataTable Load_WST_Tracking(string line_id, string wst_id, DateTime current)
        {
            string date_str = current.ToString("dd MMM yyyy");
            string time_str = current.ToString("HH:mm:ss");
            string shift_id = Get_Shift_ID(current);
            string sql_cmd = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[P007_P008_Tracking] ";
            sql_cmd += " WHERE LineID = '" + line_id + "'";
            if (wst_id != "")
            {
                sql_cmd += " AND WST_ID = '" + wst_id + "'";
            }
            sql_cmd += " AND Date = '" + date_str + "'";
            //sql_cmd += " AND '" + time_str + "' BETWEEN From_Time AND To_Time";
            sql_cmd += " AND ShiftName = '" + shift_id + "'";
            sql_cmd += " ORDER by [From_Time] DESC";

            if (WST_Tracking_dtb != null)
            {
                WST_Tracking_dtb.Clear();
            }
            WST_Tracking_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref WST_Tracking_da, ref WST_Tracking_ds);
            return WST_Tracking_dtb;
        }

        DataTable Get_Kitting_Data(DateTime date)
        {
            string sql_cmd = @"SELECT [ActiveDateTime]
                                  ,[Sector]
                                  ,[Series]
                                  ,[TopPONumber]
                                  ,[TopModel]
                                  ,[TypeCO]
                                  ,[Priority]
                                  ,[POQty]
                                FROM [OpenPOPlanner] 
                                WHERE ActiveDateTime BETWEEN '" + date.AddDays(-3).ToString("dd MMM yyyy")
                                                        + "' AND '" + date.ToString("dd MMM yyyy") + "'";
            DataTable temp_dtb = new DataTable();
            DataSet inputData_tbl = new DataSet();
            SqlDataAdapter addapter = new SqlDataAdapter();

            if (temp_dtb != null)
            {
                temp_dtb.Clear();
            }
            temp_dtb = Get_SQL_Data(Kitting_Connection_Str, sql_cmd, ref addapter, ref inputData_tbl);

            List_PO = temp_dtb.Copy();
            return temp_dtb;
        }

        public DataTable Tracking_Cur_Shift_dtb = new DataTable();
        public DataSet Tracking_Cur_Shift_ds = new DataSet();
        public SqlDataAdapter Tracking_Cur_Shift_da;
        private string Get_Shift_ID(DateTime current)
        {
            int hour = current.Hour;
            string curr_time = current.ToString("HH:mm");
            string cur_shift = "";
            string sql_cmd = "SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_006_Shift_Description] ";
            sql_cmd += "WHERE '" + curr_time + "' BETWEEN From_Time AND To_Time ";
            sql_cmd += "OR ('" + curr_time + "' BETWEEN '18:00:00' AND '23:59:59' AND ShiftName = 'SHIFT_3') ";
            sql_cmd += "OR ('" + curr_time + "' BETWEEN '00:00:00' AND '05:59:59' AND ShiftName = 'SHIFT_3') ";

            //string sql_cmd = "SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_006_Shift_Description]";
            //sql_cmd += " WHERE '" + curr_time + "' BETWEEN [From_Time] AND [To_Time]";
            //sql_cmd += " OR ('" + curr_time + "' BETWEEN '18:00:00' AND '23:59:59' AND ShiftName = 'SHIFT_3')";
            //sql_cmd += " OR ('" + curr_time + "' BETWEEN '00:00:00' AND '6:00:00' AND ShiftName = 'SHIFT_3')";

            if (Tracking_Cur_Shift_dtb != null)
            {
                Tracking_Cur_Shift_dtb.Clear();
            }
            Tracking_Cur_Shift_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref Tracking_Cur_Shift_da, ref Tracking_Cur_Shift_ds);
            if (Tracking_Cur_Shift_dtb.Rows.Count > 0)
            {
                cur_shift = Tracking_Cur_Shift_dtb.Rows[0]["ShiftName"].ToString().Trim();
            }else
            {
                cur_shift = "Shift_3";
            }
            return cur_shift;
        }

        private string Get_Line_ID_of_Part(string part)
        {
            string sql_cmd = "SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_003_Line_Desciption]";
            sql_cmd += " WHERE [PartNumber] = '" + part + "'";
            string line_id = "";

            DataTable temp_dtb = new DataTable();
            DataSet inputData_tbl = new DataSet();
            SqlDataAdapter addapter = new SqlDataAdapter();

            if (temp_dtb != null)
            {
                temp_dtb.Clear();
            }
            temp_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref addapter, ref inputData_tbl);
            if ((temp_dtb != null) && (temp_dtb.Rows.Count > 0))
            {
                line_id = temp_dtb.Rows[0]["LineID"].ToString().Trim();
            }

            return line_id;
        }

        private string[] Get_Shift_Time(string shift_name)
        {
            string[] shift_time = new string[2];
            string sql_cmd = "SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_006_Shift_Description]";
            sql_cmd += " WHERE [ShiftName] = '" + shift_name + "'";

            if (Tracking_Cur_Shift_dtb != null)
            {
                Tracking_Cur_Shift_dtb.Clear();
            }
            Tracking_Cur_Shift_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref Tracking_Cur_Shift_da, ref Tracking_Cur_Shift_ds);
            if (Tracking_Cur_Shift_dtb.Rows.Count > 0)
            {
                shift_time[0] = Tracking_Cur_Shift_dtb.Rows[0]["From_Time"].ToString().Trim();
                shift_time[1] = Tracking_Cur_Shift_dtb.Rows[0]["To_Time"].ToString().Trim();

            }
            return shift_time;
        }

        // add by thuy
        private string[] Get_Empl_Du(DateTime date, string close_his_PO, string wst)
        {
            string[] empl_info = { "", "", "" };
            SQL_API.SQL_ATC spl_obj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);

            string sql_cmd = @"SELECT Empl_ID, Empl_Name, WST_ID, From_Time
                                FROM [JOB_ASSIGNMENT_DB].[dbo].[P007_P008_Tracking] ";
            sql_cmd += "WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "' AND [PO] = '" + close_his_PO + "' AND WST_ID = '" + wst + "' ";
            sql_cmd += "ORDER BY WST_ID, From_Time desc";

            spl_obj.GET_SQL_DATA(sql_cmd);

            if ((spl_obj.DaTable != null) && (spl_obj.DaTable.Rows.Count > 0))
            {
                empl_info[0] = spl_obj.DaTable.Rows[0]["Empl_ID"].ToString().Trim();
                empl_info[1] = spl_obj.DaTable.Rows[0]["Empl_Name"].ToString().Trim();
                empl_info[2] = spl_obj.DaTable.Rows[0]["WST_ID"].ToString().Trim();
            }

            return empl_info;
        }

        private DataTable Get_Count_WST_HisPO(DateTime date, string close_his_PO)
        {
            string sql_cmd = @"SELECT distinct WST_ID
                                FROM [JOB_ASSIGNMENT_DB].[dbo].[P007_P008_Tracking]";
            sql_cmd += " WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "' AND [PO] = '" + close_his_PO + "' ORDER BY WST_ID";

            DataTable temp_dtb = new DataTable();
            DataSet inputData_tbl = new DataSet();
            SqlDataAdapter addapter = new SqlDataAdapter();

            if (temp_dtb != null)
            {
                temp_dtb.Clear();
            }
            temp_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref addapter, ref inputData_tbl);

            return temp_dtb;
        }

        private DataTable Get_Count_WST_CurPO(DateTime date, string cur_PO)
        {
            string sql_cmd = @"SELECT distinct WST_ID
                                FROM [JOB_ASSIGNMENT_DB].[dbo].[P007_P008_Tracking]";
            sql_cmd += " WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "' AND [PO] = '" + cur_PO + "' ORDER BY WST_ID";

            DataTable temp_dtb = new DataTable();
            DataSet inputData_tbl = new DataSet();
            SqlDataAdapter addapter = new SqlDataAdapter();

            if (temp_dtb != null)
            {
                temp_dtb.Clear();
            }
            temp_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref addapter, ref inputData_tbl);

            return temp_dtb;
        }
        // end

        private DataTable Get_Tracking_PO_Date(DateTime date, string po)
        {
            string sql_cmd = @"SELECT *
                                FROM [JOB_ASSIGNMENT_DB].[dbo].[P007_P008_Tracking]";
            sql_cmd += " WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "' AND [PO] = '" + po + "'";

            DataTable temp_dtb = new DataTable();
            DataSet inputData_tbl = new DataSet();
            SqlDataAdapter addapter = new SqlDataAdapter();

            if (temp_dtb != null)
            {
                temp_dtb.Clear();
            }
            temp_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref addapter, ref inputData_tbl);

            return temp_dtb;
        }

        private DataTable Load_WST_Part(string part)
        {
            string sql_cmd = "SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_003_Line_Desciption]";
            sql_cmd += " WHERE [PartNumber] = '" + part + "'";

            DataTable temp_dtb = new DataTable();
            DataSet inputData_tbl = new DataSet();
            SqlDataAdapter addapter = new SqlDataAdapter();

            if (temp_dtb != null)
            {
                temp_dtb.Clear();
            }
            temp_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref addapter, ref inputData_tbl);
            return temp_dtb;
        }

        public DataTable Current_Line_Status;
        public DataSet Current_Line_Status_ds = new DataSet();
        public SqlDataAdapter Current_Line_Status_da;

        DataTable Load_Current_Line_Status(DateTime date)
        {
            string sql_cmd = @"SELECT *
                                FROM [JOB_ASSIGNMENT_DB].[dbo].[P007_P008_Tracking]";
            sql_cmd += " WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            sql_cmd += " AND [LineID] = '" + Cur_Line_ID.Trim() +"'";
            sql_cmd += " AND [To_Time] IS NULL";

            if (Current_Line_Status != null)
            {
                Current_Line_Status.Clear();
            }
            Current_Line_Status = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref Current_Line_Status_da, ref Current_Line_Status_ds);

            BindingSource bs = new BindingSource();
            bs.DataSource = Current_Line_Status;
            Tracking_Status_GridView.DataSource = bs;
            Tracking_Kitting_PO_Grv_BindingContextChanged(null, null);

            return Current_Line_Status;
        }

        string Get_Current_Shift_Name(string line, DateTime date)
        {
            SQL_API.SQL_ATC sql_cmn = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);

            string date_str = date.ToString("dd MMM yyyy");
            string time_str = date.ToString("HH:mm");
            string sql_cmd = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[P_003_KeHoachSanXuatTheoLine] ";
            sql_cmd += " WHERE LineID = '" + line + "'";
            sql_cmd += " AND Date = '" + date_str + "'";
            sql_cmd += " AND ('" + time_str + "' BETWEEN From_Time AND To_Time";
            sql_cmd += " OR ('" + time_str + "' BETWEEN '18:00:00' AND '23:59:59' AND ShiftName = 'SHIFT_3')";
            sql_cmd += " OR ('" + time_str + "' BETWEEN '00:00:00' AND '06:00:00' AND ShiftName = 'SHIFT_3'))";
            string shift = "";

            sql_cmn.GET_SQL_DATA(sql_cmd);
            if ((sql_cmn.DaTable != null) && (sql_cmn.DaTable.Rows.Count > 0))
            {
                shift = sql_cmn.DaTable.Rows[0]["ShiftName"].ToString().Trim();
            }

            if (shift == "")
            {
                shift = Get_Shift_ID(Cur_Date);
            }
            return shift;
        }

        private bool HasEmptyWST()
        {
            string empl_id;
            Load_Current_Line_Status(Cur_Date);
            foreach (DataRow row in Current_Line_Status.Rows)
            {
                empl_id = row["Empl_ID"].ToString().Trim();
                if (empl_id == "")
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsForceClose()
        {
            SQL_API.SQL_ATC sql_cm = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string cmd = @"SELECT [ForceTurnOff] FROM [ForceTurnOff]";
            bool force_clode = true;
            sql_cm.GET_SQL_DATA(cmd);

            if ((sql_cm.DaTable != null) && (sql_cm.DaTable.Rows.Count > 0))
            {
                try
                {
                    force_clode = (bool)(sql_cm.DaTable.Rows[0]["ForceTurnOff"] == null ? false : sql_cm.DaTable.Rows[0]["ForceTurnOff"]);
                }
                catch
                {
                }
            }
            return force_clode;
        }

        private DataTable Get_Line_Cur_PO(string lineid, DateTime date)
        {
            SQL_API.SQL_ATC sql_cm = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string sql_cmd = @"SELECT distinct [PO]
                                FROM [JOB_ASSIGNMENT_DB].[dbo].[P007_P008_Tracking]";
            sql_cmd += " WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            sql_cmd += " AND [LineID] = '" + Cur_Line_ID.Trim() + "'";
            sql_cmd += " AND [To_Time] IS NULL";

            sql_cm.GET_SQL_DATA(sql_cmd);
            return sql_cm.DaTable;
        }

        private bool Is_Done_PO(string po)
        {
            SQL_API.SQL_ATC sql_cm = new SQL_API.SQL_ATC(Kitting_Connection_Str);
            string sql_cmd = @"SELECT [Sector]
                                  ,[Series]
                                  ,[TopPONumber]
                                  ,[TopModel]
                                  ,[TypeCO]
                                  ,[Priority]
                                  ,[POQty]
                                FROM [OpenPOPlanner] 
                                WHERE [TopPONumber] = '" + po.Trim() + "'";
            sql_cm.GET_SQL_DATA(sql_cmd);
            string type_po = sql_cm.DaTable.Rows[0]["TypeCO"].ToString().Trim();
            if (type_po == "Done PO")
            {
                return true;
            }
            return false;
        }

        private DataTable Get_Kitting_Cur_PO(string lineid)
        {
            SQL_API.SQL_ATC sql_cm = new SQL_API.SQL_ATC(Kitting_Connection_Str);
            string sql_cmd = @"SELECT [Sector]
                                  ,[Series]
                                  ,[TopPONumber]
                                  ,[TopModel]
                                  ,[TypeCO]
                                  ,[Priority]
                                  ,[POQty]
                                FROM [OpenPOPlanner] 
                                WHERE [lineid] like '%" + lineid.Trim() + "%'";
            sql_cm.GET_SQL_DATA(sql_cmd);
            return sql_cm.DaTable;
        }

        private string [] Get_Empl_for_WST(string wst, string shift)
        {
            string [] empl_info = {"", ""};
            bool out_manual;
            SQL_API.SQL_ATC sql_cm = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string sql_cmd = @"SELECT *
                                FROM [JOB_ASSIGNMENT_DB].[dbo].[P007_P008_Tracking]";
            sql_cmd += " WHERE [Date] = '" + Cur_Date.ToString("dd MMM yyyy") + "'";
            sql_cmd += " AND [WST_ID] = '" + wst.Trim() + "'";
            sql_cmd += " AND [ShiftName] ='" + shift + "'";
            sql_cmd += " AND [To_Time] IS NOT NULL";
            sql_cmd += " AND [Empl_ID] != '' AND [Empl_ID] is not NULL ORDER by [To_Time] DESC";
            
            sql_cm.GET_SQL_DATA(sql_cmd);

            if ((sql_cm.DaTable != null) && (sql_cm.DaTable.Rows.Count > 0))
            {
                out_manual = sql_cm.DaTable.Rows[0]["Out_Manual"] == DBNull.Value ? false : (bool)sql_cm.DaTable.Rows[0]["Out_Manual"];
                if (out_manual == false)
                {
                    empl_info[0] = sql_cm.DaTable.Rows[0]["Empl_ID"].ToString().Trim();
                    empl_info[1] = sql_cm.DaTable.Rows[0]["Empl_Name"].ToString().Trim();
                }
            }
            return empl_info;
        }
    }
}