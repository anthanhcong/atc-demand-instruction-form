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
        void PlanByDate_Create_BT_Click(object sender, EventArgs e)
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
                Create_PlanByDate(date);
            }
        }

        private bool Create_PlanByDate(DateTime date)
        {
            int count;
            string mess, sql_cmd;
            string strConnectionStringSourceDel = MasterDatabase_Connection_Str;
            string strConnectionStringTarget = MasterDatabase_Connection_Str;
            bool b;
            int i = 0, total;

            StatusLabel1.Visible = true;
            ProgressBar1.Visible = true;
            StatusLabel1.Text = "Create_PlanByDate";

            FormatDataGridViewDisplay();
            DateSelect_Dialog_Form selectDate_Dialog = new DateSelect_Dialog_Form(DateTime.Now);
            //load data by date
            sql_cmd = String.Format("SELECT * FROM [P_002_PlanForProductionByDate] WHERE [Date] = '{0}' order by LineId", date.ToString("yyyy-MM-dd"));
            b = KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);

            if (b == false)
            {
                return false;
            }

            count = KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Count;
            if (count > 0)
            {
                mess = "Plan for date:" + date.ToString("dd MMM yyyy") + "was existing\n";
                mess += "Do you want to delete and create the new one?";

                if (MessageBox.Show(mess, "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }

                DeletePlanForProductionByDate(date);
            }

            //Prepare connection string for databases ----------------------------------------------------------------
            //Prepare temporary tables to store data -----------------------------------------------------------------                   
            DataTable dtJoinPOFromP001 = null;
            //DataTable dtCopyFromP001 = null;

            string strJoinPO = @"SELECT DATEADD(day,0,DATEDIFF(day,0,Date)) as Date, PartNumber, SUM(Qty) as Qty, MIN(Priority) as Priority " +
                            "FROM P_001_InputFromPlanner " +
                            "WHERE DATEADD(day,0,DATEDIFF(day,0,Date))='" + date.Date.ToString("dd MMM yyyy") + "' " +
                            "GROUP BY DATEADD(day,0,DATEDIFF(day,0,Date)), PartNumber";
            dtJoinPOFromP001 = Get_MasterDatabase_Data(date, strJoinPO);

            //Copy data to PlanForProductionByDate Table -------------------------------------------------                    

            total = dtJoinPOFromP001.Rows.Count;
            foreach (DataRow row in dtJoinPOFromP001.Rows)
            {
                int qty, priority;
                string part = row["PartNumber"].ToString().Trim();
                string qty_str = row["Qty"].ToString().Trim();
                string priority_str = row["Priority"].ToString().Trim();
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
                DataRow new_row = KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.NewRow();
                new_row["Date"] = date;
                new_row["PartNumber"] = part;
                new_row["Qty"] = qty;
                new_row["Priority"] = priority;
                KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Add(new_row);
                i++;
                ProgressBar1.Value = i * 100 / total;
            }
            Update_SQL_Data(KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.Data_da, KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb);

            //close nonnection --------------------------------------------------------------------------------------
            dtJoinPOFromP001.Dispose();
            dtJoinPOFromP001 = null;
            //dtCopyFromP001.Dispose();
            //dtCopyFromP001 = null;

            b = KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, String.Format("SELECT * FROM [P_002_PlanForProductionByDate] WHERE [Date] = '{0}' order by SubLine_ID", date.ToString("dd MMM yyyy")));
            if (b)
            {
                String ret = Calculate(date);
                if (!String.IsNullOrEmpty(ret))
                {
                    MessageBox.Show(ret);
                }
            }
            Update_SQL_Data(KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.Data_da, KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb);

            StatusLabel1.Visible = false;
            ProgressBar1.Visible = false;
            return true;
        }

        void Button_Calculte_Click(object sender, EventArgs e)
        {
            //    FormatDataGridViewDisplay();
            DateSelect_Dialog_Form selectDate_Dialog = new DateSelect_Dialog_Form(DateTime.Now.AddDays(1));
            if (selectDate_Dialog.ShowDialog() == DialogResult.OK)
            {
                bool b = KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.Update_SQL_Data(KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.Data_da, KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb);
                if (b)
                {
                    DateTime dt = selectDate_Dialog.Select_Date;
                    //load data by date
                    b = KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, String.Format("SELECT * FROM [P_002_PlanForProductionByDate] WHERE [Date] = '{0}' order by SubLine_ID", dt.ToString("yyyy-MMM-dd")));
                    if (b)
                    {
                        String ret = Calculate(dt);
                        if (!String.IsNullOrEmpty(ret))
                        {
                            MessageBox.Show(ret);
                        }
                    }
                }
                else
                {
                }
            }
        }
    }
}