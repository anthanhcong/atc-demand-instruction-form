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
        TimeSpan Run_Time = TimeSpan.Parse("00:00:00");
        void ProductionPlanDetail_Create_BT_Click(object sender, EventArgs e)
        {
            DateTime date;
            DateSelect_Dialog_Form selectDate_Dialog;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                selectDate_Dialog = new DateSelect_Dialog_Form(DateTime.Now.AddDays(2));
            }
            else
            {
                selectDate_Dialog = new DateSelect_Dialog_Form(DateTime.Now.AddDays(1));
            }
            if (selectDate_Dialog.ShowDialog() == DialogResult.OK)
            {
                date = selectDate_Dialog.Select_Date;
                Create_Details_Plan(date);
            }
        }
        private bool Create_Details_Plan(DateTime date)
        {
            string sql_cmd, mess;
            bool b;
            int count, qty;
            string po, part;
            string line_id, line_name, subline, subline_name;
            string shift;
            int capacity;
            DataTable po_plan;
            DataTable line_plan;
            DataTable list_all_part;
            DataTable fru_plan;
            int i = 0, total = 0;

            StatusLabel1.Visible = true;
            ProgressBar1.Visible = true;
            StatusLabel1.Text = "Create Pan in Details";
            
            TimeSpan from_time_1, to_time_1, from_time_2, to_time_2, from_time_3, to_time_3, start_time, end_time, from_time, to_time, po_time;
            // lay du lieu ke hoach sx theo line cua ngay da chon
            sql_cmd = String.Format("SELECT * FROM [P_004_KeHoachSanXuatTheoTram] WHERE [Date] = '{0}' order by LineId", date.ToString("yyyy-MMM-dd"));
            b = ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            if (b == false)
            {
                return false;
            }

            count = ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Count;
            if (count > 0)
            {
                mess = "Plan for date:" + date.ToString("dd MMM yyyy") + "was existing\n";
                mess += "Do you want to delete and create the new one?";

                if (MessageBox.Show(mess, "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }
                DeletePlanDetail(date);
                ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            }

            //Load danh sách các line trong ngày
            line_plan = Load_Line_List_By_Date(date);

            if (line_plan != null)
            {
                total = line_plan.Rows.Count;
            }
            foreach (DataRow row_line in line_plan.Rows)
            {
                line_id = row_line["LineID"].ToString().Trim();
                line_name = row_line["LineName"].ToString().Trim();
                subline = row_line["SubLine_ID"].ToString().Trim();
                subline_name = row_line["SubLine_Name"].ToString().Trim();
                // Load all part in subline
                list_all_part = Load_All_PartOfLine(date, subline);
                foreach (DataRow subline_row in list_all_part.Rows)
                {
                    part = subline_row["PartNumber"].ToString().Trim();
                    from_time_1 = subline_row["Shift_1_From"].ToString().Trim() == "" ? TimeSpan.Parse("00:00:00") : (TimeSpan)subline_row["Shift_1_From"];
                    to_time_1 = subline_row["Shift_1_To"].ToString().Trim() == "" ? TimeSpan.Parse("00:00:00") : (TimeSpan)subline_row["Shift_1_To"];
                    from_time_2 = subline_row["Shift_2_From"].ToString().Trim() == "" ? TimeSpan.Parse("00:00:00") : (TimeSpan)subline_row["Shift_2_From"];
                    to_time_2 = subline_row["Shift_2_To"].ToString().Trim() == "" ? TimeSpan.Parse("00:00:00") : (TimeSpan)subline_row["Shift_2_To"];
                    from_time_3 = subline_row["Shift_3_From"].ToString().Trim() == "" ? TimeSpan.Parse("00:00:00") : (TimeSpan)subline_row["Shift_3_From"];
                    to_time_3 = subline_row["Shift_3_To"].ToString().Trim() == "" ? TimeSpan.Parse("00:00:00") : (TimeSpan)subline_row["Shift_3_To"];

                    if (from_time_1 != TimeSpan.Parse("00:00:00"))
                    {
                        shift = SHIFT_1;
                        start_time = from_time_1;
                        end_time = to_time_1;
                    }
                    else if (from_time_2 != TimeSpan.Parse("00:00:00"))
                    {
                        shift = SHIFT_2;
                        start_time = from_time_2;
                        end_time = to_time_2;
                    }
                    else if (from_time_3 != TimeSpan.Parse("00:00:00"))
                    {
                        shift = SHIFT_3;
                        start_time = from_time_3;
                        end_time = to_time_3;
                    }
                    else
                    {
                        mess = "Can't get From Time for Part: " + part;
                        // MessageBox.Show(mess, "Error");

                        shift = SHIFT_UNKNOW;
                        start_time = TimeSpan.Parse("00:00:00");
                        end_time = TimeSpan.Parse("00:00:00");
                        // return;
                    }

                    capacity = subline_row["Capacity"] == DBNull.Value ? 0 : (int)subline_row["Capacity"];
                    po_plan = Load_PO_Plan(date, part);
                    foreach (DataRow po_row in po_plan.Rows)
                    {
                        po = po_row["PO"].ToString().Trim();
                        try
                        {
                            qty = Convert.ToInt32(po_row["Qty"].ToString().Trim());
                            po_time = Get_PO_RunTime(qty, capacity);       // Get PO Run Time
                            from_time = start_time;

                            start_time = Create_Plan_for_PO(date, ref shift, po, part, line_id, line_name, subline, subline_name, capacity, qty,
                                                            ref start_time, ref end_time, from_time_1, to_time_1,
                                                            from_time_2, to_time_2, from_time_3, to_time_3);
                        }
                        catch
                        {
                            qty = 0;
                            from_time = TimeSpan.Parse("00:00:00");
                            to_time = TimeSpan.Parse("00:00:00");
                        }
                    }
                }
                i++;
                ProgressBar1.Value = i * 100 / total;
            }
            // Add Fru
            fru_plan = GetFruPlan(date);
            string fru_info = "FRU";
            foreach (DataRow row in fru_plan.Rows)
            {
                // Create_Detail_Plan_for_PO(date, fru_info, fru_info, fru_info, fru_info, fru_info, fru_info, 1, row["ShiftName"].ToString().Trim(), (TimeSpan)row["From_Time"], (TimeSpan)row["To_Time"]);
                Create_Update_PlanDetail(date, row["ShiftName"].ToString().Trim(), fru_info, fru_info, fru_info, fru_info, fru_info, fru_info, row["WST_ID"].ToString().Trim(), row["WST_Name"].ToString().Trim(), 1, (TimeSpan)row["From_Time"], (TimeSpan)row["To_Time"]);
            }


            Assign_empl_for_PlanDetail(date);
            StatusLabel1.Visible = false;
            ProgressBar1.Visible = false;
            return true;
        }

        TimeSpan Create_Plan_for_PO(DateTime date, ref string shift, string po, string part,
                                    string line_id, string line_name, string subline, string subline_name,
                                    int capacity, int qty, ref TimeSpan start_time, ref TimeSpan end_time, TimeSpan from_time_1, TimeSpan to_time_1,
                                    TimeSpan from_time_2, TimeSpan to_time_2, TimeSpan from_time_3, TimeSpan to_time_3)
        {
            TimeSpan from_time, to_time, po_time, temp_time;
            int hour, minute, temp_qty;
            string mess;

            if (shift == SHIFT_UNKNOW)
            {
                Create_Detail_Plan_for_PO(date, po, part, line_id, line_name, subline, subline_name, qty, shift, start_time, end_time);
                return start_time;
            }

            from_time = start_time;
            po_time = Get_PO_RunTime(qty, capacity);       // Get PO Run Time
            temp_time = po_time + from_time;               // Thời gian thực tế kết thúc của một PO
            if (temp_time.Days >= 1)
            {
                temp_time = temp_time - TimeSpan.Parse("23:59:59");
                temp_time = temp_time - TimeSpan.Parse("00:00:01");
            }
            if ((temp_time <= end_time) || ((temp_time <= TimeSpan.Parse("23:59:59")) && (shift == SHIFT_3)))
            {
                //Nếu kết thúc sớm hoặc đúng giờ 
                to_time = temp_time;
                Create_Detail_Plan_for_PO(date, po, part, line_id, line_name, subline, subline_name, qty, shift, from_time, to_time);
                start_time = to_time;
            }
            else
            {
                //Nếu kết thúc sau thời gian ca
                to_time = end_time;
                po_time = to_time - from_time;
                hour = po_time.Hours;
                minute = po_time.Minutes;
                // temp = hour + minute / 60;
                temp_qty = (hour * 60 * capacity + minute * capacity)/(60*8);

                Create_Detail_Plan_for_PO(date, po, part, line_id, line_name, subline, subline_name, temp_qty, shift, from_time, to_time);

                switch (shift)
                {
                    case SHIFT_1:
                        if (from_time_2 != TimeSpan.Parse("00:00:00"))
                        {
                            shift = SHIFT_2;
                            start_time = from_time_2;
                            end_time = to_time_2;
                        }
                        else if (from_time_3 != TimeSpan.Parse("00:00:00"))
                        {
                            shift = SHIFT_3;
                            start_time = from_time_3;
                            end_time = to_time_3;
                        }
                        else
                        {
                            mess = "Can't get From Time for Part: " + part;
                            MessageBox.Show(mess, "Error");
                            return start_time;
                        }
                        break;
                    case SHIFT_2:
                        if (from_time_3 != TimeSpan.Parse("00:00:00"))
                        {
                            shift = SHIFT_3;
                            start_time = from_time_3;
                            end_time = to_time_3;
                        }
                        else
                        {
                            mess = "Can't get From Time for Part: " + part;
                            MessageBox.Show(mess, "Error");
                            return start_time;
                        }
                        break;
                    default:
                        mess = "Can't get From Time for Part: " + part;
                        MessageBox.Show(mess, "Error");
                        return start_time;
                }
                from_time = start_time;
                qty = qty - temp_qty;
                po_time = Get_PO_RunTime(qty, capacity);       // Get PO Run Time
                temp_time = from_time + po_time;
                if (temp_time.Days >= 1)
                {
                    temp_time = temp_time - TimeSpan.Parse("23:59:59");
                    temp_time = temp_time - TimeSpan.Parse("00:00:01");
                }
                if ((temp_time <= end_time) || ((temp_time <= TimeSpan.Parse("23:59:59")) && (shift == SHIFT_3)))
                {
                    // Nếu kết thúc sớm hoặc đúng giờ 
                    to_time = temp_time;
                    Create_Detail_Plan_for_PO(date, po, part, line_id, line_name, subline, subline_name, qty, shift, from_time, to_time);
                    start_time = to_time;
                }
                else
                {
                    // Nếu kết thúc sau thời gian ca
                    to_time = end_time;
                    po_time = to_time - from_time;
                    hour = po_time.Hours;
                    minute = po_time.Minutes;
                    temp_qty = (hour * 60 * capacity + minute * capacity) / (60 * 8);
                    switch (shift)
                    {
                        case SHIFT_2:
                            if (from_time_3 != TimeSpan.Parse("00:00:00"))
                            {
                                shift = SHIFT_3;
                                start_time = from_time_3;
                                end_time = to_time_3;
                            }
                            else
                            {
                                mess = "Can't get From Time for Part: " + part;
                                start_time = from_time_3;
                                end_time = to_time_3;
                                MessageBox.Show(mess, "Error");
                                return start_time;
                            }
                            break;
                        default:
                            mess = "Can't get From Time for Part: " + part;
                            MessageBox.Show(mess, "Error");
                            return start_time;
                    }
                    from_time = start_time;
                    qty = qty - temp_qty;
                    po_time = Get_PO_RunTime(qty, capacity);       // Get PO Run Time
                    temp_time = from_time + po_time;
                    to_time = temp_time;
                    Create_Detail_Plan_for_PO(date, po, part, line_id, line_name, subline, subline_name, qty, shift, from_time, to_time);
                    start_time = to_time;
                }
            }
            return start_time;
        }


        void ProductionPlanDetail_Assign_BT_Click(object sender, EventArgs e)
        {
            DateTime date;
            DateSelect_Dialog_Form selectDate_Dialog;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                selectDate_Dialog = new DateSelect_Dialog_Form(DateTime.Now.AddDays(2));
            }
            else
            {
                selectDate_Dialog = new DateSelect_Dialog_Form(DateTime.Now.AddDays(1));
            }
            if (selectDate_Dialog.ShowDialog() == DialogResult.OK)
            {
                date = selectDate_Dialog.Select_Date;
                Assign_empl_for_PlanDetail(date);
            }
        }


        bool Create_Detail_Plan_for_PO(DateTime date, string po, string part, string line_id, string line_name, string subline, string subline_name,
            int qty, string shift, TimeSpan from_time, TimeSpan to_time)
        {
            string wst, wst_name;
            DataTable list_wst = Get_List_wst(part);
            foreach (DataRow wst_row in list_wst.Rows)
            {
                //Create new data for Plan of Line
                wst = wst_row["WST_ID"].ToString();
                wst_name = wst_row["WST_Name"].ToString();
                Create_Update_PlanDetail(date, shift, po, part, line_id, line_name, subline, subline_name, wst, wst_name, qty, from_time, to_time);
            }

            return true;
        }

        TimeSpan Get_PO_RunTime(int qty, int capacity)
        {
            int hour, minute;
            TimeSpan po_time = TimeSpan.Parse("0:00:00");
            if (capacity == 0)
            {
                return po_time;
            }
            hour = (int)qty * 8 / capacity;
            minute = qty * 8 * 60 / capacity;
            minute = minute % 60;
            po_time = TimeSpan.Parse(hour.ToString() + ":" + minute.ToString() + ":00");
            return po_time;
        }

        bool Assign_empl_for_PlanDetail(DateTime date)
        {
            string sql_cmd;
            bool b;
            int count;
            string line, subline, wst;
            string shift;
            string empl_id, empl_Name;
            string [] empl_info;
            bool retvar = false;

            DataTable date_Plan;
            int i = 0, total;

            StatusLabel1.Visible = true;
            ProgressBar1.Visible = true;
            StatusLabel1.Text = "Assign Empl for Detail Plan";

            sql_cmd = String.Format("SELECT * FROM [P_003_KeHoachSanXuatTheoLine] WHERE [Date] = '{0}' order by Main_Part", date.ToString("yyyy-MMM-dd"));
            date_Plan = GetSqlData(sql_cmd);

            sql_cmd = String.Format("SELECT * FROM [P_004_KeHoachSanXuatTheoTram] WHERE [Date] = '{0}' order by PartNumber", date.ToString("yyyy-MMM-dd"));
            b = ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            if (b == false)
            {
                return false;
            }

            count = ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Count;
            if (count > 0)
            {
                total = count;
                foreach (DataRow row in ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows)
                {
                    line = row["LineID"].ToString().Trim();
                    subline = row["SubLine_ID"].ToString().Trim(); 
                    wst = row["WST_ID"].ToString().Trim();
                    shift = row["ShiftName"].ToString().Trim();

                    //empl_info = Get_Empl_Line_Plan(date_Plan, shift, line, subline, wst);
                    empl_info = Get_Empl_Line_Plan(date_Plan, shift, line, subline, wst);
                    empl_id = empl_info[0];
                    empl_Name = empl_info[1];
                    row["Empl_ID"] = empl_id;
                    row["Empl_Name"] = empl_Name;
                    // break;

                    i++;
                    ProgressBar1.Value = i * 100 / total;
                }
                retvar = Update_SQL_Data(ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.Data_da, ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb);
            }
            StatusLabel1.Visible = false;
            ProgressBar1.Visible = false;
            return retvar;
        }
        //private DataTable ProductionPlanDetail_CreateListEmployee()
        //{
        //    DataTable tb = new DataTable();
        //    string cur_line = "";
        //    string cur_shift = "";


        //    tb.Columns.Add("Empl_ID", typeof(string));
        //    tb.Columns.Add("Empl_Name", typeof(string));
        //    tb.Columns.Add("Cur_Line", typeof(string));
        //    tb.Columns.Add("Cur_shift", typeof(string));
        //    DataTable empl_list = Load_All_Empl();

        //    foreach (DataRow row in empl_list.Rows)
        //    {
        //        string employeeID = Utils.ObjectToString(row["Empl_ID"]);
        //        if (!String.IsNullOrEmpty(employeeID))
        //        {
        //            DateTime date = new DateTime(2015, 6, 5);//Fixme: Remove the hardcode here. 
        //            //cur_line = Get_Empl_Cur_Line(date, employeeID);
        //            //cur_shift = Get_Empl_Cur_Shift(date, employeeID);
        //            tb.Rows.Add(new Object[] { row["Empl_ID"], row["Empl_Name"], cur_line, cur_shift });
        //        }
        //    }
        //    return tb;
        //}
        private void P_004_UpdateListEmployee(ref DataTable dtEmployee, DataTable dtPlan, DateTime datePlan)
        {
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
        void ProductionPlanDetail_MasterDatabase_GridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            string columnName = ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns[e.ColumnIndex].Name;
            if ("Empl_ID".Equals(columnName))
            {
                string employeeId = ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows[e.RowIndex].Cells["Empl_ID"].Value as string;
                DataRow searchRow = ProductionPlanDetail_tbAllEmployee.Rows.Find(employeeId);
                if (searchRow != null)
                {
                    ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Empl_Name", e.RowIndex].Value = searchRow["Empl_Name"];
                }

                // update datatable
                // ProductionPlanDetail_tbAllEmployee = ProductionPlanDetail_CreateListEmployee();
            }
        }
        void ProductionPlanDetail_MasterDatabase_GridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            String columnName = ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns[e.ColumnIndex].Name;
            if (columnName.Equals("Empl_ID"))
            {
                //oldEmployeeIdAssigned = Utils.ObjectToString(P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Empl_ID", e.RowIndex].Value);
                DateTime curDatePlan = Utils.ObjectToDateTime(ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Date", e.RowIndex].Value, DateTime.MinValue);
                DataTable dtPlan = ((BindingSource)ProductionPlanDetail_MasterDatabase.MasterDatabase_GridviewTBL.GridView.DataSource).DataSource as DataTable;

                //Check assign employee list has this date
                if (curDatePlan != DateTime.MinValue && ProductionPlanDetail_tbAllEmployee != null
                    //&& P_003_KeHoachSanXuatTheoLine_tbAllEmployee.Rows.Count > 0
                    //&& curDatePlan != Utils.ObjectToDateTime(P_003_KeHoachSanXuatTheoLine_tbAllEmployee.Rows[0]["Date"], DateTime.MinValue)
                    )
                {
                    P_004_UpdateListEmployee(ref ProductionPlanDetail_tbAllEmployee, dtPlan, curDatePlan);
                }
            }
        }
    
        
    }
}