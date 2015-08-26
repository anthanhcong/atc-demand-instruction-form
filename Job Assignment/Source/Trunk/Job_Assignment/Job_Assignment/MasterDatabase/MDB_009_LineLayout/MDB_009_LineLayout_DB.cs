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
        private DataTable Load_LineDescription_All_WST_list()
        {
            string cmd = @"SELECT distinct
                              [LineID]
                              ,[LineName]
                              ,[SubLine_ID]
                              ,[SubLine_Name]
                              ,[WST_ID]
                              ,[WST_Name]
                              ,[GroupID]
                          FROM [MDB_003_Line_Desciption]";
            DataTable table = GetSqlData(cmd);
            return table;
        }

        bool Create_Update_MDB009_WST_List(string line_id, string line_name,string subline_id,string subline_name,string wst_id,string wst_name, string group_id)
        {
            string cur_line_id, cur_line_name, cur_subline_id, cur_subline_name, cur_wst_id, cur_group_id;
            bool exist = false;
            foreach (DataRow row in LineLayout_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows)
            {
                cur_line_id = row["Line_ID"].ToString().Trim();
                cur_line_name = row["Line_Name"].ToString().Trim();
                cur_subline_id = row["SubLine_ID"].ToString().Trim();
                cur_subline_name = row["SubLine_Name"].ToString().Trim();
                cur_wst_id = row["WST_ID"].ToString().Trim();
                // cur_wst_name = row["WST_Name"].ToString().Trim();
                cur_group_id = row["GroupID"].ToString().Trim();

                if ((cur_line_id == line_id) && (cur_wst_id == wst_id))
                {
                    row["Line_Name"] = line_name;
                    row["WST_Name"] = wst_name;
                    row["SubLine_Name"] = subline_name;
                    row["SubLine_ID"]= subline_id;
                    exist = true;
                }
            }

            if (exist == false)
            {
                DataRow new_row = LineLayout_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.NewRow();
                new_row["Line_ID"] = line_id;
                new_row["Line_Name"] = line_name;
                new_row["SubLine_ID"] = subline_id;
                new_row["SubLine_Name"] = subline_name;
                new_row["WST_ID"] = wst_id;
                new_row["WST_Name"] = wst_name;
                new_row["GroupID"] = group_id;
                LineLayout_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Add(new_row);
            }
            // P_003_KeHoachSanXuatTheoLine_MasterDatabase.MasterDatabase_GridviewTBL.Save_Data();

            Update_SQL_Data(LineLayout_MasterDatabase.MasterDatabase_GridviewTBL.Data_da, LineLayout_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb);
            return true;
        }

    }
}