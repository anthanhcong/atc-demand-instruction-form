namespace LayoutControl
{
    partial class frmInfoLabel
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
            this.btn_LableOk = new System.Windows.Forms.Button();
            this.btn_LabelCancel = new System.Windows.Forms.Button();
            this.txt_LabelContent = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_LableOk
            // 
            this.btn_LableOk.Location = new System.Drawing.Point(29, 84);
            this.btn_LableOk.Name = "btn_LableOk";
            this.btn_LableOk.Size = new System.Drawing.Size(105, 26);
            this.btn_LableOk.TabIndex = 3;
            this.btn_LableOk.Text = "OK";
            this.btn_LableOk.UseVisualStyleBackColor = true;
            this.btn_LableOk.Click += new System.EventHandler(this.btn_LableOk_Click);
            // 
            // btn_LabelCancel
            // 
            this.btn_LabelCancel.Location = new System.Drawing.Point(169, 84);
            this.btn_LabelCancel.Name = "btn_LabelCancel";
            this.btn_LabelCancel.Size = new System.Drawing.Size(105, 26);
            this.btn_LabelCancel.TabIndex = 4;
            this.btn_LabelCancel.Text = "Cancel";
            this.btn_LabelCancel.UseVisualStyleBackColor = true;
            this.btn_LabelCancel.Click += new System.EventHandler(this.btn_LabelCancel_Click);
            // 
            // txt_LabelContent
            // 
            this.txt_LabelContent.Location = new System.Drawing.Point(12, 47);
            this.txt_LabelContent.Name = "txt_LabelContent";
            this.txt_LabelContent.Size = new System.Drawing.Size(286, 20);
            this.txt_LabelContent.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(282, 24);
            this.label3.TabIndex = 0;
            this.label3.Text = "Please type your new content\r\n";
            // 
            // frmInfoLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 122);
            this.Controls.Add(this.btn_LabelCancel);
            this.Controls.Add(this.btn_LableOk);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_LabelContent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "InfoFormLabel";
            this.Text = "Label Info";
            this.Load += new System.EventHandler(this.InfoForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_LableOk;
        private System.Windows.Forms.Button btn_LabelCancel;
        private System.Windows.Forms.TextBox txt_LabelContent;
        private System.Windows.Forms.Label label3;
    }
}