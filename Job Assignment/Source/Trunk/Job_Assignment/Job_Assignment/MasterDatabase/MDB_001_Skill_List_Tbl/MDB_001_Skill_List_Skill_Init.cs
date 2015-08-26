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
        MaterDatabase SkillList_MasterDatabase;

        public string SkillList_Select_CMD = @"SELECT * FROM [MDB_001_Skill_List_Tbl] ";
        public string SkillList_Init_Database_CMD = @"SELECT * FROM [MDB_001_Skill_List_Tbl] 
                                                      WHERE [Skill_ID] = ''";
        private bool SkillList_Exist = false;

        private bool SkillList_Init()
        {
            if (SkillList_Exist == true)
            {
                if (tabControl1.TabPages.Contains(SkillList_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, SkillList_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("Skill_List");
                return true;
            }
            SkillList_Exist = true;
            Init_Skill_Excel();
            SkillList_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "Skill_List", SkillList_Index, MasterDatabase_Connection_Str, 
                                                            SkillList_Init_Database_CMD, SkillList_Select_CMD,
                                                            3, Skill_Excel_Struct, filterStatusLabel, showAllLabel, 
                                                            StatusLabel1, StatusLabel2, ProgressBar1);
            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            SkillList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            return true;
        }

        ExcelImportStruct[] Skill_Excel_Struct;
        const int Skill_INDEX = 0;

        private void Init_Skill_Excel()
        {
            if (Skill_Excel_Struct == null)
            {
                Skill_Excel_Struct = new ExcelImportStruct[4];
                Skill_Excel_Struct[0] = new ExcelImportStruct(0, "Skill_ID", "Skill_ID", Excel_Col_Type.COL_STRING, 20, true);
                Skill_Excel_Struct[1] = new ExcelImportStruct(1, "Skill_Name", "Skill_Name", Excel_Col_Type.COL_STRING, 50, false);
                Skill_Excel_Struct[2] = new ExcelImportStruct(2, "Description", "Description", Excel_Col_Type.COL_STRING, 200, false);
                Skill_Excel_Struct[3] = new ExcelImportStruct(3, "Note", "Note", Excel_Col_Type.COL_STRING, 100, false);
            }
        }
    }
}