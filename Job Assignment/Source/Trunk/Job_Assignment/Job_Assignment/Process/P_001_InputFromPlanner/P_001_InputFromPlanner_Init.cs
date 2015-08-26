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
        MaterDatabase InputFromPlannerList_MasterDatabase;
        Button_Lbl InputFromPlannerList_Create_BT;

        public string InputFromPlannerList_Select_CMD = @"SELECT * FROM [P_001_InputFromPlanner] ";
        public string InputFromPlannerList_Init_Database_CMD = @"SELECT * FROM [P_001_InputFromPlanner] 
                                                      WHERE [PartNumber] = ''";
        private bool InputFromPlannerList_Exist = false;

        ExcelImportStruct[] InputFromPlanner_Excel_Struct;
        const int InputFromPlanner_INDEX = 0;

        private void Init_InputFromPlanner_Excel()
        {
            if (InputFromPlanner_Excel_Struct == null)
            {
                InputFromPlanner_Excel_Struct = new ExcelImportStruct[5];
                InputFromPlanner_Excel_Struct[0] = new ExcelImportStruct(0, "Date", "Date", Excel_Col_Type.COL_DATE, 20, true);
                InputFromPlanner_Excel_Struct[1] = new ExcelImportStruct(1, "PO", "PO", Excel_Col_Type.COL_STRING, 20, false);
                InputFromPlanner_Excel_Struct[2] = new ExcelImportStruct(2, "PartNumber", "PartNumber", Excel_Col_Type.COL_STRING, 50, false);
                InputFromPlanner_Excel_Struct[3] = new ExcelImportStruct(3, "Qty", "Qty", Excel_Col_Type.COL_INT, 20, false);
                InputFromPlanner_Excel_Struct[4] = new ExcelImportStruct(4, "Priority", "Priority", Excel_Col_Type.COL_INT, 20, false);
            }
        }

        private bool P001_InputFromPlanner_Init()
        {
            if (InputFromPlannerList_Exist == true)
            {
                if (tabControl1.TabPages.Contains(InputFromPlannerList_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, InputFromPlannerList_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("P001_InputFromPlanner");

                return true;
            }
            InputFromPlannerList_Exist = true;
            Init_InputFromPlanner_Excel();
            InputFromPlannerList_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "P001_InputFromPlanner", InputFromPlannerList_Index, MasterDatabase_Connection_Str, 
                                                            InputFromPlannerList_Init_Database_CMD, InputFromPlannerList_Select_CMD,
                                                            3, InputFromPlanner_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);
            
            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            InputFromPlannerList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;

            //Dho-Fixme: Do we need to use the button "Check_BT"?
            PosSize possize = new PosSize();
            possize.pos_x = 200;
            possize.pos_y = 90;
            InputFromPlannerList_Create_BT = new Button_Lbl(1, InputFromPlannerList_MasterDatabase.MasterDatabase_Tab, "Create", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            InputFromPlannerList_Create_BT.My_Button.Click += new EventHandler(InputFromPlannerList_Create_BT_Click);

            //InputFromPlannerList_MasterDatabase.MasterDatabase_GridviewTBL.Import_BT.Enabled 
            RoleHelper.SetRole(InputFromPlannerList_MasterDatabase, "P_001");
            InputFromPlannerList_Create_BT.My_Button.Enabled = RoleHelper.GetCurrentUserLoginRole(UserRoleName.CREATE, "P_001");

            //set role
            string moduleId = "P_001";
            RoleHelper.SetRole(InputFromPlannerList_MasterDatabase, moduleId);
            InputFromPlannerList_Create_BT.My_Button.Enabled = RoleHelper.GetCurrentUserLoginRole(UserRoleName.CREATE, moduleId);

            return true;
        } 
    }
}