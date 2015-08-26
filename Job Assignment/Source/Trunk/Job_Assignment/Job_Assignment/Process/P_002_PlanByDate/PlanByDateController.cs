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
    public class PlanByDateController
    {
        const int HOUR_ONE_SHIFT = 8;
        const char SEPARATE_SHIFT_GROUP = '-';
        public readonly string SHIFT_1 = "Shift_1";
        public readonly string SHIFT_2 = "Shift_2";
        public readonly string SHIFT_3 = "Shift_3";
        public static readonly string SHIFT_1_SHIFT_2 = "Shift_1-Shift_2";
        public static readonly string SHIFT_1_SHIFT_3 = "Shift_1-Shift_3";
        public static readonly string SHIFT_1_SHIFT_2_SHIFT_3 = "Shift_1-Shift_2-Shift_3";
        public static readonly string SHIFT_GROUP_TYPE_THREE = "SHIFT_GROUP_TYPE_THREE";
        public static readonly string SHIFT_GROUP_TYPE_ONE = "SHIFT_GROUP_TYPE_ONE";
        public static readonly string SHIFT_GROUP_TYPE_TWO = "SHIFT_GROUP_TYPE_TWO";
        MaterDatabase masterDb;
        DataTable tbLineDescription;

        public PlanByDateController(MaterDatabase _masterDb)
        {
            masterDb = _masterDb;

            SqlDataAdapter sqlAdapterLineDescription = null;
            DataSet dsLineDescription = new DataSet();
            tbLineDescription = masterDb.Get_SQL_Data(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, "select * from MDB_003_Line_Desciption", ref sqlAdapterLineDescription, ref dsLineDescription);
        }
        private String ValidateRow(DataRow row)
        {
            if (String.IsNullOrEmpty(Utils.ObjectToString(row["PartNumber"])))
                return "PartNumber is not empty";
            if (row["Date"] == DBNull.Value)
                return "Date is not empty";
            if (row["Qty"] == DBNull.Value)
                return "Qty is not empty";
            else
            {
                int qty = 0;
                bool b = int.TryParse(row["Qty"].ToString(), out qty);
                if (!b)
                    return "Qty is must number";
            }

            return "";
        }
        public String Calculate(DateTime dateCalculate, ref DataTable inputPlan)
        {
            String errMessage = "";

            try
            {
                inputPlan.PrimaryKey = new DataColumn[] { inputPlan.Columns["Date"], inputPlan.Columns["PartNumber"] };
                ArrayList arrSubLine = new ArrayList();
                for (int i = 0; i < inputPlan.Rows.Count; i++)
                {
                    UpdateRowInformation(ref inputPlan, inputPlan.Rows[i], true);
                    String subLineId = Utils.ObjectToString(inputPlan.Rows[i]["SubLine_ID"]);
                    if (!arrSubLine.Contains(subLineId) && !String.IsNullOrEmpty(subLineId))
                    {
                        arrSubLine.Add(subLineId);
                    }
                }

                // for (int i = 0; i < arrSubLine.Count && String.IsNullOrEmpty(errMessage); i++)
                for (int i = 0; i < arrSubLine.Count; i++)
                {
                    errMessage += FillShiftNameFromAndToTime(dateCalculate, arrSubLine[i].ToString(), ref inputPlan);
                }


            }
            catch (Exception ex)
            {
                errMessage += ex.Message;
                Logger.GetInstance().WriteException("Calculate", ex);
            }
            return errMessage;
        }

        public string UpdateRowInformation(ref DataTable inputPlan, DataRow subLineRow, bool bResetInformation)
        {
            string errMessage = "";
            try
            {
                errMessage = ValidateRow(subLineRow);

                if (!String.IsNullOrEmpty(errMessage))
                {
                    Logger.GetInstance().WriteLogData("UpdateRowInformation", errMessage);
                    return errMessage;
                }
                String partNumber = Utils.ObjectToString(subLineRow["PartNumber"]);
                //DataRow[] subLineRows = inputPlan.Select(String.Format("Date='{0}' and PartNumber='{1}'", dateCalculate.ToString("yyyy-MMM-dd"), partNumber));
                DataRow[] lineDescriptions = tbLineDescription.Select("PartNumber='" + partNumber + "'");
                subLineRow["ShiftNamePerLine"] = DBNull.Value;

                if (lineDescriptions.Length > 0)
                {
                    //DataRow subLineRow = subLineRows[0];
                    DataRow lineDescription = lineDescriptions[0];
                    String subLineID = Utils.ObjectToString(lineDescription["SubLine_ID"]);

                    subLineRow["SubLine_ID"] = subLineID;
                    subLineRow["SubLine_Name"] = lineDescription["SubLine_Name"];
                    subLineRow["LineId"] = lineDescription["LineId"];
                    subLineRow["LineName"] = lineDescription["LineName"];
                    subLineRow["GroupID"] = lineDescription["GroupID"];
                    int maxCapacity = Utils.ObjectToInteger(subLineRow["Capacity"], 0);
                    int maxResource = Utils.ObjectToInteger(lineDescription["MaxResource"], 0);
                    if (bResetInformation)
                    {
                        maxCapacity = Utils.ObjectToInteger(lineDescription["MaxCapacity"], 0);
                    }
                    if (maxCapacity != 0)
                    {
                        subLineRow["Capacity"] = maxCapacity;
                        double numOfShift = Math.Ceiling((1.00 * (int)subLineRow["Qty"] / maxCapacity * 100)) / 100;
                        subLineRow["NumOfShift"] = numOfShift;// Math.Round(1.00 * (int)subLineRow["Qty"] / maxCapacity, 2) + 0.01;
                        subLineRow["NumOfPerson_Per_Day"] = numOfShift * maxResource;
                    }

                    if (!string.IsNullOrEmpty(subLineID))
                    {
                        errMessage = UpdateTotalShiftOnSubLine(ref inputPlan, subLineID);
                    }
                }
                else
                {
                    Logger.GetInstance().WriteLogData("UpdateRowInformation", "Cannot find SubLineId for PartNumber=" + partNumber);
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Logger.GetInstance().WriteException("UpdateRowInformation", ex);
            }

            return errMessage;
        }

        private string UpdateTotalShiftOnSubLine(ref DataTable inputPlan, string subLine_ID)
        {
            decimal totalShiftOnSubLine = 0;
            try
            {
                DataRow[] subLineRows = inputPlan.Select(String.Format("SubLine_ID='{0}'", subLine_ID));
                foreach (DataRow row in subLineRows)
                {
                    totalShiftOnSubLine += Utils.ObjectToDecimal(row["NumOfShift"], 0);
                }
                foreach (DataRow row in subLineRows)
                {
                    row["TotalShiftPerLine"] = totalShiftOnSubLine;
                    row["ShiftNamePerLine"] = GetShiftNameByTotalShiftLine((double)totalShiftOnSubLine);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().WriteException("UpdateTotalShiftOnSubLine", ex);
                return ex.Message;
            }
            return "";
        }
                
        public String FillShiftNameFromAndToTime(DateTime dateCalculate,string subLineId, ref DataTable inputPlan)
        {
            Dictionary<String, ShiftInformationContract> shiftDesc = new Dictionary<string,ShiftInformationContract>();
            String ret = GetShiftDescription(ref shiftDesc);

            if(!String.IsNullOrEmpty(ret))
                return ret;

            DataRow[] subLineRows = inputPlan.Select(String.Format("Date='{0}' and SubLine_ID='{1}'", dateCalculate.ToString("yyyy-MMM-dd"), subLineId))
                                    .OrderBy(x => Utils.ObjectToInteger(x["Priority"], 0)).ToArray();

            //var subLineRows = from s in tempSubLineRows
            //                   orderby Utils.ObjectToInteger(s["Priority"], 0) ascending
            //                   select s;

            if (subLineRows.Count() <= 0)
                return "Cannot find LineID";

            Dictionary<string, int> mainPartIndex = new Dictionary<string, int>();
            Dictionary<string, decimal> currentShiftValue = new Dictionary<string, decimal>();
            mainPartIndex.Add(SHIFT_1, 0);
            mainPartIndex.Add(SHIFT_2, 0);
            mainPartIndex.Add(SHIFT_3, 0);
            currentShiftValue.Add(SHIFT_1, 0);
            currentShiftValue.Add(SHIFT_2, 0);
            currentShiftValue.Add(SHIFT_3, 0);
            try
            {
                decimal totalShiftPerLine = -1;
                String shiftNamePerLine = "";
                if (subLineRows.Length > 0)
                {
                    shiftNamePerLine = Utils.ObjectToString(subLineRows[0]["ShiftNamePerLine"]);
                    totalShiftPerLine = Utils.ObjectToDecimal(subLineRows[0]["TotalShiftPerLine"], -1);
                }
                if (totalShiftPerLine < 0)
                    return "totalShiftPerLine of " + subLineId + " not valid";

                //reset value
                for (int i = 0; i < subLineRows.Length; i++)
                {
                    DataRow lineRow = subLineRows[i];
                    foreach (string shiftColumn in new List<string>() { SHIFT_1, SHIFT_2, SHIFT_3 })
                    {
                        lineRow[shiftColumn + "_From"] = DBNull.Value;
                        lineRow[shiftColumn + "_To"] = DBNull.Value;
                        lineRow[shiftColumn + "_Main"] = DBNull.Value;
                        lineRow[shiftColumn + "_Qty"] = DBNull.Value;
                    }
                }

                Dictionary<String, ShiftSummaryContract> shiftDetailList = GetShiftDetailOnLine(shiftNamePerLine, (double)totalShiftPerLine);

                //calculate
                for (int i = 0; i < subLineRows.Length; i++)
                {
                    DataRow lineRow = subLineRows[i];
                    decimal thisNumOfShiftRemain = Utils.ObjectToDecimal(lineRow["NumOfShift"], -1);

                    foreach (KeyValuePair<String, ShiftSummaryContract> item in shiftDetailList)
                    {
                        string shiftColumn = item.Key;
                        ShiftSummaryContract shiftData = item.Value;

                        if (thisNumOfShiftRemain > 0)//exist shift in ShiftName, 
                        {
                            TimeSpan fromTimeOnShift = shiftData.FromTime;
                            decimal MaxshiftValue = (decimal)shiftData.ValueOnShift;
                            int lastIndex = i - 1;
                            Boolean isNotFullShift = currentShiftValue[shiftColumn] < MaxshiftValue;
                            if (lastIndex >= 0)
                            {
                                if (isNotFullShift && currentShiftValue[shiftColumn] > 0)//exists shift before
                                {
                                    fromTimeOnShift = (TimeSpan)subLineRows[lastIndex][shiftColumn + "_To"];
                                    MaxshiftValue -= currentShiftValue[shiftColumn];
                                }
                                //}
                            }
                            if (isNotFullShift)
                            {
                                decimal valueOnShift = (decimal)Math.Min(MaxshiftValue, thisNumOfShiftRemain);
                                //if (subLineId == "Base_HH02")
                                //{
                                //    int a = 0;

                                //    if (valueOnShift == 0)
                                //    {
                                //        int b = 0;
                                //    }
                                //}

                                double hour = (double)(valueOnShift * HOUR_ONE_SHIFT);
                                TimeSpan toTime = fromTimeOnShift.Add(TimeSpan.FromHours(hour));
                                lineRow[shiftColumn + "_From"] = fromTimeOnShift;
                                lineRow[shiftColumn + "_To"] = new TimeSpan(toTime.Hours, toTime.Minutes, toTime.Seconds);
                                lineRow[shiftColumn + "_Qty"] = valueOnShift;
                                currentShiftValue[shiftColumn] += valueOnShift;
                                thisNumOfShiftRemain -= valueOnShift;
                                //totalShiftRemain -= valueOnShift;
                                
                                if (valueOnShift > Utils.ObjectToDecimal(subLineRows[mainPartIndex[shiftColumn]][shiftColumn + "_Qty"], -1))
                                {
                                    mainPartIndex[shiftColumn] = i;
                                }
                            }
                        }
                    }
                }
                foreach (KeyValuePair<string, int> item in mainPartIndex)
                {
                    if (subLineRows.Length > item.Value)
                    {
                        if (Utils.ObjectToDecimal(subLineRows[item.Value][item.Key + "_Qty"], -1) > 0)//have fill this shift
                        {
                            subLineRows[item.Value][item.Key + "_Main"] = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ret = ex.Message;
                Logger.GetInstance().WriteException("FillShiftNameFromAndToTime", ex);
            }

            return ret;
        }
        //private bool CheckShiftIsOT(String ShiftNameOnLine, string shift, double totalShiftLine)
        //{
        //    bool bOT = false;

        //    if (ShiftNameOnLine == SHIFT_1_SHIFT_2_SHIFT_3)
        //    {
        //        bOT = false;
        //    }
        //    else if (ShiftNameOnLine == SHIFT_1_SHIFT_2)
        //    {
        //        bOT = false;
        //    }
        //    else if (ShiftNameOnLine == SHIFT_1_SHIFT_3)
        //    {
        //        if (totalShiftLine > 2.5)
        //            bOT = true;
        //    }
        //    else
        //    {
        //        if (totalShiftLine > 1)
        //            bOT = true;
        //    }
        //    return bOT;
        //}
        //private TimeSpan GetFromTime(ShiftInformationContract shiftContract, double totalShift, bool isOT)
        //{
        //    //if (shiftName == SHIFT_1)
        //    //    return shiftContract.FromTime;

        //    //if (shiftName == SHIFT_2)
        //    //    return shiftContract.FromTime;

        //    if (shiftContract.ShiftName == SHIFT_3)
        //    {
        //        if (isOT)
        //        {
        //            //get fromTime of Shift3OT to always end at 6:00
        //            //double totalHours = totalShift * HOUR_ONE_SHIFT;
        //            //DateTime dtTemp = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, DateTime.Now.Date.Day);
        //            //dtTemp = dtTemp.Add(shiftContract.ToTime);
        //            //dtTemp = dtTemp.Subtract(TimeSpan.FromHours(totalHours));
        //            //return dtTemp.TimeOfDay;

        //            //change: always Shift3OT start 18:00
        //            return new TimeSpan(18, 0, 0);
        //        }
        //    }

        //    return shiftContract.FromTime;
        //}
                
                
        public String ChangeMainPart(ref DataTable inputPlan, string subLineID, string partNumber, string shiftColumnName, bool value)
        {
            if (value)
                return "";//success but not allow change if thisMainPart=true

            DataRow[] lineRows = inputPlan.Select("PartNumber='" + partNumber + "'");

            if (lineRows == null || lineRows.Length <= 0)
                return "Cannot find partNumber=" + partNumber;

            if (Utils.ObjectToDecimal(lineRows[0][shiftColumnName + "_Qty"], 0) <= 0)
                return "";//success but not allow change if ShiftValue <= 0
           
            lineRows[0][shiftColumnName + "_Main"] = true;

            DataRow[] otherLineRows = inputPlan.Select("SubLine_ID='" + subLineID + "' and partNumber <> '" + partNumber + "'");
            foreach (DataRow row in otherLineRows)
            {
                if (Utils.ObjectToBoolean(row[shiftColumnName + "_Main"], false))
                {
                    row[shiftColumnName + "_Main"] = false;
                }
            }

            return "";
        }
        private Dictionary<String, ShiftSummaryContract> GetShiftDetailOnLine(String ShiftNameOnLine, double totalShiftLine)
        {
            Dictionary<String, ShiftSummaryContract> resultList = new Dictionary<string, ShiftSummaryContract>();
            Dictionary<String, ShiftInformationContract> shiftDesc = new Dictionary<string, ShiftInformationContract>();
            String ret = GetShiftDescription(ref shiftDesc);

            if (!String.IsNullOrEmpty(ret))
                return resultList;

            double val = totalShiftLine;
            if (ShiftNameOnLine == SHIFT_1_SHIFT_2_SHIFT_3)
            {
                val = Math.Min(1.0, totalShiftLine);
                totalShiftLine -= val;
                resultList.Add(SHIFT_1, new ShiftSummaryContract(SHIFT_1, val));
                
                val = Math.Min(1.0, totalShiftLine);
                totalShiftLine -= val;
                resultList.Add(SHIFT_2, new ShiftSummaryContract(SHIFT_2, val));
                
                val = Math.Min(1.0, totalShiftLine);
                totalShiftLine -= val;
                resultList.Add(SHIFT_3, new ShiftSummaryContract(SHIFT_3, val));
            }
            else if (ShiftNameOnLine == SHIFT_1_SHIFT_2)
            {
                val = Math.Min(1.0, totalShiftLine);
                totalShiftLine -= val;
                resultList.Add(SHIFT_1, new ShiftSummaryContract(SHIFT_1, val));

                val = Math.Min(1.0, totalShiftLine);
                totalShiftLine -= val;
                resultList.Add(SHIFT_2, new ShiftSummaryContract(SHIFT_2, val));
            }
            else if (ShiftNameOnLine == SHIFT_1_SHIFT_3)
            {
                if (totalShiftLine < 2)
                    val = Math.Min(1, totalShiftLine);
                else if (totalShiftLine < 2.5)
                    val = Math.Min(totalShiftLine - 1, totalShiftLine);
                else
                    val = Math.Min(1.5, totalShiftLine);

                totalShiftLine -= val;
                resultList.Add(SHIFT_1, new ShiftSummaryContract(SHIFT_1, val));

                val = Math.Min(1.5, totalShiftLine);
                resultList.Add(SHIFT_3, new ShiftSummaryContract(SHIFT_3, val));
            }
            else if (ShiftNameOnLine == SHIFT_1)
            {
                val = Math.Min(1.75, totalShiftLine);
                resultList.Add(SHIFT_1, new ShiftSummaryContract(SHIFT_1, val));
            }
            else if (ShiftNameOnLine == SHIFT_2)
            {
                val = Math.Min(1.5, totalShiftLine);
                resultList.Add(SHIFT_2, new ShiftSummaryContract(SHIFT_2, val));
            }
            else if (ShiftNameOnLine == SHIFT_3)
            {
                val = Math.Min(1.5, totalShiftLine);
                resultList.Add(SHIFT_3, new ShiftSummaryContract(SHIFT_3, val));
            }

            //set from time
            foreach (KeyValuePair<String, ShiftSummaryContract> item in resultList)
            {
                item.Value.FromTime = shiftDesc[item.Key].FromTime;

                if (item.Key == SHIFT_3)
                {
                    if(item.Value.isOT)
                        item.Value.FromTime = new TimeSpan(18, 0, 0); //Shift3OT start 18:00
                }
            }

            return resultList;
        }
        //private double GetMaxShiftValue(String ShiftNameOnLine, string shift, double totalShiftLine)
        //{
        //    if (ShiftNameOnLine == SHIFT_1_SHIFT_2_SHIFT_3)
        //    {
        //        return 1.0;//shift 1 or 2 or 3 equal = 1
        //    }
        //    else if (ShiftNameOnLine == SHIFT_1_SHIFT_2)
        //    {
        //        return 1.0; //NOT OT
        //    }
        //    else //SHIFT_1_SHIFT_3/SHIFT1,2,3
        //    {
        //        if (shift == SHIFT_1)
        //        {
        //            if (totalShiftLine <= 1.5 )
        //            {
        //                return 1.5;
        //            }
        //            else if(totalShiftLine <= 1.75)
        //            {
        //                return 1.75;
        //            }
        //            else if (totalShiftLine < 2)
        //            {
        //                return 1;
        //            }
        //            else if (totalShiftLine < 2.5)
        //            {
        //                return totalShiftLine - 1; //1.5,1.4,1.3,1.2,1.1
        //            }
        //            else
        //            {
        //                return 1.5;
        //            }
                    
        //        }
        //        else if (shift == SHIFT_3)
        //        {
        //            return 1.5;
        //        }
        //        else //SHIFT_2
        //        {
        //            return 1.5;
        //        }
        //    }
        //}
        private String GetShiftNameByTotalShiftLine(double totalShiftLine)
        {
            /*
             case TotalShiftLine 
             * [0 - 1.5]: Shift1
             * (1.5 - 2): not support -> hightlight mau xanh
             * [2 - 3]: Shift1 - Shift3
             * > 3: error
             */
            if (totalShiftLine <= 0)
                return "";

            if (totalShiftLine <= 1.5)
                return SHIFT_1;

            if (totalShiftLine <= 1.75)
                return SHIFT_1;

            if (totalShiftLine < 2)
                return SHIFT_1_SHIFT_2;

            if (totalShiftLine <= 3)
                return SHIFT_1_SHIFT_3;

            if (totalShiftLine > 3)
                return SHIFT_1_SHIFT_3;

            return "";
        }
        //private decimal GetShiftValue(double totalShiftLine)
        //{
        //    double result = 0;
        //    if (totalShiftLine <= 1)
        //        result = 1.0;
        //    else if (totalShiftLine <= 1.5)
        //        result = 1.5;
        //    else if (totalShiftLine < 2)
        //        result = 2;
        //    else if (totalShiftLine <= 2.5)
        //        result = 2.5;
        //    else if (totalShiftLine <= 3)
        //        result = 3;
        //    else if (totalShiftLine <= 3.5)
        //        result = 3.5;
        //    else
        //        result = -1;
        //    return (decimal)result;
        //}
        //public String GetLineRule(DataTable inputPlan, ref Dictionary<String, bool> result)
        //{
        //    result = new Dictionary<string, bool>();
        //    try
        //    {
        //        for (int i = 0; i < inputPlan.Rows.Count; i++)
        //        {
        //            DateTime dt;
        //            bool b = DateTime.TryParse(inputPlan.Rows[i]["Date"].ToString(), out dt);
        //            String partNumber = inputPlan.Rows[i]["PartNumber"] as String;

        //            if (b)
        //            {
        //                String key = String.Format("{0}_{1}", dt.ToString("dd/MM/yyyy"), partNumber);
        //                if (!result.ContainsKey(key))
        //                {
        //                    result.Add(key, false);
        //                }
        //                if (inputPlan.Rows[i]["TotalShiftPerLine"] != null && inputPlan.Rows[i]["TotalShiftPerLine"] != DBNull.Value && (decimal)inputPlan.Rows[i]["TotalShiftPerLine"] > MAX_SHIFT_ON_LINE)
        //                {
        //                    result[key] = true;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return "";
        //}
        public String GetTotalResource(DataTable inputPlan, ref int totalResource)
        {
            try
            {
                totalResource = 0;
                string obj = inputPlan.Compute("sum(NumOfPerson_Per_Day)", "").ToString();
                if (!String.IsNullOrEmpty(obj))
                    totalResource = int.Parse(obj);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().WriteException("GetTotalResource", ex);
                return ex.Message;
            }
            return "";
        }
        public String GetRatioRequireResource(int totalRequireEmployee, ref double result)
        {
            int totalResource = 0;
            String ret = GetTotalResource(ref totalResource);
            // totalResource = 11;
            if (!String.IsNullOrEmpty(ret))
                return ret;

            result = Math.Round((totalRequireEmployee - totalResource) * 1.0 / totalResource * 100, 2);

            if (result < 0)
                result = 0;

            return "";
            //masterDb.get
        }
        public string GetTotalResource(ref int result)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            try
            {
                DataTable tb = masterDb.Get_SQL_Data(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, "select COUNT(distinct Empl_Id) from MDB_002_Empl_Skill", ref adapter, ref ds);
                if (tb != null && tb.Rows.Count > 0)
                {
                    result = int.Parse(tb.Rows[0][0].ToString());
                }
                else
                {
                    return "Cannot get total Resource";
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().WriteException("GetTotalResource", ex);
                return ex.Message;
            }
            return "";
        }

        public DataTable GetShiftGroup(String type)
        {
            DataTable tb = new DataTable();
            tb.Columns.Add("GroupName", typeof(string));

            if (type == SHIFT_GROUP_TYPE_THREE)
            {
                tb.Rows.Add(new object[] { SHIFT_1_SHIFT_2_SHIFT_3});
                tb.Rows.Add(new object[] { SHIFT_1_SHIFT_3 });
                tb.Rows.Add(new object[] { SHIFT_1_SHIFT_2 });
            }
            else if (type == SHIFT_GROUP_TYPE_ONE)
            {
                tb.Rows.Add(new object[] { SHIFT_1 });
                tb.Rows.Add(new object[] { SHIFT_2 });
                tb.Rows.Add(new object[] { SHIFT_3 });
            }
            else
            {
                tb.Rows.Add(new object[] { SHIFT_1_SHIFT_2 });
                tb.Rows.Add(new object[] { SHIFT_1_SHIFT_3 });
            }
            return tb;
        }
        //public DataTable GetOneShiftGroup()
        //{
        //    DataTable tb = new DataTable();
        //    tb.Columns.Add("GroupName");
        //    tb.Rows.Add(new object[] { SHIFT_1 });
        //    tb.Rows.Add(new object[] { SHIFT_2 });
        //    tb.Rows.Add(new object[] { SHIFT_3 });
        //    return tb;
        //}
        public String GetShiftDescription(ref Dictionary<String, ShiftInformationContract> shifts)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            try
            {
                shifts = new Dictionary<string, ShiftInformationContract>();

                DataTable dt = masterDb.Get_SQL_Data(ApplicationSetting.GetInstance().MasterDatabaseConnectionString, "select * from MDB_006_Shift_Description", ref adapter, ref ds);
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return "Error when get ShiftGroup";
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ShiftInformationContract shift = new ShiftInformationContract();
                    shift.ShiftName = Utils.ObjectToString(dt.Rows[i]["ShiftName"]);
                    shift.FromTime = (TimeSpan)dt.Rows[i]["From_Time"];
                    shift.ToTime = (TimeSpan)dt.Rows[i]["To_Time"];
                    shifts.Add(shift.ShiftName, shift);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().WriteException("GetShiftDescription", ex);
                return ex.Message;
            }
            return "";
        }
    }
}