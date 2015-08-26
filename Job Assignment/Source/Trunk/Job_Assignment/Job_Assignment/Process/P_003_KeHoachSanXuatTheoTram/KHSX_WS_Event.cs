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
using System.Collections;

namespace Job_Assignment
{
    public partial class Form1 : SQL_APPL
    {
        private void KHSX_WS_DatePick_ValueChanged(object sender, EventArgs e)
        {
            DateTime select_date = PlanByWST_DatePick.Value;

            KHSX_WS_dtb = Load_KHSX_WS_DB_Date(select_date);

            if (KHSX_WS_dtb == null)
            {
                return;
            }
            else
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = KHSX_WS_dtb;
                KHSX_WS_dtgrid.DataSource = bs;
                return;
            }
        }

        private void KHSX_Save_BT_Click(object sender, EventArgs e)
        {
            if (Update_SQL_Data(KHSX_WS_da, KHSX_WS_dtb))
            {
                MessageBox.Show("Data is Saved", "Success");
            }
            else
            {
                MessageBox.Show("Failed to Save Data", "Failed");
            }
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            DateTime select_date = PlanByWST_DatePick.Value;
            DataTable ws_list;
            string line_id, line_name;
            DateTime date;
            string cell_value;
            string part_number;
            int songuoi;
            float soca;
            string WST_ID, WST_Name;
            DataRow new_row;
            int CapacityOfLine;
            string []shift_time;

            Clean_KHSX_WS_Date(select_date);

            Load_KHSX_DB_Date(select_date);
            //Load_KHSX_WS_DB_Date(select_date);

            if (KHSX_dtb.Rows.Count == 0)
            {
                MessageBox.Show("There's no part to build for " + select_date.ToString("dd MMM yyyy"), "Warning");
                return;
            }
            foreach (DataRow khsx_row in KHSX_dtb.Rows)
            {
                cell_value = khsx_row["Date"].ToString().Trim();

                //Get some data from KHSX by date
                date = DateTime.Parse(cell_value);
                part_number = khsx_row["PartNumber"].ToString().Trim();
                line_id = khsx_row["LineID"].ToString();
                line_name = khsx_row["LineName"].ToString();
                songuoi = (int)khsx_row["NumOfPerson_Per_Day"];

                cell_value = khsx_row["NumOfShift"].ToString();
                CapacityOfLine = (int)khsx_row["Capacity"];
                soca = float.Parse(cell_value);

                // Fill the WS.
                // Dho: Fill the list of WST for this part.
                ws_list = Load_WS_List(line_id, part_number);

                if (ws_list.Rows.Count == 0)
                {
                    MessageBox.Show("Cannot find the information for Line ID " + line_id + " AND PartNumber" + part_number, "Error");
                    return;
                }

                Empl_Shift_Name Curr_Shift_ID = Empl_Shift_Name.Shift_1;
                int Curr_Shift_Percent = 0;

                float RemainingShift = soca;
                float ShiftPercentToArrange;

                while (RemainingShift > 0)
                {
                    Get_Shift_ID(line_id, ref Curr_Shift_ID,ref Curr_Shift_Percent);

                    if (Curr_Shift_Percent == 100)
                    {
                        Curr_Shift_Percent = 0;
                        Curr_Shift_ID += 1;
                    }

                    //Fill the remaining shift
                    if ((Curr_Shift_Percent + RemainingShift * 100) > 100)
                    {
                        ShiftPercentToArrange = (100 - Curr_Shift_Percent);
                        Curr_Shift_Percent = 100;
                    }
                    else 
                    {
                        ShiftPercentToArrange = RemainingShift * 100;
                        Curr_Shift_Percent += (int)RemainingShift * 100;
                    }
                    
                    RemainingShift -= ShiftPercentToArrange/100;

                    RemainingShift = (float)Math.Round((double)RemainingShift, 2);

                    foreach (DataRow ws_row in ws_list.Rows)
                    {
                        new_row = KHSX_WS_dtb.NewRow();

                        WST_ID = ws_row["WST_ID"].ToString();
                        WST_Name = ws_row["WST_Name"].ToString();

                        new_row["Date"] = date;
                        new_row["PartNumber"] = part_number;
                        new_row["LineID"] = line_id;
                        new_row["LineName"] = line_name;
                        new_row["WST_ID"] = ws_row["WST_ID"].ToString();
                        new_row["WST_Name"] = ws_row["WST_Name"].ToString();
                        new_row["ShiftName"] = Curr_Shift_ID;
                        shift_time = Get_Shift_Time(Curr_Shift_ID.ToString());

                        new_row["From_Time"] = shift_time[0];
                        new_row["To_Time"] = shift_time[1];
                        new_row["Shift_Percent"] = ShiftPercentToArrange;
                        new_row["Capacity"] = CapacityOfLine;
                        new_row["Qty"] = Math.Round((double)(ShiftPercentToArrange * CapacityOfLine) / 100, 0);
                        //new_row["NumOfPerson_Per_Day"] = 0;
                        //new_row["NumOfShift"] = 0;

                        KHSX_WS_dtb.Rows.Add(new_row);
                    }
                }
            }
            Update_SQL_Data(KHSX_WS_da, KHSX_WS_dtb);

            BindingSource bs = new BindingSource();
            bs.DataSource = KHSX_WS_dtb;
            KHSX_WS_dtgrid.DataSource = bs;

            //Format the view using color
            FormatRowInGridView(KHSX_WS_dtgrid, 1);
        }

