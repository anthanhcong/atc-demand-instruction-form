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
        MaterDatabase Employee_Working_on_Sunday_MasterDatabase;

        public string Employee_Working_on_Sunday_Select_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_011_Employee_Working_on_Sunday] ";
        public string Employee_Working_on_Sunday_Init_Database_CMD = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_011_Employee_Working_on_Sunday] WHERE [Date] = ''";
        private bool Employee_Working_on_Sunday_Exist = false;

        private bool Employee_Working_on_Sunday_Init()
        {
            if (Employee_Working_on_Sunday_Exist == true)
            {
                if (tabControl1.TabPages.Contains(Employee_Working_on_Sunday_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, Employee_Working_on_Sunday_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("Employee Working on Sunday");
                return true;
            }
            Employee_Working_on_Sunday_Exist = true;
            Init_Employee_Working_on_Sunday_Excel();
            Employee_Working_on_Sunday_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "Employee Working on Sunday", Employee_Working_on_Sunday_Index, MasterDatabase_Connection_Str,
                                                            Employee_Working_on_Sunday_Init_Database_CMD, Employee_Working_on_Sunday_Select_CMD,
                                                            3, Employee_Working_on_Sunday_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            Employee_Working_on_Sunday_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            return true;
        }

        ExcelImportStruct[] Employee_Working_on_Sunday_Excel_Struct;

        private void Init_Employee_Working_on_Sunday_Excel()
        {
            if (Employee_Working_on_Sunday_Excel_Struct == null)
            {
                Employee_Working_on_Sunday_Excel_Struct = new ExcelImportStruct[6];
                Employee_Working_on_Sunday_Excel_Struct[0] = new ExcelImportStruct(0, "Date", "Date", Excel_Col_Type.COL_DATE, 20, false);
                Employee_Working_on_Sunday_Excel_Struct[1] = new ExcelImportStruct(1, "Empl_ID", "Empl_ID", Excel_Col_Type.COL_STRING, 20, true);
                Employee_Working_on_Sunday_Excel_Struct[2] = new ExcelImportStruct(2, "Empl_Name", "Empl_Name", Excel_Col_Type.COL_STRING, 50, false);
                Employee_Working_on_Sunday_Excel_Struct[3] = new ExcelImportStruct(3, "ShiftName", "ShiftName", Excel_Col_Type.COL_STRING, 20, false);
                Employee_Working_on_Sunday_Excel_Struct[4] = new ExcelImportStruct(4, "LineID", "LineID", Excel_Col_Type.COL_STRING, 20, false);
                Employee_Working_on_Sunday_Excel_Struct[5] = new ExcelImportStruct(5, "WST_ID", "WST_ID", Excel_Col_Type.COL_STRING, 20, true);
            }
        }
    }
}
