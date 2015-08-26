using System;
using System.Windows.Forms;

namespace LayoutControl
{
    public partial class frmInfoLabel : Form
    {
        public frmInfoLabel()
        {
            InitializeComponent();

            txt_LabelContent.Text = Label_DTO.Content;
        }

        private void InfoForm_Load(object sender, EventArgs e)
        {
            txt_LabelContent.Focus();
        }

        private void btn_LableOk_Click(object sender, EventArgs e)
        {
            if (txt_LabelContent.Text == "")
            {
                MessageBox.Show("Please fill the content of label",
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            Label_DTO.Content = txt_LabelContent.Text;
            
            //MessageBox.Show("Label's content has been updated",
            //    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void btn_LabelCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
