using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Job_Assignment
{
    public partial class frmResetPassword : Form
    {
        public frmResetPassword(string _userId)
        {
            InitializeComponent();
            tbUserId.Text = _userId;
        }

        SecurityManager security = new SecurityManager();

        private void btOK_Click(object sender, EventArgs e)
        {
            string err = ValidateInput();

            if (String.IsNullOrEmpty(err))
            {
                err = security.ResetPassword(tbUserId.Text, tbPassword.Text);
            }

            if (!String.IsNullOrEmpty(err))
                MessageBox.Show(err, "Thông báo");
            else
            {
                MessageBox.Show("Thay đổi mật khẩu thành công", "Thông báo");
                this.DialogResult = DialogResult.OK;
            }
        }

        private String ValidateInput()
        {
            if (String.IsNullOrEmpty(tbPassword.Text))
                return "Vui lòng nhập mật khẩu";

            if (tbPassword.Text != tbRetypePassword.Text)
                return "Hai mật khẩu không giống nhau";

            return "";
        }
    }
}
