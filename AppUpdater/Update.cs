using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Octokit;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.IO.Compression;
using System.Diagnostics;

namespace AppUpdater
{
	public class Update
	{
		private Release release;
		private WebClient webclient;

		private string zipReleaseUrl;
		private string zipFilename;

		public Update(Release release)
		{
			this.release = release;

			string assetUrl = "";
			if(!this.GetLatestZipReleaseAssetUrl(out assetUrl))
				throw new Exception(String.Format("{0}, {1}", release.TagName, release.Url));

			this.zipReleaseUrl = assetUrl;

			this.zipFilename = Path.Combine("downloads", this.zipReleaseUrl.Substring(this.zipReleaseUrl.LastIndexOf('/') + 1));
			if(!Directory.Exists(Path.GetDirectoryName(this.zipFilename)))
				Directory.CreateDirectory(Path.GetDirectoryName(this.zipFilename));
		}

		~Update()
		{
			if (webclient != null)
			{
				webclient.Dispose();
				webclient = null;
			}
		}

		private bool GetLatestZipReleaseAssetUrl(out string assetUrl)
		{
			assetUrl = "";
			IReadOnlyList<ReleaseAsset> assets = this.release.Assets;

			foreach(ReleaseAsset asset in assets)
			{
				if(asset.BrowserDownloadUrl.EndsWith(".zip"))
				{
					assetUrl = asset.BrowserDownloadUrl;
					return true;
				}
			}

			Log.Write(LogLevel.NORMAL, "AppUpdater.Update: Cannot get latest zip release asset url");
			return false;
		}

		public void UpdateRoutine()
		{
			if (File.Exists(this.zipFilename))
			{
				Log.Write("AppUpdater.Update: Archive does exist, unpacking and updating");
				UnpackUpdate();
				RunUpdate();
				return;
			}

			Log.Write(LogLevel.NORMAL, "AppUpdater.Update: Downloading the archive");
			RunDownload();
		}

		private void RunUpdate()
		{
			if (!File.Exists(Path.Combine("downloads", "copyupdate.bat")) ||
				!Directory.Exists(Path.Combine("downloads", "MedocUpdates")))
			{
				Log.Write("AppUpdater.Update: Cannot find all the needed files for update");
				return;
			}

			Log.Write("AppUpdater.Update: Copying the update from the temporary folder");
			Process.Start(Path.Combine("downloads", "copyupdate.bat"));
			Environment.Exit(0); // Exit this program immediately
		}

		private void UnpackUpdate()
		{
			ZipArchive arch = null;
			try
			{
				// Unzip and run
				arch = ZipFile.OpenRead(this.zipFilename);
			}
			catch (Exception ex)
			{
				File.Delete(this.zipFilename);
				Log.Write(LogLevel.NORMAL, "AppUpdater.Update: Update unpacking went wrong\r\n" + ex.Message);
			}

			// Something went wrong with opening an archive.
			if (arch == null)
			{
				Log.Write(LogLevel.NORMAL, "AppUpdater.Update: Archive is null, redownloading the update");
				RunDownload();
				return;
			}

			try
			{
				//ZipArchiveEntry entry = arch.GetEntry(this.updateFilename);
				//Stream stream = entry.Open();

				// NOTE: Throws an exception if one of the files is already exists, and doesn't proceed to extract the rest
				//arch.ExtractToDirectory(Path.GetDirectoryName(this.zipFilename));

				// Manually extract all the files
				string extractPath = Path.GetDirectoryName(this.zipFilename);
				foreach (ZipArchiveEntry entry in arch.Entries)
				{
					string entryPath = Path.GetDirectoryName(entry.FullName);

					if(!Directory.Exists(Path.Combine(extractPath, entryPath)))
						Directory.CreateDirectory(Path.Combine(extractPath, entryPath));

					if (entry.FullName.EndsWith("/")) // That's a directory
						continue;

					entry.ExtractToFile(Path.Combine(extractPath, entry.FullName), true);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.ReadLine();
			}

			arch.Dispose();
		}

		private void RunDownload()
		{
			if (webclient != null)
			{
				//	return;
				webclient.Dispose();
				webclient = null;
			}

			webclient = new WebClient();
			Uri fileURI = new Uri(this.zipReleaseUrl);

			Console.WriteLine("Downloading " + this.zipReleaseUrl);
			webclient.DownloadFile(fileURI, this.zipFilename);

			// Was at WebClient_DownloadFileCompleted previously
			UnpackUpdate();
			RunUpdate();
		}
	}
}
