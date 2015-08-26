using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterDatabase;

namespace Job_Assignment
{
    public enum UserRoleName
    {
        VIEW_ONLY,
        IMPORT,
        CREATE
    }

    public class RoleHelper
    {
        public static void SetRole(MaterDatabase master, string moduleId)
        {
            UserInfo userInfo = ApplicationSession.UserLoginInfo;

            if (userInfo == null)
                return;

            if (GetCurrentUserLoginRole(UserRoleName.VIEW_ONLY, moduleId))
            {
                master.MasterDatabase_GridviewTBL.Import_BT.Enabled = false; // sao mình không dùng visible cho nó mất luôn
                master.MasterDatabase_GridviewTBL.Submit_BT.Enabled = false;
                master.MasterDatabase_GridviewTBL.Export_BT.Enabled = false;
                master.MasterDatabase_GridviewTBL.Delete_All_BT.Enabled = false;
                master.MasterDatabase_GridviewTBL.Delete_Rows_BT.Enabled = false;
            }
            else
            {
                master.MasterDatabase_GridviewTBL.Import_BT.Enabled = GetCurrentUserLoginRole(UserRoleName.IMPORT, moduleId);
                //master.MasterDatabase_GridviewTBL.Import_BT.Enabled = userInfo.Roles[moduleId].IsCreate;
            }
        }

        public static bool GetCurrentUserLoginRole(UserRoleName roleName, string moduleId)
        {
            UserInfo userInfo = ApplicationSession.UserLoginInfo;

            if (userInfo == null)
            {
                if (roleName == UserRoleName.VIEW_ONLY)
                    return true;

                return false;
            }

            bool isViewOnly = userInfo.Roles.ContainsKey(moduleId) ? userInfo.Roles[moduleId].IsViewOnly : true;

            if (roleName == UserRoleName.VIEW_ONLY)
                return isViewOnly;
            else if(roleName == UserRoleName.CREATE)
            {
                bool isCreate = userInfo.Roles.ContainsKey(moduleId) ?  userInfo.Roles[moduleId].IsCreate: false;
                return !isViewOnly && isCreate;
            }

            bool isImport = userInfo.Roles.ContainsKey(moduleId) ? userInfo.Roles[moduleId].IsImport : false;
            return !isViewOnly && isImport;
        }
     
    }
}
