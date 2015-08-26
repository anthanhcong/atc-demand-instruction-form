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
    public partial class frmChangePasswordFirstLogin : Form
    {
        public frmChangePasswordFirstLogin()
        {
            InitializeComponent();
        }
        SecurityManager security = new SecurityManager();

        private void btOK_Click(object sender, EventArgs e)
        {
            if (ApplicationSession.UserLoginInfo == null)
                return;

            string err = ValidateInput();

            if (String.IsNullOrEmpty(err))
            {
                err = security.ResetPassword(ApplicationSession.UserLoginInfo.UserId, tbNewPassword.Text);
            }

            if (!String.IsNullOrEmpty(err))
                MessageBox.Show(err, "Thông báo");
            else
            {
                MessageBox.Show("Thay đổi mật khẩu thành công", "Thông báo");
                ApplicationSession.UserLoginInfo.Password = HashHelper.computeHash(tbNewPassword.Text);
                this.DialogResult = DialogResult.OK;
            }
        }

        private String ValidateInput()
        {
            if (String.IsNullOrEmpty(tbNewPassword.Text))
                return "Vui lòng nhập mật khẩu";

            if (tbNewPassword.Text != tbRetypeNewPassword.Text)
                return "Hai mật khẩu không giống nhau";

            return "";
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            //tbOldPassword.Focus();
        }
    }
}
