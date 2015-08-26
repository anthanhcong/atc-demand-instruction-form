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
        private bool R004_TrackingKHTT_Get_Plan_Empl(DateTime date)
        {
            string sql_cmd;
            bool b;
            string shift, line, subline, wst_id;
            string []planEmpl, tracking_empl;
            DataRow newrow;
            string mess; 
            int i = 0;
            int countT = 0;
            int total= 0;

            sql_cmd = String.Format("SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R001_Employee_AssignReport] WHERE [Date] = '{0}'", date.ToString("yyyy-MMM-dd"));
            b = R004_TrackingKHTT_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);

            if (b == false)
            {
                return false;
            }

            StatusLabel1.Visible = true;
            ProgressBar1.Visible = true;
            StatusLabel1.Text = "create TrackingKHTT";

            // Neu da cos du lieu: hoi co Xoa de taoj lai khong. neu yes thi xoa taoj lai. No thi reload len thoi
            int count = R004_TrackingKHTT_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Count;
            if (count > 0)
            {
                mess = "Plan for date:" + date.ToString("dd MMM yyyy") + "was existing\n";
                mess += "Do you want to delete and create the new one?";

                if (MessageBox.Show(mess, "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }
                //TODO: Add Funtion delete existing data in P_001_InputFromPlanner by date
                DeleteReport_TrackingPlan(date);        //Hàm này viết ở chỗ khác khong viết ở đây
                R004_TrackingKHTT_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            }
            // neu yes
            // 1. Load danh sach tat ca cac WST
            
            if (true)
            {
                DataTable all_wst_list = Load_all_WST();
                Load_Tracking_date(date);
                if (all_wst_list != null)
                {
                    countT = all_wst_list.Rows.Count;
                }
                total = countT;

                foreach (DataRow row in all_wst_list.Rows)
                {
                    line = row["LineID"].ToString().Trim();
                    subline = row["SubLine_ID"].ToString().Trim();
                    wst_id = row["WST_ID"].ToString().Trim();

                    shift = SHIFT_1;
                    planEmpl = Get_Plan_Empl(date, shift, line, wst_id);
                    tracking_empl = Get_Tracking_Empl(date, shift, line, wst_id);

                    newrow = R004_TrackingKHTT_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.NewRow();
                    newrow["Date"] = date.ToString("MM/dd/yyyy");
                    newrow["ShiftName"] = shift;
                    newrow["LineID"] = line;
                    newrow["SubLine_ID"] = subline;
                    newrow["WST_ID"] = wst_id;
                    newrow["Plan_Empl_ID"] = planEmpl[0];
                    newrow["Plan_Empl_Name"] = planEmpl[1];
                    newrow["Empl_ID"] = tracking_empl[0];
                    newrow["Empl_Name"] = tracking_empl[1];
                    R004_TrackingKHTT_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Add(newrow);

                    shift = SHIFT_2;
                    planEmpl = Get_Plan_Empl(date, shift, line, wst_id);
                    tracking_empl = Get_Tracking_Empl(date, shift, line, wst_id);
                    newrow = R004_TrackingKHTT_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.NewRow();
                    newrow["Date"] = date.ToString("MM/dd/yyyy");
                    newrow["ShiftName"] = shift;
                    newrow["LineID"] = line;
                    newrow["SubLine_ID"] = subline;
                    newrow["WST_ID"] = wst_id;
                    newrow["Plan_Empl_ID"] = planEmpl[0];
                    newrow["Plan_Empl_Name"] = planEmpl[1];
                    newrow["Empl_ID"] = tracking_empl[0];
                    newrow["Empl_Name"] = tracking_empl[1];
                    R004_TrackingKHTT_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Add(newrow);

                    shift = SHIFT_3;
                    planEmpl = Get_Plan_Empl(date, shift, line, wst_id);
                    tracking_empl = Get_Tracking_Empl(date, shift, line, wst_id);
                    newrow = R004_TrackingKHTT_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.NewRow();
                    newrow["Date"] = date.ToString("MM/dd/yyyy");
                    newrow["ShiftName"] = shift;
                    newrow["LineID"] = line;
                    newrow["SubLine_ID"] = subline;
                    newrow["WST_ID"] = wst_id;
                    newrow["Plan_Empl_ID"] = planEmpl[0];
                    newrow["Plan_Empl_Name"] = planEmpl[1];
                    newrow["Empl_ID"] = tracking_empl[0];
                    newrow["Empl_Name"] = tracking_empl[1];
                    R004_TrackingKHTT_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Add(newrow);

                    i++;
                    ProgressBar1.Value = i * 100 / total;
                }

                Update_SQL_Data(R004_TrackingKHTT_MasterDatabase.MasterDatabase_GridviewTBL.Data_da, R004_TrackingKHTT_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb);
            }

            StatusLabel1.Visible = false;
            ProgressBar1.Visible = false;
            return true;
        }

        private bool DeleteReport_TrackingPlan(DateTime date)
        {
            bool result;
            string cmd = @"Delete FROM [R001_Employee_AssignReport]
                            WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            result = Update_Data_Info(MasterDatabase_Connection_Str, cmd);
            return result;
        }

        DataTable Load_all_WST()
        {
            string sql_cmd = @"SELECT Distinct [WST_ID]
                                  ,[LineID]
                                  ,[SubLine_ID]
                                  ,[GroupID]
                              FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_004_LineSkillRequest] ";
            SQL_API.SQL_ATC sqlobj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            sqlobj.GET_SQL_DATA(sql_cmd);
            return sqlobj.DaTable;
        }
        string []Get_Tracking_Empl(DateTime date, string shift, string line, string wst_id)
        {
            string []empl_info = {"", ""};

            SQL_API.SQL_ATC spl_obj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string sqlcmd = @"SELECT distinct
                                  [Date]
                                  ,[ShiftName]
                                  ,[LineID]
                                  ,[LineName]
                                  ,[SubLine_ID]
                                  ,[SubLine_Name]
                                  ,[WST_ID]
                                  ,[WST_Name]
                                  ,[Empl_ID]
                                  ,[Empl_Name]
                                  ,[From_Time]
                                  ,[Out_Manual]
                                  ,[To_Time]
                                  ,[WorkingTime]
                              FROM [JOB_ASSIGNMENT_DB].[dbo].[P007_P008_Tracking]";
            sqlcmd += @" WHERE Date = '" + date.ToString("dd MMM yyyy") + "' and ShiftName = '" + shift + "' and LineID = '" + line + "' and WST_ID = '" + wst_id + "'";
            //sqlcmd += @" WHERE Date = '" + date.ToString("dd MMM yyyy") + "' and LineID = '" + line + "' and WST_ID = '" + wst_id + "'";
            sqlcmd += @" AND  (([ShiftName] = 'Shift_1' AND [From_Time] BETWEEN '06:00:00' and '14:00:00')";
            sqlcmd += @" OR    ([ShiftName] = 'Shift_2' AND [From_Time] BETWEEN '14:00:00' and '22:00:00')";
            sqlcmd += @" OR    ([ShiftName] = 'Shift_3' AND [From_Time] BETWEEN '22:00:00' and '23:59:59')";
            sqlcmd += @" OR    ([ShiftName] = 'Shift_3' AND [From_Time] BETWEEN '00:00:00' and '06:00:00'))";
            sqlcmd += @" ORDER by [From_Time] DESC";
            
            spl_obj.GET_SQL_DATA(sqlcmd);
            DataTable d = spl_obj.DaTable;     
            // cái này chưa đúng
            if ((spl_obj.DaTable != null) && (spl_obj.DaTable.Rows.Count > 0))
            {
                empl_info[0] = spl_obj.DaTable.Rows[0]["Empl_ID"].ToString().Trim();
                empl_info[1] = spl_obj.DaTable.Rows[0]["Empl_Name"].ToString().Trim();
            }
           
            return empl_info;
        }

        void R_004_Tracking_Control_MasterDatabase_GridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //string plan_empl, tracking_empl;
            //foreach (DataGridViewRow row in R004_TrackingKHTT_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows)
            //{
            //    // kiem tra plan và tracking giong nhau khong
            //    plan_empl = row.Cells["Plan_Empl_ID"].Value == null ? "" : row.Cells["Plan_Empl_ID"].Value.ToString().Trim();
            //    tracking_empl = row.Cells["Empl_ID"].Value == null ? "" : row.Cells["Empl_ID"].Value.ToString().Trim();
            //    if (plan_empl == "")
            //    {
            //        row.DefaultCellStyle.BackColor = Color.LightSeaGreen;
            //    }
            //    else if (tracking_empl == "")
            //    {
            //        row.DefaultCellStyle.BackColor = Color.LightBlue;
            //    }
            //    else if (plan_empl != tracking_empl)
            //    {
            //        row.DefaultCellStyle.BackColor = Color.Red;
            //    }
            //}
        }
    }
}