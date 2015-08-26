namespace JobsDisplay
{
    partial class WorkStation_Select
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
            this.Cancel_BT = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Cancel_BT
            // 
            this.Cancel_BT.Location = new System.Drawing.Point(219, 230);
            this.Cancel_BT.Name = "Cancel_BT";
            this.Cancel_BT.Size = new System.Drawing.Size(75, 23);
            this.Cancel_BT.TabIndex = 0;
            this.Cancel_BT.Text = "Cancel";
            this.Cancel_BT.UseVisualStyleBackColor = true;
            // 
            // WorkStation_Select
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 265);
            this.Controls.Add(this.Cancel_BT);
            this.Name = "WorkStation_Select";
            this.Text = "WorkStation_Select";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Cancel_BT;
    }
}