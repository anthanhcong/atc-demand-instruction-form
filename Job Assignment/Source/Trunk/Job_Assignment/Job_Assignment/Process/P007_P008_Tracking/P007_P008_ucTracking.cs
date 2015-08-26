using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MasterDatabase;
using DataGridViewAutoFilter;
using System.IO;

namespace Job_Assignment
{
    public partial class P007_P008_ucTracking : UserControl
    {
        public P007_P008_ucTracking(ToolStripProgressBar _pb1, ToolStripStatusLabel _stt1, ToolStripStatusLabel _stt2)
        {
            InitializeComponent();
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            ProgressBar1 = _pb1;
            Status_1 = _stt1;
            Status_2 = _stt2;
        }
        ToolStripProgressBar ProgressBar1;
        ToolStripStatusLabel Status_1;
        ToolStripStatusLabel Status_2;
        TrackingController contrl = new TrackingController();
        string[] hiddenColumns = { "Id", "TrackingType", "ModifyDate", "CreateDate"};
        string[] readonlyColumns = { "LineName", "WST_Name", "Empl_Name" };
        int[] columnWidths = new int[] {10, 75, 100, 100, 120, 100, 100, 120, 150, 50, 100, 100, 100  };
        Dictionary<String, DataGridViewColumnType> columnTypes = new Dictionary<string, DataGridViewColumnType>();
        DataTable datasourceTracking = new DataTable();
        DataTable datasourceLine = null;
        DataTable datasourceEmployee = null;
        DataTable datasourceWts = null;

        private void P007_P008_ucTracking_Load(object sender, EventArgs e)
        {
            dgvTracking.AutoGenerateColumns = false;
            LoadData();
        }
        private String InitTrackingGridView(DataTable dt)
        {
            String ret = "";
            string[] columns = Utils.GetListColumn(dt);
            columnTypes.Add("LineID", DataGridViewColumnType.MULTICOMBOBOX);
            columnTypes.Add("Empl_ID", DataGridViewColumnType.MULTICOMBOBOX);
            columnTypes.Add("WST_ID", DataGridViewColumnType.MULTICOMBOBOX);
            //columnTypes.Add("From_Time", DataGridViewColumnType.TIME);
            columnTypes.Add("Date", DataGridViewColumnType.DATE);
            DataGridViewHelper.InitColumns(dgvTracking, columnTypes, columns, columns, columnWidths, hiddenColumns, readonlyColumns);


            //Init datasource for LineId
            ret = contrl.GetLineDescription(ref datasourceLine);
            if (String.IsNullOrEmpty(ret))
            {
                DataGridViewMultiColumnComboBoxColumn col = dgvTracking.Columns["LineID"] as DataGridViewMultiColumnComboBoxColumn;
                col.DataSource = datasourceLine;
                col.ValueMember = "LineID";
                col.DataPropertyName = "LineID";
                col.ColumnWidths = new List<string>() { "75", "200" };
            }

            //set col empl
            ret = contrl.GetEmployeeList(ref datasourceEmployee);
            if (String.IsNullOrEmpty(ret))
            {
                DataGridViewMultiColumnComboBoxColumn col = dgvTracking.Columns["Empl_ID"] as DataGridViewMultiColumnComboBoxColumn;
                col.DataSource = datasourceEmployee;
              //  col.Width = 100;
                col.ValueMember = "Empl_ID";
                col.DataPropertyName = "Empl_ID";
                col.ColumnWidths = new List<string>() { "75", "200"};
            }
            //set col workstation
            ret = contrl.GetWorkstationDescription(ref datasourceWts);
            if (String.IsNullOrEmpty(ret))
            {
                DataGridViewMultiColumnComboBoxColumn col = dgvTracking.Columns["WST_ID"] as DataGridViewMultiColumnComboBoxColumn;
                col.DataSource = datasourceWts;
                col.ValueMember = "WST_ID";
                col.DataPropertyName = "WST_ID";
                col.ColumnWidths = new List<string>() { "75", "200" };
            }

            return ret;
        }
        private void LoadData()
        {
            String errs = contrl.GetData(dtpFrom.Value, dtpTo.Value, ref datasourceTracking);
            if (String.IsNullOrEmpty(errs))
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = datasourceTracking;
                dgvTracking.DataSource = bs;
                if (dgvTracking.Columns.Count == 0)
                {
                    InitTrackingGridView(datasourceTracking);
                }
            }
            else
            {
                MessageBox.Show(errs, "Message");
            }
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            DataTable inputTable = ((BindingSource)dgvTracking.DataSource).DataSource as DataTable;
            String errs = contrl.SaveData(inputTable);
            if (String.IsNullOrEmpty(errs))
            {
                MessageBox.Show("Save data sucessfull", "Message");
            }
            else
            {
                MessageBox.Show(errs, "Message");
            }
        }

