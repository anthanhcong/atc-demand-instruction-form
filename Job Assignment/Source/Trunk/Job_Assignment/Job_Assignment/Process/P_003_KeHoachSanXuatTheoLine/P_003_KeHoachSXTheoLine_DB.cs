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
using System.Threading;

namespace Job_Assignment
{
    public partial class Form1 : SQL_APPL
    {
        private bool DeletePlanforLine(DateTime date)
        {
            bool result;
            string cmd;
            if (Running_Mode == Run_Mode.RELEASE)
            {
                cmd = @"Delete FROM [P_003_KeHoachSanXuatTheoLine] 
                            WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            }
            else
            {
                cmd = @"Delete FROM [P_003_KeHoachSanXuatTheoLine_Test] 
                            WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            }
            result = Update_Data_Info(MasterDatabase_Connection_Str, cmd);
            return result;
        }

        public DataTable PlanDate_Temp_Tbl = new DataTable();
        public DataSet PlanDate_Temp_ds = new DataSet();
        public SqlDataAdapter PlanDate_Temp_da;

        DataTable Get_Main_Part_Shift_1(DateTime date, string subline)
        {
            string cmd = @"Select * FROM [P_002_PlanForProductionByDate] 
                            WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            cmd += " AND [SubLine_ID] = '" + subline + "' AND [Shift_1_Main] = 'true'";

            if (PlanDate_Temp_Tbl != null)
            {
                PlanDate_Temp_Tbl.Clear();
            }
            PlanDate_Temp_Tbl = Get_SQL_Data(MasterDatabase_Connection_Str, cmd, ref PlanDate_Temp_da, ref PlanDate_Temp_ds);
            return PlanDate_Temp_Tbl;
        }

        DataTable Get_Main_Part_Shift_2(DateTime date, string subline)
        {
            string cmd = @"Select * FROM [P_002_PlanForProductionByDate] 
                            WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            cmd += " AND [SubLine_ID] = '" + subline + "' AND [Shift_2_Main] = 'true'";

            if (PlanDate_Temp_Tbl != null)
            {
                PlanDate_Temp_Tbl.Clear();
            }
            PlanDate_Temp_Tbl = Get_SQL_Data(MasterDatabase_Connection_Str, cmd, ref PlanDate_Temp_da, ref PlanDate_Temp_ds);
            return PlanDate_Temp_Tbl;
        }

        TimeSpan Get_OutTime_Shift_1(DateTime date, string subline)
        {
            string outtime_str;
            TimeSpan out_time;
            string cmd = @"Select MAX([Shift_1_To]) FROM [P_002_PlanForProductionByDate] 
                            WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            cmd += " AND [SubLine_ID] = '" + subline + "'";

            if (PlanDate_Temp_Tbl != null)
            {
                PlanDate_Temp_Tbl.Clear();
            }
            PlanDate_Temp_Tbl = Get_SQL_Data(MasterDatabase_Connection_Str, cmd, ref PlanDate_Temp_da, ref PlanDate_Temp_ds);

            if ((PlanDate_Temp_Tbl != null) && (PlanDate_Temp_Tbl.Rows.Count > 0))
            {
                outtime_str = PlanDate_Temp_Tbl.Rows[0]["Column1"].ToString().Trim();
                out_time = TimeSpan.Parse(outtime_str);
            }
            else
            {
                out_time = TimeSpan.Parse("0:00:00");
            }
            return out_time;
        }

        TimeSpan Get_OutTime_Shift_2(DateTime date, string subline)
        {
            string outtime_str;
            TimeSpan out_time;
            string cmd = @"Select MAX([Shift_2_To]) FROM [P_002_PlanForProductionByDate] 
                            WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            cmd += " AND [SubLine_ID] = '" + subline + "'";

            if (PlanDate_Temp_Tbl != null)
            {
                PlanDate_Temp_Tbl.Clear();
            }
            PlanDate_Temp_Tbl = Get_SQL_Data(MasterDatabase_Connection_Str, cmd, ref PlanDate_Temp_da, ref PlanDate_Temp_ds);

            if ((PlanDate_Temp_Tbl != null) && (PlanDate_Temp_Tbl.Rows.Count > 0))
            {
                outtime_str = PlanDate_Temp_Tbl.Rows[0]["Column1"].ToString().Trim();
                out_time = TimeSpan.Parse(outtime_str);
            }
            else
            {
                out_time = TimeSpan.Parse("0:00:00");
            }
            return out_time;
        }

