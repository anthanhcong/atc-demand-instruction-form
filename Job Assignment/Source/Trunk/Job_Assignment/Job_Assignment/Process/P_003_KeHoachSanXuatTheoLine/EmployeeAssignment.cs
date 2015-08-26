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

        const string EmpAndSkill_DB_Cmd = "SELECT distinct [Skill_ID],[Empl_ID],[Empl_Name],[GroupID] FROM MDB_002_Empl_Skill";
        const string WstAndSkill_DB_Cmd = "SELECT distinct [WST_ID],[LineID],[Skill_ID],[GroupID]  FROM MDB_004_LineSkillRequest";

        string WST_ID_COL = "WST_ID";
        string LINE_ID_COL = "LineID";
        string EMPL_ID_COL = "Empl_ID";
        string EMPL_NAME_COL = "Empl_Name";
        string SKILL_ID_COL = "Skill_ID";
        string SHIFT_COL = "ShiftName";

        
        private string LoadInternalData()
        {
            string ret;

            if (_empAndSkillList == null)
            {
                ret = dao.OpenDataTable(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, ref _empAndSkillList, CommandType.Text, EmpAndSkill_DB_Cmd);
            }

            if (true)
            {
                ret = dao.OpenDataTable(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, ref _WstAndSkillList, CommandType.Text, WstAndSkill_DB_Cmd);
            }

            return ret;
        }
        
        public EmployeeAssignment()
        {
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
            string shift = "";

            //Search trong danh sách ưu tiên.
            //Yêu cầu: format của những table thuộc danh sách ưu tiên cần có: column "line_ID" WST_ID_COL "Empl_ID"
            foreach (DataRow row in _plan.Rows)
            {
                //vị trí chưa assign --> Assign
                if ((string)row[EMPL_ID_COL].ToString().Trim() == string.Empty)
                {
                    workStationID = row[WST_ID_COL].ToString().Trim();
                    lineID = row[LINE_ID_COL].ToString().Trim();
                    shift = row[SHIFT_COL].ToString().Trim();
                    //if (lineID.Trim() == "HH02")
                    //{
                    //    int a = 0;
                    //}

                    string SelectedTable = string.Empty;
                    if (GetEmployeeForThisWSTFromPriorityList(ref emp_ID, ref emp_Name, ref SelectedTable, shift, workStationID))
                    {
                        row[EMPL_ID_COL] = emp_ID;
                        row[EMPL_NAME_COL] = emp_Name;
                        row["Reason"] = SelectedTable;
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

        public DataTable GetFinalListWithShiftRotation()
        {
            if (_plan == null || _availableEmpList == null)
            {
                return null;
            }

            string workStationID, lineID;
            string emp_ID = "";
            string emp_Name = "";
            string shift = "";

            //Search trong danh sách ưu tiên.
            //Yêu cầu: format của những table thuộc danh sách ưu tiên cần có: column "line_ID" WST_ID_COL "Empl_ID"
            //
            //Input của quá trình search: line_ID, WST_ID
            //Output của quá trình search: Emp_ID (nhân viên được ưu tiên làm cho line, wst này căn cứ vào dữ liệu ưu tiên: lịch sử, fix position...)
            //

            foreach (DataRow row in _plan.Rows)
            {
                //vị trí chưa assign --> Assign
                if ((string)row[EMPL_ID_COL].ToString().Trim() == string.Empty)
                {
                    workStationID = row[WST_ID_COL].ToString().Trim();
                    lineID = row[LINE_ID_COL].ToString().Trim();
                    shift = row[SHIFT_COL].ToString().Trim();

                    //Xoay ca:
                    //Nếu wst đang tìm nhân viên ca 1--> Tìm trong lịch sử ai đã làm ca 3 ở vị trí này --> assign
                    string shiftToFindInHistory = FullRotateShift(shift);
                    string SelectedTable = string.Empty;
                    if (GetEmployeeForThisWSTFromPriorityList(ref emp_ID, ref emp_Name, ref SelectedTable, shiftToFindInHistory, workStationID))
                    {
                        row[EMPL_ID_COL] = emp_ID;
                        row[EMPL_NAME_COL] = emp_Name;
                        row["Reason"] = SelectedTable;
                    }
                }
            }

            //Normal assign dựa trên skill.
            //Sau khi search ưu tiên cho tất cả các wst --> Sẽ có wst có người, tìm được từ việc search ưu tiên
            //Tuy nhiên, có wst chưa có người, cần tìm người cho những wst này trong danh sách nhân viên available còn lại
            //Ai có skill phù hợp --> Assign
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

        public string FullRotateShift (string requiredShift)
	    {
            string shiftToFindInHistory = string.Empty;

            //Use ShiftList below if we want an anticlockwise rotation: 
            //+ Empl from shift 1 this week will be found from  shift 2 last week
            //+ Empl from shift 2 this week will be found from  shift 3 last week
            //+ Empl from shift 3 this week will be found from  shift 1 last week
            //string[] ShiftList = { "Shift_1", "Shift_2", "Shift_3" };

            //Use ShiftList below if we want an clockwise rotation: 
            //+ Empl from shift 1 this week will be found from  shift 3 last week
            //+ Empl from shift 2 this week will be found from  shift 1 last week
            //+ Empl from shift 3 this week will be found from  shift 2 last week
            string[] ShiftList = { "Shift_3", "Shift_2", "Shift_1" };


            for (int i = 0; i < ShiftList.Length; i++)
            {
                if (ShiftList[i] == requiredShift)
                {
                    int nextShiftID = (i+1) % ShiftList.Length;
                    shiftToFindInHistory = ShiftList[nextShiftID];
                    break;
                }
            }

            return shiftToFindInHistory;
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

        private bool GetEmployeeForThisWSTFromPriorityList(ref string empl_ID, ref string empl_Name, ref string tblName, string shift, string workStationID)
        {
            if (_priorityList == null || _priorityList.Count() == 0)
            {
                return false;
            }

            if (workStationID == string.Empty)
            {
                return false;
            }
            if (shift == string.Empty)
            {
                // Add to log
                return false;
            }
             
            //Check if Wst in the priotiry list
            foreach (var table in _priorityList)
            {
                if (table == null || table.Rows.Count == 0)
                {
                    continue;
                }

                //DataRow[] results = table.Select("WorkStationID = '" + workStationID + "' AND LineID = '" + lineID + "'");

                //TODO: Optimize here
                tblName = table.Rows[0]["TableName"].ToString();

                //Todo: Dho-Saperate 2 list of prioriy, one with shift, another without shift. Or check if the shift is null before assign
                DataRow[] results = table.Select(WST_ID_COL + " = '" + workStationID + "' AND " + SHIFT_COL + " = '" + shift + "'");
                
                //Trong priority list, ko support select theo shift
                //DataRow[] results = table.Select(WST_ID_COL + " = '" + workStationID + "'"); 

                if (results.Length > 0)
                {
                    for (int i = 0; i < results.Length; i++)
                    {
                        DataRow item = results[i];
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
            }

            return false;
        }

        private bool IsEmplGoToWork(string empl_ID)
        {
            //Todo: Link with the real API provided by a.Kien
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

        private List<string> GetSkillListForThisWST(string wst_ID)
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

        private List<string> GetSkillListOfThisEmpl(string emp_ID)
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