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
        string EmplID_BeforeChange = string.Empty;
        string EmplName_BeforeChange = string.Empty;

        public DataTable PlanLine_DatePlan_Tbl = new DataTable();
        public DataSet PlanLine_DatePlan_ds = new DataSet();
        public SqlDataAdapter PlanLine_DatePlan_da;
        // DateTime dateCreate;

        void PlanForLine_Create_BT_Click(object sender, EventArgs e)
        {
            DateTime date;
            // Hien thi chon ngay
            DateSelect_Dialog_Form selectDate_Dialog = new DateSelect_Dialog_Form(DateTime.Now.AddDays(1));
            if (selectDate_Dialog.ShowDialog() == DialogResult.OK)
            {
                date = selectDate_Dialog.Select_Date;
                Create_Plan_For_Line(date);
            }
        }

        private bool Create_Plan_For_Line(DateTime date)
        {
            bool b;
            string sql_cmd;
            DataTable list_wst;
            string line_id, line_name, subline, subline_name, shift, wst, wst_name, part;
            TimeSpan from_time, to_time;
            int count;
            string mess;
            DataTable mainpart_tbl;
            int i = 0, total;

            StatusLabel1.Visible = true;
            ProgressBar1.Visible = true;
            StatusLabel1.Text = "Create Plan for Line";

            // lay du lieu ke hoach sx theo line cua ngay da chon
            sql_cmd = String.Format("SELECT * FROM [P_003_KeHoachSanXuatTheoLine] WHERE [Date] = '{0}' order by SubLine_ID", date.ToString("yyyy-MMM-dd"));
            b = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            if (b == false)
            {
                return false;
            }

            //Build again list of employee of planning day for manual select
            DataTable leave_info = Load_Leave_Register(date);
            P_003_KeHoachSanXuatTheoLine_tbAllEmployee = Load_All_Empl();
            RemoveLeadEmployee(ref P_003_KeHoachSanXuatTheoLine_tbAllEmployee, leave_info);

            count = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Count;
            if (count > 0)
            {
                mess = "Plan for date:" + date.ToString("dd MMM yyyy") + "was existing\n";
                mess += "Do you want to delete and create the new one?";

                if (MessageBox.Show(mess, "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }
                DeletePlanforLine(date);
                P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            }

            sql_cmd = String.Format("SELECT DISTINCT [LineID], [LineName], [SubLine_ID] FROM [P_002_PlanForProductionByDate] WHERE [Date] = '{0}' order by SubLine_ID", date.ToString("yyyy-MMM-dd"));

            // Lay du lieu Ke hoach cua ngay da chon
            // Kiem tra co bao nhieu line duoc san xuat trong ngay
            if (PlanLine_DatePlan_Tbl != null)
            {
                PlanLine_DatePlan_Tbl.Clear();
            }
            PlanLine_DatePlan_Tbl = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref PlanLine_DatePlan_da, ref PlanLine_DatePlan_ds);

            total = PlanLine_DatePlan_Tbl.Rows.Count;
            foreach (DataRow line_plan_row in PlanLine_DatePlan_Tbl.Rows)
            {
                // for Shift_1
                shift = SHIFT_1;
                subline = line_plan_row["SubLine_ID"].ToString().Trim();
                mainpart_tbl = Get_Main_Part_Shift_1(date, subline).Copy();
                if ((mainpart_tbl != null) && (mainpart_tbl.Rows.Count > 0))
                {
                    part = mainpart_tbl.Rows[0]["PartNumber"].ToString().Trim();
                    subline_name = mainpart_tbl.Rows[0]["SubLine_Name"].ToString().Trim();
                    line_id = mainpart_tbl.Rows[0]["LineID"].ToString().Trim();
                    line_name = mainpart_tbl.Rows[0]["LineName"].ToString().Trim();
                    from_time = TimeSpan.Parse("6:00:00");
                    to_time = Get_OutTime_Shift_1(date, subline);

                    list_wst = Get_List_wst(part);
                    foreach (DataRow wst_row in list_wst.Rows)
                    {
                        //Create new data for Plan of Line
                        wst = wst_row["WST_ID"].ToString();
                        wst_name = wst_row["WST_Name"].ToString();
                        Create_Update_PlanLine(date, line_id, line_name, subline, subline_name, shift, part, wst, wst_name, from_time, to_time);
                    }
                }

                // for Shift_2
                shift = SHIFT_2;
                subline = line_plan_row["SubLine_ID"].ToString().Trim();
                mainpart_tbl = Get_Main_Part_Shift_2(date, subline).Copy();
                if ((mainpart_tbl != null) && (mainpart_tbl.Rows.Count > 0))
                {
                    part = mainpart_tbl.Rows[0]["PartNumber"].ToString().Trim();
                    line_id = mainpart_tbl.Rows[0]["LineID"].ToString().Trim();
                    line_name = mainpart_tbl.Rows[0]["LineName"].ToString().Trim();
                    subline_name = mainpart_tbl.Rows[0]["SubLine_Name"].ToString().Trim();
                    from_time = TimeSpan.Parse("14:00:00");
                    to_time = Get_OutTime_Shift_2(date, subline); //TimeSpan.Parse("22:00:00");

                    list_wst = Get_List_wst(part);
                    foreach (DataRow wst_row in list_wst.Rows)
                    {
                        //Create new data for Plan of Line
                        wst = wst_row["WST_ID"].ToString();
                        wst_name = wst_row["WST_Name"].ToString();
                        Create_Update_PlanLine(date, line_id, line_name, subline, subline_name, shift, part, wst, wst_name, from_time, to_time);
                    }
                }

                // for Shift_3
                shift = SHIFT_3;
                subline = line_plan_row["SubLine_ID"].ToString().Trim();
                mainpart_tbl = Get_Main_Part_Shift_3(date, subline).Copy();
                if ((mainpart_tbl != null) && (mainpart_tbl.Rows.Count > 0))
                {
                    part = mainpart_tbl.Rows[0]["PartNumber"].ToString().Trim();
                    subline_name = mainpart_tbl.Rows[0]["SubLine_Name"].ToString().Trim();
                    line_id = mainpart_tbl.Rows[0]["LineID"].ToString().Trim();
                    line_name = mainpart_tbl.Rows[0]["LineName"].ToString().Trim();
                    //from_time = Get_InTime_Shift_3(dateCreate, subline);
                    //to_time = TimeSpan.Parse("6:00:00");
                    from_time = Get_InTime_Shift_3(date, subline);
                    to_time = Get_OutTime_Shift_3(date, subline);

                    list_wst = Get_List_wst(part);
                    foreach (DataRow wst_row in list_wst.Rows)
                    {
                        //Create new data for Plan of Line
                        wst = wst_row["WST_ID"].ToString();
                        wst_name = wst_row["WST_Name"].ToString();
                        Create_Update_PlanLine(date, line_id, line_name, subline, subline_name, shift, part, wst, wst_name, from_time, to_time);
                    }
                }
                i++;
                ProgressBar1.Value = i * 100 / total;
            }
            StatusLabel1.Text = "Assign Employee";
            Application.DoEvents();


            // Assign_Empl_for_LinePlan(date);

            StatusLabel1.Visible = false;
            ProgressBar1.Visible = false;
            return true;
        }

        private void P_003_UpdateListEmployee(ref DataTable dtEmployee, DataTable dtPlan, DateTime datePlan)
        {
            //Build danh sách nhân viên for list box sử dụng trong manual select
            foreach (DataRow row in dtEmployee.Rows)
            {
                row["Cur_Line"] = DBNull.Value;
                row["Cur_Shift"] = DBNull.Value;
                row["Date"] = DBNull.Value;

                if (datePlan != DateTime.MinValue)
                {
                    string employeeId = Utils.ObjectToString(row["Empl_ID"]);
                    DataRow[] planRows = dtPlan.Select(String.Format("Date='{0}' and Empl_ID='{1}'", datePlan, employeeId));

                    if (planRows.Length > 0)
                    {
                        DataRow planRow = planRows[0];
                        row["Cur_Line"] = planRow["SubLine_ID"];
                        row["Cur_Shift"] = planRow["ShiftName"];
                        row["Date"] = datePlan;
                    }
                }
            }
        }

        void P_003_KeHoachSanXuatTheoLine_AddFRU_BT_Click(object sender, EventArgs e)
        {

            DateTime date;
            // Hien thi chon ngay
            DateSelect_Dialog_Form selectDate_Dialog = new DateSelect_Dialog_Form(DateTime.Now.AddDays(1));
            if (selectDate_Dialog.ShowDialog() == DialogResult.OK)
            {
                date = selectDate_Dialog.Select_Date;
                Add_FRU_For_Line(date);
            }
        }
        private bool Add_FRU_For_Line (DateTime date)
        {
            string sql_cmd;
            bool b;
            int count;
            string shift;
            int num_wst, i;
            string fru_info = "FRU";
            TimeSpan from_time, to_time;
            string []shift_time;

            sql_cmd = String.Format("SELECT * FROM [P_003_KeHoachSanXuatTheoLine] WHERE [Date] = '{0}' order by SubLine_ID", date.ToString("yyyy-MMM-dd"));
            b = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            if (b == false)
            {
                return false;
            }

            count = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Count;
            if (count > 0)
            {
                // Todo (Kien): Create FRU WST
                AddFRU.Add_FRU addfru = new AddFRU.Add_FRU();

                if (addfru.ShowDialog() == DialogResult.OK)
                {
                    shift = addfru.Shift;
                    num_wst = addfru.NumWST;
                    shift_time = Get_Shift_Time(shift);
                    try
                    {
                        from_time = TimeSpan.Parse(shift_time[0]);
                        to_time = TimeSpan.Parse(shift_time[1]);
                        // Create FRU_WST
                        for (i = 1; i <= num_wst; i++)
                        {
                            Create_Update_PlanLine(date, fru_info, fru_info, fru_info, fru_info, shift, "", fru_info + "_" + i, "", from_time, to_time);
                        }
                    }
                    catch
                    {
                        return false;
                    }                    
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        

        private DataTable GetSqlData(string sqlCmd)
        {
            DataTable temp_dtb = new DataTable();
            DataSet inputData_tbl = new DataSet();
            SqlDataAdapter addapter = new SqlDataAdapter();

            if (temp_dtb != null)
            {
                temp_dtb.Clear();
            }
            temp_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sqlCmd, ref addapter, ref inputData_tbl);

            return temp_dtb;
        }

        private DateTime GetLastWorkingDate(DateTime date)
        {
            DateTime last_date;

            if (date.DayOfWeek != DayOfWeek.Monday)
            {
                last_date = date.AddDays(-7).Date;
            }
            else
            {
                last_date = date.AddDays(-7).Date;
            }

            return last_date;

        }

        void PlanForLine_DuplicateRow_BT_Click(object sender, EventArgs e)
        {
            if ((P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView == null) ||
                (P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows.Count <= 0) ||
                (P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb == null) ||
                (P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Count == 0))

            {
                string msg = "There is nothing to duplicate";
                MessageBox.Show(msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if ((P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.SelectedCells.Count <= 0) ||
                (P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.SelectedCells.Count > 1))
            {
                string msg = "Please click on a cell of row that you would like to duplicate";
                MessageBox.Show(msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else 
            {
                int selectedrowindex = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.SelectedCells[0].RowIndex;

                DataGridViewRow row = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows[selectedrowindex];

                if ((String.IsNullOrEmpty(Utils.ObjectToString(row.Cells["WST_ID"].Value))) ||
                    (String.IsNullOrEmpty(Utils.ObjectToString(row.Cells["ShiftName"].Value))) ||
                    (String.IsNullOrEmpty(Utils.ObjectToString(row.Cells["LineID"].Value))) ||
                    (String.IsNullOrEmpty(Utils.ObjectToString(row.Cells["Date"].Value))))
                {
                    string msg = "Please click on a cell of row that you would like to duplicate";
                    MessageBox.Show(msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string WST_ID_InChecking = row.Cells["WST_ID"].Value.ToString().Trim();
                string Line_ID_InChecking = row.Cells["LineID"].Value.ToString().Trim();
                string Shift_InChecking = row.Cells["ShiftName"].Value.ToString().Trim();
                string Date_InChecking = row.Cells["Date"].Value.ToString().Trim();

                int SelectedRowIndexInDataTable = -1;
                foreach (DataRow r in P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows)
	            {
                    string WST_ID = r["WST_ID"].ToString().Trim();
                    string Line_ID = r["LineID"].ToString().Trim();
                    string Shift = r["ShiftName"].ToString().Trim();
                    string Date = r["Date"].ToString().Trim();

                    if ((WST_ID == WST_ID_InChecking) &&
                        (Line_ID == Line_ID_InChecking) &&
                        (Shift == Shift_InChecking) &&
                        (Date == Date_InChecking))
                    {
                        SelectedRowIndexInDataTable = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.IndexOf(r);
                        break;
	                }            		 
	            }

                if (SelectedRowIndexInDataTable == -1)
                {
                    return;
                }
                
                bool Result = CopyRowWithValues( ref P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb,
                                                          SelectedRowIndexInDataTable,
                                                          new string[] { "WST_ID","WST_Name","Empl_ID", "Empl_Name", "Reason" });       
            }
        }

        public bool CopyRowWithValues(ref DataTable table, int RowIndexToBaseOn, string[] ColumnNameTobeIgnoreValues)
        {
            DataRow copiedRow = table.NewRow();

            DataRow Source = table.Rows[RowIndexToBaseOn];

            for (Int32 index = 0; index < table.Columns.Count; index++)
            {
                string columnName = table.Columns[index].Caption.ToString().Trim();

                if (ColumnNameTobeIgnoreValues.Contains(columnName) == false)
                {
                    copiedRow[index] = Source[index];
                }
            }

            table.Rows.InsertAt(copiedRow, RowIndexToBaseOn + 1);
            return true;
        }

        public DataGridViewRow CopyRowWithValues(DataGridView Gridview, DataGridViewRow row, string[] ColumnNameTobeIgnoreValues)
        {
            DataGridViewRow copiedRow = (DataGridViewRow)row.Clone();
            

            for (Int32 index = 0; index < row.Cells.Count; index++)
            {
                string columnName = Gridview.Columns[index].Name.ToString().Trim();

                if (ColumnNameTobeIgnoreValues.Contains(columnName) == false)
                {
                    copiedRow.Cells[index].Value = row.Cells[index].Value;
                }
            }
            return copiedRow;
        }

        void PlanForLine_Empl_Asign_BT_Click(object sender, EventArgs e)
        {
            DateTime date;
            DateSelect_Dialog_Form selectDate_Dialog = new DateSelect_Dialog_Form(DateTime.Now);
            if (selectDate_Dialog.ShowDialog() == DialogResult.OK)
            {
                date = selectDate_Dialog.Select_Date;
                Assign_Empl_for_LinePlan(date);
            }
        }

        void P_003_KeHoachSanXuatTheoLine_MasterDatabase_GridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            string columnName = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns[e.ColumnIndex].Name;
            if ("Empl_ID".Equals(columnName))
            {
                //DataTable dtPlan = ((BindingSource)P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.DataSource).DataSource as DataTable;
                //DateTime datePlan = Utils.ObjectToDateTime(P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Date", e.RowIndex].Value, DateTime.MinValue);
                string employeeId = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows[e.RowIndex].Cells["Empl_ID"].Value as string;

                if (employeeId.Trim() == string.Empty)
                {
                    return;
                }

                P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.CellValueChanged -= P_003_KeHoachSanXuatTheoLine_MasterDatabase_GridView_CellValueChanged;

                //   UpdateListEmployee(ref P_003_KeHoachSanXuatTheoLine_tbAllEmployee, dtPlan, datePlan, oldEmployeeIdAssigned);
                //   UpdateListEmployee(ref P_003_KeHoachSanXuatTheoLine_tbAllEmployee, dtPlan, datePlan, employeeId);
                //  UpdateListEmployee(ref P_003_KeHoachSanXuatTheoLine_tbAllEmployee, dtPlan, datePlan);

                //Check xem nhân viên chuẩn bị assign đã có việc chỗ khác

                //P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.CancelEdit();
                //Nếu cell changes nằm ở Column Empl_ID. Check and fill name of this imployee

                //Kiểm tra nhân viên này có được sắp cho wst khác chưa ? nếu có, cần confirm
                string WST_Id = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows[e.RowIndex].Cells["WST_ID"].Value as string;
                string Line_Id = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows[e.RowIndex].Cells["LineID"].Value as string;
                string Shift_Name = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows[e.RowIndex].Cells["ShiftName"].Value as string;
                string Date = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows[e.RowIndex].Cells["Date"].Value.ToString().Trim();

                DataRow searchEmplRow = P_003_KeHoachSanXuatTheoLine_tbAllEmployee.Rows.Find(employeeId);

                string Empl_name = string.Empty;

                if (searchEmplRow != null)
                {
                    Empl_name = searchEmplRow["Empl_Name"].ToString();
                }

                int NumOfRowInGridview = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows.Count;
                List<int> rowIDs = new List<int>();

                int Duplicated_RowIndex = -1;
                foreach (DataGridViewRow row in P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows)
                {
                    if (row.Index == e.RowIndex)
                    {
                        continue;
                    }

                    if ((String.IsNullOrEmpty(Utils.ObjectToString(row.Cells["Empl_ID"].Value))) ||
                        (String.IsNullOrEmpty(Utils.ObjectToString(row.Cells["WST_ID"].Value))) ||
                        (String.IsNullOrEmpty(Utils.ObjectToString(row.Cells["ShiftName"].Value))) ||
                        (String.IsNullOrEmpty(Utils.ObjectToString(row.Cells["LineID"].Value))) ||
                        (String.IsNullOrEmpty(Utils.ObjectToString(row.Cells["Date"].Value)))
                       )
                    {
                        continue;
                    }

                    string Empl_ID_InChecking = row.Cells["Empl_ID"].Value.ToString().Trim();
                    string WST_ID_InChecking = row.Cells["WST_ID"].Value.ToString().Trim();
                    string Line_ID_InChecking = row.Cells["LineID"].Value.ToString().Trim();
                    string Shift_InChecking = row.Cells["ShiftName"].Value.ToString().Trim();
                    string Date_InChecking = row.Cells["Date"].Value.ToString().Trim();

                    if (Empl_ID_InChecking == employeeId && Date == Date_InChecking && (WST_Id != WST_ID_InChecking ||
                                                                                        Line_Id != Line_ID_InChecking ||
                                                                                        Shift_Name != Shift_InChecking))
                    {
                        //Duplicated found. In the same date, an employee is assigned to 2 places
                        string message = string.Format("Employee {0} - {1} has beeen assigned to:\r\n", Empl_name, employeeId);
                        message += string.Format("\n\r- Workstation : {0}", WST_ID_InChecking);
                        message += string.Format("\n\r- Line : {0}", Line_ID_InChecking);
                        message += string.Format("\n\r- Shift : {0}", Shift_InChecking);


                        message += "\n\r\n\rDo you want to move this employee to this new workstation ?";
                        message += "\n\r\n\rPress Yes to move the employee to new workstation";
                        message += "\n\r\n\rPress No to cancel this assignment";

                        DialogResult result = MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            //Tiếp tục với quá trình xử lí bên dưới,
                            Duplicated_RowIndex = row.Index;
                            break;
                        }
                        else if (result == DialogResult.No)
                        {
                            P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Empl_ID", e.RowIndex].Value = EmplID_BeforeChange;
                            P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Empl_Name", e.RowIndex].Value = EmplName_BeforeChange;
                            P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.CellValueChanged += new DataGridViewCellEventHandler(P_003_KeHoachSanXuatTheoLine_MasterDatabase_GridView_CellValueChanged);
                            return;
                        }
                    }
                }

                string EmplSkill = string.Empty;
                string RequiredSkill = string.Empty;

                if (IsEmplHaveEnoughSkill_InDetail(employeeId, WST_Id, ref EmplSkill, ref RequiredSkill) == false)
                {
                    string CheckSkillmessage = String.Empty;
                    CheckSkillmessage += string.Format("Employee does not have enough Skill to work for this WorkStation");
                    CheckSkillmessage += string.Format("\r\n\r\n- Employee Skills:\r\n {0}", EmplSkill);
                    CheckSkillmessage += string.Format("\r\n\r\n- Required Skills:\r\n {0}", RequiredSkill);

                    CheckSkillmessage += "\n\r\n\rDo you want to assign this employee to this workstation ?";
                    CheckSkillmessage += "\n\r\n\rPress Yes to assign the employee to this workstation";
                    CheckSkillmessage += "\n\r\n\rPress No to cancel this assignment";

                    DialogResult CheckSkillResult = MessageBox.Show(CheckSkillmessage, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (CheckSkillResult == DialogResult.No)
                    {
                        //Nếu trước đó, vị trí đã được gán người ==> Trả lại người cũ
                        P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Empl_ID", e.RowIndex].Value = EmplID_BeforeChange;
                        P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Empl_Name", e.RowIndex].Value = EmplName_BeforeChange;
                        P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.CellValueChanged += new DataGridViewCellEventHandler(P_003_KeHoachSanXuatTheoLine_MasterDatabase_GridView_CellValueChanged);
                        return;
                    }
                }

                if (Duplicated_RowIndex != -1)
                {
                    //Có Duplicated
                    //Remove wst cũ của employee này
                    P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Empl_ID", Duplicated_RowIndex].Value = "";
                    P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Empl_Name", Duplicated_RowIndex].Value = "";
                    P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Reason", Duplicated_RowIndex].Value = "";
                    P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows[Duplicated_RowIndex].DefaultCellStyle.BackColor = COLOR_LINE_NOT_HAVE_EMPLOYEE;
                }

                DataRow searchRow = P_003_KeHoachSanXuatTheoLine_tbAllEmployee.Rows.Find(employeeId);
                if (searchRow != null)
                {
                    P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Empl_Name", e.RowIndex].Value = searchRow["Empl_Name"];
                }

                P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Reason", e.RowIndex].Value = "Manual Input";
                P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.CellValueChanged += new DataGridViewCellEventHandler(P_003_KeHoachSanXuatTheoLine_MasterDatabase_GridView_CellValueChanged);
            }
        }

        void P_003_KeHoachSanXuatTheoLine_MasterDatabase_GridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            String columnName = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns[e.ColumnIndex].Name;
            if (columnName.Equals("Empl_ID"))
            {
                //oldEmployeeIdAssigned = Utils.ObjectToString(P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Empl_ID", e.RowIndex].Value);
                DateTime curDatePlan = Utils.ObjectToDateTime(P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Date", e.RowIndex].Value, DateTime.MinValue);
                DataTable dtPlan = ((BindingSource)P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.DataSource).DataSource as DataTable;

                //Check assign employee list has this date
                if (curDatePlan != DateTime.MinValue && P_003_KeHoachSanXuatTheoLine_tbAllEmployee != null
                    //&& P_003_KeHoachSanXuatTheoLine_tbAllEmployee.Rows.Count > 0
                    //&& curDatePlan != Utils.ObjectToDateTime(P_003_KeHoachSanXuatTheoLine_tbAllEmployee.Rows[0]["Date"], DateTime.MinValue)
                    )
                {
                    P_003_UpdateListEmployee(ref P_003_KeHoachSanXuatTheoLine_tbAllEmployee, dtPlan, curDatePlan);
                }

                EmplID_BeforeChange = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Empl_ID", e.RowIndex].Value.ToString();
                EmplName_BeforeChange = P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Empl_Name", e.RowIndex].Value.ToString();
                
                if (EmplID_BeforeChange != string.Empty || EmplName_BeforeChange != string.Empty)
                {
                    int b = 10;    
                }
            }
        }
        void P_003_KeHoachSanXuatTheoLine_MasterDatabase_GridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows)
                {
                    if (!row.IsNewRow && String.IsNullOrEmpty(Utils.ObjectToString(row.Cells["Empl_ID"].Value)))
                    {
                        row.DefaultCellStyle.BackColor = COLOR_LINE_NOT_HAVE_EMPLOYEE;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Not Apply Format");
            }
        }
    }
}