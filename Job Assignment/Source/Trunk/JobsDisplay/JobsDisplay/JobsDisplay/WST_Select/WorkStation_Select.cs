using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MasterDatabase;

namespace JobsDisplay
{
    public partial class WorkStation_Select : Form
    {
        public string WST_Select;
        public Button_Lbl[] bt_list;

        public WorkStation_Select(string [] wst_List)
        {
            InitializeComponent();

            this.MinimizeBox = false;
            this.MaximizeBox = false;
            InitializeComponent();
            // this.AcceptButton = Select_BT;

            string cur_wst;
            int total = wst_List.Count();
            int i = 0;
            PosSize possize = new PosSize();
            bt_list = new Button_Lbl[total];

            for (i = 0; i < total; i++)
            {
                cur_wst = wst_List[i].ToString().Trim();
                if (cur_wst != "")
                {
                    possize.pos_x = 6 + (i % 5) * 110;
                    possize.pos_y = 6 + (i / 5) * 28;
                    bt_list[i] = new Button_Lbl(i, null, wst_List[i], possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
                    bt_list[i].My_Button.Click += new EventHandler(Button_Click);
                    this.AcceptButton = bt_list[i].My_Button;
                    bt_list[i].My_Button.DialogResult = DialogResult.OK;
                    this.Controls.Add(bt_list[i].My_Button);
                }
            }

            this.CancelButton = Cancel_BT;
            // Select_BT.DialogResult = DialogResult.OK;
            Cancel_BT.DialogResult = DialogResult.Cancel;
            // DialogResult dialogResult = this.ShowDialog();
        }

        void Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            WST_Select = bt.Text;
        }
    }
}
