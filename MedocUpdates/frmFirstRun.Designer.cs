namespace MedocUpdates
{
	partial class frmFirstRun
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFirstRun));
			this.label1 = new System.Windows.Forms.Label();
			this.lblTelegramToken = new System.Windows.Forms.Label();
			this.tbTelegramToken = new System.Windows.Forms.TextBox();
			this.lblDownloadPath = new System.Windows.Forms.Label();
			this.tbDownloadsPath = new System.Windows.Forms.TextBox();
			this.cmbLogLevels = new System.Windows.Forms.ComboBox();
			this.lblLogLevels = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.fbdDownloadPath = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog();
			this.btnDownloadsPathBrowse = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbLanguages = new System.Windows.Forms.ComboBox();
			this.lblLanguage = new System.Windows.Forms.Label();
			this.lblLanguageNote = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(35, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(396, 26);
			this.label1.TabIndex = 0;
			this.label1.Text = "Hello. Looks like you\'ve run this application for the first time.\r\nLet\'s proceed " +
    "you through the important settings.\r\n";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblTelegramToken
			// 
			this.lblTelegramToken.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lblTelegramToken.AutoSize = true;
			this.lblTelegramToken.Location = new System.Drawing.Point(88, 242);
			this.lblTelegramToken.Name = "lblTelegramToken";
			this.lblTelegramToken.Size = new System.Drawing.Size(84, 13);
			this.lblTelegramToken.TabIndex = 3;
			this.lblTelegramToken.Text = "Telegram token:";
			// 
			// tbTelegramToken
			// 
			this.tbTelegramToken.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.tbTelegramToken.Location = new System.Drawing.Point(181, 239);
			this.tbTelegramToken.Name = "tbTelegramToken";
			this.tbTelegramToken.Size = new System.Drawing.Size(150, 20);
			this.tbTelegramToken.TabIndex = 2;
			// 
			// lblDownloadPath
			// 
			this.lblDownloadPath.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lblDownloadPath.AutoSize = true;
			this.lblDownloadPath.Location = new System.Drawing.Point(88, 216);
			this.lblDownloadPath.Name = "lblDownloadPath";
			this.lblDownloadPath.Size = new System.Drawing.Size(87, 13);
			this.lblDownloadPath.TabIndex = 5;
			this.lblDownloadPath.Text = "Downloads path:";
			// 
			// tbDownloadsPath
			// 
			this.tbDownloadsPath.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.tbDownloadsPath.Location = new System.Drawing.Point(181, 213);
			this.tbDownloadsPath.Name = "tbDownloadsPath";
			this.tbDownloadsPath.Size = new System.Drawing.Size(150, 20);
			this.tbDownloadsPath.TabIndex = 4;
			// 
			// cmbLogLevels
			// 
			this.cmbLogLevels.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.cmbLogLevels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLogLevels.FormattingEnabled = true;
			this.cmbLogLevels.Location = new System.Drawing.Point(181, 186);
			this.cmbLogLevels.Name = "cmbLogLevels";
			this.cmbLogLevels.Size = new System.Drawing.Size(150, 21);
			this.cmbLogLevels.TabIndex = 7;
			// 
			// lblLogLevels
			// 
			this.lblLogLevels.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lblLogLevels.AutoSize = true;
			this.lblLogLevels.Location = new System.Drawing.Point(88, 189);
			this.lblLogLevels.Name = "lblLogLevels";
			this.lblLogLevels.Size = new System.Drawing.Size(70, 13);
			this.lblLogLevels.TabIndex = 6;
			this.lblLogLevels.Text = "Level of logs:";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCancel.Location = new System.Drawing.Point(236, 324);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "Quit";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSave.Location = new System.Drawing.Point(155, 324);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 8;
			this.btnSave.Text = "Next >";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// fbdDownloadPath
			// 
			this.fbdDownloadPath.RootFolder = System.Environment.SpecialFolder.MyComputer;
			// 
			// btnDownloadsPathBrowse
			// 
			this.btnDownloadsPathBrowse.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnDownloadsPathBrowse.Location = new System.Drawing.Point(337, 213);
			this.btnDownloadsPathBrowse.Name = "btnDownloadsPathBrowse";
			this.btnDownloadsPathBrowse.Size = new System.Drawing.Size(75, 21);
			this.btnDownloadsPathBrowse.TabIndex = 10;
			this.btnDownloadsPathBrowse.Text = "Browse...";
			this.btnDownloadsPathBrowse.UseVisualStyleBackColor = true;
			this.btnDownloadsPathBrowse.Click += new System.EventHandler(this.btnDownloadsPathBrowse_Click);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(12, 45);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(442, 123);
			this.label2.TabIndex = 0;
			this.label2.Text = resources.GetString("label2.Text");
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// cmbLanguages
			// 
			this.cmbLanguages.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.cmbLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLanguages.FormattingEnabled = true;
			this.cmbLanguages.Location = new System.Drawing.Point(181, 265);
			this.cmbLanguages.Name = "cmbLanguages";
			this.cmbLanguages.Size = new System.Drawing.Size(150, 21);
			this.cmbLanguages.TabIndex = 12;
			// 
			// lblLanguage
			// 
			this.lblLanguage.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lblLanguage.AutoSize = true;
			this.lblLanguage.Location = new System.Drawing.Point(88, 268);
			this.lblLanguage.Name = "lblLanguage";
			this.lblLanguage.Size = new System.Drawing.Size(58, 13);
			this.lblLanguage.TabIndex = 11;
			this.lblLanguage.Text = "Language:";
			// 
			// lblLanguageNote
			// 
			this.lblLanguageNote.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lblLanguageNote.AutoSize = true;
			this.lblLanguageNote.Location = new System.Drawing.Point(65, 289);
			this.lblLanguageNote.Name = "lblLanguageNote";
			this.lblLanguageNote.Size = new System.Drawing.Size(325, 13);
			this.lblLanguageNote.TabIndex = 13;
			this.lblLanguageNote.Text = "Note that the language will change only after the application restart.";
			// 
			// frmFirstRun
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(466, 370);
			this.ControlBox = false;
			this.Controls.Add(this.lblLanguageNote);
			this.Controls.Add(this.cmbLanguages);
			this.Controls.Add(this.lblLanguage);
			this.Controls.Add(this.btnDownloadsPathBrowse);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.cmbLogLevels);
			this.Controls.Add(this.lblLogLevels);
			this.Controls.Add(this.lblDownloadPath);
			this.Controls.Add(this.tbDownloadsPath);
			this.Controls.Add(this.lblTelegramToken);
			this.Controls.Add(this.tbTelegramToken);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmFirstRun";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Medoc Updates - First run";
			this.Load += new System.EventHandler(this.frmFirstRun_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblTelegramToken;
		private System.Windows.Forms.TextBox tbTelegramToken;
		private System.Windows.Forms.Label lblDownloadPath;
		private System.Windows.Forms.TextBox tbDownloadsPath;
		private System.Windows.Forms.ComboBox cmbLogLevels;
		private System.Windows.Forms.Label lblLogLevels;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSave;
		private Ookii.Dialogs.WinForms.VistaFolderBrowserDialog fbdDownloadPath;
		private System.Windows.Forms.Button btnDownloadsPathBrowse;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbLanguages;
		private System.Windows.Forms.Label lblLanguage;
		private System.Windows.Forms.Label lblLanguageNote;
	}
}