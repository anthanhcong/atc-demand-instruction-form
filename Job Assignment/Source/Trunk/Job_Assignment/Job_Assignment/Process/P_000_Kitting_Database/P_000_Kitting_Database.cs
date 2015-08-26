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
        MaterDatabase KittingDatabase_MasterDatabase;

        public string KittingDatabase_Select_CMD = @"SELECT * FROM [OpenPOPlanner] ";
        public string KittingDatabase_Init_Database_CMD = @"SELECT * FROM [OpenPOPlanner] WHERE ActiveDateTime = ''";
        private bool KittingDatabase_Exist = false;

        //ExcelImportStruct[] InputFromPlanner_Excel_Struct;
        const int KittingDatabase_Index = 0;

        // private void Init_InputFromPlanner_Excel()
        // {
            // if (InputFromPlanner_Excel_Struct == null)
            // {
                // InputFromPlanner_Excel_Struct = new ExcelImportStruct[5];
                // InputFromPlanner_Excel_Struct[0] = new ExcelImportStruct(0, "Date", "Date", Excel_Col_Type.COL_DATE, 20, true);
                // InputFromPlanner_Excel_Struct[1] = new ExcelImportStruct(1, "PO", "PO", Excel_Col_Type.COL_STRING, 20, false);
                // InputFromPlanner_Excel_Struct[2] = new ExcelImportStruct(2, "PartNumber", "PartNumber", Excel_Col_Type.COL_STRING, 50, false);
                // InputFromPlanner_Excel_Struct[3] = new ExcelImportStruct(3, "Qty", "Qty", Excel_Col_Type.COL_INT, 20, false);
                // InputFromPlanner_Excel_Struct[4] = new ExcelImportStruct(4, "Priority", "Priority", Excel_Col_Type.COL_INT, 20, false);
            // }
        // }

        private bool KittingDatabase_Init()
        {
            if (KittingDatabase_Exist == true)
            {
                if (tabControl1.TabPages.Contains(KittingDatabase_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, KittingDatabase_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("Kitting");

                return true;
            }
            KittingDatabase_Exist = true;
            Init_InputFromPlanner_Excel();
            KittingDatabase_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "Kitting", KittingDatabase_Index, Kitting_Connection_Str, 
                                                            KittingDatabase_Init_Database_CMD, KittingDatabase_Select_CMD,
                                                            3, null, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);
            
            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            KittingDatabase_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            KittingDatabase_MasterDatabase.MasterDatabase_GridviewTBL.Import_BT.Visible = false;
            KittingDatabase_MasterDatabase.MasterDatabase_GridviewTBL.Delete_All_BT.Visible = false;
            KittingDatabase_MasterDatabase.MasterDatabase_GridviewTBL.Delete_Rows_BT.Visible = false;
            KittingDatabase_MasterDatabase.MasterDatabase_GridviewTBL.Submit_BT.Visible = false;

            //Dho-Fixme: Do we need to use the button "Check_BT"?
            // PosSize possize = new PosSize();
            // possize.pos_x = 200;
            // possize.pos_y = 90;
            // KittingDatabase_Create_BT = new Button_Lbl(1, KittingDatabase_MasterDatabase.MasterDatabase_Tab, "Create", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            // KittingDatabase_Create_BT.My_Button.Click += new EventHandler(KittingDatabase_Create_BT_Click);

            return true;
        } 
    }
}