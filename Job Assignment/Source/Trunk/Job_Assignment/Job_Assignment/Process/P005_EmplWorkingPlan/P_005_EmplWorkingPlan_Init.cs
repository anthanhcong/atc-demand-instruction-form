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
        MaterDatabase EmplWorkingPlan_MasterDatabase;
        Button_Lbl EmplWorkingPlan_Create_BT;

        public string EmplWorkingPlan_Select_CMD = @"SELECT * FROM [P_005_EmplWorkingPlan] ";
        public string EmplWorkingPlan_Init_Database_CMD = @"SELECT * FROM [P_005_EmplWorkingPlan] 
                                                      WHERE [Date] = ''";
        private bool EmplWorkingPlan_Exist = false;
		private int EmplWorkingPlan_Index = 7;
		ExcelImportStruct[] EmplWorkingPlan_Excel_Struct;
        const int EmplWorkingPlan_INDEX = 0;

        private bool P005_EmplWorkingPlan_Init()
        {
            if (EmplWorkingPlan_Exist == true)
            {
                if (tabControl1.TabPages.Contains(EmplWorkingPlan_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, EmplWorkingPlan_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("P005_EmplPlan");
                return true;
            }
            EmplWorkingPlan_Exist = true;

            // Init_EmplWorkingPlan_Excel();
            EmplWorkingPlan_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "P005_EmplPlan", EmplWorkingPlan_Index, MasterDatabase_Connection_Str, 
                                                            EmplWorkingPlan_Init_Database_CMD, EmplWorkingPlan_Select_CMD,
                                                            3, EmplWorkingPlan_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            EmplWorkingPlan_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            EmplWorkingPlan_MasterDatabase.MasterDatabase_GridviewTBL.Import_BT.Visible = false;
            EmplWorkingPlan_MasterDatabase.MasterDatabase_GridviewTBL.Review_BT.Visible = false;
            EmplWorkingPlan_MasterDatabase.MasterDatabase_GridviewTBL.Delete_Rows_BT.Visible = false;

            PosSize possize = new PosSize();
            possize.pos_x = 200;
            possize.pos_y = 90;
            EmplWorkingPlan_Create_BT = new Button_Lbl(1, EmplWorkingPlan_MasterDatabase.MasterDatabase_Tab, "Create", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            EmplWorkingPlan_Create_BT.My_Button.Click += new EventHandler(EmplWorkingPlan_Create_BT_Click);
            EmplWorkingPlan_Create_BT.My_Button.Visible = true;

            //set role
            string moduleId = "P_005";
            RoleHelper.SetRole(EmplWorkingPlan_MasterDatabase, moduleId);
            EmplWorkingPlan_Create_BT.My_Button.Enabled = RoleHelper.GetCurrentUserLoginRole(UserRoleName.CREATE, moduleId);

            return true;
        }

       
    }
}