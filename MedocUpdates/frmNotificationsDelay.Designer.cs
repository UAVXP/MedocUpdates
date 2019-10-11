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
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.rbByHour = new System.Windows.Forms.RadioButton();
			this.rbBy5Hours = new System.Windows.Forms.RadioButton();
			this.rbByManual = new System.Windows.Forms.RadioButton();
			this.rbByCustom = new System.Windows.Forms.RadioButton();
			this.timeChooser1 = new MedocUpdates.TimeChooser();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(134, 166);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Delay";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(215, 166);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// rbByHour
			// 
			this.rbByHour.AutoSize = true;
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
			this.rbBy5Hours.TabStop = true;
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
			this.rbByManual.TabStop = true;
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
			this.rbByCustom.TabStop = true;
			this.rbByCustom.Text = "custom interval:";
			this.rbByCustom.UseVisualStyleBackColor = true;
			// 
			// timeChooser1
			// 
			this.timeChooser1.Location = new System.Drawing.Point(44, 111);
			this.timeChooser1.Name = "timeChooser1";
			this.timeChooser1.Size = new System.Drawing.Size(222, 26);
			this.timeChooser1.TabIndex = 7;
			this.timeChooser1.Load += new System.EventHandler(this.timeChooser1_Load);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.rbByHour);
			this.groupBox2.Controls.Add(this.timeChooser1);
			this.groupBox2.Controls.Add(this.rbBy5Hours);
			this.groupBox2.Controls.Add(this.rbByCustom);
			this.groupBox2.Controls.Add(this.rbByManual);
			this.groupBox2.Location = new System.Drawing.Point(12, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(278, 146);
			this.groupBox2.TabIndex = 8;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Delay notifications...";
			// 
			// frmNotificationsDelay
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(302, 201);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmNotificationsDelay";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Medoc Updates - Delay notifications";
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.RadioButton rbByHour;
		private System.Windows.Forms.RadioButton rbBy5Hours;
		private System.Windows.Forms.RadioButton rbByManual;
		private System.Windows.Forms.RadioButton rbByCustom;
		private TimeChooser timeChooser1;
		private System.Windows.Forms.GroupBox groupBox2;
	}
}