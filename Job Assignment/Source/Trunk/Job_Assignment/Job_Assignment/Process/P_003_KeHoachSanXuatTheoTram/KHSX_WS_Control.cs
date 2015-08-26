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
using System.Collections;

namespace Job_Assignment
{
    public partial class Form1 : SQL_APPL
    {
        public enum Empl_Status_Type
        {
            Avaliable = 0,
            Inuse = 1,
            Absent = 2,
        }

        public enum Empl_Shift_Name
        {
            Shift_HC = 0,
            Shift_1 = 1,
            Shift_2 = 2,
            Shift_3 = 3,
        }

        public const string SHIFT_1 = "Shift_1";
        public const string SHIFT_2 = "Shift_2";
        public const string SHIFT_3 = "Shift_3";
        public const string SHIFT_UNKNOW = "UnknowShift";
        

        private void FormatRowInGridView(DataGridView gridview, int ColumIndex)
        {
            Color[] colors = new Color[5];

            colors[0] = Color.WhiteSmoke;
            colors[1] = Color.LightPink;
            colors[2] = Color.LightBlue;
            colors[3] = Color.LightGreen;
            colors[4] = Color.LightSalmon;

            Color CurrentColor = Color.White;
            int NextColor = 0;
            string PrevVal = "";

            foreach (DataGridViewRow row in KHSX_WS_dtgrid.Rows)
            {
                if ((string)row.Cells[ColumIndex].Value != PrevVal)
                {
                    PrevVal = (string)row.Cells[1].Value;

                    NextColor = (NextColor >= colors.Length - 1) ? 0 : (NextColor += 1);

                    CurrentColor = colors[NextColor];
                    row.DefaultCellStyle.BackColor = CurrentColor;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = CurrentColor;
                }
            }
        }

        public bool Get_Shift_ID(string line_id, ref Empl_Shift_Name ShiftID, ref int ShiftPercent)
        {
            //TODO: Implement Get_Shift_ID --> DONE

            //1. Get last shift of that line to see how many percent of shift was done
            Empl_Shift_Name LatestShift = Empl_Shift_Name.Shift_1;
            int LatestShiftPercent = 0;
            int CurrShiftPercent = 0;
            Empl_Shift_Name CurrentShift;

            foreach (DataRow row in KHSX_WS_dtb.Rows)
            {

                if (row["LineID"].ToString() == line_id)
                {
                    string val = (string)row["ShiftName"];
                    //CurrentShift = (Empl_Shift_Name)(Convert.ToInt32(val));

                    CurrentShift = Empl_Shift_Name.Shift_HC;

                    switch (val)
                    {
                        case "Shift_HC":
                            CurrentShift = Empl_Shift_Name.Shift_HC;
                            break;

                        case SHIFT_1:
                            CurrentShift = Empl_Shift_Name.Shift_1;
                            break;

                        case SHIFT_2:
                            CurrentShift = Empl_Shift_Name.Shift_2;
                            break;

                        case SHIFT_3:
                            CurrentShift = Empl_Shift_Name.Shift_3;
                            break;

                    }

                    CurrShiftPercent = (int)row["Shift_Percent"];

                    if ((CurrentShift > LatestShift) || (CurrShiftPercent > LatestShiftPercent))
                    {
                        LatestShift = CurrentShift;
                        LatestShiftPercent = CurrShiftPercent;
                        
                    }
                }
            }
            ShiftID = LatestShift;
            ShiftPercent = LatestShiftPercent;

            return  true;
        }

        /// <summary>
        /// Hôm qua làm WST nào. Hôm nay làm lại 
        /// 
        /// </summary>
        /// <param name="part"></param>
        /// <param name="line_id"></param>
        /// <param name="wst_id"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        private string Get_Empl_Last_Plan(DateTime date, string line_id, string wst_id, string shift, ref DataTable AvailableEmpList)
        {
            DateTime last_date;

            if (date.DayOfWeek != DayOfWeek.Monday)
            {
                last_date = date.AddDays(-1);
            }
            else
            {
                last_date = date.AddDays(-2);
            }

            //Load the list of table last day
            DataTable WorkingTable_Of_LastDay = Load_KHSX_WS_Temp_DB_Date(last_date);
            
            foreach (DataRow r in WorkingTable_Of_LastDay.Rows)
            {
                string LineID = r["LineID"].ToString().Trim();
                string WST_ID = r["WST_ID"].ToString().Trim();
                string ShiftName = r["ShiftName"].ToString().Trim();
                string Empl_Name = r["Empl_Name"].ToString().Trim();


                if ((Empl_Name != "") && (LineID == line_id) && (WST_ID == wst_id) && (ShiftName == shift))
                {
                    //MessageBox.Show("Empl" + Empl_Name + "\n\r" + "History Of Last Day :" + line_id + " - " + wst_id + " - " + shift, last_date.ToShortDateString());
                    return Empl_Name;
                }
            }

            //TODO: Implement Get_Empl_Last_Plan --> DONE
            return "";
        }

