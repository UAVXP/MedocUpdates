using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Octokit;
using System.Diagnostics;
using System.Net;

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

		public static async Task<Release> Init()
		{
			if (client == null)
			{
				Log.Write(LogLevel.EXPERT, true, "AppUpdater.MUVersion: Cannot create GithubClient object");
				return null;
			}

			if(client.Repository == null)
			{
				Log.Write(LogLevel.EXPERT, true, "AppUpdater.MUVersion: client.Repository == null");
				return null;
			}

			if (client.Repository.Release == null)
			{
				Log.Write(LogLevel.EXPERT, true, "AppUpdater.MUVersion: client.Repository.Release == null");
				return null;
			}

			// Fixes the "Could not create SSL/TLS secure channel." on Windows Server 2008 R2
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol =	SecurityProtocolType.Tls |
														SecurityProtocolType.Tls11 |
														SecurityProtocolType.Tls12 |
														SecurityProtocolType.Ssl3;
			}
			catch(Exception ex)
			{
				Log.Write(LogLevel.EXPERT, true,
							"AppUpdater.MUVersion: Cannot set the security protocol type - TLS/TLS1.1/TLS1.2/SSL3 probably not supported\r\n" + ex.Message);
				return null;
			}

			//IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("UAVXP", "MedocUpdates");

			Task<Release> release = client.Repository.Release.GetLatest("UAVXP", "MedocUpdates");
			//Release release = await client.Repository.Release.GetLatest("UAVXP", "MedocUpdates");
			if (release == null)
			{
				Log.Write(LogLevel.EXPERT, true, "AppUpdater.MUVersion: Cannot get a release list from the Github. Probably Internet was down");
				return null;
			}

			try
			{
				//latestRelease = releases.Result[0];
				//latestRelease = releases[0];
				latestRelease = await release;
				//latestRelease = release;
				//latestRelease = release.Result;
			}
			catch(Exception ex)
			{
				Log.Write(LogLevel.EXPERT, true, "AppUpdater.MUVersion: latestRelease task failed\r\n" + ex.Message);

				// FIXME: Ugly solution
				if(ex.InnerException != null)
				{
					Log.Write(LogLevel.EXPERT, true, "\t" + ex.InnerException.Message);

					if (ex.InnerException.InnerException != null)
					{
						Log.Write(LogLevel.EXPERT, true, "\t" + ex.InnerException.InnerException.Message);

						if (ex.InnerException.InnerException.InnerException != null)
							Log.Write(LogLevel.EXPERT, true, "\t" + ex.InnerException.InnerException.InnerException.Message);
					}
				}

				return null;
			}

			Log.Write(LogLevel.BASIC, true, String.Format("The latest release is tagged at {0} and is named {1}",
										latestRelease.TagName, latestRelease.Name));

			return latestRelease;
		}

		public static Version GetRemoteData()
		{
			//string versionStr = latestRelease.TagName.Substring(1); // Remove "v"
			string versionStr = latestRelease.TagName; // Doesn't need to remove "v" anymore

			Version remoteVersion = new Version(versionStr);
			//remoteVersion = new Version("11.01.025");

			Log.Write(LogLevel.BASIC, true, "Remote MedocUpdates version is " + remoteVersion);

			return remoteVersion;
		}

		public static Version GetLocalData()
		{
			FileVersionInfo vinfo = FileVersionInfo.GetVersionInfo("MedocUpdates.exe");
			Version localVersion = new Version(vinfo.ProductVersion);
			//localVersion = new Version("11.01.020");

			Log.Write(LogLevel.BASIC, true, "Local MedocUpdates version is " + localVersion);

			return localVersion;
		}
	}
}
