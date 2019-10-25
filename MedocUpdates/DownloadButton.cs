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
using System.Threading;

namespace MedocUpdates
{
	public partial class DownloadButton : UserControl
	{
		MedocDownloadItem item;
		WebClient webclient;

		string zipFilename;
		string updateFilename;

		public bool IsHighlighted { get; set; }
		public string UpdateFilename
		{
			get
			{
				return updateFilename;
			}
		}
		public bool IsUpdating
		{
			get
			{
				return webclient != null;
			}
		}


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

		private void UpdateRoutine()
		{
			if (File.Exists(this.updateFilename))
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

			// TODO: Dispose webclient here probably?

			RunDownload();
		}

		private void llblDownloadRun_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			UpdateRoutine();
		}

		private void RunUpdate()
		{
			if(!File.Exists(this.updateFilename))
				return;

			// The initial update.exe process
			Process proc = new Process();
			proc.StartInfo = new ProcessStartInfo(this.updateFilename);
			proc.Start();
			proc.WaitForExit();

			//FileDownloadedAndRunned.Invoke(this, new EventArgs()); // M.E.Doc is still updating at this point. Need to check the process

			// Wait a few moments before update.exe has spawned again,
			// because we ned to get a new update.exe, not the first one
			Thread.Sleep(5 * 1000); // TODO: Should this be configurable?

			// TODO: Maybe search for "<installation path>\update.exe" (evidences line 1) in M.E.Doc update log?
			// That would mean that update.exe was started for the second time now,
			// and this should be much stable than just thread-sleeping for a constant amount of time

			Process[] updateProcs = Process.GetProcessesByName("update");
			if(updateProcs.Length <= 0)
			{
				Log.Write(LogLevel.EXPERT, String.Format("DownloadButton ({0}): Cannot find update.exe after a short wait", this.item.version));
				MessageBox.Show(String.Format("M.E.Doc version {0} hadn't been installed properly!", this.item.version), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			string installPath = "";
			if(!MedocInternal.GetInstallationPath(out installPath))
			{
				Log.Write(LogLevel.EXPERT, String.Format("DownloadButton ({0}): Cannot get M.E.Doc installation path", this.item.version));
				return;
			}
			
			int procIdx = Array.FindIndex(updateProcs, element => Path.Equals(Path.GetDirectoryName(element.MainModule.FileName), installPath));
			if(procIdx < 0)
			{
				Log.Write(LogLevel.EXPERT, String.Format("DownloadButton ({0}): update.exe didn't appear for the second time of installation", this.item.version));
				MessageBox.Show(String.Format("M.E.Doc version {0} hadn't been installed properly!", this.item.version), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			Process updateProc = updateProcs[procIdx];
			if(updateProc == null)
			{
				Log.Write(LogLevel.EXPERT, String.Format("DownloadButton ({0}): The second update.exe process is corrupted", this.item.version));
				MessageBox.Show(String.Format("M.E.Doc version {0} hadn't been installed properly!", this.item.version), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			//updateProc.Exited += UpdateProc_Exited; // Not working, and also not needed, because we need to block the app until update installation is done
			updateProc.WaitForExit();

			// And only now we can update the main frame
			FileDownloadedAndRunned.Invoke(this, new EventArgs());
		}

		private void UnpackUpdate()
		{
			ZipArchive arch = null;
			try
			{
				// Unzip and run
				arch = ZipFile.OpenRead(this.zipFilename);
			}
			catch(Exception ex)
			{
				Log.Write(LogLevel.NORMAL, String.Format("{0} is corrupted, trying to redownload\r\n{1}", this.zipFilename, ex.Message));
				MessageBox.Show(String.Format("{0} is corrupted, trying to redownload\r\n{1}", this.zipFilename, ex.Message));

				File.Delete(this.zipFilename);
			}

			// Something went wrong with opening an archive.
			if (arch == null)
			{
				RunDownload();
				return;
			}

			//ZipArchiveEntry entry = arch.GetEntry(this.updateFilename);
			//Stream stream = entry.Open();
			arch.ExtractToDirectory(SessionStorage.inside.DownloadsFolderPath);

			arch.Dispose();
		}

		public void RunDownload()
		{
			if(webclient != null)
			{
			//	return;
				webclient.CancelAsync();
				webclient.Dispose();
				webclient = null;
			}

			llblDownloadRun.Enabled = false;

			webclient = new WebClient();
			webclient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
			webclient.DownloadProgressChanged += WebClient_DownloadProgressChanged;

			Uri fileURI = new Uri(item.link);
			webclient.DownloadFileAsync(fileURI, this.zipFilename);

			pbDownloadRun.Visible = true;
		}

		private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if(webclient != null)
				webclient.Dispose();

			UnpackUpdate();

			RunUpdate();

			llblDownloadRun.Enabled = true;
			pbDownloadRun.Visible = false;
		}

		private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			pbDownloadRun.Value = e.ProgressPercentage;
		}
	}
}
