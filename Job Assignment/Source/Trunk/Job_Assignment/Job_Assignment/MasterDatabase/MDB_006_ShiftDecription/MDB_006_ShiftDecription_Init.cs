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
        MaterDatabase ShiftDescription_MasterDatabase;

        public string ShiftDescription_Select_CMD = @"SELECT * FROM [MDB_006_Shift_Description] ";
        public string ShiftDescription_Init_Database_CMD = @"SELECT * FROM [MDB_006_Shift_Description] 
                                                      WHERE [ShiftName] = ''";
        private bool ShiftDescription_Exist = false;
        private int ShiftDescription_Index = 1;

        private bool ShiftDescription_Init()
        {
            if (ShiftDescription_Exist == true)
            {
                if (tabControl1.TabPages.Contains(ShiftDescription_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, ShiftDescription_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("ShiftDescription");
                return true;
            }
            ShiftDescription_Exist = true;

            Init_ShiftDescription_Excel();
            ShiftDescription_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "ShiftDescription", ShiftDescription_Index, MasterDatabase_Connection_Str, 
                                                            ShiftDescription_Init_Database_CMD, ShiftDescription_Select_CMD,
                                                            3, ShiftDescription_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            ShiftDescription_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            return true;
        }
        ExcelImportStruct[] ShiftDescription_Excel_Struct;
        const int ShiftDescription_INDEX = 0;

        private void Init_ShiftDescription_Excel()
        {
            if (ShiftDescription_Excel_Struct == null)
            {
                ShiftDescription_Excel_Struct = new ExcelImportStruct[4];
                ShiftDescription_Excel_Struct[0] = new ExcelImportStruct(0, "ShiftName", "ShiftName", Excel_Col_Type.COL_STRING, 20, true);
                ShiftDescription_Excel_Struct[1] = new ExcelImportStruct(1, "From_Time", "From_Time", Excel_Col_Type.COL_TIME, 50, false);
                ShiftDescription_Excel_Struct[2] = new ExcelImportStruct(2, "To_Time", "To_Time", Excel_Col_Type.COL_TIME, 20, false);
                ShiftDescription_Excel_Struct[3] = new ExcelImportStruct(3, "SpanTime", "SpanTime", Excel_Col_Type.COL_INT, 50, false);
            }
        }

    }
}