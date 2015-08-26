using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;

namespace Job_Assignment
{
    class NewEmployeeAssignment
    {
        string Plan_Colum_Line_ID = "LineID";
        string Plan_Colum_WST_ID = "WST_ID";
        string Plan_Colum_Shift = "ShiftName";
        string Plan_Colum_Empl_ID = "Empl_ID";
        string Plan_Colum_Empl_Name = "Empl_Name";

        string PRIORITY_LOG_COLUMN = "Prio";
        string EMPL_LOG_COLUMN = "Empl_ID";
        string WST_LOG_COLUMN = "WST_ID";
        string CHANGED_EMPL_LOG_COLUMN = "Changed_Empl_ID"; //Employee đã bị đổi chỗ công việc để có chỗ sắp cho employee mới
        string CHANGED_WST_LOG_COLUMN = "Changed_WST_ID";   //Wst mới của Employee bị đổi chỗ
        string NOTE_LOG_COLUMN = "Reason";

        public const string SHIFT_1 = "Shift_1";
        public const string SHIFT_2 = "Shift_2";
        public const string SHIFT_3 = "Shift_3";
        public const string SHIFT_UNKNOW = "UnknowShift";

        DataTable _plan; //Plan to arrange employee,
        DataTable _availableEmpList;//Could not be null
        List<EmployeeAssignment_ProrityTable> _priorityList;

        DataTable _empAndSkillList = null;
        DataTable _WstAndSkillList = null;
        DataTable _GroupAndLineList = null;
        DataTable _NonSwapEmplList = null;
        DataTable _ForceList = null;
        

        MSSqlDbFactory dao = new MSSqlDbFactory();

        System.Windows.Forms.ToolStripStatusLabel _StatusLabel1;
        System.Windows.Forms.ToolStripProgressBar _ProgressBar1;

        const string EmpAndSkill_DB_Cmd = "SELECT distinct [Skill_ID],[Empl_ID],[Empl_Name],[GroupID] FROM MDB_002_Empl_Skill";
        const string WstAndSkill_DB_Cmd = "SELECT distinct [WST_ID],[LineID],[Skill_ID]  FROM MDB_004_LineSkillRequest";
        // const string GroupAndLine_DB_Cmd = "SELECT distinct [GroupID],[LineID]  FROM MDB_003_Line_Desciption";
        const string GroupAndLine_DB_Cmd = "SELECT distinct [GroupID],[LineID]  FROM MDB_004_LineSkillRequest";

        string SKILL_ID_COL = "Skill_ID";

        string _LogFileName = string.Empty;

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

            if (_GroupAndLineList == null)
            {
                ret = dao.OpenDataTable(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, ref _GroupAndLineList, CommandType.Text, GroupAndLine_DB_Cmd);
            }

            return true;
        }

        public NewEmployeeAssignment()
        {
            LoadInternalData();
        }

        public NewEmployeeAssignment(DataTable plan,
                                    DataTable availableListOfEmpl, /* The list of all employee of production */
                                    List<EmployeeAssignment_ProrityTable> priorityList,
                                    System.Windows.Forms.ToolStripStatusLabel StatusLabel1,
                                    System.Windows.Forms.ToolStripProgressBar ProgressBar1,
                                    ref string InputCheckingResult)
        {
            _plan = plan;
            _availableEmpList = availableListOfEmpl;
            _priorityList = priorityList;
            _StatusLabel1 = StatusLabel1;
            _ProgressBar1 = ProgressBar1;

            LoadInternalData();

            Log(_availableEmpList, "_availableEmpList", ".csv", true);

            //Log the priority list
            LogInputPriortiyData(_priorityList, "_PrioirtyList", ".csv");

            InputCheckingResult = CheckInputData();

            DataTable test = GetAllPlanDataOfThisLine("HH02");
            DataTable test1 = GetListOfEmptyWstOfThisLine("HH02", "Shift_1");
            DataTable test2 = GetListOfEmptyWstOfThisLine("HH02", "Shift_2");
            DataTable test3 = GetListOfEmptyWstOfThisLine("HH02", "Shift_3");

            DataTable test4 = GetPlanDataOfThisLine("HH02", "Shift_1", WST_State.All);
            DataTable test5 = GetPlanDataOfThisLine("HH02", "Shift_2", WST_State.All);
            DataTable test6 = GetPlanDataOfThisLine("HH02", "Shift_3", WST_State.All);
            //DataTable test4 = GetAllPlanDataOfThisLine("HH02");

            if (test != null && test4 != null && test5 != null && test6 != null)
            {
                if (test.Rows.Count != (test4.Rows.Count + test5.Rows.Count + test6.Rows.Count ))
                {
                    //Log here.
                    int a = 0 ;
                }
            }

            //Nếu có một vài nhân viên đã được manual assign, remove những nhân viên này ra khỏi available list 
            InputCheckingResult += CheckAndRemoveMEmployeeAssignedManually();
        }

        public bool SetNonSwapList(DataTable table)
        {
            if (table != null)
            {
                _NonSwapEmplList = table;
                return true;
            }

            return false;
        }

        public bool SetForceList(DataTable table)
        {
            if (table != null)
            {
                _ForceList = table;
                return true;
            }

            return false;
        }

        
        public bool IsEmployeeCouldbeSwapped(string emplID)
        {
            if (_NonSwapEmplList != null)
            {
                string filter = (ProrityTableCollumn.EMPL_COLUMN + " = '" + emplID.Trim() + "'");
                DataRow[] SearchingResult = _NonSwapEmplList.Select(filter);
                
                if (SearchingResult.Count() > 0)
	            {
            		 return false;
	            }
            }

            return true;
        }


        private string CheckAndRemoveMEmployeeAssignedManually()
        {
            string Result = string.Empty;

            int NumOfEmployee = 0;
            int NumOfEmployeeNeedForPlan = 0;

            if (_availableEmpList != null)
            {
                DataTable EmployeeList = GetListOfAvailableEmployee();
                NumOfEmployee = EmployeeList.Rows.Count;

                if (NumOfEmployee == 0)
                {
                    return "";
                }
            }

            if (_plan != null)
            {
                string EmployeeInChecking = string.Empty;
                int NumberOfEmployeeManuallyAssigned = 0;
                foreach (DataRow row in _plan.Rows)
                {
                    EmployeeInChecking = row[Plan_Colum_Empl_ID].ToString().Trim();
                    if (EmployeeInChecking != string.Empty)
                    {
                        RemoveEmpFromAvailabelList(EmployeeInChecking);
                        NumberOfEmployeeManuallyAssigned++;
                    }
                }

                Result = string.Format("\n\rNumber of WorkStation has been assigned: {0}\r\n", NumberOfEmployeeManuallyAssigned);
            }

            return Result;
        }


