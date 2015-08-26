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
    enum RECEIVE_STATE
    {
        NORMAL = 0,
        START_FRAME,
        RECEIVE_FRAME,
        END_FRAME
    }
    enum Read_Data_State { Idle, Reading };

    public partial class Form1 : SQL_APPL
    {
        const int R_BUF_LEN = 120;
        const long RING_BUF_LEN = 400000;
        private byte[][] Serial_Receive_Buf = new byte[RING_BUF_LEN][];
        private long W_index = 0, R_index = 0;   //@NOTE (Kien): Queue Manage
        private long Item_Cnt = 0;               //@NOTE (Kien): Queue Manage
        private long Receive_Index = 0;          //@NOTE (Kien): Frame Manage

        private byte[] Normal_Buffer = new byte[R_BUF_LEN];
        private int Normal_Index = 0;

        private RECEIVE_STATE Receive_State = RECEIVE_STATE.NORMAL;
        private bool Data_Received_Flag = false;
        private bool Receive_Timeout_Flag = false;

        /***************************************************************************/
        /*   #####   #####  ##   ##    ######  #####  # # # ######
            #       #     # ###  ##    #    # #     # #   #    #  
            #       #     # # # # #    #####  #     # # # #    #  
             #####   #####  #  ## #    #       #####  #   #    #                   */
        /***************************************************************************/
        /// <summary>
        /// Name            : GetTab1SerialConfig
        /// Function        : Save all config of Comport
        /// </summary>
        private void GetTab1SerialConfig()
        {
            bool port_opened = false;
            if (SerialPort1.IsOpen == true)
            {
                port_opened = true;
                SerialPort1.Close();
            }
            try
            {

                SerialPort1.PortName = Tab1ComPortSelect.Text;
                SerialPort1.BaudRate = int.Parse(Tab1SetBaudrate.Text);
                SerialPort1.DataBits = int.Parse(Tab1SetDatabit.Text);
                SerialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), Tab1SetParity.Text);
                SerialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), Tab1SetStopbit.Text);
            }
            catch
            {
                if (Tab1ComPortSelect.Items.Count > 0)
                {
                    Used_Port = Tab1ComPortSelect.Items[0].ToString().Trim();
                }
                else
                {
                    Used_Port = "COM1";
                }
                Used_Baudrate = "9600";
                Used_Databit = "8";
                Used_Parity = "NONE";
                Used_Stopbit = "ONE";

                Tab1ComPortSelect.Text = Used_Port;
                Tab1SetBaudrate.Text = Used_Baudrate;
                Tab1SetDatabit.Text = Used_Databit;
                Tab1SetParity.Text = Used_Parity;
                Tab1SetStopbit.Text = Used_Stopbit;
                Save_Configure_File();
            }
            if (port_opened)
            {
                SerialPort1.Open();
            }
        }

        /// <summary>
        /// Name: OpenLoggerComport
        /// Function: 
        ///     + Open Comport of Tab1
        ///     + Disable Setting
        /// </summary>
        private bool Open_Logger_Comport()
        {
            try
            {
                if (SerialPort1.IsOpen == false)
                {
                    SerialPort1.Open();
                }
                Normal_Index = 0;
            }
            catch
            {
                MessageBox.Show(("Can not Open " + SerialPort1.PortName) + "\nComport is used by other Application!", "Error");
                return false;
            }

            // update status
            if (SerialPort1.IsOpen == true)
            {
                // Logger_Enable_setting(true);
                Update_Port_Status();
            }
            return true;
        }

        private bool Close_Logger_Comport()
        {
            try
            {
                if (SerialPort1.IsOpen == true)
                {
                    SerialPort1.Close();
                }
                Update_Port_Status();
            }
            catch
            {
                Update_Port_Status();
                MessageBox.Show("Comport is used by other Application!", "Error");
            }
            return true;
        }

        private int Write_Port(string data)
        {
            byte[] data_converted;
            int len;
            len = data.Length;
            data_converted = new byte[len + 5];
            len = Change_Text2Bytes(data, ref data_converted);
            len = Write_Port_Data(data_converted, len);
            return len;
        }

        private int Write_Port_Data(byte[] data, int len)
        {
            if (SerialPort1.IsOpen == true)
            {
                SerialPort1.Write(data, 0, len);
            }
            return len;
        }

        private void Clean_Receive_Buffer()
        {
            int i;
            for (i = 0; i < RING_BUF_LEN; i++)
            {
                Clean_Receive_Node(i);
            }
            R_index = 0;
            W_index = 0;
            Item_Cnt = 0;
        }

        private void Clean_Receive_Node(long node_index)
        {
            int i;
            for (i = 0; i < R_BUF_LEN; i++)
            {
                Serial_Receive_Buf[node_index][i] = 0;
            }
            Receive_Index = 0;
        }

        private void Start_TimeOut(int ms)
        {
            ForceClose_Timer.Stop();
            ForceClose_Timer.Interval = 10;
            ForceClose_Timer.Interval = ms;
            ForceClose_Timer.Start();
        }
        private void Stop_TimeOut()
        {
            ForceClose_Timer.Stop();
        }

        private void Update_Port_Status()
        {
            string status = "";
            status += SerialPort1.PortName.ToString().Trim() + " : ";
            status += SerialPort1.BaudRate.ToString().Trim() + " : ";
            status += SerialPort1.DataBits.ToString().Trim() + " : ";
            status += SerialPort1.Parity.ToString().Trim() + " : ";
            status += SerialPort1.StopBits.ToString().Trim() + " : ";

            if (SerialPort1.IsOpen == true)
            {
                status += "Opened | ";
                // OpenComport_BT.Text = "Close";
                Tab1groupSerSetting.Enabled = false;
            }
            else
            {
                status += "Closed | ";
                // OpenComport_BT.Text = "Open";
                Tab1groupSerSetting.Enabled = true;
            }
            StatusLabel1.Text = status;
        }
    }
}