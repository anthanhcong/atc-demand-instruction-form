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
        private void SerialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int len = SerialPort1.BytesToRead;
            byte[] data_read = new byte[len + 2];
            int received_byte;
            string inData;
            long i;

            switch (Receive_State)
            {
                case RECEIVE_STATE.NORMAL:
                    received_byte = SerialPort1.Read(data_read, 0, len);
                    for (i = 0; i < len; i++)
                    {
                        if (Normal_Index <= RING_BUF_LEN)
                        {
                            Normal_Buffer[Normal_Index] = data_read[i];
                            Normal_Index++;
                            if ((data_read[i] == 0x0D) || (data_read[i] == 0x0A))
                            {
                                inData = Convert_Bytes_to_String(Normal_Buffer, 0, Normal_Index);
                                Normal_Index = 0;
                                Tab1ComPortSelect.BeginInvoke(new EventHandler(delegate
                                {
                                    Process_inData(inData);
                                }));
                            }
                        }
                    }
                    break;
                case RECEIVE_STATE.START_FRAME:
                    Receive_Index = 0;

                    // Store data into buffer
                    received_byte = SerialPort1.Read(data_read, 0, len);

                    for (i = 0; i < len; i++)
                    {
                        if (Item_Cnt <= RING_BUF_LEN)
                        {
                            if ((data_read[i] != 0x0A) && (data_read[i] != 0x0D))
                            {
                                Serial_Receive_Buf[W_index][Receive_Index] = data_read[i];
                                if (Receive_Index < R_BUF_LEN - 1 )
                                {
                                    Receive_Index++;
                                }
                                else
                                {
                                    MessageBox.Show("Error Data", "Error");
                                }
                            }
                            else
                            {
                                Serial_Receive_Buf[W_index][Receive_Index] = data_read[i];
                                // Data_Received_Flag = true;
                                Tab1ComPortSelect.BeginInvoke(new EventHandler(delegate
                                {
                                    Data_Received_Flag = true;
                                }));

                                // Set Next Queue Node
                                if (W_index < RING_BUF_LEN - 1) W_index++;
                                else W_index = 0;
                                Clean_Receive_Node(W_index);

                                Item_Cnt++;
                                Receive_Index = 0;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Overload Ring Buffer: Bigger Frame", "Error");
                            //TODO: Need implement for action ???
                        }
                    }
                    Receive_State = RECEIVE_STATE.RECEIVE_FRAME;
                    break;
                case RECEIVE_STATE.RECEIVE_FRAME:
                    // Store data into buffer
                    received_byte = SerialPort1.Read(data_read, 0, len);
                    for (i = 0; i < len; i++)
                    {
                        if (Item_Cnt <= RING_BUF_LEN)
                        {
                            if ((data_read[i] != 0x0A) && (data_read[i] != 0x0D))
                            {
                                Serial_Receive_Buf[W_index][Receive_Index] = data_read[i];
                                if (Receive_Index < R_BUF_LEN - 1)
                                {
                                    Receive_Index++;
                                }
                                else
                                {
                                    MessageBox.Show("Error Data", "Error");
                                }
                            }
                            else
                            {
                                Serial_Receive_Buf[W_index][Receive_Index] = data_read[i];
                                // Data_Received_Flag = true;
                                Tab1ComPortSelect.BeginInvoke(new EventHandler(delegate
                                {
                                    Data_Received_Flag = true;
                                }));

                                // Set Next Queue Node
                                if (W_index < RING_BUF_LEN - 1) W_index++;
                                else W_index = 0;

                                Clean_Receive_Node(W_index);

                                Item_Cnt++;
                                Receive_Index = 0;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Overload Ring Buffer", "Error");
                            //TODO: Need implement for action ???
                        }
                    }
                    break;
                case RECEIVE_STATE.END_FRAME:
                    // received_byte = SerialPort1.Read(data_read, 0, len);
                    break;
                default:
                    Receive_State = RECEIVE_STATE.NORMAL;
                    break;

            }
        }

        private void TimeOut_Timer_Tick(object sender, EventArgs e)
        {
            Receive_Timeout_Flag = true;
        }
    }
}