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
        void LineLayout_Create_BT_Click(object sender, EventArgs e)
        {
            int i = 0, total;
            bool b;
            string sql_cmd;
            DataTable line_plan;
            string line_id, line_name, subline_id, sub_name, wst, wst_name, group_id;

            StatusLabel1.Visible = true;
            ProgressBar1.Visible = true;
            StatusLabel1.Text = "Prepareing data";

            // Load Du lieu cac Line 
            line_plan = Load_LineDescription_All_WST_list();

            sql_cmd = @"SELECT TOP 1000 [Line_ID]
                              ,[Line_Name]
                              ,[SubLine_ID]
                              ,[SubLine_Name]
                              ,[WST_ID]
                              ,[WST_Name]
                              ,[WST_x]
                              ,[WST_y]
                              ,[WST_width]
                              ,[WST_heigh]
                              ,[GroupID]
                              ,[Description]
                              ,[Note]
                          FROM [MDB_009_LayoutControl]";
            b = LineLayout_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            if (b == false)
            {
                StatusLabel1.Visible = false;
                ProgressBar1.Visible = false;
                return;
            }

            Application.DoEvents();
            StatusLabel1.Text = "Creating WST information";
            total = line_plan.Rows.Count;
            foreach (DataRow row in line_plan.Rows)
            {
                // Get thong tin
                line_id = row["LineID"].ToString().Trim();
                line_name = row["LineName"].ToString().Trim();
                subline_id = row["SubLine_ID"].ToString().Trim();
                sub_name = row["SubLine_Name"].ToString().Trim();
                wst = row["WST_ID"].ToString().Trim();
                wst_name = row["WST_Name"].ToString().Trim();
                group_id = row["GroupID"].ToString().Trim();

                // Create or update du lieu
                Create_Update_MDB009_WST_List(line_id, line_name, subline_id, sub_name, wst, wst_name, group_id);
                ProgressBar1.Value = i * 100 / total;
                // Application.DoEvents();
            }
            StatusLabel1.Visible = false;
            ProgressBar1.Visible = false;
        }
    }
}