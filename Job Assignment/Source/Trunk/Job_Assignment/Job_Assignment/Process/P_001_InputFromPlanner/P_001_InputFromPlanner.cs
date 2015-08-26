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
        void InputFromPlannerList_Create_BT_Click(object sender, EventArgs e)
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
                Coppy_Plan_From_Kitting(date);

            }
        }

        private bool Coppy_Plan_From_Kitting(DateTime date)
        {
            bool b;
            string sql_cmd;
            DataTable kitting_table;
            string po, part, qty_str, priority_str;
            int count, qty, priority;
            string mess;
            string error_mess = "";
            int i = 0, total;

            StatusLabel1.Visible = true;
            ProgressBar1.Visible = true;
            StatusLabel1.Text = "Get Plan From Kitting";

            // lay du lieu ke hoach sx theo line cua ngay da chon
            sql_cmd = String.Format("SELECT * FROM [P_001_InputFromPlanner] WHERE [Date] = '{0}' order by Priority", date.ToString("yyyy-MMM-dd"));
            b = InputFromPlannerList_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            if (b == false)
            {
                return false;
            }

            count = InputFromPlannerList_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Count;
            if (count > 0)
            {
                mess = "Plan for date:" + date.ToString("dd MMM yyyy") + "was existing\n";
                mess += "Do you want to delete and create the new one?";

                if (MessageBox.Show(mess, "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }
                //TODO: Add Funtion delete existing data in P_001_InputFromPlanner by date
                DeleteInputFromPlaner(date);
                InputFromPlannerList_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            }

            //TODO: Add Funtion copy data from kitting to P_001_InputFromPlanners
            kitting_table = Get_Kitting_Data(date);

            total = kitting_table.Rows.Count;

            foreach (DataRow row in kitting_table.Rows)
            {
                po = row["TopPONumber"].ToString().Trim();
                part = row["TopModel"].ToString().Trim();
                qty_str = row["POQty"].ToString().Trim();
                priority_str = row["Priority"].ToString().Trim();

                if (Check_Exit_PO_In_Kitting(InputFromPlannerList_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb, po) == false)
                {
                    try
                    {
                        qty = Convert.ToInt32(qty_str);
                        priority = Convert.ToInt32(priority_str);
                    }
                    catch
                    {
                        qty = 0;
                        priority = 0;
                    }
                    DataRow new_row = InputFromPlannerList_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.NewRow();
                    new_row["Date"] = date;
                    new_row["PO"] = po;
                    if ((po[0] == '5') || (po[0] == '8'))
                    {
                        new_row["PartNumber"] = part + "_R"; ;
                    }
                    else new_row["PartNumber"] = part;
                    new_row["Qty"] = qty;
                    new_row["Priority"] = priority;
                    InputFromPlannerList_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Add(new_row);
                }
                else
                {
                    error_mess += "PO: " + po + "  -  Part: " + part + "\n";
                }
                i++;
                ProgressBar1.Value = i * 100 / total;
            }

            Update_SQL_Data(InputFromPlannerList_MasterDatabase.MasterDatabase_GridviewTBL.Data_da, InputFromPlannerList_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb);
            if (error_mess != "")
            {
                error_mess = "Date:" + date.ToString("dd MMM yyyy") + "\nHas Duplicate PO in Plan: \n" + error_mess;
                MessageBox.Show(error_mess, "Warning");
            }

            StatusLabel1.Visible = false;
            ProgressBar1.Visible = false;
            return true;
        }

        private bool Check_Exit_PO_In_Kitting(DataTable cur_plan, string po)
        {
            string cur_po = "";
            foreach (DataRow row in cur_plan.Rows)
            {
                cur_po = row["PO"].ToString().Trim();
                if (cur_po == po.Trim())
                {
                    return true;
                }
            }
            return false;
        }
    }
}