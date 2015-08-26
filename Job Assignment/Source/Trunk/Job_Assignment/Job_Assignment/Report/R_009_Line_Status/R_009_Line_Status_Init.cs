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
        MaterDatabase R_009_Line_Status_MasterDatabase;
        Button_Lbl R_009_Line_Status_Create_BT;

        public string R_009_Line_Status_Select_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R_009_Line_Status] ";
        public string R_009_Line_Status_Init_Database_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R_009_Line_Status] 
                                                            where [Date] = ''";

        private bool R_009_Line_Status_Exist = false;
        private int R_009_Line_Status_Index = 10;

        ExcelImportStruct[] R_009_Line_Status_Excel_Struct;
        const int R_009_Line_Status_INDEX = 0;

        private bool R_009_Line_Status_Init()
        {

            if (R_009_Line_Status_Exist == true)
            {
                if (tabControl1.TabPages.Contains(R_009_Line_Status_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, R_009_Line_Status_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("R_009_Line_Status");
                return true;
            }
            R_009_Line_Status_Exist = true;

            R_009_Line_Status_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "R_009_Line_Status", R_009_Line_Status_Index, MasterDatabase_Connection_Str,
                                                            R_009_Line_Status_Init_Database_CMD, R_009_Line_Status_Select_CMD,
                                                            3, R_009_Line_Status_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            R_009_Line_Status_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            R_009_Line_Status_MasterDatabase.MasterDatabase_GridviewTBL.Privot_BT.Visible = true;
            R_009_Line_Status_MasterDatabase.MasterDatabase_GridviewTBL.Submit_BT.Visible = false;
            R_009_Line_Status_MasterDatabase.MasterDatabase_GridviewTBL.Import_BT.Visible = false;
            R_009_Line_Status_MasterDatabase.MasterDatabase_GridviewTBL.Delete_All_BT.Visible = false;
            R_009_Line_Status_MasterDatabase.MasterDatabase_GridviewTBL.Review_BT.Visible = false;
            //R_009_Line_Status_MasterDatabase.MasterDatabase_GridviewTBL.GridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(R_009_Line_Status_MasterDatabase_GridView_DataBindingComplete);
            R_009_Line_Status_MasterDatabase.MasterDatabase_GridviewTBL.GridView.ReadOnly = true;

            PosSize possize = new PosSize();
            possize.pos_x = 200;
            possize.pos_y = 90;
            R_009_Line_Status_Create_BT = new Button_Lbl(1, R_009_Line_Status_MasterDatabase.MasterDatabase_Tab, "Refresh", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            R_009_Line_Status_Create_BT.My_Button.Click += new EventHandler(R_009_Line_Status_Create_BT_Click);

            //set role
            //string moduleId = "R_009";
            //RoleHelper.SetRole(R_009_Line_Status_MasterDatabase, moduleId);
            //R_009_Line_Status_Create_BT.My_Button.Enabled = RoleHelper.GetCurrentUserLoginRole(UserRoleName.CREATE, moduleId);

            return true;
        }
    }
}
