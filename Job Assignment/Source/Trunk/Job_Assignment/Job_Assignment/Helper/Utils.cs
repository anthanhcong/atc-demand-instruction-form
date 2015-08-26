using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Job_Assignment
{
    public class Utils
    {
        public static String[] GetListColumn(DataTable dt)
        {
            String[] arr = new string[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                arr[i] = dt.Columns[i].ColumnName;
            }
            return arr;
        }
        public static double HalfRound(double num)
        {
            int result = (int)(num * 10);
            int div = ((int)(result / 10)) * 10;
            int mod = result % 10;
            result = div + (mod <= 0 ? 0 : (mod <= 5 ? 5 : 10));
            return result * 1.0 / 10;
        }
        public static decimal HalfRound(decimal num)
        {
            return (decimal)Utils.HalfRound((double)num);
        }
        public static String ObjectToString(Object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return "";

            return obj.ToString();
        }
        public static int ObjectToInteger(Object obj, int defaultValue)
        {
            if (obj == null || obj == DBNull.Value)
                return defaultValue;

            try
            {
                return int.Parse(obj.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }
        public static decimal ObjectToDecimal(Object obj, decimal defaultValue)
        {
            if (obj == null || obj == DBNull.Value)
                return defaultValue;

            return (decimal)obj;

        }
        public static DateTime ObjectToDecimal(Object obj, DateTime defaultValue)
        {
            if (obj == null || obj == DBNull.Value)
                return defaultValue;

            return (DateTime)obj;

        }
        public static bool ObjectToBoolean(Object obj, bool defaultValue)
        {
            if (obj == null || obj == DBNull.Value)
                return defaultValue;

            return (bool)obj;

        }
        public static DateTime ObjectToDateTime(Object obj, DateTime defaultValue)
        {
            if (obj == null || obj == DBNull.Value)
                return defaultValue;

            return (DateTime)obj;

        }
        public static void ImportRowToDataTable(ref DataTable dt, DataRow[] rows)
        {
            foreach (DataRow r in rows)
            {
                dt.ImportRow(r);
            }
        }
    }
}
