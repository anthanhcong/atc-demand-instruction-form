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
        private bool R_007_Emlpoyee_In_WST_Count_GetWST()
        {
            string sql_cmd;
            bool b;
            DataRow newrow;
            string mess;
            int i = 0;
            int countT = 0;
            int total = 0;
            string wst, num_of_person_do_WST;

            sql_cmd = @"SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R007_Employee_In_WST_Count] ";
            b = R_007_Emlpoyee_In_WST_Count_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            if (b == false)
            {
                return false;
            }

            StatusLabel1.Visible = true;
            ProgressBar1.Visible = true;
            StatusLabel1.Text = "create Employee_In_WST_Count";

            int count = R_007_Emlpoyee_In_WST_Count_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Count;
            if (count > 0)
            {
                mess = "Employee In WST data was existing\n";
                mess += "Do you want to delete and create the new one?";

                if (MessageBox.Show(mess, "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    StatusLabel1.Visible = false;
                    ProgressBar1.Visible = false;
                    return false;
                }

                DeleteReport_R_007_Emlpoyee_In_WST_Count();
                R_007_Emlpoyee_In_WST_Count_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            }

            DataTable empl_in_wst = Load_Empl_In_WST();
            if (empl_in_wst != null)
            {
                countT = empl_in_wst.Rows.Count;
            }
            total = countT;

            foreach (DataRow row in empl_in_wst.Rows)
            {
                wst = row["WST_ID"].ToString().Trim();
                num_of_person_do_WST = row["num_of_person_do_WST"].ToString().Trim();

                newrow = R_007_Emlpoyee_In_WST_Count_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.NewRow();
                newrow["WST_ID"] = wst;
                newrow["num_of_person_do_WST"] = num_of_person_do_WST;
                R_007_Emlpoyee_In_WST_Count_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Add(newrow);

                i++;
                ProgressBar1.Value = i * 100 / total;
            }

            Update_SQL_Data(R_007_Emlpoyee_In_WST_Count_MasterDatabase.MasterDatabase_GridviewTBL.Data_da, R_007_Emlpoyee_In_WST_Count_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb);

            StatusLabel1.Visible = false;
            ProgressBar1.Visible = false;

            return true;
        }

        private bool R_007_Emlpoyee_In_WST_Get_Empl_WST()
        {
            string sql_cmd;
            bool b;
            DataRow newrow;
            string mess;
            int i = 0;
            int countT = 0;
            int total = 0;
            string lineID, wst, sub_Line_ID, empl_ID, empl_Name, groupID;


            sql_cmd = "SELECT * FROM [JOB_ASSIGNMENT_DB].[dbo].[R007_Employee_In_WST]";
            b = R_007_Emlpoyee_In_WST_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);

            if (b == false)
            {
                return false;
            }

            StatusLabel1.Visible = true;
            ProgressBar1.Visible = true;
            StatusLabel1.Text = "create Employee_In_WST";

            int count = R_007_Emlpoyee_In_WST_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Count;
            if (count > 0)
            {
                mess = "Employee In WST data was existing\n";
                mess += "Do you want to delete and create the new one?";

                if (MessageBox.Show(mess, "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    StatusLabel1.Visible = false;
                    ProgressBar1.Visible = false;
                    return false;
                }

                DeleteReport_R_007_Emlpoyee_In_WST();
                R_007_Emlpoyee_In_WST_MasterDatabase.MasterDatabase_GridviewTBL.Load_DataBase(MasterDatabase_Connection_Str, sql_cmd);
            }

            DataTable all_wst = Load_all_WST();
            if (all_wst != null)
            {
                countT = all_wst.Rows.Count;
            }
            total = countT;

            DataTable empl_all = Get_Empl();

            foreach (DataRow row in all_wst.Rows)
            {
                lineID = row["LineID"].ToString().Trim();
                wst = row["WST_ID"].ToString().Trim();
                sub_Line_ID = row["SubLine_ID"].ToString().Trim();
                groupID = row["GroupID"].ToString().Trim();
                foreach (DataRow empl in empl_all.Rows)
                {
                    empl_ID = empl["Empl_ID"].ToString().Trim();
                    empl_Name = empl["Empl_Name"].ToString().Trim();
                    if (IsEmplHaveEnoughSkill(empl_ID, wst))
                    {
                        newrow = R_007_Emlpoyee_In_WST_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.NewRow();
                        newrow["GroupID"] = groupID;
                        newrow["SubLine_ID"] = sub_Line_ID;
                        newrow["LineID"] = lineID;
                        newrow["WST_ID"] = wst;
                        newrow["Empl_ID"] = empl_ID;
                        newrow["Empl_Name"] = empl_Name;
                        R_007_Emlpoyee_In_WST_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb.Rows.Add(newrow);
                    }
                }
                i++;
                ProgressBar1.Value = i * 100 / total;
            }

            Update_SQL_Data(R_007_Emlpoyee_In_WST_MasterDatabase.MasterDatabase_GridviewTBL.Data_da, R_007_Emlpoyee_In_WST_MasterDatabase.MasterDatabase_GridviewTBL.Data_dtb);

            StatusLabel1.Visible = false;
            ProgressBar1.Visible = false;

            return true;
        }

        private bool DeleteReport_R_007_Emlpoyee_In_WST()
        {
            bool result;
            string cmd = @"Delete FROM [R007_Employee_In_WST]";
                            //WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            result = Update_Data_Info(MasterDatabase_Connection_Str, cmd);
            return result;
        }

        private DataTable Get_Empl()
        {
            string cmd = @"select distinct Empl_ID, Empl_Name FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_002_Empl_Skill]";
            SQL_API.SQL_ATC sqlobj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            sqlobj.GET_SQL_DATA(cmd);
            return sqlobj.DaTable;
        }

        private bool DeleteReport_R_007_Emlpoyee_In_WST_Count()
        {
            bool result;
            string cmd = @"Delete FROM [R007_Employee_In_WST_Count]";
            //WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            result = Update_Data_Info(MasterDatabase_Connection_Str, cmd);
            return result;
        }

        DataTable Load_Empl_In_WST()
        {
            string sql_cmd = @"SELECT WST_ID, COUNT(WST_ID) as 'num_of_person_do_WST' 
                               FROM [JOB_ASSIGNMENT_DB].[dbo].[R007_Employee_In_WST]
                               GROUP BY WST_ID ";
            SQL_API.SQL_ATC sqlobj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);
            sqlobj.GET_SQL_DATA(sql_cmd);
            return sqlobj.DaTable;
        }
    }
}
