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
using System.Globalization;
using System.Threading;
using LayoutControl;
using DataGridViewAutoFilter;
using JobsDisplay.Statistics;

namespace JobsDisplay
{
    public partial class Form1 : SQL_APPL
    {
        enum Working_Mode
        {
            VIEW = 0,
            TRACKING = 1,
        }
        //public static string MasterDatabase_Connection_Str = @"server=(local)\SQLEXPRESS;database=JOB_ASSIGNMENT_DB;Integrated Security = TRUE";
        //public static string LeaveRegister_Connection_Str = @"server=(local)\SQLEXPRESS;database=SHIFT_REGISTER_DB;Integrated Security = TRUE";
        public static string MasterDatabase_Connection_Str;
        public static string LeaveRegister_Connection_Str;
        public static string Kitting_Connection_Str;

        private string Folder_Path = @"C:\ATC\Setting";
        private string Configure_FileName = "setting.bak";
        private string Cur_Path;
        private string Configure_path;
        private string Cur_PO = "";
        private string Cur_Part = "";
        DataTable List_PO;

        private Working_Mode Mode;
        private DateTime Cur_Date;
        public string Cur_Line_ID = "";
        public string Cur_WST_ID = "";
        private string Used_Port = "";
        private string Used_Baudrate = "";
        private string Used_Databit = "";
        private string Used_Parity = "";
        private string Used_Stopbit = "";
        EmptyFormState Cur_EmtyForm_State = EmptyFormState.GET_MORE;
        int Wait_Counter = 0;
        int ForceClose_Counter = 0;
        int CheckPO_Counter = 0;
        
        public Form1()
        {
            CultureInfo culture;
            culture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            InitializeComponent();

            filterStatusLabel.Text = "";
            filterStatusLabel.Visible = false;
            showAllLabel.Text = "Show &All";
            showAllLabel.Visible = false;
            showAllLabel.IsLink = true;
            showAllLabel.LinkBehavior = LinkBehavior.HoverUnderline;
            // Init_Configure();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            this.Text = "Line Status Tracking     Release 034";
            Cur_Path = Directory.GetCurrentDirectory();
            Configure_path = Cur_Path + @"\Configure";
            if (Directory.Exists(Configure_path) == false)
            {
                Directory.CreateDirectory(Configure_path);
            }
            
            MasterDatabase_Connection_Str = Get_JobAssiment_Connect_str();
            LeaveRegister_Connection_Str = Get_LeaveRegister_Connect_str();
            Kitting_Connection_Str = Get_Kitting_Connect_str();
            Init_Configure();
            Load_List_PO(date);
            Application.DoEvents();
            Cur_Date = DateTime.Now;
            if (IsForceClose() == true)
            {
                if (Display_Thread != null)
                {
                    Display_Thread.Abort();
                }
                AutoClosingMessageBox.Show("Chương Trình đang được cập nhật. \nBạn vui lòng chạy lại ứng dụng sau một vài phút", "Warning", 2000);
                this.Close();
            }
                
            Load_Current_Line_Status(Cur_Date);

            AutoCheck_Timer.Start();
            ForceClose_Timer.Start();

            if ((Cur_Line_ID == FRU_LINE) || (Cur_Line_ID == STAND_LINE))
            {
                FRU_Rb.Checked = true;
                FRU_Rb.Text = Cur_Line_ID;
                In_Manual_Rb.Enabled = false;
                Setting_In_Check.Enabled = false;
            }

            // an tool
            trackingViewToolStripMenuItem.Visible = false;
            layoutManagementToolStripMenuItem.Visible = false;
        }

        private void ShowUpdateWarning()
        {
            MessageBox.Show("Updating Application", "Warning");
        }

        public void Init_Configure()
        {
            COMPORT_INIT();
            Application.DoEvents();
            Load_Configure_File();
            Init_Setting_Tab();
            Application.DoEvents();
            GetTab1SerialConfig();
            Application.DoEvents();
            Open_Logger_Comport();
        }

