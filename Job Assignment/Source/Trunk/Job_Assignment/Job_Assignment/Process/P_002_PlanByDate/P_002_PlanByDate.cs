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
        readonly Color COLOR_NUMOFSHIFT_TOO_SMALL = Color.Red;
        readonly Color COLOR_SHIFTNAME_ON_LINE_INVALID = Color.Red;
        readonly Color COLOR_TOTAL_SHIFT_INVALID = Color.Red;
        readonly Color COLOR_TOTAL_SHIFT_VALID = Color.White;
        readonly Color COLOR_LINE_ODD = Color.White;
        readonly Color COLOR_LINE_EVEN = Color.WhiteSmoke;
        readonly Color COLOR_EDITABLE_COLUMN = Color.Violet;
        readonly Color COLOR_EDITABLE_COLUMN_UNDER_1_5_SHIFT = Color.Violet;
        readonly Color COLOR_EDITABLE_COLUMN_FROM_1_5_TO_2_0_SHIFT = Color.Violet;
        const int MAX_SHIFT_ON_LINE = 3;
        
        MaterDatabase KeHoachSanXuatTheoNgayList_MasterDatabase;
        Button_Lbl PlanByDate_Create_BT;
        Button_Lbl PlanByDate_Calculate_BT;
        TextBox_Lbl txtTotalRequireResource;
        TextBox_Lbl txtInterestRequireResource;
        DataTable dtThreeShiftGroup = null;
        DataTable dtOneShiftGroup = null;
        DataTable dtTwoShiftGroup = null;
       // Button_Lbl PlanByDate_Check_BT;
        //Dho-Fixme: Do we need to use the button "Check_BT"?
        PlanByDateController planByDateController;
        List<string> EditableColumn = new List<string>() { "Date", "PartNumber", "Qty", "Priority", "Capacity" };

        public string KeHoachSanXuatTheoNgayList_Select_CMD = @"SELECT * FROM [P_002_PlanForProductionByDate]";
        public string KeHoachSanXuatTheoNgayList_Init_Database_CMD = @"SELECT * FROM [P_002_PlanForProductionByDate] 
                                                      WHERE [Date] = ''";
        private bool KeHoachSanXuatTheoNgayList_Exist = false;

        private bool P002_PlanByDate_Init()
        {
            if (KeHoachSanXuatTheoNgayList_Exist == true)
            {
                if (tabControl1.TabPages.Contains(KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_Tab) == false)
                {
                    tabControl1.TabPages.Insert(tabControl1.TabPages.Count, KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_Tab);
                }
                tabControl1.SelectTab("P002_PlanByDate");
                return true;
            }
            KeHoachSanXuatTheoNgayList_Exist = true;
            Init_KeHoachSanXuatTheoNgay_Excel();
            KeHoachSanXuatTheoNgayList_MasterDatabase = new MaterDatabase(OpenXL, tabControl1, "P002_PlanByDate", ProductionPlanByDate_Index, MasterDatabase_Connection_Str, 
                                                            KeHoachSanXuatTheoNgayList_Init_Database_CMD, KeHoachSanXuatTheoNgayList_Select_CMD,
                                                            3, KeHoachSanXuatTheoNgay_Excel_Struct, filterStatusLabel, showAllLabel,
                                                            StatusLabel1, StatusLabel2, ProgressBar1);
            
            // Empl_Skill_List_MasterDatabase.MasterDatabase_GridviewTBL.dataGridView_View.Columns["Line_ID"].Frozen = true;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.BackgroundColor = Color.White;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.Privot_BT.Visible = true;

            planByDateController = new PlanByDateController(KeHoachSanXuatTheoNgayList_MasterDatabase);

            //Dho-Fixme: Do we need to use the button "Check_BT"?
            PosSize possize = new PosSize();
            possize.pos_x = 200;
            possize.pos_y = 90;
            PlanByDate_Create_BT = new Button_Lbl(1, KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_Tab, "Create", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            PlanByDate_Create_BT.My_Button.Click += new EventHandler(PlanByDate_Create_BT_Click);

            possize.pos_x = 300;
            possize.pos_y = 90;
            PlanByDate_Calculate_BT = new Button_Lbl(1, KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_Tab, "Calculate", possize, (AnchorStyles)AnchorStyles.Left | AnchorStyles.Top);
            PlanByDate_Calculate_BT.My_Button.Click += new EventHandler(Button_Calculte_Click);

            possize.pos_x = 700;
            possize.pos_y = 90;
            txtTotalRequireResource = new TextBox_Lbl(1, KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_Tab, "Total", TextBox_Type.TEXT, possize, AnchorType.LEFT);
            txtTotalRequireResource.My_TextBox.ReadOnly = true;
            txtTotalRequireResource.My_TextBox.TextAlign = HorizontalAlignment.Right;
            txtTotalRequireResource.My_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            possize.pos_x = 880;
            txtInterestRequireResource = new TextBox_Lbl(1, KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_Tab, "Ratio increasing", TextBox_Type.TEXT, possize, AnchorType.LEFT);
            txtInterestRequireResource.My_TextBox.Location = new Point(possize.pos_x + 100, possize.pos_y);
            txtInterestRequireResource.My_TextBox.ReadOnly = true;
            txtInterestRequireResource.My_TextBox.TextAlign = HorizontalAlignment.Right;
            txtInterestRequireResource.My_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            dtOneShiftGroup = planByDateController.GetShiftGroup(PlanByDateController.SHIFT_GROUP_TYPE_ONE);
            dtThreeShiftGroup = planByDateController.GetShiftGroup(PlanByDateController.SHIFT_GROUP_TYPE_THREE);
            dtTwoShiftGroup = planByDateController.GetShiftGroup(PlanByDateController.SHIFT_GROUP_TYPE_TWO);
            
            //add column ShiftNamePerLine -> allow manual shiftname on line
            DataGridViewMultiColumnComboBoxColumn col = new DataGridViewMultiColumnComboBoxColumn();
            col.Name = "ShiftNamePerLine";
            col.DataPropertyName = "ShiftNamePerLine";
            col.DataSource = dtThreeShiftGroup;
            col.ValueMember = "GroupName";
            col.ColumnNames = new List<string> { "GroupName" };
            col.ColumnWidths = new List<string>() { "200" };

            if (KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns.Contains("ShiftNamePerLine"))
            {
                int index = KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["ShiftNamePerLine"].Index;
                KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns.RemoveAt(index);
                KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns.Insert(index, col);
            }
            //end add column ShiftNamePerLine

            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.ColumnHeadersHeight = 50;


            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["TotalShiftPerLine"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Capacity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["NumOfShift"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["NumOfPerson_Per_Day"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_1_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_2_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_3_Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["NumOfPerson_Per_Day"].Width = 70;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["TotalShiftPerLine"].Width = 70;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Capacity"].Width = 70;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["NumOfShift"].Width = 70;

            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Priority"].Width = 50;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_1_From"].Width = 70;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_1_To"].Width = 70;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_1_Qty"].Width = 70;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_1_Main"].Width = 70;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_2_From"].Width = 70;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_2_To"].Width = 70;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_2_Main"].Width = 70;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_2_Qty"].Width = 70;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_3_From"].Width = 70;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_3_To"].Width = 70;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_3_Qty"].Width = 70;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_3_Main"].Width = 70;

            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["ShiftNamePerLine"].HeaderText = "Shift name per line";
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["TotalShiftPerLine"].HeaderText = "Total shift per line";
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["NumOfShift"].HeaderText = "Num of shift";
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["NumOfPerson_Per_Day"].HeaderText = "Num Of Person Per Day";
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_1_From"].HeaderText = "Shift_1 From";
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_1_To"].HeaderText = "Shift_1 To";
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_1_Qty"].HeaderText = "Shift_1 Qty";
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_1_Main"].HeaderText = "Shift_1 Main";
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_2_From"].HeaderText = "Shift_2 From";
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_2_To"].HeaderText = "Shift_2 To";
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_2_Qty"].HeaderText = "Shift_2 Qty";
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_2_Main"].HeaderText = "Shift_2 Main";
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_3_From"].HeaderText = "Shift_3 From";
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_3_To"].HeaderText = "Shift_3 To";
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_3_Qty"].HeaderText = "Shift_3 Qty";
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns["Shift_3_Main"].HeaderText = "Shift_3 Main";
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.EditMode = DataGridViewEditMode.EditOnEnter;
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.Delete_Rows_BT.Visible = true;

            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.CellValueChanged += new DataGridViewCellEventHandler(KeHoachSanXuatTheoNgayList_MasterDatabase_GridView_CellValueChanged);
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.CellContentClick += new DataGridViewCellEventHandler(KeHoachSanXuatTheoNgayList_MasterDatabase_GridView_CellContentClick);
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(KeHoachSanXuatTheoNgayList_MasterDatabase_GridView_DataBindingComplete);
            KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.CellBeginEdit += new DataGridViewCellCancelEventHandler(KeHoachSanXuatTheoNgayList_MasterDatabase_GridView_CellBeginEdit);

            //set role
            string moduleId = "P_002";
            RoleHelper.SetRole(KeHoachSanXuatTheoNgayList_MasterDatabase, moduleId);
            PlanByDate_Create_BT.My_Button.Enabled = RoleHelper.GetCurrentUserLoginRole(UserRoleName.CREATE, moduleId);
            PlanByDate_Calculate_BT.My_Button.Enabled = RoleHelper.GetCurrentUserLoginRole(UserRoleName.CREATE, moduleId);

            return true;
        }

        void KeHoachSanXuatTheoNgayList_MasterDatabase_GridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            string columnName = KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns[e.ColumnIndex].Name;
            DataGridViewRow row = KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows[e.RowIndex];

            if (columnName.Equals("ShiftNamePerLine"))
            {
                double totalShiftPerLine = (double)Utils.ObjectToDecimal(row.Cells["TotalShiftPerLine"].Value, -1);
                String shiftNamePerLine = Utils.ObjectToString(row.Cells["ShiftNamePerLine"].Value);
                String subLineID = Utils.ObjectToString(row.Cells["SubLine_ID"].Value);

                if (totalShiftPerLine <= 0)
                    return;

                //if (shiftNamePerLine == PlanByDateController.SHIFT_1_SHIFT_3
                //       || shiftNamePerLine == PlanByDateController.SHIFT_1_SHIFT_2_SHIFT_3)
                //{
                //    var cell = row.Cells["ShiftNamePerLine"] as DataGridViewMultiColumnComboBoxCell;
                //    cell.DataSource = dtThreeShiftGroup;
                //}
                //else
                //{
                if (totalShiftPerLine > 0 && totalShiftPerLine <= 1.75)
                {
                    var cell = row.Cells["ShiftNamePerLine"] as DataGridViewMultiColumnComboBoxCell;
                    cell.DataSource = dtOneShiftGroup;
                }
                //else if (totalShiftPerLine <= 1.75)
                //{
                //}
                else if (totalShiftPerLine < 2)
                {
                    var cell = row.Cells["ShiftNamePerLine"] as DataGridViewMultiColumnComboBoxCell;
                    cell.DataSource = dtTwoShiftGroup;
                }
                else
                {
                    var cell = row.Cells["ShiftNamePerLine"] as DataGridViewMultiColumnComboBoxCell;
                    cell.DataSource = dtThreeShiftGroup;
                }

            }
        }

        void KeHoachSanXuatTheoNgayList_MasterDatabase_GridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                FormatDataGridViewDisplay();
            }
            catch
            {
                MessageBox.Show("Not apply Format");
            }
        }

        void KeHoachSanXuatTheoNgayList_MasterDatabase_GridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.CurrentCell.RowIndex == -1)
                return;
            int columnIndex = KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.CurrentCell.ColumnIndex;
            int rowIndex = KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.CurrentCell.RowIndex;

            string columnName = KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns[columnIndex].Name;
            if (new List<string> { "Shift_1_Main", "Shift_2_Main", "Shift_3_Main" }.Contains(columnName))
            {
                KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        void KeHoachSanXuatTheoNgayList_MasterDatabase_GridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex  == -1)
                return;
            
            string columnName = KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns[e.ColumnIndex].Name;

            DataTable inputTable = ((BindingSource)KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.DataSource).DataSource as DataTable;
            String subLineID = Utils.ObjectToString( KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView["SubLine_ID", e.RowIndex].Value);
            String partNumber = Utils.ObjectToString( KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView["PartNumber", e.RowIndex].Value);
            if (columnName == "ShiftNamePerLine" && !String.IsNullOrEmpty(subLineID))
            {
                //MessageBox.Show("Change " + KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns[e.ColumnIndex].Name);
                String shiftNamePerLine = (String)KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView[e.ColumnIndex, e.RowIndex].Value;

                foreach (DataRow row in inputTable.Rows )
                {
                    String subLine = Utils.ObjectToString(row["SubLine_ID"]);

                    if (subLine == subLineID)
                    {
                        row["ShiftNamePerLine"] = shiftNamePerLine;
                    }
                }
                DateTime dateCalculate = Utils.ObjectToDecimal(KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView["Date", e.RowIndex].Value, DateTime.MinValue);

                if (dateCalculate != DateTime.MinValue)
                {
                    String ret = planByDateController.FillShiftNameFromAndToTime(dateCalculate, subLineID, ref inputTable);
                    if (!String.IsNullOrEmpty(ret))
                    {
                        Logger.GetInstance().WriteLogData("KeHoachSanXuatTheoNgayList_MasterDatabase_GridView_CellValueChanged", ret);
                        MessageBox.Show(ret);
                    }
                }
            }
            else if (new List<string> { "Shift_1_Main", "Shift_2_Main", "Shift_3_Main" }.Contains(columnName))
            {
                String shitColumnName = columnName.Replace("_Main", "");
                //bool oldValue = false;
                bool oldValue = !Utils.ObjectToBoolean(KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView[e.ColumnIndex, e.RowIndex].Value, false);

                //if (KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView[e.ColumnIndex, e.RowIndex].Value != DBNull.Value)
                //{
                //    oldValue = !(Boolean)KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView[e.ColumnIndex, e.RowIndex].Value;
                //}
                if (!oldValue && Utils.ObjectToDecimal(KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView[shitColumnName + "_Qty", e.RowIndex].Value, 0) > 0)
                {
                    string ret = planByDateController.ChangeMainPart(ref inputTable, subLineID, partNumber, shitColumnName, oldValue);

                    if (!String.IsNullOrEmpty(ret))
                    {
                        Logger.GetInstance().WriteLogData("KeHoachSanXuatTheoNgayList_MasterDatabase_GridView_CellValueChanged", ret);
                        MessageBox.Show(ret);
                    }
                }
            }
            else if (new List<string> { "Capacity", "Qty"}.Contains(columnName))
            {
                var drv = ((BindingSource)KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.DataSource).Current as DataRowView;
                if (drv != null)
                {
                    DataRow row = drv.Row;
                    string ret = planByDateController.UpdateRowInformation(ref inputTable, row, false);

                    if (!String.IsNullOrEmpty(ret))
                        MessageBox.Show(ret);
                    else
                    {
                        DateTime date = Utils.ObjectToDateTime(row["Date"], DateTime.MinValue);
                        if (date != DateTime.MinValue)
                        {
                            ret = planByDateController.FillShiftNameFromAndToTime(date, subLineID, ref inputTable);
                            if (!String.IsNullOrEmpty(ret))
                            {
                                Logger.GetInstance().WriteLogData("KeHoachSanXuatTheoNgayList_MasterDatabase_GridView_CellValueChanged", ret);
                                MessageBox.Show(ret);
                            }
                        }

                    }
                }

            }
        }

        void FormatDataGridViewDisplay()
        {
            DataTable inputTable = ((BindingSource)KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.DataSource).DataSource as DataTable;
            foreach (DataGridViewColumn column in KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Columns)
            {
                if (!EditableColumn.Contains(column.Name))
                {
                    column.ReadOnly = true;
                }
            }

            //DataGridViewRow lastRow = null; 
            int currentIndexRow = 0;
            foreach (DataGridViewRow row in KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.Rows)
            {
                if (row.IsNewRow)
                    continue;

                double numOfShift = (double)Utils.ObjectToDecimal(row.Cells["NumOfShift"].Value, -1);
                double totalShiftPerLine = (double)Utils.ObjectToDecimal(row.Cells["TotalShiftPerLine"].Value, -1);
                String shiftNamePerLine = Utils.ObjectToString(row.Cells["ShiftNamePerLine"].Value);
                String subLineID = Utils.ObjectToString(row.Cells["SubLine_ID"].Value);

                //hightlight editable column
                foreach (string col in EditableColumn)
                {
                    row.Cells[col].Style.BackColor = COLOR_EDITABLE_COLUMN;
                }
                //hightlight odd and even row
                row.DefaultCellStyle.BackColor = currentIndexRow % 2 == 0 ? COLOR_LINE_ODD : COLOR_LINE_EVEN;
                currentIndexRow += 1;

                if (totalShiftPerLine <= 0)
                {
                    row.Cells["ShiftNamePerLine"].ReadOnly = true;
                    row.Cells["ShiftNamePerLine"].Style.BackColor = COLOR_SHIFTNAME_ON_LINE_INVALID;
                }
                else if (totalShiftPerLine < 0.5)
                {
                    row.Cells["ShiftNamePerLine"].ReadOnly = false;
                    // row.Cells["ShiftNamePerLine"].Style.BackColor = COLOR_EDITABLE_COLUMN_UNDER_1_SHIFT;
                    row.Cells["ShiftNamePerLine"].Style.BackColor = COLOR_NUMOFSHIFT_TOO_SMALL;
                    //var cell = row.Cells["ShiftNamePerLine"] as DataGridViewMultiColumnComboBoxCell;
                    //cell.DataSource = dtOneShiftGroup;
                }
                else if (totalShiftPerLine <= 1.5)
                {
                    row.Cells["ShiftNamePerLine"].ReadOnly = false;
                    row.Cells["ShiftNamePerLine"].Style.BackColor = COLOR_EDITABLE_COLUMN_UNDER_1_5_SHIFT;
                }
                else if (totalShiftPerLine <= 1.75)
                {
                    row.Cells["ShiftNamePerLine"].ReadOnly = false;
                    row.Cells["TotalShiftPerLine"].Style.BackColor = COLOR_SHIFTNAME_ON_LINE_INVALID;
                    row.Cells["ShiftNamePerLine"].Style.BackColor = COLOR_SHIFTNAME_ON_LINE_INVALID;
                }
                else if (totalShiftPerLine < 2)
                {
                    row.Cells["ShiftNamePerLine"].ReadOnly = false;
                    row.Cells["TotalShiftPerLine"].Style.BackColor = COLOR_SHIFTNAME_ON_LINE_INVALID;
                    row.Cells["ShiftNamePerLine"].Style.BackColor = COLOR_EDITABLE_COLUMN_FROM_1_5_TO_2_0_SHIFT;
                    //var cell = row.Cells["ShiftNamePerLine"] as DataGridViewMultiColumnComboBoxCell;
                    //cell.DataSource = dtTwoShiftGroup;
                }
                else if (totalShiftPerLine <= 3)
                {
                    row.Cells["ShiftNamePerLine"].ReadOnly = false;
                    row.Cells["ShiftNamePerLine"].Style.BackColor = COLOR_EDITABLE_COLUMN;
                }
                else if (totalShiftPerLine > MAX_SHIFT_ON_LINE)
                {
                    row.Cells["ShiftNamePerLine"].ReadOnly = false;
                    row.Cells["TotalShiftPerLine"].Style.BackColor = COLOR_TOTAL_SHIFT_INVALID;
                    row.Cells["ShiftNamePerLine"].Style.BackColor = COLOR_EDITABLE_COLUMN;
                }

                //if (numOfShift >=0 && numOfShift < 0.5)
                //{
                //    row.Cells["NumOfShift"].Style.BackColor = COLOR_NUMOFSHIFT_TOO_SMALL;
                //}

                //hightlight MainPart
                foreach (string shift in new List<string> { "Shift_1", "Shift_2", "Shift_3" })
                {
                    if (Utils.ObjectToDecimal(row.Cells[shift + "_Qty"].Value, -1) > 0)
                    {
                        row.Cells[shift + "_Main"].ReadOnly = false;
                        row.Cells[shift + "_Main"].Style.BackColor = COLOR_EDITABLE_COLUMN;
                    }
                }
            }

        }

        private string Calculate(DateTime dateCalculate)
        {
            DataTable inputPlan = ((BindingSource)KeHoachSanXuatTheoNgayList_MasterDatabase.MasterDatabase_GridviewTBL.GridView.DataSource).DataSource as DataTable;
            String ret = planByDateController.Calculate(dateCalculate, ref inputPlan);
            inputPlan.DefaultView.Sort = "SubLine_ID ASC, Priority ASC";
         
            //show TotalResource
            int totalResource = 0;
            ret = planByDateController.GetTotalResource(inputPlan, ref totalResource);
            //if (!String.IsNullOrEmpty(ret))
            //{
            //    return ret;
            //}
            txtTotalRequireResource.My_TextBox.Text = totalResource.ToString("###,###,###,###");

            //show ratio resouce
            double ratio = 0;
            ret = planByDateController.GetRatioRequireResource(totalResource, ref ratio);
            txtInterestRequireResource.My_TextBox.Text = String.Format("{0}%", ratio);

         //   FormatDataGridViewDisplay();
            return "";
        }

        ExcelImportStruct[] KeHoachSanXuatTheoNgay_Excel_Struct;
        const int KeHoachSanXuatTheoNgay_INDEX = 0;

        private void Init_KeHoachSanXuatTheoNgay_Excel()
        {
            if (KeHoachSanXuatTheoNgay_Excel_Struct == null)
            {
                KeHoachSanXuatTheoNgay_Excel_Struct = new ExcelImportStruct[26];

                KeHoachSanXuatTheoNgay_Excel_Struct[0] = new ExcelImportStruct(0, "Date", "Date", Excel_Col_Type.COL_DATE, 10, true);
                KeHoachSanXuatTheoNgay_Excel_Struct[1] = new ExcelImportStruct(1, "PartNumber", "PartNumber", Excel_Col_Type.COL_STRING, 20, true);
                KeHoachSanXuatTheoNgay_Excel_Struct[2] = new ExcelImportStruct(2, "Priority", "Priority", Excel_Col_Type.COL_INT, 20, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[3] = new ExcelImportStruct(3, "LineID", "LineID", Excel_Col_Type.COL_STRING, 20, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[4] = new ExcelImportStruct(4, "LineName", "LineName", Excel_Col_Type.COL_STRING, 50, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[5] = new ExcelImportStruct(5, "GroupID", "GroupID", Excel_Col_Type.COL_STRING, 20, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[6] = new ExcelImportStruct(6, "TotalShiftPerLine", "TotalShiftPerLine", Excel_Col_Type.COL_DECIMAL, 50, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[7] = new ExcelImportStruct(7, "Capacity", "Capacity", Excel_Col_Type.COL_INT, 20, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[8] = new ExcelImportStruct(8, "Qty", "Qty", Excel_Col_Type.COL_INT, 20, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[9] = new ExcelImportStruct(9, "NumOfShift", "NumOfShift", Excel_Col_Type.COL_DECIMAL, 20, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[10] = new ExcelImportStruct(10, "NumOfPerson_Per_Day", "NumOfPerson_Per_Day", Excel_Col_Type.COL_INT, 50, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[11] = new ExcelImportStruct(11, "ShiftNamePerLine", "ShiftNamePerLine", Excel_Col_Type.COL_STRING, 100, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[12] = new ExcelImportStruct(12, "SubLine_ID", "SubLine_ID", Excel_Col_Type.COL_STRING, 20, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[13] = new ExcelImportStruct(13, "SubLine_Name", "SubLine_Name", Excel_Col_Type.COL_STRING, 50, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[14] = new ExcelImportStruct(14, "Shift_1_Main", "Shift_1_Main", Excel_Col_Type.COL_BOOL, 50, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[15] = new ExcelImportStruct(15, "Shift_2_Main", "Shift_2_Main", Excel_Col_Type.COL_BOOL, 50, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[16] = new ExcelImportStruct(16, "Shift_3_Main", "Shift_3_Main", Excel_Col_Type.COL_BOOL, 50, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[17] = new ExcelImportStruct(17, "Shift_1_Qty", "Shift_1_Qty", Excel_Col_Type.COL_DECIMAL, 50, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[18] = new ExcelImportStruct(18, "Shift_1_From", "Shift_1_From", Excel_Col_Type.COL_TIME, 50, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[19] = new ExcelImportStruct(19, "Shift_1_To", "Shift_1_To", Excel_Col_Type.COL_TIME, 50, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[20] = new ExcelImportStruct(20, "Shift_2_Qty", "Shift_2_Qty", Excel_Col_Type.COL_DECIMAL, 50, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[21] = new ExcelImportStruct(21, "Shift_2_From", "Shift_2_From", Excel_Col_Type.COL_TIME, 50, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[22] = new ExcelImportStruct(22, "Shift_2_To", "Shift_2_To", Excel_Col_Type.COL_TIME, 50, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[23] = new ExcelImportStruct(23, "Shift_3_Qty", "Shift_3_Qty", Excel_Col_Type.COL_DECIMAL, 50, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[24] = new ExcelImportStruct(24, "Shift_3_From", "Shift_3_From", Excel_Col_Type.COL_TIME, 50, false);
                KeHoachSanXuatTheoNgay_Excel_Struct[25] = new ExcelImportStruct(25, "Shift_3_To", "Shift_3_To", Excel_Col_Type.COL_TIME, 50, false);
            }                  
        }

        private bool DeletePlanForProductionByDate(DateTime date)
        {
            bool result;
            string cmd = @"Delete FROM [P_002_PlanForProductionByDate] 
                            WHERE [Date] = '" + date.ToString("dd MMM yyyy") + "'";
            result = Update_Data_Info(MasterDatabase_Connection_Str, cmd);
            return result;
        }

        DataTable Get_MasterDatabase_Data(DateTime date, string sql_cmd)
        {
            //string sql_cmd = @"SELECT * FROM [OpenPOPlanner] WHERE ActiveDateTime = '" + date.ToString("dd MMM yyyy") + "'";
            //string sql_cmd = @"SELECT DATEADD(day,0,DATEDIFF(day,0,Date)) as Date, PartNumber, SUM(Qty) as Qty, MIN(Priority) as Priority " +
            //                        "FROM P_001_InputFromPlanner " +
            //                        "WHERE DATEADD(day,0,DATEDIFF(day,0,Date))='" + date.Date.ToString("dd MMM yyyy") + "' " +
            //                        "GROUP BY DATEADD(day,0,DATEDIFF(day,0,Date)), PartNumber";
            DataTable temp_dtb = new DataTable();
            DataSet inputData_tbl = new DataSet();
            SqlDataAdapter addapter = new SqlDataAdapter();
            if (temp_dtb != null)
            {
                temp_dtb.Clear();
            }
            temp_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref addapter, ref inputData_tbl);
            return temp_dtb;
        }
    }
}