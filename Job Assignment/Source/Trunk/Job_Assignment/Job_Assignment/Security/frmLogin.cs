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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        MSSqlDbFactory Dao = new MSSqlDbFactory();
        private void btnOK_Click(object sender, EventArgs e)
        {
            string err = ValidateInput();

            if (String.IsNullOrEmpty(err))
            {
                err = LoginProcess(tbUserName.Text, tbPassword.Text);
            }

            if (!String.IsNullOrEmpty(err))
                MessageBox.Show(err, "Thông báo");
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private string LoginProcess(string userId, string password)
        {
            SecurityManager securityManager = new SecurityManager();
            UserInfo loginInfo = null;

            string err = securityManager.GetUserInfo(userId, ref loginInfo);

            if (!String.IsNullOrEmpty(err))
                return err;

            if (loginInfo == null)
                return "Tài khoản không tồn tại";

            if (loginInfo.PinStatus != "01")
                return "Tài khoản chưa được kích hoạt";

            String hashPassword = HashHelper.computeHash(password);

            if (hashPassword != loginInfo.Password)
                return "Mật khẩu không đúng";

            //load role
            Dictionary<string, UserRole> roles = null;
            err = securityManager.LoadUserRole(userId, ref roles);
            if (!String.IsNullOrEmpty(err))
                return err;

            loginInfo.Roles = roles;
            ApplicationSession.UserLoginInfo = loginInfo;

            err = Dao.ExecuteNonQuery(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, CommandType.Text, "update S001_AccountManage set LastLoginDate=? where UserId=?", DateTime.Now, userId);

            return "";
        }


        private String ValidateInput()
        {
            if (String.IsNullOrEmpty(tbUserName.Text))
                return "Vui lòng nhập tài khoản";

            if (String.IsNullOrEmpty(tbPassword.Text))
                return "Vui lòng nhập mật khẩu";

            return "";
        }
        private void btCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            tbUserName.Focus();
        }
    }
}
