using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AppUpdater;

namespace MedocUpdates
{
	public partial class frmMUUpdates : Form
	{
		Version remoteVersion;
		Version localVersion;

		public frmMUUpdates()
		{
			InitializeComponent();
		}

		public int GetUpdateState()
		{
			return this.remoteVersion.CompareTo(this.localVersion);
		}

		private void frmMUUpdates_Load(object sender, EventArgs e)
		{
			MUVersion.Init();

			this.remoteVersion = MUVersion.GetRemoteData();
			this.localVersion = MUVersion.GetLocalData();

			switch (GetUpdateState())
			{
			case 0:
				lblUpdateStatus.Text = "No updates for MedocUpdates\r\nVersion " + this.remoteVersion;
				break;
			case 1:
				lblUpdateStatus.Text = String.Format("New build of MedocUpdates was released!\r\n\r\nRemote version: {0}\r\nLocal version: {1}",
														this.remoteVersion, this.localVersion);
				break;
			case -1:
				lblUpdateStatus.Text =
					String.Format("You probably have a development build. It's not recommended to update from the public builds\r\n\r\nRemote version: {0}\r\nLocal version: {1}",
									this.remoteVersion, this.localVersion);
				break;
			default:
				lblUpdateStatus.Text = "Something went wrong";
				break;
			}
		}

		private void ForceUpdate()
		{
			Update update = new Update(MUVersion.LatestRelease);
			update.UpdateRoutine();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			DialogResult result;
			switch (GetUpdateState())
			{
			case 0:
				result = MessageBox.Show("MedocUpdates is already up-to-date.\r\n\r\nDo you really want to forcibly update this application?",
														"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if(result == DialogResult.Yes)
					ForceUpdate();

				break;
			case 1:
				ForceUpdate();
				break;
			case -1:
				result = MessageBox.Show("You have a development build of MedocUpdates\r\n\r\nDo you really want to forcibly update this application?",
														"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (result == DialogResult.Yes)
					ForceUpdate();

				break;
			default:
				break;
			}
		}
	}
}
