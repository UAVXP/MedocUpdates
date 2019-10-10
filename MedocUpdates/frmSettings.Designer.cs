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
			this.gbUpdating = new System.Windows.Forms.GroupBox();
			this.cbUpdateTimes = new System.Windows.Forms.ComboBox();
			this.cbUpdateChecks = new System.Windows.Forms.CheckBox();
			this.gbUpdating.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbUpdating
			// 
			this.gbUpdating.Controls.Add(this.cbUpdateTimes);
			this.gbUpdating.Controls.Add(this.cbUpdateChecks);
			this.gbUpdating.Location = new System.Drawing.Point(12, 12);
			this.gbUpdating.Name = "gbUpdating";
			this.gbUpdating.Size = new System.Drawing.Size(278, 166);
			this.gbUpdating.TabIndex = 0;
			this.gbUpdating.TabStop = false;
			this.gbUpdating.Text = "Updating";
			// 
			// cbUpdateTimes
			// 
			this.cbUpdateTimes.FormattingEnabled = true;
			this.cbUpdateTimes.Location = new System.Drawing.Point(151, 17);
			this.cbUpdateTimes.Name = "cbUpdateTimes";
			this.cbUpdateTimes.Size = new System.Drawing.Size(121, 21);
			this.cbUpdateTimes.TabIndex = 1;
			// 
			// cbUpdateChecks
			// 
			this.cbUpdateChecks.AutoSize = true;
			this.cbUpdateChecks.Checked = true;
			this.cbUpdateChecks.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbUpdateChecks.Location = new System.Drawing.Point(15, 19);
			this.cbUpdateChecks.Name = "cbUpdateChecks";
			this.cbUpdateChecks.Size = new System.Drawing.Size(133, 17);
			this.cbUpdateChecks.TabIndex = 0;
			this.cbUpdateChecks.Text = "Enable update checks";
			this.cbUpdateChecks.UseVisualStyleBackColor = true;
			this.cbUpdateChecks.CheckedChanged += new System.EventHandler(this.cbUpdateChecks_CheckedChanged);
			// 
			// frmSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(527, 336);
			this.Controls.Add(this.gbUpdating);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmSettings";
			this.Text = "Medoc Updates - Settings";
			this.Load += new System.EventHandler(this.frmSettings_Load);
			this.gbUpdating.ResumeLayout(false);
			this.gbUpdating.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbUpdating;
		private System.Windows.Forms.CheckBox cbUpdateChecks;
		private System.Windows.Forms.ComboBox cbUpdateTimes;
	}
}