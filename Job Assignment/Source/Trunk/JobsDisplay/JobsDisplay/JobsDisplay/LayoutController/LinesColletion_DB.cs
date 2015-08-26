using System.Data;
using System.Data.SqlClient;
using MasterDatabase;


namespace LayoutControl
{
    public class LinesColletion_DataBase : SQL_APPL
    {
        //public static string MasterDatabase_Connection_Str = @"server=(local)\SQLEXPRESS;database=JOB_ASSIGNMENT_DB;Integrated Security = TRUE;Data Source=PHAMQUANGTHAI\DHO_SQLEXPRESS";
        public static string MasterDatabase_Connection_Str = "";

        public DataTable List_Line_dtb = new DataTable();
        public DataSet List_Line_ds = new DataSet();
        public SqlDataAdapter List_Line_da;

        public DataTable List_WST_dtb = new DataTable();
        public DataSet List_WST_ds = new DataSet();
        public SqlDataAdapter List_WST_da;

        public LinesColletion_DataBase(string conection_str)
        {
            MasterDatabase_Connection_Str = conection_str;
        }

        public DataTable Load_List_of_Line()
        {
            string sql_cmd = @"SELECT distinct [Line_ID],[Line_Name] FROM " + LayoutControlSetting.DatabaseName;

            if (List_Line_dtb != null)
            {
                List_Line_dtb.Clear();
            }
            List_Line_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref List_Line_da, ref List_Line_ds);

            return List_Line_dtb;
        }

        public string FindLineID(string lineName)
        {
            string sql_cmd = @"SELECT distinct [Line_ID]
                                FROM " + LayoutControlSetting.DatabaseName;

            sql_cmd += " WHERE [Line_ID] = '" + lineName + "'";

            if (List_Line_dtb != null)
            {
                List_Line_dtb.Clear();
            }
            List_Line_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref List_Line_da, ref List_Line_ds);

            if (List_Line_dtb.Rows.Count > 0)
            {
                return List_Line_dtb.Rows[0][0].ToString();
            }