        TimeSpan Get_OutTime_Shift_3(DateTime date, string subline)
        {
            string outtime_str;
            TimeSpan out_time;
            string cmd = @"Select MAX([Shift_3_To]) FROM [P_002_PlanForProductionByDate] 
                            WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            cmd += " AND [Shift_3_To] < '20:00:00'";               
            cmd += " AND [SubLine_ID] = '" + subline + "'";

            if (PlanDate_Temp_Tbl != null)
            {
                PlanDate_Temp_Tbl.Clear();
            }
            PlanDate_Temp_Tbl = Get_SQL_Data(MasterDatabase_Connection_Str, cmd, ref PlanDate_Temp_da, ref PlanDate_Temp_ds);

            if ((PlanDate_Temp_Tbl != null) && (PlanDate_Temp_Tbl.Rows.Count > 0))
            {
                outtime_str = PlanDate_Temp_Tbl.Rows[0]["Column1"].ToString().Trim();
                if (outtime_str != "")
                {
                    out_time = TimeSpan.Parse(outtime_str);
                }
                else
                {
                    out_time = TimeSpan.Parse("0:00:00");
                }
            }
            else
            {
                out_time = TimeSpan.Parse("0:00:00");
            }
            return out_time;
        }

        TimeSpan Get_InTime_Shift_3(DateTime date, string subline)
        {
            string outtime_str;
            TimeSpan out_time;
            string cmd = @"Select MIN([Shift_3_From]) FROM [P_002_PlanForProductionByDate] 
                            WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            cmd += " AND [SubLine_ID] = '" + subline + "'";
            cmd += " AND [Shift_3_From] > '12:00:00'";

            if (PlanDate_Temp_Tbl != null)
            {
                PlanDate_Temp_Tbl.Clear();
            }
            PlanDate_Temp_Tbl = Get_SQL_Data(MasterDatabase_Connection_Str, cmd, ref PlanDate_Temp_da, ref PlanDate_Temp_ds);

            if ((PlanDate_Temp_Tbl != null) && (PlanDate_Temp_Tbl.Rows.Count > 0))
            {
                outtime_str = PlanDate_Temp_Tbl.Rows[0]["Column1"].ToString().Trim();
                out_time = TimeSpan.Parse(outtime_str);
            }
            else
            {
                out_time = TimeSpan.Parse("0:00:00");
            }
            return out_time;
        }

        DataTable Get_Main_Part_Shift_3(DateTime date, string subline)
        {
            string cmd = @"Select * FROM [P_002_PlanForProductionByDate] 
                            WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            cmd += " AND [SubLine_ID] = '" + subline + "' AND [Shift_3_Main] = 'true'";

            if (PlanDate_Temp_Tbl != null)
            {
                PlanDate_Temp_Tbl.Clear();
            }
            PlanDate_Temp_Tbl = Get_SQL_Data(MasterDatabase_Connection_Str, cmd, ref PlanDate_Temp_da, ref PlanDate_Temp_ds);
            return PlanDate_Temp_Tbl;
        }

        DataTable Get_List_wst(string part)
        {
            string cmd = @"Select * FROM [MDB_003_Line_Desciption] 
                            WHERE [PartNumber] = '" + part + "'";
            if (PlanDate_Temp_Tbl != null)
            {
                PlanDate_Temp_Tbl.Clear();
            }
            PlanDate_Temp_Tbl = Get_SQL_Data(MasterDatabase_Connection_Str, cmd, ref PlanDate_Temp_da, ref PlanDate_Temp_ds);
            return PlanDate_Temp_Tbl;
        }

