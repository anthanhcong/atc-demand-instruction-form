using System;
using System.Windows.Forms;

namespace LayoutControl
{
    public partial class frmInfo : Form
    {
        public frmInfo()
        {
            InitializeComponent();

            txt_WorkStationCode.Text = WST_DTO.WST_ID;
            txt_WorkStationDesc.Text = WST_DTO.WST_Desc;
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            if (txt_WorkStationCode.Text == "")
            {
                MessageBox.Show("Please fill the info of WorkStation Code. OR Press Cancel to exit without saving",
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            if (txt_WorkStationDesc.Text == "")
            {
                MessageBox.Show("Please fill the info of WorkStation Name. OR Press Cancel to exit without saving",
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            WST_DTO.WST_ID = txt_WorkStationCode.Text;
            WST_DTO.WST_Desc = txt_WorkStationDesc.Text;

            //MessageBox.Show("WorkStation's info has been updated",
            //                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void InfoForm_Load(object sender, EventArgs e)
        {
            txt_WorkStationDesc.Focus();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
