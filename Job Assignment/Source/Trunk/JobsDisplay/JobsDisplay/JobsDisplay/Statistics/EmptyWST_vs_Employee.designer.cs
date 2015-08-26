namespace JobsDisplay.Statistics
{
    partial class EmptyWST_vs_Employee
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.StopLine_BT = new System.Windows.Forms.Button();
            this.InputMore_BT = new System.Windows.Forms.Button();
            this.RunWithCurrent_BT = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_CurrentEmplSkill = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_CurrentEmpl = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_RequiredSkill = new System.Windows.Forms.TextBox();
            this.txt_CurrentWST = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridviewWST = new System.Windows.Forms.DataGridView();
            this.gridview_FreeEmployee = new System.Windows.Forms.DataGridView();
            this.AutoClose_Timer = new System.Windows.Forms.Timer(this.components);
            this.panel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridviewWST)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridview_FreeEmployee)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label2.Location = new System.Drawing.Point(125, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 34);
            this.label2.TabIndex = 4;
            this.label2.Text = "List of Employee need to Assign Job";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(70, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(257, 34);
            this.label1.TabIndex = 5;
            this.label1.Text = "List of WorkStation need to be filled with Employee";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.StopLine_BT);
            this.panel3.Controls.Add(this.InputMore_BT);
            this.panel3.Controls.Add(this.RunWithCurrent_BT);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(862, 135);
            this.panel3.TabIndex = 6;
            // 
            // StopLine_BT
            // 
            this.StopLine_BT.Location = new System.Drawing.Point(305, 66);
            this.StopLine_BT.Name = "StopLine_BT";
            this.StopLine_BT.Size = new System.Drawing.Size(115, 63);
            this.StopLine_BT.TabIndex = 1;
            this.StopLine_BT.Text = "Ngưng Sản Xuất";
            this.StopLine_BT.UseVisualStyleBackColor = true;
            this.StopLine_BT.Click += new System.EventHandler(this.StopLine_BT_Click);
            // 
            // InputMore_BT
            // 
            this.InputMore_BT.Location = new System.Drawing.Point(151, 66);
            this.InputMore_BT.Name = "InputMore_BT";
            this.InputMore_BT.Size = new System.Drawing.Size(118, 63);
            this.InputMore_BT.TabIndex = 1;
            this.InputMore_BT.Text = "Nhận Thêm Người";
            this.InputMore_BT.UseVisualStyleBackColor = true;
            this.InputMore_BT.Click += new System.EventHandler(this.InputMore_BT_Click);
            // 
            // RunWithCurrent_BT
            // 
            this.RunWithCurrent_BT.Location = new System.Drawing.Point(12, 66);
            this.RunWithCurrent_BT.Name = "RunWithCurrent_BT";
            this.RunWithCurrent_BT.Size = new System.Drawing.Size(107, 63);
            this.RunWithCurrent_BT.TabIndex = 1;
            this.RunWithCurrent_BT.Text = "Sản xuất với số người hiện tại";
            this.RunWithCurrent_BT.UseVisualStyleBackColor = true;
            this.RunWithCurrent_BT.Click += new System.EventHandler(this.RunWithCurrent_BT_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(166, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(536, 37);
            this.label7.TabIndex = 0;
            this.label7.Text = "LIST WST REQUEST EMPLOYEE";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txt_CurrentEmplSkill);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txt_CurrentEmpl);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(3, 40);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(429, 113);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // txt_CurrentEmplSkill
            // 
            this.txt_CurrentEmplSkill.BackColor = System.Drawing.Color.White;
            this.txt_CurrentEmplSkill.Location = new System.Drawing.Point(120, 59);
            this.txt_CurrentEmplSkill.Multiline = true;
            this.txt_CurrentEmplSkill.Name = "txt_CurrentEmplSkill";
            this.txt_CurrentEmplSkill.ReadOnly = true;
            this.txt_CurrentEmplSkill.Size = new System.Drawing.Size(289, 40);
            this.txt_CurrentEmplSkill.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Skills have";
            // 
            // txt_CurrentEmpl
            // 
            this.txt_CurrentEmpl.BackColor = System.Drawing.Color.White;
            this.txt_CurrentEmpl.Location = new System.Drawing.Point(120, 26);
            this.txt_CurrentEmpl.Name = "txt_CurrentEmpl";
            this.txt_CurrentEmpl.ReadOnly = true;
            this.txt_CurrentEmpl.Size = new System.Drawing.Size(289, 20);
            this.txt_CurrentEmpl.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Selected Employee";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txt_RequiredSkill);
            this.groupBox1.Controls.Add(this.txt_CurrentWST);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(6, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(414, 113);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txt_RequiredSkill
            // 
            this.txt_RequiredSkill.BackColor = System.Drawing.Color.White;
            this.txt_RequiredSkill.Location = new System.Drawing.Point(128, 54);
            this.txt_RequiredSkill.Multiline = true;
            this.txt_RequiredSkill.Name = "txt_RequiredSkill";
            this.txt_RequiredSkill.ReadOnly = true;
            this.txt_RequiredSkill.Size = new System.Drawing.Size(260, 42);
            this.txt_RequiredSkill.TabIndex = 1;
            // 
            // txt_CurrentWST
            // 
            this.txt_CurrentWST.BackColor = System.Drawing.Color.White;
            this.txt_CurrentWST.Location = new System.Drawing.Point(128, 23);
            this.txt_CurrentWST.Name = "txt_CurrentWST";
            this.txt_CurrentWST.ReadOnly = true;
            this.txt_CurrentWST.Size = new System.Drawing.Size(260, 20);
            this.txt_CurrentWST.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Required Skills";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Sellected WorkStation";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 135);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.gridviewWST);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.gridview_FreeEmployee);
            this.splitContainer1.Size = new System.Drawing.Size(862, 495);
            this.splitContainer1.SplitterDistance = 423;
            this.splitContainer1.TabIndex = 7;
            // 
            // gridviewWST
            // 
            this.gridviewWST.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridviewWST.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridviewWST.Location = new System.Drawing.Point(6, 159);
            this.gridviewWST.Name = "gridviewWST";
            this.gridviewWST.Size = new System.Drawing.Size(413, 333);
            this.gridviewWST.TabIndex = 0;
            this.gridviewWST.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridviewWST_CellClick);
            // 
            // gridview_FreeEmployee
            // 
            this.gridview_FreeEmployee.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridview_FreeEmployee.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridview_FreeEmployee.Location = new System.Drawing.Point(3, 159);
            this.gridview_FreeEmployee.Name = "gridview_FreeEmployee";
            this.gridview_FreeEmployee.Size = new System.Drawing.Size(429, 333);
            this.gridview_FreeEmployee.TabIndex = 0;
            this.gridview_FreeEmployee.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridview_FreeEmployee_CellClick);
            // 
            // AutoClose_Timer
            // 
            this.AutoClose_Timer.Interval = 1000;
            this.AutoClose_Timer.Tick += new System.EventHandler(this.AutoClose_Timer_Tick);
            // 
            // EmptyWST_vs_Employee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 630);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel3);
            this.Name = "EmptyWST_vs_Employee";
            this.Text = "EmptyWST_vs_Employee";
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.EmptyWST_vs_Employee_MouseMove);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridviewWST)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridview_FreeEmployee)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt_CurrentEmplSkill;
        private System.Windows.Forms.TextBox txt_CurrentEmpl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_RequiredSkill;
        private System.Windows.Forms.TextBox txt_CurrentWST;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView gridviewWST;
        private System.Windows.Forms.DataGridView gridview_FreeEmployee;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button StopLine_BT;
        private System.Windows.Forms.Button InputMore_BT;
        private System.Windows.Forms.Button RunWithCurrent_BT;
        private System.Windows.Forms.Timer AutoClose_Timer;
    }
}