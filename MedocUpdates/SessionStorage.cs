using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MedocUpdates
{
	// TODO: Implement saving this storage
	
	public static class SessionStorage
	{
		[Serializable]
		public class Inside
		{
			public double NotificationDelay = 0;
		}

		public static Inside inside = new Inside();
		
		public static event EventHandler NotificationDelayChanged = delegate { };
		public static void NotificationDelayChangedFunc(object sender)
		{
			NotificationDelayChanged.Invoke(sender, new EventArgs());
		}

		public static void Save()
		{
			IFormatter formatter = new BinaryFormatter();
			Stream filestream = new FileStream("settings.dat", FileMode.Create, FileAccess.Write);
			formatter.Serialize(filestream, inside);
			filestream.Close();
		}

		public static void Restore()
		{
			IFormatter formatter = new BinaryFormatter();
			Stream filestream = new FileStream("settings.dat", FileMode.Open, FileAccess.Read);
			SessionStorage.Inside test = (SessionStorage.Inside)formatter.Deserialize(filestream);
			inside = test;
		}
	}
}
