using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using AppUpdater;
using System.Net;

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
			// Windows Forms specific
			Application.ThreadException += Application_ThreadException;
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

			// Non-UI specific
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			System.Reflection.Assembly entryassembly = System.Reflection.Assembly.GetEntryAssembly();
			if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(entryassembly.Location)).Count() > 1)
			{
				MessageBox.Show("Cannot run another instance of this app!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// Fix for Telegram.Bot not being able to do something on Windows Server 2008 R2
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
														SecurityProtocolType.Tls11 |
														SecurityProtocolType.Tls12 |
														SecurityProtocolType.Ssl3;
			}
			catch (Exception ex)
			{
				MessageBox.Show("MedocUpdates: Cannot set the security protocol type - TLS/TLS1.1/TLS1.2/SSL3 probably not supported\r\n" + ex.Message);
				return;
			}

			ParsedArgs.SetArgs(Environment.GetCommandLineArgs());
			ParsedArgs.PrintArgs();

			bool bSettingsWasRestoredFromFile = SessionStorage.Restore();
			TelegramChatsStorage.Restore();

			Log.Init();
			Log.Write("");
			Log.Write(String.Format("MedocUpdates: Initializing version {0}...", entryassembly.GetName().Version));

			string forcedLang = ParsedArgs.GetArgument("forcelanguage");
			if (forcedLang.Trim().Length > 0)
				Loc.Init(forcedLang);
			else
				Loc.Init();

			Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
			//MUVersion.Init();
			//Update update = new Update(MUVersion.latestRelease);
			//update.UpdateRoutine();

			// Crash test zone
			//throw new ArgumentException("The parameter was invalid");
#endif

			if (/*true || */!bSettingsWasRestoredFromFile)
				Application.Run(new frmFirstRun()); // Only show if SessionStorage file doesn't exist

			Application.Run(new frmMain());

			Log.Write("MedocUpdates: Shutting down the application");
			SessionStorage.Save();
		}

		private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			Log.Write(LogLevel.EXPERT, true, "MedocUpdates: Application was crashed in the UI thread\r\n" + e.Exception.Message);

			// TODO: Restart the app
			System.Diagnostics.Process.Start("AppUpdater.exe", "-forcerestart");
			Environment.Exit(2);
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = (Exception)e.ExceptionObject;
			Log.Write(LogLevel.EXPERT, true, String.Format("MedocUpdates: Application was crashed in non-UI thread\r\n{0}\r\n{1}",
															ex.Message, (e.IsTerminating ? "CLR is terminating" : "CLR isn't terminating")));

			// TODO: Restart the app
			System.Diagnostics.Process.Start("AppUpdater.exe", "-forcerestart");
			Environment.Exit(3);
		}
	}
}
