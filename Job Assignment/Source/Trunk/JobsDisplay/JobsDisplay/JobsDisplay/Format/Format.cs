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
        private int Change_HexString_to_Int(string indata)
        {
            int in_len;
            int value = 0;
            if (indata == "") return 0;
            in_len = indata.Length;

            if (in_len == 4)
            {
                if ((((indata[0] == '0') && (indata[1] == 'x')) || ((indata[0] == '\\') && (indata[1] == 'x'))) 
                    &&(((indata[2] >= '0') && (indata[2] <= '9')) ||
                     ((indata[2] >= 'a') && (indata[2] <= 'f')) ||
                     ((indata[2] >= 'A') && (indata[2] <= 'F'))) 
                    &&(((indata[3] >= '0') && (indata[3] <= '9')) ||
                     ((indata[3] >= 'a') && (indata[3] <= 'f')) ||
                     ((indata[3] >= 'A') && (indata[3] <= 'F'))))
                {
                    value = int.Parse(indata.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                }
            }
            return value;
        }

        private int Change_Text2Bytes(string indata, ref byte[] outdata_ptr)
        {
            int i, in_len, out_len = 0;

            // check correct data
            if (indata == "") return 0;
            in_len = indata.Length;
            for (i = 0; i < in_len; i++)
            {
                if (i < in_len - 3)
                {
                    if (((indata[i] == '\\') && ((indata[i + 1] == 'x') || (indata[i + 1] == 'x'))) &&
                        (((indata[i + 2] >= '0') && (indata[i + 2] <= '9')) ||
                         ((indata[i + 2] >= 'a') && (indata[i + 2] <= 'f')) ||
                         ((indata[i + 2] >= 'A') && (indata[i + 2] <= 'F'))) &&
                        (((indata[i + 3] >= '0') && (indata[i + 3] <= '9')) ||
                         ((indata[i + 3] >= 'a') && (indata[i + 3] <= 'f')) ||
                         ((indata[i + 3] >= 'A') && (indata[i + 3] <= 'F'))))
                    {
                        outdata_ptr[out_len] = byte.Parse(indata.Substring(i + 2, 2), System.Globalization.NumberStyles.HexNumber);
                        i += 3;
                    }
                    else
                    {
                        outdata_ptr[out_len] = (byte)indata[i];
                    }
                }
                else
                {
                    outdata_ptr[out_len] = (byte)indata[i];
                }
                out_len++;
            }
            return out_len;
        }

        private string Convert_Bytes_to_String(byte[] input, int start, int len)
        {
            StringBuilder sb = new StringBuilder(len * 4);
            int i;

            for (i = start; i < start + len; i++)
            {
                // if (input[i] > 127)
                //if ((input[i] > 127)
                //    || ((input[i] < 0x20) && (input[i] != 0x0d) && (input[i] != 0x0a) && (input[i] != 0)))
                //{
                //    sb.Append("{" + Convert.ToString(input[i], 16) + "}");
                //}
                //else if (input[i] == 0)
                //{
                //    break;
                //}
                //else
                //{
                //    sb.Append((char)input[i]);
                //}

                //if (input[i] == 0)
                //{
                //    break;
                //}
                //else
                //{
                //    sb.Append((char)input[i]);
                //}
                
                sb.Append((char)input[i]);
                if ((input[i] == 0x0d) || (input[i] == 0x0a))
                {
                    break;
                }
            }
            return sb.ToString();
        }

        private string Change_HexString2String(string indata)
        {
            int i, in_len;
            Int32 value;

            // check correct data
            if (indata == "") return "";
            in_len = indata.Length;
            StringBuilder sb = new StringBuilder(in_len);
            for (i = 0; i < in_len; i++)
            {
                if (i < in_len - 3)
                {
                    if (((indata[i] == '\\') && ((indata[i + 1] == 'x') || (indata[i + 1] == 'x'))) &&
                        (((indata[i + 2] >= '0') && (indata[i + 2] <= '9')) ||
                         ((indata[i + 2] >= 'a') && (indata[i + 2] <= 'f')) ||
                         ((indata[i + 2] >= 'A') && (indata[i + 2] <= 'F'))) &&
                        (((indata[i + 3] >= '0') && (indata[i + 3] <= '9')) ||
                         ((indata[i + 3] >= 'a') && (indata[i + 3] <= 'f')) ||
                         ((indata[i + 3] >= 'A') && (indata[i + 3] <= 'F'))))
                    {
                        value = Int32.Parse(indata.Substring(i + 2, 2), System.Globalization.NumberStyles.HexNumber);
                        if ((value > 127) || (value == 0))
                        {
                            sb.Append("{");
                            sb.Append(Convert.ToString(value, 16));
                            sb.Append("}");
                        }
                        else
                        {
                            sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(indata.Substring(i + 2, 2), System.Globalization.NumberStyles.HexNumber))));
                        }
                        i += 3;
                    }
                    else
                    {
                        sb.Append(indata[i]);
                    }
                }
                else
                {
                    sb.Append(indata[i]);
                }
            }
            return sb.ToString();
        }

        private string Convert_Bytes_to_HexString(byte[] input, int start, int len)
        {
            StringBuilder sb = new StringBuilder(len * 4);
            int i;

            for (i = start; i < start + len; i++)
            {
                sb.Append("{" + Convert.ToString(input[i], 16) + "}");
            }
            return sb.ToString();
        }

        private string Change_CRLF(string data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (char b in data)
            {
                if (b == 5)
                {
                    sb.Append("{ENQ}");
                }
                else if (b == 6)
                {
                    sb.Append("{ACK}");
                }
                else if ((b >= 0x20) && (b <= 0x7E))
                {
                    sb.Append(b);
                }
                else if (b == '\n')
                {
                    sb.Append("{LF}\n");
                }
                else if (b == '\r')
                {
                    sb.Append("{CR}\r");
                }
                else if (b == '\x15')
                {
                    sb.Append("{ACK}");
                }
                else
                {
                    sb.Append("{" + Convert.ToString(Convert.ToByte(b), 16) + "}");
                }
            }
            return sb.ToString();
        }

        private string Change_Special_Char(string data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            string hex_char = "";
            foreach (char b in data)
            {
                if ((b < 0x20) || (b > 0x7e) || (b == ','))
                {
                    hex_char = Convert.ToString(Convert.ToByte(b), 16);
                    if (hex_char.Length == 1)
                    {
                        hex_char = "0" + hex_char;
                    }
                    sb.Append(@"\x" + hex_char);
                }
                else
                {
                    sb.Append(b);
                }
            }
            return sb.ToString();
        }

        private string Encrypt_Pass(string data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            Random random = new Random();
            int addnew;

            foreach (char b in data)
            {
                sb.Append(Convert.ToString(Convert.ToByte(b), 16));
                addnew = random.Next('0', '9');
                sb.Append(addnew - '0');
            }
            return sb.ToString().ToUpper();
        }

        private string Decrypt_Pass(string indata)
        {
            int i, in_len;
            string char_str;
            Int32 value;

            // check correct data
            if (indata == "") return "";
            in_len = indata.Length;
            StringBuilder sb = new StringBuilder(in_len);
            for (i = 0; i < in_len - 2; i = i + 3)
            {
                char_str = indata.Substring(i, 2);
                value = Int32.Parse(char_str, System.Globalization.NumberStyles.HexNumber);
                if ((value < 127) || (value > 0))
                {
                    sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(char_str, System.Globalization.NumberStyles.HexNumber))));
                }
                else return "";
            }
            return sb.ToString();
        }
    }
}