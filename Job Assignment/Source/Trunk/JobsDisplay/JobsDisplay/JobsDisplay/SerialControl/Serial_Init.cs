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
        DataTable Comport_List;

        private bool COMPORT_INIT()
        {
            string[] port_list = System.IO.Ports.SerialPort.GetPortNames();
            if (Comport_List == null)
            {
                Comport_List = new DataTable();
                Comport_List.Columns.Add("PortName");
            }
            Comport_List.Clear();
            foreach (string port_name in port_list)
            {
                Comport_List.Rows.Add(port_name);
            }

            Tab1ComPortSelect.DataSource = Comport_List;
            Tab1ComPortSelect.DisplayMember = "PortName";
            Tab1ComPortSelect.ValueMember = "PortName";

            for (Int64 i = 0; i < RING_BUF_LEN; i++)
            {
                Serial_Receive_Buf[i] = new byte[R_BUF_LEN];
            }
            return true;
        }
    }
}