using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Job_Assignment
{
    public class ApplicationSetting
    {
        private static ApplicationSetting me = null;
        public ApplicationSetting()
        {
        }
        public static ApplicationSetting GetInstance()
        {
            if (me == null)
            {
                me = new ApplicationSetting();
            }
            return me;
        }
        public String ApplicationLog
        {
            get { return "" + ConfigurationManager.AppSettings["ApplicationLog"]; }
            set { ConfigurationManager.AppSettings["ApplicationLog"] = value; }
        }
        public String MasterDatabaseConnectionString
        {
            get;
            set;
            //get { return "" + ConfigurationManager.AppSettings["MasterDatabaseConnectionString"]; }
            //set { ConfigurationManager.AppSettings["MasterDatabaseConnectionString"] = value; }
        }
        public String LeaveRegisterConnectionString
        {
            get;
            set;
            //get { return "" + ConfigurationManager.AppSettings["LeaveRegisterConnectionString"]; }
            //set { ConfigurationManager.AppSettings["LeaveRegisterConnectionString"] = value; }
        }
    }
}
