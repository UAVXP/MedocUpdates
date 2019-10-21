namespace MedocUpdates
{
	partial class frmSettings
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
			this.gbLogLevels = new System.Windows.Forms.GroupBox();
			this.cmbLogLevels = new System.Windows.Forms.ComboBox();
			this.lblLogLevels = new System.Windows.Forms.Label();
			this.cbLogs = new System.Windows.Forms.CheckBox();
			this.gbTelegram = new System.Windows.Forms.GroupBox();
			this.lblTelegramToken = new System.Windows.Forms.Label();
			this.tbTelegramToken = new System.Windows.Forms.TextBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.gbDownloads = new System.Windows.Forms.GroupBox();
			this.tbDownloadsPath = new System.Windows.Forms.TextBox();
			this.lblDownloadPath = new System.Windows.Forms.Label();
			this.btnDownloadsPathBrowse = new System.Windows.Forms.Button();
			this.fbdDownloadPath = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog();
			this.gbLogLevels.SuspendLayout();
			this.gbTelegram.SuspendLayout();
			this.gbDownloads.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbLogLevels
			// 
			this.gbLogLevels.Controls.Add(this.cmbLogLevels);
			this.gbLogLevels.Controls.Add(this.lblLogLevels);
			this.gbLogLevels.Controls.Add(this.cbLogs);
			this.gbLogLevels.Location = new System.Drawing.Point(12, 12);
			this.gbLogLevels.Name = "gbLogLevels";
			this.gbLogLevels.Size = new System.Drawing.Size(234, 81);
			this.gbLogLevels.TabIndex = 0;
			this.gbLogLevels.TabStop = false;
			this.gbLogLevels.Text = "Logs";
			// 
			// cmbLogLevels
			// 
			this.cmbLogLevels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLogLevels.FormattingEnabled = true;
			this.cmbLogLevels.Location = new System.Drawing.Point(91, 42);
			this.cmbLogLevels.Name = "cmbLogLevels";
			this.cmbLogLevels.Size = new System.Drawing.Size(121, 21);
			this.cmbLogLevels.TabIndex = 2;
			// 
			// lblLogLevels
			// 
			this.lblLogLevels.AutoSize = true;
			this.lblLogLevels.Location = new System.Drawing.Point(15, 45);
			this.lblLogLevels.Name = "lblLogLevels";
			this.lblLogLevels.Size = new System.Drawing.Size(70, 13);
			this.lblLogLevels.TabIndex = 1;
			this.lblLogLevels.Text = "Level of logs:";
			// 
			// cbLogs
			// 
			this.cbLogs.AutoSize = true;
			this.cbLogs.Location = new System.Drawing.Point(15, 19);
			this.cbLogs.Name = "cbLogs";
			this.cbLogs.Size = new System.Drawing.Size(96, 17);
			this.cbLogs.TabIndex = 0;
			this.cbLogs.Text = "Enable logging";
			this.cbLogs.UseVisualStyleBackColor = true;
			this.cbLogs.CheckedChanged += new System.EventHandler(this.cbLogs_CheckedChanged);
			// 
			// gbTelegram
			// 
			this.gbTelegram.Controls.Add(this.lblTelegramToken);
			this.gbTelegram.Controls.Add(this.tbTelegramToken);
			this.gbTelegram.Location = new System.Drawing.Point(12, 99);
			this.gbTelegram.Name = "gbTelegram";
			this.gbTelegram.Size = new System.Drawing.Size(234, 55);
			this.gbTelegram.TabIndex = 1;
			this.gbTelegram.TabStop = false;
			this.gbTelegram.Text = "Telegram";
			// 
			// lblTelegramToken
			// 
			this.lblTelegramToken.AutoSize = true;
			this.lblTelegramToken.Location = new System.Drawing.Point(15, 22);
			this.lblTelegramToken.Name = "lblTelegramToken";
			this.lblTelegramToken.Size = new System.Drawing.Size(41, 13);
			this.lblTelegramToken.TabIndex = 1;
			this.lblTelegramToken.Text = "Token:";
			// 
			// tbTelegramToken
			// 
			this.tbTelegramToken.Location = new System.Drawing.Point(62, 19);
			this.tbTelegramToken.Name = "tbTelegramToken";
			this.tbTelegramToken.Size = new System.Drawing.Size(150, 20);
			this.tbTelegramToken.TabIndex = 0;
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(587, 169);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(668, 169);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// gbDownloads
			// 
			this.gbDownloads.Controls.Add(this.btnDownloadsPathBrowse);
			this.gbDownloads.Controls.Add(this.lblDownloadPath);
			this.gbDownloads.Controls.Add(this.tbDownloadsPath);
			this.gbDownloads.Location = new System.Drawing.Point(252, 12);
			this.gbDownloads.Name = "gbDownloads";
			this.gbDownloads.Size = new System.Drawing.Size(489, 81);
			this.gbDownloads.TabIndex = 4;
			this.gbDownloads.TabStop = false;
			this.gbDownloads.Text = "Download settings";
			// 
			// tbDownloadsPath
			// 
			this.tbDownloadsPath.Location = new System.Drawing.Point(109, 17);
			this.tbDownloadsPath.Name = "tbDownloadsPath";
			this.tbDownloadsPath.Size = new System.Drawing.Size(363, 20);
			this.tbDownloadsPath.TabIndex = 0;
			// 
			// lblDownloadPath
			// 
			this.lblDownloadPath.AutoSize = true;
			this.lblDownloadPath.Location = new System.Drawing.Point(16, 20);
			this.lblDownloadPath.Name = "lblDownloadPath";
			this.lblDownloadPath.Size = new System.Drawing.Size(87, 13);
			this.lblDownloadPath.TabIndex = 1;
			this.lblDownloadPath.Text = "Downloads path:";
			// 
			// btnDownloadsPathBrowse
			// 
			this.btnDownloadsPathBrowse.Location = new System.Drawing.Point(397, 43);
			this.btnDownloadsPathBrowse.Name = "btnDownloadsPathBrowse";
			this.btnDownloadsPathBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnDownloadsPathBrowse.TabIndex = 2;
			this.btnDownloadsPathBrowse.Text = "Browse...";
			this.btnDownloadsPathBrowse.UseVisualStyleBackColor = true;
			this.btnDownloadsPathBrowse.Click += new System.EventHandler(this.btnDownloadPathBrowse_Click);
			// 
			// fbdDownloadPath
			// 
			this.fbdDownloadPath.RootFolder = System.Environment.SpecialFolder.MyComputer;
			// 
			// frmSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(755, 204);
			this.Controls.Add(this.gbDownloads);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.gbTelegram);
			this.Controls.Add(this.gbLogLevels);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmSettings";
			this.Text = "Medoc Updates - Settings";
			this.Load += new System.EventHandler(this.frmSettings_Load);
			this.gbLogLevels.ResumeLayout(false);
			this.gbLogLevels.PerformLayout();
			this.gbTelegram.ResumeLayout(false);
			this.gbTelegram.PerformLayout();
			this.gbDownloads.ResumeLayout(false);
			this.gbDownloads.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbLogLevels;
		private System.Windows.Forms.GroupBox gbTelegram;
		private System.Windows.Forms.ComboBox cmbLogLevels;
		private System.Windows.Forms.Label lblLogLevels;
		private System.Windows.Forms.CheckBox cbLogs;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblTelegramToken;
		private System.Windows.Forms.TextBox tbTelegramToken;
		private System.Windows.Forms.GroupBox gbDownloads;
		private System.Windows.Forms.Button btnDownloadsPathBrowse;
		private System.Windows.Forms.Label lblDownloadPath;
		private System.Windows.Forms.TextBox tbDownloadsPath;
		private Ookii.Dialogs.WinForms.VistaFolderBrowserDialog fbdDownloadPath;
	}
}