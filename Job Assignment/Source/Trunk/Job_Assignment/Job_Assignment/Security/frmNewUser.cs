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
    public partial class frmNewUser : Form
    {
        public frmNewUser()
        {
            InitializeComponent();
        }

        MSSqlDbFactory Dao = new MSSqlDbFactory();
        private void btOK_Click(object sender, EventArgs e)
        {
            if (ApplicationSession.UserLoginInfo == null)
                return;

            string err = ValidateUser();

            if (String.IsNullOrEmpty(err))
            {
                String password = HashHelper.computeHash(tbPassword.Text);

                err = Dao.ExecuteNonQuery(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, CommandType.Text, "insert into S001_AccountManage(UserId,Name,UserPin,PinStatus, IsAdmin, CreateDate, CreateBy) values(? , ?, ?, ?, ?, ?, ?)", tbUserId.Text, tbName.Text, password, "01", chbAdmin.Checked, DateTime.Now, ApplicationSession.UserLoginInfo.UserId);
                if (String.IsNullOrEmpty(err))
                {
                    err = Dao.ExecuteNonQuery(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, CommandType.Text, String.Format("insert into S002_UserRoleMapping(UserId,ModuleId,ModuleName,P_ViewOnly,P_Create,P_Import) select '{0}', ModuleId,ModuleName,P_ViewOnly,P_Create,P_Import from S002_UserRoleMapping where UserId='{1}'", tbUserId.Text, cbUserList.Text));
                }
            }

            if (String.IsNullOrEmpty(err))
            {
                MessageBox.Show("Tạo tài khoản thành công", "Thông báo");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(err, "Thông báo");
            }
        }

        private String ValidateUser()
        {
            if (String.IsNullOrEmpty(tbUserId.Text))
                return "Vui lòng nhập tài khoản";

            if (tbUserId.Text.Length > 20)
                return "Tên tài khoản tối đa 20 kí tự";

            if (String.IsNullOrEmpty(tbName.Text))
                return "Vui lòng nhập Tên Người Dùng";

            if (tbName.Text.Length > 50)
                return "Tên Người Dùng chỉ được nhập 50 ký tự";

            if (String.IsNullOrEmpty(tbPassword.Text))
                return "Vui lòng nhập mật khẩu";

            if (tbPassword.Text != tbRetypePassword.Text)
                return "Hai mật khẩu không giống nhau";

            if (cbUserList.SelectedIndex == -1 || cbUserList.Text == "")
                return "Vui lòng chọn Quyền hạn";

            return "";
        }

        private void frmNewUser_Load(object sender, EventArgs e)
        {
            DataTable tb = new DataTable();
            String err = Dao.OpenDataTable(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, ref tb, CommandType.Text, "select UserId, Name from S001_AccountManage where PinStatus='01'");

            if (string.IsNullOrEmpty(err))
            {
                cbUserList.DataSource = tb;
                cbUserList.DisplayMember = "UserId";
                cbUserList.ValueMember = "UserId";
                cbUserList.ColumnWidths = "50;200";
            }
            tbUserId.Focus();
        }
    }
}
