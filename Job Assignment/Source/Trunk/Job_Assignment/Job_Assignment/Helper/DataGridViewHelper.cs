using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MasterDatabase;
using System.Data;

namespace Job_Assignment
{
    public enum DataGridViewColumnType
    {
        TEXT,
        DATE,
        TIME,
        NUMBER,
        COMBOBOX,
        MULTICOMBOBOX,
        CHECKBOX,
    }
    public class DataGridViewHelper
    {
        const int defaultColumnWidth = 50;
        public static DataGridViewColumn CreateColumn( DataGridViewColumnType columnType, String columnName, String columnText)
        {
            DataGridViewColumn col = null;
            switch (columnType)
            {
                case DataGridViewColumnType.MULTICOMBOBOX:
                    col = new DataGridViewMultiColumnComboBoxColumn();
                    break;
                case DataGridViewColumnType.COMBOBOX:
                    col = new DataGridViewComboBoxColumn();
                    break;
                case DataGridViewColumnType.TIME:
                    col = new DataGridViewTimeColumn();
                    break;
                case DataGridViewColumnType.DATE:
                    col = new DataGridViewDateTimeColumn();
                    break;
                case DataGridViewColumnType.CHECKBOX:
                    col = new DataGridViewCheckBoxColumn();
                    break;
                case DataGridViewColumnType.NUMBER:
                default:
                    col = new DataGridViewTextBoxColumn();
                    break;
            }
            col.DataPropertyName = columnName;
            col.Name = columnName;
            col.HeaderText = columnText;
            return col;
        }
        public static void InitColumns(DataGridView dgv, Dictionary<string, DataGridViewColumnType> columnTypes, string[] columnNames, string[] columnTexts, int[] columnWidths, string[] hiddenColumns, string[] readonlyColumns)
        {
            dgv.Columns.Clear();
            for (int i = 0; i < columnNames.Length; i++)
            {
                DataGridViewColumnType type = columnTypes.ContainsKey(columnNames[i]) ? columnTypes[columnNames[i]] :  DataGridViewColumnType.TEXT;
                DataGridViewColumn col = CreateColumn(type, columnNames[i], columnTexts[i]);
                dgv.Columns.Add(col);
                col.Visible = !hiddenColumns.Contains(columnNames[i]);
                col.ReadOnly = readonlyColumns.Contains(columnNames[i]);
                col.Width = i < columnWidths.Length ? columnWidths[i] : defaultColumnWidth;
            }
        }

        public static BindingSource BindingTableToGridView(DataGridView dgv, DataTable tb)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = tb;

            dgv.DataSource = bs;

            return bs;
        }
    }
}