        private bool UpdateEmplStatus(string Empl_ID, ref DataTable Empl_List, Empl_Status_Type Status)
        {
            if (Empl_ID != "")
            {
                foreach (DataRow row in Empl_List.Rows)
                {
                    if (row["Empl_ID"].ToString().Trim() == Empl_ID)
                    {
                        row["Status"] = Status;
                    }
                }
            }

            return false;

        }

        private string GetRequiredSkillForThisWST(string LineID, string WST_ID, string ShiftName)
        {
            String Skill_Id;
            DataTable Tmp_dtb;

            Tmp_dtb = Load_MDB04_Line_Vs_Skill();

            string Line_id, Wst_id;

            //From work Station, find the needed skill
            Skill_Id = "";

            foreach (DataRow row in Tmp_dtb.Rows)
            {
                Line_id = row["LineID"].ToString().Trim();
                Wst_id = row["WST_ID"].ToString().Trim();

                if ((Line_id == LineID) && (Wst_id == WST_ID))
                {
                    Skill_Id = row["Skill_ID"].ToString().Trim(); ;
                    break;
                }
            }

            return Skill_Id;
        
        }
        /// <summary>
        /// Sắp nhân viên cho các line mới phát sinh. Cách thực hiện:
        /// 1. Tìm danh sách các workstation cần sắp
        /// 2. Tìm danh sách nhân viên có kĩ năng phù hợp với workstation
        /// 3. Chọn nhân viên có số kỹ năng ít nhất để sắp cho workstation đó.
        /// Trường hợp có nhiều nhân viên có số kỹ năng giống nhau --> Chọn nhân viên đầu tiên trong số đó
        /// </summary>
        /// <param name="part"></param>
        /// <param name="line_id"></param>
        /// <param name="wst_id"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        private string Get_Empl_New_Plan(DateTime date, string input_line_id, string input_wst_id, string shift, ref DataTable Empl_List)
        {
            String Skill_Id, Emp_Id;
            DataTable Tmp_dtb;
            Tmp_dtb = Load_MDB04_Line_Vs_Skill();

            string Line_id,Wst_id;

            //From work Station, find the needed skill
            Skill_Id = "";

            foreach (DataRow row in Tmp_dtb.Rows)
            {
                Line_id = row["LineID"].ToString().Trim();
                Wst_id = row["WST_ID"].ToString().Trim();

                if ((Line_id == input_line_id) && (Wst_id == input_wst_id))
                {
                    Skill_Id = row["Skill_ID"].ToString().Trim(); ;
                    break;
                }
            }
            
            //Warning if we cannot find the skill that map with the workstation
            if (Skill_Id == "")
            {
                MessageBox.Show("Cannot find the skill ID associated with WorkStation: " + input_wst_id, "Error !!!");
                return "";
            }

            //From the employee list, find the employee have needed skill
            Emp_Id = "";
            string Emp_Skill = "";
            Empl_Status_Type EmplStatus;

            DataTable SuitableEmpList = new DataTable();

            SuitableEmpList.Columns.Add("Empl_ID", typeof(String));
            SuitableEmpList.Columns.Add("Empl_Name", typeof(String));
            SuitableEmpList.Columns.Add("NumOfSkill", typeof(int));

            foreach (DataRow row in Empl_List.Rows)
            {
                EmplStatus = (Empl_Status_Type) row["Status"];
                Emp_Skill = row["Skill_ID"].ToString().Trim();

                if ((EmplStatus == Empl_Status_Type.Avaliable) && (Emp_Skill == Skill_Id))
                {
                    //Find the employee have correct skill. Put in the list and we will select one of them later

                    DataRow newRow = SuitableEmpList.NewRow();

                    newRow["Empl_ID"]          = row["Empl_ID"];
                    newRow["Empl_Name"]          = row["Empl_Name"];
                    newRow["NumOfSkill"]    = 0;//Num of skill will be calculated later
                    SuitableEmpList.Rows.Add(newRow);
                    //Emp_Id = row["Empl_ID"].ToString().Trim();
                    //break;
                }
            }

            foreach (DataRow row in SuitableEmpList.Rows)
            {
                string ID = row["Empl_ID"].ToString().Trim();

                int NumOf_Skill = 0;

                foreach (DataRow r in Empl_List.Rows)
                {
                    String EmplID = r["Empl_ID"].ToString().Trim();

                    //Count the skill of employee
                    if (EmplID == ID)
                    {
                        NumOf_Skill++;
                    }
                }
                row["NumOfSkill"] = NumOf_Skill;            
            }

            //Now, select the employee with smallest the number of skill
            int minNumberOfSkill = int.MaxValue;
            int maxNumberOfSkill = int.MinValue;

            foreach (DataRow dr in SuitableEmpList.Rows)
            {
                int Level = dr.Field<int>("NumOfSkill");
                minNumberOfSkill = Math.Min(minNumberOfSkill, Level);
                maxNumberOfSkill = Math.Max(maxNumberOfSkill, Level);
            }

            foreach (DataRow row in SuitableEmpList.Rows)
            {
                if ((int)row["NumOfSkill"] == minNumberOfSkill)
                {
                    Emp_Id = row["Empl_ID"].ToString().Trim();
                    break;
                }
            }
            
            //Mark that this employee is used
            if (Emp_Id != "")
            {
                foreach (DataRow row in Empl_List.Rows)
                {
                    if (row["Empl_ID"].ToString().Trim() == Emp_Id)
                    {
                        row["Status"] = Empl_Status_Type.Inuse;
                    }
                }         
            }

            //TODO: Implement Get_Empl_New_Plan --> DONE
            return Emp_Id;
        }

