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
        private bool DeleteInputFromPlaner(DateTime date)
        {
            bool result;
            string cmd = @"Delete FROM [P_001_InputFromPlanner] 
                            WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            result = Update_Data_Info(MasterDatabase_Connection_Str, cmd);
            return result;
        }

        DataTable Get_Kitting_Data(DateTime date)
        {
            string sql_cmd = @"SELECT * FROM [OpenPOPlanner] WHERE ActiveDateTime = '" + date.ToString("dd MMM yyyy") + "'";
            DataTable temp_dtb = new DataTable();
            DataSet inputData_tbl = new DataSet();
            SqlDataAdapter addapter = new SqlDataAdapter();
            if (temp_dtb != null)
            {
                temp_dtb.Clear();
            }
            temp_dtb = Get_SQL_Data(Kitting_Connection_Str, sql_cmd, ref addapter, ref inputData_tbl);
            return temp_dtb;
        }
    }
}