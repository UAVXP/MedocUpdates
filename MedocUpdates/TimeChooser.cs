using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedocUpdates
{
	public partial class TimeChooser : UserControl
	{
		private TimeSpan delay;
		public TimeSpan Delay
		{
			get { return delay; }
			set
			{
				delay = value;
				TimeChanged.Invoke(this, new EventArgs());

				// Updating UI
				nudHours.Value = delay.Hours;
				nudMinutes.Value = delay.Minutes;
				nudSeconds.Value = delay.Seconds;
			}
		}

	//	public event EventHandler<TimeChangedEventArgs> TimeChanged = delegate { };
		public event EventHandler TimeChanged = delegate { };
		public event EventHandler HoursChanged = delegate { };
		public event EventHandler MinutesChanged = delegate { };
		public event EventHandler SecondsChanged = delegate { };

		public TimeChooser()
		{
			InitializeComponent();

			this.Delay = new TimeSpan();
		}

		public TimeChooser(int hours, int minutes, int seconds) : base()
		{
			this.Delay = new TimeSpan(hours, minutes, seconds);
		}

		public override string ToString()
		{
			return this.Delay.ToString();
		}

		public double ToMilliseconds()
		{
			return this.Delay.TotalMilliseconds;
		}

		private void nudHours_ValueChanged(object sender, EventArgs e)
		{
			int hours = Decimal.ToInt32(nudHours.Value);
			TimeSpan ts = new TimeSpan(hours, this.Delay.Minutes, this.Delay.Seconds);
			this.Delay = ts;

			HoursChanged.Invoke(this, new EventArgs());
		}

		private void nudMinutes_ValueChanged(object sender, EventArgs e)
		{
			int minutes = Decimal.ToInt32(nudMinutes.Value);
			TimeSpan ts = new TimeSpan(this.Delay.Hours, minutes, this.Delay.Seconds);
			this.Delay = ts;

			MinutesChanged.Invoke(this, new EventArgs());
		}

		private void nudSeconds_ValueChanged(object sender, EventArgs e)
		{
			int seconds = Decimal.ToInt32(nudSeconds.Value);
			TimeSpan ts = new TimeSpan(this.Delay.Hours, this.Delay.Minutes, seconds);
			this.Delay = ts;

			SecondsChanged.Invoke(this, new EventArgs());
		}
	}
}
