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
	public partial class frmNotificationsDelay : Form
	{
		public double Delay
		{
			get
			{
				if(rbByHour.Checked)
					return 1 * 60 * 60 * 1000;
				else if(rbBy5Hours.Checked)
					return 5 * 60 * 60 * 1000;
				else if(rbByManual.Checked)
					return 0;
				else if(rbByCustom.Checked)
					return timeChooser1.ToMilliseconds();

				return 0;
			}

			set
			{
				timeChooser1.Delay = TimeSpan.FromMilliseconds(value);

				switch (value)
				{
				case (1 * 60 * 60 * 1000):
					rbByHour.Checked = true;
					break;
				case (5 * 60 * 60 * 1000):
					rbBy5Hours.Checked = true;
					break;
				case 0:
			//	case 1: // Hate this (see frmMain_NotificationDelayChanged) // Okay, fixed (see delayNotificationsToolStripMenuItem_Click)
					rbByManual.Checked = true;
					break;
				default:
					rbByCustom.Checked = true;
					break;
				}
			}
		}

		public frmNotificationsDelay()
		{
			InitializeComponent();
		}

		private void timeChooser1_Load(object sender, EventArgs e)
		{
			timeChooser1.Enabled = rbByCustom.Checked;
			timeChooser1.TimeChanged += FrmNotificationsDelay_TimeChanged;
		}

		private void FrmNotificationsDelay_TimeChanged(object sender, EventArgs e)
		{
			TimeChooser tc = (sender as TimeChooser);
			Console.WriteLine("{0} ({1}ms)", tc.ToString(), tc.ToMilliseconds());
		}

		private void rbByCustom_CheckedChanged(object sender, EventArgs e)
		{
			timeChooser1.Enabled = rbByCustom.Checked;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnDone_Click(object sender, EventArgs e)
		{
			SessionStorage.inside.NotificationDelay = this.Delay;
			SessionStorage.NotificationDelayChangedFunc(this);
			this.Close();
		}
	}
}
