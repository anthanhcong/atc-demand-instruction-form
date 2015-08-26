using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.IO;
using System.IO.Ports;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using MasterDatabase;


namespace Job_Assignment
{
    public partial class Form1 : SQL_APPL
    {
        public string MasterDatabase_Connection_Str = "";
        public string LeaveRegister_Connection_Str = "";
        public string Kitting_Connection_Str = "";
        private string Cur_Path;
        private string Configure_path;
        private string Database_Type;
        private Run_Mode Running_Mode;

        public Form1()
        {
            InitializeComponent();

            // MasterDatabase_Connection_Str = ApplicationSetting.GetInstance().MasterDatabaseConnectionString;
            // LeaveRegister_Connection_Str = ApplicationSetting.GetInstance().LeaveRegisterConnectionString;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("vi");
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
                

            StatusLabel1.Text = "";
            StatusLabel2.Text = "";
            ProgressBar1.Visible = false;

            filterStatusLabel.Text = "";
            filterStatusLabel.Visible = false;
            showAllLabel.Text = "Show & All";
            showAllLabel.Visible = false;
            showAllLabel.IsLink = true;

            Cur_Path = Directory.GetCurrentDirectory();
            Configure_path = Cur_Path + @"\Configure";
            if (Directory.Exists(Configure_path) == false)
            {
                Directory.CreateDirectory(Configure_path);
            }
            MasterDatabase_Connection_Str = Get_JobAssiment_Connect_str();
            LeaveRegister_Connection_Str = Get_LeaveRegister_Connect_str();
            Kitting_Connection_Str = Get_Kitting_Connect_str();
            Running_Mode = Get_Run_Mode();

            if ((Running_Mode == Run_Mode.RELEASE) 
                &&(MasterDatabase_Connection_Str == "SERVER=10.84.10.67\\SIPLACE_2008R2EX;DATABASE=JOB_ASSIGNMENT_DB;UID=read;PWD=read"))
            {
                this.Text = "Job Assignment       Release 054 " + Database_Type + " - " + Running_Mode.ToString().Trim();
            }
            else 
            {
                if (MasterDatabase_Connection_Str == "SERVER=10.84.10.67\\SIPLACE_2008R2EX;DATABASE=JOB_ASSIGNMENT_DB;UID=read;PWD=read")
                {
                    this.Text = "Job Assignment       Release 054 " + Database_Type + " - " + Running_Mode.ToString().Trim();
                }
                else
                {
                    this.Text = "Test JAS   Release 054 " + Database_Type + " - " + Running_Mode.ToString().Trim();
                }
                Features_Tab.BackColor = Color.Orange;
            }

            tabControl1.TabPages.Remove(PLan_by_WST);

            // BOM_Manage_Init();
            OpenXL = new Excel.Application();

            System.Globalization.CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            OpenXL.SheetsInNewWorkbook = 1;
            OpenXL.Visible = false;
            OpenXL.DisplayAlerts = false;

            ApplicationSetting.GetInstance().MasterDatabaseConnectionString = MasterDatabase_Connection_Str;
            ApplicationSetting.GetInstance().LeaveRegisterConnectionString = LeaveRegister_Connection_Str;

            frmLogin frmLogin = new frmLogin();

            if (frmLogin.ShowDialog() == DialogResult.OK)
            {
                manageUserToolStripMenuItem.Visible = ApplicationSession.UserLoginInfo.IsAdmin;
                toolStripStatusUserLogin.Text = String.Format("{0} (Last access time: {1})", ApplicationSession.UserLoginInfo.UserId, ApplicationSession.UserLoginInfo.LastLoginDate.ToString("dd/MM/yyyy HH:mm"));

                if (ApplicationSession.UserLoginInfo.LastLoginDate == DateTime.MinValue)
                {
                    frmChangePasswordFirstLogin frm = new frmChangePasswordFirstLogin();
                    frm.ShowDialog();
                }
            }
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.Graphics.DrawString("x", e.Font, Brushes.Black, e.Bounds.Right - 15, e.Bounds.Top + 4);
            e.Graphics.DrawString(this.tabControl1.TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 12, e.Bounds.Top + 4);
            e.DrawFocusRectangle();
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            //Looping through the controls.
            for (int i = 0; i < this.tabControl1.TabPages.Count; i++)
            {
                if (tabControl1.TabPages[i] != Features_Tab)
                {
                    Rectangle r = tabControl1.GetTabRect(i);
                    //Getting the position of the "x" mark.
                    Rectangle closeButton = new Rectangle(r.Right - 15, r.Top + 4, 9, 7);
                    if (closeButton.Contains(e.Location))
                    {
                        if (MessageBox.Show("Would you like to Close this Tab?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.tabControl1.TabPages.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        private void btn_Create_All_Plan_Click(object sender, EventArgs e)
        {
            DateTime date;
            // Hien thi chon ngay
            DateSelect_Dialog_Form selectDate_Dialog = new DateSelect_Dialog_Form(DateTime.Now.AddDays(1));
            if (selectDate_Dialog.ShowDialog() == DialogResult.OK)
            {
                date = selectDate_Dialog.Select_Date;
                Create_All_Plan_for_Date(date);
            }
        }

        private void manageUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageUser frm = new frmManageUser();
            frm.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword();
            frm.ShowDialog();
        }
    }
}
