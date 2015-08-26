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
        private bool R_006_TrackingTT_PlanTL_Get_Plan_Empl(DateTime date)
        {
            string sql_cmd;
            bool b;
            DataRow newrow;
            string mess;
            string shiftName, lineID, wst, plan_Empl_ID, plan_Empl_Name, his_Empl_ID, his_Empl_Name, sub_Line_ID;
            string[] employee_history;
            string[] employee_plan;
            int i = 0;
            int countT = 0;
            int total = 0;


            sql_cmd = String.Format("SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R_006_TrackingTT_PlanTL_Report] WHERE [Date] = '{0}'", date.ToString("yyyy-MMM-dd"));
            b = R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);

            if (b == false)
            {
                return false;
            }

            StatusLabel1.Visible = true;
            ProgressBar1.Visible = true;
            StatusLabel1.Text = "create TrackingTT_PlanTL";

            int count = R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Count;
            if (count > 0)
            {
                mess = "Plan for date:" + date.ToString("dd MMM yyyy") + "was existing\n";
                mess += "Do you want to delete and create the new one?";

                if (MessageBox.Show(mess, "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }

                DeleteReport_R_006_TrackingTT_PlanTL(date);
                R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            }

//            sql_cmd = @"SELECT [Date]
//                              ,[LineID]
//                              ,[WST_ID]
//                              ,[ShiftName]
//                              ,[Empl_ID] as Plan_Empl_ID
//                              ,[Empl_Name] as Plan_Empl_Name
//                        FROM [JOB_ASSIGNMENT_DB].[dbo].[P_003_KeHoachSanXuatTheoLine]
//                        where [Date] = '" + date.ToString("dd MMM yyyy") + @"'
//                        ORDER BY [Date], LineID";

//            SQL_API.SQL_ATC sqlobj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
//            sqlobj.GET_SQL_DATA(sql_cmd);

            // Load Danh sách Tat ca cac trạm
            //DataTable all_wst = Get_All_WST()
            // P007_Tracking_Get_Plan_Empl(date);
            // P007_Tracking_View_Init();
            // P007_Tracking_Get_Plan_Empl(date);

            Load_Tracking_date(date);

            DataTable all_wst = Load_all_WST();

            if (all_wst != null)
            {
                countT = all_wst.Rows.Count;
            }
            total = countT;

            foreach (DataRow row in all_wst.Rows)
            {
                //string d = row["Date"].ToString().Trim();
                //datetime = DateTime.Parse(d);
                //shiftName = row["ShiftName"].ToString().Trim();
                lineID = row["LineID"].ToString().Trim();
                wst = row["WST_ID"].ToString().Trim();
                sub_Line_ID = row["SubLine_ID"].ToString().Trim();

                shiftName = "Shift_1";
                employee_plan = Get_Plan_Empl(date, shiftName, lineID, wst);
                plan_Empl_ID = employee_plan[0];
                plan_Empl_Name = employee_plan[1];
                employee_history = Get_employee_History(lineID, wst, shiftName, date);
                his_Empl_ID = employee_history[0];
                his_Empl_Name = employee_history[1];

                newrow = R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.NewRow();
                newrow["Date"] = date.ToString("MM/dd/yyyy");
                newrow["ShiftName"] = shiftName;
                newrow["LineID"] = lineID;
                newrow["WST_ID"] = wst;
                newrow["His_Empl_ID"] = his_Empl_ID;
                newrow["His_Empl_Name"] = his_Empl_Name;
                newrow["Plan_Empl_ID"] = plan_Empl_ID;
                newrow["Plan_Empl_Name"] = plan_Empl_Name;
                R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Add(newrow);

                shiftName = "Shift_2";

                employee_plan = Get_Plan_Empl(date, shiftName, lineID, wst);
                plan_Empl_ID = employee_plan[0];
                plan_Empl_Name = employee_plan[1];
                employee_history = Get_employee_History(lineID, wst, shiftName, date);
                his_Empl_ID = employee_history[0];
                his_Empl_Name = employee_history[1];

                newrow = R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.NewRow();
                newrow["Date"] = date.ToString("MM/dd/yyyy");
                newrow["ShiftName"] = shiftName;
                newrow["LineID"] = lineID;
                newrow["WST_ID"] = wst;
                newrow["His_Empl_ID"] = his_Empl_ID;
                newrow["His_Empl_Name"] = his_Empl_Name;
                newrow["Plan_Empl_ID"] = plan_Empl_ID;
                newrow["Plan_Empl_Name"] = plan_Empl_Name;
                R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Add(newrow);

                shiftName = "Shift_3";
                employee_plan = Get_Plan_Empl(date, shiftName, lineID, wst);
                plan_Empl_ID = employee_plan[0];
                plan_Empl_Name = employee_plan[1];
                employee_history = Get_employee_History(lineID, wst, shiftName, date);
                his_Empl_ID = employee_history[0];
                his_Empl_Name = employee_history[1];

                newrow = R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.NewRow();
                newrow["Date"] = date.ToString("MM/dd/yyyy");
                newrow["ShiftName"] = shiftName;
                newrow["LineID"] = lineID;
                newrow["WST_ID"] = wst;
                newrow["His_Empl_ID"] = his_Empl_ID;
                newrow["His_Empl_Name"] = his_Empl_Name;
                newrow["Plan_Empl_ID"] = plan_Empl_ID;
                newrow["Plan_Empl_Name"] = plan_Empl_Name;
                R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Add(newrow);

                i++;
                ProgressBar1.Value = i * 100 / total;
            }

            Update_SQL_Data(R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_GridviewTBL.Data_da, R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb);

            StatusLabel1.Visible = false;
            ProgressBar1.Visible = false;
            return true;
        }
        
        private bool DeleteReport_R_006_TrackingTT_PlanTL(DateTime date)
        {
            bool result;
            string cmd = @"Delete FROM [R_006_TrackingTT_PlanTL_Report]
                            WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            result = Update_Data_Info(MasterDatabase_Connection_Str, cmd);
            return result;
        }

        string[] Get_employee_History(string lineID, string wst, string shift, DateTime date)
        {
            string[] empl_info = { "", "" };

            SQL_API.SQL_ATC spl_obj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string sqlcmd = @"SELECT distinct [Empl_ID] ,[Empl_Name], [From_Time] 
                              FROM [JOB_ASSIGNMENT_DB].[dbo].[P007_P008_Tracking]";
            if (shift == SHIFT_1)
            {
                sqlcmd += @" WHERE Date = '" + date.AddDays(-1).ToString("dd MMM yyyy") + "' and LineID = '" + lineID + "' and WST_ID = '" + wst + "'";
                sqlcmd += @" AND  (([ShiftName] = 'Shift_1' AND [From_Time] BETWEEN '06:00:00' and '14:00:00')";
                sqlcmd += @" OR    ([ShiftName] = 'Shift_2' AND [From_Time] BETWEEN '14:00:00' and '22:00:00')";
                sqlcmd += @" OR    ([ShiftName] = 'Shift_3' AND [From_Time] BETWEEN '22:00:00' and '23:59:59')";
                sqlcmd += @" OR    ([ShiftName] = 'Shift_3' AND [From_Time] BETWEEN '00:00:00' and '06:00:00'))";
                sqlcmd += @" AND    ([ShiftName] = '" + shift + "')";
                sqlcmd += @" ORDER by [From_Time] DESC";
            }
            else
            {
                sqlcmd += @" WHERE Date = '" + date.AddDays(-2).ToString("dd MMM yyyy") + "' and LineID = '" + lineID + "' and WST_ID = '" + wst + "'";
                sqlcmd += @" AND  (([ShiftName] = 'Shift_1' AND [From_Time] BETWEEN '06:00:00' and '14:00:00')";
                sqlcmd += @" OR    ([ShiftName] = 'Shift_2' AND [From_Time] BETWEEN '14:00:00' and '22:00:00')";
                sqlcmd += @" OR    ([ShiftName] = 'Shift_3' AND [From_Time] BETWEEN '22:00:00' and '23:59:59')";
                sqlcmd += @" OR    ([ShiftName] = 'Shift_3' AND [From_Time] BETWEEN '00:00:00' and '06:00:00'))";
                sqlcmd += @" AND    ([ShiftName] = '" + shift + "')";
                sqlcmd += @" ORDER by [From_Time] DESC";
            }


            spl_obj.GET_SQL_DATA(sqlcmd);
            
            if ((spl_obj.DaTable != null) && (spl_obj.DaTable.Rows.Count > 0))
            {
                empl_info[0] = spl_obj.DaTable.Rows[0]["Empl_ID"].ToString().Trim();
                empl_info[1] = spl_obj.DaTable.Rows[0]["Empl_Name"].ToString().Trim();
            }

            return empl_info;
        }

        void R_006_TrackingTT_PlanTL_Control_MasterDatabase_GridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            string plan_empl, his_empl_id;
            try
            {
                foreach (DataGridViewRow row in R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows)
                {
                    // kiem tra plan và history giong nhau khong
                    plan_empl = row.Cells["Plan_Empl_ID"].Value == null ? "" : row.Cells["Plan_Empl_ID"].Value.ToString().Trim();
                    his_empl_id = row.Cells["His_Empl_ID"].Value == null ? "" : row.Cells["His_Empl_ID"].Value.ToString().Trim();
                    if (his_empl_id == "")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightSeaGreen;
                    }
                    else if (plan_empl == "")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightBlue;
                    }
                    else if (plan_empl != his_empl_id)
                    {
                        row.DefaultCellStyle.BackColor = Color.Red;
                    }
                }
            }
            catch
            {

            }
        }
    }
}
