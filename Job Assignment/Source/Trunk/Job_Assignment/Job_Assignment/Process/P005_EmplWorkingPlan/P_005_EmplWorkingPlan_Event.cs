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
        void EmplWorkingPlan_Create_BT_Click(object sender, EventArgs e)
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
                Create_Empl_Plan(date);
            }
        }

        private bool Create_Empl_Plan(DateTime date)
        {
            string sql_cmd, mess;
            bool b;
            int count;
            int i = 0, total;

            StatusLabel1.Visible = true;
            ProgressBar1.Visible = true;
            StatusLabel1.Text = "Create Empl Working Plan";
            
            SQL_API.SQL_ATC all_empl_list = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            // lay du lieu ke hoach sx theo line cua ngay da chon
            sql_cmd = String.Format("SELECT * FROM [P_005_EmplWorkingPlan] WHERE [Date] = '{0}' order by LineId", date.ToString("yyyy-MMM-dd"));
            b = EmplWorkingPlan_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            if (b == false)
            {
                return false;
            }

            count = EmplWorkingPlan_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Count;
            if (count > 0)
            {
                mess = "Plan for date:" + date.ToString("dd MMM yyyy") + "was existing\n";
                mess += "Do you want to delete and create the new one?";

                if (MessageBox.Show(mess, "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }
                DeleteEmpl_plan(date);
                EmplWorkingPlan_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            }

            // Load All Empl
            sql_cmd = @"Select Distinct [Empl_ID], [Empl_Name] FROM [MDB_002_Empl_Skill]";
            all_empl_list.GET_SQL_DATA(sql_cmd);

            total = all_empl_list.DaTable.Rows.Count;
            foreach (DataRow row in all_empl_list.DaTable.Rows)
            {
                DataRow newrow = EmplWorkingPlan_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.NewRow();
                newrow["Empl_ID"] = row["Empl_ID"];
                newrow["Empl_Name"] = row["Empl_Name"];
                newrow["Date"] = date.ToString("MM/dd/yyyy");

                if (GetEmplPlan(ref newrow) == true)
                {
                    EmplWorkingPlan_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Add(newrow);
                }
                i++;
                ProgressBar1.Value = i * 100 / total;
            }

            Update_SQL_Data(EmplWorkingPlan_MasterDatabase.MasterDatabase_GridviewTBL.Data_da, EmplWorkingPlan_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb);
            StatusLabel1.Visible = false;
            ProgressBar1.Visible = false;
            return true;
        }
    }
}
