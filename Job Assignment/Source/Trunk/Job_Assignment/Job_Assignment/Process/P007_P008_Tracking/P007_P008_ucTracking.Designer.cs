namespace Job_Assignment
{
    partial class P007_P008_ucTracking
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.btSearch = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btExportExcel = new System.Windows.Forms.Button();
            this.btDuplicate = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.dgvTracking = new System.Windows.Forms.DataGridView();
            this.btDeleteRow = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTracking)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dtpTo);
            this.panel1.Controls.Add(this.dtpFrom);
            this.panel1.Controls.Add(this.btSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(788, 41);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(299, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "To";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(163, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "From";
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd/MM/yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(325, 12);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(98, 20);
            this.dtpTo.TabIndex = 2;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd/MM/yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(196, 12);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(101, 20);
            this.dtpFrom.TabIndex = 1;
            // 
            // btSearch
            // 
            this.btSearch.Location = new System.Drawing.Point(429, 11);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(75, 23);
            this.btSearch.TabIndex = 0;
            this.btSearch.Text = "Search";
            this.btSearch.UseVisualStyleBackColor = true;
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btDeleteRow);
            this.panel2.Controls.Add(this.btExportExcel);
            this.panel2.Controls.Add(this.btDuplicate);
            this.panel2.Controls.Add(this.btSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 397);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(788, 40);
            this.panel2.TabIndex = 1;
            // 
            // btExportExcel
            // 
            this.btExportExcel.Location = new System.Drawing.Point(325, 6);
            this.btExportExcel.Name = "btExportExcel";
            this.btExportExcel.Size = new System.Drawing.Size(75, 23);
            this.btExportExcel.TabIndex = 3;
            this.btExportExcel.Text = "Export excel";
            this.btExportExcel.UseVisualStyleBackColor = true;
            this.btExportExcel.Click += new System.EventHandler(this.btExportExcel_Click);
            // 
            // btDuplicate
            // 
            this.btDuplicate.Location = new System.Drawing.Point(175, 6);
            this.btDuplicate.Name = "btDuplicate";
            this.btDuplicate.Size = new System.Drawing.Size(99, 23);
            this.btDuplicate.TabIndex = 2;
            this.btDuplicate.Text = "Duplicate row";
            this.btDuplicate.UseVisualStyleBackColor = true;
            this.btDuplicate.Click += new System.EventHandler(this.btDuplicate_Click);
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(445, 6);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 1;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // dgvTracking
            // 
            this.dgvTracking.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTracking.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTracking.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvTracking.Location = new System.Drawing.Point(0, 41);
            this.dgvTracking.Name = "dgvTracking";
            this.dgvTracking.Size = new System.Drawing.Size(788, 356);
            this.dgvTracking.TabIndex = 2;
            this.dgvTracking.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTracking_CellValueChanged);
            this.dgvTracking.BindingContextChanged += new System.EventHandler(this.dgvTracking_BindingContextChanged);
            this.dgvTracking.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTracking_CellClick);
            // 
            // btDeleteRow
            // 
            this.btDeleteRow.Location = new System.Drawing.Point(54, 6);
            this.btDeleteRow.Name = "btDeleteRow";
            this.btDeleteRow.Size = new System.Drawing.Size(82, 23);
            this.btDeleteRow.TabIndex = 4;
            this.btDeleteRow.Text = "Delete row";
            this.btDeleteRow.UseVisualStyleBackColor = true;
            this.btDeleteRow.Click += new System.EventHandler(this.btDeleteRow_Click);
            // 
            // P007_P008_ucTracking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvTracking);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P007_P008_ucTracking";
            this.Size = new System.Drawing.Size(788, 437);
            this.Load += new System.EventHandler(this.P007_P008_ucTracking_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTracking)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvTracking;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btDuplicate;
        private System.Windows.Forms.Button btExportExcel;
        private System.Windows.Forms.Button btDeleteRow;
    }
}
