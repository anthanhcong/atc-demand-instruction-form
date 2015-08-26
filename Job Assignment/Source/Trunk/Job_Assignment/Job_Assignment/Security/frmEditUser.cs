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
    public partial class frmEditUser : Form
    {
        public frmEditUser(string userId)
        {
            InitializeComponent();
            tbUserId.Text = userId;
        }
        SecurityManager security = new SecurityManager();
        UserInfo info;
        MSSqlDbFactory Dao = new MSSqlDbFactory();
        private void btOK_Click(object sender, EventArgs e)
        {
            if (ApplicationSession.UserLoginInfo == null)
                return;

            string err = ValidateUser();

            if (String.IsNullOrEmpty(err))
            {

                err = Dao.ExecuteNonQuery(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, CommandType.Text, "update S001_AccountManage set Name=?, IsAdmin=? where UserId=?", tbName.Text, chbAdmin.Checked, info.UserId);
                //if (String.IsNullOrEmpty(err))
                //{
                //err = Dao.ExecuteNonQuery(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, CommandType.Text, String.Format("insert into S002_UserRoleMapping(UserId,ModuleId,ModuleName,P_ViewOnly,P_Create,P_Import) select '{0}', ModuleId,ModuleName,P_ViewOnly,P_Create,P_Import from S002_UserRoleMapping where UserId='{1}'", tbUserId.Text, cbUserList.Text));
                //}
            }

            if (String.IsNullOrEmpty(err))
            {
                MessageBox.Show("Thay đổi thành công", "Thông báo");
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(err, "Thông báo");
            }
        }

        private String ValidateUser()
        {
            if (String.IsNullOrEmpty(tbUserId.Text))
                return "Tài khoản không được trống";

            if (String.IsNullOrEmpty(tbName.Text))
                return "Vui lòng nhập Tên Người Dùng";

            if (tbName.Text.Length > 50)
                return "Tên Người Dùng chỉ được nhập 50 ký tự";

            return "";
        }

        private void frmNewUser_Load(object sender, EventArgs e)
        {
            string err = security.GetUserInfo(tbUserId.Text, ref info);
            if (!String.IsNullOrEmpty(err))
            {
                this.Close();
                MessageBox.Show(err, "Thông báo");
            }
            else
            {
                tbName.Text = info.Name;
                chbAdmin.Checked = info.IsAdmin;
            }

        }
    }
}
