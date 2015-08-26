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
        MaterDatabase SpecialLine_MasterDatabase;

        public string SpecialLine_Select_CMD = @"SELECT * FROM [MDB_008_Special_Line] ";
        public string SpecialLine_Init_Database_CMD = @"SELECT * FROM [MDB_008_Special_Line] 
                                                      WHERE [LineID] = ''";
        private bool SpecialLine_Exist = false;
		private int SpecialLine_Index = 7;

        private bool SpecialLine_Init()
        {
            if (SpecialLine_Exist == true)
            {
                if (tabControl1.TabPages.Contains(SpecialLine_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, SpecialLine_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("SpecialLine");
                return true;
            }
            SpecialLine_Exist = true;

            Init_SpecialLine_Excel();
            SpecialLine_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "SpecialLine", SpecialLine_Index, MasterDatabase_Connection_Str, 
                                                            SpecialLine_Init_Database_CMD, SpecialLine_Select_CMD,
                                                            3, SpecialLine_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            SpecialLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            return true;
        }
        ExcelImportStruct[] SpecialLine_Excel_Struct;
        const int SpecialLine_INDEX = 0;

        private void Init_SpecialLine_Excel()
        {
            if (SpecialLine_Excel_Struct == null)
            {
                SpecialLine_Excel_Struct = new ExcelImportStruct[3];
                SpecialLine_Excel_Struct[0] = new ExcelImportStruct(0, "LineID", "LineID", Excel_Col_Type.COL_STRING, 20, true);
                SpecialLine_Excel_Struct[1] = new ExcelImportStruct(1, "LineName", "LineName", Excel_Col_Type.COL_STRING, 50, false);
                SpecialLine_Excel_Struct[2] = new ExcelImportStruct(2, "Mode", "Mode", Excel_Col_Type.COL_STRING, 20, false);
            }
        }

    }
}