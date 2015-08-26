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
        MaterDatabase LineWorkStationMapping_MasterDatabase;

        public string LineWorkStationMapping_Select_CMD = @"SELECT * FROM [MDB_005_LineWorkStationMapping] ";
        public string LineWorkStationMapping_Init_Database_CMD = @"SELECT * FROM [MDB_005_LineWorkStationMapping]";
        private bool LineWorkStationMapping_Exist = false;

        private bool LineWorkStationMapping_Init()
        {
            if (LineWorkStationMapping_Exist == true)
            {
                if (tabControl1.TabPages.Contains(LineWorkStationMapping_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, LineWorkStationMapping_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("WorkStationDescription");
                return true;
            }
            LineWorkStationMapping_Exist = true;

            Init_LineWorkStationMapping_Excel();
            LineWorkStationMapping_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "WorkStationDescription", WorkStationDescription_Index, MasterDatabase_Connection_Str, 
                                                            LineWorkStationMapping_Init_Database_CMD, LineWorkStationMapping_Select_CMD,
                                                            3, LineSkillRequest_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            LineWorkStationMapping_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            return true;
        }
        ExcelImportStruct[] LineWorkStationMapping_Excel_Struct;
        //const int LineWorkStationMapping_Index = 0;

        private void Init_LineWorkStationMapping_Excel()
        {
            if (LineSkillRequest_Excel_Struct == null)
            {
                LineWorkStationMapping_Excel_Struct = new ExcelImportStruct[4];
                LineWorkStationMapping_Excel_Struct[0] = new ExcelImportStruct(0, "LineID", "LineID", Excel_Col_Type.COL_STRING, 20, true);
                LineWorkStationMapping_Excel_Struct[1] = new ExcelImportStruct(1, "WstID", "WstID", Excel_Col_Type.COL_STRING, 50, true);
                LineWorkStationMapping_Excel_Struct[2] = new ExcelImportStruct(2, "WstName", "WstName", Excel_Col_Type.COL_STRING, 20, false);
                LineWorkStationMapping_Excel_Struct[3] = new ExcelImportStruct(3, "Note", "Note", Excel_Col_Type.COL_STRING, 50, false);
            }
        }

    }
}