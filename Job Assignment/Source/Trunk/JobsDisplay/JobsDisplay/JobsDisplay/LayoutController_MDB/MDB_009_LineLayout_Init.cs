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

namespace JobsDisplay
{
    public partial class Form1 : SQL_APPL
    {
        MaterDatabase LineLayout_MasterDatabase;

        public string LineLayout_Select_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_009_LayoutControl] ";
        public string LineLayout_Init_Database_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_009_LayoutControl] 
                                                      WHERE [Line_ID] = ''";
        private bool LineLayout_Exist = false;
		private int LineLayout_Index = 9;

        private bool LineLayout_Init()
        {
            if (LineLayout_Exist == true)
            {
                tabControl1.SelectTab("LineLayout");
                return true;
            }
            LineLayout_Exist = true;

            Init_LineLayout_Excel();
            LineLayout_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "LineLayout", LineLayout_Index, MasterDatabase_Connection_Str, 
                                                            LineLayout_Init_Database_CMD, LineLayout_Select_CMD,
                                                            3, LineLayout_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            LineLayout_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            return true;
        }
        ExcelImportStruct[] LineLayout_Excel_Struct;
        const int LineLayout_INDEX = 0;

        private void Init_LineLayout_Excel()
        {
            if (LineLayout_Excel_Struct == null)
            {
                LineLayout_Excel_Struct = new ExcelImportStruct[11];
                LineLayout_Excel_Struct[0] = new ExcelImportStruct(0, "Line_ID", "Line_ID", Excel_Col_Type.COL_STRING, 20, true);
                LineLayout_Excel_Struct[1] = new ExcelImportStruct(1, "Line_Name", "Line_Name", Excel_Col_Type.COL_STRING, 50, false);
                LineLayout_Excel_Struct[2] = new ExcelImportStruct(2, "WST_ID", "WST_ID", Excel_Col_Type.COL_STRING, 20, true);
                LineLayout_Excel_Struct[3] = new ExcelImportStruct(3, "WST_Name", "WST_Name", Excel_Col_Type.COL_STRING, 50, false);
                LineLayout_Excel_Struct[4] = new ExcelImportStruct(4, "WST_x", "WST_x", Excel_Col_Type.COL_INT, 20, false);
                LineLayout_Excel_Struct[5] = new ExcelImportStruct(5, "WST_y", "WST_y", Excel_Col_Type.COL_INT, 20, false);
                LineLayout_Excel_Struct[6] = new ExcelImportStruct(6, "WST_width", "WST_width", Excel_Col_Type.COL_INT, 20, false);
                LineLayout_Excel_Struct[7] = new ExcelImportStruct(7, "WST_heigh", "WST_heigh", Excel_Col_Type.COL_INT, 20, false);
                LineLayout_Excel_Struct[8] = new ExcelImportStruct(8, "GroupID", "GroupID", Excel_Col_Type.COL_STRING, 20, false);
                LineLayout_Excel_Struct[9] = new ExcelImportStruct(9, "Description", "Description", Excel_Col_Type.COL_STRING, 200, false);
                LineLayout_Excel_Struct[10] = new ExcelImportStruct(10, "Note", "Note", Excel_Col_Type.COL_STRING, 200, false);
            }
        }

    }
}