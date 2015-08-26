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
using DataGridViewAutoFilter;

namespace JobsDisplay
{
    public partial class Form1 : SQL_APPL
    {
        private void YourJob_MSNV_Txt_TextChanged(object sender, EventArgs e)
        {
            string msnv = YourJob_MSNV_Txt.Text.Trim();

            if (msnv != "")
            {
                if (msnv.Length < 8)
                {
                    return;
                }
                
                string empl_name = Get_Empl_Name(msnv);
                Cur_Date = DateTime.Now;
                YourJob_EmplName_Lbl.Text = empl_name;
                YourJobs_Date_Lbl.Text = Cur_Date.ToString("dd MMM yyyy");
                YourJob_Shift_LBL.Text = Get_Shift_ID(Cur_Date);
                Show_Plan(msnv);
            }
        }

        private bool Show_Plan(string msnv)
        {
            DateTime date = Cur_Date;
            DataTable plan_table;
            string select_line, select_WST;
            string empl_name;
            string datestr;
            DateTime selectdate;

            plan_table = Load_Job_Plan(msnv, date);
            if (plan_table != null)
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = JobsPlan_dtb;
                YourJob_GridView.DataSource = bs;
                Jobs_GridView_BindingContextChanged(null, null);

                if (JobsPlan_dtb.Rows.Count > 0)
                {
                    select_line = JobsPlan_dtb.Rows[0]["LineID"].ToString().Trim();
                    select_WST = JobsPlan_dtb.Rows[0]["WST_ID"].ToString().Trim();
                    empl_name = JobsPlan_dtb.Rows[0]["Empl_name"].ToString().Trim();
                    YourJob_EmplName_Lbl.Text = empl_name;

                    // HH02_09
                    //Layout_HH02 layout_Display = new Layout_HH02(select_WST);
                    //layout_Display.ShowDialog();
                }

                Application.DoEvents();
                foreach (DataGridViewRow row in YourJob_GridView.Rows)
                {
                    if (row.Cells["Date"].Value != null)
                    {
                        datestr = row.Cells["Date"].Value.ToString().Trim();
                        if (datestr != "")
                        {
                            try
                            {
                                selectdate = DateTime.Parse(datestr);
                                if (date.Date == selectdate.Date)
                                {
                                    row.DefaultCellStyle.BackColor = Color.Yellow;
                                }
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }
            return true;
        }

        private void Jobs_GridView_BindingContextChanged(object sender, EventArgs e)
        {
            string col_name;
            if (YourJob_GridView.DataSource == null) return;

            foreach (DataGridViewColumn col in YourJob_GridView.Columns)
            {
                //col.HeaderCell = new
                //    DataGridViewAutoFilterColumnHeaderCell(col.HeaderCell);
                col_name = col.Name.ToString().Trim();
                if ((col_name == "LineID") || (col_name == "WST_ID") || (col_name == "WST_Name") || 
                    (col_name == "ShiftName") || (col_name == "PartNumber")
                    || (col_name == "From_Time") || (col_name == "To_Time") || (col_name == "Date"))
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


        private void StatusLabel4_Click(object sender, EventArgs e)
        {
            DataGridViewAutoFilterTextBoxColumn.RemoveFilter(YourJob_GridView);
        }

        private void Jobs_GridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            String filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(YourJob_GridView);
            if (String.IsNullOrEmpty(filterStatus))
            {
                showAllLabel.Visible = false;
                filterStatusLabel.Visible = false;
            }
            else
            {
                showAllLabel.Visible = true;
                filterStatusLabel.Visible = true;
                filterStatusLabel.Text = filterStatus;
            }
        }
   }
}