        //private DataRow getDataTableRow()
        //{
        //    BindingSource bs = (BindingSource)dgvTracking.DataSource;
        //    DataTable inputTable = bs.DataSource as DataTable;
        //    var drv = bs. as DataRowView;
        //}

        private void btDuplicate_Click(object sender, EventArgs e)
        {
            BindingSource bs = (BindingSource)dgvTracking.DataSource;
            DataTable inputTable = bs.DataSource as DataTable;
            var drv = bs.Current as DataRowView;
            if (drv != null && drv.Row != null)
            {
                int currentIndex = inputTable.Rows.IndexOf(drv.Row);
                if (currentIndex != -1)
                {
                    DataRow toInsert = inputTable.NewRow();
                    toInsert.ItemArray = drv.Row.ItemArray.Clone() as object[];
                    if (currentIndex + 1 < inputTable.Rows.Count)
                    {
                        inputTable.Rows.InsertAt(toInsert, currentIndex + 1);
                    }
                    else
                    {
                        inputTable.Rows.Add(toInsert);
                    }
                }
            }
        }

        private void dgvTracking_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if ("LineID".Equals(dgvTracking.Columns[e.ColumnIndex].Name))
                {
                    string lineId = dgvTracking[e.ColumnIndex, e.RowIndex].Value as string;
                    DataRow[] searchRows = datasourceLine.Select("LineID='" + lineId + "'");
                    if (searchRows.Length > 0)
                    {
                        dgvTracking[e.ColumnIndex + 1, e.RowIndex].Value = searchRows[0]["LineName"];
                    }
                }
                else if ("Empl_ID".Equals(dgvTracking.Columns[e.ColumnIndex].Name))
                {
                    string employId = dgvTracking.Rows[e.RowIndex].Cells["Empl_ID"].Value as string;
                    DataRow[] searchRows = datasourceEmployee.Select("Empl_ID ='" + employId + "'");
                    if (searchRows.Length > 0)
                    {
                        dgvTracking[e.ColumnIndex + 1, e.RowIndex].Value = searchRows[0]["Empl_Name"];
                    }
                }
                else if ("WST_ID".Equals(dgvTracking.Columns[e.ColumnIndex].Name))
                {
                    string WstId = dgvTracking.Rows[e.RowIndex].Cells["WST_ID"].Value as string;
                    DataRow[] searchRows = datasourceWts.Select("WST_ID='" + WstId + "'");
                    if (searchRows.Length > 0)
                    {
                        dgvTracking[e.ColumnIndex + 1, e.RowIndex].Value = searchRows[0]["WST_Name"];
                    }
                }
            }
        }

        private void dgvTracking_BindingContextChanged(object sender, EventArgs e)
        {
            if (dgvTracking.DataSource == null) return;

            foreach (DataGridViewColumn col in dgvTracking.Columns)
            {
                col.HeaderCell = new DataGridViewAutoFilterColumnHeaderCell(col.HeaderCell);
            }
            //dgvTracking.AutoResizeColumns();
        }

        private void btExportExcel_Click(object sender, EventArgs e)
        {
            DataTable inputTable = ((BindingSource)dgvTracking.DataSource).DataSource as DataTable;
            String ret = contrl.SaveData(inputTable);
            if (!String.IsNullOrEmpty(ret))
            {
                MessageBox.Show(ret, "Thông báo");
                return;
            }
            SaveFileDialog save_diaglog = new SaveFileDialog();
            save_diaglog.Filter = "Excel file (*.xlsx;*.xls)|*.xlsx;*.xls|All files (*.*)|*.*";
            if (save_diaglog.ShowDialog() == DialogResult.OK)
            {
                String file_name = save_diaglog.FileName;
                String fInfo = new FileInfo(save_diaglog.FileName).Extension;
                if ((fInfo == ".xlsx") || (fInfo == ".xls"))
                {
                    ret = ExcelHelper.ExportGridviewToExcel(file_name, fInfo, "Tracking", dgvTracking, ProgressBar1, Status_1, Status_2);
                }
                if (String.IsNullOrEmpty(ret))
                {
                    MessageBox.Show("Export File thành công", "Thông báo");
                }
                else
                {
                    MessageBox.Show(ret, "Thông báo");
                }
            }
        }

        private void dgvTracking_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1)
            {
                dgvTracking.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
                dgvTracking.EndEdit();
            }
            else
            {
                dgvTracking.EditMode = DataGridViewEditMode.EditOnEnter;
                dgvTracking.BeginEdit(false);
            }
        }

        private void btDeleteRow_Click(object sender, EventArgs e)
        {
            if (dgvTracking.CurrentCell != null)
            {
                dgvTracking.Rows.RemoveAt(dgvTracking.CurrentCell.RowIndex);
            }
        }

        //private void dgvTracking_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{

        //}
    }
}
