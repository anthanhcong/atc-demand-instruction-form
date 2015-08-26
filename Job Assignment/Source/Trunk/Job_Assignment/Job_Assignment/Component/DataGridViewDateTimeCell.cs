using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace MasterDatabase
{
    public class DataGridViewDateTimeCell : DataGridViewTextBoxCell
    {
        private DateTimePickerFormat _format;
        private string _customFormat;
        private bool _showUpDown;

        #region Constructor
        public DataGridViewDateTimeCell()
        {
            _format = DateTimePickerFormat.Custom;
            _customFormat = "dd/MM/yyyy";
            _showUpDown = false;
        }
        #endregion
        #region 
        public DateTimePickerFormat Format
        {
            get
            {
                return _format;
            }

            set
            {
                _format = value;
            }
        }
        public string CustomFormat
        {
            get
            {
                return _customFormat;
            }

            set
            {
                _customFormat = value;
            }
        }
        public bool ShowUpDown
        {
            get
            {
                return _showUpDown;
            }

            set
            {
                _showUpDown = value;
            }
        }
        internal void SetShowUpDown(int rowIndex, bool value)
        {
            _showUpDown = value;
            if (OwnsEditingdateTimePicker(rowIndex))
            {
                EditingDateTimePicker.ShowUpDown = value;
            }
        }
        internal void SetFormat(int rowIndex, DateTimePickerFormat value)
        {
            _format = value;
            if (OwnsEditingdateTimePicker(rowIndex))
            {
                EditingDateTimePicker.Format = value;
            }
        }
        internal void SetCustomFormat(int rowIndex, string value)
        {
            Debug.Assert(value != null);
            _customFormat = value;
            if (OwnsEditingdateTimePicker(rowIndex))
            {
                EditingDateTimePicker.CustomFormat = value;
            }
        }
        /// <summary>
        /// Returns the current DataGridView EditingControl as a DataGridViewNumericUpDownEditingControl control
        /// </summary>
        private DateTimePickerEditingControl EditingDateTimePicker
        {
            get
            {
                return this.DataGridView.EditingControl as DateTimePickerEditingControl;
            }
        }
        /// <summary>
        /// Determines whether this cell, at the given row index, shows the grid's editing control or not.
        /// The row index needs to be provided as a parameter because this cell may be shared among multiple rows.
        /// </summary>
        private bool OwnsEditingdateTimePicker(int rowIndex)
        {
            if (rowIndex == -1 || this.DataGridView == null)
            {
                return false;
            }
            DateTimePickerEditingControl dateTimePickerEditingControl = this.DataGridView.EditingControl as DateTimePickerEditingControl;
            return dateTimePickerEditingControl != null && rowIndex == ((IDataGridViewEditingControl)dateTimePickerEditingControl).EditingControlRowIndex;
        }
        /// <summary>
        /// Called when a cell characteristic that affects its rendering and/or preferred size has changed.
        /// This implementation only takes care of repainting the cells. The DataGridView's autosizing methods
        /// also need to be called in cases where some grid elements autosize.
        /// </summary>
        private void OnCommonChange()
        {
            if (this.DataGridView != null && !this.DataGridView.IsDisposed && !this.DataGridView.Disposing)
            {
                if (this.RowIndex == -1)
                {
                    // Invalidate and autosize column
                    this.DataGridView.InvalidateColumn(this.ColumnIndex);

                    // TODO: Add code to autosize the cell's column, the rows, the column headers 
                    // and the row headers depending on their autosize settings.
                    // The DataGridView control does not expose a public method that takes care of this.
                }
                else
                {
                    // The DataGridView control exposes a public method called UpdateCellValue
                    // that invalidates the cell so that it gets repainted and also triggers all
                    // the necessary autosizing: the cell's column and/or row, the column headers
                    // and the row headers are autosized depending on their autosize settings.
                    this.DataGridView.UpdateCellValue(this.ColumnIndex, this.RowIndex);
                }
            }
        }
        #endregion

        #region Overrides
        public override Type EditType
        {
            get
            {
                return typeof(DateTimePickerEditingControl);
            }
        }
        public override Type ValueType
        {
            get
            {
                // Return the type of the value that CalendarCell contains. 
                return typeof(DateTime);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                // Use the current date and time as the default value. 
                return DateTime.Now;
            }
        }
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value. 
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            DateTimePickerEditingControl ctl = DataGridView.EditingControl as DateTimePickerEditingControl;

            if (ctl != null)
            {
                ctl.Format = Format;
                ctl.CustomFormat = CustomFormat;
                ctl.ShowUpDown = ShowUpDown;
                // Use the default row value when Value property is null. 
                if (this.Value == null || this.Value == DBNull.Value)
                {
                    ctl.Value = (DateTime)this.DefaultNewRowValue;
                }
                else
                {
                    ctl.Value = (DateTime)this.Value;
                }
            }
        }

        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, System.ComponentModel.TypeConverter valueTypeConverter, System.ComponentModel.TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            if (value == null || value == DBNull.Value)
            {
                value = String.Empty;
                return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
            }
            DataGridViewDateTimeColumn col = (DataGridViewDateTimeColumn)OwningColumn;
            if (col.Format == DateTimePickerFormat.Custom)
            {
                value = ((DateTime)value).ToString(col.CustomFormat);
            }
            else if (col.Format == DateTimePickerFormat.Long)
                value = ((DateTime)value).ToLongDateString();
            else if (col.Format == DateTimePickerFormat.Short)
                value = ((DateTime)value).ToShortDateString();
            else
                value = ((DateTime)value).ToLongTimeString();
            return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
        }
        #endregion
    }
}
