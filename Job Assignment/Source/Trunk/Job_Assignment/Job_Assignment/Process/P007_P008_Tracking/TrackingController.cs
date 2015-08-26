using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using MasterDatabase;
using System.Collections;

namespace Job_Assignment
{
    public class TrackingController
    {
        MSSqlDbFactory db = new MSSqlDbFactory();
        SqlDataAdapter dataAdapterTracking;
        DataSet dsTracking = new DataSet();

        public TrackingController()
        {
        }
        public String GetData(DateTime dtFrom, DateTime dtTo, ref DataTable dt)
        {
            String ret = db.Get_SQL_Data(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, ref dataAdapterTracking, ref dsTracking, CommandType.Text, "select * from P007_P008_Tracking where date between ? and ?", dtFrom.Date, dtTo.Date);
            
            if(String.IsNullOrEmpty(ret))
                dt = dsTracking.Tables[0];

            return ret;
        }
        public String SaveData(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState == DataRowState.Added)
                {
                    row["TrackingType"] = "T";
                    row["ModifyDate"] = DateTime.Now;
                    row["CreateDate"] = DateTime.Now;
                }
                else if (row.RowState == DataRowState.Modified)
                {
                    row["ModifyDate"] = DateTime.Now;
                }
            }
            return db.Update_SQL_Data(dataAdapterTracking, dt);
        }

        public String GetLineDescription(ref DataTable dt)
        {
            dt = new DataTable ();
            return db.OpenDataTable(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, ref dt, CommandType.Text, "select distinct LineID, LineName from MDB_003_Line_Desciption");
        }
        public String GetWorkstationDescription(ref DataTable dt)
        {
            dt = new DataTable();
            return db.OpenDataTable(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, ref dt, CommandType.Text, "select distinct WST_ID, WST_Name from MDB_003_Line_Desciption");
        }
        public String GetEmployeeList(ref DataTable dt)
        {
            dt = new DataTable();
            return db.OpenDataTable(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, ref dt, CommandType.Text, "select Empl_ID, Last_Name as Name from Empl_list");
        }
    }
}