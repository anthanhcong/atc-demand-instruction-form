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
using System.Threading;
using LayoutControl;
using DataGridViewAutoFilter;
using JobsDisplay.Statistics;

namespace JobsDisplay
{
    public partial class Form1 : SQL_APPL
    {
        const string FRU_WST = "FRU";
        const string FRU_LINE = "FRU";
        const string STAND_LINE = "STANDHH";

        class WST_Status
        {
            public string Line_ID = "";
            public string Line_Name = "";
            public string WST_ID = "";
            public string WST_Name = "";
            public string Shift = "";
            public string PO = "";
            public string Part = "";
        }

        private bool Create_Tracking(string msnv)
        {
            DateTime date = Cur_Date;
            string line_id;
            string wst_id;
            string plan_wst = "";
            DataTable job_plan;

            bool use_for_cur_PO;
            string po_wst;
            string[] wst_list;
            string empl_name = "";
            string cur_empl, out_time;
            frmClient frmLayout;
            Line line;

            Load_List_of_WST(Cur_Line_ID);
            int total = List_WST_dtb.Rows.Count;
            Cur_WST_ID = "";

            switch (Tracking_Mode)
            {
                case TRACKING_MODE.MANUAL_OUT:
                    return Empl_Check_out(msnv);
                    break;

                case TRACKING_MODE.MANUAL_IN:
                case TRACKING_MODE.NORMAL_IN:
                    empl_name = Get_Empl_Name(msnv);
                    // Get WST in Plan
                    job_plan = Load_Cur_JobsPlan(msnv, date);
                    if ((job_plan != null) && (job_plan.Rows.Count > 0))
                    {
                        plan_wst = job_plan.Rows[0]["WST_ID"].ToString().Trim();
                    }

                    // WST_plan in this Line or Not: if WST_Plan in this line set WST value to Cur_WST_ID
                    if (Tracking_Mode == TRACKING_MODE.NORMAL_IN)
                    {
                        wst_list = new string[total];
                        for (int i = 0; i < total; i++)
                        {
                            wst_list[i] = List_WST_dtb.Rows[i]["WST_ID"].ToString().Trim();
                            if ((plan_wst == wst_list[i]) && (plan_wst != ""))
                            {
                                Cur_WST_ID = plan_wst;
                            }
                        }
                    }

                    use_for_cur_PO = false;
                    foreach (DataRow row in Current_Line_Status.Rows)
                    {
                        po_wst = row["WST_ID"].ToString().Trim();
                        if (Cur_WST_ID == po_wst)
                        {
                            use_for_cur_PO = true;
                            break;
                        }
                    }

                    if (use_for_cur_PO == false)
                    {
                        if (MessageBox.Show("PO hiện tại không bao gồm vị trí (WST) trong kế hoạch của Bạn. \nBạn có vào WST khác không?", "Warning", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        {
                            MessageBox.Show(msnv + ": Bạn vui lòng về Line STAND", "Warning");
                            return false;
                        }
                    }


                    // If WST_Plan no in current line ==> Allow to select this WST  
                    if ((Cur_WST_ID == "") || (Is_Empty_WST(Cur_WST_ID) == false))
                    {
                        if (Cur_WST_ID == "")
                        {
                            if (MessageBox.Show("Bạn không được sắp trong line này. \nBạn có chắc bạn sẽ vào line này không?", "Warning", MessageBoxButtons.YesNo) != DialogResult.Yes)
                            {
                                return false;
                            }
                        }
                        else if (Is_Empty_WST(Cur_WST_ID) == false)
                        {
                            if (MessageBox.Show("WST làm việc của bạn trong kế hoạch đã có nhân viên khác làm. \nBạn có vào WST khác không?", "Warning", MessageBoxButtons.YesNo) != DialogResult.Yes)
                            {
                                return false;
                            }
                        }

                        use_for_cur_PO = false;
                        foreach (DataRow row in Current_Line_Status.Rows)
                        {
                            po_wst = row["WST_ID"].ToString().Trim();
                            if (Cur_WST_ID == po_wst)
                            {
                                use_for_cur_PO = true;
                                break;
                            }
                        }

                        if (use_for_cur_PO == true)
                        {
                            if (MessageBox.Show("PO hiện tại không bao gồm vị trí (WST) trong kế hoạch của Bạn. \nBạn có vào WST khác không?", "Warning", MessageBoxButtons.YesNo) != DialogResult.Yes)
                            {
                                MessageBox.Show(msnv + ": Bạn vui lòng về Line STAND", "Warning");
                                return false;
                            }
                        }

                        frmLayout = new frmClient("Please Select WST", Cur_Line_ID, MasterDatabase_Connection_Str);
                        line = frmLayout.GetLineInstant();

                        line.SetDisableLine();

                        // Create WST Ready List
                        foreach (DataRow row in Current_Line_Status.Rows)
                        {
                            wst_id = row["WST_ID"].ToString().Trim();
                            cur_empl = row["Empl_ID"].ToString().Trim();
                            out_time = row["To_Time"].ToString().Trim();
                            if ((cur_empl != "") && (out_time == ""))
                            {
                                line.SetReady_WST(wst_id, cur_empl);
                            }
                            else
                            {
                                line.SetInactiveWST(wst_id);
                            }
                        }

                        if (frmLayout.ShowDialog() == DialogResult.OK)
                        {
                            Cur_WST_ID = frmLayout.WST_Selected.Trim();
                            if (Cur_WST_ID == "")
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }

                    if ((Cur_WST_ID != null) && (Cur_WST_ID != ""))
                    {
                        // Show Current Empty Position
                        frmLayout = new frmClient("", Cur_Line_ID, MasterDatabase_Connection_Str);
                        line = frmLayout.GetLineInstant();
                        line.SetActiveWST(Cur_WST_ID, "");
                        frmLayout.ShowDialog();

                        /********************************************************************************************/
                        //@TODO (1): need to apply: Một nhân viên không được làm 2 line 
                        //      --> nhưng một nhân viên có thể làm nhiều trạm trên cùng 1 line
                        //      Load_Cur_JobsPlan_Details(msnv, date);
                        Load_Job_Tracking(msnv, date);

                        // Close Current Line 
                        foreach (DataRow row in JobsTracking_dtb.Rows)
                        {
                            line_id = row["LineID"].ToString().Trim();
                            wst_id = row["WST_ID"].ToString().Trim();
                            out_time = row["To_Time"].ToString().Trim();
                            //if ((line_id != Cur_Line_ID) && (out_time != ""))
                            if ((line_id != Cur_Line_ID) && (out_time == "")) // edit Thuy
                            {
                                // row["To_Time"] = Cur_Date.TimeOfDay;
                                // Hiển thị thông báo một người không được làm 2 line và thoát ra
                                MessageBox.Show("Một người không được làm 2 line", "Warning");
                                return false;
                            }
                        }
                        /********************************************************************************************/
                        // add new record
                        // Check In
                        Empl_Check_in(Cur_WST_ID, msnv, empl_name);
                        Update_SQL_Data(Current_Line_Status_da, Current_Line_Status);
                        return true;
                    }
                    break;
                case TRACKING_MODE.FRU_IN:
                    empl_name = Get_Empl_Name(msnv);
                    if ((Cur_Part == "") || (Cur_PO == ""))
                    {
                        MessageBox.Show("Bạn chưa chọn PO.\nVui Lòng Chọn PO!", "Cảnh Báo");
                        return false;
                    }

                    if (MessageBox.Show("Bạn sẽ làm PO " + FRU_Rb.Text.Trim() + "?", "Cảnh Báo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return false;
                    }

                    frmLayout = new frmClient("Please Select WST", Cur_Line_ID, MasterDatabase_Connection_Str);
                    line = frmLayout.GetLineInstant();

                    line.SetInactiveLine();

                    // Create WST Ready List
                    foreach (DataRow row in Current_Line_Status.Rows)
                    {
                        wst_id = row["WST_ID"].ToString().Trim();
                        line.SetDisable_WST(wst_id);
                    }

                    if (frmLayout.ShowDialog() == DialogResult.OK)
                    {
                        Cur_WST_ID = frmLayout.WST_Selected.Trim();
                        if (Cur_WST_ID == "")
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                    frmLayout = new frmClient("", Cur_Line_ID, MasterDatabase_Connection_Str);
                    line = frmLayout.GetLineInstant();
                    line.SetActiveWST(Cur_WST_ID, "");
                    frmLayout.ShowDialog();

                    Empl_FRU_Check_in(Cur_WST_ID, msnv, empl_name);

                    break;
            }
            return false;
        }


        private WST_Status Get_WST_Status(string line_id, string wst_id, DateTime current)
        {
            WST_Status wst_status = new WST_Status();
            DataTable plan_tbl;
            plan_tbl = Load_Production_Plan(line_id, wst_id, current);
            if ((plan_tbl != null) && (plan_tbl.Rows.Count > 0))
            {
                DataRow row = plan_tbl.Rows[0];
                wst_status.Line_ID = line_id;
                wst_status.Line_Name = row["LineName"].ToString().Trim();
                wst_status.WST_ID = wst_id;
                wst_status.WST_Name = row["WST_Name"].ToString().Trim();
                wst_status.Shift = Get_Shift_ID(current); //row["ShiftName"].ToString().Trim();
                wst_status.PO = row["PO"].ToString().Trim();
                wst_status.Part = row["PartNumber"].ToString().Trim();
            }
            return wst_status;
        }

        private bool Load_List_PO (DateTime date)
        {
            DataTable list_po = Get_Kitting_Data(date);
            BindingSource bs = new BindingSource();
            bs.DataSource = list_po;
            Tracking_Kitting_PO_Grv.DataSource = bs;
            Jobs_GridView_BindingContextChanged(null, null);
            
            return true;
        }

        private bool Check_Correct_Part_inLine(string lineid, string part)
        {
            string request_line = Get_Line_ID_of_Part(part);

            if ((lineid == request_line) && (request_line != ""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Check_Build_PO_Part(DateTime date, string po)
        {
            DataTable tracking_tables = Get_Tracking_PO_Date(date, po);
            if ((tracking_tables != null) && (tracking_tables.Rows.Count > 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Check_PO_Running(DateTime date, string po)
        {
            string select_po;
            if (Current_Line_Status == null)
            {
                Load_Current_Line_Status(date);
            }

            foreach (DataRow row in Current_Line_Status.Rows)
            {
                select_po = row["PO"].ToString().Trim();
                if (po == select_po)
                {
                    return true;
                }
            }
            return false;
        }

        private string  Check_Conflict_WST(string part)
        {
            DataTable list_wst = Load_WST_Part(part);
            string part_wst, line_wst;
            string wst_conflict = "";
            foreach (DataRow row in list_wst.Rows)
            {
                part_wst = row["WST_ID"].ToString().Trim();
                foreach (DataRow ws_row in Current_Line_Status.Rows)
                {
                    line_wst = ws_row["WST_ID"].ToString().Trim();
                    if ((part_wst == line_wst)&& (part_wst!= ""))
                    {
                        wst_conflict += "\t" + part_wst + "\n";
                    }
                }
            }
            return wst_conflict;
        }

        private bool Add_WST_for_Build_Part(DateTime date, string po, string part, string shift)
        {
            DataTable list_wst = Load_WST_Part(part);

            foreach (DataRow row in list_wst.Rows)
            {
                DataRow new_record = Current_Line_Status.NewRow();
                new_record["Date"] = Cur_Date.Date;
                new_record["PartNumber"] = part;
                new_record["PO"] = po;
                new_record["LineID"] = Cur_Line_ID;
                new_record["LineName"] = row["LineName"];
                new_record["WST_ID"] = row["WST_ID"];
                new_record["WST_Name"] = row["WST_Name"];
                new_record["SubLine_ID"] = row["SubLine_ID"];
                new_record["SubLine_Name"] = row["SubLine_Name"];
                // new_record["ShiftName"] = Get_Shift_ID(Cur_Date);
                new_record["ShiftName"] = Get_Current_Shift_Name(Cur_Line_ID, Cur_Date);
                Current_Line_Status.Rows.Add(new_record);
            }
            Update_SQL_Data(Current_Line_Status_da, Current_Line_Status);
            return true;
        }

        private bool Empl_Check_in(string wst, string empl_id, string empl_name)
        {
            string select_wst;
            bool ret = false;
            foreach (DataRow row in Current_Line_Status.Rows)
            {
                select_wst = row["WST_ID"].ToString().Trim();
                if (select_wst == wst)
                {
                    row["Empl_ID"] = empl_id;
                    row["Empl_Name"] = empl_name;
                    row["From_Time"] = DateTime.Now.TimeOfDay;
                    ret = true;
                }
            }
            Update_SQL_Data(Current_Line_Status_da, Current_Line_Status);
            return ret;
        }

        private bool Empl_FRU_Check_in(string wst, string empl_id, string empl_name)
        {
            string select_wst;
            bool ret = false;
            foreach (DataRow row in Current_Line_Status.Rows)
            {
                select_wst = row["WST_ID"].ToString().Trim();
                if (select_wst == wst)
                {
                    return false;
                }
            }

            DataRow new_row = Current_Line_Status.NewRow();
            new_row["Date"] = Cur_Date.Date;
            new_row["ShiftName"] = Get_Current_Shift_Name(Cur_Line_ID, Cur_Date);
            new_row["LineID"] = Cur_Line_ID;
            new_row["SubLine_ID"] = FRU_Rb.Text.Trim();
            new_row["WST_ID"] = wst;
            new_row["PO"] = Cur_PO;
            new_row["PartNumber"] = Cur_Part;
            new_row["Empl_ID"] = empl_id;
            new_row["Empl_Name"] = empl_name;
            new_row["From_Time"] = DateTime.Now.TimeOfDay;
            Current_Line_Status.Rows.Add(new_row);

            Update_SQL_Data(Current_Line_Status_da, Current_Line_Status);
            return ret;
        }
        

        private bool Show_Current_Line_Status()
        {
            // HH02_09
            if (Display_Thread != null)
            {
                Display_Thread.Abort();
            }

            Load_Current_Line_Status(Cur_Date);

            Display_Thread = new Thread(() => Show_Display());//The first show of layout with all ready layout
            Display_Thread.SetApartmentState(ApartmentState.STA);
            Display_Thread.Start();

            AutoCheck_Timer.Stop();
            AutoCheck_Timer.Interval = 3000;
            Wait_Counter = 0;
            AutoCheck_Timer.Start();

            return true;
        }

        private bool Empl_Check_out(string empl_id)
        {
            string select_empl;
            bool ret = false;
            string subline = "";
            DataTable temp_tbl = Current_Line_Status.Clone();
            foreach (DataRow row in Current_Line_Status.Rows)
            {
                select_empl = row["Empl_ID"].ToString().Trim();
                if (select_empl == empl_id)
                {
                    subline = row["SubLine_ID"].ToString().Trim();
                    row["To_Time"] = DateTime.Now.TimeOfDay;
                    row["Out_Manual"] = true;
                    temp_tbl.ImportRow(row);
                    ret = true;
                }
            }

            if ((subline != FRU_WST)&&(subline != STAND_LINE))
            {
                if (ret == true)
                {
                    foreach (DataRow temp_row in temp_tbl.Rows)
                    {
                        DataRow new_record = Current_Line_Status.NewRow();
                        new_record["Date"] = temp_row["Date"];
                        new_record["PartNumber"] = temp_row["PartNumber"];
                        new_record["PO"] = temp_row["PO"];
                        new_record["LineID"] = Cur_Line_ID;
                        new_record["LineName"] = temp_row["LineName"];
                        new_record["WST_ID"] = temp_row["WST_ID"];
                        new_record["WST_Name"] = temp_row["WST_Name"];
                        new_record["SubLine_ID"] = temp_row["SubLine_ID"];
                        new_record["SubLine_Name"] = temp_row["SubLine_Name"];
                        new_record["ShiftName"] = temp_row["ShiftName"];

                        Current_Line_Status.Rows.Add(new_record);
                    }
                }
            }
            Update_SQL_Data(Current_Line_Status_da, Current_Line_Status);
            return ret;
        }

        private bool Close_PO(string po)
        {
            string select_po;
            bool ret = false;
            foreach (DataRow row in Current_Line_Status.Rows)
            {
                select_po = row["PO"].ToString().Trim();
                if (select_po == po)
                {
                    row["To_Time"] = DateTime.Now.TimeOfDay;
                    ret = true;
                }
            }
            Update_SQL_Data(Current_Line_Status_da, Current_Line_Status);
            Load_Current_Line_Status(Cur_Date);
            return ret;
        }

        private bool Close_Line()
        {
            foreach (DataRow row in Current_Line_Status.Rows)
            {
                row["To_Time"] = DateTime.Now.TimeOfDay;
            }
            Update_SQL_Data(Current_Line_Status_da, Current_Line_Status);
            Load_Current_Line_Status(Cur_Date);
            return true;
        }

        private bool Assign_Cur_Empl()
        {
            string wst, shift;
            string empl_id, empl_name;
            string [] empl_info;
            foreach (DataRow row in Current_Line_Status.Rows)
            {
                wst = row["WST_ID"].ToString().Trim();
                shift = row["ShiftName"].ToString().Trim();
                empl_id = row["Empl_ID"].ToString().Trim();
                if (empl_id == "")
                {
                    empl_info = Get_Empl_for_WST(wst, shift);
                    empl_id = empl_info[0];
                    empl_name = empl_info[1];
                    if ((empl_id != null) && (empl_id != ""))
                    {
                        row["Empl_ID"] = empl_id;
                        row["Empl_Name"] = empl_name;
                        row["From_Time"] = DateTime.Now.TimeOfDay;
                    }
                }
            }

            Update_SQL_Data(Current_Line_Status_da, Current_Line_Status);
            Load_Current_Line_Status(Cur_Date);

            //TODO: SWAP EMPL
            return true;
        }

        private void Show_Display()
        {
            string wst_id, cur_empl, out_time;
            string subline;

            frmClient frmLayout = new frmClient("", Cur_Line_ID, MasterDatabase_Connection_Str);
            Line line = frmLayout.GetLineInstant();
            line.SetInactiveLine();
            line.SetDisableLine();

            // Create WST Ready List & WST Inactive List
            foreach (DataRow row in Current_Line_Status.Rows)
            {
                subline = row["SubLine_ID"].ToString().Trim();
                wst_id = row["WST_ID"].ToString().Trim();
                cur_empl = row["Empl_ID"].ToString().Trim();
                out_time = row["To_Time"].ToString().Trim();
                if ((subline == FRU_LINE) || (subline == STAND_LINE))
                {
                    if ((cur_empl != "") && (out_time == ""))
                    {
                        line.SetReady_FRU_WST(wst_id, cur_empl);
                    }
                    else
                    {
                        line.SetReady_FRU_Inactive_WST(wst_id);
                    }
                }
                else
                {
                    if ((cur_empl != "") && (out_time == ""))
                    {
                        line.SetReady_WST(wst_id, cur_empl);
                    }
                    else
                    {

                        line.SetInactiveWST(wst_id);
                    }
                }
            }
            frmLayout.ShowDialog();
        }


        private bool Is_Empty_WST(string wst)
        {
            /********************************************************************************************/
            //TODO (2): need to apply: Trạm đã có người không cho nhân viên khác vô
            /********************************************************************************************/
            string wst_id, cur_empl, out_time;

            // Create WST Ready List
            foreach (DataRow row in Current_Line_Status.Rows)
            {
                wst_id = row["WST_ID"].ToString().Trim();
                cur_empl = row["Empl_ID"].ToString().Trim();
                out_time = row["To_Time"].ToString().Trim();
                if ((cur_empl != "") && (out_time == "") && (wst == wst_id))
                {
                    return false;
                }
            }
            return true;
        }
    }
}