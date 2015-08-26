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
        MaterDatabase R_006_TrackingTT_PlanTL_MasterDatabase;
        Button_Lbl R_006_TrackingTT_PlanTL_Create_BT;

        public string R_006_TrackingTT_PlanTL_Select_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R_006_TrackingTT_PlanTL_Report] ";
        public string R_006_TrackingTT_PlanTL_Init_Database_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R_006_TrackingTT_PlanTL_Report]
                                                            where Date = ''";

        private bool R_006_TrackingTT_PlanTL_Exist = false;
        private int R_006_TrackingTT_PlanTL_Index = 11;

        ExcelImportStruct[] R_006_TrackingTT_PlanTL_Excel_Struct;
        const int R_006_TrackingTT_PlanTL_INDEX = 0;

        private bool R_006_TrackingTT_PlanTL_Init()
        {

            if (R_006_TrackingTT_PlanTL_Exist == true)
            {
                if (tabControl1.TabPages.Contains(R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("R_006_TrackingTT_PlanTL");
                return true;
            }
            R_006_TrackingTT_PlanTL_Exist = true;

            //R_006_TrackingTT_PlanTL_Excel();
            R_006_TrackingTT_PlanTL_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "R_006_TrackingTT_PlanTL", R_006_TrackingTT_PlanTL_Index, MasterDatabase_Connection_Str,
                                                            R_006_TrackingTT_PlanTL_Init_Database_CMD, R_006_TrackingTT_PlanTL_Select_CMD,
                                                            3, R_006_TrackingTT_PlanTL_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_GridviewTBL.GridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(R_006_TrackingTT_PlanTL_Control_MasterDatabase_GridView_DataBindingComplete);
            R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_GridviewTBL.Privot_BT.Visible = true;

            PosSize possize = new PosSize();
            possize.pos_x = 200;
            possize.pos_y = 90;
            R_006_TrackingTT_PlanTL_Create_BT = new Button_Lbl(1, R_006_TrackingTT_PlanTL_MasterDatabase.MasterDatabase_Tab, "Create Report", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            R_006_TrackingTT_PlanTL_Create_BT.My_Button.Click += new EventHandler(R_006_TrackingTT_PlanTL_Create_BT_Click);

            //set role
            string moduleId = "R_006";
            RoleHelper.SetRole(R_006_TrackingTT_PlanTL_MasterDatabase, moduleId);
            R_006_TrackingTT_PlanTL_Create_BT.My_Button.Enabled = RoleHelper.GetCurrentUserLoginRole(UserRoleName.CREATE, moduleId);

            return true;
        }

        //private void R_006_TrackingTT_PlanTL_Excel()
        //{
        //    if (R_006_TrackingTT_PlanTL_Excel_Struct == null)
        //    {
        //        R_006_TrackingTT_PlanTL_Excel_Struct = new ExcelImportStruct[8];
        //        R_006_TrackingTT_PlanTL_Excel_Struct[0] = new ExcelImportStruct(0, "Date", "Date", Excel_Col_Type.COL_STRING, 20, false);
        //        R_006_TrackingTT_PlanTL_Excel_Struct[1] = new ExcelImportStruct(1, "ShiftName", "ShiftName", Excel_Col_Type.COL_STRING, 20, false);
        //        R_006_TrackingTT_PlanTL_Excel_Struct[2] = new ExcelImportStruct(2, "LineID", "LineID", Excel_Col_Type.COL_STRING, 20, false);
        //        R_006_TrackingTT_PlanTL_Excel_Struct[3] = new ExcelImportStruct(3, "WST_ID", "WST_ID", Excel_Col_Type.COL_DATE, 20, false);
        //        R_006_TrackingTT_PlanTL_Excel_Struct[4] = new ExcelImportStruct(4, "Empl_ID", "Empl_ID", Excel_Col_Type.COL_STRING, 20, false);
        //        R_006_TrackingTT_PlanTL_Excel_Struct[5] = new ExcelImportStruct(5, "Empl_Name", "Empl_Name", Excel_Col_Type.COL_STRING, 50, false);
        //        R_006_TrackingTT_PlanTL_Excel_Struct[3] = new ExcelImportStruct(6, "Plan_Empl_ID", "Plan_Empl_ID", Excel_Col_Type.COL_DATE, 20, false);
        //        R_006_TrackingTT_PlanTL_Excel_Struct[4] = new ExcelImportStruct(7, "Plan_Empl_Name", "Plan_Empl_Name", Excel_Col_Type.COL_STRING, 50, false);

        //    }
        //}
    }
}
