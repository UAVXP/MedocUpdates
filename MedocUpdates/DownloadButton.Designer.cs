namespace MedocUpdates
{
	partial class DownloadButton
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.llblDownload = new System.Windows.Forms.LinkLabel();
			this.labelVersion = new System.Windows.Forms.Label();
			this.llblDownloadRun = new System.Windows.Forms.LinkLabel();
			this.pbDownloadRun = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// llblDownload
			// 
			this.llblDownload.AutoSize = true;
			this.llblDownload.Location = new System.Drawing.Point(16, 48);
			this.llblDownload.Name = "llblDownload";
			this.llblDownload.Size = new System.Drawing.Size(55, 13);
			this.llblDownload.TabIndex = 0;
			this.llblDownload.TabStop = true;
			this.llblDownload.Text = "Download";
			this.llblDownload.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// labelVersion
			// 
			this.labelVersion.AutoSize = true;
			this.labelVersion.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelVersion.Location = new System.Drawing.Point(3, 10);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(193, 29);
			this.labelVersion.TabIndex = 2;
			this.labelVersion.Text = "VERSION HERE";
			// 
			// llblDownloadRun
			// 
			this.llblDownloadRun.AutoSize = true;
			this.llblDownloadRun.Location = new System.Drawing.Point(90, 48);
			this.llblDownloadRun.Name = "llblDownloadRun";
			this.llblDownloadRun.Size = new System.Drawing.Size(94, 13);
			this.llblDownloadRun.TabIndex = 3;
			this.llblDownloadRun.TabStop = true;
			this.llblDownloadRun.Text = "Download and run";
			this.llblDownloadRun.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblDownloadRun_LinkClicked);
			// 
			// pbDownloadRun
			// 
			this.pbDownloadRun.Location = new System.Drawing.Point(8, 64);
			this.pbDownloadRun.Name = "pbDownloadRun";
			this.pbDownloadRun.Size = new System.Drawing.Size(188, 10);
			this.pbDownloadRun.TabIndex = 4;
			this.pbDownloadRun.Visible = false;
			// 
			// DownloadButton
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Info;
			this.Controls.Add(this.pbDownloadRun);
			this.Controls.Add(this.llblDownloadRun);
			this.Controls.Add(this.labelVersion);
			this.Controls.Add(this.llblDownload);
			this.Name = "DownloadButton";
			this.Size = new System.Drawing.Size(201, 88);
			this.Load += new System.EventHandler(this.DownloadButton_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.LinkLabel llblDownload;
		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.LinkLabel llblDownloadRun;
		private System.Windows.Forms.ProgressBar pbDownloadRun;
	}
}
