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
	}
}
