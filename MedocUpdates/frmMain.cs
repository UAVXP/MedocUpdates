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
		//int lastDownloadsCount = 0;

		private void InitializeLocalization()
		{
			this.trayIcon.Text = Loc.Get("frmMain.trayIcon.Text"); // "Medoc Updates";
			this.checkForUpdatesToolStripMenuItem.Text = Loc.Get("frmMain.checkForUpdatesToolStripMenuItem.Text"); // "Check for updates";
			this.delayNotificationsToolStripMenuItem.Text = Loc.Get("frmMain.delayNotificationsToolStripMenuItem.Text"); // "Delay notifications...";
			this.exitToolStripMenuItem.Text = Loc.Get("frmMain.exitToolStripMenuItem.Text"); // "Exit";
			this.fileToolStripMenuItem.Text = Loc.Get("frmMain.fileToolStripMenuItem.Text"); // "File";
			this.labelVersion.Text = Loc.Get("frmMain.labelVersion.Text"); // "VERSION HERE";
			this.menuStrip1.Text = Loc.Get("frmMain.menuStrip1.Text"); // "menuStrip1";
			this.editToolStripMenuItem.Text = Loc.Get("frmMain.editToolStripMenuItem.Text"); // "Edit";
			this.settingsToolStripMenuItem.Text = Loc.Get("frmMain.settingsToolStripMenuItem.Text"); // "Settings";
			this.labelLocalVersion.Text = Loc.Get("frmMain.labelLocalVersion.Text"); // "LOCAL VERSION HERE";
			this.statusStrip1.Text = Loc.Get("frmMain.statusStrip1.Text"); // "statusStrip1";
			this.toolStripStatusLabel1.Text = Loc.Get("frmMain.toolStripStatusLabel1.Text"); // "toolStripStatusLabel1";
			this.Text = Loc.Get("frmMain.Text"); // "Medoc Updates";
		}

		public frmMain()
		{
			Log.Write(LogLevel.EXPERT, "Loading main frame");
			InitializeComponent();
			InitializeLocalization();

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
			//flowDownloads.Controls.Clear();

			labelVersion.Text = Loc.Get("frmMain.labelVersion.Text.CheckingRoutine");
			Log.Write("Checking for updates on the medoc.ua server");

			bool success = medoc.RefreshDoc();
			if (success)
			{
				MedocVersion version = medoc.GetLatestVersion();
				if (!MedocVersion.IsValid(version))
				{
					Log.Write("Application cannot get the latest remote M.E.Doc version");
					return;
				}

				//version = "11.01.023";
				labelVersion.Text = String.Format(Loc.Get("frmMain.labelVersion.Text.CheckingRoutine.LatestVersion"), version);

				//MedocVersion test = new MedocVersion(version);

				//MedocVersion test2 = "11.01.024";
				//MedocVersion test3 = "11.01.023";


				MedocVersion localversion = localmedoc.LocalVersion;
				if(!MedocVersion.IsValid(version))
				{
					Log.Write("Application cannot get a local version of M.E.Doc installation.");
					MessageBox.Show(Loc.Get("frmMain.MessageBox.CheckingRoutine.NoLocalVersion"), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

					return;
				}
				//localversion = "11.01.021";
				labelLocalVersion.Text = String.Format(Loc.Get("frmMain.labelLocalVersion.Text.CheckingRoutine.LatestLocalVersion"), localversion);

				Log.Write(labelVersion.Text);
				Log.Write(labelLocalVersion.Text);

				// Does some updates are performing now? Then don't recreate the buttons
				// FIXME: Still can be a better solution probably
				bool isStillUpdating = false;
				foreach(DownloadButton button in flowDownloads.Controls)
				{
					if(button.IsUpdating)
					{
						isStillUpdating = true;
						break;
					}
				}

				if(!isStillUpdating)
				{
					MedocDownloadItem[] items;
					success = medoc.GetItems(out items);
					if (success)
					{
						/*
							if (lastDownloadsCount != items.Length)
							{
								// Initial download items update
								//if(lastDownloadsCount == 0)
								if(false) // Test
								{
									flowDownloads.Controls.Clear();
									foreach (MedocDownloadItem item in items)
									{
										DownloadButton btn = new DownloadButton(item);
										btn.IsHighlighted = (item.version > localversion);
										btn.FileDownloadedAndRunned += Btn_FileDownloadedAndRunned;
										flowDownloads.Controls.Add(btn);

										//	Console.WriteLine("Added {0}", item.link);
									}
								}
								else // Update count was changed since the last checking for updates
								{
									// Determine what count should we add to existing download items
									int newItemsCount = items.Length - lastDownloadsCount;
									int i = 0;
									for (; newItemsCount > 0; newItemsCount--, i++)
									{
										//MedocDownloadItem item = items[newItemsCount-1]; // Reverse order
										MedocDownloadItem item = items[i];
										DownloadButton btn = new DownloadButton(item);
										btn.IsHighlighted = (item.version > localversion);
										btn.FileDownloadedAndRunned += Btn_FileDownloadedAndRunned;
										flowDownloads.Controls.Add(btn); // This whole thing might be working if I could add to the begin of the Controls
									}
								}

								lastDownloadsCount = items.Length;
							}
						*/

						flowDownloads.Controls.Clear();
						foreach (MedocDownloadItem item in items)
						{
							DownloadButton btn = new DownloadButton(item);
							btn.IsHighlighted = (item.version > localversion);
							btn.FileDownloadedAndRunned += Btn_FileDownloadedAndRunned;
							flowDownloads.Controls.Add(btn);

							//	Console.WriteLine("Added {0}", item.link);
						}
					}
				}

				//Status("Done");
				Status(Loc.Get("frmMain_Done"));
				trayIcon.Text = labelVersion.Text + "\r\n" + labelLocalVersion.Text;

				if (localversion != version)
				{
					trayIcon.ShowBalloonTip(5000, Loc.Get("frmMain.trayIcon.BalloonTipTitle.CheckingRoutine.UpdateReleased"),
						labelVersion.Text + "\r\n" + labelLocalVersion.Text, ToolTipIcon.Info);
				}
				else
				{
					if(this.WindowState != FormWindowState.Minimized)
						trayIcon.ShowBalloonTip(5000, Loc.Get("frmMain.trayIcon.BalloonTipTitle.CheckingRoutine.NoUpdates"),
										String.Format(Loc.Get("frmMain.trayIcon.BalloonTipText.CheckingRoutine.NoUpdates"),
																				labelVersion.Text + "\r\n" +
																				labelLocalVersion.Text), ToolTipIcon.Info);
				}

				if(ParsedArgs.GetToken("telegramforcemsg") || localversion != version)
					telegram.SendMessageAll(String.Format(Loc.Get("frmMain.telegram.CheckingRoutine.UpdateAvailable"), localversion, version));
			}
			else
			{
				labelVersion.Text = Loc.Get("frmMain.labelVersion.Text.CheckingRoutine.Error");
				Log.Write("Cannot connect to medoc.ua");
				Status(Loc.Get("frmMain.Status.CheckingRoutine.CannotConnect"));
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
			if(ParsedArgs.GetToken("minimize"))
				this.WindowState = FormWindowState.Minimized;

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
			settings.Dispose();
			settings = null;
		}

		private void delayNotificationsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmNotificationsDelay delay = new frmNotificationsDelay();
			delay.Delay = SessionStorage.inside.NotificationDelay; // timerUpdate.Interval
			delay.ShowDialog();
			Console.WriteLine(SessionStorage.inside.NotificationDelay);

			delay.Dispose();
			delay = null;
		}

		private void frmMain_Resize(object sender, EventArgs e)
		{
			this.ShowInTaskbar = (this.WindowState != FormWindowState.Minimized);

			if(!this.ShowInTaskbar && !isMinimizedMessageShown)
			{
				trayIcon.ShowBalloonTip(5000,	Loc.Get("frmMain.trayIcon.BalloonTipTitle.CheckingRoutine.WindowMinimized"),
												Loc.Get("frmMain.trayIcon.BalloonTipText.CheckingRoutine.WindowMinimized"), ToolTipIcon.Info);
				isMinimizedMessageShown = true;
			}
		}
	}
}
