using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.IO;
using System.IO.Ports;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using MasterDatabase;


namespace Job_Assignment
{
    public partial class Form1 : SQL_APPL
    {
        enum Run_Mode
        {
            DEBUG,
            RELEASE,
        }

        private Run_Mode Get_Run_Mode()
        {
            string file_name = Configure_path + @"\RunMode.ifo";
            string runmode_str;
            Run_Mode runMode = Run_Mode.DEBUG;
            StreamReader sr;
            StreamWriter wr;
            string conection_str;

            if (File.Exists(file_name) == false)
            {
                wr = new StreamWriter(file_name);
                runmode_str = "RELEASE";
                wr.WriteLine(runmode_str);
                wr.Close();
            }

            sr = new StreamReader(file_name);
            runmode_str = sr.ReadLine();

            if ((runmode_str != "DEBUG") && (runmode_str == "RELEASE"))
            {
                sr.Close();
                File.Delete(file_name);
                wr = new StreamWriter(file_name);
                runmode_str = @"RELEASE";
                wr.WriteLine(runmode_str);
                wr.Close();
            }

            if (runmode_str == "DEBUG")
            {
                runMode = Run_Mode.DEBUG;
            }
            else
            {
                runMode = Run_Mode.RELEASE;
            }
            return runMode;
        }

        private string Get_JobAssiment_Connect_str()
        {
            string file_name = Configure_path + @"\JobAssignmentServer.ifo";
            string server_name;
            string database;
            string user;
            string pass;
            StreamReader sr;
            StreamWriter wr;
            string conection_str;

            if (File.Exists(file_name) == false)
            {
                wr = new StreamWriter(file_name);
                server_name = @".\SQLEXPRESS";
                database = @"JOB_ASSIGNMENT_DB";
                user = @"sa2";
                pass = @"anthanhcong";
                wr.WriteLine(server_name);
                wr.WriteLine(database);
                wr.WriteLine(user);
                wr.WriteLine(pass);
                wr.Close();
            }

            sr = new StreamReader(file_name);
            server_name = sr.ReadLine();
            database = sr.ReadLine();
            user = sr.ReadLine();
            pass = sr.ReadLine();

            if ((server_name == null) || (database == null) || (user == null) || (pass == null)
            || (server_name == "") || (database == "") || (user == "") || (pass == ""))
            {
                sr.Close();
                File.Delete(file_name);
                wr = new StreamWriter(file_name);
                server_name = @".\SQLEXPRESS";
                database = @"JOB_ASSIGNMENT_DB";
                user = @"sa2";
                pass = @"anthanhcong";
                wr.WriteLine(server_name);
                wr.WriteLine(database);
                wr.WriteLine(user);
                wr.WriteLine(pass);
                wr.Close();
            }

            if (database != "JOB_ASSIGNMENT_DB")
            {
                Database_Type = "All";
            }
            conection_str = "SERVER=" + server_name + ";DATABASE=" + database + ";UID=" + user + ";PWD=" + pass;
            return conection_str;
        }

        private string Get_LeaveRegister_Connect_str()
        {
            string file_name = Configure_path + @"\ShiftRegisterServer.ifo";
            string server_name;
            string database;
            string user;
            string pass;
            StreamReader sr;
            StreamWriter wr;
            string conection_str;

            if (File.Exists(file_name) == false)
            {
                wr = new StreamWriter(file_name);
                server_name = @".\SQLEXPRESS";
                database = @"SHIFT_REGISTER_DB";
                user = @"sa2";
                pass = @"anthanhcong";
                wr.WriteLine(server_name);
                wr.WriteLine(database);
                wr.WriteLine(user);
                wr.WriteLine(pass);
                wr.Close();
            }

            sr = new StreamReader(file_name);
            server_name = sr.ReadLine();
            database = sr.ReadLine();
            user = sr.ReadLine();
            pass = sr.ReadLine();

            if ((server_name == null) || (database == null) || (user == null) || (pass == null)
            || (server_name == "") || (database == "") || (user == "") || (pass == ""))
            {
                sr.Close();
                File.Delete(file_name);
                wr = new StreamWriter(file_name);
                server_name = @".\SQLEXPRESS";
                database = @"SHIFT_REGISTER_DB";
                user = @"sa2";
                pass = @"anthanhcong";
                wr.WriteLine(server_name);
                wr.WriteLine(database);
                wr.WriteLine(user);
                wr.WriteLine(pass);
                wr.Close();
            }
            conection_str = "SERVER=" + server_name + ";DATABASE=" + database + ";UID="+ user + ";PWD="+ pass;
            return conection_str;
        }

        private string Get_Kitting_Connect_str()
        {
            string file_name = Configure_path + @"\KittingServer.ifo";
            string server_name;
            string database;
            string user;
            string pass;
            StreamReader sr;
            StreamWriter wr;
            string conection_str;

            if (File.Exists(file_name) == false)
            {
                wr = new StreamWriter(file_name);
                server_name = @".\SQLEXPRESS";
                database = @"KittingDatabase";
                user = @"sa2";
                pass = @"anthanhcong";
                wr.WriteLine(server_name);
                wr.WriteLine(database);
                wr.WriteLine(user);
                wr.WriteLine(pass);
                wr.Close();
            }

            sr = new StreamReader(file_name);
            server_name = sr.ReadLine();
            database = sr.ReadLine();
            user = sr.ReadLine();
            pass = sr.ReadLine();

            if ((server_name == null) || (database == null) || (user == null) || (pass == null)
            || (server_name == "") || (database == "") || (user == "") || (pass == ""))
            {
                sr.Close();
                File.Delete(file_name);
                wr = new StreamWriter(file_name);
                server_name = @".\SQLEXPRESS";
                database = @"KittingDatabase";
                user = @"sa2";
                pass = @"anthanhcong";
                wr.WriteLine(server_name);
                wr.WriteLine(database);
                wr.WriteLine(user);
                wr.WriteLine(pass);
                wr.Close();
            }
            conection_str = "SERVER=" + server_name + ";DATABASE=" + database + ";UID=" + user + ";PWD=" + pass;
            return conection_str;
        }

    }
}