        private string CheckInputData()
        {
            int NumOfEmployee = 0;
            int NumOfEmployeeNeedForPlan = 0;

            if (_availableEmpList != null)
            {
                DataTable EmployeeList = GetListOfAvailableEmployee();
                NumOfEmployee = EmployeeList.Rows.Count;

                if (NumOfEmployee == 0)
                {
                    return "No Employee To Assign";
                }
            }

            if (_plan != null)
            {
                NumOfEmployeeNeedForPlan = _plan.Rows.Count;

                if (NumOfEmployeeNeedForPlan == 0)
                {
                    return "No WorkStation To Assign";
                }
            }

            //if (NumOfEmployee < NumOfEmployeeNeedForPlan)
            {
                StringBuilder msg = new StringBuilder();
                //msg.Append("There are more WorkStations than Employees\r\n\r\n");
                //msg.Append("Number of WorkStations and Employees\r\n\r\n");
                msg.AppendFormat("Number of WorkStation in plan: {0}\r\n", NumOfEmployeeNeedForPlan);
                msg.AppendFormat("Number of Available Employee: {0}\r\n", NumOfEmployee);
                return msg.ToString();
            }

            //return string.Empty;
        }


        private bool LogInputPriortiyData(List<EmployeeAssignment_ProrityTable> priorityList, string fileName, string fileType)
        {
            StringBuilder logData = new StringBuilder();

            foreach (var priorityTable in priorityList)
            {
                if (priorityTable == null || priorityTable.Data.Rows.Count == 0)
                {
                    continue;
                }

                if (logData.Length == 0)
                {
                    logData.Append(priorityTable.Data.ToCSV_WithColumnName());
                }
                else
                {
                    logData.Append(priorityTable.Data.ToCSV_WithoutColumnName());
                }
            }

            if (logData.Length > 0)
            {
                CreateAndWriteLogFile(logData.ToString(), fileName, fileType, false);
            }

            return true;
        }

