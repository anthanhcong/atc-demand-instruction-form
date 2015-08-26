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
        enum TRACKING_MODE
        {
            NORMAL_IN,
            MANUAL_IN,
            FRU_IN,
            STAND_IN,
            MANUAL_OUT,
        }

        TRACKING_MODE Tracking_Mode = new TRACKING_MODE();
        private Thread Display_Thread;

        const int FORCE_CLOSE_TIME = 10;        // 10s
        const int AUTO_CHECKPO_TIME = 20;       // 20s
        const int CHECK_STATUS_LINE_TIME = 300; // 5min
        string Last_Empl = "";

        private void Tracking_Mode_Change(object sender, EventArgs e)
        {
            if (Setting_In_Check.Checked == true)
            {
                Tracking_Mode = TRACKING_MODE.NORMAL_IN;
            }
            else if (In_Manual_Rb.Checked == true)
            {
                Tracking_Mode = TRACKING_MODE.MANUAL_IN;
            }
            else if (FRU_Rb.Checked == true)
            {
                Tracking_Mode = TRACKING_MODE.FRU_IN;
            }
            else if (Setting_Out_Check.Checked == true)
            {
                Tracking_Mode = TRACKING_MODE.MANUAL_OUT;
            }
        }

        private void Tracking_MSNV_Txt_TextChanged(object sender, EventArgs e)
        {
            string msnv = Tracking_MSNV_Txt.Text.Trim();

            Close_Logger_Comport();
            Last_Empl = msnv;

            if (msnv.Length < 8)
            {
                return;
            }
            string empl_name = Get_Empl_Name(msnv);
            Tracking_EmplName_Lbl.Text = empl_name;
            Tracking_Shift_LBL.Text = Get_Shift_ID(Cur_Date);

            // HH02_09
            if (Display_Thread != null)
            {
                Display_Thread.Abort();
            }
            Create_Tracking(msnv); //The first show of layout. Just only the WST that activated
            Show_Current_Line_Status();
            Open_Logger_Comport();
        }

        private void Tracking_Kitting_PO_Grv_BindingContextChanged(object sender, EventArgs e)
        {
            if (Tracking_Kitting_PO_Grv.DataSource == null) return;

            foreach (DataGridViewColumn col in Tracking_Kitting_PO_Grv.Columns)
            {
                col.HeaderCell = new DataGridViewAutoFilterColumnHeaderCell(col.HeaderCell);
            }
            Tracking_Kitting_PO_Grv.AutoResizeColumns();
        }

        private void Tracking_Kitting_PO_Grv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((Tracking_Kitting_PO_Grv.SelectedCells != null)
               && (Tracking_Kitting_PO_Grv.SelectedCells.Count > 0))
            {
                DataGridViewCell cur_cell = Tracking_Kitting_PO_Grv.SelectedCells[0];
                int col = cur_cell.ColumnIndex;
                int row_index = cur_cell.RowIndex;
                string po = Tracking_Kitting_PO_Grv.Rows[row_index].Cells["TopPONumber"].Value == null ? "" : Tracking_Kitting_PO_Grv.Rows[row_index].Cells["TopPONumber"].Value.ToString().Trim();
                string part = Tracking_Kitting_PO_Grv.Rows[row_index].Cells["TopModel"].Value == null ? "" : Tracking_Kitting_PO_Grv.Rows[row_index].Cells["TopModel"].Value.ToString().Trim();
                Tracking_PartNumber_Txt.Text = part;
                Traking_PO.Text = po;
                Cur_Part = part;
                Cur_PO = po;
            }
        }

        private void Tracking_Status_GridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((Tracking_Status_GridView.SelectedCells != null)
               && (Tracking_Status_GridView.SelectedCells.Count > 0))
            {
                DataGridViewCell cur_cell = Tracking_Status_GridView.SelectedCells[0];
                int col = cur_cell.ColumnIndex;
                int row_index = cur_cell.RowIndex;
                string po = Tracking_Status_GridView.Rows[row_index].Cells["PO"].Value == null ? "" : Tracking_Status_GridView.Rows[row_index].Cells["PO"].Value.ToString().Trim();
                string part = Tracking_Status_GridView.Rows[row_index].Cells["PartNumber"].Value == null ? "" : Tracking_Status_GridView.Rows[row_index].Cells["PartNumber"].Value.ToString().Trim();
                Tracking_PartNumber_Txt.Text = part;
                Traking_PO.Text = po;
                Cur_Part = part;
                Cur_PO = po;
            }
        }

        private void Tracking_LayoutBT_Click(object sender, EventArgs e)
        {
            Show_Current_Line_Status();
        }

        private void Tracking_GridView_BindingContextChanged(object sender, EventArgs e)
        {
            string col_name;
            if (Tracking_Status_GridView.DataSource == null) return;

            foreach (DataGridViewColumn col in Tracking_Status_GridView.Columns)
            {
                col_name = col.Name.ToString().Trim();
                if ((col_name == "Empl_ID") || (col_name == "Empl_Name") || (col_name == "ShiftName")
                    || (col_name == "WST_ID") || (col_name == "LineID") || (col_name == "SubLine_ID")
                    || (col_name == "PartNumber") || (col_name == "PO")
                    || (col_name == "From_Time") || (col_name == "To_Time"))
                {
                    col.Visible = true;
                }
                else
                {
                    col.Visible = false;
                }
            }
            YourJob_GridView.AutoResizeColumns();
        }

        private void Tracking_RefreshPO_BT_Click(object sender, EventArgs e)
        {
            DateTime date = Cur_Date;
            Load_List_PO(date);
        }

        /*************************************************************************
             ##### #######  ###   ###### #######    ##### #######  #####  ######
            ##        #     # #   #    #    #      ##        #   ##     # #    #
              ####    #    #   #  ######    #        ####    #   ##     # ######
            #    ##   #   ####### #    #    #      ##   ##   #    #    ## #     
             #####    #   #     # #    #    #       #####    #     ####   #     
         * 
                                       #####    ####                            
                                       #    # ##   ##                           
                                       # # ## #     ##                          
                                       #      #     #                           
                                       #       #####                            

         ***************************************************************************/

        string[] close_his_PO = { "" };

        private void Tracking_StopPO_BT_Click(object sender, EventArgs e)
        {
            Cur_PO = Traking_PO.Text.Trim();
            Cur_Part = Tracking_PartNumber_Txt.Text.Trim();
            Close_PO(Cur_PO);
            close_his_PO[0] = Cur_PO;
        }

        private void Tracking_StartPO_BT_Click(object sender, EventArgs e)
        {
            string mess;
            string shift;

            string close_PO = close_his_PO[0].ToString().Trim();

            if ((Cur_Line_ID == FRU_LINE) || (Cur_Line_ID == STAND_LINE))
            {
                MessageBox.Show("Bạn đang ở Line: " + Cur_Line_ID + "\nKhông cần Start PO", "Thông Báo");
                return;
            }

            //TODO: Implement start PO
            Cur_Date = DateTime.Now.AddMinutes(10);
            Cur_Date = Cur_Date.Hour < 6 ? Cur_Date.AddDays(-1) : Cur_Date;
            Cur_PO = Traking_PO.Text.Trim();
            Cur_Part = Tracking_PartNumber_Txt.Text.Trim();
            shift = Get_Current_Shift_Name(Cur_Line_ID, Cur_Date);
            Cur_EmtyForm_State = JobsDisplay.Statistics.EmptyFormState.GET_MORE;
            AutoCheck_Timer.Start();

            // Check Correct PO input
            if ((Cur_PO == "") || (Cur_Part == null))
            {
                MessageBox.Show("Please Select PO Input!", "Warning");
                return;
            }

            // Check part able to build in this line
            if (Check_Correct_Part_inLine(Cur_Line_ID, Cur_Part) == false)
            {
                MessageBox.Show("This PO is not able build in this line!", "Warning");
                return;
            }

            // Check PO is runing or not
            if (Check_PO_Running(Cur_Date, Cur_PO) == true)
            {
                MessageBox.Show("This PO is running!", "Warning");
                return;
            }

            if (Check_Build_PO_Part(Cur_Date, Cur_PO) == true)
            {
                if (MessageBox.Show("This PO is already build. Do you want to Repoen this PO", "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                // add by thuy
                DataTable his_PO_Close = Get_Count_WST_HisPO(Cur_Date, close_PO);
                DataTable cur_PO_Open = Get_Count_WST_CurPO(Cur_Date, Cur_PO);
                string his_WST, cur_WST;
                string message = "";
                string message_wst_lamduoc = "";
                string message_wst_khonglamduoc = "";
                int count_his = his_PO_Close.Rows.Count;
                int count_cur = cur_PO_Open.Rows.Count;
                int count;
                string empl_ID_hisPO, empl_Name_hisPO, his_WSTPO;
                string[] empl_hisPO;
                string wst;

                for (int i = 0; i < count_his; i++)
                {
                    his_WST = his_PO_Close.Rows[i]["WST_ID"].ToString().Trim();
                    count = 0;
                    for (int j = 0; j < count_cur; j++)
                    {
                        cur_WST = cur_PO_Open.Rows[j]["WST_ID"].ToString().Trim();
                        
                        if (his_WST != cur_WST)
                        {
                            count++;
                        }
                    }

                    // Hiển thị trạm dư, người dư
                    // người dư làm được vị trí nào trong line hay không?
                    if (count == count_cur)
                    {
                        empl_hisPO = Get_Empl_Du(Cur_Date, close_PO, his_WST);
                        his_WSTPO = empl_hisPO[2];
                        if (his_WST == his_WSTPO)
                        {
                            empl_ID_hisPO = empl_hisPO[0].ToString().Trim();
                            empl_Name_hisPO = empl_hisPO[1];
                            if (empl_ID_hisPO != null &&  empl_ID_hisPO.Length > 0)
                            {
                                foreach (DataRow row in cur_PO_Open.Rows)
                                {
                                    wst = row["WST_ID"].ToString().Trim();
                                    LoadInternalData();
                                    if (IsEmplHaveEnoughSkill(empl_ID_hisPO, wst))
                                    {
                                        message_wst_lamduoc += empl_ID_hisPO + " có thể làm được " + wst + "\n";
                                    }
                                    else
                                    {
                                        message_wst_khonglamduoc += empl_ID_hisPO + " không thể làm được " + wst + "\n";
                                    }
                                }
                                message += his_WST + " \tEmpl_ID: " + empl_ID_hisPO + " \tEmpl_Name: " + empl_Name_hisPO + "\n";
                            }
                        }
                    }
                }

                if (message != null && message != "")
                {
                    MessageBox.Show("Dư người ở trạm:\n" + message, "Information");
                }

                if (message_wst_lamduoc != null && message_wst_lamduoc != "")
                {
                    MessageBox.Show("Các trạm làm được trong line: \n" + message_wst_lamduoc, "Information");
                }

                if (message_wst_khonglamduoc != null && message_wst_khonglamduoc != "")
                {
                    MessageBox.Show("Các trạm không làm được trong line:\n" + message_wst_khonglamduoc, "Information");
                }
            }
            //end

            mess = Check_Conflict_WST(Cur_Part);
            if (mess != "")
            {
                mess = "WST not ready for build PO: " + Cur_PO + ":\n" + mess;
                MessageBox.Show(mess, "Warning");
                return;
            }

            Add_WST_for_Build_Part(Cur_Date, Cur_PO, Cur_Part, shift);
            Update_SQL_Data(Current_Line_Status_da, Current_Line_Status);

            Load_Current_Line_Status(Cur_Date);
            Assign_Cur_Empl();
        }

        private void AutoCheck_Timer_Tick(object sender, EventArgs e)
        {
            AutoCheck_Timer.Stop();
            DataTable list_cur_po;
            string check_po, done_po_list = "", partnumber, shift;

            bool AutoCheck_PO = false;

            ForceClose_Counter++;
            CheckPO_Counter++;
            Wait_Counter++;

            if (ForceClose_Counter >= FORCE_CLOSE_TIME)
            {
                ForceClose_Counter = 0;
                if (IsForceClose() == true)
                {
                    Close_Line();
                    if (Display_Thread != null)
                    {
                        Display_Thread.Abort();
                    }
                    this.Close();
                }
            }


            // if ((CheckPO_Counter >= AUTO_CHECKPO_TIME) && (Mode == Working_Mode.TRACKING))
            if ((CheckPO_Counter >= AUTO_CHECKPO_TIME) && (Mode == Working_Mode.TRACKING) && (AutoCheck_PO == true))
            {
                // check Close DONE PO
                CheckPO_Counter = 0;

                list_cur_po = Get_Line_Cur_PO(Cur_Line_ID, Cur_Date);

                foreach (DataRow row in list_cur_po.Rows)
                {
                    check_po = row["PO"].ToString().Trim();
                    if (Is_Done_PO(check_po) == true)
                    {
                        Close_PO(check_po);
                    }
                }

                // Open New_PO
                list_cur_po = Get_Kitting_Cur_PO(Cur_Line_ID);
                shift = Get_Current_Shift_Name(Cur_Line_ID, DateTime.Now);
                if (list_cur_po != null)
                {
                    foreach (DataRow row in list_cur_po.Rows)
                    {
                        check_po = row["TopPONumber"].ToString().Trim();
                        partnumber = row["TopModel"].ToString().Trim();
                        if (Check_PO_Running(Cur_Date, check_po) == false)
                        {
                            // Auto Start PO
                            Traking_PO.Text = check_po;
                            Tracking_PartNumber_Txt.Text = partnumber;
                            Tracking_StartPO_BT_Click(null, null);
                        }
                    }
                }
            }

            if (Wait_Counter >= CHECK_STATUS_LINE_TIME)
            {
                Wait_Counter = 0;
                if ((Mode == Working_Mode.TRACKING) && (Cur_EmtyForm_State == EmptyFormState.GET_MORE))
                {
                    if (HasEmptyWST() == true)
                    {
                        if (Display_Thread != null)
                        {
                            Display_Thread.Abort();
                        }

                        EmptyWST_vs_Employee frmLayout = new EmptyWST_vs_Employee(MasterDatabase_Connection_Str, Cur_Line_ID, Cur_Date, "Shift_2");
                        frmLayout.ShowDialog();
                        Cur_EmtyForm_State = frmLayout.State;
                        switch (Cur_EmtyForm_State)
                        {
                            case EmptyFormState.GET_MORE:
                                break;
                            case EmptyFormState.RUN_WITH_CURRENT:
                                break;
                            case EmptyFormState.STOP_LINE:
                                Cur_EmtyForm_State = EmptyFormState.GET_MORE;
                                Close_Line();
                                break;
                        }
                        ForceClose_Counter = FORCE_CLOSE_TIME;
                        Show_Current_Line_Status();
                    }
                }
            }
            AutoCheck_Timer.Start();
        }
        private void ForceClose_Timer_Tick(object sender, EventArgs e)
        {
            ForceClose_Timer.Stop();
            ForceClose_Timer.Start();
        }


        private void Tracking_Find_BT_Click(object sender, EventArgs e)
        {
            Wait_Counter = CHECK_STATUS_LINE_TIME;
            Cur_EmtyForm_State = EmptyFormState.GET_MORE;
            AutoCheck_Timer_Tick(null, null);
        }
    }
}