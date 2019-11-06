using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Octokit;
using System.Diagnostics;

namespace AppUpdater
{
	class Program
	{
		static GitHubClient client = new GitHubClient(new ProductHeaderValue("MedocUpdates.AppUpdater"));

		private static async void GetData()
		{
			//var user = await client.User.Get("UAVXP");
			//Console.WriteLine("{0} has {1} public repositories - go check out their profile at {2}",
			//	user.Name,
			//	user.PublicRepos,
			//	user.Url);

			IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("UAVXP", "MedocUpdates");
			Release latest = releases[0];
			Console.WriteLine(
				"The latest release is tagged at {0} and is named {1}",
				latest.TagName,
				latest.Name);

			//string versionStr = latest.TagName.Substring(1); // Remove "v"
			string versionStr = latest.TagName; // Don't need to remove "v" anymore

			Version versionOnline = new Version(versionStr);
			//Version versionOnline = new Version("11.01.025");

			FileVersionInfo vinfo = FileVersionInfo.GetVersionInfo("MedocUpdates.exe");
			Version versionLocal = new Version(vinfo.ProductVersion);
			//Version versionLocal = new Version("11.01.020");

			Console.WriteLine("MedocUpdates version is " + versionLocal);

			switch (versionOnline.CompareTo(versionLocal))
			{
			case 0:
				Console.WriteLine("Versions are equal");

				Console.WriteLine("You can proceed by pressing any key now...");
				Process.Start("MedocUpdates.exe");
				break;
			case 1:
				Console.WriteLine("Online version is newer");
				// TODO: Trigger the update


				Console.WriteLine("You can proceed by pressing any key now...");
				Process.Start("MedocUpdates.exe");
				break;
			case -1:
				Console.WriteLine("Local version is newer");

				Console.WriteLine("You can proceed by pressing any key now...");
				Process.Start("MedocUpdates.exe");
				break;
			default:
				Console.WriteLine("Something went wrong");
				break;
			}
		}

		static void Main(string[] args)
		{
			GetData();

			Console.ReadLine();
		}
	}
}
