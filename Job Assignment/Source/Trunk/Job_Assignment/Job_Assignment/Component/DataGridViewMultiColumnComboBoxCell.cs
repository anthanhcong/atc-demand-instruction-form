using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace MasterDatabase
{
    /// <summary>
    /// Displays a DataGridViewMultiColumnComboBoxEditingControl in a <see cref="T:System.Windows.Forms.DataGridView"/> control.
    /// </summary>
    public class DataGridViewMultiColumnComboBoxCell : DataGridViewTextBoxCell
    {
        #region "Member Variables"

        private List<string> _columnNames = new List<string>();
        private List<string> _columnWidths = new List<string>();
        private Color _evenRowsBackColor = SystemColors.Control;
        private Color _oddRowsBackColor = SystemColors.Control;
        private object _dataSource;
        private string _valueMember = "";
        //private string _displayMember = "";
        private string _linkedColumnName = "";
        private int? _linkedColumnIndex = null;
        
        // Constants
        private const String EvenRowsBackColorErrorMsg = "The EvenRowsBackColor property cannot be null.";
        private const String OddRowsBackColorErrorMsg = "The OddRowsBackColor property cannot be null.";

        // Type of this cell's editing control
        private static Type _defaultEditType = typeof(DataGridViewMultiColumnComboBoxEditingControl);

        #endregion

        #region "Properties"
        /// <summary>
        /// Define the type of the cell's editing control
        /// </summary>
        /// <returns>A Type of <see cref="DataGridViewMultiColumnComboBoxEditingControl"/>.</returns>
        public override Type EditType
        {
            get { return _defaultEditType; }
        }
        /// <summary>
        /// The LinkedColumName property replicates the one from the DataGridViewMultiColumnComboBoxEditingControl control
        /// </summary>
        /// <exception cref="T:System.ArgumentNullException">When property is null.</exception>
        public int? LinkedColumnIndex
        {
            get
            {
                return _linkedColumnIndex;
            }
            set
            {
                _linkedColumnIndex = value;
            }
        }
        /// <summary>
        /// The LinkedColumName property replicates the one from the DataGridViewMultiColumnComboBoxEditingControl control
        /// </summary>
        /// <exception cref="T:System.ArgumentNullException">When property is null.</exception>
        public String LinkedColummName 
        {
            get
            {
                return _linkedColumnName;
            }
            set
            {
                _linkedColumnName = value;
                if (this.DataGridView!= null && this.DataGridView.Columns[_linkedColumnName] != null)
                {
                    this.DataGridView.Columns[_linkedColumnName].ReadOnly = true;
                }
            }
        }
        /// <summary>
        /// The ColumnNames property replicates the one from the DataGridViewMultiColumnComboBoxEditingControl control
        /// </summary>
        /// <exception cref="T:System.ArgumentNullException">When property is null.</exception>
        public List<String> ColumnNames
        {
            get
            {
                return _columnNames;
            }
            set
            {
                _columnNames = value ?? new List<string>();
            }
        }

        /// <summary>
        /// The ColumnWidths property replicates the one from the DataGridViewMultiColumnComboBoxEditingControl control
        /// </summary>
        /// <exception cref="T:System.ArgumentNullException">When property is null.</exception>
        public List<String> ColumnWidths
        {
            get
            {
                return _columnWidths;
            }
            set
            {
                _columnWidths = value ?? new List<string>();
            }
        }

        /// <summary>
        /// Gets or sets the background color for the even rows portion of the DataGridViewMultiColumnComboBoxEditingControl control.  The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"/> property.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Drawing.Color"/> that represents the background color of the even rows portion of the DataGridViewMultiColumnComboBoxEditingControl.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">When property is null.</exception>
        public Color EvenRowsBackColor
        {
            get
            {
                return _evenRowsBackColor;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(EvenRowsBackColorErrorMsg);
                }

                _evenRowsBackColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the background color for the odd rows portion of the DataGridViewMultiColumnComboBoxEditingControl control.  The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"/> property.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Drawing.Color"/> that represents the background color of the odd rows portion of the DataGridViewMultiColumnComboBoxEditingControl.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">When property is null.</exception>
        public Color OddRowsBackColor
        {
            get
            {
                return _oddRowsBackColor;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(OddRowsBackColorErrorMsg);
                }
                
                _oddRowsBackColor = value;
            }
        }
        public object DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                _dataSource = value;
            }
        }

        //public String DisplayMember
        //{
        //    get
        //    {
        //        if (_displayMember == null)
        //            _displayMember = _valueMember;

        //        return _displayMember;
        //    }
        //    set
        //    {
        //        _displayMember = value;
        //    }
        //}
        public String ValueMember
        {
            get
            {
                return _valueMember;
            }
            set
            {
                //if (value == null)
                //{
                //    throw new ArgumentNullException(ValueMemberErrorMsg);
                //}

                _valueMember = value;
            }
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Creates an exact copy of this cell, copies all the custom properties.
        /// </summary>
        /// 
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the cloned <see cref="T:DGMCCBD.Controls.DataGridViewMultiColumnComboBoxCell"/>.
        /// </returns>
        public override object Clone()
        {
            var clone = (DataGridViewMultiColumnComboBoxCell)base.Clone();

            // Make sure to copy added properties.
            clone.LinkedColummName = LinkedColummName;
            clone.LinkedColumnIndex = LinkedColumnIndex;
            clone.ColumnNames = ColumnNames;
            clone.ColumnWidths = ColumnWidths;
            clone.EvenRowsBackColor = EvenRowsBackColor;
            clone.OddRowsBackColor = OddRowsBackColor;
            clone.DataSource = DataSource;
            clone.ValueMember = ValueMember;
            //clone.DisplayMember = DisplayMember;

            return clone;
        }

        /// <summary>
        /// Custom implementation of the InitializeEditingControl function. This function is called by the DataGridView control 
        /// at the beginning of an editing session. It makes sure that the properties of the DataGridViewMultiColumnComboBoxEditingControl editing control are 
        /// set according to the cell properties.
        /// </summary>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            
            var editingControl = DataGridView.EditingControl as DataGridViewMultiColumnComboBoxEditingControl;
            // Just return if editing control is null.
            if (editingControl == null) return;
            
            // Set custom properties of Multi Column Combo Box.
            editingControl.DataSource = null;
            editingControl.ValueMember = null;
            editingControl.Items.Clear();
            editingControl.DataSource = DataSource;
            editingControl.ValueMember = ValueMember;
            editingControl.DisplayMember = ValueMember;
            editingControl.DropDownStyle = ComboBoxStyle.DropDown;

            editingControl.LinkedColumnName = LinkedColummName;
            editingControl.LinkedColumnIndex = LinkedColumnIndex;
            editingControl.ColumnNames = ColumnNames;
            editingControl.ColumnWidths = ColumnWidths;
            editingControl.BackColorEven = EvenRowsBackColor;
            editingControl.BackColorOdd = OddRowsBackColor;
            editingControl.OwnerCell = this;
            editingControl.AutoComplete = true;

            string str = initialFormattedValue as string;
            if (str == null)
            {
                str = string.Empty;
            }
            editingControl.SelectedValue  = str;

            //if (Value != null)
            //    editingControl.SelectedValue = Value;
            //else
            //    editingControl.Text = "";

            //if (!AutoComplete) return;

            //editingControl.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //editingControl.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
 

        //protected override bool SetValue(int rowIndex, object value)
        //{
        //    if (OwnsEditingMultiColumnComboBox(rowIndex))
        //    {
        //        return base.SetValue(rowIndex, this.EditingMultiColumnComboBox.SelectedValue);
        //    }
        //    else
        //    {
        //        return base.SetValue(rowIndex, value);
        //    }
        //}
        /// <summary>
        /// Returns a standard textual representation of the cell.
        /// </summary>
        public override string ToString()
        {
            return string.Format("DataGridViewMultiColumnComboBoxCell {{ ColumnIndex={0}, RowIndex={1} }}", ColumnIndex.ToString(CultureInfo.CurrentCulture), RowIndex.ToString(CultureInfo.CurrentCulture));
        }
        /// <summary>
        /// Utility function that sets a new value for the _linkedColumName property of the cell. This function is used by
        /// the cell and column _linkedColumName property. The column uses this method instead of the _linkedColumName
        /// property for performance reasons. This way the column can invalidate the entire column at once instead of 
        /// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
        /// this cell may be shared among multiple rows.
        /// </summary>
        internal void SetLinkedColumnIndex(int rowIndex, int? value)
        {
            _linkedColumnIndex = value;
            if (OwnsEditingMultiColumnComboBox(rowIndex))
            {
                EditingMultiColumnComboBox.LinkedColumnIndex = value;
            }
        }
        /// <summary>
        /// Utility function that sets a new value for the _linkedColumName property of the cell. This function is used by
        /// the cell and column _linkedColumName property. The column uses this method instead of the _linkedColumName
        /// property for performance reasons. This way the column can invalidate the entire column at once instead of 
        /// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
        /// this cell may be shared among multiple rows.
        /// </summary>
        internal void SetLinkedColumName(int rowIndex, string value)
        {
            _linkedColumnName  = value;
            if (OwnsEditingMultiColumnComboBox(rowIndex))
            {
                EditingMultiColumnComboBox.LinkedColumnName = value;
            }
        }
        /// <summary>
        /// Utility function that sets a new value for the ColumnNames property of the cell. This function is used by
        /// the cell and column ColumnNames property. The column uses this method instead of the ColumnNames
        /// property for performance reasons. This way the column can invalidate the entire column at once instead of 
        /// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
        /// this cell may be shared among multiple rows.
        /// </summary>
        internal void SetColumnNames(int rowIndex, List<string> value)
        {
            Debug.Assert(value != null);
            _columnNames = value;
            if (OwnsEditingMultiColumnComboBox(rowIndex))
            {
                EditingMultiColumnComboBox.ColumnNames = value;
            }
        }

        /// <summary>
        /// Utility function that sets a new value for the ColumnWidths property of the cell. This function is used by
        /// the cell and column ColumnWidths property. The column uses this method instead of the ColumnWidths
        /// property for performance reasons. This way the column can invalidate the entire column at once instead of 
        /// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
        /// this cell may be shared among multiple rows.
        /// </summary>
        internal void SetColumnWidths(int rowIndex, List<string> value)
        {
            Debug.Assert(value != null);
            _columnWidths = value;
            if (OwnsEditingMultiColumnComboBox(rowIndex))
            {
                EditingMultiColumnComboBox.ColumnWidths = value;
            }
        }

        /// <summary>
        /// Utility function that sets a new value for the EvenRowsBackColor property of the cell. This function is used by
        /// the cell and column EvenRowsBackColor property. The column uses this method instead of the EvenRowsBackColor
        /// property for performance reasons. This way the column can invalidate the entire column at once instead of 
        /// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
        /// this cell may be shared among multiple rows.
        /// </summary>
        internal void SetEvenRowsBackColor(int rowIndex, Color value)
        {
            Debug.Assert(value != null);
            _evenRowsBackColor = value;
            if (OwnsEditingMultiColumnComboBox(rowIndex))
            {
                EditingMultiColumnComboBox.BackColorEven = value;
            }
        }

        internal void SetDataSource(int rowIndex, object value)
        {
            _dataSource = value;
            if (OwnsEditingMultiColumnComboBox(rowIndex))
            {
                EditingMultiColumnComboBox.DataSource = value;
            }
        }

        internal void SetValueMember(int rowIndex, string value)
        {
            Debug.Assert(value != null);
            _valueMember = value;
            if (OwnsEditingMultiColumnComboBox(rowIndex))
            {
                EditingMultiColumnComboBox.ValueMember = value;
            }
        }
        //internal void SetDisplayMember(int rowIndex, string value)
        //{
        //    Debug.Assert(value != null);
        //    _displayMember = value;
        //    if (OwnsEditingMultiColumnComboBox(rowIndex))
        //    {
        //        EditingMultiColumnComboBox.DisplayMember = value;
        //    }
        //}
        /// <summary>
        /// Utility function that sets a new value for the OddRowsBackColor property of the cell. This function is used by
        /// the cell and column OddRowsBackColor property. The column uses this method instead of the OddRowsBackColor
        /// property for performance reasons. This way the column can invalidate the entire column at once instead of 
        /// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
        /// this cell may be shared among multiple rows.
        /// </summary>
        internal void SetOddRowsBackColor(int rowIndex, Color value)
        {
            Debug.Assert(value != null);
            _oddRowsBackColor = value;
            if (OwnsEditingMultiColumnComboBox(rowIndex))
            {
                EditingMultiColumnComboBox.BackColorOdd = value;
            }
        }

        /// <summary>
        /// Determines whether this cell, at the given row index, shows the grid's editing control or not.
        /// The row index needs to be provided as a parameter because this cell may be shared among multiple rows.
        /// </summary>
        private bool OwnsEditingMultiColumnComboBox(int rowIndex)
        {
            if (rowIndex == -1 || DataGridView == null)
            {
                return false;
            }
            var editingControl = DataGridView.EditingControl as DataGridViewMultiColumnComboBoxEditingControl;
            return editingControl != null && rowIndex == ((IDataGridViewEditingControl)editingControl).EditingControlRowIndex;
        }

        /// <summary>
        /// Returns the current DataGridView EditingControl as a DataGridViewMultiColumnComboBoxEditingControl control
        /// </summary>
        private DataGridViewMultiColumnComboBoxEditingControl EditingMultiColumnComboBox
        {
            get
            {
                return DataGridView.EditingControl as DataGridViewMultiColumnComboBoxEditingControl;
            }
        }

        #endregion

    }
}
