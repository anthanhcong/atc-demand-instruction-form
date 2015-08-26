using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
namespace Job_Assignment
{
    public class Logger 
    {
        protected static Logger logger = null;
        protected Stream loggerStream = null;
        protected int level = 10;
        protected int indent = 0;
        protected StreamWriter writer = null;

        public Stream LoggerStream
        {
            get
            {
                return loggerStream;
            }
        }

        protected StreamWriter Writer
        {
            get
            {
                return this.writer;
            }
        }

        public static Logger GetInstance()
        {
            ApplicationSetting setting = ApplicationSetting.GetInstance();
            return GetInstance(setting.ApplicationLog);
        }

        public static Logger GetInstance(String source)
        {
            if (logger == null)
            {
                logger = new Logger();

                String fileName = source;
                fileName = fileName.Replace("{yyyyMMdd}", System.DateTime.Today.ToString("yyyyMMdd"));
                fileName = fileName.Replace("{yyyyMM}", System.DateTime.Today.ToString("yyyyMM"));
                fileName = fileName.Replace("{yyyy}", System.DateTime.Today.ToString("yyyy"));
                fileName = fileName.Replace("{MMdd}", System.DateTime.Today.ToString("MMdd"));
                if (!File.Exists(fileName))
                {
                    Directory.CreateDirectory(fileName);
                    Directory.Delete(fileName);
                }

                logger.loggerStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                logger.writer = new StreamWriter(logger.loggerStream);
                logger.writer.AutoFlush = true;

            }
            return logger;
        }

        public void WriteLogData(String function, string s)
        {
            try
            {
                writer.WriteLine(String.Format("{0}:INFO.[{1}] {2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), function, s));
            }
            catch
            {
            }
        }

        public void WriteException(String function, Exception ex)
        {
            try
            {
                while (ex != null)
                {
                    writer.WriteLine(String.Format("{0}:DEBUG.[{1}] {2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), function, ex.Message));
                    ex = ex.InnerException;
                }
            }
            catch
            {
            }
        }
     

    }

}