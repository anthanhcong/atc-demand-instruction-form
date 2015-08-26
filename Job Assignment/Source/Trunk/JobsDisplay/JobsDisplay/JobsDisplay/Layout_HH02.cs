using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JobsDisplay
{
    public partial class Layout_HH02 : Form
    {
        string Active_Node = "";
        string Ready_Notes = "";
        public string WST_Selected;


        public Layout_HH02(string active_node, string ready_nodes, string title)
        {
            InitializeComponent();
            Active_Node = active_node;
            Ready_Notes = ready_nodes;
            Active_Node = Active_Node.Replace('-', '_');
            this.AcceptButton = Confirm_BT;
            Confirm_BT.DialogResult = DialogResult.OK;
            Title_Lbl.Text = title;
        }

        private void Layout_HH02_Load(object sender, EventArgs e)
        {
            //Sample of using Layout_HH02_Controller
            Layout_HH02_Controller ctrl = new Layout_HH02_Controller(this);

            //Add the list of button(wst).
            //Note: The name of button have to be the same with the name of wst in that line
            ctrl.Add(HH02_14);
            ctrl.Add(HH02_13);
            ctrl.Add(HH02_12);
            ctrl.Add(HH02_11);

            ctrl.Add(HH02_10);
            ctrl.Add(HH02_09);
            ctrl.Add(HH02_08);
            ctrl.Add(HH02_07);
            ctrl.Add(HH02_06);
            ctrl.Add(HH02_05);
            ctrl.Add(HH02_04);
            ctrl.Add(HH02_03);
            ctrl.Add(HH02_02);
            ctrl.Add(HH02_01);

            ctrl.Add(SH02_01);
            ctrl.Add(SH02_02);
            ctrl.Add(SH02_03);
            

            //Dho: Extra setting to customize if needed
            // ctrl.SetInActiveColor(Color.LightGreen);
            ctrl.SetInActiveColor(Color.Red);
            ctrl.SetReadyColor(Color.LightGreen);
            ctrl.SetActiveColor(Color.LightYellow);
            ctrl.SetAltColorForBlink(Color.Yellow);
            ctrl.SetBlinkingSpeed(600);

            //Dho: Need to call this line below to refresh after init
            ctrl.SetInactiveLine();

            foreach (Control control in this.Controls)
            {
                if ((control is Button) &&( control != Confirm_BT))
                {
                    control.Click += new EventHandler(control_Click);
                    this.AcceptButton = (Button)control;
                    ((Button)control).DialogResult = DialogResult.OK;
                }
            }

            //When employee scan their name card, the system know
            //- which work station name is suitable for this employee
            //- Highlight the position of workstation in the line for employee to recognize where to go
            try
            {
                ctrl.SetActive(Active_Node);
            }
            catch
            {
                ctrl.SetInactiveLine();
            }

            string []ready_nodes = Ready_Notes.Split(';');
            string node;
            if (ready_nodes.Count() > 0)
            {
                foreach(string ready_node in ready_nodes)
                {
                    node = ready_node.Replace('_', '-');
                    ctrl.SetReady_WST(ready_node);
                }
            }

        }

        void control_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            string name = bt.Name;
            name = name.Replace('-', '_');
            WST_Selected = name;
        }


        public void Layout_HH02_Set_StaticActive(string wst_id)
        {
            Layout_HH02_Controller ctrl = new Layout_HH02_Controller(this);
            wst_id = wst_id.Replace('-', '_');

            try
            {
                ctrl.SetActive(wst_id);
                // ctrl.SetStaticActive(wst_id);
                // ctrl._tmr.Stop();
            }
            catch
            {
                // ctrl.SetInactiveLine();
            }
        }

    }
}
