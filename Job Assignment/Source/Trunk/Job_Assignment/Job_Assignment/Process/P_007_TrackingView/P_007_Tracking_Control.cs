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

        SQL_API.SQL_ATC P007_Tracking_sqlObj;
        private bool P007_Tracking_Get_Plan_Empl(DateTime date)
        {
            string sql_cmd;
            bool b;
            string shift, line, wst_id;
            string[] planEmpl;

            sql_cmd = String.Format("SELECT * FROM [P007_P008_Tracking] WHERE [Date] = '{0}' order by LineId", date.ToString("yyyy-MMM-dd"));
            b = P007_Tracking_View_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);

            if (b == false)
            {
                return false;
            }

            Load_Tracking_date(date);

            foreach (DataRow row in P007_Tracking_View_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows)
            {
                shift = row["ShiftName"].ToString().Trim();
                line = row["LineID"].ToString().Trim();
                wst_id = row["WST_ID"].ToString().Trim();
                planEmpl = Get_Plan_Empl(date, shift, line, wst_id);
                row["Plan_Empl_ID"] = planEmpl[0];
                row["Plan_Empl_Name"] = planEmpl[1];
            }

            Update_SQL_Data(P007_Tracking_View_MasterDatabase.MasterDatabase_GridviewTBL.Data_da, P007_Tracking_View_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb);
            // P007_Tracking_View_MasterDatabase.MasterDatabase_GridviewTBL.Save_Data();

            return true;
        }

        bool Load_Tracking_date(DateTime date)
        {
            string sql_cmd;
            if (P007_Tracking_sqlObj == null)
            {
                P007_Tracking_sqlObj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            }
            sql_cmd = "SELECT [ShiftName], [Date], [LineID], [WST_ID], [Empl_ID], [Empl_Name] FROM [JOB_ASSIGNMENT_DB].[dbo].[P_003_KeHoachSanXuatTheoLine] ";
            sql_cmd += " WHERE Date = '" + date.ToString("dd MMM yyyy") + "' ";
            P007_Tracking_sqlObj.GET_SQL_DATA(sql_cmd);
            return true;
        }

        string[] Get_Plan_Empl(DateTime date, string shift, string line, string wst_id)
        {
            string[] empl_info = { "", "" };
            string cur_date, cur_shift, cur_line, cur_wst_id;
            DateTime cur_date1;
            string empl_id, empl_name;
            foreach (DataRow row in P007_Tracking_sqlObj.DaTable.Rows)
            {
                cur_date = row["Date"].ToString().Trim();
                cur_date1 = DateTime.Parse(cur_date);
                cur_shift = row["ShiftName"].ToString().Trim();
                cur_line = row["LineID"].ToString().Trim();
                cur_wst_id = row["WST_ID"].ToString().Trim();

                //empl_id = row["Empl_ID"].ToString().Trim();
                //empl_name = row["Empl_Name"].ToString().Trim();

                if (cur_date1.Date == date.Date && cur_shift == shift && cur_line == line && wst_id == cur_wst_id)
                {
                    empl_info[0] = row["Empl_ID"].ToString().Trim();
                    empl_info[1] = row["Empl_Name"].ToString().Trim();
                }
            }
           return empl_info;

        }

        void P_007_Tracking_Control_MasterDatabase_GridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //string plan_empl, tracking_empl;
            ////foreach (DataGridViewRow row in P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows)
            //foreach (DataGridViewRow row in P007_Tracking_View_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows)
            //{
            //    // kiem tra plan và tracking giong nhau khong
            //    plan_empl = row.Cells["Plan_Empl_ID"].Value == null ? "" : row.Cells["Plan_Empl_ID"].Value.ToString().Trim();
            //    tracking_empl = row.Cells["Empl_ID"].Value == null ? "" : row.Cells["Empl_ID"].Value.ToString().Trim();
            //    if (plan_empl == "")
            //    {
            //        row.DefaultCellStyle.BackColor = Color.Yellow;
            //    }
            //    else if (tracking_empl == "")
            //    {
            //        row.DefaultCellStyle.BackColor = Color.LightBlue;
            //    }
            //    else if (plan_empl != tracking_empl)
            //    {
            //        row.DefaultCellStyle.BackColor = COLOR_LINE_NOT_HAVE_EMPLOYEE;
            //    }
            //}
        }
    }
}