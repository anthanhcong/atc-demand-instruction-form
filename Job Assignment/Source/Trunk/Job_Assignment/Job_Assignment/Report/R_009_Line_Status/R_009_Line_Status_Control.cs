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
        private bool R_009_Line_Status_Refresh()
        {
            string sql_cmd;
            bool b;
            DataRow newrow;
            string mess;
            int i = 0;
            int countT = 0;
            int total = 0;
            string wst_id, wst_name;
            string[] current_Tracking, current_Plan, get_Line;
            string cur_shiftName, cur_lineID, cur_subLine_ID;
            string plan_shiftName, plan_lineID, plan_subLine_ID;
            string line_shiftName, line_lineID, line_subLine_ID;
            string line;

            sql_cmd = String.Format("SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R_009_Line_Status] WHERE [Date] = '{0}'", DateTime.Now.ToString("MMM dd yyyy"));
            b = R_009_Line_Status_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);

            if (b == false)
            {
                return false;
            }

            StatusLabel1.Visible = true;
            ProgressBar1.Visible = true;
            StatusLabel1.Text = "Refresh Line_Status";

            DeleteReport_Line_Status(DateTime.Now);
            R_009_Line_Status_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            //int count = R_009_Line_Status_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Count;
            //if (count > 0)
            //{
            //    mess = "Plan for date:" + DateTime.Now.ToString("MMM dd yyyy") + "was existing\n";
            //    mess += "Do you want to delete and create the new one?";

            //    if (MessageBox.Show(mess, "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
            //    {
            //        return false;
            //    }

            //    DeleteReport_Line_Status(DateTime.Now);
            //    R_009_Line_Status_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            //}

            // load all WST
            DataTable all_wst = Get_All_WST();

            if (all_wst != null)
            {
                countT = all_wst.Rows.Count;
            }
            total = countT;

            foreach (DataRow row in all_wst.Rows)
            {
                wst_id = row["WST_ID"].ToString().Trim();
                wst_name = row["WST_Name"].ToString().Trim();
                line = row["LineID"].ToString().Trim();

                current_Tracking = Get_Current_Tracking(wst_id, DateTime.Now);
                cur_shiftName = current_Tracking[0];
                cur_lineID = current_Tracking[1];
                cur_subLine_ID = current_Tracking[2];

                current_Plan = Get_Current_Plan(wst_id, DateTime.Now);
                plan_shiftName = current_Plan[0];
                plan_lineID = current_Plan[1];
                plan_subLine_ID = current_Plan[2];

                get_Line = Get_line(wst_id);
                line_shiftName = get_Line[0];
                line_lineID = get_Line[1];
                line_subLine_ID = get_Line[2];

                newrow = R_009_Line_Status_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.NewRow();
                newrow["Date"] = DateTime.Now.ToString("MM/dd/yyyy");
                newrow["WST_ID"] = wst_id;
                newrow["WST_Name"] = wst_name;
                newrow["LineID"] = line;

                if (cur_lineID != "" && plan_lineID != "")
                {
                    newrow["ShiftName"] = cur_shiftName;
                    newrow["SubLine_ID"] = cur_subLine_ID;
                    newrow["Current_Status"] = true;
                    newrow["Plan_Status"] = true;
                }
                else if (cur_lineID != "" && plan_lineID == "")
                {
                    newrow["ShiftName"] = cur_shiftName;
                    newrow["SubLine_ID"] = cur_subLine_ID;
                    newrow["Current_Status"] = true;
                }
                else if (cur_lineID == "" && plan_lineID != "")
                {
                    newrow["ShiftName"] = plan_shiftName;
                    newrow["SubLine_ID"] = plan_subLine_ID;
                    newrow["Plan_Status"] = true;
                }
                else
                {
                    newrow["ShiftName"] = line_shiftName;
                    newrow["SubLine_ID"] = line_subLine_ID;
                }

                R_009_Line_Status_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Add(newrow);

                i++;
                ProgressBar1.Value = i * 100 / total;
            }

            Update_SQL_Data(R_009_Line_Status_MasterDatabase.MasterDatabase_GridviewTBL.Data_da, R_009_Line_Status_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb);

            StatusLabel1.Visible = false;
            ProgressBar1.Visible = false;
                        
            return true;
        }

        //void R_009_Line_Status_MasterDatabase_GridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        //{
        //    bool plan, current;
        //    foreach (DataGridViewRow row in R_009_Line_Status_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows)
        //    {
        //        plan = row.Cells["Plan_Status"].Selected;
        //        current = row.Cells["Current_Status"].Selected;
        //        if (plan == true && current == true)
        //        {
        //            row.DefaultCellStyle.BackColor = Color.LightBlue;
        //        }
        //    }
        //}

        string[] Get_line(string wst_id)
        {
            string[] get_info = { "", "", "" };
            SQL_API.SQL_ATC sql_obj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string sql_cmd = @"SELECT distinct ShiftName, LineID, SubLine_ID 
                                FROM P_003_KeHoachSanXuatTheoLine 
                                WHERE WST_ID = '" + wst_id + "'";
            sql_obj.GET_SQL_DATA(sql_cmd);
            if (sql_obj.DaTable.Rows.Count > 0 && sql_obj.DaTable != null)
            {
                get_info[0] = sql_obj.DaTable.Rows[0]["ShiftName"].ToString().Trim();
                get_info[1] = sql_obj.DaTable.Rows[0]["LineID"].ToString().Trim();
                get_info[2] = sql_obj.DaTable.Rows[0]["SubLine_ID"].ToString().Trim();
            }
            return get_info;
        }

        string[] Get_Current_Plan(string wst_id, DateTime date)
        {
            string[] get_info = { "", "", "" };
            SQL_API.SQL_ATC sql_obj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string sql_cmd = @"SELECT distinct ShiftName, LineID, SubLine_ID
                                FROM P_003_KeHoachSanXuatTheoLine
                                WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "' and WST_ID = '" + wst_id + "'";
            sql_obj.GET_SQL_DATA(sql_cmd);
            if (sql_obj.DaTable.Rows.Count > 0 && sql_obj.DaTable != null)
            {
                get_info[0] = sql_obj.DaTable.Rows[0]["ShiftName"].ToString().Trim();
                get_info[1] = sql_obj.DaTable.Rows[0]["LineID"].ToString().Trim();
                get_info[2] = sql_obj.DaTable.Rows[0]["SubLine_ID"].ToString().Trim();
            }
            return get_info;
        }

        string[] Get_Current_Tracking(string wst_id, DateTime date)
        {
            string[] get_info = { "", "", "" };
            SQL_API.SQL_ATC sql_obj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string sql_cmd = @"SELECT distinct ShiftName, LineID, SubLine_ID
                                FROM P007_P008_Tracking
                                WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "' and WST_ID = '" + wst_id + " '";
            sql_cmd += @" and To_Time is null";
            sql_obj.GET_SQL_DATA(sql_cmd);
            if (sql_obj.DaTable.Rows.Count > 0 && sql_obj.DaTable != null)
            {
                get_info[0] = sql_obj.DaTable.Rows[0]["ShiftName"].ToString().Trim();
                get_info[1] = sql_obj.DaTable.Rows[0]["LineID"].ToString().Trim();
                get_info[2] = sql_obj.DaTable.Rows[0]["SubLine_ID"].ToString().Trim();
            }
            return get_info;
        }

        private DataTable Get_All_WST()
        {
            //string sql_cmd = @"select distinct WST_ID, WST_Name from MDB_003_Line_Desciption";
            string sql_cmd = @"select distinct LineID, WST_ID, WST_Name from MDB_003_Line_Desciption";
            SQL_API.SQL_ATC sqlObj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            sqlObj.GET_SQL_DATA(sql_cmd);
            return sqlObj.DaTable;
        }

        private bool DeleteReport_Line_Status(DateTime date)
        {
            bool result;
            string cmd = @"Delete FROM [R_009_Line_Status] ";
            //cmd += "WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            result = Update_Data_Info(MasterDatabase_Connection_Str, cmd);
            return result;
        }
    }
}
