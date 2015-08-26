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
        bool simulateMonday = false;
        readonly Color COLOR_LINE_NOT_HAVE_EMPLOYEE = Color.Red;
        MaterDatabase P_003_KeHoachSanXuatTheoLine_MasterDatabase;
        Button_Lbl PlanForLine_Create_BT;
        Button_Lbl PlanForLine_Empl_Asign_BT;
        Button_Lbl PlanForLine_Create_FRU_BT;
        Button_Lbl PlanForLine_DuplicateRow_BT;

        DataTable P_003_KeHoachSanXuatTheoLine_tbAllEmployee;
        public string P_003_KeHoachSanXuatTheoLine_Select_CMD = @"SELECT * FROM [P_003_KeHoachSanXuatTheoLine] ";
        public string P_003_KeHoachSanXuatTheoLine_Init_Database_CMD = @"SELECT * FROM [P_003_KeHoachSanXuatTheoLine] 
                                                      WHERE [Date] = ''";
        private bool P_003_KeHoachSanXuatTheoLine_Exist = false;

        ExcelImportStruct[] P_003_KeHoachSanXuatTheoLine_Excel_Struct;
        const int P_003_KeHoachSanXuatTheoLine_INDEX = 0;

        private void Init_P_003_KeHoachSanXuatTheoLine_Excel()
        {
            if (P_003_KeHoachSanXuatTheoLine_Excel_Struct == null)
            {
                P_003_KeHoachSanXuatTheoLine_Excel_Struct = new ExcelImportStruct[14];
                P_003_KeHoachSanXuatTheoLine_Excel_Struct[0] = new ExcelImportStruct(0, "Date", "Date", Excel_Col_Type.COL_DATE, 20, true);
                P_003_KeHoachSanXuatTheoLine_Excel_Struct[1] = new ExcelImportStruct(1, "LineID", "LineID", Excel_Col_Type.COL_STRING, 20, true);
                P_003_KeHoachSanXuatTheoLine_Excel_Struct[2] = new ExcelImportStruct(2, "WST_ID", "WST_ID", Excel_Col_Type.COL_STRING, 20, true);
                P_003_KeHoachSanXuatTheoLine_Excel_Struct[3] = new ExcelImportStruct(3, "ShiftName", "ShiftName", Excel_Col_Type.COL_STRING, 20, true);
                P_003_KeHoachSanXuatTheoLine_Excel_Struct[4] = new ExcelImportStruct(4, "Empl_ID", "Empl_ID", Excel_Col_Type.COL_STRING, 20, false);
                P_003_KeHoachSanXuatTheoLine_Excel_Struct[5] = new ExcelImportStruct(5, "Empl_Name", "Empl_Name", Excel_Col_Type.COL_STRING, 50, false);
                P_003_KeHoachSanXuatTheoLine_Excel_Struct[6] = new ExcelImportStruct(6, "LineName", "LineName", Excel_Col_Type.COL_STRING, 50, false);
                P_003_KeHoachSanXuatTheoLine_Excel_Struct[7] = new ExcelImportStruct(7, "SubLine_ID", "SubLine_ID", Excel_Col_Type.COL_STRING, 20, false);
                P_003_KeHoachSanXuatTheoLine_Excel_Struct[8] = new ExcelImportStruct(8, "SubLine_Name", "SubLine_Name", Excel_Col_Type.COL_STRING, 50, false);
                P_003_KeHoachSanXuatTheoLine_Excel_Struct[9] = new ExcelImportStruct(9, "Main_Part", "Main_Part", Excel_Col_Type.COL_STRING, 50, false);
                P_003_KeHoachSanXuatTheoLine_Excel_Struct[10] = new ExcelImportStruct(10, "WST_Name", "WST_Name", Excel_Col_Type.COL_STRING, 50, false);
                P_003_KeHoachSanXuatTheoLine_Excel_Struct[11] = new ExcelImportStruct(11, "From_Time", "From_Time", Excel_Col_Type.COL_TIME, 50, false);
                P_003_KeHoachSanXuatTheoLine_Excel_Struct[12] = new ExcelImportStruct(12, "To_Time", "To_Time", Excel_Col_Type.COL_TIME, 50, false);
                P_003_KeHoachSanXuatTheoLine_Excel_Struct[13] = new ExcelImportStruct(13, "Reason", "Reason", Excel_Col_Type.COL_STRING, 100, false);
            }
        }

        private bool P003_AssignEmpl_Init()
        {
            if (P_003_KeHoachSanXuatTheoLine_Exist == true)
            {
                if (tabControl1.TabPages.Contains(P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("P003_AssignEmployee");
                return true;
            }

            if (Running_Mode == Run_Mode.DEBUG)
            {
                P_003_KeHoachSanXuatTheoLine_Select_CMD = @"SELECT * FROM [P_003_KeHoachSanXuatTheoLine_Test] ";
                P_003_KeHoachSanXuatTheoLine_Init_Database_CMD = @"SELECT * FROM [P_003_KeHoachSanXuatTheoLine_Test] 
                                                       WHERE [Date] = ''";
            }
            P_003_KeHoachSanXuatTheoLine_Exist = true;
            Init_P_003_KeHoachSanXuatTheoLine_Excel();
            P_003_KeHoachSanXuatTheoLine_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "P003_AssignEmployee", P_003_KeHoachSanXuatTheoLine_INDEX, MasterDatabase_Connection_Str,
                                                            P_003_KeHoachSanXuatTheoLine_Init_Database_CMD, P_003_KeHoachSanXuatTheoLine_Select_CMD,
                                                            3, P_003_KeHoachSanXuatTheoLine_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);

            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Import_BT.Visible = true;
            P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Delete_All_BT.Visible = false;
            P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Privot_BT.Visible = true;

            //Dho-Fixme: Do we need to use the button "Check_BT"?
            PosSize possize = new PosSize();
            possize.pos_x = 200;
            possize.pos_y = 90;
            PlanForLine_Create_BT = new Button_Lbl(1, P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_Tab, "Create", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            PlanForLine_Create_BT.My_Button.Click += new EventHandler(PlanForLine_Create_BT_Click);

            possize.pos_x = 300;
            possize.pos_y = 90;
            PlanForLine_Empl_Asign_BT = new Button_Lbl(2, P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_Tab, "Add FRU", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            PlanForLine_Empl_Asign_BT.My_Button.Click += new EventHandler(P_003_KeHoachSanXuatTheoLine_AddFRU_BT_Click);

            possize.pos_x = 400;
            possize.pos_y = 90;
            PlanForLine_Create_FRU_BT = new Button_Lbl(2, P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_Tab, "Assign", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            PlanForLine_Create_FRU_BT.My_Button.Click += new EventHandler(PlanForLine_Empl_Asign_BT_Click);

            possize.pos_x = 700;
            possize.pos_y = 90;
            PlanForLine_DuplicateRow_BT = new Button_Lbl(3, P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_Tab, "Duplicate Current Row", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            PlanForLine_DuplicateRow_BT.My_Button.Click += new EventHandler(PlanForLine_DuplicateRow_BT_Click);


            //Chỉ cần load danh sách nhân viên available ?
            //DataTable leave_info = Load_Leave_Register(date);
            P_003_KeHoachSanXuatTheoLine_tbAllEmployee = Load_All_Empl();
            P_003_KeHoachSanXuatTheoLine_tbAllEmployee.PrimaryKey = new DataColumn[] { P_003_KeHoachSanXuatTheoLine_tbAllEmployee.Columns["Empl_ID"] };

            if (P_003_KeHoachSanXuatTheoLine_tbAllEmployee.Columns.Contains("Cur_Line") == false)
            {
                P_003_KeHoachSanXuatTheoLine_tbAllEmployee.Columns.Add("Cur_Line", typeof(String));
            }
            if (P_003_KeHoachSanXuatTheoLine_tbAllEmployee.Columns.Contains("Cur_Shift") == false)
            {
                P_003_KeHoachSanXuatTheoLine_tbAllEmployee.Columns.Add("Cur_Shift", typeof(String));
            }
            if (P_003_KeHoachSanXuatTheoLine_tbAllEmployee.Columns.Contains("Date") == false)
            {
                P_003_KeHoachSanXuatTheoLine_tbAllEmployee.Columns.Add("Date", typeof(DateTime));
            }
            //P_003_KeHoachSanXuatTheoLine_tbAllEmployee.Columns.Add("Cur_Line", typeof(String));
            //P_003_KeHoachSanXuatTheoLine_tbAllEmployee.Columns.Add("Cur_Shift", typeof(String));
            //P_003_KeHoachSanXuatTheoLine_tbAllEmployee.Columns.Add("Date", typeof(DateTime));
            DataGridViewMultiColumnComboBoxColumn col = new DataGridViewMultiColumnComboBoxColumn();
            col.Name = "Empl_ID";
            col.DataPropertyName = "Empl_ID";
            col.ValueMember = "Empl_ID";
            col.DataSource = P_003_KeHoachSanXuatTheoLine_tbAllEmployee;
            col.ColumnWidths = new List<string>() { "55", "150", "60", "50", "65" };

            if (P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns.Contains("Empl_ID"))
            {
                int index = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Empl_ID"].Index;
                P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns.RemoveAt(index);
                P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns.Insert(index, col);
                col.HeaderCell = new DataGridViewAutoFilterColumnHeaderCell(col.HeaderCell);
            }

            P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.CellValueChanged += new DataGridViewCellEventHandler(P_003_KeHoachSanXuatTheoLine_MasterDatabase_GridView_CellValueChanged);
            P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(P_003_KeHoachSanXuatTheoLine_MasterDatabase_GridView_DataBindingComplete);
            //P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.DataSourceChanged += new EventHandler(P_003_KeHoachSanXuatTheoLine_MasterDatabase_GridView_DataSourceChanged);
            P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.CellBeginEdit += new DataGridViewCellCancelEventHandler(P_003_KeHoachSanXuatTheoLine_MasterDatabase_GridView_CellBeginEdit);

            //set role
            string moduleId = "P_003";
            RoleHelper.SetRole(P_003_KeHoachSanXuatTheoLine_MasterDatabase, moduleId);
            PlanForLine_Create_BT.My_Button.Enabled = RoleHelper.GetCurrentUserLoginRole(UserRoleName.CREATE, moduleId);
            PlanForLine_Empl_Asign_BT.My_Button.Enabled = RoleHelper.GetCurrentUserLoginRole(UserRoleName.CREATE, moduleId);
            PlanForLine_Create_FRU_BT.My_Button.Enabled = RoleHelper.GetCurrentUserLoginRole(UserRoleName.CREATE, moduleId);

            return true;

        }


        private bool Assign_Empl_for_LinePlan(DateTime date)
        {
            bool AssignJobForLineLeaderEmployee = false;
            DataTable LeadEmployeeTble = null;
            DataTable availabelTble = null;
            DataTable leave_info = Load_Leave_Register(date);
            DataTable prioTbl = null;
            DataTable HistiryTbl;
            string retVal;
            string empl, wst, line;
            bool ut3_Check_Skill;
            string load_database_str;

            if (Running_Mode == Run_Mode.RELEASE)
            {
                load_database_str = @"SELECT * FROM [P_003_KeHoachSanXuatTheoLine] 
                                        WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            }
            else
            {
                load_database_str = @"SELECT * FROM [P_003_KeHoachSanXuatTheoLine_Test] 
                                        WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            }

            // lay du lieu ke hoach sx theo line cua ngay da chon
            P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, load_database_str);
            DataTable PlanLine_WST_Plan = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb;

            //Clean up the reason colum
            string AssignedEmpl_ID = string.Empty;
            foreach (DataRow row in PlanLine_WST_Plan.Rows)
            {
                AssignedEmpl_ID = row["Empl_ID"].ToString().Trim();

                //Clean up only if wst have not been assigned
                if (AssignedEmpl_ID == string.Empty)
                {
                    row["Empl_Name"] = "";
                    row["Reason"] = "";
                }
            }

            if ((PlanLine_WST_Plan == null) || (PlanLine_WST_Plan.Rows.Count == 0))
            {
                MessageBox.Show("No task to arrange for Employee", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //Add another column to show the information why we assign imployee
            AddColumnToTable(ref PlanLine_WST_Plan, "Reason", ""); //Mặc định là sắp xếp theo skill

            MSSqlDbFactory dao = new MSSqlDbFactory();
            //Todo: Load the real list of availabel employee instead of the list below
            const string availabelList = "SELECT distinct [Empl_ID],[Empl_Name] FROM MDB_002_Empl_Skill";
            retVal = dao.OpenDataTable(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, ref availabelTble, CommandType.Text, availabelList);

            //Kien: Remove Empl have leave ==> Dinh Check Giup nha
            RemoveLeadEmployee(ref availabelTble, leave_info);

            //Taọ danh sách dữ liệu dành cho việc ưu tiên sắp xếp, vd: history list, fix position list.
            //Chương trình sẽ căn cứ vào những danh sách này và sắp xếp trước một số record nếu thỏa mãn điều kiện.
            //Sau đó sẽ chạy phần sắp xếp thông thường dựa trên skill...
            List<DataTable> prioTblList = new List<DataTable>();

            if (prioTbl != null && prioTbl.Rows.Count > 0)
            {
                AddColumnToTable(ref prioTbl, "TableName", "Priority");
                prioTblList.Add(prioTbl);
            }

            HistiryTbl = GetTrackingHistory(date);
            if (HistiryTbl != null && HistiryTbl.Rows.Count > 0)
            {
                AddColumnToTable(ref HistiryTbl, "TableName", "History");
                prioTblList.Add(HistiryTbl);
            }

            if (MessageBox.Show("Check Skill in UT_3", "Thông Báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ut3_Check_Skill = true;
            }
            else
            {
                ut3_Check_Skill = false;
            }



            //Todo: Build the data for employee assignment; 
            //The required column for the EmployeeAssignment_ProrityTable.Data  is in EmployeeAssignment_ProrityTableColumn.cs
            List<EmployeeAssignment_ProrityTable> PriorityTableList = new List<EmployeeAssignment_ProrityTable>();

            EmployeeAssignment_ProrityTable priorityTbl_1 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 0.5. Lead ở line cố định
            priorityTbl_1.ID = "UT_01";
            priorityTbl_1.Name = "Ưu tiên 01. Lead ở line cố định";
            priorityTbl_1.Data = Get_LeadFixposition();
            foreach (DataRow leadrow in priorityTbl_1.Data.Rows)
            {
                empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                leadrow[ProrityTableCollumn.IS_STAND] = false;
                leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = false;
                leadrow[ProrityTableCollumn.IS_CHECK_WST_COLUMN] = true;
            }
            if (priorityTbl_1.Data != null && priorityTbl_1.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_1);

            EmployeeAssignment_ProrityTable priorityTbl_1_1 = new EmployeeAssignment_ProrityTable(); //Ưu tiên bầu ở wst cố định,ca cố định
            priorityTbl_1_1.ID = "UT_01_TS_1";
            priorityTbl_1_1.Name = "Ưu tiên 01_TS_1. bầu ở wst cố định, ca cố định";
            priorityTbl_1_1.Data = Get_TS_List();
            foreach (DataRow row in priorityTbl_1_1.Data.Rows)
            {
                row[ProrityTableCollumn.IS_CHECK_WST_COLUMN] = true;
                row[ProrityTableCollumn.SHIFT_COLUMN] = "Shift_1";
            }
            if (priorityTbl_1_1.Data != null && priorityTbl_1_1.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_1_1);

            if (simulateMonday || (date.DayOfWeek == DayOfWeek.Monday))
            {
                EmployeeAssignment_ProrityTable priorityTbl_1_K = new EmployeeAssignment_ProrityTable(); //Ưu tiên 0.5. Lead ở line cố định
                priorityTbl_1_K.ID = "UT_01_K";
                priorityTbl_1_K.Name = "Ưu tiên 01.K Lead ở line cố định";
                priorityTbl_1_K.Data = Get_LeadFixposition();
                foreach (DataRow leadrow in priorityTbl_1_K.Data.Rows)
                {
                    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                    leadrow[ProrityTableCollumn.IS_STAND] = false;
                    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
                    leadrow[ProrityTableCollumn.IS_CHECK_WST_COLUMN] = true;
                    leadrow[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN] = true;
                }
                if (priorityTbl_1_K.Data != null && priorityTbl_1_K.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_1_K);
            }

            EmployeeAssignment_ProrityTable priorityTbl_1_5 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 0.5. Lead ở line cố định
            priorityTbl_1_5.ID = "UT_01.5";
            priorityTbl_1_5.Name = "Ưu tiên 01.5. Nhân Viên ở WST cố định";
            priorityTbl_1_5.Data = Get_Fixposition_WST();
            foreach (DataRow leadrow in priorityTbl_1_5.Data.Rows)
            {
                empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                leadrow[ProrityTableCollumn.IS_STAND] = false;
                leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = false;
                leadrow[ProrityTableCollumn.IS_CHECK_WST_COLUMN] = true;
            }
            if (priorityTbl_1_5.Data != null && priorityTbl_1_5.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_1_5);

            if (simulateMonday || (date.DayOfWeek == DayOfWeek.Monday))
            {
                EmployeeAssignment_ProrityTable priorityTbl_1_5_K = new EmployeeAssignment_ProrityTable(); //Ưu tiên 0.5. Lead ở line cố định
                priorityTbl_1_5_K.ID = "UT_01_K";
                priorityTbl_1_5_K.Name = "Ưu tiên 01.5.K Nhân Viên ở WST cố định";
                priorityTbl_1_5_K.Data = Get_Fixposition_WST();
                foreach (DataRow leadrow in priorityTbl_1_5_K.Data.Rows)
                {
                    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                    leadrow[ProrityTableCollumn.IS_STAND] = false;
                    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
                    leadrow[ProrityTableCollumn.IS_CHECK_WST_COLUMN] = true;
                    leadrow[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN] = true;
                }
                if (priorityTbl_1_5_K.Data != null && priorityTbl_1_5_K.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_1_5_K);
            }


            // Trong tuan: UT 3 chay truoc UT2
            if (date.DayOfWeek != DayOfWeek.Monday)
            {
                EmployeeAssignment_ProrityTable priorityTbl_3 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 1    History của WST ngày trước và ca
                priorityTbl_3.ID = "UT_03";
                priorityTbl_3.Name = "Ưu tiên 03.  History của WST ngày trước và ca";
                priorityTbl_3.Data = GetTrackingHistory(date);
                foreach (DataRow leadrow in priorityTbl_3.Data.Rows)
                {
                    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                    leadrow[ProrityTableCollumn.IS_STAND] = false;
                    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = ut3_Check_Skill;
                    leadrow[ProrityTableCollumn.IS_CHECK_WST_COLUMN] = true;
                }
                if (priorityTbl_3.Data != null && priorityTbl_3.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_3);

                if (simulateMonday || (date.DayOfWeek == DayOfWeek.Monday))
                {
                    EmployeeAssignment_ProrityTable priorityTbl_3_K = new EmployeeAssignment_ProrityTable(); //Ưu tiên 1    History của WST ngày trước và ca
                    priorityTbl_3_K.ID = "UT_03.K";
                    priorityTbl_3_K.Name = "Ưu tiên 03.K  History của WST ngày trước và ca";
                    priorityTbl_3_K.Data = GetTrackingHistory(date);
                    foreach (DataRow leadrow in priorityTbl_3_K.Data.Rows)
                    {
                        empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                        leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                        leadrow[ProrityTableCollumn.IS_STAND] = false;
                        leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = ut3_Check_Skill;
                        leadrow[ProrityTableCollumn.IS_CHECK_WST_COLUMN] = true;
                        leadrow[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN] = true;
                    }
                    if (priorityTbl_3_K.Data != null && priorityTbl_3_K.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_3_K);
                }



                EmployeeAssignment_ProrityTable priorityTbl_3_1 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 1    History của WST ngày trước và ca
                priorityTbl_3_1.ID = "UT_03.1";
                priorityTbl_3_1.Name = "Ưu tiên 03_1. History của Line ngày trước và ca";
                priorityTbl_3_1.Data = GetTrackingHistory(date);
                foreach (DataRow leadrow in priorityTbl_3_1.Data.Rows)
                {
                    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                    leadrow[ProrityTableCollumn.IS_STAND] = false;
                    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = ut3_Check_Skill;
                    leadrow[ProrityTableCollumn.IS_CHECK_LINE] = true;
                }
                if (priorityTbl_3_1.Data != null && priorityTbl_3_1.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_3_1);

                if (simulateMonday || (date.DayOfWeek == DayOfWeek.Monday))
                {
                    EmployeeAssignment_ProrityTable priorityTbl_3_1_K = new EmployeeAssignment_ProrityTable(); //Ưu tiên 1    History của WST ngày trước và ca
                    priorityTbl_3_1_K.ID = "UT_03.1.K";
                    priorityTbl_3_1_K.Name = "Ưu tiên 03_1.K History của Line ngày trước và ca";
                    priorityTbl_3_1_K.Data = GetTrackingHistory(date);
                    foreach (DataRow leadrow in priorityTbl_3_1_K.Data.Rows)
                    {
                        empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                        leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                        leadrow[ProrityTableCollumn.IS_STAND] = false;
                        leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = ut3_Check_Skill;
                        leadrow[ProrityTableCollumn.IS_CHECK_LINE] = true;
                        leadrow[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN] = true;
                    }
                    if (priorityTbl_3_1_K.Data != null && priorityTbl_3_1_K.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_3_1_K);
                }

                EmployeeAssignment_ProrityTable priorityTbl_3_2 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 1    History của WST ngày trước và ca
                priorityTbl_3_2.ID = "UT_03.2";
                priorityTbl_3_2.Name = "Ưu tiên 03_2. History của Line ngày trước và ca + Hoán Người";
                priorityTbl_3_2.Data = GetTrackingHistory(date);
                foreach (DataRow leadrow in priorityTbl_3_2.Data.Rows)
                {
                    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                    leadrow[ProrityTableCollumn.IS_STAND] = false;
                    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = ut3_Check_Skill;
                    leadrow[ProrityTableCollumn.IS_CHECK_LINE] = true;
                    leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = true;
                }
                if (priorityTbl_3_2.Data != null && priorityTbl_3_2.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_3_2);

                if (simulateMonday || (date.DayOfWeek == DayOfWeek.Monday))
                {
                    EmployeeAssignment_ProrityTable priorityTbl_3_2_K = new EmployeeAssignment_ProrityTable(); //Ưu tiên 1    History của WST ngày trước và ca
                    priorityTbl_3_2_K.ID = "UT_03.2.K";
                    priorityTbl_3_2_K.Name = "Ưu tiên 03_2.K History của Line ngày trước và ca + Hoán Người";
                    priorityTbl_3_2_K.Data = GetTrackingHistory(date);
                    foreach (DataRow leadrow in priorityTbl_3_2_K.Data.Rows)
                    {
                        empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                        leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                        leadrow[ProrityTableCollumn.IS_STAND] = false;
                        leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = ut3_Check_Skill;
                        leadrow[ProrityTableCollumn.IS_CHECK_LINE] = true;
                        leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = true;
                        leadrow[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN] = true;
                    }
                    if (priorityTbl_3_2_K.Data != null && priorityTbl_3_2_K.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_3_2_K);
                }
            }

            EmployeeAssignment_ProrityTable priorityTbl_2 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 0.0. Fix position cho nhân viên ngày đầu tuần
            priorityTbl_2.ID = "UT_02";
            priorityTbl_2.Name = "Ưu tiên 02. Nhân viên chính của line";
            priorityTbl_2.Data = Get_Fixposition();
            foreach (DataRow leadrow in priorityTbl_2.Data.Rows)
            {
                empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                leadrow[ProrityTableCollumn.IS_STAND] = false;
                leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
                leadrow[ProrityTableCollumn.IS_CHECK_LINE] = true;
            }
            if (priorityTbl_2.Data != null && priorityTbl_2.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_2);

            if (simulateMonday || (date.DayOfWeek == DayOfWeek.Monday))
            {
                EmployeeAssignment_ProrityTable priorityTbl_2_K = new EmployeeAssignment_ProrityTable(); //Ưu tiên 0.0. Fix position cho nhân viên ngày đầu tuần
                priorityTbl_2_K.ID = "UT_02_K";
                priorityTbl_2_K.Name = "Ưu tiên 02.K Nhân viên chính của line";
                priorityTbl_2_K.Data = Get_Fixposition();
                foreach (DataRow leadrow in priorityTbl_2_K.Data.Rows)
                {
                    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                    leadrow[ProrityTableCollumn.IS_STAND] = false;
                    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
                    leadrow[ProrityTableCollumn.IS_CHECK_LINE] = true;
                    leadrow[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN] = true;
                }
                if (priorityTbl_2_K.Data != null && priorityTbl_2_K.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_2_K);
            }

            EmployeeAssignment_ProrityTable priorityTbl_2_1 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 0.0. Fix position cho nhân viên ngày đầu tuần
            priorityTbl_2_1.ID = "UT_02_1";
            priorityTbl_2_1.Name = "Ưu tiên 02.1. Nhân viên chính của line + Hoán Người";
            priorityTbl_2_1.Data = Get_Fixposition();
            foreach (DataRow leadrow in priorityTbl_2_1.Data.Rows)
            {
                empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                leadrow[ProrityTableCollumn.IS_STAND] = false;
                leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
                leadrow[ProrityTableCollumn.IS_CHECK_LINE] = true;
                leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = true;
            }
            if (priorityTbl_2_1.Data != null && priorityTbl_2_1.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_2_1);

            if (simulateMonday || (date.DayOfWeek == DayOfWeek.Monday))
            {
                EmployeeAssignment_ProrityTable priorityTbl_2_1_K = new EmployeeAssignment_ProrityTable(); //Ưu tiên 0.0. Fix position cho nhân viên ngày đầu tuần
                priorityTbl_2_1_K.ID = "UT_02_1_K";
                priorityTbl_2_1_K.Name = "Ưu tiên 02.1.K Nhân viên chính của line + Hoán Người";
                priorityTbl_2_1_K.Data = Get_Fixposition();
                foreach (DataRow leadrow in priorityTbl_2_1_K.Data.Rows)
                {
                    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                    leadrow[ProrityTableCollumn.IS_STAND] = false;
                    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
                    leadrow[ProrityTableCollumn.IS_CHECK_LINE] = true;
                    leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = true;
                    leadrow[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN] = true;
                }
                if (priorityTbl_2_1_K.Data != null && priorityTbl_2_1_K.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_2_1_K);
            }

            if (date.DayOfWeek == DayOfWeek.Monday)
            {
                EmployeeAssignment_ProrityTable priorityTbl_3 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 1    History của WST ngày trước và ca
                priorityTbl_3.ID = "UT_03";
                priorityTbl_3.Name = "Ưu tiên 03.  History của WST ngày trước và ca";
                priorityTbl_3.Data = GetTrackingHistory(date);
                foreach (DataRow leadrow in priorityTbl_3.Data.Rows)
                {
                    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                    leadrow[ProrityTableCollumn.IS_STAND] = false;
                    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = ut3_Check_Skill;
                    leadrow[ProrityTableCollumn.IS_CHECK_WST_COLUMN] = true;
                }
                if (priorityTbl_3.Data != null && priorityTbl_3.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_3);

                if (simulateMonday || (date.DayOfWeek == DayOfWeek.Monday))
                {
                    EmployeeAssignment_ProrityTable priorityTbl_3_K = new EmployeeAssignment_ProrityTable(); //Ưu tiên 1    History của WST ngày trước và ca
                    priorityTbl_3_K.ID = "UT_03.K";
                    priorityTbl_3_K.Name = "Ưu tiên 03.K  History của WST ngày trước và ca";
                    priorityTbl_3_K.Data = GetTrackingHistory(date);
                    foreach (DataRow leadrow in priorityTbl_3_K.Data.Rows)
                    {
                        empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                        leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                        leadrow[ProrityTableCollumn.IS_STAND] = false;
                        leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
                        leadrow[ProrityTableCollumn.IS_CHECK_WST_COLUMN] = ut3_Check_Skill;
                        leadrow[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN] = true;
                    }
                    if (priorityTbl_3_K.Data != null && priorityTbl_3_K.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_3_K);
                }

                EmployeeAssignment_ProrityTable priorityTbl_3_1 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 1    History của WST ngày trước và ca
                priorityTbl_3_1.ID = "UT_03.1";
                priorityTbl_3_1.Name = "Ưu tiên 03_1. History của Line ngày trước và ca";
                priorityTbl_3_1.Data = GetTrackingHistory(date);
                foreach (DataRow leadrow in priorityTbl_3_1.Data.Rows)
                {
                    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                    leadrow[ProrityTableCollumn.IS_STAND] = false;
                    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = ut3_Check_Skill;
                    leadrow[ProrityTableCollumn.IS_CHECK_LINE] = true;
                }
                if (priorityTbl_3_1.Data != null && priorityTbl_3_1.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_3_1);

                if (simulateMonday || (date.DayOfWeek == DayOfWeek.Monday))
                {
                    EmployeeAssignment_ProrityTable priorityTbl_3_1_K = new EmployeeAssignment_ProrityTable(); //Ưu tiên 1    History của WST ngày trước và ca
                    priorityTbl_3_1_K.ID = "UT_03.1.K";
                    priorityTbl_3_1_K.Name = "Ưu tiên 03_1.K History của Line ngày trước và ca";
                    priorityTbl_3_1_K.Data = GetTrackingHistory(date);
                    foreach (DataRow leadrow in priorityTbl_3_1_K.Data.Rows)
                    {
                        empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                        leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                        leadrow[ProrityTableCollumn.IS_STAND] = false;
                        leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = ut3_Check_Skill;
                        leadrow[ProrityTableCollumn.IS_CHECK_LINE] = true;
                        leadrow[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN] = true;
                    }
                    if (priorityTbl_3_1_K.Data != null && priorityTbl_3_1_K.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_3_1_K);
                }

                EmployeeAssignment_ProrityTable priorityTbl_3_2 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 1    History của WST ngày trước và ca
                priorityTbl_3_2.ID = "UT_03.2";
                priorityTbl_3_2.Name = "Ưu tiên 03_2. History của Line ngày trước và ca + Hoán Người";
                priorityTbl_3_2.Data = GetTrackingHistory(date);
                foreach (DataRow leadrow in priorityTbl_3_2.Data.Rows)
                {
                    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                    leadrow[ProrityTableCollumn.IS_STAND] = false;
                    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = ut3_Check_Skill;
                    leadrow[ProrityTableCollumn.IS_CHECK_LINE] = true;
                    leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = true;
                }
                if (priorityTbl_3_2.Data != null && priorityTbl_3_2.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_3_2);

                if (simulateMonday || (date.DayOfWeek == DayOfWeek.Monday))
                {
                    EmployeeAssignment_ProrityTable priorityTbl_3_2_K = new EmployeeAssignment_ProrityTable(); //Ưu tiên 1    History của WST ngày trước và ca
                    priorityTbl_3_2_K.ID = "UT_03.2.K";
                    priorityTbl_3_2_K.Name = "Ưu tiên 03_2.K History của Line ngày trước và ca + Hoán Người";
                    priorityTbl_3_2_K.Data = GetTrackingHistory(date);
                    foreach (DataRow leadrow in priorityTbl_3_2_K.Data.Rows)
                    {
                        empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                        leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                        leadrow[ProrityTableCollumn.IS_STAND] = false;
                        leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = ut3_Check_Skill;
                        leadrow[ProrityTableCollumn.IS_CHECK_LINE] = true;
                        leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = true;
                        leadrow[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN] = true;
                    }
                    if (priorityTbl_3_2_K.Data != null && priorityTbl_3_2_K.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_3_2_K);
                }
            }


            EmployeeAssignment_ProrityTable priorityTbl_4 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 1    History của WST ngày trước và ca
            priorityTbl_4.ID = "UT_04";
            priorityTbl_4.Name = "Ưu tiên 04. Lịch sử đi ca + Nhóm sản Phẩm(Theo line hiện tại)";
            priorityTbl_4.Data = GetTrackingHistory(date);
            foreach (DataRow leadrow in priorityTbl_4.Data.Rows)
            {
                empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                line = leadrow[ProrityTableCollumn.LINE_COLUMN].ToString().Trim();
                leadrow[ProrityTableCollumn.GROUP_COLUMN] = Get_GroupofLine(line);
                leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                leadrow[ProrityTableCollumn.IS_STAND] = false;
                leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
                leadrow[ProrityTableCollumn.IS_CHECK_GROUP_COLUMN] = true;
                leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = false;
            }
            if (priorityTbl_4.Data != null && priorityTbl_4.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_4);

            if (simulateMonday || (date.DayOfWeek == DayOfWeek.Monday))
            {
                EmployeeAssignment_ProrityTable priorityTbl_4_K = new EmployeeAssignment_ProrityTable(); //Ưu tiên 1    History của WST ngày trước và ca
                priorityTbl_4_K.ID = "UT_04.K";
                priorityTbl_4_K.Name = "Ưu tiên 04. Lịch sử đi ca + Nhóm sản Phẩm(Theo line hiện tại)";
                priorityTbl_4_K.Data = GetTrackingHistory(date);
                foreach (DataRow leadrow in priorityTbl_4_K.Data.Rows)
                {
                    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                    line = leadrow[ProrityTableCollumn.LINE_COLUMN].ToString().Trim();
                    leadrow[ProrityTableCollumn.GROUP_COLUMN] = Get_GroupofLine(line);
                    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                    leadrow[ProrityTableCollumn.IS_STAND] = false;
                    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
                    leadrow[ProrityTableCollumn.IS_CHECK_GROUP_COLUMN] = true;
                    leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = false;
                    leadrow[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN] = true;
                }
                if (priorityTbl_4_K.Data != null && priorityTbl_4_K.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_4_K);
            }



            //EmployeeAssignment_ProrityTable priorityTbl_4_1 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 1    History của WST ngày trước và ca
            //priorityTbl_4_1.ID = "UT_04_1";
            //priorityTbl_4_1.Name = "Ưu tiên 04.1. Lịch sử đi ca + Nhóm sản Phẩm(Theo line hiện tại) + Hoán Người";
            //priorityTbl_4_1.Data = GetTrackingHistory(date);
            //foreach (DataRow leadrow in priorityTbl_4_1.Data.Rows)
            //{
            //    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
            //    line = leadrow[ProrityTableCollumn.LINE_COLUMN].ToString().Trim();
            //    leadrow[ProrityTableCollumn.GROUP_COLUMN] = Get_GroupofLine(line);
            //    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
            //    leadrow[ProrityTableCollumn.IS_STAND] = false;
            //    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
            //    leadrow[ProrityTableCollumn.IS_CHECK_GROUP_COLUMN] = true;
            //    leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = true;
            //}
            //if (priorityTbl_4_1.Data != null && priorityTbl_4_1.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_4_1);

            //if (simulateMonday || (date.DayOfWeek == DayOfWeek.Monday))
            //{
            //    EmployeeAssignment_ProrityTable priorityTbl_4_1_K = new EmployeeAssignment_ProrityTable(); //Ưu tiên 1    History của WST ngày trước và ca
            //    priorityTbl_4_1_K.ID = "UT_04_1.K";
            //    priorityTbl_4_1_K.Name = "Ưu tiên 04.1.K Lịch sử đi ca + Nhóm sản Phẩm(Theo line hiện tại) + Hoán Người";
            //    priorityTbl_4_1_K.Data = GetTrackingHistory(date);
            //    foreach (DataRow leadrow in priorityTbl_4_1_K.Data.Rows)
            //    {
            //        empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
            //        line = leadrow[ProrityTableCollumn.LINE_COLUMN].ToString().Trim();
            //        leadrow[ProrityTableCollumn.GROUP_COLUMN] = Get_GroupofLine(line);
            //        leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
            //        leadrow[ProrityTableCollumn.IS_STAND] = false;
            //        leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
            //        leadrow[ProrityTableCollumn.IS_CHECK_GROUP_COLUMN] = true;
            //        leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = true;
            //        leadrow[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN] = true;
            //    }
            //    if (priorityTbl_4_1_K.Data != null && priorityTbl_4_1_K.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_4_1_K);
            //}

            EmployeeAssignment_ProrityTable priorityTbl_4_2 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 2    Skill + Lịch sử đi ca + Ưu tiên cùng nhóm sản Phẩm: HHS, MOB, FRS, FFC
            priorityTbl_4_2.ID = "UT_04.2";
            priorityTbl_4_2.Name = "Ưu tiên 04_2.  Skill + Lịch sử đi ca + Ưu tiên cùng nhóm sản Phẩm(Theo Profile)";
            priorityTbl_4_2.Data = Get_Empl_Skill_Group_List();
            foreach (DataRow leadrow in priorityTbl_4_2.Data.Rows)
            {
                empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                leadrow[ProrityTableCollumn.IS_STAND] = false;
                leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
                leadrow[ProrityTableCollumn.IS_CHECK_GROUP_COLUMN] = true;
                leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = false;
            }
            if (priorityTbl_4_2.Data != null && priorityTbl_4_2.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_4_2);

            if (simulateMonday || (date.DayOfWeek == DayOfWeek.Monday))
            {
                EmployeeAssignment_ProrityTable priorityTbl_4_2_K = new EmployeeAssignment_ProrityTable(); //Ưu tiên 2    Skill + Lịch sử đi ca + Ưu tiên cùng nhóm sản Phẩm: HHS, MOB, FRS, FFC
                priorityTbl_4_2_K.ID = "UT_04.2.K";
                priorityTbl_4_2_K.Name = "Ưu tiên 04_2.K  Skill + Lịch sử đi ca + Ưu tiên cùng nhóm sản Phẩm(Theo Profile)";
                priorityTbl_4_2_K.Data = Get_Empl_Skill_Group_List();
                foreach (DataRow leadrow in priorityTbl_4_2_K.Data.Rows)
                {
                    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                    leadrow[ProrityTableCollumn.IS_STAND] = false;
                    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
                    leadrow[ProrityTableCollumn.IS_CHECK_GROUP_COLUMN] = true;
                    leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = false;
                    leadrow[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN] = true;
                }
                if (priorityTbl_4_2_K.Data != null && priorityTbl_4_2_K.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_4_2_K);
            }

            //EmployeeAssignment_ProrityTable priorityTbl_4_3 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 2    Skill + Lịch sử đi ca + Ưu tiên cùng nhóm sản Phẩm: HHS, MOB, FRS, FFC
            //priorityTbl_4_3.ID = "UT_04.3";
            //priorityTbl_4_3.Name = "Ưu tiên 04_3.  Skill + Lịch sử đi ca + Ưu tiên cùng nhóm sản Phẩm(Theo Profile) + Hoán Người";
            //priorityTbl_4_3.Data = Get_Empl_Skill_Group_List();
            //foreach (DataRow leadrow in priorityTbl_4_3.Data.Rows)
            //{
            //    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
            //    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
            //    leadrow[ProrityTableCollumn.IS_STAND] = false;
            //    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
            //    leadrow[ProrityTableCollumn.IS_CHECK_GROUP_COLUMN] = true;
            //    leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = true;
            //}
            //if (priorityTbl_4_3.Data != null && priorityTbl_4_3.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_4_3);

            //if (simulateMonday || (date.DayOfWeek == DayOfWeek.Monday))
            //{
            //    EmployeeAssignment_ProrityTable priorityTbl_4_3_K = new EmployeeAssignment_ProrityTable(); //Ưu tiên 2    Skill + Lịch sử đi ca + Ưu tiên cùng nhóm sản Phẩm: HHS, MOB, FRS, FFC
            //    priorityTbl_4_3_K.ID = "UT_04.3.K";
            //    priorityTbl_4_3_K.Name = "Ưu tiên 04_3.K  Skill + Lịch sử đi ca + Ưu tiên cùng nhóm sản Phẩm(Theo Profile) + Hoán Người";
            //    priorityTbl_4_3_K.Data = Get_Empl_Skill_Group_List();
            //    foreach (DataRow leadrow in priorityTbl_4_3_K.Data.Rows)
            //    {
            //        empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
            //        leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
            //        leadrow[ProrityTableCollumn.IS_STAND] = false;
            //        leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
            //        leadrow[ProrityTableCollumn.IS_CHECK_GROUP_COLUMN] = true;
            //        leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = true;
            //        leadrow[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN] = true;
            //    }
            //    if (priorityTbl_4_3_K.Data != null && priorityTbl_4_3_K.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_4_3_K);
            //}

            EmployeeAssignment_ProrityTable priorityTbl_5 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 3    Skill + Lịch sử đi ca
            priorityTbl_5.ID = "UT_05";
            priorityTbl_5.Name = "Ưu tiên 05.  Skill + Lịch sử đi ca";
            priorityTbl_5.Data = Get_Empl_Skill_List();
            foreach (DataRow leadrow in priorityTbl_5.Data.Rows)
            {
                empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                leadrow[ProrityTableCollumn.IS_STAND] = false;
                leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
                leadrow[ProrityTableCollumn.IS_CHECK_ALL_COLUMN] = true;
                leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = false;
            }
            if (priorityTbl_5.Data != null && priorityTbl_5.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_5);

            if (simulateMonday || (date.DayOfWeek == DayOfWeek.Monday))
            {
                EmployeeAssignment_ProrityTable priorityTbl_5_K = new EmployeeAssignment_ProrityTable(); //Ưu tiên 3    Skill + Lịch sử đi ca
                priorityTbl_5_K.ID = "UT_05.K";
                priorityTbl_5_K.Name = "Ưu tiên 05.K  Skill + Lịch sử đi ca";
                priorityTbl_5_K.Data = Get_Empl_Skill_List();
                foreach (DataRow leadrow in priorityTbl_5_K.Data.Rows)
                {
                    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
                    leadrow[ProrityTableCollumn.IS_STAND] = false;
                    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
                    leadrow[ProrityTableCollumn.IS_CHECK_ALL_COLUMN] = true;
                    leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = false;
                    leadrow[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN] = true;
                }
                if (priorityTbl_5_K.Data != null && priorityTbl_5_K.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_5_K);

            }

            //EmployeeAssignment_ProrityTable priorityTbl_5_1 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 3    Skill + Lịch sử đi ca
            //priorityTbl_5_1.ID = "UT_05.1";
            //priorityTbl_5_1.Name = "Ưu tiên 05.1  Skill + Lịch sử đi ca + Hoán Người";
            //priorityTbl_5_1.Data = Get_Empl_Skill_List();
            //foreach (DataRow leadrow in priorityTbl_5_1.Data.Rows)
            //{
            //    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
            //    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
            //    leadrow[ProrityTableCollumn.IS_STAND] = false;
            //    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
            //    leadrow[ProrityTableCollumn.IS_CHECK_ALL_COLUMN] = true;
            //    leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = true;
            //}
            //if (priorityTbl_5_1.Data != null && priorityTbl_5_1.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_5_1);

            //if (simulateMonday || (date.DayOfWeek == DayOfWeek.Monday))
            //{
            //    EmployeeAssignment_ProrityTable priorityTbl_5_1_K = new EmployeeAssignment_ProrityTable(); //Ưu tiên 3    Skill + Lịch sử đi ca
            //    priorityTbl_5_1_K.ID = "UT_05.1.K";
            //    priorityTbl_5_1_K.Name = "Ưu tiên 05.1.K  Skill + Lịch sử đi ca + Hoán Người";
            //    priorityTbl_5_1_K.Data = Get_Empl_Skill_List();
            //    foreach (DataRow leadrow in priorityTbl_5_1_K.Data.Rows)
            //    {
            //        empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
            //        leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
            //        leadrow[ProrityTableCollumn.IS_STAND] = false;
            //        leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
            //        leadrow[ProrityTableCollumn.IS_CHECK_ALL_COLUMN] = true;
            //        leadrow[ProrityTableCollumn.IS_SWAP_COLUMN] = true;
            //        leadrow[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN] = true;
            //    }
            //    if (priorityTbl_5_1_K.Data != null && priorityTbl_5_1_K.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_5_1_K);
            //}

            //EmployeeAssignment_ProrityTable priorityTbl_10 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 3    Skill + Lịch sử đi ca
            //priorityTbl_10.ID = "UT_10";               
            //priorityTbl_10.Name = "Ưu tiên 10  STAND + Lịch sử đi ca";
            //priorityTbl_10.Data = Get_Empl_Skill_List();
            //foreach (DataRow leadrow in priorityTbl_10.Data.Rows)
            //{
            //    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
            //    wst = leadrow[ProrityTableCollumn.WST_COLUMN].ToString().Trim();
            //    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
            //    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
            //    leadrow[ProrityTableCollumn.IS_OPTIMIZE_COLUMN] = false;
            //    leadrow[ProrityTableCollumn.IS_STAND] = true;
            //}
            //if (priorityTbl_10.Data != null && priorityTbl_10.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_10);

            //EmployeeAssignment_ProrityTable priorityTbl_11 = new EmployeeAssignment_ProrityTable(); //Ưu tiên 3    Skill + Lịch sử đi ca
            //priorityTbl_11.ID = "UT_11";             
            //priorityTbl_11.Name = "Ưu tiên 11. STAND + Lịch sử đi ca + Xoay ca trong tuần";
            //priorityTbl_11.Data = Get_Empl_Skill_List();
            //foreach (DataRow leadrow in priorityTbl_11.Data.Rows)
            //{
            //    empl = leadrow[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
            //    wst = leadrow[ProrityTableCollumn.WST_COLUMN].ToString().Trim();
            //    leadrow[ProrityTableCollumn.SHIFT_COLUMN] = Get_History_shift(empl, date, HistiryTbl);
            //    leadrow[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN] = true;
            //    leadrow[ProrityTableCollumn.IS_OPTIMIZE_COLUMN] = true;
            //    leadrow[ProrityTableCollumn.IS_STAND] = true;
            //}
            //if (priorityTbl_11.Data != null && priorityTbl_11.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_11);

            //Force go to line
            //EmployeeAssignment_ProrityTable priorityTbl_x = new EmployeeAssignment_ProrityTable(); //Ưu tiên 0.0. Fix position cho nhân viên ngày đầu tuần
            //priorityTbl_x.ID = "UT_0x";
            //priorityTbl_x.Name = "Ưu tiên 0x. Force go to line";
            //priorityTbl_x.Data = Get_Fixposition();
            //foreach (DataRow row in priorityTbl_x.Data.Rows)
            //{
            //    row[ProrityTableCollumn.IS_FORCE_GOTO_LINE] = true;
            //}
            //if (priorityTbl_x.Data != null && priorityTbl_x.Data.Rows.Count > 0) PriorityTableList.Add(priorityTbl_x);


            //Find the skill for each workstation
            //EmployeeAssignment engine = new EmployeeAssignment(PlanLine_WST_Plan, availabelTble, prioTblList);
            string inputDataChecking = string.Empty;
            NewEmployeeAssignment engine = new NewEmployeeAssignment(PlanLine_WST_Plan, availabelTble, PriorityTableList, StatusLabel1, ProgressBar1, ref inputDataChecking);

            if (inputDataChecking != string.Empty)
            {
                MessageBox.Show(inputDataChecking, "Info");
            }

            DataTable nonSwapEmpls = Get_NonSwapList(new string[] { "Lead", "TS1" });

            if (nonSwapEmpls != null)
            {
                ////For testing only
                //DataRow row = nonSwapEmpls.NewRow();

                //row[ProrityTableCollumn.EMPL_COLUMN] = "20100095";
                //row[ProrityTableCollumn.EMPL_NAME_COLUMN] = "Thái Thị Thùy Hương";
                //nonSwapEmpls.Rows.Add(row);
            }
            engine.SetNonSwapList(nonSwapEmpls);

            DataTable ForceLists = Get_ForceList();

            engine.SetForceList(ForceLists);
            PlanLine_WST_Plan = engine.GetFinalList();
            //PlanLine_WST_Plan = engine.GetFinalListWithShiftRotation();
            // ApplyListEmployeeIntoGridView();//Long_15_06
            Update_SQL_Data(P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_da, P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb);
            return true;
        }

        bool AddColumnToTable(ref DataTable tbl, string newColumnName, string defaultVal)
        {
            if (tbl == null || newColumnName == string.Empty)
            {
                return false;
            }

            if (tbl.Columns.Contains(newColumnName) == false)
            {
                tbl.Columns.Add(newColumnName, typeof(string));

                //Reset the value
                foreach (DataRow row in tbl.Rows)
                {
                    row[newColumnName] = defaultVal;
                }
            }
            return true;
        }

        #region TEMP CODE, WILL BE REMOVED IN FUTURE

        //Remove danh sách các lead ra khỏi danh sách employee được xếp lịch
        //Function này chỉ dùng tạm thời, sẽ remove khi chương trình chạy ổn định
        private bool RemoveLeadEmployee(ref DataTable availabelTble, DataTable LeadEmployeeTble)
        {
            string EMPL_ID_COL = "Empl_ID";

            if (availabelTble == null)
            {
                return false;
            }

            if (LeadEmployeeTble == null || LeadEmployeeTble.Rows.Count == 0)
            {
                //Nothing to remove
                return true;
            }

            List<DataRow> rowsToDelete = new List<DataRow>();

            //Find Lead Employee ID and remove from availabelTble.
            foreach (DataRow LeadEmployeeRecord in LeadEmployeeTble.Rows)
            {
                foreach (DataRow AvailableEmployeeRecord in availabelTble.Rows)
                {
                    if (AvailableEmployeeRecord[EMPL_ID_COL].ToString().Trim() == LeadEmployeeRecord[EMPL_ID_COL].ToString().Trim())
                    {
                        rowsToDelete.Add(AvailableEmployeeRecord);
                    }
                }
            }

            foreach (DataRow row in rowsToDelete)
            {
                availabelTble.Rows.Remove(row);
            }

            return true;
        }

        #endregion

        DataTable Get_LeadFixposition()
        {
            SQL_API.SQL_ATC sqlObj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string cmd = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_007_Fix_Position]
                              Where [Position] = 'Lead'";
            sqlObj.GET_SQL_DATA(cmd);
            return sqlObj.DaTable;
        }

        DataTable Get_Fixposition_WST()
        {
            SQL_API.SQL_ATC sqlObj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string cmd = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_007_Fix_Position]
                              Where [WST_ID] <> '' or [WST_ID] is NULL";
            sqlObj.GET_SQL_DATA(cmd);
            return sqlObj.DaTable;
        }


        DataTable Get_TS_List()
        {
            //string[] conditions = new string[] {"TS1","TS2"};
            string[] conditions = new string[] {"TS1"};

            string filter = string.Empty;

            foreach (var condition in conditions)
            {
                if (filter != string.Empty)
                {
                    filter += " OR ";
                }
                filter += string.Format("[Position] = '{0}'", condition.ToString().Trim());
            }

            SQL_API.SQL_ATC sqlObj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string cmd = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_007_Fix_Position] Where " + filter;

            sqlObj.GET_SQL_DATA(cmd);
            return sqlObj.DaTable;
        }
                
        DataTable Get_ForceList()
        {
            DataTable retult;
            string filter = string.Empty;

            //filter += string.Format("[Position] = 'Lead' OR [Position] = 'TS1' OR [Position] = 'TS2' ");

            //Lead ko nằm trong danh sách force, chỉ nằm trong danh sách ưu tiên (nếu ko tìm được chỗ, vẫn có thể qua wst #)
            //Force: ko tìm được chỗ phù hợp ==> Qua stand
            filter += string.Format("[Position] = 'TS1' OR [Position] = 'TS2' ");

            SQL_API.SQL_ATC sqlObj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string cmd = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_007_Fix_Position] Where "+ filter;

            sqlObj.GET_SQL_DATA(cmd);
            retult =  sqlObj.DaTable;

            CheckAndInsertColumIfNotExist(ref retult, "ForceShift", -1, typeof(string));
            CheckAndInsertColumIfNotExist(ref retult, "ForceWST", -1, typeof(string));

            foreach (DataRow row in retult.Rows)
	        {
                string Position = row["Position"].ToString();

                if (Position == "TS1" || Position == "TS2")
                {
                    row["ForceShift"] = "Shift_1";
                }

                if (Position == "TS1")
                {
                    row["ForceWST"] = row["WST_ID"].ToString(); 
                }
	        }

            return retult;            
        }
        
        private bool CheckAndInsertColumIfNotExist(ref DataTable tbl, string ColumnName, int ColumnOrder, Type type)
        {
            if (tbl == null || ColumnName == string.Empty)
            {
                return false;
            }

            if (tbl.Columns.Contains(ColumnName) == false)
            {
                tbl.Columns.Add(ColumnName, type);
            }

            if(ColumnOrder != -1) tbl.Columns[ColumnName].SetOrdinal(ColumnOrder);

            return true;
        }

        DataTable Get_NonSwapList(string[] conditions)
        {
            string filter = string.Empty;

            foreach (var condition in conditions)
            {
                if (filter != string.Empty)
                {
                    filter += " OR ";
                }
                filter += string.Format("[Position] = '{0}'", condition.ToString().Trim());                
            }

            SQL_API.SQL_ATC sqlObj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string cmd = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_007_Fix_Position] Where "+ filter;

            sqlObj.GET_SQL_DATA(cmd);
            return sqlObj.DaTable;
        }


        DataTable Get_Fixposition()
        {
            SQL_API.SQL_ATC sqlObj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string cmd = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_007_Fix_Position]
                              Where [Position] != 'Lead' OR [Position] is null";
            sqlObj.GET_SQL_DATA(cmd);
            return sqlObj.DaTable;
        }

        DataTable Get_Empl_HadJob(DateTime date)
        {
            SQL_API.SQL_ATC sqlObj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string cmd = @"SELECT  distinct [Empl_ID]
                              FROM [JOB_ASSIGNMENT_DB].[dbo].[P_003_KeHoachSanXuatTheoLine]
                              Where [Empl_ID] != '' and [Empl_ID] is not NULL";
            cmd += " AND Date = '" + date.ToString("dd MMM yyyy") + "'";

            sqlObj.GET_SQL_DATA(cmd);
            return sqlObj.DaTable;
        }

        string Get_GroupofLine(string line)
        {
            string group = "";
            SQL_API.SQL_ATC sqlObj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string cmd = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_004_LineSkillRequest]
                              Where [LineID] = '" + line + "'";
            sqlObj.GET_SQL_DATA(cmd);

            if ((sqlObj.DaTable != null) && (sqlObj.DaTable.Rows.Count > 0))
            {
                group = sqlObj.DaTable.Rows[0]["GroupID"].ToString().Trim();
            }
            return group;

        }

        //DataTable GetTrackingHistory( DateTime date)
        //{
        //    DateTime from_date;

        //    from_date = date.AddDays(-1);


        //    SQL_API.SQL_ATC sqlObj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
        //    string cmd = @"SELECT distinct [Date],[From_Time],[ShiftName],[Empl_ID],[Empl_Name],[WST_ID],[LineID] FROM P007_P008_Tracking";
        //        cmd += @" WHERE (([Date] = '" + from_date.ToString("dd MMM yyyy") + "' AND [ShiftName] = 'Shift_1' AND [From_Time] BETWEEN '6:00:00' and '14:00:00')";
        //        cmd += @" OR    ([Date] = '" + from_date.AddDays(-1).ToString("dd MMM yyyy") + "' AND [ShiftName] = 'Shift_2' AND [From_Time] BETWEEN '14:00:00' and '22:00:00')";
        //        cmd += @" OR    ([Date] = '" + from_date.AddDays(-1).ToString("dd MMM yyyy") + "' AND [ShiftName] = 'Shift_3' AND [From_Time] BETWEEN '22:00:00' and '23:59:59')";
        //        cmd += @" OR    ([Date] = '" + from_date.AddDays(-1).ToString("dd MMM yyyy") + "' AND [ShiftName] = 'Shift_3' AND [From_Time] BETWEEN '00:00:00' and '06:00:00'))";
        //        cmd += @" AND ([WST_ID] is not NULL AND [WST_ID] != '')";
        //        cmd += @" AND ([SubLine_ID] != 'FRU')";


        //    //if ((date.DayOfWeek == DayOfWeek.Tuesday) && (DateTime.Now.DayOfWeek == DayOfWeek.Monday))
        //    //{
        //    //    cmd += @" WHERE (([Date] = '" + date.AddDays(-1).ToString("dd MMM yyyy") + "' AND [ShiftName] = 'Shift_1')";
        //    //    cmd += @" OR     ([Date] = '" + from_date.ToString("dd MMM yyyy") + "' AND [ShiftName] = 'Shift_2')";
        //    //    cmd += @" OR     ([Date] = '" + from_date.ToString("dd MMM yyyy") + "' AND [ShiftName] = 'Shift_3'))";
        //    //    cmd += @" AND ([WST_ID] is not NULL AND [WST_ID] != '')";
        //    //    cmd += @" AND ([SubLine_ID] != 'FRU')";
        //    //}
        //    //else if ((date.DayOfWeek == DayOfWeek.Tuesday) && (DateTime.Now.Date == date.Date))
        //    //{
        //    //    cmd += @" WHERE (([Date] = '" + from_date.ToString("dd MMM yyyy") + "' AND [ShiftName] = 'Shift_1')";
        //    //    cmd += @" OR     ([Date] = '" + from_date.ToString("dd MMM yyyy") + "' AND [ShiftName] = 'Shift_2')";
        //    //    cmd += @" OR     ([Date] = '" + from_date.ToString("dd MMM yyyy") + "' AND [ShiftName] = 'Shift_3'))";
        //    //    cmd += @" AND ([WST_ID] is not NULL AND [WST_ID] != '')";
        //    //    cmd += @" AND ([SubLine_ID] != 'FRU')";
        //    //}
        //    //else
        //    //{
        //    //    cmd += @" WHERE (([Date] = '" + from_date.ToString("dd MMM yyyy") + "' AND [ShiftName] = 'Shift_1')";
        //    //    cmd += @" OR    ([Date] = '" + from_date.AddDays(-1).ToString("dd MMM yyyy") + "' AND [ShiftName] = 'Shift_2')";
        //    //    cmd += @" OR    ([Date] = '" + from_date.AddDays(-1).ToString("dd MMM yyyy") + "' AND [ShiftName] = 'Shift_3'))";
        //    //    cmd += @" AND ([WST_ID] is not NULL AND [WST_ID] != '')";
        //    //    cmd += @" AND ([SubLine_ID] != 'FRU')";
        //    //}
        //    cmd += @" AND ([LineID] is not NULL AND [LineID] != '')" + " ORDER by [Date] DESC ,[From_Time] DESC ";
        //    sqlObj.GET_SQL_DATA(cmd);
        //    return sqlObj.DaTable;
        //}

        DataTable GetTrackingHistory(DateTime date)
        {
            DateTime from_date;

            if (date.DayOfWeek == DayOfWeek.Monday)
            {
                from_date = date.AddDays(-2);
            }
            else
            {
                from_date = date.AddDays(-1);

            }

            SQL_API.SQL_ATC sqlObj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string cmd = @"SELECT distinct [Date],[From_Time],[ShiftName],[Empl_ID],[Empl_Name],[WST_ID],[LineID] FROM P_003_KeHoachSanXuatTheoLine";
            cmd += @" WHERE ([Date] = '" + from_date.ToString("dd MMM yyyy") + "')";
            cmd += @" AND ([WST_ID] is not NULL AND [WST_ID] != '')";
            cmd += @" AND ([SubLine_ID] != 'FRU')";

            cmd += @" AND ([LineID] is not NULL AND [LineID] != '')" + " ORDER by [Date] DESC ,[From_Time] DESC ";
            sqlObj.GET_SQL_DATA(cmd);
            return sqlObj.DaTable;
        }

        DataTable Get_Empl_Skill_Group_List()
        {
            SQL_API.SQL_ATC sqlObj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);

            //Dho: Hiện tại, nếu sử dụng câu query bên trên --> ứng với mỗi nhân viên sẽ tìm được nhiều records
            //(mỗi record ứng với 1 skill...)
            //Việc này có lẽ ko cần thiết ??? Chỉ cần return danh sách employee (1 employee <-> 1 record)
            //Các thông tin cần thiết khác, engine trong NewEmployeeAssignmnet đã tự lấy

            string cmd = @"Select Distinct [Empl_ID], [Empl_Name] ,[GroupID] FROM [MDB_002_Empl_Skill]";


            sqlObj.GET_SQL_DATA(cmd);
            return sqlObj.DaTable;
        }

        DataTable Get_Empl_Skill_List()
        {
            SQL_API.SQL_ATC sqlObj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);

            //Dho: Hiện tại, nếu sử dụng câu query bên trên --> ứng với mỗi nhân viên sẽ tìm được nhiều records
            //(mỗi record ứng với 1 skill...)
            //Việc này có lẽ ko cần thiết ??? Chỉ cần return danh sách employee (1 employee <-> 1 record)
            //Các thông tin cần thiết khác, engine trong NewEmployeeAssignmnet đã tự lấy

            string cmd = @"Select Distinct [Empl_ID], [Empl_Name] FROM [MDB_002_Empl_Skill]";

            sqlObj.GET_SQL_DATA(cmd);
            return sqlObj.DaTable;
        }


        string Get_History_shift(string empl, DateTime plandate, DataTable history)
        {
            string filter_str = "Empl_ID = '" + empl.Trim() + "'";
            string date_str;
            DateTime history_date;
            DateTime sunday;
            string shift = "";

            if (history == null || empl == string.Empty)
            {
                return string.Empty;
            }

            DataRow[] rows = history.Select(filter_str);

            if (rows.Length > 0)
            {
                shift = rows[0]["ShiftName"].ToString().Trim();
                date_str = rows[0]["Date"].ToString().Trim();
                history_date = DateTime.Parse(date_str);
                sunday = Get_Last_Sunday(history_date);
                if (simulateMonday || (sunday < plandate))
                {
                    shift = Hoan_Doi_Ca(shift);
                }
            }
            if (shift == "")
            {

                history_date = plandate;
                shift = GetNearestShiftBasePlan(ref history_date, empl);

                if (shift != "")
                {
                    sunday = Get_Last_Sunday(history_date);

                    if (simulateMonday || (sunday < plandate))
                    {
                        shift = Hoan_Doi_Ca(shift);
                    }
                }
            }

            if (shift == "")
            {
                shift = SHIFT_UNKNOW;
            }

            return shift;
        }

        string GetNearestShiftBasePlan(ref DateTime plandate, string empl)
        {
            //Search trong vòng 7 ngày trước xem ca của nhân viên này
            int NumOfDayInPlanToCheck = 6;

            string shift = "";

            for (int i = 1; i < NumOfDayInPlanToCheck; i++)
            {
                shift = Get_Plan_Shift(empl, plandate.AddDays(-i));

                if (shift != "")
                {
                    plandate = plandate.AddDays(-i);
                    return shift;
                }
            }

            return "";
        }

        bool IsMonday(DateTime date)
        {
            if (date != null)
            {
                if (simulateMonday || (date.DayOfWeek == DayOfWeek.Monday))
                {
                    return true;
                }
            }

            return false;
        }

        DateTime Get_Last_Sunday(DateTime history_date)
        {
            while (history_date.DayOfWeek != DayOfWeek.Sunday)
            {
                history_date = history_date.AddDays(1);
            }
            return history_date;
        }

        string Hoan_Doi_Ca(string shift)
        {
            switch (shift)
            {
                case SHIFT_1:
                    shift = SHIFT_3;
                    break;
                case SHIFT_2:
                    shift = SHIFT_1;
                    break;
                case SHIFT_3:
                    shift = SHIFT_1;
                    break;
                default:
                    shift = SHIFT_UNKNOW;
                    break;
            }
            return shift;
        }

        string Get_Plan_Shift(string empl, DateTime date)
        {
            string shift = "";
            SQL_API.SQL_ATC sql_obj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            string cmd = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[P_003_KeHoachSanXuatTheoLine]";
            cmd += " Where [Empl_ID] = '" + empl + "'";
            cmd += " AND [Date] = '" + date.ToString("dd MMM yyyy") + "'";

            sql_obj.GET_SQL_DATA(cmd);

            if ((sql_obj.DaTable != null) && (sql_obj.DaTable.Rows.Count > 0))
            {
                shift = sql_obj.DaTable.Rows[0]["ShiftName"].ToString().Trim();
            }
            return shift;
        }
    }
}