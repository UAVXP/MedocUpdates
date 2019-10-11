namespace MedocUpdates
{
	partial class frmNotificationsDelay
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNotificationsDelay));
			this.btnDone = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.rbByHour = new System.Windows.Forms.RadioButton();
			this.rbBy5Hours = new System.Windows.Forms.RadioButton();
			this.rbByManual = new System.Windows.Forms.RadioButton();
			this.rbByCustom = new System.Windows.Forms.RadioButton();
			this.timeChooser1 = new MedocUpdates.TimeChooser();
			this.gbDelays = new System.Windows.Forms.GroupBox();
			this.gbDelays.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnDone
			// 
			this.btnDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDone.Location = new System.Drawing.Point(134, 166);
			this.btnDone.Name = "btnDone";
			this.btnDone.Size = new System.Drawing.Size(75, 23);
			this.btnDone.TabIndex = 0;
			this.btnDone.Text = "Delay";
			this.btnDone.UseVisualStyleBackColor = true;
			this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(215, 166);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// rbByHour
			// 
			this.rbByHour.AutoSize = true;
			this.rbByHour.Checked = true;
			this.rbByHour.Location = new System.Drawing.Point(17, 19);
			this.rbByHour.Name = "rbByHour";
			this.rbByHour.Size = new System.Drawing.Size(69, 17);
			this.rbByHour.TabIndex = 2;
			this.rbByHour.TabStop = true;
			this.rbByHour.Text = "by a hour";
			this.rbByHour.UseVisualStyleBackColor = true;
			// 
			// rbBy5Hours
			// 
			this.rbBy5Hours.AutoSize = true;
			this.rbBy5Hours.Location = new System.Drawing.Point(17, 42);
			this.rbBy5Hours.Name = "rbBy5Hours";
			this.rbBy5Hours.Size = new System.Drawing.Size(83, 17);
			this.rbBy5Hours.TabIndex = 4;
			this.rbBy5Hours.Text = "by a 5 hours";
			this.rbBy5Hours.UseVisualStyleBackColor = true;
			// 
			// rbByManual
			// 
			this.rbByManual.AutoSize = true;
			this.rbByManual.Location = new System.Drawing.Point(17, 65);
			this.rbByManual.Name = "rbByManual";
			this.rbByManual.Size = new System.Drawing.Size(66, 17);
			this.rbByManual.TabIndex = 5;
			this.rbByManual.Text = "manually";
			this.rbByManual.UseVisualStyleBackColor = true;
			// 
			// rbByCustom
			// 
			this.rbByCustom.AutoSize = true;
			this.rbByCustom.Location = new System.Drawing.Point(17, 88);
			this.rbByCustom.Name = "rbByCustom";
			this.rbByCustom.Size = new System.Drawing.Size(99, 17);
			this.rbByCustom.TabIndex = 6;
			this.rbByCustom.Text = "custom interval:";
			this.rbByCustom.UseVisualStyleBackColor = true;
			this.rbByCustom.CheckedChanged += new System.EventHandler(this.rbByCustom_CheckedChanged);
			// 
			// timeChooser1
			// 
			this.timeChooser1.Enabled = false;
			this.timeChooser1.Location = new System.Drawing.Point(44, 111);
			this.timeChooser1.Name = "timeChooser1";
			this.timeChooser1.Size = new System.Drawing.Size(222, 26);
			this.timeChooser1.TabIndex = 7;
			this.timeChooser1.Load += new System.EventHandler(this.timeChooser1_Load);
			// 
			// gbDelays
			// 
			this.gbDelays.Controls.Add(this.rbByHour);
			this.gbDelays.Controls.Add(this.timeChooser1);
			this.gbDelays.Controls.Add(this.rbBy5Hours);
			this.gbDelays.Controls.Add(this.rbByCustom);
			this.gbDelays.Controls.Add(this.rbByManual);
			this.gbDelays.Location = new System.Drawing.Point(12, 12);
			this.gbDelays.Name = "gbDelays";
			this.gbDelays.Size = new System.Drawing.Size(278, 146);
			this.gbDelays.TabIndex = 8;
			this.gbDelays.TabStop = false;
			this.gbDelays.Text = "Delay notifications...";
			// 
			// frmNotificationsDelay
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(302, 201);
			this.Controls.Add(this.gbDelays);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnDone);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmNotificationsDelay";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Medoc Updates - Delay notifications";
			this.gbDelays.ResumeLayout(false);
			this.gbDelays.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnDone;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.RadioButton rbByHour;
		private System.Windows.Forms.RadioButton rbBy5Hours;
		private System.Windows.Forms.RadioButton rbByManual;
		private System.Windows.Forms.RadioButton rbByCustom;
		private TimeChooser timeChooser1;
		private System.Windows.Forms.GroupBox gbDelays;
	}
}