        public DataTable GetFinalList()
        {
            int i = 0, total = 0;
            if (_plan == null || _availableEmpList == null)
            {
                return null;
            }

            // _LogFileName = EmployeeAssignmentReport.CreateLogFile();
            DataTable dtAssigmentOperationLog = CreateLogTable();

            //lần lượt tìm kiếm trong các danh sách, từ mức ưu tiên cao nhất.
            foreach (var priorityTable in _priorityList)
            {
                if (priorityTable == null || priorityTable.Data.Rows.Count == 0)
                {
                    continue;
                }

                _StatusLabel1.Text = "Asigned with: " + priorityTable.Data.TableName;
                total = priorityTable.Data.Rows.Count;
                i = 0;

                foreach (DataRow row in priorityTable.Data.Rows)
                {
                    //lấy từng vị trí trong danh sách ưu tiên,...nếu vị trí đó hôm nay có plan chạy và chưa có ai được sắp vị trí của plan 
                    //==> sắp vào

                    string groupID_InChecking = row[ProrityTableCollumn.GROUP_COLUMN].ToString().Trim();
                    string lineID_InChecking = row[ProrityTableCollumn.LINE_COLUMN].ToString().Trim();
                    string emp_ID_InChecking = row[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                    string wst_ID_InChecking = row[ProrityTableCollumn.WST_COLUMN].ToString().Trim();
                    string shift_InChecking = row[ProrityTableCollumn.SHIFT_COLUMN].ToString().Trim();

                    string changedEmployee = "";
                    string changedWst = "";

                    bool stand = row[ProrityTableCollumn.IS_STAND].ToString().Trim() == "" ? false : (bool)row[ProrityTableCollumn.IS_STAND];
                    bool forceGotoLine = row[ProrityTableCollumn.IS_FORCE_GOTO_LINE].ToString().Trim() == "" ? false : (bool)row[ProrityTableCollumn.IS_FORCE_GOTO_LINE];

                    bool check_skill = row[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN].ToString().Trim() == "" ? false : (bool)row[ProrityTableCollumn.IS_CHECK_SKILL_COLUMN];
                    bool check_wst = row[ProrityTableCollumn.IS_CHECK_WST_COLUMN].ToString().Trim() == "" ? false : (bool)row[ProrityTableCollumn.IS_CHECK_WST_COLUMN];
                    bool check_line = row[ProrityTableCollumn.IS_CHECK_LINE].ToString().Trim() == "" ? false : (bool)row[ProrityTableCollumn.IS_CHECK_LINE];
                    bool check_Group = row[ProrityTableCollumn.IS_CHECK_GROUP_COLUMN].ToString().Trim() == "" ? false : (bool)row[ProrityTableCollumn.IS_CHECK_GROUP_COLUMN];
                    bool check_all = row[ProrityTableCollumn.IS_CHECK_ALL_COLUMN].ToString().Trim() == "" ? false : (bool)row[ProrityTableCollumn.IS_CHECK_ALL_COLUMN];
                    bool swap = row[ProrityTableCollumn.IS_SWAP_COLUMN].ToString().Trim() == "" ? false : (bool)row[ProrityTableCollumn.IS_SWAP_COLUMN];


                    bool optimize_shift = row[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN].ToString().Trim() == "" ? false : (bool)row[ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN];

                    if ((priorityTable.ID == "UT_01_TS_1"))
                    {
                        int a = 0;
                    }

                    if ((emp_ID_InChecking == "15001170") && (priorityTable.ID == "UT_03"))
                    {
                        int a = 0;
                    }

                    if ((emp_ID_InChecking == "15001170") && (priorityTable.ID == "UT_03"))
                    {
                        int a = 0;
                    }


                    DataRow plan = null;

                    if (emp_ID_InChecking == string.Empty)
                    {
                        continue;
                    }

                    if (IsEmployeeInAvailabelList(emp_ID_InChecking) == false)
                    {
                        //Employee đã có việc
                        continue;
                    }

                    DataRow logRecord = dtAssigmentOperationLog.NewRow();
                    

                    logRecord[PRIORITY_LOG_COLUMN] = priorityTable.ID;
                    logRecord[EMPL_LOG_COLUMN] = emp_ID_InChecking;

                    #region old codes
                    /*
                    if (forceGotoLine == true)
                    {
                        string AssignedWST = string.Empty;
                        string AssignedLine = string.Empty;

                        if (TryToAssignEmplToLine(Emp_ID_InChecking, LineID_InChecking, Shift_InChecking, ref AssignedWST, ref AssignedLine))
                        { 
                            //Log here.
                            int b = 0;
                        }
                    }
                    else if (Wst_ID_InChecking != string.Empty)
                    {
                        //Các trường hợp rơi vào danh sách này: 
                        //Fix position cho lead, Fix position ngày đầu tuần, history + ca
                        //
                        plan = HasPlanForThisWST(Wst_ID_InChecking, Shift_InChecking, stand, optimize_shift);
                    }
                    else
                    {
                        //Sắp xếp dựa trên skill và một số yếu tố khác
                        if (check_Group == true)
                        {
                            //Sắp dựa trên profie (Skill, Group ID) + Lich su ca
                            plan = HaveJobForThisEmployee_Profile(Emp_ID_InChecking,
                                                                    GroupID_InChecking,
                                                                    Shift_InChecking,
                                                                    stand,
                                                                    optimize_shift);
                        }
                        else if (check_skill == true)
                        {
                            //Sắp dựa trên history
                            plan = HaveJobForThisEmployee_History(Emp_ID_InChecking,
                                                                    Shift_InChecking,
                                                                    LineID_InChecking,
                                                                    check_line,
                                                                    stand,
                                                                    optimize_shift,
                                                                    ref ChangedEmployee,
                                                                    ref ChangedWst, priorityTable.ID);
                        }
                    }
                    */
                    #endregion 

                    if (emp_ID_InChecking == "20120689")
                    {
                        int a = 0;
                    }

                    string ExtraReason = string.Empty;

                    plan = Find_WST_FOR_EMPL(emp_ID_InChecking, shift_InChecking, groupID_InChecking, lineID_InChecking, wst_ID_InChecking,
                                            check_skill, check_wst, check_line, check_Group, check_all, swap, stand, optimize_shift, ref changedEmployee, ref changedWst,
                                            priorityTable.ID, ref ExtraReason );
                    

                    if (plan != null)
                    {
                        plan[Plan_Colum_Empl_ID] = emp_ID_InChecking;
                        plan[Plan_Colum_Empl_Name] = GetEmplName(emp_ID_InChecking);

                        //Cập nhật thêm thông tin lí do select
                        
                        plan["Reason"] += priorityTable.Name;
                        plan["Reason"] +=  ", " + ExtraReason;
                        RemoveEmpFromAvailabelList(emp_ID_InChecking);

                        logRecord[CHANGED_WST_LOG_COLUMN] = changedWst;
                        logRecord[CHANGED_EMPL_LOG_COLUMN] = changedEmployee;
                        logRecord[WST_LOG_COLUMN] = plan[Plan_Colum_WST_ID];
                        if (emp_ID_InChecking == "20120689")
                        {
                            int a = 0;
                        }
                    }
                    else
                    {
                        logRecord[NOTE_LOG_COLUMN] = "Fail To Assign" + "," + ExtraReason;
                    }

                    dtAssigmentOperationLog.Rows.Add(logRecord);

                    _ProgressBar1.Value = i * 100 / total;
                    i++;
                }
            }

            //Sau khi hoàn tất tìm kiếm tất cả các thông tin input để sắp xếp,
            //Có thể tiến hành chạy optimize nếu thấy cần thiết (employee chưa có việc làm..., wst còn thiếu người)

            //Log the operation of engine
            Log(dtAssigmentOperationLog, "_AssignmentOperation", ".csv", true);

            //Log Assignment result
            LogAssignmentResult(_plan);

            //Log remaining list and skill 
            LogRemainEmployeeWithSkill(_availableEmpList);

            return _plan;
        }


        private DataTable CreateLogTable()
        {
            DataTable tbl = new DataTable();

            tbl.Columns.Add(PRIORITY_LOG_COLUMN, typeof(string));

            //Employee in assignment
            tbl.Columns.Add(EMPL_LOG_COLUMN, typeof(string));
            tbl.Columns.Add(WST_LOG_COLUMN, typeof(string));

            //Employee changed to support assignment
            tbl.Columns.Add(CHANGED_EMPL_LOG_COLUMN, typeof(string));
            tbl.Columns.Add(CHANGED_WST_LOG_COLUMN, typeof(string));

            tbl.Columns.Add(NOTE_LOG_COLUMN, typeof(string));

            return tbl;
        }

        private void LogAssignmentResult(DataTable data)
        {
            Log(data, "_Result", ".csv", true);
        }

        private bool Log(DataTable data, string fileName, string fileType, bool withColumnName)
        {
            if (data == null || fileName == string.Empty || fileType == string.Empty)
            {
                return false;
            }

            StringBuilder logDataResult = new StringBuilder();

            if (withColumnName)
            {
                logDataResult.Append(data.ToCSV_WithColumnName());
            }
            else
            {
                logDataResult.Append(data.ToCSV_WithoutColumnName());
            }

            string LogFileName = EmployeeAssignmentReport.CreateLogFile(fileName, false);

            if (LogFileName != string.Empty)
            {
                EmployeeAssignmentReport.Log(LogFileName, fileType, logDataResult.ToString());
            }

            return true;
        }


        private void LogRemainEmployeeWithSkill(DataTable RemainingEmpList)
        {
            if (RemainingEmpList != null || RemainingEmpList.Rows.Count > 0)
            {
                RemainingEmpList.Columns.Add("Skill", typeof(string));

                //search and find the skill for each employee
                foreach (DataRow row in RemainingEmpList.Rows)
                {
                    string EmplID = row[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim();
                    string Skills = string.Empty;

                    if (EmplID != string.Empty)
                    {
                        Skills = GetSkillListOfThisEmpl(EmplID, "-");
                    }

                    row["Skill"] = Skills;
                }

                //Log to file
                Log(RemainingEmpList, "_RemainingEmployeeWithSkill", ".csv", true);
            }
        }

        private List<string> GetLinesInGroup(string GroupID)
        {
            if (_GroupAndLineList == null)
            {
                return null;
            }

            string GroupID_Column = "GroupID";
            string LineID_Column = "LineID";
            DataRow[] LineIDs = _GroupAndLineList.Select(GroupID_Column + " = '" + GroupID + "'");

            List<string> result = new List<string>();

            foreach (var item in LineIDs)
            {
                result.Add(item[LineID_Column].ToString().Trim());
            }

            return result;
        }

        private DataRow HaveJobForThisEmployee_Profile(string Emp_ID_InChecking, string GroupID_InChecking, string shift, bool stand, bool optimize_shift)
        {
            string filter = "";
            if (_plan == null || _plan.Rows.Count == 0)
            {
                return null;
            }

            //Tìm tất cả các công việc thuộc GroupID_InChecking
            List<string> LinesInGroup = GetLinesInGroup(GroupID_InChecking);

            //Kiểm tra employee này có phù hợp với công việc nào trong danh sách công việc tìm được
            foreach (var line in LinesInGroup)
            {
                if (line != string.Empty)
                {
                    //Tìm những việc chưa được assign liên quan đến line
                    if (optimize_shift)
                    {
                        switch (shift)
                        {
                            case SHIFT_UNKNOW:
                            case "":
                                filter = Plan_Colum_Shift + " = '" + SHIFT_1 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_2 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_3 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                                break;
                            case SHIFT_1:
                                filter = Plan_Colum_Shift + " = '" + SHIFT_1 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_2 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                                break;
                            case SHIFT_2:
                                filter = Plan_Colum_Shift + " = '" + SHIFT_1 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_2 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_3 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                                break;
                            case SHIFT_3:
                                filter = Plan_Colum_Shift + " = '" + SHIFT_3 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_2 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                                break;
                        }

                    }
                    else
                    {
                        switch (shift)
                        {
                            case SHIFT_1:
                                filter = Plan_Colum_Shift + " = '" + SHIFT_1 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                                break;
                            case SHIFT_2:
                                filter = Plan_Colum_Shift + " = '" + SHIFT_2 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                                break;
                            case SHIFT_3:
                                filter = Plan_Colum_Shift + " = '" + SHIFT_3 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                                break;
                            default:
                                return null;
                        }
                    }

                    if (stand == false)
                    {
                        if (filter == "")
                        {
                            filter = " LineID not like '%STAND%'";
                        }
                        else
                        {
                            filter = "(" + filter + ") AND LineID not like '%STAND%'";
                        }
                    }

                    DataRow[] SearchingPlan = _plan.Select(Plan_Colum_Line_ID + " = '" + line + "' AND (" + filter + ")");

                    foreach (var plan in SearchingPlan)
                    {
                        if (plan[Plan_Colum_Empl_ID].ToString().Trim() == string.Empty)
                        {
                            string WST_ID = plan[Plan_Colum_WST_ID].ToString().Trim();

                            //Vị trí chưa có người trên line
                            if (IsEmplHaveEnoughSkill(Emp_ID_InChecking, WST_ID))
                            {
                                return plan;
                            }
                        }
                    }
                }
            }

            return null;
        }

        private DataRow HaveJobForThisEmployee_History(string Emp_ID_InChecking,
                                                        string shift,
                                                        string line,
                                                        bool checkline,
                                                        bool stand,
                                                        bool optimize_shift,
                                                        ref string changedEmpl,
                                                        ref string changedWst, string table_id
                                                    )
        {
            string filter = "";
            if (_plan == null || _plan.Rows.Count == 0)
            {
                return null;
            }

            //Tìm những việc chưa được assign liên quan đến Shift_InChecking 
            if (optimize_shift)
            {
                switch (shift)
                {
                    case SHIFT_UNKNOW:
                    case "":
                        filter = Plan_Colum_Shift + " = '" + SHIFT_1 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_2 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_3 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                        break;
                    case SHIFT_1:
                        filter = Plan_Colum_Shift + " = '" + SHIFT_1 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_2 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                        break;
                    case SHIFT_2:
                        filter = Plan_Colum_Shift + " = '" + SHIFT_1 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_2 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_3 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                        break;
                    case SHIFT_3:
                        filter = Plan_Colum_Shift + " = '" + SHIFT_3 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_2 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                        break;
                }

            }
            else
            {
                switch (shift)
                {
                    case SHIFT_1:
                        filter = Plan_Colum_Shift + " = '" + SHIFT_1 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                        break;
                    case SHIFT_2:
                        filter = Plan_Colum_Shift + " = '" + SHIFT_2 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                        break;
                    case SHIFT_3:
                        filter = Plan_Colum_Shift + " = '" + SHIFT_3 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                        break;
                    default:
                        return null;
                }
            }

            if (checkline == true)
            {
                filter = "(" + filter + ") AND LineID = '" + line + "'";
            }

            if (stand == false)
            {
                filter = "(" + filter + ") AND LineID not like '%STAND%'";
            }

            DataRow[] SearchingPlan = _plan.Select(filter);

            foreach (var plan in SearchingPlan)
            {
                if (plan[Plan_Colum_Empl_ID].ToString().Trim() == string.Empty)
                {
                    string WST_ID = plan[Plan_Colum_WST_ID].ToString().Trim();

                    //Vị trí chưa có người trên line
                    if (IsEmplHaveEnoughSkill(Emp_ID_InChecking, WST_ID))
                    {
                        return plan;
                    }
                }
            }

            if (table_id == "UT_03.1")
            {
                // Hoán người trong line
                //DataRow[] temp_tbl;
                //SearchingPlan.CopyTo(temp_tbl, 0);
                int i = 0, j = 0;
                int total = SearchingPlan.Count();
                int remove_index = -1, new_index = -1;

                for (i = 0; i < total; i++)
                {
                    string cur_wst = SearchingPlan[i][Plan_Colum_WST_ID].ToString().Trim();
                    string cur_empl = SearchingPlan[i][Plan_Colum_Empl_ID].ToString().Trim();
                    if (cur_empl != string.Empty) //vị trí hiện tại trong line chưa có người
                    {
                        if (IsEmplHaveEnoughSkill(Emp_ID_InChecking, cur_wst))
                        {
                            for (j = 0; j < total; j++)
                            {
                                if (SearchingPlan[j][Plan_Colum_Empl_ID].ToString().Trim() == string.Empty)
                                {
                                    string WST_ID = SearchingPlan[j][Plan_Colum_WST_ID].ToString().Trim();
                                    if (IsEmplHaveEnoughSkill(cur_empl, WST_ID))
                                    {
                                        remove_index = i;
                                        new_index = j;
                                        SearchingPlan[new_index][Plan_Colum_Empl_ID] = SearchingPlan[remove_index][Plan_Colum_Empl_ID];
                                        SearchingPlan[new_index][Plan_Colum_Empl_Name] = SearchingPlan[remove_index][Plan_Colum_Empl_Name];

                                        changedEmpl = SearchingPlan[remove_index][Plan_Colum_Empl_ID].ToString();
                                        changedWst = WST_ID;

                                        SearchingPlan[new_index]["Reason"] = "Ưu tiên 03_1. Replace for: " + Emp_ID_InChecking;
                                        return SearchingPlan[remove_index];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        enum WST_State
        {
            Empty,
            HasEmployee,
            All
        }

        private DataTable GetPlanDataOfThisLine(string LineID, string shift, WST_State wstState)
        {
            if (LineID == string.Empty || _plan == null)
            {
                return null;
            }

            DataTable planForThisLine = _plan.Clone();

            string filter = "";

            string line = LineID;

            filter = Plan_Colum_Line_ID + " = '" + line + "' ";

            if (shift != string.Empty)
            {
                filter += "AND " + Plan_Colum_Shift + " = '" + shift + "'";
            }



            DataRow[] plans = _plan.Select(filter);

            foreach (var item in plans)
            {
                string wst = item[Plan_Colum_WST_ID].ToString().Trim();

                if ((wstState == WST_State.Empty && wst == string.Empty) ||
                    (wstState == WST_State.HasEmployee && wst != string.Empty) ||
                    (wstState == WST_State.All))
                {
                    planForThisLine.Rows.Add(item.ItemArray);
                }
            }
            return planForThisLine;
        }

        //lấy thông tin toàn bộ những wst có plan chạy 
        private DataTable GetListOfEmptyWstOfThisLine(string LineID, string shift)
        {
            DataTable plan = GetPlanDataOfThisLine(LineID, shift, WST_State.Empty);

            return plan;
        }

        private DataTable GetAllPlanDataOfThisLine(string LineID)
        {
            return GetPlanDataOfThisLine(LineID, "", WST_State.All);
        }

        private DataTable GetAllPlanDataOfThisLine(string LineID, string Shift)
        {
            return GetPlanDataOfThisLine(LineID, Shift, WST_State.All);
        }

        private DataTable FindSlotForThisEmployee(string Empl_Id, DataTable plan)
        {
            if (Empl_Id == string.Empty || plan == null || plan.Rows.Count == 0)
            {
                return null;
            }

            DataTable result = new DataTable();
            result = plan.Clone();

            foreach (DataRow row in plan.Rows)
            {
                string wstID = row[Plan_Colum_WST_ID].ToString().Trim();

                if (IsEmplHaveEnoughSkill(Empl_Id, wstID))
                {
                    //vị trí nhân viên này có thể vào được
                    result.Rows.Add(row.ItemArray);
                }
            }

            return result;
        }

        private bool TryToAssignEmplToLine(string Empl_ID, string LineID, string Shift, 
                                            ref string returnedWST_ID, //vị trí wst tìm được cho employee
                                            ref string returnedLineID) //vị trí line tìm được cho employee
        {
            string line;

            returnedWST_ID = string.Empty;
            returnedLineID = string.Empty;

            if (Empl_ID == string.Empty || LineID == string.Empty)
            {
                return false;
            }

            line = LineID;

            DataTable planForThisLine = GetAllPlanDataOfThisLine(line, Shift);

            DataTable remainEmptyWst = GetListOfEmptyWstOfThisLine(line,Shift);
            DataTable SuitableSlot = FindSlotForThisEmployee(Empl_ID, planForThisLine);

            if (planForThisLine == null || planForThisLine.Rows.Count == 0 ||
                remainEmptyWst == null || remainEmptyWst.Rows.Count == 0 ||
                remainEmptyWst == null || remainEmptyWst.Rows.Count == 0)
            {
                //Ko có plan cho line hoặc line đã fill đủ người, hoặc ko tìm được chỗ phù hợp cho employee này
                return false;
            }
            

            //Level 1: Cố gắng tìm vị trí wst trống để gán,
            foreach (DataRow row in SuitableSlot.Rows)
	        {
                string CurrentEmployeeAtThisSlot = row[Plan_Colum_Empl_ID].ToString().Trim();

        		if (CurrentEmployeeAtThisSlot == string.Empty)
                {   
                    returnedWST_ID = row[Plan_Colum_WST_ID].ToString().Trim();
                    returnedLineID = row[Plan_Colum_Line_ID].ToString().Trim();


                    //Lấy chỗ vừa kiếm được cho nhân viên đang kiểm tra
                    returnedWST_ID = row[Plan_Colum_WST_ID].ToString().Trim();
                    returnedLineID = row[Plan_Colum_Line_ID].ToString().Trim();

                    //Apply nhân viên vào vị trí mới
                    string OldEmployee = string.Empty;
                    AssignEmployeeToPlan(Empl_ID, returnedWST_ID, returnedLineID, Shift, ref OldEmployee);
                    return true;
                }
	        }

            //Level 1: Cho phép hoán một người đã có việc. Người có việc sẽ di chuyển vào một vị trí còn trống
            foreach (DataRow row in SuitableSlot.Rows)
            {
                string CurrentEmployeeAtThisSlot = row[Plan_Colum_Empl_ID].ToString().Trim();

                if (CurrentEmployeeAtThisSlot != string.Empty)
                {
                    //Tìm chỗ mới cho nhân viên
                    DataTable result = FindSlotForThisEmployee(CurrentEmployeeAtThisSlot, remainEmptyWst);

                    if (result != null && result.Rows.Count > 0)
                    {
                        //Move nhân viên hiện tại trên line vào chỗ mới,
                        string newWST = result.Rows[0][Plan_Colum_WST_ID].ToString();

                        string OldEmployee = string.Empty;
                        AssignEmployeeToPlan(CurrentEmployeeAtThisSlot, newWST, line, Shift, ref OldEmployee);

                        //Lấy chỗ vừa kiếm được cho nhân viên đang kiểm tra
                        returnedWST_ID = row[Plan_Colum_WST_ID].ToString().Trim();
                        returnedLineID = row[Plan_Colum_Line_ID].ToString().Trim();

                        //Apply nhân viên vào vị trí mới
                        AssignEmployeeToPlan(Empl_ID, returnedWST_ID, returnedLineID, Shift, ref OldEmployee);
                        return true;
                    }

                }
            }

            return false;
        }

        private bool AssignEmployeeToPlan(  string Empl,
                                            string WST_ID_ToAssign, 
                                            string LineID_ToAssign, 
                                            string Shift, 
                                            ref string Old_EmployeeID)
        { 
            string filter = string.Empty;
            Old_EmployeeID = string.Empty;

            filter +=          Plan_Colum_WST_ID + " = '" + WST_ID_ToAssign + "'";
            filter += "AND " + Plan_Colum_Line_ID + " = '" + LineID_ToAssign + "'";
            filter += "AND " + Plan_Colum_Shift   + " = '" + Shift + "'";

            DataRow[] SearchingPlan = _plan.Select(filter);

            if (SearchingPlan != null &&  SearchingPlan.Length > 0)
            {
                //return nhân viên hiện tại nếu có
                Old_EmployeeID = SearchingPlan[0][Plan_Colum_Empl_ID].ToString();

                //Gán nhân viên 
                SearchingPlan[0][Plan_Colum_Empl_ID] = Empl;
                SearchingPlan[0][Plan_Colum_Empl_Name] = GetEmplName(Empl);
                return true;
            }

            return false;
        }

        private bool TryToAssignEmplToGroup(string Empl_ID, string GroupID, ref string LineID, ref string WST_ID)
        {
            //Tìm line thuộc group, 

            //Lần lượt tìm các vị trí trống trên line, vị trí nào phù hợp với employee này, 

            //Nếu vị trí hiện tại đã có người, có thể chuyển người này vào một vị trí trống nào khác ko
            //If yes, assign,
            //Thuật toán hoán trên line nên support case: 
            // + chỉ hỗ trợ hoán 1 người 
            // + hỗ trợ hoán nhiều người để đạt mục tiêu

            return true;
        }



        private bool CheckAndLog(StringBuilder builder, string[] data)
        {
            bool LogEnabled = true;
            string SAPERATOR = ",";

            if (builder == null || data == null || data.Length == 0)
            {
                return false;
            }

            if (LogEnabled)
            {
                foreach (var item in data)
                {
                    builder.Append(item.ToString());
                    builder.Append(SAPERATOR);
                }
            }

            return true;

        }

        private bool CheckAndLog(StringBuilder builder, string data)
        {
            bool LogEnabled = true;

            if (builder == null || data == string.Empty)
            {
                return false;
            }

            if (LogEnabled)
            {
                builder.Append(data);
            }

            return true;
        }

        private bool CreateAndWriteLogFile(string logData, string fileName, string fileType, bool includeHeader)
        {
            if (fileName == string.Empty || fileType == string.Empty)
            {
                return false;
            }

            string LogFileName = EmployeeAssignmentReport.CreateLogFile(fileName, includeHeader);

            if (LogFileName != string.Empty)
            {
                EmployeeAssignmentReport.Log(LogFileName, fileType, logData);
                return true;
            }

            return false;
        }

        //Check if wst have job in this shift, if yes, return the row in the table
        private DataRow HasPlanForThisWST(string WST_ID, string Shift, bool stand, bool optimize_shift)
        {
            string filter = "";
            if (_plan == null || _plan.Rows.Count == 0)
            {
                return null;
            }


            if (optimize_shift)
            {
                switch (Shift)
                {
                    case SHIFT_UNKNOW:
                    case "":
                        filter = Plan_Colum_Shift + " = '" + SHIFT_1 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_2 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_3 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                        break;
                    case SHIFT_1:
                        filter = Plan_Colum_Shift + " = '" + SHIFT_1 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_2 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                        break;
                    case SHIFT_2:
                        filter = Plan_Colum_Shift + " = '" + SHIFT_1 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_2 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_3 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                        break;
                    case SHIFT_3:
                        filter = Plan_Colum_Shift + " = '" + SHIFT_3 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_2 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                        break;
                }

            }
            else
            {
                switch (Shift)
                {
                    case SHIFT_1:
                        filter = Plan_Colum_Shift + " = '" + SHIFT_1 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                        break;
                    case SHIFT_2:
                        filter = Plan_Colum_Shift + " = '" + SHIFT_2 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                        break;
                    case SHIFT_3:
                        filter = Plan_Colum_Shift + " = '" + SHIFT_3 + "' OR " + Plan_Colum_Shift + " = '" + SHIFT_UNKNOW + "'";
                        break;
                    default:
                        return null;
                }
            }

            if (stand == false)
            {
                filter = "(" + filter + ") AND LineID not like '%STAND%'";
            }

            DataRow[] Plan_Results = _plan.Select(Plan_Colum_WST_ID + " = '" + WST_ID + "' AND (" + filter + ")");

            if (Plan_Results.Length > 0)
            {
                if (Plan_Results.Length > 1)
                {
                    //Debug, Checking. Should not be here ???
                    //Wrong. Expected = 1 vì wst là duy nhất ???
                }

                DataRow plan = Plan_Results[0];

                string CurrentPlan_Empl_ID = plan[Plan_Colum_Empl_ID].ToString().Trim();
                string CurrentPlan_Empl_Name = plan[Plan_Colum_Empl_Name].ToString().Trim();

                if (CurrentPlan_Empl_ID == string.Empty)
                {
                    return plan;
                }
            }

            return null;
        }

        private string GetEmplName(string EmployeeID)
        {
            //Todo: Link with the real API provided by a.Kien
            if (_availableEmpList == null || EmployeeID == string.Empty)
            {
                return string.Empty;
            }

            DataRow[] results = _availableEmpList.Select(ProrityTableCollumn.EMPL_COLUMN + " = '" + EmployeeID + "'");

            if (results.Length > 0)
            {
                return results[0][ProrityTableCollumn.EMPL_NAME_COLUMN].ToString().Trim();
            }

            return string.Empty;
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

        private bool RemoveEmpFromAvailabelList(string empl_ID)
        {
            if (_availableEmpList == null)
            {
                return false;
            }

            List<DataRow> rowsToDelete = new List<DataRow>();

            foreach (DataRow row in _availableEmpList.Rows)
            {
                if (row[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim() == empl_ID)
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

        private bool IsEmployeeInAvailabelList(string empl_ID)
        {
            if (_availableEmpList == null)
            {
                return false;
            }

            List<DataRow> SearchingResult = new List<DataRow>();

            foreach (DataRow row in _availableEmpList.Rows)
            {
                if (row[ProrityTableCollumn.EMPL_COLUMN].ToString().Trim() == empl_ID.Trim())
                {
                    SearchingResult.Add(row);
                }
            }

            if (SearchingResult != null && SearchingResult.Count > 0)
            {
                return true;
            }

            //Employee is not available
            return false;
        }

        private DataTable GetListOfEmployee()
        {
            if (_empAndSkillList == null)
            {
                return null;
            }

            DataView view = new DataView(_empAndSkillList);
            DataTable distinctValues = view.ToTable(true, ProrityTableCollumn.EMPL_NAME_COLUMN, ProrityTableCollumn.EMPL_COLUMN);

            return distinctValues;
        }

        private DataTable GetListOfAvailableEmployee()
        {
            if (_availableEmpList == null)
            {
                return null;
            }

            DataView view = new DataView(_availableEmpList);
            DataTable distinctValues = view.ToTable(true, ProrityTableCollumn.EMPL_COLUMN, ProrityTableCollumn.EMPL_NAME_COLUMN);

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
            else
            {
                int a = 0; //Dummy for debug
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

        private string GetStringOfSkillListForThisWST(string wst_ID)
        {
            string SkillString = string.Empty;

            if (wst_ID == string.Empty)
            {
                return SkillString;
            }

            string skillID;

            DataRow[] results = _WstAndSkillList.Select(ProrityTableCollumn.WST_COLUMN + " = '" + wst_ID + "'");

            if (results.Length > 0)
            {
                foreach (DataRow item in results)
                {
                    skillID = item[SKILL_ID_COL].ToString().Trim();
                    SkillString += skillID;
                    SkillString += ",";
                }
            }

            return SkillString;
        }

        private string GetStringOfSkillListOfThisEmpl(string emp_ID)
        {
            string SkillString = string.Empty;

            if (emp_ID == string.Empty)
            {
                return SkillString;
            }

            string skillID;

            DataRow[] results = _empAndSkillList.Select(ProrityTableCollumn.EMPL_COLUMN + " = '" + emp_ID + "'");

            if (results.Length > 0)
            {
                foreach (DataRow item in results)
                {
                    skillID = item[SKILL_ID_COL].ToString().Trim();
                    SkillString += skillID;
                    SkillString += ",";
                }
            }

            return SkillString;
        }

        private string GetSkillListOfThisEmpl(string emp_ID, string skillSeperatorChar)
        {
            if (emp_ID == string.Empty)
            {
                return null;
            }

            StringBuilder skills = new StringBuilder();
            string skillID;

            DataRow[] results = _empAndSkillList.Select(ProrityTableCollumn.EMPL_COLUMN + " = '" + emp_ID + "'");

            if (results.Length > 0)
            {
                foreach (DataRow item in results)
                {

                    skillID = item[SKILL_ID_COL].ToString().Trim();
                    skills.Append(skillID);
                    skills.Append(skillSeperatorChar);
                }
            }

            return skills.ToString();
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

        private DataRow Find_WST_FOR_EMPL(string Emp_ID_InChecking,
                                            string shift, string group, string line, string wst, 
                                            bool check_skill, bool check_wst, bool check_line, bool check_group, bool check_all, 
                                            bool swap, bool stand, bool optimizie_shift,
                                            ref string changedEmpl,
                                            ref string changedWst, string table_id,
                                            ref string ExtraReason)
        {
            string cur_empl_id;
            string filter = "";
             ExtraReason = "";
            if (_plan == null || _plan.Rows.Count == 0)
            {
                return null;
            }


            //Check nếu empl này trong list force shift, line 
            if (_ForceList != null)
            {
                string forceListFilter = string.Format("Empl_ID = '{0}'", Emp_ID_InChecking.Trim());
                DataRow[] forcelistFiltered = _ForceList.Select(forceListFilter);

                if (forcelistFiltered.Count() > 0)
	            {
                    string forceShift = forcelistFiltered[0]["ForceShift"].ToString().Trim();
                    string forceWST = forcelistFiltered[0]["ForceWST"].ToString().Trim();
            		
                    //case 1: Force shift & wst
                    if (forceShift != string.Empty && forceWST != string.Empty)
	                {
                        filter = string.Format("{0} = '{1}' AND {2} = '{3}'",Plan_Colum_Shift,forceShift,Plan_Colum_WST_ID, forceWST);

                        DataRow[] SearchingPlanForForceList = _plan.Select(filter);

                        foreach (var plan in SearchingPlanForForceList)
                        {
                            cur_empl_id = plan[Plan_Colum_Empl_ID].ToString().Trim();

                            if (cur_empl_id == string.Empty)
                            {
                                string WST_ID = plan[Plan_Colum_WST_ID].ToString().Trim();

                                //Vị trí chưa có người trên line
                                if (IsEmplHaveEnoughSkill(Emp_ID_InChecking, WST_ID))
                                {
                                    ExtraReason = "Force WST and Shift";
                                    return plan;
                                }
                                else
                                {
                                    //Check taị sao gán cho ts vị trí ko đủ skill
                                    int b = 0;
                                    ExtraReason = "Not enought Skill for: " + WST_ID;
                                }
                            }
                        }

                        return null;
	                }
                    else if (forceShift != string.Empty && forceWST == string.Empty)
                    {
                        //Todo: Test again this case, seem i miss this case in release ForMonday 0.01

                        //Chỉ force shift
                        shift = SHIFT_1;
                        optimizie_shift = false;

                        ExtraReason = "Force shift";
                        //Add extra reason
                    }
	            }
            }

            /*********************************************************************/
            // Chạy Optimize lần 1: 
            //     3'--> 2'
            /*********************************************************************/
            filter = "";

            //Tìm những việc chưa được assign liên quan đến Shift_InChecking 
            if (optimizie_shift == false)
            {
                switch (shift)
                {
                    case SHIFT_1:
                        filter = "(" + Plan_Colum_Shift + " = '" + SHIFT_1 + "')"; ;
                        break;
                    case SHIFT_2:
                        filter = "(" + Plan_Colum_Shift + " = '" + SHIFT_2 + "')";
                        break;
                    case SHIFT_3:
                        filter = "(" + Plan_Colum_Shift + " = '" + SHIFT_3 + "')";
                        break;
                    default:
                        return null;
                }
            }
            else
            {
                switch (shift)
                {
                    case SHIFT_1:
                        filter = "(" + Plan_Colum_Shift + " = '" + SHIFT_1 + "')"; ;
                        break;
                    case SHIFT_2:
                        filter = "(" + Plan_Colum_Shift + " = '" + SHIFT_2 + "')";
                        break;
                    case SHIFT_3:
                        filter = "(" + Plan_Colum_Shift + " = '" + SHIFT_2 + "')";
                        break;
                    default:
                        return null;
                }
            }

            if (check_wst == true)
            {
                filter = Add_Filter(filter, "WST_ID = '" + wst + "'");
            }
            else
            {
                if (check_all == true)
                {
                    // Don't need
                    // filter += " AND (" + filter + ") AND GroupID = '" + group + "'";
                }
                else
                {
                    if (check_group == true)
                    {

                        filter = Add_Filter(filter, "GroupID = '" + group + "'");
                    }
                    else
                    {
                        if (check_line == true)
                        {
                            filter = Add_Filter(filter, "LineID = '" + line + "'");
                        }
                    }
                }
            }
            if (Emp_ID_InChecking == "20120689")
            {
                int a = 0;
            }

            if (stand == false)
            {
                // filter = "(" + filter + ") AND LineID not like '%STAND%'";
                filter = Add_Filter(filter, "LineID not like '%STAND%'");
            }

            DataRow[] SearchingPlan = _plan.Select(filter);

            foreach (var plan in SearchingPlan)
            {
                cur_empl_id = plan[Plan_Colum_Empl_ID].ToString().Trim();
                if (cur_empl_id == string.Empty)
                {
                    string WST_ID = plan[Plan_Colum_WST_ID].ToString().Trim();
                    //Vị trí chưa có người trên line
                    if (check_skill == true)
                    {
                        if (IsEmplHaveEnoughSkill(Emp_ID_InChecking, WST_ID))
                        {
                            return plan;
                        }
                        ExtraReason = "Not enought Skill for: " + WST_ID;
                    }
                    else
                    {
                        return plan;
                    }
                }
            }

            if ((SearchingPlan != null) && (SearchingPlan.Count() > 0) && (check_wst == true))
            {
                //TODO: Need Put Warning Log for remove empl has not enough skill in history WST 
            }

            int i = 0, j = 0;
            int total = SearchingPlan.Count();
            int remove_index = -1, new_index = -1;

            if (swap == true)
            {
                for (i = 0; i < total; i++)
                {
                    string cur_wst = SearchingPlan[i][Plan_Colum_WST_ID].ToString().Trim();
                    string cur_empl = SearchingPlan[i][Plan_Colum_Empl_ID].ToString().Trim();

                    if (cur_empl != string.Empty && IsEmployeeCouldbeSwapped(cur_empl) == true) //vị trí hiện tại trong đã có người & người ngày có thể swap
                    {
                        if (IsEmplHaveEnoughSkill(Emp_ID_InChecking, cur_wst))
                        {
                            //Tìm trong line/group đang xem xét...vị trí còn trống thích hợp cho nhân viên chuẩn bị hoán
                            for (j = 0; j < total; j++)
                            {
                                if (SearchingPlan[j][Plan_Colum_Empl_ID].ToString().Trim() == string.Empty)
                                {
                                    string WST_ID = SearchingPlan[j][Plan_Colum_WST_ID].ToString().Trim();
                                    if (IsEmplHaveEnoughSkill(cur_empl, WST_ID))
                                    {
                                        remove_index = i;
                                        new_index = j;
                                        SearchingPlan[new_index][Plan_Colum_Empl_ID] = SearchingPlan[remove_index][Plan_Colum_Empl_ID];
                                        SearchingPlan[new_index][Plan_Colum_Empl_Name] = SearchingPlan[remove_index][Plan_Colum_Empl_Name];


                                        changedEmpl = SearchingPlan[remove_index][Plan_Colum_Empl_ID].ToString();
                                        changedWst = WST_ID;
                                        string history = SearchingPlan[remove_index]["Reason"].ToString();

                                        string info = string.Format("Moved from {0}.Reserved for {1}-{2}. History: {3}", 
                                                                                                            cur_wst, 
                                                                                                            Emp_ID_InChecking, 
                                                                                                            table_id, 
                                                                                                            history);

                                        SearchingPlan[new_index]["Reason"] = info;
                                        string SwapInfo = string.Format("Replace for {0} ", changedEmpl);
                                        SearchingPlan[remove_index]["Reason"] = SwapInfo;
                                        return SearchingPlan[remove_index];
                                    }
                                    ExtraReason = "Not enought Skill for: " + WST_ID;
                                }
                            }
                        }
                    }
                }
            }

            /*********************************************************************/
            // Chạy Optimize lần 2: 
            //     2'--> 1'
            //     3'--> 1'
            /*********************************************************************/
            filter = "";

            //Tìm những việc chưa được assign liên quan đến Shift_InChecking 
            if (optimizie_shift == false)
            {
                switch (shift)
                {
                    case SHIFT_1:
                        filter = "(" + Plan_Colum_Shift + " = '" + SHIFT_1 + "')"; ;
                        break;
                    case SHIFT_2:
                        filter = "(" + Plan_Colum_Shift + " = '" + SHIFT_2 + "')";
                        break;
                    case SHIFT_3:
                        filter = "(" + Plan_Colum_Shift + " = '" + SHIFT_3 + "')";
                        break;
                    default:
                        return null;
                }
            }
            else
            {
                switch (shift)
                {
                    case SHIFT_1:
                        filter = "(" + Plan_Colum_Shift + " = '" + SHIFT_1 + "')"; ;
                        break;
                    case SHIFT_2:
                        filter = "(" + Plan_Colum_Shift + " = '" + SHIFT_2 + "')";
                        break;
                    case SHIFT_3:
                        filter = "(" + Plan_Colum_Shift + " = '" + SHIFT_2 + "')";
                        break;
                    default:
                        return null;
                }
            }

            if (check_wst == true)
            {
                filter = Add_Filter(filter, "WST_ID = '" + wst + "'");
            }
            else
            {
                if (check_all == true)
                {
                    // Don't need
                    // filter += " AND (" + filter + ") AND GroupID = '" + group + "'";
                }
                else
                {
                    if (check_group == true)
                    {

                        filter = Add_Filter(filter, "GroupID = '" + group + "'");
                    }
                    else
                    {
                        if (check_line == true)
                        {
                            filter = Add_Filter(filter, "LineID = '" + line + "'");
                        }
                    }
                }
            }
            if (Emp_ID_InChecking == "20120689")
            {
                int a = 0;
            }

            if (stand == false)
            {
                // filter = "(" + filter + ") AND LineID not like '%STAND%'";
                filter = Add_Filter(filter, "LineID not like '%STAND%'");
            }

           SearchingPlan = _plan.Select(filter);

            foreach (var plan in SearchingPlan)
            {
                cur_empl_id = plan[Plan_Colum_Empl_ID].ToString().Trim();
                if (cur_empl_id == string.Empty)
                {
                    string WST_ID = plan[Plan_Colum_WST_ID].ToString().Trim();
                    //Vị trí chưa có người trên line
                    if (check_skill == true)
                    {
                        if (IsEmplHaveEnoughSkill(Emp_ID_InChecking, WST_ID))
                        {
                            return plan;
                        }
                        ExtraReason = "Not enought Skill for: " + WST_ID;
                    }
                    else
                    {
                        return plan;
                    }
                }
            }

            if ((SearchingPlan != null) && (SearchingPlan.Count() > 0) && (check_wst == true))
            {
                //TODO: Need Put Warning Log for remove empl has not enough skill in history WST 
            }

            i = 0; j = 0;
            total = SearchingPlan.Count();
            remove_index = -1; new_index = -1;

            if (swap == true)
            {
                for (i = 0; i < total; i++)
                {
                    string cur_wst = SearchingPlan[i][Plan_Colum_WST_ID].ToString().Trim();
                    string cur_empl = SearchingPlan[i][Plan_Colum_Empl_ID].ToString().Trim();

                    if (cur_empl != string.Empty && IsEmployeeCouldbeSwapped(cur_empl) == true) //vị trí hiện tại trong đã có người & người ngày có thể swap
                    {
                        if (IsEmplHaveEnoughSkill(Emp_ID_InChecking, cur_wst))
                        {
                            //Tìm trong line/group đang xem xét...vị trí còn trống thích hợp cho nhân viên chuẩn bị hoán
                            for (j = 0; j < total; j++)
                            {
                                if (SearchingPlan[j][Plan_Colum_Empl_ID].ToString().Trim() == string.Empty)
                                {
                                    string WST_ID = SearchingPlan[j][Plan_Colum_WST_ID].ToString().Trim();
                                    if (IsEmplHaveEnoughSkill(cur_empl, WST_ID))
                                    {
                                        remove_index = i;
                                        new_index = j;
                                        SearchingPlan[new_index][Plan_Colum_Empl_ID] = SearchingPlan[remove_index][Plan_Colum_Empl_ID];
                                        SearchingPlan[new_index][Plan_Colum_Empl_Name] = SearchingPlan[remove_index][Plan_Colum_Empl_Name];


                                        changedEmpl = SearchingPlan[remove_index][Plan_Colum_Empl_ID].ToString();
                                        changedWst = WST_ID;
                                        string history = SearchingPlan[remove_index]["Reason"].ToString();

                                        string info = string.Format("Moved from {0}.Reserved for {1}-{2}. History: {3}",
                                                                                                            cur_wst,
                                                                                                            Emp_ID_InChecking,
                                                                                                            table_id,
                                                                                                            history);

                                        SearchingPlan[new_index]["Reason"] = info;
                                        string SwapInfo = string.Format("Replace for {0} ", changedEmpl);
                                        SearchingPlan[remove_index]["Reason"] = SwapInfo;
                                        return SearchingPlan[remove_index];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        string Add_Filter(string cur_filter, string new_filter)
        {
            string ret;
            if (cur_filter.Trim() == "")
            {
                return new_filter;
            }
            else
            {
                ret = cur_filter + " AND " + new_filter;
            }
            return ret;
        }

    }
}