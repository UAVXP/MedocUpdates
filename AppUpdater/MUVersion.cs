using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Octokit;
using System.Diagnostics;

namespace AppUpdater
{
	public class MUVersion
	{
		static GitHubClient client = new GitHubClient(new ProductHeaderValue("MedocUpdates.AppUpdater"));
		static Release latestRelease;

		public static Release LatestRelease
		{
			get
			{
				return latestRelease;
			}
		}

		public static void Init()
		{
			Task<IReadOnlyList<Release>> releases = client.Repository.Release.GetAll("UAVXP", "MedocUpdates");
			latestRelease = releases.Result[0];

			Console.WriteLine(
				"The latest release is tagged at {0} and is named {1}",
				latestRelease.TagName,
				latestRelease.Name);
		}

		public static Version GetRemoteData()
		{
			//string versionStr = latestRelease.TagName.Substring(1); // Remove "v"
			string versionStr = latestRelease.TagName; // Don't need to remove "v" anymore

			Version remoteVersion = new Version(versionStr);
			//remoteVersion = new Version("11.01.025");

			return remoteVersion;
		}

		public static Version GetLocalData()
		{
			FileVersionInfo vinfo = FileVersionInfo.GetVersionInfo("MedocUpdates.exe");
			Version localVersion = new Version(vinfo.ProductVersion);
			//localVersion = new Version("11.01.020");

			Console.WriteLine("Local MedocUpdates version is " + localVersion);

			return localVersion;
		}
	}
}
