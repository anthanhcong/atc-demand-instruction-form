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
        MaterDatabase R_007_Emlpoyee_In_WST_Count_MasterDatabase;
        Button_Lbl R_007_Emlpoyee_In_WST_Count_Create_BT;

//        public string R_007_Emlpoyee_In_WST_Count_Select_CMD = @"SELECT WST_ID, COUNT(WST_ID) N'Số người làm được' 
//                                                                FROM [JOB_ASSIGNMENT_DB].[dbo].[R007_Employee_In_WST]
//                                                                GROUP BY WST_ID ";

        public string R_007_Emlpoyee_In_WST_Count_Select_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R007_Employee_In_WST_Count] ";
        public string R_007_Emlpoyee_In_WST_Count_Init_Database_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R007_Employee_In_WST_Count] ";

        private bool R_007_Emlpoyee_In_WST_Count_Exist = false;
        private int R_007_Emlpoyee_In_WST_Count_Index = 11;

        ExcelImportStruct[] R_007_Emlpoyee_In_WST_Excel_Count_Struct;

        private bool R_007_Emlpoyee_In_WST_Count_Init()
        {
            if (R_007_Emlpoyee_In_WST_Count_Exist == true)
            {
                if (tabControl1.TabPages.Contains(R_007_Emlpoyee_In_WST_Count_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, R_007_Emlpoyee_In_WST_Count_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("R_007_Emlpoyee_In_WST_Count");
                return true;
            }
            R_007_Emlpoyee_In_WST_Count_Exist = true;

            R_007_Emlpoyee_In_WST_Count_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "R_007_Emlpoyee_In_WST_Count", R_007_Emlpoyee_In_WST_Count_Index, MasterDatabase_Connection_Str,
                                                            R_007_Emlpoyee_In_WST_Count_Init_Database_CMD, R_007_Emlpoyee_In_WST_Count_Select_CMD,
                                                            3, R_007_Emlpoyee_In_WST_Excel_Count_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            R_007_Emlpoyee_In_WST_Count_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            R_007_Emlpoyee_In_WST_Count_MasterDatabase.MasterDatabase_GridviewTBL.Privot_BT.Visible = false;
            R_007_Emlpoyee_In_WST_Count_MasterDatabase.MasterDatabase_GridviewTBL.Delete_All_BT.Visible = false;
            R_007_Emlpoyee_In_WST_Count_MasterDatabase.MasterDatabase_GridviewTBL.Export_BT.Visible = false;
            R_007_Emlpoyee_In_WST_Count_MasterDatabase.MasterDatabase_GridviewTBL.Import_BT.Visible = false;
            R_007_Emlpoyee_In_WST_Count_MasterDatabase.MasterDatabase_GridviewTBL.Submit_BT.Visible = false;

            PosSize possize = new PosSize();
            possize.pos_x = 200;
            possize.pos_y = 90;
            R_007_Emlpoyee_In_WST_Count_Create_BT = new Button_Lbl(1, R_007_Emlpoyee_In_WST_Count_MasterDatabase.MasterDatabase_Tab, "Create Report", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            R_007_Emlpoyee_In_WST_Count_Create_BT.My_Button.Click += new EventHandler(R_007_Emlpoyee_In_WST_Count_Create01_BT_Click);

            return true;
        }
    }
}