            return "";
        }

        public string FindLineName(string lineID)
        {
            string sql_cmd = @"SELECT distinct [Line_Name]
                                FROM " + LayoutControlSetting.DatabaseName;

            sql_cmd += " WHERE [Line_ID] = '" + lineID + "'";

            if (List_Line_dtb != null)
            {
                List_Line_dtb.Clear();
            }
            List_Line_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref List_Line_da, ref List_Line_ds);

            if (List_Line_dtb.Rows.Count > 0)
            {
                return List_Line_dtb.Rows[0][0].ToString();
            }

            return "";
        }

        public DataTable Load_List_of_WST(string line_id)
        {
            string sql_cmd = @"SELECT distinct [WST_ID],[WST_Name],[WST_x],[WST_y],[WST_width],[WST_heigh] 
                                FROM " + LayoutControlSetting.DatabaseName;
            sql_cmd += " WHERE [Line_ID] = '" + line_id + "'";

            if (List_WST_dtb != null)
            {
                List_WST_dtb.Clear();
            }
            List_WST_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref List_WST_da, ref List_WST_ds);
            return List_WST_dtb;
        }

        public DataTable Load_All_LineInfo(string line_id)
        {
            string sql_cmd = @"SELECT * FROM " + LayoutControlSetting.DatabaseName;
            sql_cmd += " WHERE [Line_ID] = '" + line_id + "'";

            if (List_WST_dtb != null)
            {
                List_WST_dtb.Clear();
            }
            List_WST_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref List_WST_da, ref List_WST_ds);
            return List_WST_dtb;
        }

        public void Save_LineInfo(Line line)
        {
            string lineID = FindLineID(line.LineName);

            DataTable tbl = Load_FullLineInfo(lineID);

            //Update datatable with new info
            foreach (DataRow row in tbl.Rows)
            {
                string WST_ID = row["WST_ID"].ToString();

                int x, y, w, h;

                //Get the information of object and update to the table
                if (line.Get_WST_LocationAndSize(WST_ID,out x, out y, out w, out h))
                {
                    row["WST_x"] = x;
                    row["WST_y"] = y;
                    row["WST_width"] = w;
                    row["WST_heigh"] = h;
                }
            }

            Update_SQL_Data(List_Line_da, tbl);
            return;
            
        }

        public DataTable Load_FullLineInfo(string lineID)
        {
            string sql_cmd = @"SELECT * FROM " + LayoutControlSetting.DatabaseName;
            sql_cmd += " WHERE Line_ID = '" + lineID + "'";

            if (List_Line_dtb != null)
            {
                List_Line_dtb.Clear();
            }
            List_Line_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref List_Line_da, ref List_Line_ds);

            return List_Line_dtb;
        }

        //==================== add by thuy
        public DataTable List_Line_Status_dtb = new DataTable();
        public DataSet List_Line_Status_ds = new DataSet();
        public SqlDataAdapter List_Line_Status_da;

        SQL_API.SQL_ATC sqlObj = new SQL_API.SQL_ATC(MasterDatabase_Connection_Str);

        public DataTable Load_List_of_LineStatus()
        {
            //string sql_cmd = @"SELECT distinct LineID FROM [R_009_Line_Status] WHERE LineID is not null and LineID != ''";
            string sql_cmd = @"SELECT distinct LineID FROM MDB_003_Line_Desciption order by LineID";
            if (List_Line_Status_dtb != null)
            {
                List_Line_Status_dtb.Clear();
            }
            List_Line_Status_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref List_Line_Status_da, ref List_Line_Status_ds);

            return List_Line_Status_dtb;
        }

        public void Save_LineInfo_LineStatus(Line line, string lineID, DataTable tbl)
        {
            //Update datatable with new info
            foreach (DataRow row in tbl.Rows)
            {
                string LineID = row["LineID"].ToString();
                string sql_cmd;
                int x, y, w, h;

                //Get the information of object and update to the table
                if (line.Get_WST_LocationAndSize(LineID, out x, out y, out w, out h))
                {
                    row["Line_x"] = x;
                    row["Line_y"] = y;
                    row["Line_width"] = w;
                    row["Line_height"] = h;
                    sql_cmd = "update [R_009_Line_Status] set [Line_x] = '" + x + "', Line_y = '" + y + "', Line_width = '" + w + "', Line_height = '" + h + "'";
                    sql_cmd += " where LineID = '" + LineID + "'";
                    sqlObj.Execute_SQL_CMD(sql_cmd);
                    sqlObj.Save_My_Data();
                }
            }
            return;
        }

        public DataTable Load_FullLineInfo_Status(string lineID)
        {
            string sql_cmd = @"SELECT * FROM R_009_Line_Status ";
            sql_cmd += " WHERE Line_ID = '" + lineID + "'";

            if (List_Line_Status_dtb != null)
            {
                List_Line_Status_dtb.Clear();
            }
            List_Line_Status_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref List_Line_Status_da, ref List_Line_Status_ds);

            return List_Line_Status_dtb;
        }

        public string Load_LineCurrent_Status(string lineid)
        {
            string info = "";
            string sql_cmd = @"SELECT distinct LineID FROM [R_009_Line_Status] 
                                WHERE (LineID is not null and LineID != '') and [Current_Status] = 'True' and LineID = '" + lineid + "'";
            sqlObj.GET_SQL_DATA(sql_cmd);
            if (sqlObj.DaTable != null && sqlObj.DaTable.Rows.Count > 0)
            {
                info = sqlObj.DaTable.Rows[0]["LineID"].ToString().Trim();
            }
            return info;
        }

        public string Load_LineCurrent_Status_plan(string lineid)
        {
            string info = "";
            string sql_cmd = @"SELECT distinct LineID FROM [R_009_Line_Status] 
                                WHERE (LineID is not null and LineID != '') and [Plan_Status] = 'True' and LineID = '" + lineid + "'";
            sqlObj.GET_SQL_DATA(sql_cmd);
            if (sqlObj.DaTable != null && sqlObj.DaTable.Rows.Count > 0)
            {
                info = sqlObj.DaTable.Rows[0]["LineID"].ToString().Trim();
            }
            return info;
        }

        public DataTable List_WST_Status_dtb = new DataTable();
        public DataSet List_WST_Status_ds = new DataSet();
        public SqlDataAdapter List_WST_Status_da;

        public DataTable Load_List_of_WSTStatus(string lineID)
        {
            //string sql_cmd = @"SELECT distinct WST_ID, WST_Name, WST_x, WST_y, WST_width, WST_height FROM [R_009_Line_Status] WHERE LineID = '" + lineID + "'";
            string sql_cmd = @"SELECT distinct WST_ID, WST_Name FROM [R_009_Line_Status] WHERE LineID = '" + lineID + "'";
            if (List_WST_Status_dtb != null)
            {
                List_WST_Status_dtb.Clear();
            }
            List_WST_Status_dtb = Get_SQL_Data(MasterDatabase_Connection_Str, sql_cmd, ref List_WST_Status_da, ref List_WST_Status_ds);

            return List_WST_Status_dtb;
        }

        public bool Load_List_of_WST_Cur(string wst_ID)
        {
            string sql_cmd = @"SELECT * FROM [R_009_Line_Status] WHERE [Current_Status] = 'true' and [Plan_Status]  is null and WST_ID = '" + wst_ID + "'";
            sqlObj.GET_SQL_DATA(sql_cmd);
            if (sqlObj.DaTable != null && sqlObj.DaTable.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool Load_List_of_WST_plan(string wst_ID)
        {
            string sql_cmd = @"SELECT * FROM [R_009_Line_Status] WHERE [Current_Status] is null and [Plan_Status]  = 'true' and WST_ID = '" + wst_ID + "'";
            sqlObj.GET_SQL_DATA(sql_cmd);
            if (sqlObj.DaTable != null && sqlObj.DaTable.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool Load_List_of_WST_plancur(string wst_ID)
        {
            string sql_cmd = @"SELECT * FROM [R_009_Line_Status] WHERE [Current_Status] = 'true' and [Plan_Status]  = 'true' and WST_ID = '" + wst_ID + "'";
            sqlObj.GET_SQL_DATA(sql_cmd);
            if (sqlObj.DaTable != null && sqlObj.DaTable.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool Load_List_of_WST_NO_plancur(string wst_ID)
        {
            string sql_cmd = @"SELECT * FROM [R_009_Line_Status] WHERE [Current_Status] is null and [Plan_Status]  is null and WST_ID = '" + wst_ID + "'";
            sqlObj.GET_SQL_DATA(sql_cmd);
            if (sqlObj.DaTable != null && sqlObj.DaTable.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
