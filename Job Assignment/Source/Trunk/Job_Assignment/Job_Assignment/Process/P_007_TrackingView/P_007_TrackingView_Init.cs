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
        MaterDatabase P007_Tracking_View_MasterDatabase;
        Button_Lbl P007_Tracking_View_Create_BT;

        public string P007_Tracking_View_Select_CMD = @"SELECT * FROM [P007_P008_Tracking] ";
        public string P007_Tracking_View_Init_Database_CMD = @"SELECT * FROM [P007_P008_Tracking] 
                                                      WHERE [Date] = ''";
        private bool P007_Tracking_View_Exist = false;
        private int P007_Tracking_View_Index = 9;

        private bool P007_Tracking_View_Init()
        {

            if (P007_Tracking_View_Exist == true)
            {
                if (tabControl1.TabPages.Contains(P007_Tracking_View_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, P007_Tracking_View_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("P007_Tracking_View");
                return true;
            }
            P007_Tracking_View_Exist = true;

            // Init_P007_Tracking_View_Excel();
            P007_Tracking_View_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "P007_Tracking_View", P007_Tracking_View_Index, MasterDatabase_Connection_Str,
                                                            P007_Tracking_View_Init_Database_CMD, P007_Tracking_View_Select_CMD,
                                                            3, P007_Tracking_View_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            P007_Tracking_View_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            P007_Tracking_View_MasterDatabase.MasterDatabase_GridviewTBL.GridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(P_007_Tracking_Control_MasterDatabase_GridView_DataBindingComplete);
            

            //Dho-Fixme: Do we need to use the button "Check_BT"?
            //PosSize possize = new PosSize();
            //possize.pos_x = 200;
            //possize.pos_y = 90;
            //P007_Tracking_View_Create_BT = new Button_Lbl(1, P007_Tracking_View_MasterDatabase.MasterDatabase_Tab, "Plan_Empl", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            //P007_Tracking_View_Create_BT.My_Button.Click += new EventHandler(P007_Tracking_View_Create_BT_Click);

            // khong set role được, vì chưa có tạo nút button nào cả
            //set role
            //string moduleId = "P_007";
            //RoleHelper.SetRole(P007_Tracking_View_MasterDatabase, moduleId);
            //P007_Tracking_View_Create_BT.My_Button.Enabled = RoleHelper.GetCurrentUserLoginRole(UserRoleName.CREATE, moduleId);

            return true;
        }
        ExcelImportStruct[] P007_Tracking_View_Excel_Struct;
        const int P007_Tracking_View_INDEX = 0;

        //private void Init_P007_Tracking_View_Excel()
        //{
        //    if (P007_Tracking_View_Excel_Struct == null)
        //    {
        //        P007_Tracking_View_Excel_Struct = new ExcelImportStruct[11];
        //        P007_Tracking_View_Excel_Struct[0] = new ExcelImportStruct(0, "Line_ID", "Line_ID", Excel_Col_Type.COL_STRING, 20, true);
        //        P007_Tracking_View_Excel_Struct[1] = new ExcelImportStruct(1, "Line_Name", "Line_Name", Excel_Col_Type.COL_STRING, 50, false);
        //        P007_Tracking_View_Excel_Struct[2] = new ExcelImportStruct(2, "WST_ID", "WST_ID", Excel_Col_Type.COL_STRING, 20, true);
        //        P007_Tracking_View_Excel_Struct[3] = new ExcelImportStruct(3, "WST_Name", "WST_Name", Excel_Col_Type.COL_STRING, 50, false);
        //        P007_Tracking_View_Excel_Struct[4] = new ExcelImportStruct(4, "WST_x", "WST_x", Excel_Col_Type.COL_INT, 20, false);
        //        P007_Tracking_View_Excel_Struct[5] = new ExcelImportStruct(5, "WST_y", "WST_y", Excel_Col_Type.COL_INT, 20, false);
        //        P007_Tracking_View_Excel_Struct[6] = new ExcelImportStruct(6, "WST_width", "WST_width", Excel_Col_Type.COL_INT, 20, false);
        //        P007_Tracking_View_Excel_Struct[7] = new ExcelImportStruct(7, "WST_heigh", "WST_heigh", Excel_Col_Type.COL_INT, 20, false);
        //        P007_Tracking_View_Excel_Struct[8] = new ExcelImportStruct(8, "GroupID", "GroupID", Excel_Col_Type.COL_STRING, 20, false);
        //        P007_Tracking_View_Excel_Struct[9] = new ExcelImportStruct(9, "Description", "Description", Excel_Col_Type.COL_STRING, 200, false);
        //        P007_Tracking_View_Excel_Struct[10] = new ExcelImportStruct(10, "Note", "Note", Excel_Col_Type.COL_STRING, 200, false);
        //    }
        //}

    }
}