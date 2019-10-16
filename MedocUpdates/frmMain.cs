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
		MedocTelegram telegram = new MedocTelegram();

		public frmMain()
		{
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

		private void trayIcon_BalloonTipClicked(object sender, EventArgs e)
		{
			ShowMainFrame();
		}

		private void ShowMainFrame()
		{
			this.Visible = true;

			if(this.WindowState == FormWindowState.Minimized)
				this.WindowState = FormWindowState.Normal;

			this.Show();

			if(this.CanFocus)
				this.Focus();

			Log.Write(LogLevel.NORMAL, "Restoring main frame");
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void CheckingRoutine()
		{
			// Cleaning up a bit
			flowDownloads.Controls.Clear();

			labelVersion.Text = "Checking...";
			Log.Write("Checking for updates on the medoc.ua server");

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

				Log.Write(labelVersion.Text);
				Log.Write(labelLocalVersion.Text);

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
#if !DEBUG
				//	telegram.SendMessageAll(labelVersion.Text); // FIXME: Uncomment this
#endif
				}

#if DEBUG
				// FIXME: Sending this everytime just to make sure Telegram connection is working
				telegram.SendMessageAll(labelVersion.Text);
#endif
			}
			else
			{
				labelVersion.Text = "Something went wrong";
				Log.Write("Cannot connect to medoc.ua");
				Status("Cannot connect to medoc.ua");
			}
		}

		private void TimerRoutine()
		{
			timerUpdate.Interval = (int)SessionStorage.inside.NotificationDelay + 1; // Wow, first hack

			if (SessionStorage.inside.NotificationDelay == 0)
				timerUpdate.Stop();
			else
				timerUpdate.Start();
		}

		private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckingRoutine();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			CheckingRoutine();

			TimerRoutine();

			SessionStorage.NotificationDelayChanged += frmMain_NotificationDelayChanged;
		}

		private void timerUpdate_Tick(object sender, EventArgs e)
		{
			CheckingRoutine();

			TimerRoutine();
		}

		private void frmMain_NotificationDelayChanged(object sender, EventArgs e)
		{
			TimerRoutine();

			SessionStorage.Save();
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmSettings settings = new frmSettings();
			settings.ShowDialog();
		}

		private void delayNotificationsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmNotificationsDelay delay = new frmNotificationsDelay();
			delay.Delay = SessionStorage.inside.NotificationDelay; // timerUpdate.Interval
			delay.ShowDialog();
			Console.WriteLine(SessionStorage.inside.NotificationDelay);
		}

		private void frmMain_Resize(object sender, EventArgs e)
		{
			this.ShowInTaskbar = (this.WindowState != FormWindowState.Minimized);
		}
	}
}
