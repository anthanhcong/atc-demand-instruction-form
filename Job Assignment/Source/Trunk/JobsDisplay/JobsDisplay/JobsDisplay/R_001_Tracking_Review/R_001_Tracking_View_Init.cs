﻿using System;
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
        MaterDatabase Tracking_View_MasterDatabase;

        public string Tracking_View_Select_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[P007_P008_Tracking] ";
        public string Tracking_View_Init_Database_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[P007_P008_Tracking]
                      WHERE Date = ''";
        private bool Tracking_View_Exist = false;

        private bool Tracking_View_Init()
        {
            if (Tracking_View_Exist == true)
            {
                tabControl1.SelectTab("Tracking View");
                return true;
            }
            Tracking_View_Exist = true;
            // Init_Line_Desciption_Excel();
            Tracking_View_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "Tracking View", 0, MasterDatabase_Connection_Str, 
                                                            Tracking_View_Init_Database_CMD, Tracking_View_Select_CMD,
                                                            3, null, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);
            
            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            Tracking_View_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            Tracking_View_MasterDatabase.MasterDatabase_GridviewTBL.Import_BT.Visible = false;
            return true;
        }

        //ExcelImportStruct[] Line_Desciption_Excel_Struct;//  = new ExcelImportStruct[7];
        //const int Line_Desciption_INDEX = 0;

        //private void Init_Line_Desciption_Excel()
        //{
        //    if (Line_Desciption_Excel_Struct == null)
        //    {
        //        Line_Desciption_Excel_Struct = new ExcelImportStruct[12];
        //        Line_Desciption_Excel_Struct[0] = new ExcelImportStruct(0, "PartNumber", "PartNumber", Excel_Col_Type.COL_STRING, 20, true);
        //        Line_Desciption_Excel_Struct[1] = new ExcelImportStruct(1, "PartName", "PartName", Excel_Col_Type.COL_STRING, 20, false);
        //        Line_Desciption_Excel_Struct[2] = new ExcelImportStruct(2, "LineID", "LineID", Excel_Col_Type.COL_STRING, 20, true);
        //        Line_Desciption_Excel_Struct[3] = new ExcelImportStruct(3, "LineName", "LineName", Excel_Col_Type.COL_STRING, 50, false);
        //        Line_Desciption_Excel_Struct[4] = new ExcelImportStruct(4, "WST_ID", "WST_ID", Excel_Col_Type.COL_STRING, 50, true);
        //        Line_Desciption_Excel_Struct[5] = new ExcelImportStruct(5, "WST_Name", "WST_Name", Excel_Col_Type.COL_STRING, 50, false);
        //        Line_Desciption_Excel_Struct[6] = new ExcelImportStruct(6, "GroupID", "GroupID", Excel_Col_Type.COL_STRING, 20, false);
        //        Line_Desciption_Excel_Struct[7] = new ExcelImportStruct(7, "Description", "Description", Excel_Col_Type.COL_STRING, 20, false);
        //        Line_Desciption_Excel_Struct[8] = new ExcelImportStruct(8, "Note", "Note", Excel_Col_Type.COL_STRING, 20, false);
        //        Line_Desciption_Excel_Struct[9] = new ExcelImportStruct(9, "MinResource", "MinResource", Excel_Col_Type.COL_INT, 20, false);
        //        Line_Desciption_Excel_Struct[10] = new ExcelImportStruct(10, "MaxResource", "MaxResource", Excel_Col_Type.COL_INT, 20, false);
        //        Line_Desciption_Excel_Struct[11] = new ExcelImportStruct(11, "MaxCapacity", "MaxCapacity", Excel_Col_Type.COL_INT, 20, false);

        //    }
        //}
    }
}