        private bool Update_EmployeeStatus(Empl_Status_Type State)
        {
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="empl_id"></param>
        /// <returns></returns>
        /// 
        private string Get_Empl_Name(string empl_id)
        {
            //TODO: Optimize this in phase 2
            DataTable Dtb = Load_MDB_002_Empl_Skill();            

            string name = "";
            string ID;
            foreach (DataRow row in Dtb.Rows)
            {
                ID = row["Empl_ID"].ToString().Trim();

                if (ID == empl_id)
                {
                    name = row["Empl_Name"].ToString().Trim();
                    break;
                }
            }
            //TODO: Implement Get_Empl_Name --> Done
            return name;
        }

        private DataTable Load_Avaliable_Empl_List(DateTime date)
        {
            string empl_id;
            DataTable all_empl_table = Load_All_Empl_Skill();
            DataTable all_leave_empl = Load_Leave_Register(date);

            foreach (DataRow row in all_empl_table.Rows)
            {
                empl_id = row["Empl_ID"].ToString().Trim();

                if (Is_Absent(empl_id, date)) //Search in Leave_dtb
                {
                    row["Status"] = Empl_Status_Type.Absent;
                    row[SHIFT_1] = 0;
                    row[SHIFT_2] = 0;
                    row[SHIFT_3] = 0;
                    all_empl_table.Rows.Remove(row);
                }else{
                    row["Status"] = Empl_Status_Type.Avaliable;
                    row[SHIFT_1] = 0;
                    row[SHIFT_2] = 0;
                    row[SHIFT_3] = 0;
                }
            }

            return all_empl_table;
        }
        private bool Is_Employee_Has_This_Skill(string iEmp_ID, string iSkill_ID, DataTable Empl_List)
        {
            string Emp_Skill = "";
            string Emp_ID = "";

            foreach (DataRow row in Empl_List.Rows)
            {
                Emp_ID = row["Empl_ID"].ToString().Trim();
                Emp_Skill = row["Skill_ID"].ToString().Trim();

                if ((Emp_ID == iEmp_ID) && (Emp_Skill == iSkill_ID))
                {
                    return true;
                }
            }

            return false;
        
        }
        private bool Check_Empl_Able_for_Other_WST(string CurrEmpl_Id_ToCheck, 
                                                    ref string AlternateEmpID, 
                                                    ref string AlternateEmpName,
                                                    ref int NewPlaceForCurrEmpl,
                                                    DataTable EmployeeList, 
                                                    DataTable Remained_WST, 
                                                    DataTable Remained_Empl)
        {
            //Todo: Check_Empl_Able_for_Other_WST
            //Rule: 
            // 1. Kiểm tra xem nếu nhân viên có skill phù hợp với WST còn trống
            // 2. Kiểm tra xem có nhân viên nào khác trong số những nhân viên chưa có vị trí làm việc có thể thay thế cho nhân viên này
            // 3. Nếu thỏa mãn cả 2 điều kiện --> Return true & và return ID nhân viên có thể thay thế trong AlternateEmpID
            String Required_Skill = "";

            foreach (DataRow row in Remained_WST.Rows)
            {
                Required_Skill = row["Required_Skill"].ToString().Trim();

                if (Is_Employee_Has_This_Skill(CurrEmpl_Id_ToCheck, Required_Skill, EmployeeList))
                {
                    //Kiểm tra skill hiện tại Empl đang dùng ở WST hiện tại.
                    String SkillInUse = "";
                    foreach (DataRow r in KHSX_WS_dtb.Rows)
                    {
                        if (r["Empl_ID"].ToString().Trim() == CurrEmpl_Id_ToCheck)
                        { 
                            String LineID = r["LineID"].ToString().Trim();
                            String WST_ID = r["WST_ID"].ToString().Trim();
                            String Shiftname = r["WST_ID"].ToString().Trim();
                            SkillInUse = GetRequiredSkillForThisWST(LineID, WST_ID, Shiftname);
                        }
                    }

                    String Emp_ID, Emp_Name;
                    foreach (DataRow r in Remained_Empl.Rows)
                    {
                        Emp_ID = r["Empl_ID"].ToString().Trim();
                        Emp_Name = r["Empl_Name"].ToString().Trim();

                        if (Is_Employee_Has_This_Skill(Emp_ID, Required_Skill, EmployeeList))
                        {
                            AlternateEmpID = Emp_ID;
                            AlternateEmpName = Emp_Name;
                            NewPlaceForCurrEmpl = (int)row["Index_InMasterTbl"];
                            return true;
                        }

                    }
                }
                
            }
            return false;
        }

        private bool Set_New_WST(string empl_id, string shift)
        {
            //Todo: Set_New_WST
            return false;
        }
    }
}