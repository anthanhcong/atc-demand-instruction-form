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
        public DataTable All_Empl_List_Tbl = new DataTable();
        DataSet All_Empl_List_ds = new DataSet();
        SqlDataAdapter All_Empl_List_da;

        private bool Load_All_Empl_List()
        {
            string sql_cmd = @"SELECT * FROM [SHIFT_REGISTER_DB].[dbo].[Empl_List] Where [Active] = 'True'";
            if (All_Empl_List_Tbl != null)
            {
                All_Empl_List_Tbl.Clear();
            }
            All_Empl_List_Tbl = Get_SQL_Data(LeaveRegister_Connection_Str, sql_cmd, ref All_Empl_List_da, ref All_Empl_List_ds);
            return true;
        }

        private bool Is_exist_Empl_List(string empl_id)
        {
            string cur_empl_id;
            foreach (DataRow row in All_Empl_List_Tbl.Rows)
            {
                cur_empl_id = row["Empl_ID"].ToString().Trim();
                if (cur_empl_id == empl_id)
                {
                    return true;
                }
            }
            return false;
        }

        //public string Get_Empl_Name(string cur_msnv)
        //{
        //    string empl_name = "", msnv;

        //    foreach (DataRow row in All_Empl_List_Tbl.Rows)
        //    {
        //        msnv = row["Empl_ID"].ToString().Trim();
        //        if (msnv == cur_msnv)
        //        {
        //            empl_name = row["Last_Name"].ToString().Trim();
        //            empl_name += " " + row["Mid_Name"].ToString().Trim();
        //            empl_name += " " + row["First_Name"].ToString().Trim();
        //            break;
        //        }
        //    }
        //    return empl_name;
        //}

        public string Get_Empl_Name(string cur_msnv)
        {
            string empl_name = "";
            SQL_API.SQL_ATC sql_cm = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string sql_cmd = @"SELECT *
                                FROM [MDB_002_Empl_Skill] 
                                WHERE [Empl_ID] = '" + cur_msnv.Trim() + "'";
            sql_cm.GET_SQL_DATA(sql_cmd);
            if ((sql_cm.DaTable != null) && (sql_cm.DaTable.Rows.Count > 0))
            {
                empl_name = sql_cm.DaTable.Rows[0]["Empl_Name"].ToString().Trim();
            }

            return empl_name;
        }

        private string[] Get_Name_and_Department_Empl(string msnv)
        {
            string[] ret_val = { "", "" };
            string cur_msnv;

            foreach (DataRow row in All_Empl_List_Tbl.Rows)
            {
                cur_msnv = row["Empl_ID"].ToString().Trim();
                if (msnv == cur_msnv)
                {
                    ret_val[0] = row["Last_Name"].ToString().Trim();
                    ret_val[0] += " " + row["Mid_Name"].ToString().Trim();
                    ret_val[0] += " " + row["First_Name"].ToString().Trim();
                    ret_val[1] = row["Department"].ToString().Trim();
                    break;
                }
            }
            return ret_val;
        }

        
    }
}