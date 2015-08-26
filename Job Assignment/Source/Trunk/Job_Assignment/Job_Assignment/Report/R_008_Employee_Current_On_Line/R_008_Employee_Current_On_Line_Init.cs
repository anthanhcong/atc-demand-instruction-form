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
        MaterDatabase R_008_Employee_Current_On_Line_MasterDatabase;
        Button_Lbl R_008_Employee_Current_On_Line_Create_BT;

        // Hiển thị tất cả các Line hiện đang chạy theo KH, các WST đang chạy, WST nào không chạy theo Kế hoạch (màu đỏ)
//        public string R_008_Employee_Current_On_Line_Select_CMD = String.Format(@"select distinct tk.ShiftName, tk.LineID, tk.Subline_ID, tk.WST_ID, tk.WST_Name, tk.Empl_ID, tk.Empl_Name, pl.Empl_ID 'Plan_Empl_ID', pl.Empl_Name 'Plan_Empl_Name' 
//                                                                                from p007_p008_tracking tk, p_003_kehoachsanxuattheoline pl 
//                                                                                where tk.to_time is null and tk.[Date] = '{0}' 
//                                                                                and pl.[date] = tk.[date] and pl.lineid = tk.lineid and pl.wst_id = tk.wst_id and pl.shiftname = tk.shiftname 
//                                                                                order by tk.lineid, tk.empl_id", DateTime.Now);

        // Hiển thị tất cả các Line hiện đang chạy, các WST đang chạy, WST nào không theo Kế hoạch (màu đỏ), WST chạy nhưng kế hoach không có (màu vàng), WST chạy nhung chưa nhập data và kế hoach không có (màu cam)
        public string R_008_Employee_Current_On_Line_Select_CMD = String.Format(@"select distinct tk.ShiftName, tk.LineID, tk.Subline_ID, tk.WST_ID, tk.WST_Name, tk.Empl_ID, tk.Empl_Name, pl.Empl_ID 'Plan_Empl_ID', pl.Empl_Name 'Plan_Empl_Name'
                                                                                from P007_p008_tracking tk full outer join p_003_kehoachsanxuattheoline pl 
                                                                                on tk.[Date] = pl.[Date] and tk.WST_ID = pl.WST_ID and tk.ShiftName = pl.ShiftName
                                                                                where tk.[Date] = '{0}' and tk.To_Time is null 
                                                                                order by tk.LineID, tk.Empl_ID", DateTime.Now);


        private bool R_008_Employee_Current_On_Line_Exist = false;
        private int R_008_Employee_Current_On_Line_Index = 11;

        ExcelImportStruct[] R_008_Employee_Current_On_Line_Excel_Struct;
        const int R_008_Employee_Current_On_Line_INDEX = 0;

        private bool R_008_Employee_Current_On_Line_Init()
        {
            if (R_008_Employee_Current_On_Line_Exist == true)
            {
                if (tabControl1.TabPages.Contains(R_008_Employee_Current_On_Line_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, R_008_Employee_Current_On_Line_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("R_008_Employee_Current_On_Line");
                return true;
            }
            R_008_Employee_Current_On_Line_Exist = true;

            R_008_Employee_Current_On_Line_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "R_008_Employee_Current_On_Line", R_008_Employee_Current_On_Line_Index, MasterDatabase_Connection_Str,
                                                            R_008_Employee_Current_On_Line_Select_CMD, R_008_Employee_Current_On_Line_Select_CMD,
                                                            3, R_008_Employee_Current_On_Line_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            R_008_Employee_Current_On_Line_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            R_008_Employee_Current_On_Line_MasterDatabase.MasterDatabase_GridviewTBL.GridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(R_008_Employee_Current_On_Line_MasterDatabase_GridView_DataBindingComplete);
            R_008_Employee_Current_On_Line_MasterDatabase.MasterDatabase_GridviewTBL.Privot_BT.Visible = true;
            R_008_Employee_Current_On_Line_MasterDatabase.MasterDatabase_GridviewTBL.Delete_All_BT.Visible = false;
            R_008_Employee_Current_On_Line_MasterDatabase.MasterDatabase_GridviewTBL.Submit_BT.Visible = false;
            R_008_Employee_Current_On_Line_MasterDatabase.MasterDatabase_GridviewTBL.Import_BT.Visible = false;
            R_008_Employee_Current_On_Line_MasterDatabase.MasterDatabase_GridviewTBL.Export_BT.Visible = false;
            
            return true;
        }

        void R_008_Employee_Current_On_Line_MasterDatabase_GridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            string plan_empl, tracking_empl;
            foreach (DataGridViewRow row in R_008_Employee_Current_On_Line_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows)
            {
                plan_empl = row.Cells["Plan_Empl_ID"].Value == null ? "" : row.Cells["Plan_Empl_ID"].Value.ToString().Trim();
                tracking_empl = row.Cells["Empl_ID"].Value == null ? "" : row.Cells["Empl_ID"].Value.ToString().Trim();
                if (plan_empl == "")
                {
                    if (tracking_empl != "")
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.Orange;
                    }
                }
                else
                {
                    if (tracking_empl == "")
                    {
                        row.DefaultCellStyle.BackColor = Color.Red;
                    }
                }
            }
        }
    }
}
