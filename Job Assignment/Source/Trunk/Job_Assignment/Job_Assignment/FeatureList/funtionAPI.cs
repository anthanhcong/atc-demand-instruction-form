using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.IO.Ports;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using MasterDatabase;

namespace Job_Assignment
{
    public partial class Form1 : SQL_APPL
    {
        DataTable _empAndSkillList = null;
        DataTable _WstAndSkillList = null;
        DataTable _GroupAndLineList = null;

        MSSqlDbFactory dao = new MSSqlDbFactory();

        string SKILL_ID_COL = "Skill_ID";

        const string EmpAndSkill_DB_Cmd = "SELECT distinct [Skill_ID],[Empl_ID],[Empl_Name] FROM MDB_002_Empl_Skill";
        const string WstAndSkill_DB_Cmd = "SELECT distinct [WST_ID],[LineID],[Skill_ID]  FROM MDB_004_LineSkillRequest";
        const string GroupAndLine_DB_Cmd = "SELECT distinct [GroupID],[LineID]  FROM MDB_004_LineSkillRequest";

        public bool IsEmplHaveEnoughSkill(string emplID, string wstID)
        {
            if (_empAndSkillList == null || _WstAndSkillList == null || emplID == string.Empty || wstID == string.Empty)
            {
                return false;
            }

            //Find Skill ID
            List<string> list_SkillID_ForThisWST = GetSkillListForThisWST(wstID);
            List<string> list_SkillID_OfThisEmp = GetSkillListOfThisEmpl(emplID); ;

            if (list_SkillID_OfThisEmp != null && list_SkillID_ForThisWST != null)
            {
                if (CompareSkill_vs_RequiredSkill(emplID, "", list_SkillID_OfThisEmp, list_SkillID_ForThisWST))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsEmplHaveEnoughSkill_InDetail(string emplID, string wstID, ref string empSkill, ref string requiredSkill)
        {
            if (_empAndSkillList == null || _WstAndSkillList == null)
            {
                LoadInternalData();
            }

            if (_empAndSkillList == null || _WstAndSkillList == null || emplID == string.Empty || wstID == string.Empty)
            {
                return false;
            }

            //Find Skill ID
            List<string> list_SkillID_ForThisWST = GetSkillListForThisWST(wstID);
            List<string> list_SkillID_OfThisEmp = GetSkillListOfThisEmpl(emplID); ;

            empSkill = string.Empty;
            requiredSkill = string.Empty;

            foreach (var skill in list_SkillID_OfThisEmp)
            {
                if (empSkill == "")
                {
                    empSkill += skill.ToString();
                }
                else
                {
                    empSkill += string.Format(" - {0}", skill.ToString());
                }
            }

            foreach (var skill in list_SkillID_ForThisWST)
            {
                if (requiredSkill == "")
                {
                    requiredSkill += skill.ToString();
                }
                else
                {
                    requiredSkill += string.Format(" - {0}", skill.ToString());
                }
            }

            if (list_SkillID_OfThisEmp != null && list_SkillID_ForThisWST != null)
            {
                if (CompareSkill_vs_RequiredSkill(emplID, "", list_SkillID_OfThisEmp, list_SkillID_ForThisWST))
                {
                    return true;
                }
            }

            return false;
        }

        private List<string> GetSkillListForThisWST(string wst_ID)
        {
            if (wst_ID == string.Empty)
            {
                return null;
            }

            List<string> lst = new List<string>();
            string skillID;

            DataRow[] results = _WstAndSkillList.Select(ProrityTableCollumn.WST_COLUMN + " = '" + wst_ID + "'");

            if (results.Length > 0)
            {
                foreach (DataRow item in results)
                {
                    skillID = item[SKILL_ID_COL].ToString().Trim();
                    lst.Add(skillID);
                }
            }

            return lst;
        }

        private List<string> GetSkillListOfThisEmpl(string emp_ID)
        {
            if (emp_ID == string.Empty)
            {
                return null;
            }

            List<string> lst = new List<string>();
            string skillID;

            DataRow[] results = _empAndSkillList.Select(ProrityTableCollumn.EMPL_COLUMN + " = '" + emp_ID + "'");

            if (results.Length > 0)
            {
                foreach (DataRow item in results)
                {
                    skillID = item[SKILL_ID_COL].ToString().Trim();
                    lst.Add(skillID);
                }
            }

            return lst;
        }

        private bool CompareSkill_vs_RequiredSkill(string empl_ID, string empl_Name, List<string> listSkillOfThisEmpl, List<string> RequiredSkill)
        {
            if (listSkillOfThisEmpl == null || RequiredSkill == null)
            {
                return false;
            }

            if (ContainsAllItems(listSkillOfThisEmpl, RequiredSkill))
            {
                return true;
            }

            return false;
        }

        private bool ContainsAllItems(List<string> a, List<string> b)
        {
            return !b.Except(a).Any();
        }

        private bool LoadInternalData()
        {
            string ret;

            if (_empAndSkillList == null)
            {
                ret = dao.OpenDataTable(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, ref _empAndSkillList, CommandType.Text, EmpAndSkill_DB_Cmd);
            }

            if (_WstAndSkillList == null)
            {
                ret = dao.OpenDataTable(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, ref _WstAndSkillList, CommandType.Text, WstAndSkill_DB_Cmd);
            }

            return true;
        }
    }
}
