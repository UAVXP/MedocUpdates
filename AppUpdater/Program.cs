using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Reflection;

namespace AppUpdater
{
	class Program
	{
		static void Main(string[] args)
		{
			Log.Init();
			Log.Write("");
			Log.Write("AppUpdater: Start");

			// Check if the program is running right now
			Process[] mainAppProcesses = Process.GetProcessesByName("MedocUpdates");
			while (mainAppProcesses.Count() > 0)
			{
				Console.Write("MedocUpdates is still running. Do you want to kill it and continue the updating (Y) or cancel the update (N)? ");

				ConsoleKeyInfo kinfo = Console.ReadKey();
				Console.WriteLine();

				switch (kinfo.Key)
				{
				case ConsoleKey.Y:
					{
						foreach (Process proc in mainAppProcesses)
						{
							proc.CloseMainWindow();
						}

						break;
					}
				case ConsoleKey.N:
					{
						return; // Exit this program
					}
				default:
					break;
				}

				Thread.Sleep(1 * 1000);
				mainAppProcesses = Process.GetProcessesByName("MedocUpdates");
			}

			if(!MUVersion.Init())
			{
				Log.Write(LogLevel.NORMAL, "AppUpdater: Cannot retrieve the latest releases from Github. Check your Internet connection");
				return;
			}

			Version remoteVersion = MUVersion.GetRemoteData();

			// Check if the program is present in current directory
			string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string muExePath = Path.Combine(exePath, "MedocUpdates.exe");
			while (!File.Exists(muExePath))
			{
				Console.Write("Do you want to download the latest version of MedocUpdates (Y) or exit this program (N)? ");

				ConsoleKeyInfo kinfo = Console.ReadKey();
				Console.WriteLine();

				switch (kinfo.Key)
				{
				case ConsoleKey.Y:
					{
						Update update = null;
						try
						{
							update = new Update(MUVersion.LatestRelease);
						}
						catch(Exception ex)
						{
							Log.Write(LogLevel.NORMAL, "AppUpdater (first run): Cannot get the latest zip release asset URL\r\n" + ex.Message);
							return;
						}

						if(update == null)
						{
							Log.Write(LogLevel.NORMAL, "AppUpdater (first run): Cannot find a proper update archive at the latest Github release");
							return;
						}

						update.UpdateRoutine();

						// NOTE: This point cannot be reached

						break;
					}
				case ConsoleKey.N:
					{
						return; // Exit this program
					}
				default:
					break;
				}
			}

			Version localVersion = MUVersion.GetLocalData();

			// Comparing both versions
			switch (remoteVersion.CompareTo(localVersion))
			{
			case 0:
				Log.Write(LogLevel.BASIC, true, "Versions are equal");

				string forceRestart = args.FirstOrDefault(elem => elem.Equals("-forcerestart"));
				if (forceRestart != null && forceRestart.Trim().Length > 0)
				{
					Process.Start("MedocUpdates.exe");
					return; // Exit this program
				}

				break;
			case 1:
				Log.Write(LogLevel.BASIC, true, "Online version is newer");
				
				if(MUVersion.LatestRelease == null)
				{
					Log.Write(LogLevel.NORMAL, "AppUpdater (updating): No Github releases has been loaded");
					return;
				}

				Update update = null;
				try
				{
					update = new Update(MUVersion.LatestRelease);
				}
				catch(Exception ex)
				{
					Log.Write(LogLevel.NORMAL, "AppUpdater (updating): Cannot get the latest zip release asset URL\r\n" + ex.Message);
					return;
				}

				if(update == null)
				{
					Log.Write(LogLevel.NORMAL, "AppUpdater (updating): Cannot find a proper update archive at the latest Github release");
					return;
				}

				update.UpdateRoutine();

				// NOTE: This point cannot be reached

				break;
			case -1:
				Log.Write(LogLevel.BASIC, true, "Local version is newer");
				break;
			default:
				Log.Write(LogLevel.NORMAL,
							String.Format("frmMUUpdates: Cannot compare the local version with the remote. Wrong version format probably ({0}/{1})",
											remoteVersion, localVersion));
				break;
			}

			Log.Write(LogLevel.BASIC, true, "\r\nMedocUpdates has been updated.");
			Console.ReadLine();
		}
	}
}
