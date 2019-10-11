namespace MedocUpdates
{
	partial class TimeChooser
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
			this.nudHours = new System.Windows.Forms.NumericUpDown();
			this.nudMinutes = new System.Windows.Forms.NumericUpDown();
			this.nudSeconds = new System.Windows.Forms.NumericUpDown();
			this.lblHours = new System.Windows.Forms.Label();
			this.lblMinutes = new System.Windows.Forms.Label();
			this.lblSeconds = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nudHours)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMinutes)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSeconds)).BeginInit();
			this.SuspendLayout();
			// 
			// nudHours
			// 
			this.nudHours.Location = new System.Drawing.Point(3, 3);
			this.nudHours.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
			this.nudHours.Name = "nudHours";
			this.nudHours.Size = new System.Drawing.Size(40, 20);
			this.nudHours.TabIndex = 0;
			this.nudHours.ValueChanged += new System.EventHandler(this.nudHours_ValueChanged);
			// 
			// nudMinutes
			// 
			this.nudMinutes.Location = new System.Drawing.Point(80, 3);
			this.nudMinutes.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
			this.nudMinutes.Name = "nudMinutes";
			this.nudMinutes.Size = new System.Drawing.Size(40, 20);
			this.nudMinutes.TabIndex = 1;
			this.nudMinutes.ValueChanged += new System.EventHandler(this.nudMinutes_ValueChanged);
			// 
			// nudSeconds
			// 
			this.nudSeconds.Location = new System.Drawing.Point(157, 3);
			this.nudSeconds.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
			this.nudSeconds.Name = "nudSeconds";
			this.nudSeconds.Size = new System.Drawing.Size(40, 20);
			this.nudSeconds.TabIndex = 2;
			this.nudSeconds.ValueChanged += new System.EventHandler(this.nudSeconds_ValueChanged);
			// 
			// lblHours
			// 
			this.lblHours.AutoSize = true;
			this.lblHours.Location = new System.Drawing.Point(49, 5);
			this.lblHours.Name = "lblHours";
			this.lblHours.Size = new System.Drawing.Size(16, 13);
			this.lblHours.TabIndex = 3;
			this.lblHours.Text = "h.";
			// 
			// lblMinutes
			// 
			this.lblMinutes.AutoSize = true;
			this.lblMinutes.Location = new System.Drawing.Point(126, 5);
			this.lblMinutes.Name = "lblMinutes";
			this.lblMinutes.Size = new System.Drawing.Size(18, 13);
			this.lblMinutes.TabIndex = 4;
			this.lblMinutes.Text = "m.";
			// 
			// lblSeconds
			// 
			this.lblSeconds.AutoSize = true;
			this.lblSeconds.Location = new System.Drawing.Point(203, 5);
			this.lblSeconds.Name = "lblSeconds";
			this.lblSeconds.Size = new System.Drawing.Size(15, 13);
			this.lblSeconds.TabIndex = 5;
			this.lblSeconds.Text = "s.";
			// 
			// TimeChooser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblSeconds);
			this.Controls.Add(this.lblMinutes);
			this.Controls.Add(this.lblHours);
			this.Controls.Add(this.nudSeconds);
			this.Controls.Add(this.nudMinutes);
			this.Controls.Add(this.nudHours);
			this.Name = "TimeChooser";
			this.Size = new System.Drawing.Size(222, 26);
			((System.ComponentModel.ISupportInitialize)(this.nudHours)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMinutes)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSeconds)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NumericUpDown nudHours;
		private System.Windows.Forms.NumericUpDown nudMinutes;
		private System.Windows.Forms.NumericUpDown nudSeconds;
		private System.Windows.Forms.Label lblHours;
		private System.Windows.Forms.Label lblMinutes;
		private System.Windows.Forms.Label lblSeconds;
	}
}
