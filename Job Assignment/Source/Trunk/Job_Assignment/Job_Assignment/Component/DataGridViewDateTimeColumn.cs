using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace MasterDatabase
{
    public class DataGridViewDateTimeColumn : DataGridViewTextBoxColumn
    {
        #region Constructor
        public DataGridViewDateTimeColumn()
        {
            CellTemplate = new DataGridViewDateTimeCell();
        }
        #endregion

        #region Properties
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                DataGridViewDateTimeCell dataGridViewDateTimeCell = value as DataGridViewDateTimeCell;
                if (value != null && dataGridViewDateTimeCell == null)
                {
                    throw new InvalidCastException("Value provided for CellTemplate must be of type DataGridViewDateTimeCell or derive from it.");
                }
                base.CellTemplate = value;
            }
        }
        private DataGridViewDateTimeCell DateTimeCellTemplate
        {
            get
            {
                return (DataGridViewDateTimeCell)this.CellTemplate;
            }
        }
        [Category("Behavior")]
        [Description("Sets the custom format string for the DateTimePicker")]
        [DefaultValue("dd/MM/yyyy")]
        public string CustomFormat
        {
            get
            {
                if (this.DateTimeCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return this.DateTimeCellTemplate.CustomFormat;
            }
            set
            {
                if (DateTimeCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                // Update the template cell so that subsequent cloned cells use the new value.
                DateTimeCellTemplate.CustomFormat = value;
                if (DataGridView == null) return;

                // Update all the existing DataGridViewMultiColumnComboBoxCell cells in the column accordingly.
                var dataGridViewRows = DataGridView.Rows;
                var rowCount = dataGridViewRows.Count;
                for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    // Be careful not to unshare rows unnecessarily. 
                    // This could have severe performance repercussions.
                    var dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                    var dataGridViewCell = dataGridViewRow.Cells[Index] as DataGridViewDateTimeCell;
                    if (dataGridViewCell != null)
                    {
                        // Call the internal SetColumnNames method instead of the property to avoid invalidation 
                        // of each cell. The whole column is invalidated later in a single operation for better performance.
                        dataGridViewCell.SetCustomFormat(rowIndex, value);
                    }
                }
                DataGridView.InvalidateColumn(Index);
            }
        }

        [Category("Behavior")]
        [Description("Sets the format for the DateTimePicker")]
        [DefaultValue(typeof(DateTimePickerFormat), "4")]
        public DateTimePickerFormat Format
        {
            get
            {
                if (this.DateTimeCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return this.DateTimeCellTemplate.Format;
            }
            set
            {
                if (DateTimeCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                // Update the template cell so that subsequent cloned cells use the new value.
                DateTimeCellTemplate.Format = value;
                if (DataGridView == null) return;

                // Update all the existing DataGridViewMultiColumnComboBoxCell cells in the column accordingly.
                var dataGridViewRows = DataGridView.Rows;
                var rowCount = dataGridViewRows.Count;
                for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    // Be careful not to unshare rows unnecessarily. 
                    // This could have severe performance repercussions.
                    var dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                    var dataGridViewCell = dataGridViewRow.Cells[Index] as DataGridViewDateTimeCell;
                    if (dataGridViewCell != null)
                    {
                        // Call the internal SetColumnNames method instead of the property to avoid invalidation 
                        // of each cell. The whole column is invalidated later in a single operation for better performance.
                        dataGridViewCell.SetFormat(rowIndex, value);
                    }
                }
                DataGridView.InvalidateColumn(Index);
            }
        }

        [Category("Appearance")]
        [Description("If true the DateTimePicker shows the up/down button and not the calander")]
        [DefaultValue(false)]
        public bool ShowUpDown
        {
            get
            {
                if (this.DateTimeCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return this.DateTimeCellTemplate.ShowUpDown;
            }
            set
            {
                if (DateTimeCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                // Update the template cell so that subsequent cloned cells use the new value.
                DateTimeCellTemplate.ShowUpDown = value;
                if (DataGridView == null) return;

                // Update all the existing DataGridViewMultiColumnComboBoxCell cells in the column accordingly.
                var dataGridViewRows = DataGridView.Rows;
                var rowCount = dataGridViewRows.Count;
                for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    // Be careful not to unshare rows unnecessarily. 
                    // This could have severe performance repercussions.
                    var dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                    var dataGridViewCell = dataGridViewRow.Cells[Index] as DataGridViewDateTimeCell;
                    if (dataGridViewCell != null)
                    {
                        // Call the internal SetColumnNames method instead of the property to avoid invalidation 
                        // of each cell. The whole column is invalidated later in a single operation for better performance.
                        dataGridViewCell.SetShowUpDown(rowIndex, value);
                    }
                }
                DataGridView.InvalidateColumn(Index);
            }
        }
        #endregion
    }
}
