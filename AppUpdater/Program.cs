using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Octokit;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace AppUpdater
{
	class Program
	{
		static GitHubClient client = new GitHubClient(new ProductHeaderValue("MedocUpdates.AppUpdater"));
		static Release latestRelease;

		static Version remoteVersion;
		static Version localVersion;

		private static void GetRemoteData()
		{
			//var user = await client.User.Get("UAVXP");
			//Console.WriteLine("{0} has {1} public repositories - go check out their profile at {2}",
			//	user.Name,
			//	user.PublicRepos,
			//	user.Url);

			Task<IReadOnlyList<Release>> releases = client.Repository.Release.GetAll("UAVXP", "MedocUpdates");
			latestRelease = releases.Result[0];
			Console.WriteLine(
				"The latest release is tagged at {0} and is named {1}",
				latestRelease.TagName,
				latestRelease.Name);

			//string versionStr = latestRelease.TagName.Substring(1); // Remove "v"
			string versionStr = latestRelease.TagName; // Don't need to remove "v" anymore

			remoteVersion = new Version(versionStr);
			//remoteVersion = new Version("11.01.025");
		}

		private static void GetLocalData()
		{
			FileVersionInfo vinfo = FileVersionInfo.GetVersionInfo("MedocUpdates.exe");
			localVersion = new Version(vinfo.ProductVersion);
			//localVersion = new Version("11.01.020");

			Console.WriteLine("Local MedocUpdates version is " + localVersion);
		}

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

			GetRemoteData();

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
						Update update = new Update(latestRelease);
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

			GetLocalData();

			// Comparing both versions
			switch (remoteVersion.CompareTo(localVersion))
			{
			case 0:
				Console.WriteLine("Versions are equal");
				break;
			case 1:
				Console.WriteLine("Online version is newer");
				// TODO: Trigger the update
				Update update = new Update(latestRelease);
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
