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
        void P007_Tracking_View_Create_BT_Click(object sender, EventArgs e)
        {
            DateTime date;

            DateSelect_Dialog_Form selectDate_Dialog = new DateSelect_Dialog_Form(DateTime.Now);
            if (selectDate_Dialog.ShowDialog() == DialogResult.OK)
            {
                date = selectDate_Dialog.Select_Date;
                P007_Tracking_Get_Plan_Empl(date);
            }
        }
    }
}