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

		public frmMain()
		{
			InitializeComponent();
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
			this.Show();
			this.Focus();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}

		private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			labelVersion.Text = "Checking...";
			bool success = medoc.RefreshDoc();
			if (success)
			{
				string version = medoc.GetLatestVersion();
				labelVersion.Text = "Latest version: " + version;
			}
			else
			{
				labelVersion.Text = "Something went wrong";
			}
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			medoc.RefreshDoc();
			string version = medoc.GetLatestVersion();
			labelVersion.Text = "Latest version: " + version;

			MedocDownloadItem[] items;
			bool success = medoc.GetItems(out items);
			if (success)
			{
				foreach (MedocDownloadItem item in items)
				{
					DownloadButton btn = new DownloadButton(item);
					flowDownloads.Controls.Add(btn);

					Console.WriteLine("Added {0}", item.link);
				}
			}
		}
	}
}
