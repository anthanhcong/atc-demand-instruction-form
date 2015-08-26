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
        private void Setting_Save_BT_Click(object sender, EventArgs e)
        {
            Save_Configure_File();
            Show_Current_Line_Status();
        }
        private void Setting_LineID_Cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            string line_id = Setting_LineID_Cbx.Text.Trim();
            Cur_Line_ID = line_id;
            Load_List_of_WST(line_id);
            Setting_WSTID_Cbx.DataSource = List_WST_dtb;
            Setting_WSTID_Cbx.DisplayMember = "WST_ID";
            Setting_WSTID_Cbx.ValueMember = "WST_Name";
        }

        private void Setting_WSTID_Cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            string wst_id = Setting_WSTID_Cbx.Text.ToString().Trim();
            Cur_WST_ID = wst_id;
            Get_WST_Status(Cur_Line_ID, Cur_WST_ID, Cur_Date);
        }

        private void Setting_ViewMode_Rbt_CheckedChanged(object sender, EventArgs e)
        {
            bool tracking = Setting_Tracking_Rbt.Checked;
            Set_Tracking_Mode(tracking);
        }

        private void Setting_Tracking_Rbt_CheckedChanged(object sender, EventArgs e)
        {
            bool tracking = Setting_Tracking_Rbt.Checked;
            Set_Tracking_Mode(tracking);
        }


        private void Setting_MSNV_Txt_TextChanged(object sender, EventArgs e)
        {
            string msnv = Setting_MSNV_Txt.Text.Trim();
            Process_inData(msnv);
        }
    }
}