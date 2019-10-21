using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace MedocUpdates
{
	public partial class DownloadButton : UserControl
	{
		MedocDownloadItem item;
		WebClient webclient;

		string zipFilename;
		string updateFilename;

		public bool IsHighlighted { get; set; }


		public event EventHandler FileDownloadedAndRunned = delegate { };


		public DownloadButton(MedocDownloadItem item)
		{
			InitializeComponent();

			this.item = item;
			this.labelVersion.Text = item.version;

			this.zipFilename = Path.Combine(SessionStorage.inside.DownloadsFolderPath, this.item.link.Substring(this.item.link.LastIndexOf('/') + 1)); // Take the latest part from URL
			this.updateFilename = Path.ChangeExtension(this.zipFilename, ".upd");
		}

		private void DownloadButton_Load(object sender, EventArgs e)
		{
			if (this.IsHighlighted)
			{
				//this.BackColor = SystemColors.Highlight;
				this.BackColor = SystemColors.ActiveCaption;
				llblDownload.LinkColor = Color.Yellow;
				llblDownload.Font = new Font(llblDownload.Font.Name, llblDownload.Font.Size, FontStyle.Bold);
			}
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(item.link); // Open URL in the default browser
		}

		private void llblDownloadRun_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if(File.Exists(this.updateFilename))
			{
				RunUpdate();
				return;
			}

			if (File.Exists(this.zipFilename))
			{
				UnpackUpdate();
				RunUpdate();
				return;
			}

			llblDownloadRun.Enabled = false;

			webclient = new WebClient();
			webclient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
			webclient.DownloadProgressChanged += WebClient_DownloadProgressChanged;

			Uri fileURI = new Uri(item.link);
			webclient.DownloadFileAsync(fileURI, Path.Combine(SessionStorage.inside.DownloadsFolderPath, this.zipFilename));

			pbDownloadRun.Visible = true;
		}

		private void RunUpdate()
		{
			//Process.Start(Path.Combine(SessionStorage.inside.DownloadsFolderPath, this.updateFilename));

			Process proc = new Process();
			proc.StartInfo = new ProcessStartInfo(Path.Combine(SessionStorage.inside.DownloadsFolderPath, this.updateFilename));
			proc.Start();
			proc.WaitForExit();

			FileDownloadedAndRunned.Invoke(this, new EventArgs());
		}

		private void UnpackUpdate()
		{
			// Unzip and run
			ZipArchive arch = ZipFile.OpenRead(this.zipFilename);

			//ZipArchiveEntry entry = arch.GetEntry(this.updateFilename);
			//Stream stream = entry.Open();

			if (!File.Exists(Path.Combine(SessionStorage.inside.DownloadsFolderPath, this.updateFilename)))
				arch.ExtractToDirectory(SessionStorage.inside.DownloadsFolderPath);
		}

		private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			llblDownloadRun.Enabled = true;
			pbDownloadRun.Visible = false;

			UnpackUpdate();

			RunUpdate();
		}

		private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			pbDownloadRun.Value = e.ProgressPercentage;
		}
	}
}
