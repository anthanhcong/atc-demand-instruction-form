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
        MaterDatabase Leave_Info_MasterDatabase;

        public string Leave_Info_Select_CMD = @"SELECT [Empl_ID], [Name], [DepartmentCode], [AttDate], [LeaveCode], [Reason] FROM [SHIFT_REGISTER_DB].[dbo].[Leave_Register_DB] ";
        public string Leave_Info_Init_Database_CMD = @"SELECT [Empl_ID], [Name], [DepartmentCode], [AttDate], [LeaveCode], [Reason] FROM [SHIFT_REGISTER_DB].[dbo].[Leave_Register_DB] WHERE [Empl_ID] = ''";
        private bool Leave_Info_Exist = false;

        private bool Leave_Info_Init()
        {
            if (Leave_Info_Exist == true)
            {
                if (tabControl1.TabPages.Contains(Leave_Info_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, Leave_Info_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("Leave_Info");
                return true;
            }
            Leave_Info_Exist = true;
            Init_Leave_Info_Excel();
            Leave_Info_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "Leave_Info", Leave_Info_Index, LeaveRegister_Connection_Str,
                                                            Leave_Info_Init_Database_CMD, Leave_Info_Select_CMD,
                                                            3, Leave_Info_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            Leave_Info_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            return true;
        }

        ExcelImportStruct[] Leave_Info_Excel_Struct;

        private void Init_Leave_Info_Excel()
        {
            if (Leave_Info_Excel_Struct == null)
            {
                Leave_Info_Excel_Struct = new ExcelImportStruct[6];
                Leave_Info_Excel_Struct[0] = new ExcelImportStruct(0, "Empl_ID", "Empl_ID", Excel_Col_Type.COL_STRING, 10, true);
                Leave_Info_Excel_Struct[1] = new ExcelImportStruct(1, "Name", "Name", Excel_Col_Type.COL_STRING, 100, false);
                Leave_Info_Excel_Struct[2] = new ExcelImportStruct(2, "DepartmentCode", "DepartmentCode", Excel_Col_Type.COL_STRING, 30, false);
                Leave_Info_Excel_Struct[3] = new ExcelImportStruct(3, "AttDate", "AttDate", Excel_Col_Type.COL_DATE, 20, true);
                Leave_Info_Excel_Struct[4] = new ExcelImportStruct(4, "LeaveCode", "LeaveCode", Excel_Col_Type.COL_STRING, 20, false);
                Leave_Info_Excel_Struct[5] = new ExcelImportStruct(5, "Reason", "Reason", Excel_Col_Type.COL_STRING, 50, false);

            }
        }
    }
}