        public void Load_Configure_File()
        {
            string file_path = Folder_Path + "\\" + Configure_FileName;
            StreamReader myfile;
            StreamWriter writeStream;
            string strline;
            string mode_str;

            if (Directory.Exists(Folder_Path) == false)
            {
                Directory.CreateDirectory(Folder_Path);
            }
            if (File.Exists(file_path) == false)
            {
                writeStream = File.CreateText(file_path);
                writeStream.WriteLine("Mode: VIEW");
                writeStream.WriteLine("Line_ID");
                writeStream.WriteLine("WST_ID");
                writeStream.WriteLine("COM1");
                writeStream.WriteLine("9600");
                writeStream.WriteLine("8");
                writeStream.WriteLine("NONE");
                writeStream.WriteLine("ONE");
                writeStream.Close();
            }

            myfile = File.OpenText(file_path);

            try
            {
                strline = myfile.ReadLine();
                mode_str = strline.Split(':')[1].Trim();
                if (mode_str == "TRACKING")
                {
                    Mode = Working_Mode.TRACKING;
                }
                else
                {
                    Mode = Working_Mode.VIEW;
                }

                Cur_Line_ID = myfile.ReadLine().Trim();
                Cur_WST_ID = myfile.ReadLine().Trim();

                Used_Port = myfile.ReadLine().Trim();
                Used_Baudrate = myfile.ReadLine().Trim();
                Used_Databit = myfile.ReadLine().Trim();
                Used_Parity = myfile.ReadLine().Trim();
                Used_Stopbit = myfile.ReadLine().Trim();
                myfile.Close();
            }
            catch
            {
                myfile.Close();
                File.Delete(file_path);
                writeStream = File.CreateText(file_path);
                writeStream.WriteLine("Mode: VIEW");
                writeStream.WriteLine("Line_ID");
                writeStream.WriteLine("WST_ID");
                writeStream.WriteLine("COM1");
                writeStream.WriteLine("9600");
                writeStream.WriteLine("8");
                writeStream.WriteLine("NONE");
                writeStream.WriteLine("ONE");
                writeStream.Close();

                Mode = Working_Mode.VIEW;
                Cur_Line_ID = "Line_ID";
                Cur_WST_ID = "WST_ID";
                Used_Port = "COM1";
                Used_Baudrate = "9600";
                Used_Databit = "8";
                Used_Parity = "NONE";
                Used_Stopbit = "ONE";
            }
        }


        private void Save_Configure_File()
        {
            string file_path = Folder_Path + "\\" + Configure_FileName;
            StreamWriter writeStream;;

            if (Directory.Exists(Folder_Path) == false)
            {
                Directory.CreateDirectory(Folder_Path);
            }
            if (File.Exists(file_path) == true)
            {
                File.Delete(file_path);
            }

            writeStream = File.CreateText(file_path);
            if (Mode == Working_Mode.TRACKING)
            {
                writeStream.WriteLine("Mode: TRACKING");
            }
            else
            {
                writeStream.WriteLine("Mode: VIEW");
            }
            Used_Port = Tab1ComPortSelect.Text.Trim();
            Used_Baudrate = Tab1SetBaudrate.Text.Trim();
            Used_Databit = Tab1SetDatabit.Text.Trim();
            Used_Parity = Tab1SetParity.Text.Trim();
            Used_Stopbit = Tab1SetStopbit.Text.Trim();
            writeStream.WriteLine(Cur_Line_ID);
            writeStream.WriteLine(Cur_WST_ID);
            writeStream.WriteLine(Used_Port);
            writeStream.WriteLine(Used_Baudrate);
            writeStream.WriteLine(Used_Databit);
            writeStream.WriteLine(Used_Parity);
            writeStream.WriteLine(Used_Stopbit);
            writeStream.Close();
        }

        private void Process_inData(string indata)
        {
            if (Mode == Working_Mode.VIEW)
            {
                tabControl1.SelectTab(YourJobs_TabPage);
                // Show_Plan(indata);
                YourJob_MSNV_Txt.Text = "";
                Application.DoEvents();
                YourJob_MSNV_Txt.Text = indata;
            }
            else
            {
                tabControl1.SelectTab(Tracking_Tab);
                Tracking_MSNV_Txt.Text = "";
                Application.DoEvents();
                Tracking_MSNV_Txt.Text = indata;
            }
        }

        private void trackingViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tracking_View_Init();
        }

        private void Port_Close_BT_Click(object sender, EventArgs e)
        {
            Close_Logger_Comport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetTab1SerialConfig();
            Open_Logger_Comport();
        }

        private void designToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster(MasterDatabase_Connection_Str);
            frm.ShowDialog();
        }

        private void lineLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterLineStatus frm = new frmMasterLineStatus(MasterDatabase_Connection_Str);
            frm.ShowDialog();
        }

        private void databaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LineLayout_Init();
        }

        private void In_Manual_Rb_Click(object sender, EventArgs e)
        {
            //Login frmLogin = new Login(Setting_In_Check);
            //frmLogin.ShowDialog(this);
        }
    }

    public class AutoClosingMessageBox
    {
        System.Threading.Timer _timeoutTimer;
        string _caption;
        AutoClosingMessageBox(string text, string caption, int timeout)
        {
            _caption = caption;
            _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                null, timeout, System.Threading.Timeout.Infinite);
            MessageBox.Show(text, caption);
        }
        public static void Show(string text, string caption, int timeout)
        {
            new AutoClosingMessageBox(text, caption, timeout);
        }
        void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow(null, _caption);
            if (mbWnd != IntPtr.Zero)
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            _timeoutTimer.Dispose();
        }
        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }
}
