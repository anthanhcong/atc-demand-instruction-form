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
        MaterDatabase ProductionPlanDetail_MasterDatabase;
        Button_Lbl ProductionPlanDetail_Create_BT;
        Button_Lbl ProductionPlanDetail_Assign_BT;
        DataTable ProductionPlanDetail_tbAllEmployee;

        public string ProductionPlanDetail_Select_CMD = @"SELECT * FROM [P_004_KeHoachSanXuatTheoTram] ";
        public string ProductionPlanDetail_Init_Database_CMD = @"SELECT * FROM [P_004_KeHoachSanXuatTheoTram] 
                                                      WHERE [LineID] = ''";
        private bool ProductionPlanDetail_Exist = false;
		private int ProductionPlanDetail_Index = 7;

        private bool P004_ProductionPlanDetail_Init()
        {
            if (ProductionPlanDetail_Exist == true)
            {
                if (tabControl1.TabPages.Contains(ProductionPlanDetail_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, ProductionPlanDetail_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("P004_PlanInDetail");
                return true;
            }
            ProductionPlanDetail_Exist = true;

            // Init_ProductionPlanDetail_Excel();
            ProductionPlanDetail_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "P004_PlanInDetail", ProductionPlanDetail_Index, MasterDatabase_Connection_Str, 
                                                            ProductionPlanDetail_Init_Database_CMD, ProductionPlanDetail_Select_CMD,
                                                            3, ProductionPlanDetail_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.Import_BT.Visible = false;
            ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.Review_BT.Visible = false;
            ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.Delete_Rows_BT.Visible = false;

            //Dho-Fixme: Do we need to use the button "Check_BT"?
            PosSize possize = new PosSize();
            possize.pos_x = 200;
            possize.pos_y = 90;
            ProductionPlanDetail_Create_BT = new Button_Lbl(1, ProductionPlanDetail_MasterDatabase.MasterDatabase_Tab, "Create", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            ProductionPlanDetail_Create_BT.My_Button.Click += new EventHandler(ProductionPlanDetail_Create_BT_Click);
            ProductionPlanDetail_Create_BT.My_Button.Visible = true;

            possize.pos_x = 300;
            possize.pos_y = 90;
            ProductionPlanDetail_Assign_BT = new Button_Lbl(1, ProductionPlanDetail_MasterDatabase.MasterDatabase_Tab, "Assign", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            ProductionPlanDetail_Assign_BT.My_Button.Click += new EventHandler(ProductionPlanDetail_Assign_BT_Click);
            ProductionPlanDetail_Assign_BT.My_Button.Visible = true;

            //Add column combobox employee

            ProductionPlanDetail_tbAllEmployee = Load_All_Empl();
            ProductionPlanDetail_tbAllEmployee.PrimaryKey = new DataColumn[] { ProductionPlanDetail_tbAllEmployee.Columns["Empl_ID"] };
            if (ProductionPlanDetail_tbAllEmployee.Columns.Contains("Cur_Line") == false)
            {
                ProductionPlanDetail_tbAllEmployee.Columns.Add("Cur_Line", typeof(String));
            }
            if (ProductionPlanDetail_tbAllEmployee.Columns.Contains("Cur_Shift") == false)
            {
                ProductionPlanDetail_tbAllEmployee.Columns.Add("Cur_Shift", typeof(String));
            }
            if (ProductionPlanDetail_tbAllEmployee.Columns.Contains("Date") == false)
            {
                ProductionPlanDetail_tbAllEmployee.Columns.Add("Date", typeof(DateTime));
            }
            DataGridViewMultiColumnComboBoxColumn col = new DataGridViewMultiColumnComboBoxColumn();
            col.Name = "Empl_ID";
            col.DataPropertyName = "Empl_ID";
            col.ValueMember = "Empl_ID";
            col.DataSource = ProductionPlanDetail_tbAllEmployee;
            col.ColumnWidths = new List<string>() { "55", "150", "60", "50", "65" };

            if (ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns.Contains("Empl_ID"))
            {
                int index = ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Empl_ID"].Index;
                ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns.RemoveAt(index);
                ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns.Insert(index, col);
                col.HeaderCell = new DataGridViewAutoFilterColumnHeaderCell(col.HeaderCell);
            }

            ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.GridView.CellValueChanged += new DataGridViewCellEventHandler(ProductionPlanDetail_MasterDatabase_GridView_CellValueChanged);
            ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.GridView.CellBeginEdit += new DataGridViewCellCancelEventHandler(ProductionPlanDetail_MasterDatabase_GridView_CellBeginEdit);

            //set role
            string moduleId = "P_004";
            RoleHelper.SetRole(ProductionPlanDetail_MasterDatabase, moduleId);
            ProductionPlanDetail_Create_BT.My_Button.Enabled = RoleHelper.GetCurrentUserLoginRole(UserRoleName.CREATE, moduleId);
            ProductionPlanDetail_Assign_BT.My_Button.Enabled = RoleHelper.GetCurrentUserLoginRole(UserRoleName.CREATE, moduleId);

            return true;
        }
        ExcelImportStruct[] ProductionPlanDetail_Excel_Struct;
        const int ProductionPlanDetail_INDEX = 0;

        private void Init_ProductionPlanDetail_Excel()
        {
            if (ProductionPlanDetail_Excel_Struct == null)
            {
                ProductionPlanDetail_Excel_Struct = new ExcelImportStruct[18];
                ProductionPlanDetail_Excel_Struct[0] = new ExcelImportStruct(0, "Date", "Date", Excel_Col_Type.COL_DATE, 20, true);
                ProductionPlanDetail_Excel_Struct[1] = new ExcelImportStruct(1, "ShiftName", "ShiftName", Excel_Col_Type.COL_STRING, 50, true);
                ProductionPlanDetail_Excel_Struct[2] = new ExcelImportStruct(2, "Shift_Percent", "Shift_Percent", Excel_Col_Type.COL_STRING, 20, false);
                ProductionPlanDetail_Excel_Struct[3] = new ExcelImportStruct(3, "LineID", "LineID", Excel_Col_Type.COL_STRING, 20, false);
                ProductionPlanDetail_Excel_Struct[4] = new ExcelImportStruct(4, "LineName", "LineName", Excel_Col_Type.COL_STRING, 20, false);
                ProductionPlanDetail_Excel_Struct[5] = new ExcelImportStruct(5, "SubLine_ID", "SubLine_ID", Excel_Col_Type.COL_STRING, 50, false);
                ProductionPlanDetail_Excel_Struct[6] = new ExcelImportStruct(6, "SubLine_Name", "SubLine_Name", Excel_Col_Type.COL_STRING, 50, false);
                ProductionPlanDetail_Excel_Struct[7] = new ExcelImportStruct(7, "WST_ID", "WST_ID", Excel_Col_Type.COL_STRING, 20, true);
                ProductionPlanDetail_Excel_Struct[8] = new ExcelImportStruct(8, "WST_Name", "WST_Name", Excel_Col_Type.COL_STRING, 50, false);
                ProductionPlanDetail_Excel_Struct[9] = new ExcelImportStruct(9, "PartNumber", "PartNumber", Excel_Col_Type.COL_STRING, 20, true);
                ProductionPlanDetail_Excel_Struct[10] = new ExcelImportStruct(10, "Empl_ID", "Empl_ID", Excel_Col_Type.COL_STRING, 20, false);
                ProductionPlanDetail_Excel_Struct[11] = new ExcelImportStruct(11, "Empl_Name", "Empl_Name", Excel_Col_Type.COL_STRING, 50, false);
                ProductionPlanDetail_Excel_Struct[12] = new ExcelImportStruct(12, "From_Time", "From_Time", Excel_Col_Type.COL_TIME, 20, false);
                ProductionPlanDetail_Excel_Struct[13] = new ExcelImportStruct(13, "To_Time", "To_Time", Excel_Col_Type.COL_TIME, 20, false);
                ProductionPlanDetail_Excel_Struct[14] = new ExcelImportStruct(14, "Capacity", "Capacity", Excel_Col_Type.COL_DECIMAL, 20, false);
                ProductionPlanDetail_Excel_Struct[15] = new ExcelImportStruct(15, "Qty", "Qty", Excel_Col_Type.COL_INT, 20, false);
                ProductionPlanDetail_Excel_Struct[16] = new ExcelImportStruct(16, "Total_Qty", "Total_Qty", Excel_Col_Type.COL_INT, 20, false);
                ProductionPlanDetail_Excel_Struct[17] = new ExcelImportStruct(17, "PO", "PO", Excel_Col_Type.COL_STRING, 20, true);
            }
        }
    }
}