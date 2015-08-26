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
        MaterDatabase FixPosition_MasterDatabase;

        public string FixPosition_Select_CMD = @"SELECT * FROM [MDB_007_Fix_Position] ";
        public string FixPosition_Init_Database_CMD = @"SELECT * FROM [MDB_007_Fix_Position] 
                                                      WHERE [LineID] = ''";
        private bool FixPosition_Exist = false;
		private int FixPosition_Index = 7;

        private bool FixPosition_Init()
        {
            if (FixPosition_Exist == true)
            {
                if (tabControl1.TabPages.Contains(FixPosition_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, FixPosition_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("FixPosition");
                return true;
            }
            FixPosition_Exist = true;

            Init_FixPosition_Excel();
            FixPosition_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "FixPosition", FixPosition_Index, MasterDatabase_Connection_Str, 
                                                            FixPosition_Init_Database_CMD, FixPosition_Select_CMD,
                                                            3, FixPosition_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            FixPosition_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            return true;
        }
        ExcelImportStruct[] FixPosition_Excel_Struct;
        const int FixPosition_INDEX = 0;

        private void Init_FixPosition_Excel()
        {
            if (FixPosition_Excel_Struct == null)
            {
                FixPosition_Excel_Struct = new ExcelImportStruct[6];
                FixPosition_Excel_Struct[0] = new ExcelImportStruct(0, "Empl_ID", "Empl_ID", Excel_Col_Type.COL_STRING, 20, true);
                FixPosition_Excel_Struct[1] = new ExcelImportStruct(1, "Empl_Name", "Empl_Name", Excel_Col_Type.COL_STRING, 50, false);
                FixPosition_Excel_Struct[2] = new ExcelImportStruct(2, "LineID", "LineID", Excel_Col_Type.COL_STRING, 20, false);
                FixPosition_Excel_Struct[3] = new ExcelImportStruct(3, "LineName", "LineName", Excel_Col_Type.COL_STRING, 50, false);
                FixPosition_Excel_Struct[4] = new ExcelImportStruct(4, "WST_ID", "WST_ID", Excel_Col_Type.COL_STRING, 20, false);
                FixPosition_Excel_Struct[5] = new ExcelImportStruct(5, "WST_Name", "WST_Name", Excel_Col_Type.COL_STRING, 50, false);
                FixPosition_Excel_Struct[5] = new ExcelImportStruct(6, "Position", "Position", Excel_Col_Type.COL_STRING, 10, false);
            }
        }

    }
}