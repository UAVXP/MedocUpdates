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

		private void InitializeLocalization()
		{
			this.button1.Text = Loc.Get("frmMUUpdates.button1.Text", "Update");
			this.lblUpdateStatus.Text = Loc.Get("frmMUUpdates.lblUpdateStatus.Text", "Cannot check for MedocUpdates updates");
			this.Text = Loc.Get("frmMUUpdates.Text", "MedocUpdates updates");
		}

		public frmMUUpdates()
		{
			InitializeComponent();
			InitializeLocalization();
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
				lblUpdateStatus.Text = String.Format(Loc.Get("frmMUUpdates.UpdateState.NoUpdates", "No updates for MedocUpdates\r\nVersion {0}"), this.remoteVersion);
				break;
			case 1:
				lblUpdateStatus.Text = String.Format(Loc.Get("frmMUUpdates.UpdateState.NewUpdates", "New build of MedocUpdates was released!\r\n\r\nRemote version: {0}\r\nLocal version: {1}"),
														this.remoteVersion, this.localVersion);
				break;
			case -1:
				lblUpdateStatus.Text =
					String.Format(Loc.Get("frmMUUpdates.UpdateState.DevBuild", "You probably have a development build. It's not recommended to update from the public builds\r\n\r\nRemote version: {0}\r\nLocal version: {1}"),
									this.remoteVersion, this.localVersion);
				break;
			default:
				lblUpdateStatus.Text = Loc.Get("frmMUUpdates.UpdateState.Error", "Something went wrong");
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
				result = MessageBox.Show(Loc.Get("frmMUUpdates.UpdateButton.NoUpdates", "MedocUpdates is already up-to-date.\r\n\r\nDo you really want to forcibly update this application?"),
														"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if(result == DialogResult.Yes)
					ForceUpdate();

				break;
			case 1:
				ForceUpdate();
				break;
			case -1:
				result = MessageBox.Show(Loc.Get("frmMUUpdates.UpdateState.DevBuild", "You have a development build of MedocUpdates\r\n\r\nDo you really want to forcibly update this application?"),
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
