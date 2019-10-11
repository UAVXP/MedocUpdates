using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedocUpdates
{
	public static class SessionStorage
	{
		public static double NotificationDelay = 0;
		public static event EventHandler NotificationDelayChanged = delegate { };
		public static void NotificationDelayChangedFunc(object sender)
		{
			NotificationDelayChanged.Invoke(sender, new EventArgs());
		}
	}
}
