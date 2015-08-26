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
        public DataTable Empl_Skill_dtb = new DataTable();
        public DataSet Empl_Skill_ds = new DataSet();
        public SqlDataAdapter Empl_Skill_da;

        public DataTable Load_All_Empl_Skill()
        {
            string sql_cmd = @"SELECT * FROM [MDB_002_Empl_Skill] ";

            if (Empl_Skill_dtb != null)
            {
                Empl_Skill_dtb.Clear();
            }
            Empl_Skill_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref Empl_Skill_da, ref Empl_Skill_ds);
            return Empl_Skill_dtb;
        }

        public DataTable Load_All_Empl()
        {
            string sql_cmd = @"SELECT distinct [Empl_ID],[Empl_Name] 
                                FROM [MDB_002_Empl_Skill] ";

            if (Empl_Skill_dtb != null)
            {
                Empl_Skill_dtb.Clear();
            }
            Empl_Skill_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref Empl_Skill_da, ref Empl_Skill_ds);
            return Empl_Skill_dtb;
        }
    }
}