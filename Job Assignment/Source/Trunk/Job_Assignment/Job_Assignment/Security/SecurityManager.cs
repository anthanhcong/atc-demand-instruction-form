using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Job_Assignment
{
    public class SecurityManager
    {
        MSSqlDbFactory Dao = new MSSqlDbFactory();

        //public String ValidUser(string userId, string password)
        //{
        //    DataTable data = new DataTable();
        //    String err = Dao.OpenDataTable(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, ref data, CommandType.Text, "select * from S001_AccountManage where UserId=?", loginInfo.UserId);

        //    if (data == null || data.Rows.Count == 0)
        //        return "Tài khoản không tồn tại";

        //    String inputPassword = HashHelper.computeHash(password, "");

        //    if (inputPassword != data.Rows[0]["UserPin"])
        //        return "Mật khẩu không đúng";

        //    return "";
        //}
        //public string LoginProcess(string userId, string password)
        //{

        //}
        public String DeteleAccount(string userId)
        {
            String err = Dao.ExecuteNonQuery(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, CommandType.Text, "update S001_AccountManage set PinStatus=? where UserId=?", "99", userId);

            return err;
        }

        public String ResetPassword(string userId, string password)
        {
            String inputPassword = HashHelper.computeHash(password);
            String err = Dao.ExecuteNonQuery(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, CommandType.Text, "update S001_AccountManage set UserPin=? where UserId=?", inputPassword, userId );

            return err;
        }

        public String GetUserInfo(string userId, ref UserInfo info)
        {
            DataTable data = new DataTable();
            try
            {
                String err = Dao.OpenDataTable(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, ref data, CommandType.Text, "select * from S001_AccountManage where UserId=?", userId);

                if (String.IsNullOrEmpty(err) && data != null && data.Rows.Count > 0)
                {
                    info = new UserInfo();
                    info.UserId = userId;
                    info.Name = Utils.ObjectToString(data.Rows[0]["Name"]);
                    info.Password = Utils.ObjectToString(data.Rows[0]["UserPin"]);
                    info.PinStatus = Utils.ObjectToString(data.Rows[0]["PinStatus"]);
                    info.LastLoginDate = Utils.ObjectToDateTime(data.Rows[0]["LastLoginDate"], DateTime.MinValue);
                    info.IsAdmin = Utils.ObjectToBoolean(data.Rows[0]["IsAdmin"], false);
                }
                return err;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string LoadUserRole(string userId, ref Dictionary<string, UserRole> userRoles)
        {
            userRoles = new Dictionary<string, UserRole>();
            DataTable data = new DataTable();
            try
            {
                String err = Dao.OpenDataTable(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, ref data, CommandType.Text, "select * from S002_UserRoleMapping where UserId=?", userId);

                if (!String.IsNullOrEmpty(err))
                    return err;

                for (int i = 0; i < data.Rows.Count; i++)
                {
                    UserRole role = new UserRole();
                    role.UserId = Utils.ObjectToString(data.Rows[i]["UserId"]);
                    role.Module = Utils.ObjectToString(data.Rows[i]["ModuleId"]);
                    role.IsViewOnly = Utils.ObjectToBoolean(data.Rows[i]["P_ViewOnly"], false);
                    role.IsCreate = Utils.ObjectToBoolean(data.Rows[i]["P_Create"], false);
                    role.IsImport = Utils.ObjectToBoolean(data.Rows[i]["P_Import"], false);

                    if (!userRoles.ContainsKey(role.Module))
                    {
                        userRoles.Add(role.Module, role);
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }
    }
}
