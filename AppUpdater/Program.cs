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

			var releases = await client.Repository.Release.GetAll("UAVXP", "MedocUpdates");
			var latest = releases[0];
			Console.WriteLine(
				"The latest release is tagged at {0} and is named {1}",
				latest.TagName,
				latest.Name);

			string versionStr = latest.TagName.Substring(1); // Remove "v"
			Version versionOnline = new Version(versionStr);
			//Version versionOnline = new Version("11.01.025");

			FileVersionInfo vinfo = FileVersionInfo.GetVersionInfo("MedocUpdates.exe");
			Version versionLocal = new Version(vinfo.ProductVersion);
			//Version versionLocal = new Version("11.01.020");

			switch (versionOnline.CompareTo(versionLocal))
			{
			case 0:
				Console.WriteLine("Versions are equal");
				break;
			case 1:
				Console.WriteLine("Online version is newer");
				break;
			case -1:
				Console.WriteLine("Local version is newer");
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
