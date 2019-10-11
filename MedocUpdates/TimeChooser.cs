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
		public int Hours { get; set; }
		public int Minutes { get; set; }
		public int Seconds { get; set; }

	//	public event EventHandler<TimeChangedEventArgs> TimeChanged = delegate { };
		public event EventHandler TimeChanged = delegate { };
		public event EventHandler HoursChanged = delegate { };
		public event EventHandler MinutesChanged = delegate { };
		public event EventHandler SecondsChanged = delegate { };

		public TimeChooser()
		{
			InitializeComponent();

			this.Hours = 0;
			this.Minutes = 0;
			this.Seconds = 0;
		}

		public TimeChooser(int hours, int minutes, int seconds) : base()
		{
			this.Hours = hours;
			this.Minutes = minutes;
			this.Seconds = seconds;
		}

		public int ToMilliseconds()
		{
			return this.Hours * this.Minutes * this.Seconds * 1000; // TODO: Is wrong
		}

		public override string ToString()
		{
			return String.Format("{0}:{1}:{2}", this.Hours.ToString("D"), this.Minutes.ToString("D2"), this.Seconds.ToString("D2"));
		}

		private void nudHours_ValueChanged(object sender, EventArgs e)
		{
			this.Hours = Decimal.ToInt32(nudHours.Value);

			TimeChanged.Invoke(this, new EventArgs());
			HoursChanged.Invoke(this, new EventArgs());
		}

		private void nudMinutes_ValueChanged(object sender, EventArgs e)
		{
			this.Minutes = Decimal.ToInt32(nudMinutes.Value);

			TimeChanged.Invoke(this, new EventArgs());
			MinutesChanged.Invoke(this, new EventArgs());
		}

		private void nudSeconds_ValueChanged(object sender, EventArgs e)
		{
			this.Seconds = Decimal.ToInt32(nudSeconds.Value);

			TimeChanged.Invoke(this, new EventArgs());
			SecondsChanged.Invoke(this, new EventArgs());
		}
	}
}
