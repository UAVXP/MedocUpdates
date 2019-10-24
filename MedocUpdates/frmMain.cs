using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using System.IO;
using System.Reflection;

namespace MedocUpdates
{
	public partial class frmMain : Form
	{
		MedocAPI medoc = new MedocAPI();
		MedocInternal localmedoc = new MedocInternal();
		MedocTelegram telegram = new MedocTelegram();

		// https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.control.invoke?redirectedfrom=MSDN&view=netframework-4.7.2#System_Windows_Forms_Control_Invoke_System_Delegate_
		MedocNetwork network;
		public delegate void RefreshVersionStatus();
		public RefreshVersionStatus NetworkCheckingDelegate;


		bool isMinimizedMessageShown = false;

		public frmMain()
		{
			Log.Write(LogLevel.EXPERT, "Loading main frame");
			InitializeComponent();

			// Network checking
			network = new MedocNetwork(this);
			NetworkCheckingDelegate = new RefreshVersionStatus(CheckingRoutine);
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
				MedocVersion version = medoc.GetLatestVersion();
				//version = "11.01.023";
				labelVersion.Text = "Latest version: " + version;

				//MedocVersion test = new MedocVersion(version);

				//MedocVersion test2 = "11.01.024";
				//MedocVersion test3 = "11.01.023";


				MedocVersion localversion = localmedoc.LocalVersion;
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
						btn.IsHighlighted = (item.version > localversion);
						btn.FileDownloadedAndRunned += Btn_FileDownloadedAndRunned;
						flowDownloads.Controls.Add(btn);

					//	Console.WriteLine("Added {0}", item.link);
					}
				}

				Status("Done");
				trayIcon.Text = labelVersion.Text + "\r\n" + labelLocalVersion.Text;

				if (localversion != version)
				{
					trayIcon.ShowBalloonTip(5000, "M.E.Doc update has been released!",	labelVersion.Text + "\r\n" +
																						labelLocalVersion.Text, ToolTipIcon.Info);
				}
				else
				{
					if(this.WindowState != FormWindowState.Minimized)
						trayIcon.ShowBalloonTip(5000, "No updates for M.E.Doc", "Minimize the app to deny \"no updates\" notifications\r\n" +
																				labelVersion.Text + "\r\n" +
																				labelLocalVersion.Text, ToolTipIcon.Info);
				}

				if(ParsedArgs.GetToken("telegramforcemsg") || localversion != version)
					telegram.SendMessageAll(String.Format("Update from {0} to {1} is available", localversion, version));
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
			this.Text += " - " + Assembly.GetEntryAssembly().GetName().Version;

			CheckingRoutine();

			TimerRoutine();

			SessionStorage.NotificationDelayChanged += frmMain_NotificationDelayChanged;
			network.NetworkIsUp += Network_NetworkIsUp;
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

		private void Network_NetworkIsUp(object sender, EventArgs e)
		{
			new Thread(new ThreadStart(network.Run)).Start();
		}

		private void Btn_FileDownloadedAndRunned(object sender, EventArgs e)
		{
			CheckingRoutine();

			// FIXME: Should I do this in DownloadButton maybe?
			if(SessionStorage.inside.RemoveUpdateFileAfterInstall)
			{
				DownloadButton btn = (sender as DownloadButton);
				File.Delete(btn.UpdateFilename);
			}
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

			if(!this.ShowInTaskbar && !isMinimizedMessageShown)
			{
				trayIcon.ShowBalloonTip(5000, "Medoc Updates window has been minimized", "You can reach Medoc Updates through the tray icon now", ToolTipIcon.Info);
				isMinimizedMessageShown = true;
			}
		}
	}
}
