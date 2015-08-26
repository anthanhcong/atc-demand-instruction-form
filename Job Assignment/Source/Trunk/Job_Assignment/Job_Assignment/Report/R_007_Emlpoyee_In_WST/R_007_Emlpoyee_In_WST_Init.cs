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
        MaterDatabase R_007_Emlpoyee_In_WST_MasterDatabase;
        Button_Lbl R_007_Emlpoyee_In_WST_Create_BT;
        Button_Lbl R_007_Emlpoyee_In_WST_CountA_Create_BT;

        public string R_007_Emlpoyee_In_WST_Select_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R007_Employee_In_WST] ";
        public string R_007_Emlpoyee_In_WST_Init_Database_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R007_Employee_In_WST]
                                                            where LineID = ''";

        private bool R_007_Emlpoyee_In_WST_Exist = false;
        private int R_007_Emlpoyee_In_WST_Index = 11;

        ExcelImportStruct[] R_007_Emlpoyee_In_WST_Excel_Struct;
        const int R_007_Emlpoyee_In_WST_INDEX = 0;

        private bool R_007_Emlpoyee_In_WST_Init()
        {
            LoadInternalData();
            if (R_007_Emlpoyee_In_WST_Exist == true)
            {
                if (tabControl1.TabPages.Contains(R_007_Emlpoyee_In_WST_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, R_007_Emlpoyee_In_WST_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("R_007_Emlpoyee_In_WST");
                return true;
            }
            R_007_Emlpoyee_In_WST_Exist = true;

            //R_007_Emlpoyee_In_WST_Excel();
            R_007_Emlpoyee_In_WST_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "R_007_Emlpoyee_In_WST", R_007_Emlpoyee_In_WST_Index, MasterDatabase_Connection_Str,
                                                            R_007_Emlpoyee_In_WST_Init_Database_CMD, R_007_Emlpoyee_In_WST_Select_CMD,
                                                            3, R_007_Emlpoyee_In_WST_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            R_007_Emlpoyee_In_WST_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            //R_007_Emlpoyee_In_WST_MasterDatabase.MasterDatabase_GridviewTBL.GridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(R_007_Emlpoyee_In_WST_Control_MasterDatabase_GridView_DataBindingComplete);
            R_007_Emlpoyee_In_WST_MasterDatabase.MasterDatabase_GridviewTBL.Privot_BT.Visible = true;

            PosSize possize = new PosSize();
            possize.pos_x = 200;
            possize.pos_y = 90;
            R_007_Emlpoyee_In_WST_Create_BT = new Button_Lbl(1, R_007_Emlpoyee_In_WST_MasterDatabase.MasterDatabase_Tab, "Create Report", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            R_007_Emlpoyee_In_WST_Create_BT.My_Button.Click += new EventHandler(R_007_Emlpoyee_In_WST_Create_BT_Click);

            possize.pos_x = 300;
            possize.pos_y = 90;
            R_007_Emlpoyee_In_WST_CountA_Create_BT = new Button_Lbl(1, R_007_Emlpoyee_In_WST_MasterDatabase.MasterDatabase_Tab, "Count", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            R_007_Emlpoyee_In_WST_CountA_Create_BT.My_Button.Click += new EventHandler(R_007_Emlpoyee_In_WST_Count_Create_BT_Click);

            //set role
            string moduleId = "R_007";
            RoleHelper.SetRole(R_007_Emlpoyee_In_WST_MasterDatabase, moduleId);
            R_007_Emlpoyee_In_WST_Create_BT.My_Button.Enabled = RoleHelper.GetCurrentUserLoginRole(UserRoleName.CREATE, moduleId);
            R_007_Emlpoyee_In_WST_CountA_Create_BT.My_Button.Enabled = RoleHelper.GetCurrentUserLoginRole(UserRoleName.CREATE, moduleId);

            return true;
        }
    }
}
