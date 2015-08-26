namespace AddFRU
{
    partial class Add_FRU
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Shift_1 = new System.Windows.Forms.RadioButton();
            this.Shift_3 = new System.Windows.Forms.RadioButton();
            this.Shift_2 = new System.Windows.Forms.RadioButton();
            this.NumOfWST = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OK_BT = new System.Windows.Forms.Button();
            this.Cancel_BT = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Shift_1);
            this.groupBox1.Controls.Add(this.Shift_3);
            this.groupBox1.Controls.Add(this.Shift_2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(103, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // Shift_1
            // 
            this.Shift_1.AutoSize = true;
            this.Shift_1.Location = new System.Drawing.Point(6, 19);
            this.Shift_1.Name = "Shift_1";
            this.Shift_1.Size = new System.Drawing.Size(58, 17);
            this.Shift_1.TabIndex = 2;
            this.Shift_1.TabStop = true;
            this.Shift_1.Text = "Shift_1";
            this.Shift_1.UseVisualStyleBackColor = true;
            this.Shift_1.CheckedChanged += new System.EventHandler(this.Shift_CheckChange);
            // 
            // Shift_3
            // 
            this.Shift_3.AutoSize = true;
            this.Shift_3.Location = new System.Drawing.Point(6, 65);
            this.Shift_3.Name = "Shift_3";
            this.Shift_3.Size = new System.Drawing.Size(58, 17);
            this.Shift_3.TabIndex = 1;
            this.Shift_3.TabStop = true;
            this.Shift_3.Text = "Shift_3";
            this.Shift_3.UseVisualStyleBackColor = true;
            this.Shift_3.CheckedChanged += new System.EventHandler(this.Shift_CheckChange);
            // 
            // Shift_2
            // 
            this.Shift_2.AutoSize = true;
            this.Shift_2.Location = new System.Drawing.Point(6, 42);
            this.Shift_2.Name = "Shift_2";
            this.Shift_2.Size = new System.Drawing.Size(58, 17);
            this.Shift_2.TabIndex = 1;
            this.Shift_2.TabStop = true;
            this.Shift_2.Text = "Shift_2";
            this.Shift_2.UseVisualStyleBackColor = true;
            this.Shift_2.CheckedChanged += new System.EventHandler(this.Shift_CheckChange);
            // 
            // NumOfWST
            // 
            this.NumOfWST.Location = new System.Drawing.Point(130, 30);
            this.NumOfWST.Name = "NumOfWST";
            this.NumOfWST.Size = new System.Drawing.Size(91, 20);
            this.NumOfWST.TabIndex = 1;
            this.NumOfWST.Text = "1";
            this.NumOfWST.TextChanged += new System.EventHandler(this.NumOfShift_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(127, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Num of WST";
            // 
            // OK_BT
            // 
            this.OK_BT.Location = new System.Drawing.Point(130, 56);
            this.OK_BT.Name = "OK_BT";
            this.OK_BT.Size = new System.Drawing.Size(75, 23);
            this.OK_BT.TabIndex = 3;
            this.OK_BT.Text = "OK";
            this.OK_BT.UseVisualStyleBackColor = true;
            this.OK_BT.Click += new System.EventHandler(this.OK_BT_Click);
            // 
            // Cancel_BT
            // 
            this.Cancel_BT.Location = new System.Drawing.Point(130, 85);
            this.Cancel_BT.Name = "Cancel_BT";
            this.Cancel_BT.Size = new System.Drawing.Size(75, 23);
            this.Cancel_BT.TabIndex = 4;
            this.Cancel_BT.Text = "Cancel";
            this.Cancel_BT.UseVisualStyleBackColor = true;
            // 
            // Add_FRU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(233, 128);
            this.Controls.Add(this.Cancel_BT);
            this.Controls.Add(this.OK_BT);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NumOfWST);
            this.Controls.Add(this.groupBox1);
            this.Name = "Add_FRU";
            this.Text = "Add_FRU";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton Shift_2;
        private System.Windows.Forms.RadioButton Shift_1;
        private System.Windows.Forms.RadioButton Shift_3;
        private System.Windows.Forms.TextBox NumOfWST;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OK_BT;
        private System.Windows.Forms.Button Cancel_BT;
    }
}