using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Job_Assignment
{
    public partial class frmManageUser : Form
    {
        public frmManageUser()
        {
            InitializeComponent();
        }
        
        MSSqlDbFactory Dao = new MSSqlDbFactory();
     //   SqlDataAdapter UserListAdapter = new SqlDataAdapter();
        SqlDataAdapter UserRoleMappingAdapter = new SqlDataAdapter();
        DataSet dsUserRoleMapping = new DataSet();
        SecurityManager security = new SecurityManager();

        private void frmManageUser_Load(object sender, EventArgs e)
        {
            dgvUser.AutoGenerateColumns = false;
            dgvUser.Columns.Add(DataGridViewHelper.CreateColumn(DataGridViewColumnType.TEXT, "UserId", "Tài khoản"));
            dgvUser.Columns.Add(DataGridViewHelper.CreateColumn(DataGridViewColumnType.TEXT, "Name", "Tên người dùng"));
            dgvUser.Columns.Add(DataGridViewHelper.CreateColumn(DataGridViewColumnType.CHECKBOX, "IsAdmin", "Admin"));
            dgvUser.Columns["UserId"].ReadOnly = true;
            dgvUser.Columns["Name"].ReadOnly = true;
            dgvUser.Columns["IsAdmin"].ReadOnly = true;
            
            dgvUser.Columns["UserId"].Width = 70;
            dgvUser.Columns["Name"].Width = 100;

            dgvRoleMapping.AutoGenerateColumns = false;
            dgvRoleMapping.Columns.Add(DataGridViewHelper.CreateColumn(DataGridViewColumnType.TEXT, "UserId", "Tài khoản"));
            dgvRoleMapping.Columns.Add(DataGridViewHelper.CreateColumn(DataGridViewColumnType.TEXT, "ModuleId", "Module"));
            dgvRoleMapping.Columns.Add(DataGridViewHelper.CreateColumn(DataGridViewColumnType.TEXT, "ModuleName", "Module"));
            dgvRoleMapping.Columns.Add(DataGridViewHelper.CreateColumn(DataGridViewColumnType.CHECKBOX, "P_ViewOnly", "View Only"));
            dgvRoleMapping.Columns.Add(DataGridViewHelper.CreateColumn(DataGridViewColumnType.CHECKBOX, "P_Create", "Create"));
            dgvRoleMapping.Columns.Add(DataGridViewHelper.CreateColumn(DataGridViewColumnType.CHECKBOX, "P_Import", "Import"));

            dgvRoleMapping.Columns["UserId"].Visible = false;
            dgvRoleMapping.Columns["ModuleId"].Visible = false;
            dgvRoleMapping.Columns["ModuleName"].ReadOnly = true;
            dgvRoleMapping.Columns["ModuleName"].Width = 200;
            LoadUser();
        }

        private string LoadUser()
        {
            DataTable tb = new DataTable();
            String err = Dao.OpenDataTable(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, ref tb, CommandType.Text, "select * from S001_AccountManage where PinStatus = ?", "01");
            if (String.IsNullOrEmpty(err))
            {
                DataGridViewHelper.BindingTableToGridView(dgvUser, tb);
            }
            return err;
        }
                
        private void btNewUser_Click(object sender, EventArgs e)
        {
            frmNewUser frmNewUser = new frmNewUser();

            if (frmNewUser.ShowDialog() == DialogResult.OK)
            {
                LoadUser();
            }
        }

        private void dgvUser_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUser.SelectedRows.Count > 0)
            {
                DataGridViewRow rowSelected = dgvUser.SelectedRows[0];
                String err = Dao.Get_SQL_Data(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, ref UserRoleMappingAdapter, ref dsUserRoleMapping, CommandType.Text, "select * from S002_UserRoleMapping where UserId=?", rowSelected.Cells["UserId"].Value);

                if (String.IsNullOrEmpty(err))
                {
                    DataGridViewHelper.BindingTableToGridView(dgvRoleMapping, dsUserRoleMapping.Tables[0]);
                }
                else
                {
                    MessageBox.Show(err, "Lỗi");
                }
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            String err = Dao.Update_SQL_Data(UserRoleMappingAdapter, dsUserRoleMapping.Tables[0]);
            if (String.IsNullOrEmpty(err))
            {
                MessageBox.Show("Cập nhật dữ liệu thành công", "Thông báo");
            }
            else
            {
                MessageBox.Show(err, "Lỗi");
            }
        }

        private void resetPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow currentRow = dgvUser.CurrentRow;

            if (currentRow != null)
            {
                string userId = Utils.ObjectToString( currentRow.Cells["UserId"].Value);

                if (!string.IsNullOrEmpty(userId))
                {
                    frmResetPassword frm = new frmResetPassword(userId);
                    frm.ShowDialog();

                }
            }
        }

        private void dgvUser_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    deleteAccountToolStripMenuItem.Enabled = true;
                    resetPasswordToolStripMenuItem.Enabled = true;
                    editAccountToolStripMenuItem.Enabled = true;

                    dgvUser.CurrentCell = dgvUser.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    // Can leave these here - doesn't hurt
                    dgvUser.Rows[e.RowIndex].Selected = true;
                    dgvUser.Focus();
                }
                else
                {
                    deleteAccountToolStripMenuItem.Enabled = false;
                    resetPasswordToolStripMenuItem.Enabled = false;
                    editAccountToolStripMenuItem.Enabled = false;
                }
            }
        }

        private void deleteAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow currentRow = dgvUser.CurrentRow;

            if (currentRow != null)
            {
                string userId = Utils.ObjectToString(currentRow.Cells["UserId"].Value);

                if (!string.IsNullOrEmpty(userId))
                {
                    if("admin".Equals(userId.ToLower()))
                    {
                        MessageBox.Show(String.Format("Bạn không thể xóa tài khoản {0}", userId), "Thông báo");
                        return;
                    }

                    if (MessageBox.Show(String.Format("Bạn có chắc chắn xóa tài khoản {0}?", userId), "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        string err = security.DeteleAccount(userId);

                        if (String.IsNullOrEmpty(err))
                        {
                            LoadUser();
                        }
                        else
                        {
                            MessageBox.Show(err, "Lỗi");
                        }
                    }
                }
            }

        }

        private void editAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmEditUser frm = new frmEditUser();
            DataGridViewRow currentRow = dgvUser.CurrentRow;

            if (currentRow != null)
            {
                string userId = Utils.ObjectToString(currentRow.Cells["UserId"].Value);

                if (!string.IsNullOrEmpty(userId))
                {
                    if ("admin".Equals(userId.ToLower()))
                    {
                        MessageBox.Show(String.Format("Bạn không được phép chỉnh sửa tài khoản {0}", userId), "Thông báo");
                        return;
                    }

                    frmEditUser frm = new frmEditUser(userId);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        LoadUser();
                    }

                }
            }
        }

    }
}