        private void KHSX_WS_Export_BT_Click(object sender, EventArgs e)
        {
            SaveFileDialog save_diaglog = new SaveFileDialog();
            string file_name, fInfo;
            string temp;

            if (KHSX_WS_dtgrid.DataSource == null)
            {
                MessageBox.Show("Vui lòng chọn dữ liệu", "Thông báo");
                return;
            }

            if (Update_SQL_Data(KHSX_WS_da, KHSX_WS_dtb) == false)
            {
                MessageBox.Show("Cập nhật thay đổi trước khi export file thất bại", "Thông báo");
            }

            save_diaglog.Filter = "Excel file (*.xlsx;*.xls)|*.xlsx;*.xls|All files (*.*)|*.*";
            if (save_diaglog.ShowDialog() == DialogResult.OK)
            {
                file_name = save_diaglog.FileName;
                fInfo = Path.GetExtension(save_diaglog.FileName);
                temp = PlanByWST_Export_BT.Text;
                PlanByWST_Export_BT.Text = "Exporting ...";
                PlanByWST_Export_BT.Enabled = false;
                if ((fInfo == ".xlsx") || (fInfo == ".xls"))
                {
                    // ExportDataToExcel(file_name, fInfo, Group_Name, Data_dtb, ProgressBar1);
                    ExportGridviewToExcel(file_name, fInfo, "Ke Hoach San Xuat Theo WorkSpace", KHSX_WS_dtgrid, ProgressBar1, StatusLabel1, StatusLabel2);
                }
                PlanByWST_Export_BT.Enabled = true;
                PlanByWST_Export_BT.Text = temp;
                MessageBox.Show("Export File thành công", "Thông báo");
            }
        }

        private void PlanByWst_Color_BT_Click(object sender, EventArgs e)
        {
            FormatRowInGridView(KHSX_WS_dtgrid, 1);
        }

