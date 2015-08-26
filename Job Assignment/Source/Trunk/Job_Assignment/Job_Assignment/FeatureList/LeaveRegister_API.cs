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

namespace Job_Assignment
{
    public partial class Form1 : SQL_APPL
    {
        SQL_API.SQL_ATC Leave_info;

        public DataTable Load_Leave_Register(DateTime select_date)
        {
            if (Leave_info == null)
            {
                Leave_info = new SQL_API.SQL_ATC(LeaveRegister_Connection_Str);
            }
            string sql_cmd = @"SELECT * FROM [SHIFT_REGISTER_DB].[dbo].[Leave_Register_DB]";
            sql_cmd += " WHERE [AttDate] = '" + select_date.ToString("dd MMM yyyy") + "'";
            Leave_info.GET_SQL_DATA(sql_cmd);
            return Leave_info.DaTable;
        }

        public bool Is_Absent(string empl_id, DateTime date)
        {
            string cur_empl;

            //TODO: Implement Is_Absent --> DONE
            foreach (DataRow row in Leave_info.DaTable.Rows)
            {
                cur_empl = row["Empl_ID"].ToString().Trim();
                if (empl_id == cur_empl)
                {
                    return true;
                }
            }
            return false;
        }
    }
}