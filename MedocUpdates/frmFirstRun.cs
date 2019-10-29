using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace MedocUpdates
{
	public partial class frmFirstRun : Form
	{
		private class LogLevelHelper
		{
			public int Level { get; set; } // LogLevel
			public string Name { get; set; }
		}

		private class LanguageHelper
		{
			public string Abbr { get; set; }
			public string Name { get; set; }
		}

		private List<LogLevelHelper> logLevels = new List<LogLevelHelper>();
		private List<LanguageHelper> languages = new List<LanguageHelper>();

		private void InitializeLocalization()
		{
			this.label1.Text = Loc.Get("frmFirstRun.label1.Text"); // "Hello. Looks like you\'ve run this application for the first time.\r\nLet\'s proceed you through the important settings.\r\n";
			this.lblTelegramToken.Text = Loc.Get("frmFirstRun.lblTelegramToken.Text"); // "Telegram token:";
			this.lblDownloadPath.Text = Loc.Get("frmFirstRun.lblDownloadPath.Text"); // "Downloads path:";
			this.lblLogLevels.Text = Loc.Get("frmFirstRun.lblLogLevels.Text"); // "Level of logs:";
			this.btnCancel.Text = Loc.Get("frmFirstRun.btnCancel.Text"); // "Quit";
			this.btnSave.Text = Loc.Get("frmFirstRun.btnSave.Text"); // "Next >";
			this.btnDownloadsPathBrowse.Text = Loc.Get("frmFirstRun.btnDownloadsPathBrowse.Text"); // "Browse...";
			this.label2.Text = Loc.Get("frmFirstRun.label2.Text"); // resources.GetString("label2.Text")
			this.Text = Loc.Get("frmFirstRun.Text"); // "Medoc Updates - First run";
		}

		public frmFirstRun()
		{
			InitializeComponent();
			InitializeLocalization();

			for (int i = LogLevel.BASIC; i < LogLevel.MAXLOGLEVELS; i++)
			{
				logLevels.Add(new LogLevelHelper() { Level = i, Name = LogLevel.GetName(i) });
			}

			string[] locfiles;
			string[] locs;
			Loc.GetLocalizations(out locs, out locfiles); // TODO: Check

			for (int i = 0; i < locs.Length; i++)
			{
				languages.Add(new LanguageHelper() { Abbr = Path.GetFileNameWithoutExtension(locfiles[i]), Name = locs[i] });
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

			cmbLanguages.DisplayMember = "Name";
			cmbLanguages.ValueMember = "Abbr";
			cmbLanguages.DataSource = languages;
			cmbLanguages.SelectedValue = SessionStorage.inside.SelectedLanguage;
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

			if (SessionStorage.inside.SelectedLanguage != cmbLanguages.SelectedValue.ToString())
			{
				SessionStorage.inside.SelectedLanguage = cmbLanguages.SelectedValue.ToString();
				SessionStorage.SelectedLanguageChangedFunc(this);
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