        bool Create_Update_PlanLine(DateTime date, string line_id, string line_name, string subline, string subline_name, string shift, string part, string wst, string wst_name, TimeSpan from_time, TimeSpan to_time)
        {
            DateTime cur_date;
            string cur_date_str, cur_line, cur_shift, cur_wst;
            bool exist = false;

            if (P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Columns.Contains("GroupID") == false)
            {
                P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Columns.Add("GroupID", typeof(string));
            }

            foreach (DataRow row in P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows)
            {
                cur_date_str = row["Date"].ToString().Trim();
                cur_date = DateTime.Parse(cur_date_str);
                cur_line = row["SubLine_ID"].ToString().Trim();
                cur_shift = row["ShiftName"].ToString().Trim();
                cur_wst = row["WST_ID"].ToString().Trim();

                // GroupID 
                if ((cur_date.Date == date.Date) && (cur_line == subline)
                    & (cur_shift == shift) && (cur_wst == wst))
                {
                    row["SubLine_Name"] = subline_name;
                    row["LineID"] = line_id;
                    row["LineName"] = line_name;
                    row["WST_Name"] = wst_name;
                    row["From_Time"] = from_time;
                    row["To_Time"] = to_time;
                    row["Main_Part"] = part;
                    row["GroupID"] = GetGroupID(line_id);
                    exist = true;
                }
            }

            if (exist == false)
            {
                DataRow new_row = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.NewRow();
                new_row["Date"] = date.ToString("dd MMM yyyy");
                new_row["SubLine_ID"] = subline;
                new_row["SubLine_Name"] = subline_name;
                new_row["LineID"] = line_id;
                new_row["LineName"] = line_name;
                new_row["ShiftName"] = shift;
                new_row["WST_ID"] = wst;
                new_row["WST_Name"] = wst_name;
                new_row["From_Time"] = from_time;
                new_row["To_Time"] = to_time;
                new_row["Main_Part"] = part;
                new_row["GroupID"] = GetGroupID(line_id);
                P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Add(new_row);
            }
            // P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Save_Data();

            Update_SQL_Data(P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_da, P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb);
            return true;
        }

        DataTable Get_PlanByWST()
        {
            //string cmd = @"Select * FROM [P_003_KeHoachSanXuatTheoLine]";

            //if (PlanDate_Temp_Tbl != null)
            //{
            //    PlanDate_Temp_Tbl.Clear();
            //}
            //PlanDate_Temp_Tbl = Get_SQL_Data(MasterDatabase_Connection_Str, cmd, ref PlanDate_Temp_da, ref PlanDate_Temp_ds);
            //return PlanDate_Temp_Tbl;
            return P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb;
        }

        DataTable Get_Lines_Plan_All_Part(DateTime date)
        {
            string sql_cmd_2 = @"SELECT * FROM [P_002_PlanForProductionByDate] ";
            DataTable temp_tbl = GetSqlData(sql_cmd_2);
            return temp_tbl;
        }

        DataTable GetFruPlan(DateTime date)
        {
            SQL_API.SQL_ATC sql_obj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string cmd = @"Select * FROM [P_003_KeHoachSanXuatTheoLine] 
                            WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            cmd += " AND LineID = 'FRU'";
            sql_obj.GET_SQL_DATA(cmd);
            return sql_obj.DaTable;
        }

        public bool AssignEmpl_in_Thread(DateTime date)
        {
            // Thread SendMail_Thread = new Thread(new ThreadStart(Send_Mail_Exchange(mail_address, title, body, cc_to_manager)));
            int value = 0;
            Thread SendMail_Thread = new Thread(() => Assign_Empl_for_LinePlan(date));
            SendMail_Thread.SetApartmentState(ApartmentState.STA);
            SendMail_Thread.Start();
            while (SendMail_Thread.IsAlive == true)
            {
                Thread.Sleep(200);
                StatusLabel1.Visible = true;
                StatusLabel1.Text = "Sending email...";
                ProgressBar1.Visible = true;
                ProgressBar1.Value = value % 100;
                value++;
                Application.DoEvents();
            }
            ProgressBar1.Visible = false;
            StatusLabel1.Visible = false;
            return false;
        }
    }
}