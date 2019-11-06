using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Threading;
using System.IO;

namespace AppUpdater
{
	class Program
	{
		static void Main(string[] args)
		{
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

			MUVersion.Init();

			Version remoteVersion = MUVersion.GetRemoteData();

			// Check if the program is present in current directory
			while (!File.Exists("MedocUpdates.exe")) // TODO: Proper full path
			{
				// TODO: Prompt if user wants to download the latest version of an app
				Console.Write("Do you want to download the latest version of MedocUpdates (Y) or exit this program (N)? ");

				ConsoleKeyInfo kinfo = Console.ReadKey();
				Console.WriteLine();

				switch (kinfo.Key)
				{
				case ConsoleKey.Y:
					{
						Update update = new Update(MUVersion.latestRelease);
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
				Console.WriteLine("Versions are equal");

				string forceRestart = args.FirstOrDefault(elem => elem.Equals("-forcerestart"));
				if (forceRestart != null && forceRestart.Trim().Length > 0)
				{
					Process.Start("MedocUpdates.exe");
					return; // Exit this program
				}

				break;
			case 1:
				Console.WriteLine("Online version is newer");
				// TODO: Trigger the update
				Update update = new Update(MUVersion.latestRelease);
				update.UpdateRoutine();

				// NOTE: This point cannot be reached

				break;
			case -1:
				Console.WriteLine("Local version is newer");
				break;
			default:
				Console.WriteLine("Something went wrong");
				break;
			}

			Console.WriteLine("\r\nMedocUpdates is updated.");
			Console.ReadLine();
		}
	}
}
