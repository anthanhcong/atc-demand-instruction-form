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
        MaterDatabase R004_TrackingKHTT_MasterDatabase;
        Button_Lbl R004_Tracking_KHTT_Create_BT;

        //public string R004_Tracking_KHTT_Select_CMD = @"select Date, ShiftName, LineID, SubLine_ID, WST_ID, Empl_ID as Plan_Empl_ID, Empl_Name as Plan_Empl_Name from P_003_KeHoachSanXuatTheoLine ";
        //public string R004_Tracking_KHTT_Init_Database_CMD = @"select Date, ShiftName, LineID, SubLine_ID, WST_ID, Empl_ID as Plan_Empl_ID, Empl_Name as Plan_Empl_Name from P_003_KeHoachSanXuatTheoLine where Date = ''";

        public string R004_Tracking_KHTT_Select_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R001_Employee_AssignReport] ";
        public string R004_Tracking_KHTT_Init_Database_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R001_Employee_AssignReport] 
                                                            where Date = ''";

        private bool R004_Tracking_KHTT_Exist = false;
        private int R004_Tracking_KHTT_Index = 9;

        ExcelImportStruct[] R004_Tracking_View_Excel_Struct;
        const int R004_Tracking_View_INDEX = 0;

        private bool R004_Tracking_KHTT_Init()
        {

            if (R004_Tracking_KHTT_Exist == true)
            {
                if (tabControl1.TabPages.Contains(R004_TrackingKHTT_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, R004_TrackingKHTT_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("R004_Tracking_KHTT");
                return true;
            }
            R004_Tracking_KHTT_Exist = true;

            // Init_P007_Tracking_View_Excel();
            R004_TrackingKHTT_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "R004_Tracking_KHTT", R004_Tracking_KHTT_Index, MasterDatabase_Connection_Str,
                                                            R004_Tracking_KHTT_Init_Database_CMD, R004_Tracking_KHTT_Select_CMD,
                                                            3, R004_Tracking_View_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            R004_TrackingKHTT_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            R004_TrackingKHTT_MasterDatabase.MasterDatabase_GridviewTBL.GridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(R_004_Tracking_Control_MasterDatabase_GridView_DataBindingComplete);
            R004_TrackingKHTT_MasterDatabase.MasterDatabase_GridviewTBL.Privot_BT.Visible = true;

            PosSize possize = new PosSize();
            possize.pos_x = 200;
            possize.pos_y = 90;
            R004_Tracking_KHTT_Create_BT = new Button_Lbl(1, R004_TrackingKHTT_MasterDatabase.MasterDatabase_Tab, "Create Report", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            R004_Tracking_KHTT_Create_BT.My_Button.Click += new EventHandler(R004_Tracking_KHTT_Create_BT_Click);

            //set role
            string moduleId = "R_004";
            RoleHelper.SetRole(R004_TrackingKHTT_MasterDatabase, moduleId);
            R004_Tracking_KHTT_Create_BT.My_Button.Enabled = RoleHelper.GetCurrentUserLoginRole(UserRoleName.CREATE, moduleId);

            return true;
        }

    }
}
