using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedocUpdates
{
	public partial class frmMain : Form
	{
		MedocAPI medoc = new MedocAPI();
		MedocInternal localmedoc = new MedocInternal();
		Log log = new Log();

		public frmMain()
		{
			log.Write("Initializing the main frame");
			InitializeComponent();
		}

		private void Status(string message)
		{
			toolStripStatusLabel1.Text = message;
		}

		private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ShowMainFrame();
		}

		private void trayIcon_MouseClick(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
				ShowMainFrame();
		}

		private void ShowMainFrame()
		{
			this.Visible = true;
			this.Show();

			if(this.CanFocus)
				this.Focus();

			log.Write("Restoring main frame");
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			log.Write("Shutting down the application");
			Application.Exit();
		}

		private void CheckingRoutine()
		{
			// Cleaning up a bit
			flowDownloads.Controls.Clear();

			labelVersion.Text = "Checking...";
			log.Write("Checking for updates on the medoc.ua server");

			bool success = medoc.RefreshDoc();
			if (success)
			{
				string version = medoc.GetLatestVersion();
				//version = "11.01.023";
				labelVersion.Text = "Latest version: " + version;

				//MedocVersion test = new MedocVersion(version);

				//MedocVersion test2 = "11.01.024";
				//MedocVersion test3 = "11.01.023";


				string localversion = localmedoc.LocalVersion;
				//localversion = "11.01.021";
				labelLocalVersion.Text = "Latest local version: " + localversion;

				log.Write(labelVersion.Text);
				log.Write(labelLocalVersion.Text);

				MedocDownloadItem[] items;
				success = medoc.GetItems(out items);
				if (success)
				{
					foreach (MedocDownloadItem item in items)
					{
						DownloadButton btn = new DownloadButton(item);
						btn.IsHighlighted = (item.version > localversion); // TODO
						flowDownloads.Controls.Add(btn);

					//	Console.WriteLine("Added {0}", item.link);
					}
				}

				Status("Done");

				if (localversion != version)
				{
					trayIcon.ShowBalloonTip(5000, "M.E.Doc update has been released!", labelVersion.Text, ToolTipIcon.Info);
				}
			}
			else
			{
				labelVersion.Text = "Something went wrong";
				log.Write("Cannot connect to medoc.ua");
				Status("Cannot connect to medoc.ua");
			}
		}

		private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckingRoutine();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			CheckingRoutine();
		}
	}
}
