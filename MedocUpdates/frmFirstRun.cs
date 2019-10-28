﻿using System;
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
	public partial class frmFirstRun : Form
	{
		private class LogLevelHelper
		{
			public int Level { get; set; } // LogLevel
			public string Name { get; set; }
		}

		private List<LogLevelHelper> logLevels = new List<LogLevelHelper>();
		public frmFirstRun()
		{
			InitializeComponent();

			for (int i = LogLevel.BASIC; i < LogLevel.MAXLOGLEVELS; i++)
			{
				logLevels.Add(new LogLevelHelper() { Level = i, Name = LogLevel.GetName(i) });
			}
		}

		private void frmFirstRun_Load(object sender, EventArgs e)
		{
			cmbLogLevels.DisplayMember = "Name";
			cmbLogLevels.ValueMember = "Level";
			cmbLogLevels.DataSource = logLevels;
			cmbLogLevels.SelectedIndex = SessionStorage.inside.LoggingLevel;

			tbTelegramToken.Text = SessionStorage.inside.TelegramToken;

			tbDownloadsPath.Text = SessionStorage.inside.DownloadsFolderPath;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (SessionStorage.inside.LoggingLevel != (int)cmbLogLevels.SelectedValue) // LogLevel
			{
				SessionStorage.inside.LoggingLevel = (int)cmbLogLevels.SelectedValue; // LogLevel
				SessionStorage.LoggingLevelChangedFunc(this);
			}

			if (SessionStorage.inside.TelegramToken != tbTelegramToken.Text)
			{
				SessionStorage.inside.TelegramToken = tbTelegramToken.Text;
				SessionStorage.TelegramTokenChangedFunc(this);
			}

			if (SessionStorage.inside.DownloadsFolderPath != tbDownloadsPath.Text)
			{
				SessionStorage.inside.DownloadsFolderPath = tbDownloadsPath.Text;
				SessionStorage.DownloadsFolderPathChangedFunc(this);
			}

			Log.SetLevel(SessionStorage.inside.LoggingLevel);

			SessionStorage.Save();
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			//Application.Exit();
			Environment.Exit(0);
		}

		private void btnDownloadsPathBrowse_Click(object sender, EventArgs e)
		{
			//DialogResult result = fbdDownloadPath.ShowDialog();
			DialogResult result = fbdDownloadPath.ShowDialog();
			if (result == DialogResult.Cancel)
				return;

			tbDownloadsPath.Text = fbdDownloadPath.SelectedPath;
		}
	}
}