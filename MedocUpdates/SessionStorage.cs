using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;

namespace MedocUpdates
{
	public static class SessionStorage
	{
		[Serializable]
		public class Inside
		{
			public double NotificationDelay = 1 * 60 * 60 * 1000; // Default notification delay - 1 hour
		}

		private static string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		private static string m_settingsPath = m_exePath + "\\mu.dat";

		public static Inside inside = new Inside();

		public static event EventHandler NotificationDelayChanged = delegate { };
		public static void NotificationDelayChangedFunc(object sender)
		{
			NotificationDelayChanged.Invoke(sender, new EventArgs());
		}

		public static void Save()
		{
			IFormatter formatter = new BinaryFormatter();
			Stream filestream = new FileStream(m_settingsPath, FileMode.Create, FileAccess.Write);
			if (filestream == null)
			{
				Log.Write(LogLevel.NORMAL, "SessionStorage: Cannot save session storage. Check your permissions");
				return;
			}

			formatter.Serialize(filestream, inside);
			filestream.Close();
		}

		public static void Restore()
		{
			IFormatter formatter = new BinaryFormatter();
			Stream filestream = new FileStream(m_settingsPath, FileMode.OpenOrCreate, FileAccess.Read);
			if (filestream == null)
			{
				Log.Write(LogLevel.NORMAL, "SessionStorage: Cannot load session storage file. Check your permissions");

				// Initializing and saving a default values
				Save();

				return;
			}

			if (filestream.Length <= 0)
			{
				Log.Write(LogLevel.NORMAL, "SessionStorage: Cannot load saved session");
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
