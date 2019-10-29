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
	public partial class frmSettings : Form
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
			this.gbLogLevels.Text = Loc.Get("frmSettings.gbLogLevels.Text"); // "Logs"
			this.lblLogLevels.Text = Loc.Get("frmSettings.lblLogLevels.Text"); // "Level of logs:"
			this.cbLogs.Text = Loc.Get("frmSettings.cbLogs.Text"); // "Enable logging"
			this.gbTelegram.Text = Loc.Get("frmSettings.gbTelegram.Text"); // "Telegram"
			this.lblTelegramToken.Text = Loc.Get("frmSettings.lblTelegramToken.Text"); // "Token:"
			this.btnSave.Text = Loc.Get("frmSettings.btnSave.Text"); // "Save"
			this.btnCancel.Text = Loc.Get("frmSettings.btnCancel.Text"); // "Cancel"
			this.gbDownloads.Text = Loc.Get("frmSettings.gbDownloads.Text"); // "Download settings"
			this.btnDownloadsPathBrowse.Text = Loc.Get("frmSettings.btnDownloadsPathBrowse.Text"); // "Browse..."
			this.lblDownloadPath.Text = Loc.Get("frmSettings.lblDownloadPath.Text"); // "Downloads path:"
			this.cbRemoveUpdateFile.Text = Loc.Get("frmSettings.cbRemoveUpdateFile.Text"); // "Remove .upd files after update installation"
			this.Text = Loc.Get("frmSettings.Text"); // "Medoc Updates - Settings"

			this.gbLanguage.Text = Loc.Get("frmSettings.gbLanguage.Text"); // "Language setup";
			this.lblLanguage.Text = Loc.Get("frmSettings.lblLanguage.Text"); // "Language:";
			this.lblLanguageNote.Text = Loc.Get("frmSettings.lblLanguageNote.Text"); // "Note that the language will change only after the application restart.";
		}

		public frmSettings()
		{
			InitializeComponent();
			InitializeLocalization();

			for (int i = LogLevel.BASIC; i < LogLevel.MAXLOGLEVELS; i++)
			{
				logLevels.Add( new LogLevelHelper() { Level = i, Name = LogLevel.GetName(i) });
			}

			string[] locfiles;
			string[] locs;
			Loc.GetLocalizations(out locs, out locfiles); // TODO: Check

			for (int i = 0; i < locs.Length; i++)
			{
				languages.Add(new LanguageHelper() { Abbr = Path.GetFileNameWithoutExtension(locfiles[i]), Name = locs[i] });
			}
		}

		private void frmSettings_Load(object sender, EventArgs e)
		{
			cbLogs.Checked = SessionStorage.inside.LogsEnabled;

			cmbLogLevels.Enabled = cbLogs.Checked;
			cmbLogLevels.DisplayMember = "Name";
			cmbLogLevels.ValueMember = "Level";
			cmbLogLevels.DataSource = logLevels;
			cmbLogLevels.SelectedIndex = SessionStorage.inside.LoggingLevel;

			tbTelegramToken.Text = SessionStorage.inside.TelegramToken;

			tbDownloadsPath.Text = SessionStorage.inside.DownloadsFolderPath;
			fbdDownloadPath.SelectedPath = tbDownloadsPath.Text;

			cbRemoveUpdateFile.Checked = SessionStorage.inside.RemoveUpdateFileAfterInstall;

			cmbLanguages.DisplayMember = "Name";
			cmbLanguages.ValueMember = "Abbr";
			cmbLanguages.DataSource = languages;
			cmbLanguages.SelectedValue = SessionStorage.inside.SelectedLanguage;
		}

		private void cbLogs_CheckedChanged(object sender, EventArgs e)
		{
			cmbLogLevels.Enabled = cbLogs.Checked;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if(SessionStorage.inside.LogsEnabled != cbLogs.Checked)
			{
				SessionStorage.inside.LogsEnabled = cbLogs.Checked;
				SessionStorage.LogsEnabledChangedFunc(this);
			}

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

			if (SessionStorage.inside.RemoveUpdateFileAfterInstall != cbRemoveUpdateFile.Checked)
			{
				SessionStorage.inside.RemoveUpdateFileAfterInstall = cbRemoveUpdateFile.Checked;
				SessionStorage.RemoveUpdateFileAfterInstallChangedFunc(this);
			}

			if (SessionStorage.inside.SelectedLanguage != cmbLanguages.SelectedValue.ToString())
			{
				SessionStorage.inside.SelectedLanguage = cmbLanguages.SelectedValue.ToString();
				SessionStorage.SelectedLanguageChangedFunc(this);
			}

			Log.SetEnabled(SessionStorage.inside.LogsEnabled);
			Log.SetLevel(SessionStorage.inside.LoggingLevel);

			SessionStorage.Save();
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnDownloadPathBrowse_Click(object sender, EventArgs e)
		{
			//DialogResult result = fbdDownloadPath.ShowDialog();
			DialogResult result = fbdDownloadPath.ShowDialog();
			if (result == DialogResult.Cancel)
				return;

			tbDownloadsPath.Text = fbdDownloadPath.SelectedPath;
		}
	}
}
