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
        public DataTable KHSX_WS_dtb = new DataTable();
        public DataSet KHSX_WS_ds = new DataSet();
        public SqlDataAdapter KHSX_WS_da;

        public DataTable KHSX_WS_Temp_dtb = new DataTable();
        public DataSet KHSX_WS_Temp_ds = new DataSet();
        public SqlDataAdapter KHSX_WS_Temp_da;

        public DataTable KHSX_dtb = new DataTable();
        public DataSet KHSX_ds = new DataSet();
        public SqlDataAdapter KHSX_da;

        public DataTable WS_List_dtb = new DataTable();
        public DataSet WS_List_ds = new DataSet();
        public SqlDataAdapter WS_List_da;

        public DataTable MDB04_Line_Vs_Skill_dtb = new DataTable();
        public DataSet MDB04_Line_Vs_Skill_ds = new DataSet();
        public SqlDataAdapter MDB04_Line_Vs_Skill_da;

        public DataTable MDB_002_Empl_Skill_dtb = new DataTable();
        public DataSet MDB_002_Empl_Skill_ds = new DataSet();
        public SqlDataAdapter MDB_002_Empl_Skill_da;
        
        public DataTable Load_KHSX_WS_Temp_DB_Date(DateTime select_date)
        {
            string sql_cmd = @"SELECT * FROM [P_004_KeHoachSanXuatTheoTram]";
            sql_cmd += " WHERE [Date] = '" + select_date.ToString("dd MMM yyyy") + "'";

            if (KHSX_WS_Temp_dtb != null)
            {
                KHSX_WS_Temp_dtb.Clear();
            }
            KHSX_WS_Temp_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref KHSX_WS_Temp_da, ref KHSX_WS_Temp_ds);
            return KHSX_WS_Temp_dtb;
        }

        public DataTable Load_KHSX_WS_DB_Date(DateTime select_date)
        {
            string sql_cmd = @"SELECT * FROM [P_004_KeHoachSanXuatTheoTram]";
            sql_cmd += " WHERE [Date] = '" + select_date.ToString("dd MMM yyyy") + "'";

            if (KHSX_WS_dtb != null)
            {
                KHSX_WS_dtb.Clear();
            }
            KHSX_WS_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref KHSX_WS_da, ref KHSX_WS_ds);
            return KHSX_WS_dtb;
        }

        public DataTable Load_KHSX_DB_Date(DateTime select_date)
        {
            string sql_cmd = @"SELECT * FROM [P_002_PlanForProductionByDate]";
            sql_cmd += " WHERE [Date] = '" + select_date.ToString("dd MMM yyyy") + "' ORDER BY Priority";

            if (KHSX_dtb != null)
            {
                KHSX_dtb.Clear();
            }
            KHSX_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref KHSX_da, ref KHSX_ds);
            return KHSX_dtb;
        }

        private bool Clean_KHSX_WS_Date(DateTime select_date )
        {
            Load_KHSX_WS_DB_Date(select_date);
            //KHSX_WS_dtb.Clear();

            var rows = KHSX_WS_dtb.Select();

            foreach (var row in rows)
            {
                row.Delete();            
            }

            Update_SQL_Data(KHSX_WS_da, KHSX_WS_dtb);
            return true;
        }

        private DataTable Load_WS_List(string LineID, string PartNumber)
        {
            string sql_cmd = @"SELECT * FROM [MDB_003_Line_Desciption]";
            sql_cmd += " WHERE [LineID] = '" + LineID + "' AND [PartNumber] = '" + PartNumber + "'";

            if (WS_List_dtb != null)
            {
                WS_List_dtb.Clear();
            }
            WS_List_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref WS_List_da, ref WS_List_ds);
            return WS_List_dtb;
        }

        private DataTable Load_MDB04_Line_Vs_Skill ()
        {
            string sql_cmd = @"SELECT * FROM [MDB_004_LineSkillRequest]";

            if (MDB04_Line_Vs_Skill_dtb != null)
            {
                MDB04_Line_Vs_Skill_dtb.Clear();
            }
            MDB04_Line_Vs_Skill_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref MDB04_Line_Vs_Skill_da, ref MDB04_Line_Vs_Skill_ds);
            return MDB04_Line_Vs_Skill_dtb;
        }

        private DataTable Load_MDB_002_Empl_Skill()
        {
            string sql_cmd = @"SELECT * FROM [MDB_002_Empl_Skill]";

            if (MDB_002_Empl_Skill_dtb != null)
            {
                MDB_002_Empl_Skill_dtb.Clear();
            }
            MDB_002_Empl_Skill_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref MDB_002_Empl_Skill_da, ref MDB_002_Empl_Skill_ds);
            return MDB_002_Empl_Skill_dtb;
        }

        private DataTable Load_MDB_002_Empl_Skill(string Empl_Id)
        {
            string sql_cmd = @"SELECT * FROM [MDB_002_Empl_Skill]";
            sql_cmd += " WHERE [Empl_Id] = " + Empl_Id + "'";

            if (MDB_002_Empl_Skill_dtb != null)
            {
                MDB_002_Empl_Skill_dtb.Clear();
            }
            MDB_002_Empl_Skill_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref MDB_002_Empl_Skill_da, ref MDB_002_Empl_Skill_ds);
            return MDB_002_Empl_Skill_dtb;
        }
        
    }
}