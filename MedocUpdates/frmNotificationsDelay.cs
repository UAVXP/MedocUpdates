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
		public frmNotificationsDelay()
		{
			InitializeComponent();
		}

		private void timeChooser1_Load(object sender, EventArgs e)
		{
			timeChooser1.TimeChanged += FrmNotificationsDelay_TimeChanged;
		}

		private void FrmNotificationsDelay_TimeChanged(object sender, EventArgs e)
		{
			TimeChooser tc = (sender as TimeChooser);
			Console.WriteLine("{0} ({1}ms)", tc.ToString(), tc.ToMilliseconds());
		}
	}
}
