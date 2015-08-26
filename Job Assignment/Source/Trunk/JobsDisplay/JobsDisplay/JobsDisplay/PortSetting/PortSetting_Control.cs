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

namespace JobsDisplay
{
    public partial class Form1 : SQL_APPL
    {
        public DataTable List_Line_dtb = new DataTable();
        public DataSet List_Line_ds = new DataSet();
        public SqlDataAdapter List_Line_da;

        public DataTable List_WST_dtb = new DataTable();
        public DataSet List_WST_ds = new DataSet();
        public SqlDataAdapter List_WST_da;


        private DataTable Load_List_of_Line()
        {
            string sql_cmd = @"SELECT distinct [LineID],[LineName] 
                                FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_003_Line_Desciption] ORDER by [LineID]";

            if (List_Line_dtb != null)
            {
                List_Line_dtb.Clear();
            }
            List_Line_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref List_Line_da, ref List_Line_ds);
            return List_Line_dtb;
        }

        private DataTable Load_List_of_WST(string line_id)
        {
            string sql_cmd = @"SELECT distinct [WST_ID],[WST_Name] 
                                FROM [JOB_ASSIGNMENT_DB].[dbo].[MDB_003_Line_Desciption] ";
            sql_cmd += " WHERE [LineID] = '" + line_id + "'";

            if (List_WST_dtb != null)
            {
                List_WST_dtb.Clear();
            }
            List_WST_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref List_WST_da, ref List_WST_ds);
            return List_WST_dtb;
        }

        private void Init_Setting_Tab()
        {
            Load_WST_Info();
            Load_Port_Setting();
        }

        public void Load_WST_Info()
        {
            int total_item, i;
            string line_id, wst_id;
            bool line_match = false, wst_match = false;

            Load_List_of_Line();
            Setting_LineID_Cbx.DataSource = List_Line_dtb;
            Setting_LineID_Cbx.DisplayMember = "LineID";
            Setting_LineID_Cbx.ValueMember = "LineID";
            Application.DoEvents();
            total_item = Setting_LineID_Cbx.Items.Count;
            if (total_item > 0)
            {
                for (i = 0; i < total_item; i++)
                {
                    Setting_LineID_Cbx.SelectedIndex = i;
                    Application.DoEvents();
                    line_id = Setting_LineID_Cbx.Text.Trim();
                    if (line_id == Cur_Line_ID)
                    {
                        line_match = true;
                        break;
                    }
                }

                if (line_match == false)
                {
                    Setting_LineID_Cbx.SelectedIndex = 0;
                    Application.DoEvents();
                    line_id = Setting_LineID_Cbx.Text.Trim();
                    Cur_Line_ID = line_id;
                }

                Load_List_of_WST(Cur_Line_ID);
                Setting_WSTID_Cbx.DataSource = List_WST_dtb;
                Setting_WSTID_Cbx.DisplayMember = "WST_ID";
                Setting_WSTID_Cbx.ValueMember = "WST_Name";
                Application.DoEvents();
                total_item = Setting_WSTID_Cbx.Items.Count;
            }

            Setting_LineID_Cbx.SelectedIndexChanged += new System.EventHandler(this.Setting_LineID_Cbx_SelectedIndexChanged);
            // Setting_WSTID_Cbx.SelectedIndexChanged += new System.EventHandler(this.Setting_WSTID_Cbx_SelectedIndexChanged);
        }

        private void Load_Port_Setting()
        {
            Tab1ComPortSelect.Text = Used_Port;
            Tab1SetBaudrate.Text = Used_Baudrate;
            Tab1SetDatabit.Text = Used_Databit;
            Tab1SetParity.Text = Used_Parity;
            Tab1SetStopbit.Text = Used_Stopbit;

            if (Mode == Working_Mode.TRACKING)
            {
                Setting_Tracking_Rbt.Checked = true;
            }
            else
            {
                Setting_ViewMode_Rbt.Checked = true;
            }
        }


        private void Set_Tracking_Mode(bool tracking)
        {
            if (tracking == true)
            {
                Mode = Working_Mode.TRACKING;
            }
            else
            {
                Mode = Working_Mode.VIEW;
            }

        }
    }
}