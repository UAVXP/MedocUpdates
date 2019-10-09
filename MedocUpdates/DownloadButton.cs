using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;

namespace MedocUpdates
{
	public partial class DownloadButton : UserControl
	{
		MedocDownloadItem item;
		public bool IsHighlighted { get; set; }

		public DownloadButton(MedocDownloadItem item)
		{
			InitializeComponent();

			this.item = item;
			this.labelVersion.Text = item.version;

			
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(item.link);
		}

		private void DownloadButton_Load(object sender, EventArgs e)
		{
			if (this.IsHighlighted)
			{
				this.BackColor = SystemColors.Highlight;
				linkLabel1.LinkColor = Color.Yellow;
				linkLabel1.Font = new Font(linkLabel1.Font.Name, linkLabel1.Font.Size, FontStyle.Bold);
			}
		}
	}
}
