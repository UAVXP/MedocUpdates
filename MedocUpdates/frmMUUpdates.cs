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

		private async void frmMUUpdates_Load(object sender, EventArgs e)
		{
			if(await MUVersion.Init() == null)
			{
				Log.Write("frmMUUpdates: Cannot retrieve the latest releases from Github. Check your Internet connection");
				return;
			}

			this.remoteVersion = MUVersion.GetRemoteData();
			this.localVersion = MUVersion.GetLocalData();

			if(this.remoteVersion == null || this.localVersion == null)
			{
				Log.Write(String.Format("frmMUUpdates: Cannot get any of the versions (remote is {0}, local is {1})",
										(this.remoteVersion == null ? "null" : "not null"),
										(this.localVersion == null ? "null" : "not null")));
				return;
			}

			int updateState = GetUpdateState();

			switch (updateState)
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
				Log.Write(String.Format("frmMUUpdates: Cannot compare the local version with the remote ({0}). Wrong version format probably ({1}/{2})",
										updateState, this.remoteVersion, this.localVersion));
				break;
			}
		}

		private void ForceUpdate()
		{
			if(MUVersion.LatestRelease == null)
			{
				Log.Write("frmMUUpdates: ForceUpdate(): No Github releases has been loaded");
				return;
			}

			Update update = null;
			try
			{
				update = new Update(MUVersion.LatestRelease);
			}
			catch(Exception ex)
			{
				Log.Write("frmMUUpdates: ForceUpdate(): Cannot get the latest zip release asset URL\r\n" + ex.Message);
				return;
			}

			if(update == null)
			{
				Log.Write("frmMUUpdates: ForceUpdate(): Cannot find a proper update archive at the latest Github release");
				return;
			}

			update.UpdateRoutine();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			DialogResult result;
			int updateState = GetUpdateState();
			switch (updateState)
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
				result = MessageBox.Show(Loc.Get("frmMUUpdates.UpdateButton.DevBuild", "You have a development build of MedocUpdates\r\n\r\nDo you really want to forcibly update this application?"),
														"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (result == DialogResult.Yes)
					ForceUpdate();

				break;
			default:
				Log.Write(String.Format("frmMUUpdates: Update state is wrong ({0})", updateState));
				break;
			}
		}
	}
}
