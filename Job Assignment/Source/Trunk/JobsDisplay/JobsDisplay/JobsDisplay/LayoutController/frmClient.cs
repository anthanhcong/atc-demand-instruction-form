using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LayoutControl
{
    //Form that support view only
    class frmClient  : Form
    {
        private Line _line;

        private Label _title_Lbl = new Label();
        private Panel _panel = new Panel();
        private const bool _allowToModify = false;
        public string WST_Selected = "";

        private void InitLabel()
        {
            _title_Lbl.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            _title_Lbl.AutoSize = true;
            _title_Lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            _title_Lbl.ForeColor = System.Drawing.Color.Blue;
            _title_Lbl.Location = new System.Drawing.Point(98, 21);
            _title_Lbl.Name = "Title_Lbl";
            _title_Lbl.Size = new System.Drawing.Size(76, 33);
            _title_Lbl.TabIndex = 3;
            _title_Lbl.Text = "Title";
            _title_Lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        }

        private void InitPanel()
        {
            _panel.AutoScroll = true;
            _panel.Controls.Add(this._title_Lbl);
            _panel.Dock = System.Windows.Forms.DockStyle.Fill;
            _panel.Location = new System.Drawing.Point(0, 0);
            _panel.Name = "panel";
            _panel.Size = new System.Drawing.Size(284, 262);
            _panel.TabIndex = 4;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this._panel);
            this.Name = "ClientForm";
            this.Text = "ClientForm";
            this._panel.ResumeLayout(false);
            this._panel.PerformLayout();
            this.ResumeLayout(false);
        }

        public frmClient(string title, string lineID, string connection_str)
        {
            InitLabel();
            InitPanel();
            _line = new Line(this, _panel, false, false);

            //Get the list of wst from database
            LinesColletion_DataBase conn = new LinesColletion_DataBase(connection_str);

            if (lineID == "")
            {
                return;
            }

            string lineName = conn.FindLineName(lineID);

            if (title == "")
            {
                _line.LineName = lineName;
                _title_Lbl.Text = lineName + " Status";
            }
            else
            {
                _title_Lbl.Text = title;
            }

            DataTable wstList = conn.Load_List_of_WST(lineID);

            foreach (DataRow row in wstList.Rows)
            {
                int x = row["WST_x"] == DBNull.Value ? 0 : (int)row["WST_x"];
                int y = row["WST_y"] == DBNull.Value ? 0 : (int)row["WST_y"];

                int w = row["WST_width"] == DBNull.Value ? 30 : (int)row["WST_width"];
                int h = row["WST_heigh"] == DBNull.Value ? 30 : (int)row["WST_heigh"];

                if (w == 0) w = 30;
                if (h == 0) h = 30;

                Point initPoint = new Point(x, y);
                Size initSize = new Size(w, h);
                WST wst = new WST(initPoint, initSize, _allowToModify);
                wst.DescString = row["WST_Name"].ToString();
                wst.ID = row["WST_ID"].ToString();
                wst.RefreshInfo();
                _line.Add_Object(wst);

                wst.Click += new EventHandler(control_Click);
                this.AcceptButton = (Button)wst;
                ((Button)wst).DialogResult = DialogResult.OK;
            }

            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = false;
        }

        void control_Click(object sender, EventArgs e)
        {
            WST bt = (WST)sender;
            string name = bt.ID;
            if (bt.BackColor != Color.Red)
            {
                WST_Selected = "";
            }
            else
            {
                WST_Selected = name;
            }
            
        }

        public Line GetLineInstant()
        {
            return _line;
        }
    }
}
