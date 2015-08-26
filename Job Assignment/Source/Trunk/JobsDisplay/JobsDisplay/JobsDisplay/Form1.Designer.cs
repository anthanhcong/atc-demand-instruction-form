namespace JobsDisplay
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.SerialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.YourJobs_TabPage = new System.Windows.Forms.TabPage();
            this.YourJob_GridView = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.YourJob_MSNV_Txt = new System.Windows.Forms.TextBox();
            this.YourJob_EmplName_Lbl = new System.Windows.Forms.Label();
            this.YourJobs_Date_Lbl = new System.Windows.Forms.Label();
            this.YourJob_Shift_LBL = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Tracking_Tab = new System.Windows.Forms.TabPage();
            this.Tracking_Status_GridView = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Tracking_Find_BT = new System.Windows.Forms.Button();
            this.Tracking_LayoutBT = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.FRU_Rb = new System.Windows.Forms.RadioButton();
            this.In_Manual_Rb = new System.Windows.Forms.RadioButton();
            this.Setting_Out_Check = new System.Windows.Forms.RadioButton();
            this.Setting_In_Check = new System.Windows.Forms.RadioButton();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.Tracking_Kitting_PO_Grv = new System.Windows.Forms.DataGridView();
            this.Tracking_StopPO_BT = new System.Windows.Forms.Button();
            this.Tracking_RefreshPO_BT = new System.Windows.Forms.Button();
            this.Tracking_StartPO_BT = new System.Windows.Forms.Button();
            this.Tracking_MSNV_Txt = new System.Windows.Forms.TextBox();
            this.Tracking_EmplName_Lbl = new System.Windows.Forms.Label();
            this.Tracking_PartNumber_Txt = new System.Windows.Forms.Label();
            this.Traking_PO = new System.Windows.Forms.Label();
            this.Tracking_Shift_LBL = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Setting_Tab = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.Port_Close_BT = new System.Windows.Forms.Button();
            this.Setting_MSNV_Txt = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.Setting_Tracking_Rbt = new System.Windows.Forms.RadioButton();
            this.Setting_ViewMode_Rbt = new System.Windows.Forms.RadioButton();
            this.Setting_Save_BT = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Setting_WSTID_Cbx = new System.Windows.Forms.ComboBox();
            this.Setting_LineID_Cbx = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Tab1groupSerSetting = new System.Windows.Forms.GroupBox();
            this.Tab1SetBRLabel = new System.Windows.Forms.Label();
            this.Tab1SetStopbit = new System.Windows.Forms.ComboBox();
            this.Tab1ComPortSelect = new System.Windows.Forms.ComboBox();
            this.Tab1SetParity = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Tab1SetDatabit = new System.Windows.Forms.ComboBox();
            this.Tab1SetBaudrate = new System.Windows.Forms.ComboBox();
            this.Tab1SetDataBitLabel = new System.Windows.Forms.Label();
            this.Tab1SetParityLabel = new System.Windows.Forms.Label();
            this.Tab1SetStopBitLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trackingViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.designToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.filterStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.showAllLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.AutoCheck_Timer = new System.Windows.Forms.Timer(this.components);
            this.ForceClose_Timer = new System.Windows.Forms.Timer(this.components);
            this.lineLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.YourJobs_TabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.YourJob_GridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.Tracking_Tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tracking_Status_GridView)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tracking_Kitting_PO_Grv)).BeginInit();
            this.Setting_Tab.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.Tab1groupSerSetting.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SerialPort1
            // 
            this.SerialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.YourJobs_TabPage);
            this.tabControl1.Controls.Add(this.Tracking_Tab);
            this.tabControl1.Controls.Add(this.Setting_Tab);
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(686, 535);
            this.tabControl1.TabIndex = 0;
            // 
            // YourJobs_TabPage
            // 
            this.YourJobs_TabPage.Controls.Add(this.YourJob_GridView);
            this.YourJobs_TabPage.Controls.Add(this.groupBox1);
            this.YourJobs_TabPage.Location = new System.Drawing.Point(4, 22);
            this.YourJobs_TabPage.Name = "YourJobs_TabPage";
            this.YourJobs_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.YourJobs_TabPage.Size = new System.Drawing.Size(678, 509);
            this.YourJobs_TabPage.TabIndex = 0;
            this.YourJobs_TabPage.Text = "Your Jobs";
            this.YourJobs_TabPage.UseVisualStyleBackColor = true;
            // 
            // YourJob_GridView
            // 
            this.YourJob_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.YourJob_GridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.YourJob_GridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.YourJob_GridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.YourJob_GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.YourJob_GridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.YourJob_GridView.Location = new System.Drawing.Point(6, 145);
            this.YourJob_GridView.Name = "YourJob_GridView";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.YourJob_GridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.YourJob_GridView.Size = new System.Drawing.Size(664, 358);
            this.YourJob_GridView.TabIndex = 4;
            this.YourJob_GridView.BindingContextChanged += new System.EventHandler(this.Jobs_GridView_BindingContextChanged);
            this.YourJob_GridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.Jobs_GridView_DataBindingComplete);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.YourJob_MSNV_Txt);
            this.groupBox1.Controls.Add(this.YourJob_EmplName_Lbl);
            this.groupBox1.Controls.Add(this.YourJobs_Date_Lbl);
            this.groupBox1.Controls.Add(this.YourJob_Shift_LBL);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(6, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(664, 136);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // YourJob_MSNV_Txt
            // 
            this.YourJob_MSNV_Txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.YourJob_MSNV_Txt.ForeColor = System.Drawing.Color.Blue;
            this.YourJob_MSNV_Txt.Location = new System.Drawing.Point(92, 23);
            this.YourJob_MSNV_Txt.MaxLength = 8;
            this.YourJob_MSNV_Txt.Name = "YourJob_MSNV_Txt";
            this.YourJob_MSNV_Txt.Size = new System.Drawing.Size(192, 29);
            this.YourJob_MSNV_Txt.TabIndex = 1;
            this.YourJob_MSNV_Txt.TextChanged += new System.EventHandler(this.YourJob_MSNV_Txt_TextChanged);
            // 
            // YourJob_EmplName_Lbl
            // 
            this.YourJob_EmplName_Lbl.AutoSize = true;
            this.YourJob_EmplName_Lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.YourJob_EmplName_Lbl.ForeColor = System.Drawing.Color.Blue;
            this.YourJob_EmplName_Lbl.Location = new System.Drawing.Point(6, 58);
            this.YourJob_EmplName_Lbl.Name = "YourJob_EmplName_Lbl";
            this.YourJob_EmplName_Lbl.Size = new System.Drawing.Size(107, 24);
            this.YourJob_EmplName_Lbl.TabIndex = 2;
            this.YourJob_EmplName_Lbl.Text = "Họ và Tên";
            // 
            // YourJobs_Date_Lbl
            // 
            this.YourJobs_Date_Lbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.YourJobs_Date_Lbl.AutoSize = true;
            this.YourJobs_Date_Lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.YourJobs_Date_Lbl.ForeColor = System.Drawing.Color.Blue;
            this.YourJobs_Date_Lbl.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.YourJobs_Date_Lbl.Location = new System.Drawing.Point(489, 16);
            this.YourJobs_Date_Lbl.Name = "YourJobs_Date_Lbl";
            this.YourJobs_Date_Lbl.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.YourJobs_Date_Lbl.Size = new System.Drawing.Size(147, 24);
            this.YourJobs_Date_Lbl.TabIndex = 0;
            this.YourJobs_Date_Lbl.Text = "dd MMM yyyyy";
            this.YourJobs_Date_Lbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // YourJob_Shift_LBL
            // 
            this.YourJob_Shift_LBL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.YourJob_Shift_LBL.AutoSize = true;
            this.YourJob_Shift_LBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.YourJob_Shift_LBL.ForeColor = System.Drawing.Color.Blue;
            this.YourJob_Shift_LBL.Location = new System.Drawing.Point(489, 58);
            this.YourJob_Shift_LBL.Name = "YourJob_Shift_LBL";
            this.YourJob_Shift_LBL.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.YourJob_Shift_LBL.Size = new System.Drawing.Size(50, 24);
            this.YourJob_Shift_LBL.TabIndex = 0;
            this.YourJob_Shift_LBL.Text = "Shift";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(6, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 24);
            this.label2.TabIndex = 0;
            this.label2.Text = "MSNV: ";
            // 
            // Tracking_Tab
            // 
            this.Tracking_Tab.Controls.Add(this.Tracking_Status_GridView);
            this.Tracking_Tab.Controls.Add(this.groupBox3);
            this.Tracking_Tab.Location = new System.Drawing.Point(4, 22);
            this.Tracking_Tab.Name = "Tracking_Tab";
            this.Tracking_Tab.Size = new System.Drawing.Size(678, 509);
            this.Tracking_Tab.TabIndex = 2;
            this.Tracking_Tab.Text = "Tracking";
            this.Tracking_Tab.UseVisualStyleBackColor = true;
            // 
            // Tracking_Status_GridView
            // 
            this.Tracking_Status_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Tracking_Status_GridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.Tracking_Status_GridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Tracking_Status_GridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.Tracking_Status_GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Tracking_Status_GridView.DefaultCellStyle = dataGridViewCellStyle5;
            this.Tracking_Status_GridView.Location = new System.Drawing.Point(7, 218);
            this.Tracking_Status_GridView.Name = "Tracking_Status_GridView";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Tracking_Status_GridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.Tracking_Status_GridView.Size = new System.Drawing.Size(663, 285);
            this.Tracking_Status_GridView.TabIndex = 6;
            this.Tracking_Status_GridView.BindingContextChanged += new System.EventHandler(this.Tracking_GridView_BindingContextChanged);
            this.Tracking_Status_GridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Tracking_Status_GridView_CellContentClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.Tracking_Find_BT);
            this.groupBox3.Controls.Add(this.Tracking_LayoutBT);
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.dateTimePicker1);
            this.groupBox3.Controls.Add(this.Tracking_Kitting_PO_Grv);
            this.groupBox3.Controls.Add(this.Tracking_StopPO_BT);
            this.groupBox3.Controls.Add(this.Tracking_RefreshPO_BT);
            this.groupBox3.Controls.Add(this.Tracking_StartPO_BT);
            this.groupBox3.Controls.Add(this.Tracking_MSNV_Txt);
            this.groupBox3.Controls.Add(this.Tracking_EmplName_Lbl);
            this.groupBox3.Controls.Add(this.Tracking_PartNumber_Txt);
            this.groupBox3.Controls.Add(this.Traking_PO);
            this.groupBox3.Controls.Add(this.Tracking_Shift_LBL);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(7, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(664, 206);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            // 
            // Tracking_Find_BT
            // 
            this.Tracking_Find_BT.Location = new System.Drawing.Point(216, 106);
            this.Tracking_Find_BT.Name = "Tracking_Find_BT";
            this.Tracking_Find_BT.Size = new System.Drawing.Size(75, 23);
            this.Tracking_Find_BT.TabIndex = 38;
            this.Tracking_Find_BT.Text = "Find Empl";
            this.Tracking_Find_BT.UseVisualStyleBackColor = true;
            this.Tracking_Find_BT.Click += new System.EventHandler(this.Tracking_Find_BT_Click);
            // 
            // Tracking_LayoutBT
            // 
            this.Tracking_LayoutBT.Location = new System.Drawing.Point(216, 135);
            this.Tracking_LayoutBT.Name = "Tracking_LayoutBT";
            this.Tracking_LayoutBT.Size = new System.Drawing.Size(75, 23);
            this.Tracking_LayoutBT.TabIndex = 37;
            this.Tracking_LayoutBT.Text = "Layout";
            this.Tracking_LayoutBT.UseVisualStyleBackColor = true;
            this.Tracking_LayoutBT.Click += new System.EventHandler(this.Tracking_LayoutBT_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.FRU_Rb);
            this.groupBox5.Controls.Add(this.In_Manual_Rb);
            this.groupBox5.Controls.Add(this.Setting_Out_Check);
            this.groupBox5.Controls.Add(this.Setting_In_Check);
            this.groupBox5.Location = new System.Drawing.Point(6, 168);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(285, 32);
            this.groupBox5.TabIndex = 36;
            this.groupBox5.TabStop = false;
            // 
            // FRU_Rb
            // 
            this.FRU_Rb.AutoSize = true;
            this.FRU_Rb.Location = new System.Drawing.Point(124, 9);
            this.FRU_Rb.Name = "FRU_Rb";
            this.FRU_Rb.Size = new System.Drawing.Size(47, 17);
            this.FRU_Rb.TabIndex = 1;
            this.FRU_Rb.TabStop = true;
            this.FRU_Rb.Text = "FRU";
            this.FRU_Rb.UseVisualStyleBackColor = true;
            this.FRU_Rb.CheckedChanged += new System.EventHandler(this.Tracking_Mode_Change);
            // 
            // In_Manual_Rb
            // 
            this.In_Manual_Rb.AutoSize = true;
            this.In_Manual_Rb.Location = new System.Drawing.Point(46, 9);
            this.In_Manual_Rb.Name = "In_Manual_Rb";
            this.In_Manual_Rb.Size = new System.Drawing.Size(72, 17);
            this.In_Manual_Rb.TabIndex = 1;
            this.In_Manual_Rb.TabStop = true;
            this.In_Manual_Rb.Text = "In Manual";
            this.In_Manual_Rb.UseVisualStyleBackColor = true;
            this.In_Manual_Rb.Click += new System.EventHandler(this.In_Manual_Rb_Click);
            this.In_Manual_Rb.CheckedChanged += new System.EventHandler(this.Tracking_Mode_Change);
            // 
            // Setting_Out_Check
            // 
            this.Setting_Out_Check.AutoSize = true;
            this.Setting_Out_Check.Location = new System.Drawing.Point(237, 9);
            this.Setting_Out_Check.Name = "Setting_Out_Check";
            this.Setting_Out_Check.Size = new System.Drawing.Size(42, 17);
            this.Setting_Out_Check.TabIndex = 0;
            this.Setting_Out_Check.Text = "Out";
            this.Setting_Out_Check.UseVisualStyleBackColor = true;
            this.Setting_Out_Check.CheckedChanged += new System.EventHandler(this.Tracking_Mode_Change);
            // 
            // Setting_In_Check
            // 
            this.Setting_In_Check.AutoSize = true;
            this.Setting_In_Check.Checked = true;
            this.Setting_In_Check.Location = new System.Drawing.Point(6, 9);
            this.Setting_In_Check.Name = "Setting_In_Check";
            this.Setting_In_Check.Size = new System.Drawing.Size(34, 17);
            this.Setting_In_Check.TabIndex = 0;
            this.Setting_In_Check.TabStop = true;
            this.Setting_In_Check.Text = "In";
            this.Setting_In_Check.UseVisualStyleBackColor = true;
            this.Setting_In_Check.CheckedChanged += new System.EventHandler(this.Tracking_Mode_Change);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(6, 14);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 6;
            // 
            // Tracking_Kitting_PO_Grv
            // 
            this.Tracking_Kitting_PO_Grv.AllowUserToAddRows = false;
            this.Tracking_Kitting_PO_Grv.AllowUserToDeleteRows = false;
            this.Tracking_Kitting_PO_Grv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Tracking_Kitting_PO_Grv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Tracking_Kitting_PO_Grv.Location = new System.Drawing.Point(308, 14);
            this.Tracking_Kitting_PO_Grv.Name = "Tracking_Kitting_PO_Grv";
            this.Tracking_Kitting_PO_Grv.Size = new System.Drawing.Size(350, 186);
            this.Tracking_Kitting_PO_Grv.TabIndex = 5;
            this.Tracking_Kitting_PO_Grv.BindingContextChanged += new System.EventHandler(this.Tracking_Kitting_PO_Grv_BindingContextChanged);
            this.Tracking_Kitting_PO_Grv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Tracking_Kitting_PO_Grv_CellClick);
            // 
            // Tracking_StopPO_BT
            // 
            this.Tracking_StopPO_BT.Location = new System.Drawing.Point(216, 69);
            this.Tracking_StopPO_BT.Name = "Tracking_StopPO_BT";
            this.Tracking_StopPO_BT.Size = new System.Drawing.Size(75, 23);
            this.Tracking_StopPO_BT.TabIndex = 4;
            this.Tracking_StopPO_BT.Text = "Stop PO";
            this.Tracking_StopPO_BT.UseVisualStyleBackColor = true;
            this.Tracking_StopPO_BT.Click += new System.EventHandler(this.Tracking_StopPO_BT_Click);
            // 
            // Tracking_RefreshPO_BT
            // 
            this.Tracking_RefreshPO_BT.Location = new System.Drawing.Point(216, 11);
            this.Tracking_RefreshPO_BT.Name = "Tracking_RefreshPO_BT";
            this.Tracking_RefreshPO_BT.Size = new System.Drawing.Size(75, 23);
            this.Tracking_RefreshPO_BT.TabIndex = 4;
            this.Tracking_RefreshPO_BT.Text = "Refresh";
            this.Tracking_RefreshPO_BT.UseVisualStyleBackColor = true;
            this.Tracking_RefreshPO_BT.Click += new System.EventHandler(this.Tracking_RefreshPO_BT_Click);
            // 
            // Tracking_StartPO_BT
            // 
            this.Tracking_StartPO_BT.Location = new System.Drawing.Point(216, 40);
            this.Tracking_StartPO_BT.Name = "Tracking_StartPO_BT";
            this.Tracking_StartPO_BT.Size = new System.Drawing.Size(75, 23);
            this.Tracking_StartPO_BT.TabIndex = 4;
            this.Tracking_StartPO_BT.Text = "Start PO";
            this.Tracking_StartPO_BT.UseVisualStyleBackColor = true;
            this.Tracking_StartPO_BT.Click += new System.EventHandler(this.Tracking_StartPO_BT_Click);
            // 
            // Tracking_MSNV_Txt
            // 
            this.Tracking_MSNV_Txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Tracking_MSNV_Txt.ForeColor = System.Drawing.Color.Blue;
            this.Tracking_MSNV_Txt.Location = new System.Drawing.Point(56, 114);
            this.Tracking_MSNV_Txt.MaxLength = 8;
            this.Tracking_MSNV_Txt.Name = "Tracking_MSNV_Txt";
            this.Tracking_MSNV_Txt.Size = new System.Drawing.Size(117, 21);
            this.Tracking_MSNV_Txt.TabIndex = 1;
            this.Tracking_MSNV_Txt.TextChanged += new System.EventHandler(this.Tracking_MSNV_Txt_TextChanged);
            // 
            // Tracking_EmplName_Lbl
            // 
            this.Tracking_EmplName_Lbl.AutoSize = true;
            this.Tracking_EmplName_Lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Tracking_EmplName_Lbl.ForeColor = System.Drawing.Color.Blue;
            this.Tracking_EmplName_Lbl.Location = new System.Drawing.Point(3, 143);
            this.Tracking_EmplName_Lbl.Name = "Tracking_EmplName_Lbl";
            this.Tracking_EmplName_Lbl.Size = new System.Drawing.Size(71, 15);
            this.Tracking_EmplName_Lbl.TabIndex = 2;
            this.Tracking_EmplName_Lbl.Text = "Họ và Tên";
            // 
            // Tracking_PartNumber_Txt
            // 
            this.Tracking_PartNumber_Txt.AutoSize = true;
            this.Tracking_PartNumber_Txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Tracking_PartNumber_Txt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.Tracking_PartNumber_Txt.Location = new System.Drawing.Point(6, 86);
            this.Tracking_PartNumber_Txt.Name = "Tracking_PartNumber_Txt";
            this.Tracking_PartNumber_Txt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Tracking_PartNumber_Txt.Size = new System.Drawing.Size(46, 24);
            this.Tracking_PartNumber_Txt.TabIndex = 0;
            this.Tracking_PartNumber_Txt.Text = "Part";
            // 
            // Traking_PO
            // 
            this.Traking_PO.AutoSize = true;
            this.Traking_PO.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Traking_PO.ForeColor = System.Drawing.Color.Blue;
            this.Traking_PO.Location = new System.Drawing.Point(6, 64);
            this.Traking_PO.Name = "Traking_PO";
            this.Traking_PO.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Traking_PO.Size = new System.Drawing.Size(29, 16);
            this.Traking_PO.TabIndex = 0;
            this.Traking_PO.Text = "PO";
            // 
            // Tracking_Shift_LBL
            // 
            this.Tracking_Shift_LBL.AutoSize = true;
            this.Tracking_Shift_LBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Tracking_Shift_LBL.ForeColor = System.Drawing.Color.Blue;
            this.Tracking_Shift_LBL.Location = new System.Drawing.Point(6, 40);
            this.Tracking_Shift_LBL.Name = "Tracking_Shift_LBL";
            this.Tracking_Shift_LBL.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Tracking_Shift_LBL.Size = new System.Drawing.Size(33, 13);
            this.Tracking_Shift_LBL.TabIndex = 0;
            this.Tracking_Shift_LBL.Text = "Shift";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(3, 119);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "MSNV: ";
            // 
            // Setting_Tab
            // 
            this.Setting_Tab.Controls.Add(this.button1);
            this.Setting_Tab.Controls.Add(this.Port_Close_BT);
            this.Setting_Tab.Controls.Add(this.Setting_MSNV_Txt);
            this.Setting_Tab.Controls.Add(this.groupBox4);
            this.Setting_Tab.Controls.Add(this.Setting_Save_BT);
            this.Setting_Tab.Controls.Add(this.label3);
            this.Setting_Tab.Controls.Add(this.groupBox2);
            this.Setting_Tab.Controls.Add(this.Tab1groupSerSetting);
            this.Setting_Tab.Location = new System.Drawing.Point(4, 22);
            this.Setting_Tab.Name = "Setting_Tab";
            this.Setting_Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Setting_Tab.Size = new System.Drawing.Size(678, 509);
            this.Setting_Tab.TabIndex = 1;
            this.Setting_Tab.Text = "Setting";
            this.Setting_Tab.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(184, 229);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 37;
            this.button1.Text = "Open";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Port_Close_BT
            // 
            this.Port_Close_BT.Location = new System.Drawing.Point(103, 229);
            this.Port_Close_BT.Name = "Port_Close_BT";
            this.Port_Close_BT.Size = new System.Drawing.Size(75, 23);
            this.Port_Close_BT.TabIndex = 25;
            this.Port_Close_BT.Text = "Close";
            this.Port_Close_BT.UseVisualStyleBackColor = true;
            this.Port_Close_BT.Click += new System.EventHandler(this.Port_Close_BT_Click);
            // 
            // Setting_MSNV_Txt
            // 
            this.Setting_MSNV_Txt.Location = new System.Drawing.Point(108, 166);
            this.Setting_MSNV_Txt.Name = "Setting_MSNV_Txt";
            this.Setting_MSNV_Txt.Size = new System.Drawing.Size(160, 20);
            this.Setting_MSNV_Txt.TabIndex = 36;
            this.Setting_MSNV_Txt.TextChanged += new System.EventHandler(this.Setting_MSNV_Txt_TextChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.Setting_Tracking_Rbt);
            this.groupBox4.Controls.Add(this.Setting_ViewMode_Rbt);
            this.groupBox4.Location = new System.Drawing.Point(288, 74);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(273, 77);
            this.groupBox4.TabIndex = 35;
            this.groupBox4.TabStop = false;
            // 
            // Setting_Tracking_Rbt
            // 
            this.Setting_Tracking_Rbt.AutoSize = true;
            this.Setting_Tracking_Rbt.Location = new System.Drawing.Point(6, 47);
            this.Setting_Tracking_Rbt.Name = "Setting_Tracking_Rbt";
            this.Setting_Tracking_Rbt.Size = new System.Drawing.Size(67, 17);
            this.Setting_Tracking_Rbt.TabIndex = 0;
            this.Setting_Tracking_Rbt.Text = "Tracking";
            this.Setting_Tracking_Rbt.UseVisualStyleBackColor = true;
            this.Setting_Tracking_Rbt.CheckedChanged += new System.EventHandler(this.Setting_Tracking_Rbt_CheckedChanged);
            // 
            // Setting_ViewMode_Rbt
            // 
            this.Setting_ViewMode_Rbt.AutoSize = true;
            this.Setting_ViewMode_Rbt.Checked = true;
            this.Setting_ViewMode_Rbt.Location = new System.Drawing.Point(6, 23);
            this.Setting_ViewMode_Rbt.Name = "Setting_ViewMode_Rbt";
            this.Setting_ViewMode_Rbt.Size = new System.Drawing.Size(75, 17);
            this.Setting_ViewMode_Rbt.TabIndex = 0;
            this.Setting_ViewMode_Rbt.TabStop = true;
            this.Setting_ViewMode_Rbt.Text = "View_Only";
            this.Setting_ViewMode_Rbt.UseVisualStyleBackColor = true;
            this.Setting_ViewMode_Rbt.CheckedChanged += new System.EventHandler(this.Setting_ViewMode_Rbt_CheckedChanged);
            // 
            // Setting_Save_BT
            // 
            this.Setting_Save_BT.Location = new System.Drawing.Point(3, 229);
            this.Setting_Save_BT.Name = "Setting_Save_BT";
            this.Setting_Save_BT.Size = new System.Drawing.Size(75, 23);
            this.Setting_Save_BT.TabIndex = 34;
            this.Setting_Save_BT.Text = "Save";
            this.Setting_Save_BT.UseVisualStyleBackColor = true;
            this.Setting_Save_BT.Click += new System.EventHandler(this.Setting_Save_BT_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(12, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "MSNV";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Setting_WSTID_Cbx);
            this.groupBox2.Controls.Add(this.Setting_LineID_Cbx);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(6, 74);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(276, 77);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            // 
            // Setting_WSTID_Cbx
            // 
            this.Setting_WSTID_Cbx.FormattingEnabled = true;
            this.Setting_WSTID_Cbx.Location = new System.Drawing.Point(102, 43);
            this.Setting_WSTID_Cbx.Name = "Setting_WSTID_Cbx";
            this.Setting_WSTID_Cbx.Size = new System.Drawing.Size(160, 21);
            this.Setting_WSTID_Cbx.TabIndex = 6;
            // 
            // Setting_LineID_Cbx
            // 
            this.Setting_LineID_Cbx.FormattingEnabled = true;
            this.Setting_LineID_Cbx.Location = new System.Drawing.Point(102, 17);
            this.Setting_LineID_Cbx.Name = "Setting_LineID_Cbx";
            this.Setting_LineID_Cbx.Size = new System.Drawing.Size(160, 21);
            this.Setting_LineID_Cbx.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(6, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "Work Station: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(6, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Line: ";
            // 
            // Tab1groupSerSetting
            // 
            this.Tab1groupSerSetting.Controls.Add(this.Tab1SetBRLabel);
            this.Tab1groupSerSetting.Controls.Add(this.Tab1SetStopbit);
            this.Tab1groupSerSetting.Controls.Add(this.Tab1ComPortSelect);
            this.Tab1groupSerSetting.Controls.Add(this.Tab1SetParity);
            this.Tab1groupSerSetting.Controls.Add(this.label1);
            this.Tab1groupSerSetting.Controls.Add(this.Tab1SetDatabit);
            this.Tab1groupSerSetting.Controls.Add(this.Tab1SetBaudrate);
            this.Tab1groupSerSetting.Controls.Add(this.Tab1SetDataBitLabel);
            this.Tab1groupSerSetting.Controls.Add(this.Tab1SetParityLabel);
            this.Tab1groupSerSetting.Controls.Add(this.Tab1SetStopBitLabel);
            this.Tab1groupSerSetting.Location = new System.Drawing.Point(6, 6);
            this.Tab1groupSerSetting.Name = "Tab1groupSerSetting";
            this.Tab1groupSerSetting.Size = new System.Drawing.Size(555, 62);
            this.Tab1groupSerSetting.TabIndex = 32;
            this.Tab1groupSerSetting.TabStop = false;
            // 
            // Tab1SetBRLabel
            // 
            this.Tab1SetBRLabel.AutoSize = true;
            this.Tab1SetBRLabel.Location = new System.Drawing.Point(94, 11);
            this.Tab1SetBRLabel.Name = "Tab1SetBRLabel";
            this.Tab1SetBRLabel.Size = new System.Drawing.Size(50, 13);
            this.Tab1SetBRLabel.TabIndex = 21;
            this.Tab1SetBRLabel.Text = "Baudrate";
            // 
            // Tab1SetStopbit
            // 
            this.Tab1SetStopbit.FormattingEnabled = true;
            this.Tab1SetStopbit.Items.AddRange(new object[] {
            "One",
            "Two",
            "OnePointFive"});
            this.Tab1SetStopbit.Location = new System.Drawing.Point(331, 28);
            this.Tab1SetStopbit.Name = "Tab1SetStopbit";
            this.Tab1SetStopbit.Size = new System.Drawing.Size(65, 21);
            this.Tab1SetStopbit.TabIndex = 12;
            this.Tab1SetStopbit.Text = "One";
            // 
            // Tab1ComPortSelect
            // 
            this.Tab1ComPortSelect.FormattingEnabled = true;
            this.Tab1ComPortSelect.Location = new System.Drawing.Point(8, 28);
            this.Tab1ComPortSelect.Name = "Tab1ComPortSelect";
            this.Tab1ComPortSelect.Size = new System.Drawing.Size(75, 21);
            this.Tab1ComPortSelect.TabIndex = 1;
            this.Tab1ComPortSelect.Text = "NONE";
            // 
            // Tab1SetParity
            // 
            this.Tab1SetParity.FormattingEnabled = true;
            this.Tab1SetParity.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space"});
            this.Tab1SetParity.Location = new System.Drawing.Point(246, 28);
            this.Tab1SetParity.Name = "Tab1SetParity";
            this.Tab1SetParity.Size = new System.Drawing.Size(52, 21);
            this.Tab1SetParity.TabIndex = 11;
            this.Tab1SetParity.Text = "None";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "ComPort";
            // 
            // Tab1SetDatabit
            // 
            this.Tab1SetDatabit.FormattingEnabled = true;
            this.Tab1SetDatabit.Items.AddRange(new object[] {
            "8",
            "7",
            "6",
            "5"});
            this.Tab1SetDatabit.Location = new System.Drawing.Point(176, 28);
            this.Tab1SetDatabit.Name = "Tab1SetDatabit";
            this.Tab1SetDatabit.Size = new System.Drawing.Size(47, 21);
            this.Tab1SetDatabit.TabIndex = 10;
            this.Tab1SetDatabit.Tag = "";
            this.Tab1SetDatabit.Text = "8";
            // 
            // Tab1SetBaudrate
            // 
            this.Tab1SetBaudrate.FormattingEnabled = true;
            this.Tab1SetBaudrate.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.Tab1SetBaudrate.Location = new System.Drawing.Point(97, 28);
            this.Tab1SetBaudrate.Name = "Tab1SetBaudrate";
            this.Tab1SetBaudrate.Size = new System.Drawing.Size(60, 21);
            this.Tab1SetBaudrate.TabIndex = 9;
            this.Tab1SetBaudrate.Text = "9600";
            // 
            // Tab1SetDataBitLabel
            // 
            this.Tab1SetDataBitLabel.AutoSize = true;
            this.Tab1SetDataBitLabel.Location = new System.Drawing.Point(173, 11);
            this.Tab1SetDataBitLabel.Name = "Tab1SetDataBitLabel";
            this.Tab1SetDataBitLabel.Size = new System.Drawing.Size(50, 13);
            this.Tab1SetDataBitLabel.TabIndex = 22;
            this.Tab1SetDataBitLabel.Text = "Data Bits";
            // 
            // Tab1SetParityLabel
            // 
            this.Tab1SetParityLabel.AutoSize = true;
            this.Tab1SetParityLabel.Location = new System.Drawing.Point(243, 11);
            this.Tab1SetParityLabel.Name = "Tab1SetParityLabel";
            this.Tab1SetParityLabel.Size = new System.Drawing.Size(33, 13);
            this.Tab1SetParityLabel.TabIndex = 23;
            this.Tab1SetParityLabel.Text = "Parity";
            // 
            // Tab1SetStopBitLabel
            // 
            this.Tab1SetStopBitLabel.AutoSize = true;
            this.Tab1SetStopBitLabel.Location = new System.Drawing.Point(328, 11);
            this.Tab1SetStopBitLabel.Name = "Tab1SetStopBitLabel";
            this.Tab1SetStopBitLabel.Size = new System.Drawing.Size(49, 13);
            this.Tab1SetStopBitLabel.TabIndex = 24;
            this.Tab1SetStopBitLabel.Text = "Stop Bits";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(686, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolToolStripMenuItem
            // 
            this.toolToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trackingViewToolStripMenuItem,
            this.layoutManagementToolStripMenuItem,
            this.lineLayoutToolStripMenuItem});
            this.toolToolStripMenuItem.Name = "toolToolStripMenuItem";
            this.toolToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.toolToolStripMenuItem.Text = "Tool";
            // 
            // trackingViewToolStripMenuItem
            // 
            this.trackingViewToolStripMenuItem.Name = "trackingViewToolStripMenuItem";
            this.trackingViewToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.trackingViewToolStripMenuItem.Text = "Tracking_View";
            this.trackingViewToolStripMenuItem.Click += new System.EventHandler(this.trackingViewToolStripMenuItem_Click);
            // 
            // layoutManagementToolStripMenuItem
            // 
            this.layoutManagementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.designToolToolStripMenuItem,
            this.databaseToolStripMenuItem});
            this.layoutManagementToolStripMenuItem.Name = "layoutManagementToolStripMenuItem";
            this.layoutManagementToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.layoutManagementToolStripMenuItem.Text = "Layout Management";
            // 
            // designToolToolStripMenuItem
            // 
            this.designToolToolStripMenuItem.Name = "designToolToolStripMenuItem";
            this.designToolToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.designToolToolStripMenuItem.Text = "Design Tool";
            this.designToolToolStripMenuItem.Click += new System.EventHandler(this.designToolToolStripMenuItem_Click);
            // 
            // databaseToolStripMenuItem
            // 
            this.databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            this.databaseToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.databaseToolStripMenuItem.Text = "Database";
            this.databaseToolStripMenuItem.Click += new System.EventHandler(this.databaseToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel1,
            this.StatusLabel2,
            this.filterStatusLabel,
            this.showAllLabel,
            this.ProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 565);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(686, 22);
            this.statusStrip1.TabIndex = 33;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel1
            // 
            this.StatusLabel1.Name = "StatusLabel1";
            this.StatusLabel1.Size = new System.Drawing.Size(73, 17);
            this.StatusLabel1.Text = "StatusLabel1";
            // 
            // StatusLabel2
            // 
            this.StatusLabel2.Name = "StatusLabel2";
            this.StatusLabel2.Size = new System.Drawing.Size(73, 17);
            this.StatusLabel2.Text = "StatusLabel2";
            // 
            // filterStatusLabel
            // 
            this.filterStatusLabel.Name = "filterStatusLabel";
            this.filterStatusLabel.Size = new System.Drawing.Size(73, 17);
            this.filterStatusLabel.Text = "StatusLabel3";
            // 
            // showAllLabel
            // 
            this.showAllLabel.Name = "showAllLabel";
            this.showAllLabel.Size = new System.Drawing.Size(73, 17);
            this.showAllLabel.Text = "StatusLabel4";
            this.showAllLabel.Click += new System.EventHandler(this.StatusLabel4_Click);
            // 
            // ProgressBar1
            // 
            this.ProgressBar1.Name = "ProgressBar1";
            this.ProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // AutoCheck_Timer
            // 
            this.AutoCheck_Timer.Interval = 1000;
            this.AutoCheck_Timer.Tick += new System.EventHandler(this.AutoCheck_Timer_Tick);
            // 
            // ForceClose_Timer
            // 
            this.ForceClose_Timer.Interval = 1000;
            this.ForceClose_Timer.Tick += new System.EventHandler(this.ForceClose_Timer_Tick);
            // 
            // lineLayoutToolStripMenuItem
            // 
            this.lineLayoutToolStripMenuItem.Name = "lineLayoutToolStripMenuItem";
            this.lineLayoutToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.lineLayoutToolStripMenuItem.Text = "Line Layout";
            this.lineLayoutToolStripMenuItem.Click += new System.EventHandler(this.lineLayoutToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 587);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Jobs Display ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.YourJobs_TabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.YourJob_GridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.Tracking_Tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Tracking_Status_GridView)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tracking_Kitting_PO_Grv)).EndInit();
            this.Setting_Tab.ResumeLayout(false);
            this.Setting_Tab.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.Tab1groupSerSetting.ResumeLayout(false);
            this.Tab1groupSerSetting.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort SerialPort1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage YourJobs_TabPage;
        private System.Windows.Forms.TabPage Setting_Tab;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.GroupBox Tab1groupSerSetting;
        private System.Windows.Forms.Label Tab1SetBRLabel;
        private System.Windows.Forms.ComboBox Tab1ComPortSelect;
        private System.Windows.Forms.ComboBox Tab1SetParity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Tab1SetDatabit;
        private System.Windows.Forms.ComboBox Tab1SetBaudrate;
        private System.Windows.Forms.Label Tab1SetDataBitLabel;
        private System.Windows.Forms.Label Tab1SetParityLabel;
        private System.Windows.Forms.Label Tab1SetStopBitLabel;
        private System.Windows.Forms.ComboBox Tab1SetStopbit;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel filterStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel showAllLabel;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar1;
        private System.Windows.Forms.TextBox YourJob_MSNV_Txt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label YourJob_EmplName_Lbl;
        private System.Windows.Forms.DataGridView YourJob_GridView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label YourJobs_Date_Lbl;
        private System.Windows.Forms.Label YourJob_Shift_LBL;
        private System.Windows.Forms.ToolStripMenuItem toolToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Timer AutoCheck_Timer;
        private System.Windows.Forms.TabPage Tracking_Tab;
        private System.Windows.Forms.DataGridView Tracking_Status_GridView;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox Tracking_MSNV_Txt;
        private System.Windows.Forms.Label Tracking_EmplName_Lbl;
        private System.Windows.Forms.Label Tracking_Shift_LBL;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox Setting_WSTID_Cbx;
        private System.Windows.Forms.ComboBox Setting_LineID_Cbx;
        private System.Windows.Forms.Timer ForceClose_Timer;
        private System.Windows.Forms.Button Setting_Save_BT;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton Setting_Tracking_Rbt;
        private System.Windows.Forms.RadioButton Setting_ViewMode_Rbt;
        private System.Windows.Forms.TextBox Setting_MSNV_Txt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem trackingViewToolStripMenuItem;
        private System.Windows.Forms.Button Port_Close_BT;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem layoutManagementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem designToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseToolStripMenuItem;
        private System.Windows.Forms.Button Tracking_StopPO_BT;
        private System.Windows.Forms.Button Tracking_StartPO_BT;
        private System.Windows.Forms.Button Tracking_RefreshPO_BT;
        private System.Windows.Forms.DataGridView Tracking_Kitting_PO_Grv;
        private System.Windows.Forms.Label Tracking_PartNumber_Txt;
        private System.Windows.Forms.Label Traking_PO;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton Setting_Out_Check;
        private System.Windows.Forms.RadioButton Setting_In_Check;
        private System.Windows.Forms.Button Tracking_LayoutBT;
        private System.Windows.Forms.RadioButton In_Manual_Rb;
        private System.Windows.Forms.RadioButton FRU_Rb;
        private System.Windows.Forms.Button Tracking_Find_BT;
        private System.Windows.Forms.ToolStripMenuItem lineLayoutToolStripMenuItem;
    }
}

