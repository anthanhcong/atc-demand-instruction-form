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
        public DataTable Tracking_Cur_Shift_dtb = new DataTable();
        public DataSet Tracking_Cur_Shift_ds = new DataSet();
        public SqlDataAdapter Tracking_Cur_Shift_da;
        private string Get_Shift_ID(DateTime current)
        {
            int hour = current.Hour;
            string curr_time = current.ToString("HH:mm");
            string cur_shift = "";
            string sql_cmd = "SELECT * FROM [MDB_006_Shift_Description]";
            sql_cmd += " WHERE " + curr_time + " BETWEEN [From_Time] AND [To_Time]";

            if (Tracking_Cur_Shift_dtb != null)
            {
                Tracking_Cur_Shift_dtb.Clear();
            }
            Tracking_Cur_Shift_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref Tracking_Cur_Shift_da, ref Tracking_Cur_Shift_ds);
            if (Tracking_Cur_Shift_dtb.Rows.Count > 0)
            {
                cur_shift = Tracking_Cur_Shift_dtb.Rows[0]["ShiftName"].ToString().Trim();
            }

            return cur_shift;
        }

        private string[] Get_Shift_Time(string shift_name)
        {
            string[] shift_time = new string[2];
            string sql_cmd = "SELECT * FROM [MDB_006_Shift_Description]";
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
    }
}