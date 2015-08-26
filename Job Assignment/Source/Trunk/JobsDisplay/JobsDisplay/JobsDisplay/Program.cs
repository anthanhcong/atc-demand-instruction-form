using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LayoutControl;
using JobsDisplay.Statistics;

namespace JobsDisplay
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //Application.Run(new frmMaster());
            // Application.Run(new EmptyWST_vs_Employee());
        }
    }
}
