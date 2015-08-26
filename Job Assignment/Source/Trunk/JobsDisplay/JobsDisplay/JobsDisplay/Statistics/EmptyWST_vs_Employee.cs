using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Job_Assignment;
using JobsDisplay;

namespace JobsDisplay.Statistics
{
    public enum EmptyFormState
    {
        GET_MORE,
        RUN_WITH_CURRENT,
        STOP_LINE
    }
    public partial class EmptyWST_vs_Employee : Form
    {
        public EmptyFormState State = EmptyFormState.GET_MORE;

        DataTable _wstList;
        DataTable _empList;

        const int WST_ID_IN_GRID = 0;

        MSSqlDbFactory dao = new MSSqlDbFactory();
        string connString = @"server=(local)\SQLEXPRESS;database=JOB_ASSIGNMENT_DB;Integrated Security = TRUE";
        string LineID;
        EmployeeAssignment _engine;

        private int Close_Counter = 0;
        const int Close_Time = 30;

        public EmptyWST_vs_Employee(string connect_str, string line_id, DateTime date, string shift )
        {
            InitializeComponent();
            connString = connect_str;
            LineID = line_id;

            //Load the list of wst that need to apply people
            // const string WstList = "SELECT distinct [WST_ID],[WST_Name] FROM MDB_004_LineSkillRequest";
            string WstList = @"SELECT distinct [WST_ID],[WST_Name] FROM [JOB_ASSIGNMENT_DB].[dbo].[P007_P008_Tracking]";
            WstList +=  " Where LineID = '"+ LineID + "' AND [Date] = '" + date.ToString("dd MMM yyyy")+"'";
            WstList +=  " AND To_Time is NULL AND (Empl_ID is NULL OR Empl_ID = '')";
            dao.OpenDataTable(connString, ref _wstList, CommandType.Text, WstList);

            // Load the list of people in line stand
            // const string EmplList = "SELECT distinct [Empl_ID],[Empl_Name] FROM MDB_002_Empl_Skill";
            const string EmplList = @"SELECT [Date]
	                                          ,[ShiftName]
                                              ,[Empl_ID]
                                              ,[Empl_Name]
                                              ,[LineID]
                                              ,[LineName]
                                              ,[SubLine_ID]
                                              ,[WST_ID]
                                              ,[From_Time]
                                              ,[To_Time]
                                      FROM [JOB_ASSIGNMENT_DB].[dbo].[P007_P008_Tracking]
                                      Where LineID like '%STAND%' AND [Date] = '29 June 2015'  
                                      AND Empl_ID != ''";

            dao.OpenDataTable(connString, ref _empList, CommandType.Text, EmplList);

            gridviewWST.DataSource = _wstList;
            gridview_FreeEmployee.DataSource = _empList;

            this.AcceptButton = RunWithCurrent_BT;
            this.AcceptButton = InputMore_BT;
            this.AcceptButton = StopLine_BT;
            RunWithCurrent_BT.DialogResult = DialogResult.OK;
            InputMore_BT.DialogResult = DialogResult.OK;
            StopLine_BT.DialogResult = DialogResult.OK;
            AutoClose_Timer.Start();
        }

        public EmptyWST_vs_Employee(DataTable wstList, DataTable emplList)
        {
            InitializeComponent();

            _wstList = wstList;
            _empList = emplList;
            gridviewWST.DataSource = _wstList;
            gridview_FreeEmployee.DataSource = _empList;
        }

        private void gridviewWST_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (gridviewWST.CurrentCell != null && gridviewWST.CurrentCell.Value != null)
                {
                    string WorkStationID = gridviewWST.Rows[e.RowIndex].Cells["WST_ID"].Value.ToString();

                    if (WorkStationID == string.Empty)
                    {
                        //User Click on an empty line
                        txt_CurrentWST.Text = WorkStationID;
                        txt_RequiredSkill.Text = string.Empty;
                        UnHighlightEmployee();
                        return;
                    }

                    List<string> EmplWithRighSkillList = new List<string>();
                    List<string> SkillForWSTList = GetEmployeeAssignmentInstance().GetSkillListForThisWST(WorkStationID);
                    foreach (DataRow row in _empList.Rows)
	                {
                        string empl_ID = row[0].ToString();
                        if (GetEmployeeAssignmentInstance().IsEmplHaveEnoughSkill(empl_ID, WorkStationID))
	                    {
                    		EmplWithRighSkillList.Add(empl_ID)     ;
	                    }
	                }

                    txt_CurrentWST.Text = WorkStationID;
                    //txt_RequiredSkill.Text = String.Join(String.Empty, SkillForWSTList.ToArray()); ;
                    txt_RequiredSkill.Text = String.Join("    ", SkillForWSTList.ToArray()); ;

                    HighlightEmployee(EmplWithRighSkillList);
                }
            }
        }

        private void UnHighlightEmployee()
        {
            foreach (DataGridViewRow row in gridview_FreeEmployee.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void HighlightEmployee(List<string> EmplWithRighSkillList)
        {
            foreach (DataGridViewRow row in gridview_FreeEmployee.Rows)
            {
                if (IsEmptyLine(row) == false)
                {
                    string EmplID = row.Cells["Empl_ID"].Value.ToString();
                    var match = EmplWithRighSkillList.FirstOrDefault(n => n.Contains(EmplID));

                    if (match != null)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightBlue;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }            
        }

        private EmployeeAssignment GetEmployeeAssignmentInstance()
        {
            if (_engine == null)
            {
                _engine = new EmployeeAssignment(connString);
            }

            return _engine;
        }

        private bool IsEmptyLine(DataGridViewRow currentRow)
        {
            if (currentRow.Cells.Count > 0) 
            {      
                foreach(DataGridViewCell cell in currentRow.Cells)    
                {
                   if(cell.Value != null) 
                   {
                       return false;
                   }    
                }
            }

            return true;
        }
        private void gridview_FreeEmployee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (gridview_FreeEmployee.CurrentCell != null && gridview_FreeEmployee.CurrentCell.Value != null)
                {
                    string EmplID = gridview_FreeEmployee.Rows[e.RowIndex].Cells["Empl_ID"].Value.ToString();

                    if (EmplID == string.Empty)
                    {
                        txt_CurrentEmpl.Text = string.Empty;
                        txt_CurrentEmplSkill.Text = string.Empty;
                        return;
                    }

                    List<string> SkillOfEmplList = GetEmployeeAssignmentInstance().GetSkillListOfThisEmpl(EmplID);

                    txt_CurrentEmpl.Text = EmplID;
                    txt_CurrentEmplSkill.Text = String.Join("    ", SkillOfEmplList.ToArray());
                }
            }
        }

        private void RunWithCurrent_BT_Click(object sender, EventArgs e)
        {
            State = EmptyFormState.RUN_WITH_CURRENT;
        }

        private void InputMore_BT_Click(object sender, EventArgs e)
        {
            State = EmptyFormState.GET_MORE;
        }

        private void StopLine_BT_Click(object sender, EventArgs e)
        {
            State = EmptyFormState.STOP_LINE;
        }

        private void AutoClose_Timer_Tick(object sender, EventArgs e)
        {
            Close_Counter++;
            if (Close_Counter >= Close_Time)
            {
                this.Close();
            }
        }

        private void EmptyWST_vs_Employee_MouseMove(object sender, MouseEventArgs e)
        {
            //Close_Counter = 0;
        }
    }
}
