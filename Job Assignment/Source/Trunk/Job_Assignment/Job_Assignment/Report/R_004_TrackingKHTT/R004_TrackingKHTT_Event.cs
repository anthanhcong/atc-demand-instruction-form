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
        void R004_Tracking_KHTT_Create_BT_Click(object sender, EventArgs e)
        {
            DateTime from_date;
            DateTime to_date;

            DateInput_Dialog_Form selectDate_Dialog = new DateInput_Dialog_Form();
            if (selectDate_Dialog.DateInput_Dialog(DateTime.Now) == DialogResult.OK)
            {
                from_date = selectDate_Dialog.FromDate;
                to_date = selectDate_Dialog.ToDate;
                if (from_date > to_date)
                {
                    MessageBox.Show("Please Select Correct Date");
                    return;
                }

                while (from_date <= to_date)
                {
                    R004_TrackingKHTT_Get_Plan_Empl(from_date);
                    from_date = from_date.AddDays(1);
                }
            }
        }
    }
}
