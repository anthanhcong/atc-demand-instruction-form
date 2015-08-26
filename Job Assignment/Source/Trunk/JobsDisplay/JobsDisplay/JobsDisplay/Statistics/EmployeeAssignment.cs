using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Job_Assignment
{
    class EmployeeAssignment
    {
        DataTable _plan;            //Plan to arrange employee,
        DataTable _availableEmpList;//Could not be null
        List<DataTable> _priorityList;

        DataTable _empAndSkillList = null;
        DataTable _WstAndSkillList = null;

        MSSqlDbFactory dao = new MSSqlDbFactory();

        const string EmpAndSkill_DB_Cmd = "SELECT distinct [Skill_ID],[Empl_ID],[Empl_Name],[Priority] FROM MDB_002_Empl_Skill";
        const string WstAndSkill_DB_Cmd = "SELECT distinct [WST_ID],[LineID],[Skill_ID]  FROM MDB_004_LineSkillRequest";

        string WST_ID_COL = "WST_ID";
        string LINE_ID_COL = "LineID";
        string EMPL_ID_COL = "Empl_ID";
        string EMPL_NAME_COL = "Empl_Name";
        string SKILL_ID_COL = "Skill_ID";

        string ConnectionString = ""; //TODO:
        
        private string LoadInternalData()
        {
            string ret;
            

            if (_empAndSkillList == null)
            {
                ret = dao.OpenDataTable(ConnectionString, ref _empAndSkillList, CommandType.Text, EmpAndSkill_DB_Cmd);
            }

            if (true)
            {
                ret = dao.OpenDataTable(ConnectionString, ref _WstAndSkillList, CommandType.Text, WstAndSkill_DB_Cmd);
            }

            return ret;
        }

        public EmployeeAssignment(string connect_str)
        {
            ConnectionString = connect_str;
            LoadInternalData();
        }

        public EmployeeAssignment(DataTable plan,
                                    DataTable availableListOfEmpl,
                                    List<DataTable> priorityList)
        {
            _plan = plan;
            _availableEmpList = availableListOfEmpl;
            _priorityList = priorityList;

            LoadInternalData();
        }

        public DataTable GetFinalList()
        {
            if (_plan == null || _availableEmpList == null)
            {
                return null;
            }

            string workStationID, lineID;
            string emp_ID = "";
            string emp_Name = "";

            //Search trong danh sách ưu tiên.
            //Yêu cầu: format của những table thuộc danh sách ưu tiên cần có: column "line_ID" WST_ID_COL "Empl_ID"
            foreach (DataRow row in _plan.Rows)
            {
                //vị trí chưa assign --> Assign
                if ((string)row[EMPL_ID_COL].ToString().Trim() == string.Empty)
                {
                    workStationID = row[WST_ID_COL].ToString().Trim();
                    lineID = row[LINE_ID_COL].ToString().Trim();

                    if (GetEmployeeForThisWSTFromPriorityList(ref emp_ID, ref emp_Name, workStationID))
                    {
                        row[EMPL_ID_COL] = emp_ID;
                        row[EMPL_NAME_COL] = emp_Name;
                    }
                }
            }

            //Normal assign dựa trên skill.
            foreach (DataRow row in _plan.Rows)
            {
                //vị trí chưa assign --> Assign
                if (row[EMPL_ID_COL].ToString().Trim() == string.Empty)
                {
                    workStationID = row[WST_ID_COL].ToString().Trim();
                    lineID = row[LINE_ID_COL].ToString().Trim();

                    if (GetEmployeeForThisWST(ref emp_ID, ref emp_Name, workStationID))
                    {
                        row[EMPL_ID_COL] = emp_ID;
                        row[EMPL_NAME_COL] = emp_Name;
                    }
                }
            }

            //Run optimize

            return _plan;
        }

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

        private bool GetEmployeeForThisWST(ref string empl_ID, ref string empl_Name, string workStationID)
        {
            if (_plan == null || _empAndSkillList == null || _WstAndSkillList == null || _availableEmpList == null || _availableEmpList.Rows.Count == 0)
            {
                return false;
            }

            if (workStationID == string.Empty)
            {
                return false;
            }

            //Find Skill ID
            List<string> list_SkillID_ForThisWST = GetSkillListForThisWST(workStationID);
            List<string> list_SkillID_OfThisEmp;

            DataTable listOfEmp = GetListOfAvailableEmployee();

            foreach (DataRow item in listOfEmp.Rows)
            {
                string ID = item[EMPL_ID_COL].ToString().Trim();
                string name = item[EMPL_NAME_COL].ToString().Trim();
                list_SkillID_OfThisEmp = GetSkillListOfThisEmpl(ID);

                if (list_SkillID_OfThisEmp == null)
                {
                    continue;
                }

                if (CompareSkill_vs_RequiredSkill(ID, name, list_SkillID_OfThisEmp, list_SkillID_ForThisWST))
                {
                    empl_Name = name;
                    empl_ID = ID;

                    //remove this imployee from the list
                    RemoveEmpFromAvailabelList(empl_ID);

                    return true;
                }
            }

            return true;
        }

        private bool RemoveEmpFromAvailabelList(string empl_ID)
        {
            if (_availableEmpList == null)
            {
                return false;                
            }

            List<DataRow> rowsToDelete = new List<DataRow>();

            foreach (DataRow row in _availableEmpList.Rows)
            {
                if (row[EMPL_ID_COL].ToString().Trim() == empl_ID)
                {
                    rowsToDelete.Add(row);
                }
            }

            foreach (DataRow row in rowsToDelete)
            {
                _availableEmpList.Rows.Remove(row);
            }

            return true;
        }

        private bool GetEmployeeForThisWSTFromPriorityList(ref string empl_ID, ref string empl_Name, string workStationID)
        {
            if (_priorityList == null || _priorityList.Count() == 0)
            {
                return false;
            }

            if (workStationID == string.Empty)
            {
                return false;
            }

            //Check if Wst in the priotiry list
            foreach (var table in _priorityList)
            {
                //DataRow[] results = table.Select("WorkStationID = '" + workStationID + "' AND LineID = '" + lineID + "'");
                DataRow[] results = table.Select(WST_ID_COL + " = '" + workStationID + "'"); 

                if (results.Length > 0)
                {
                    DataRow item = results[0];
                    empl_ID = item[EMPL_ID_COL].ToString().Trim();
                    empl_Name = item[EMPL_NAME_COL].ToString().Trim();

                    //Kiểm tra xem nhân viên có đi làm
                    if (IsEmplGoToWork(empl_ID))
                    {
                        RemoveEmpFromAvailabelList(empl_ID);
                        return true;
                    }
                }                               
            }

            return false;
        }

        private bool IsEmplGoToWork(string empl_ID)
        {
            if (_availableEmpList == null || empl_ID == string.Empty)
            {
                return false;
            }

            DataRow[] results = _availableEmpList.Select(EMPL_ID_COL + " = '" + empl_ID + "'");

            if (results.Length > 0)
            {
                return true;
            }
            
            return false;
        }

        private DataTable GetListOfEmployee()
        {
            if (_empAndSkillList == null)
            {
                return null;
            }

            DataView view = new DataView(_empAndSkillList);
            DataTable distinctValues = view.ToTable(true, EMPL_NAME_COL, EMPL_ID_COL);

            return distinctValues;
        }

        private DataTable GetListOfAvailableEmployee()
        {
            if (_availableEmpList == null)
            {
                return null;
            }

            DataView view = new DataView(_availableEmpList);
            DataTable distinctValues = view.ToTable(true, EMPL_ID_COL, EMPL_NAME_COL);

            return distinctValues;
        }

        private bool ContainsAllItems(List<string> a, List<string> b)
        {
            return !b.Except(a).Any();
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

        public List<string> GetSkillListForThisWST(string wst_ID)
        {
            if (wst_ID == string.Empty)
            {
                return null;
            }

            List<string> lst = new List<string>();
            string skillID;

            DataRow[] results = _WstAndSkillList.Select(WST_ID_COL + " = '" + wst_ID + "'");

            if (results.Length > 0)
            {
                foreach (DataRow item in results)
                {
                    skillID = item[SKILL_ID_COL].ToString();
                    lst.Add(skillID);
                }
            }

            return lst;
        }

        public List<string> GetSkillListOfThisEmpl(string emp_ID)
        {
            if (emp_ID == string.Empty)
            {
                return null;
            }

            List<string> lst = new List<string>();
            string skillID;

            DataRow[] results = _empAndSkillList.Select(EMPL_ID_COL +" = '" + emp_ID + "'");

            if (results.Length > 0)
            {
                foreach (DataRow item in results)
                {
                    skillID = item[SKILL_ID_COL].ToString();
                    lst.Add(skillID);
                }
            }

            return lst;
        }
    }
}