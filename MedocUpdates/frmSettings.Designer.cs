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
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.gbLogLevels.SuspendLayout();
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
			// groupBox2
			// 
			this.groupBox2.Location = new System.Drawing.Point(12, 99);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(234, 100);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "groupBox2";
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(250, 231);
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
			this.btnCancel.Location = new System.Drawing.Point(331, 231);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// frmSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(418, 266);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.gbLogLevels);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmSettings";
			this.Text = "Medoc Updates - Settings";
			this.Load += new System.EventHandler(this.frmSettings_Load);
			this.gbLogLevels.ResumeLayout(false);
			this.gbLogLevels.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbLogLevels;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox cmbLogLevels;
		private System.Windows.Forms.Label lblLogLevels;
		private System.Windows.Forms.CheckBox cbLogs;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
	}
}