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
        MaterDatabase Empl_Skill_List_MasterDatabase;

        public string Empl_Skill_List_Select_CMD = @"SELECT * FROM [MDB_002_Empl_Skill] ";
        public string Empl_Skill_List_Init_Database_CMD = @"SELECT * FROM [MDB_002_Empl_Skill] 
                                                      WHERE [Empl_ID] = ''";
        private bool Empl_Skill_List_Exist = false;

        SQL_API.SQL_ATC All_Skill_List;

        private bool Empl_Skill_List_Init()
        {
            if (Empl_Skill_List_Exist == true)
            {
                if (tabControl1.TabPages.Contains(Empl_Skill_List_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, Empl_Skill_List_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("Employee_vs_Skill");
                return true;
            }
            Empl_Skill_List_Exist = true;
            Init_Empl_Skill_Excel();
            Empl_Skill_List_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "Employee_vs_Skill", SkillList_Index, MasterDatabase_Connection_Str, 
                                                            Empl_Skill_List_Init_Database_CMD, Empl_Skill_List_Select_CMD,
                                                            3, Empl_Skill_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);
            
            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;

            All_Skill_List = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string sql_cmd = @"SELECT [Skill_ID] ,[Skill_Name] FROM [MDB_001_Skill_List_Tbl]";
            All_Skill_List.GET_SQL_DATA(sql_cmd);

            DataGridViewMultiColumnComboBoxColumn col = new DataGridViewMultiColumnComboBoxColumn();
            col.Name = "Skill_ID";
            col.DataPropertyName = "Skill_ID";
            col.ValueMember = "Skill_ID";
            col.DataSource = All_Skill_List.DaTable;
            col.ColumnWidths = new List<string>() { "60", "150" };

            if (Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns.Contains("Skill_ID"))
            {
                int index = Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Skill_ID"].Index;
                Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns.RemoveAt(index);
                Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns.Insert(index, col);
                col.HeaderCell = new DataGridViewAutoFilterColumnHeaderCell(col.HeaderCell);
            }
            Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.GridView.CellValueChanged += new DataGridViewCellEventHandler(Empl_Skill_List_MasterDatabase_GridView_CellValueChanged);
            return true;
        }

        ExcelImportStruct[] Empl_Skill_Excel_Struct;//  = new ExcelImportStruct[7];
        const int EMPL_SKILL_INDEX_INDEX = 0;

        private void Init_Empl_Skill_Excel()
        {
            if (Empl_Skill_Excel_Struct == null)
            {
                Empl_Skill_Excel_Struct = new ExcelImportStruct[6];
                Empl_Skill_Excel_Struct[0] = new ExcelImportStruct(0, "Empl_ID", "Empl_ID", Excel_Col_Type.COL_STRING, 20, true);
                Empl_Skill_Excel_Struct[1] = new ExcelImportStruct(1, "Empl_Name", "Empl_Name", Excel_Col_Type.COL_STRING, 50, false);
                Empl_Skill_Excel_Struct[2] = new ExcelImportStruct(2, "Skill_ID", "Skill_ID", Excel_Col_Type.COL_STRING, 20, true);
                Empl_Skill_Excel_Struct[3] = new ExcelImportStruct(3, "Skill_Name", "Skill_Name", Excel_Col_Type.COL_STRING, 50, false);
                Empl_Skill_Excel_Struct[4] = new ExcelImportStruct(4, "Priority", "Priority", Excel_Col_Type.COL_STRING, 20, false);
                Empl_Skill_Excel_Struct[5] = new ExcelImportStruct(5, "GroupID", "GroupID", Excel_Col_Type.COL_STRING, 20, false);
            }
        }


        void Empl_Skill_List_MasterDatabase_GridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            string columnName = Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns[e.ColumnIndex].Name;
            if ("Skill_ID".Equals(columnName))
            {

                string employId = Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows[e.RowIndex].Cells["Skill_ID"].Value as string;
                DataRow[] searchRows = All_Skill_List.DaTable.Select("Skill_ID ='" + employId + "'");
                if (searchRows.Length > 0)
                {
                    Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.GridView[e.ColumnIndex + 1, e.RowIndex].Value = searchRows[0]["Skill_Name"];
                }

                //update datatable
                // ProductionPlanDetail_tbAllEmployee = ProductionPlanDetail_CreateListEmployee();
            }
        }
    }
}