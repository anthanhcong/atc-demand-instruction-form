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
        MaterDatabase KeHoachSanXuatTheoTramList_MasterDatabase;
        //Button_Lbl Check_BT;
        //Dho-Fixme: Do we need to use the button "Check_BT"?

        public string KeHoachSanXuatTheoTramList_Select_CMD = @"SELECT * FROM [P_004_KeHoachSanXuatTheoTram] ";
        public string KeHoachSanXuatTheoTramList_Init_Database_CMD = @"SELECT * FROM [P_004_KeHoachSanXuatTheoTram] 
                                                      WHERE [Date] = ''";
        private bool KeHoachSanXuatTheoTramList_Exist = false;

        private bool KeHoachSanXuatTheoTramList_Init()
        {
            if (KeHoachSanXuatTheoTramList_Exist == true)
            {
                if (tabControl1.TabPages.Contains(KeHoachSanXuatTheoTramList_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, KeHoachSanXuatTheoTramList_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("KeHoachSanXuatTheoTram");
                return true;
            }
            KeHoachSanXuatTheoTramList_Exist = true;
            Init_KeHoachSanXuatTheoTram_Excel();
            KeHoachSanXuatTheoTramList_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "KeHoachSanXuatTheoTram", ProductionPlanByWorkStation_Index, MasterDatabase_Connection_Str, 
                                                            KeHoachSanXuatTheoTramList_Init_Database_CMD, KeHoachSanXuatTheoTramList_Select_CMD,
                                                            3, KeHoachSanXuatTheoTram_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);
            
            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            KeHoachSanXuatTheoTramList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            PosSize possize = new PosSize();
            possize.pos_x = 6;
            possize.pos_y = 6;
            
            //Dho-Fixme: Do we need to use the button "Check_BT"?
            //Check_BT = new Button_Lbl(1, KeHoachSanXuatTheoTramList_MasterDatabase.MasterDatabase_Tab, "Check", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            return true;
        }
        ExcelImportStruct[] KeHoachSanXuatTheoTram_Excel_Struct;
        const int KeHoachSanXuatTheoTram_INDEX = 0;

        private void Init_KeHoachSanXuatTheoTram_Excel()
        {
            if (KeHoachSanXuatTheoTram_Excel_Struct == null)
            {
                KeHoachSanXuatTheoTram_Excel_Struct = new ExcelImportStruct[15];
                KeHoachSanXuatTheoTram_Excel_Struct[1] = new ExcelImportStruct(1, "Date", "Date", Excel_Col_Type.COL_DATE, 20, true);
                KeHoachSanXuatTheoTram_Excel_Struct[0] = new ExcelImportStruct(0, "PartNumber", "PartNumber", Excel_Col_Type.COL_STRING, 20, true);
                KeHoachSanXuatTheoTram_Excel_Struct[2] = new ExcelImportStruct(2, "LineID", "LineID", Excel_Col_Type.COL_STRING, 20, true);
                KeHoachSanXuatTheoTram_Excel_Struct[3] = new ExcelImportStruct(3, "LineName", "LineName", Excel_Col_Type.COL_STRING, 50, false);
                KeHoachSanXuatTheoTram_Excel_Struct[4] = new ExcelImportStruct(4, "WST_ID", "WST_ID", Excel_Col_Type.COL_STRING, 20, true);
                KeHoachSanXuatTheoTram_Excel_Struct[5] = new ExcelImportStruct(5, "WST_Name", "WST_Name", Excel_Col_Type.COL_STRING, 50, false);
                KeHoachSanXuatTheoTram_Excel_Struct[6] = new ExcelImportStruct(6, "ShiftName", "ShiftName", Excel_Col_Type.COL_STRING, 50, true);
                KeHoachSanXuatTheoTram_Excel_Struct[7] = new ExcelImportStruct(7, "Shift_Percent", "Shift_Percent", Excel_Col_Type.COL_INT, 20, false);
                KeHoachSanXuatTheoTram_Excel_Struct[8] = new ExcelImportStruct(8, "Empl_ID", "Empl_ID", Excel_Col_Type.COL_STRING, 20, false);
                KeHoachSanXuatTheoTram_Excel_Struct[9] = new ExcelImportStruct(9, "Empl_Name", "Empl_Name", Excel_Col_Type.COL_STRING, 50, false);
                KeHoachSanXuatTheoTram_Excel_Struct[10] = new ExcelImportStruct(10, "From_Time", "From_Time", Excel_Col_Type.COL_DATE, 20, false);
                KeHoachSanXuatTheoTram_Excel_Struct[11] = new ExcelImportStruct(11, "To_Time", "To_Time", Excel_Col_Type.COL_DATE, 20, false);
                KeHoachSanXuatTheoTram_Excel_Struct[12] = new ExcelImportStruct(12, "Capacity", "Capacity", Excel_Col_Type.COL_INT, 20, false);
                KeHoachSanXuatTheoTram_Excel_Struct[13] = new ExcelImportStruct(13, "Qty", "Qty", Excel_Col_Type.COL_INT, 20, false);
                KeHoachSanXuatTheoTram_Excel_Struct[14] = new ExcelImportStruct(14, "Total_Qty", "Total_Qty", Excel_Col_Type.COL_INT, 20, false);
            }
        }
    }
}