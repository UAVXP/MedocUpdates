using System;
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
			System.Reflection.Assembly entryassembly = System.Reflection.Assembly.GetEntryAssembly();
			if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(entryassembly.Location)).Count() > 1)
			{
				MessageBox.Show("Cannot run another instance of this app!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			Loc.Init("ru");

			ParsedArgs.SetArgs(Environment.GetCommandLineArgs());
			ParsedArgs.PrintArgs();

			bool bSettingsWasRestoredFromFile = SessionStorage.Restore();

			Log.Init();
			Log.Write("");
			Log.Write(String.Format("MedocUpdates: Initializing version {0}...", entryassembly.GetName().Version));

			Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			if(/*true || */!bSettingsWasRestoredFromFile)
				Application.Run(new frmFirstRun()); // Only show if SessionStorage file doesn't exist

			Application.Run(new frmMain());

			Log.Write("MedocUpdates: Shutting down the application");
			SessionStorage.Save();
		}
    }
}
