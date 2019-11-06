namespace MedocUpdates
{
	partial class frmMUUpdates
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMUUpdates));
			this.button1 = new System.Windows.Forms.Button();
			this.lblUpdateStatus = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(129, 137);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Update";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// lblUpdateStatus
			// 
			this.lblUpdateStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblUpdateStatus.Location = new System.Drawing.Point(30, 28);
			this.lblUpdateStatus.Name = "lblUpdateStatus";
			this.lblUpdateStatus.Size = new System.Drawing.Size(277, 91);
			this.lblUpdateStatus.TabIndex = 1;
			this.lblUpdateStatus.Text = "label1";
			this.lblUpdateStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// frmMUUpdates
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(337, 175);
			this.Controls.Add(this.lblUpdateStatus);
			this.Controls.Add(this.button1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmMUUpdates";
			this.Text = "MedocUpdates updates";
			this.Load += new System.EventHandler(this.frmMUUpdates_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label lblUpdateStatus;
	}
}