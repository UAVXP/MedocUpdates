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
			this.GetLatestZipReleaseAssetUrl(out assetUrl); // TODO: Check
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

			return false;
		}

		public void UpdateRoutine()
		{
			if (File.Exists(this.zipFilename))
			{
				UnpackUpdate();
				RunUpdate();
				return;
			}

			RunDownload();
		}

		private void RunUpdate()
		{
			if (!File.Exists(Path.Combine("downloads", "copyupdate.bat")) ||
				!Directory.Exists(Path.Combine("downloads", "MedocUpdates")))
			{
				return;
			}

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
			}

			// Something went wrong with opening an archive.
			if (arch == null)
			{
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
