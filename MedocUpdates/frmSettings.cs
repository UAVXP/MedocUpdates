using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedocUpdates
{
	public partial class frmSettings : Form
	{
		private UpdateTime[] updateTimes = new UpdateTime[]
		{
			new UpdateTime("Every 5 minutes", 5 * 60 * 1000),
			new UpdateTime("Every 30 minutes", 30 * 60 * 1000),
			new UpdateTime("Every 1 hour", 1 * 60 * 60 * 1000),
			new UpdateTime("Every 2 hours", 2 * 60 * 60 * 1000),
			new UpdateTime("Every 5 hours", 5 * 60 * 60 * 1000),
			new UpdateTime("Every 12 hours", 12 * 60 * 60 * 1000),
			new UpdateTime("Every 24 hours", 24 * 60 * 60 * 1000),
		};

		public frmSettings()
		{
			InitializeComponent();
		}

		private void frmSettings_Load(object sender, EventArgs e)
		{
			foreach(UpdateTime ut in updateTimes)
			{
				cbUpdateTimes.Items.Add(ut.desc);
			}
		}

		private void cbUpdateChecks_CheckedChanged(object sender, EventArgs e)
		{
			cbUpdateTimes.Enabled = cbUpdateChecks.Checked;
		}
	}

	class UpdateTime
	{
		public string desc;
		public int value;

		public UpdateTime(string desc, int value)
		{
			this.desc = desc;
			this.value = value;
		}
	}
}
