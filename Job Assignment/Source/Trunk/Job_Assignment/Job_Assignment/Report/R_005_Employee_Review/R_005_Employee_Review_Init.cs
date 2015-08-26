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
        MaterDatabase R_005_Employee_Review_MasterDatabase;
        Button_Lbl R_005_Employee_Review_Create_BT;

        public string R_005_Employee_Review_Select_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R005_Employee_Review_Report] ";
        public string R_005_Employee_Review_Init_Database_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R005_Employee_Review_Report] 
                                                            where Date = ''";

        private bool R_005_Employee_Review_Exist = false;
        private int R_005_Employee_Review_Index = 10;

        ExcelImportStruct[] R_005_Employee_Review_Excel_Struct;
        const int R_005_Employee_Review_INDEX = 0;

        private bool R_005_Employee_Review_Init()
        {

            if (R_005_Employee_Review_Exist == true)
            {
                if (tabControl1.TabPages.Contains(R_005_Employee_Review_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, R_005_Employee_Review_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("R_005_Employee_Review");
                return true;
            }
            R_005_Employee_Review_Exist = true;

            // Init_R_005_Employee_Revieww_Excel();
            R_005_Employee_Review_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "R_005_Employee_Review", R_005_Employee_Review_Index, MasterDatabase_Connection_Str,
                                                            R_005_Employee_Review_Init_Database_CMD, R_005_Employee_Review_Select_CMD,
                                                            3, R_005_Employee_Review_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            R_005_Employee_Review_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            R_005_Employee_Review_MasterDatabase.MasterDatabase_GridviewTBL.Privot_BT.Visible = true;
            //R_005_Employee_Review_MasterDatabase.MasterDatabase_GridviewTBL.GridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(R_005_Employee_Review_Control_MasterDatabase_GridView_DataBindingComplete);


            PosSize possize = new PosSize();
            possize.pos_x = 200;
            possize.pos_y = 90;
            R_005_Employee_Review_Create_BT = new Button_Lbl(1, R_005_Employee_Review_MasterDatabase.MasterDatabase_Tab, "Create Report", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            R_005_Employee_Review_Create_BT.My_Button.Click += new EventHandler(R_005_Employee_Review_Create_BT_Click);

            //set role
            string moduleId = "R_005";
            RoleHelper.SetRole(R_005_Employee_Review_MasterDatabase, moduleId);
            R_005_Employee_Review_Create_BT.My_Button.Enabled = RoleHelper.GetCurrentUserLoginRole(UserRoleName.CREATE, moduleId);

            return true;
        }
    }
}
