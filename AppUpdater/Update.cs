using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Octokit;
using System.Net;
using System.IO;
using System.ComponentModel;

namespace AppUpdater
{
	class Update
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
				//webclient.CancelAsync();
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
		}

		private void UnpackUpdate()
		{
		}

		public void RunDownload()
		{
			if (webclient != null)
			{
				//	return;
				//webclient.CancelAsync();
				webclient.Dispose();
				webclient = null;
			}

			webclient = new WebClient();
			webclient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
			webclient.DownloadProgressChanged += WebClient_DownloadProgressChanged;

			Uri fileURI = new Uri(this.zipReleaseUrl);
			webclient.DownloadFile(fileURI, this.zipFilename);
		}

		private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (webclient != null)
			{
				//webclient.CancelAsync(); // Probably not needed here, but for refactoring reasons I'll leave it
				webclient.Dispose();
				webclient = null;
			}

			UnpackUpdate();

			RunUpdate();
		}

		private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			Console.Write("|");
		}
	}
}
