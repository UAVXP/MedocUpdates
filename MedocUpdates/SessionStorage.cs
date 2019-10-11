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
			public double NotificationDelay = 1 * 60 * 60 * 1000; // Default notification delay - 1 hour
		}

		public static Inside inside = new Inside();
		static Log log = new Log();

		public static event EventHandler NotificationDelayChanged = delegate { };
		public static void NotificationDelayChangedFunc(object sender)
		{
			NotificationDelayChanged.Invoke(sender, new EventArgs());
		}

		public static void Save()
		{
			IFormatter formatter = new BinaryFormatter();
			Stream filestream = new FileStream("settings.dat", FileMode.Create, FileAccess.Write);
			if (filestream == null)
			{
				log.Write("SessionStorage: Cannot save session storage. Check your permissions");
				return;
			}

			formatter.Serialize(filestream, inside);
			filestream.Close();
		}

		public static void Restore()
		{
			IFormatter formatter = new BinaryFormatter();
			Stream filestream = new FileStream("settings.dat", FileMode.OpenOrCreate, FileAccess.Read);
			if (filestream == null)
			{
				log.Write("SessionStorage: Cannot load session storage file. Check your permissions");

				// Initializing and saving a default values
				Save();

				return;
			}

			if (filestream.Length <= 0)
			{
				log.Write("SessionStorage: Cannot load saved session");
				filestream.Close();

				// Initializing and saving a default values
				Save();

				return;
			}

			SessionStorage.Inside test = (SessionStorage.Inside)formatter.Deserialize(filestream);
			filestream.Close();

			inside = test;
		}
	}
}
