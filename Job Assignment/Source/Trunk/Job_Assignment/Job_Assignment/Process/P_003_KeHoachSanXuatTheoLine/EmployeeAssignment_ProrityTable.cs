using System.Data;
using System;

namespace Job_Assignment
{
    class EmployeeAssignment_ProrityTable
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        DataTable _data = new DataTable();

        public DataTable Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                CheckAndInsertMissingColumn();
            }
        }


        public EmployeeAssignment_ProrityTable()
        {
            ID = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            //_data = null;
        }

        public EmployeeAssignment_ProrityTable(string tableName, string description, DataTable tblData)
        {
            Name = tableName;
            Description = description;

            if (tblData != null)
            {
                _data = tblData;

                //If the input table does not have some collumn ==> Automatic create it
                CheckAndInsertMissingColumn();
            }
        }

        private void CheckAndInsertMissingColumn()
        {
            if (_data == null)
            {
                return;
            }

            //If the input table does not have some collumn ==> Automatic create it and arrange it to right order
            int i = 0;


            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.TABLE_ID_COLUMN, i++, typeof(string));

            //Fill the id of table for all row. So we can use this info when debug later
            foreach (DataRow row in _data.Rows)
            {
                row[ProrityTableCollumn.TABLE_ID_COLUMN] = this.ID;
            }

            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.DATE_COLUMN, i++, typeof(string));
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.FROMTIME_COLUMN, i++, typeof(string));     // new
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.SHIFT_COLUMN, i++, typeof(string));
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.EMPL_COLUMN, i++, typeof(string));
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.EMPL_NAME_COLUMN, i++, typeof(string));
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.POSITION_COLUMN, i++, typeof(string));     // new
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.WST_COLUMN, i++, typeof(string));
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.WST_NAME_COLUMN, i++, typeof(string));
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.LINE_COLUMN, i++, typeof(string));
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.LINE_NAME_COLUMN, i++, typeof(string));
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.GROUP_COLUMN, i++, typeof(string));
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.IS_CHECK_SKILL_COLUMN, i++, typeof(bool));
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.IS_CHECK_WST_COLUMN, i++, typeof(bool));
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.IS_CHECK_LINE, i++, typeof(bool));
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.IS_CHECK_GROUP_COLUMN, i++, typeof(bool));
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.IS_CHECK_ALL_COLUMN, i++, typeof(bool));
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.IS_SWAP_COLUMN, i++, typeof(bool));
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.IS_OPTIMIZE_SHIFT_COLUMN, i++, typeof(bool));
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.IS_STAND, i++, typeof(bool));
            CheckAndInsertColumIfNotExist(ref _data, ProrityTableCollumn.IS_FORCE_GOTO_LINE, i++, typeof(bool));
        }

        private bool CheckAndInsertColumIfNotExist(ref DataTable tbl, string ColumnName, int ColumnOrder, Type type)
        {
            if (tbl == null || ColumnName == string.Empty)
            {
                return false;
            }

            if (tbl.Columns.Contains(ColumnName) == false)
            {
                tbl.Columns.Add(ColumnName, type);
            }

            tbl.Columns[ColumnName].SetOrdinal(ColumnOrder);

            return true;
        }
    }
}