        /// <summary>
        /// //Dho: Assign the employee here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlanByWST_Empl_BT_Click(object sender, EventArgs e)
        {
            KHSX_WS_DatePick_ValueChanged(null, null);
            string part, line_id, wst_id, shift;
            string empl_id, cur_empl_Id, Cur_Empl_Name ;
            DateTime date = PlanByWST_DatePick.Value;
            DataTable Avaliable_Empl_List_dtb;

            // Load DS Nhan Vien Di lam Trong ngay + Skill
            Avaliable_Empl_List_dtb = Load_Avaliable_Empl_List(date);
            
            //Hôm qua làm WST nào. Hôm nay làm lại 
            int COUNT = KHSX_WS_dtb.Rows.Count;

            for (int i = 0; i < COUNT; i++)
            //foreach (DataRow row in KHSX_WS_dtb.Rows)
            {
                DataRow row = KHSX_WS_dtb.Rows[i];
                part = row["PartNumber"].ToString().Trim();
                line_id = row["LineID"].ToString().Trim();
                wst_id = row["WST_ID"].ToString().Trim();
                shift = row["ShiftName"].ToString().Trim();

                empl_id = Get_Empl_Last_Plan(date, line_id, wst_id, shift, ref Avaliable_Empl_List_dtb);//Hôm qua làm WST nào. Hôm nay làm lại

                if (empl_id != "")
                {
                    row["Empl_ID"] = empl_id;
                    row["Empl_Name"] = Get_Empl_Name(empl_id);

                    //percent = (int)row["Shift_Percent"];
                    //switch (shift)
                    //{
                    //    case "0":
                    //        row["Shift_HC"] = (float)(8 * percent / 100);
                    //        break;
                    //    case "1":
                    //        row["Shift_1"] = (float)(8 * percent / 100);
                    //        break;
                    //    case "2":
                    //        row["Shift_2"] = (float)(8 * percent / 100);
                    //        break;
                    //    case "3":
                    //        row["Shift_3"] = (float)(8 * percent / 100);
                    //        break;
                    //    default:
                    //        break;
                    //}
                }
            }

            // Sắp Lịch cho các WST mới phát sinh
            foreach (DataRow row in KHSX_WS_dtb.Rows)
            {
                part = row["PartNumber"].ToString().Trim();
                line_id = row["LineID"].ToString().Trim();
                wst_id = row["WST_ID"].ToString().Trim();
                shift = row["ShiftName"].ToString().Trim();

                //if (cur_empl_Id != "")
                //{
                //rule
                empl_id = Get_Empl_New_Plan(date, line_id, wst_id, shift, ref Avaliable_Empl_List_dtb);// Sắp Lịch cho các WST mới phát sinh

                if (empl_id != "")
                {
                    row["Empl_ID"] = empl_id;
                    row["Empl_Name"] = Get_Empl_Name(empl_id);

                    UpdateEmplStatus(empl_id, ref Avaliable_Empl_List_dtb, Empl_Status_Type.Inuse);
                    //percent = (int)row["Shift_Percent"];
                    //Update_All_Empl_Skill();
                    //row[shift] = (float)(8 * percent / 100);
                    //switch (shift)
                    //{
                    //    case "Shift_HC":
                    //        row["Shift_HC"] = (float)(8 * percent / 100);
                    //        break;
                    //    case "1":
                    //        row["Shift_1"] = (float)(8 * percent / 100);
                    //        break;
                    //    case "2":
                    //        row["Shift_2"] = (float)(8 * percent / 100);
                    //        break;
                    //    case "3":
                    //        row["Shift_3"] = (float)(8 * percent / 100);
                    //        break;
                    //    default:
                    //        break;
                    //}
                }
                
            }

            // Create Datatable của các vị trí chưa có người làm
            // Table_A(Line, WST) <== KHSX_WS_dtb)
            
            DataTable RemaingWST_Tbl = new DataTable();

            RemaingWST_Tbl.Columns.Add("LineID", typeof(String));
            RemaingWST_Tbl.Columns.Add("WST_ID", typeof(String));
            RemaingWST_Tbl.Columns.Add("ShiftName", typeof(String));
            RemaingWST_Tbl.Columns.Add("Required_Skill", typeof(String));
            RemaingWST_Tbl.Columns.Add("Index_InMasterTbl", typeof(int));

            // Search for the remaing list of WST & empl ID
            int CurrIndex = 0;
            foreach (DataRow row in KHSX_WS_dtb.Rows)
            {
                 if (row["Empl_ID"].ToString().Trim() == "") //Currently have no employee in this WST
                 {
                    DataRow newRow = RemaingWST_Tbl.NewRow();

                    newRow["LineID"] = row["LineID"];
                    newRow["WST_ID"] = row["WST_ID"];
                    newRow["ShiftName"] = row["ShiftName"];

                    string LineID = row["LineID"].ToString().Trim();
                    string WST_ID = row["WST_ID"].ToString().Trim();
                    string ShiftName = row["ShiftName"].ToString().Trim();

                    newRow["Required_Skill"] = GetRequiredSkillForThisWST(LineID, WST_ID, ShiftName);
                    newRow["Index_InMasterTbl"] = CurrIndex;

                    RemaingWST_Tbl.Rows.Add(newRow);
                    CurrIndex++;
                 }
            }

            // Create Datatable của các nhân viên chưa có việc làm
            DataTable RemaingEmpl_Tbl = new DataTable();

            RemaingEmpl_Tbl.Columns.Add("Empl_ID", typeof(String));
            RemaingEmpl_Tbl.Columns.Add("Empl_Name", typeof(String));

            // Search for the remaing list of WST & empl ID
            foreach (DataRow row in Avaliable_Empl_List_dtb.Rows)
            {
                if ((Empl_Status_Type)row["Status"] == Empl_Status_Type.Avaliable)
                {
                    DataRow newRow = RemaingEmpl_Tbl.NewRow();

                    newRow["Empl_ID"] = row["Empl_ID"];
                    newRow["Empl_Name"] = row["Empl_Name"];
                    RemaingEmpl_Tbl.Rows.Add(newRow);
                }
            }


            // Thay Thế Nhân Viên ít Skill hơn
            foreach (DataRow row in KHSX_WS_dtb.Rows)
            {
                wst_id = row["WST_ID"].ToString().Trim();
                shift = row["ShiftName"].ToString().Trim();
                cur_empl_Id = row["Empl_ID"].ToString().Trim();
                Cur_Empl_Name = row["Empl_Name"].ToString().Trim();

                string AlternateEmpID = "";
                string AlternateEmpName = "";
                int NewPlaceForCurrEmpl = -1;
                if ((cur_empl_Id != "") && (Check_Empl_Able_for_Other_WST(cur_empl_Id,
                                                                            ref AlternateEmpID,
                                                                            ref AlternateEmpName,
                                                                            ref NewPlaceForCurrEmpl,
                                                                            Avaliable_Empl_List_dtb, 
                                                                            RemaingWST_Tbl, 
                                                                            RemaingEmpl_Tbl) == true))
                {
                    //Thay đổi vị trí 2 nhân viên
                    row["Empl_ID"] = AlternateEmpID;
                    row["Empl_Name"] = AlternateEmpName;

                    DataRow newRow = KHSX_WS_dtb.Rows[NewPlaceForCurrEmpl];
                    newRow["Empl_ID"] = (string)cur_empl_Id;
                    newRow["Empl_Name"] = Cur_Empl_Name;

                    //empl_id = Get_Empl_New_Plan(date, "", wst_id, shift, ref Tmp_dtb);

                    //if (empl_id != "")
                    //{
                    //    Set_New_WST(cur_empl_Id, shift);
                    //    row["Empl_ID"] = empl_id;
                    //    row["Empl_Name"] = Get_Empl_Name(empl_id);
                    //    percent = (int)row["Shift_Percent"];
                    //    switch (shift)
                    //    {
                    //        case "0":
                    //            row["Shift_HC"] = (float)(8 * percent / 100);
                    //            break;
                    //        case "1":
                    //            row["Shift_1"] = (float)(8 * percent / 100);
                    //            break;
                    //        case "2":
                    //            row["Shift_2"] = (float)(8 * percent / 100);
                    //            break;
                    //        case "3":
                    //            row["Shift_3"] = (float)(8 * percent / 100);
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    }
                }
            }   
     }
}