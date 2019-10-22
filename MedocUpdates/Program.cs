﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedocUpdates
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
			ParsedArgs.SetArgs(Environment.GetCommandLineArgs());
			ParsedArgs.PrintArgs();

			SessionStorage.Restore();

			Log.Init();
			Log.Write("MedocUpdates: Initializing...");

			Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());

			Log.Write("MedocUpdates: Shutting down the application");
			SessionStorage.Save();
		}
    }
}
