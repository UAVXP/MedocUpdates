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

		[Serializable]
		public class Inside_v2
		{
			public double NotificationDelay = 1 * 60 * 60 * 1000; // Default notification delay - 1 hour
			public List<MedocTelegramUser> TelegramUsers = new List<MedocTelegramUser>();
			public bool LogsEnabled = true;
			public int LoggingLevel = LogLevel.BASIC; // LogLevel
			public string TelegramToken = "";

			public static implicit operator Inside_v2(Inside v)
			{
				Inside_v2 inside_v2 = new Inside_v2();
				inside_v2.NotificationDelay = v.NotificationDelay;
				return inside_v2;
			}
		}

		private static string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		private static string m_settingsPath = m_exePath + "\\mu.dat";

		public static Inside_v2 inside = new Inside_v2();

		public static event EventHandler NotificationDelayChanged = delegate { };
		public static event EventHandler LogsEnabledChanged = delegate { };
		public static event EventHandler LoggingLevelChanged = delegate { };
		public static event EventHandler TelegramTokenChanged = delegate { };

		// TODO: Maybe do this in properties and not calling ...ChangedFunc?
		// Actually, then I cannot use sender object
		// Or INotifyPropertyChanged
		public static void NotificationDelayChangedFunc(object sender)
		{
			NotificationDelayChanged.Invoke(sender, new EventArgs());
		}
		public static void LogsEnabledChangedFunc(object sender)
		{
			LogsEnabledChanged.Invoke(sender, new EventArgs());
		}
		public static void LoggingLevelChangedFunc(object sender)
		{
			LoggingLevelChanged.Invoke(sender, new EventArgs());
		}
		public static void TelegramTokenChangedFunc(object sender)
		{
			TelegramTokenChanged.Invoke(sender, new EventArgs());
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


			SessionStorage.Inside_v2 temp = new Inside_v2();

			try
			{
				object deserialized = formatter.Deserialize(filestream);
				Type insideType = deserialized.GetType();

				switch (insideType.Name)
				{
				case "Inside":
					temp = (SessionStorage.Inside)deserialized;
					break;
				case "Inside_v2":
					temp = (SessionStorage.Inside_v2)deserialized;
					break;
				}
			}
			catch (SerializationException ex)
			{
				Log.Write(LogLevel.NORMAL, "SessionStorage: Cannot retrieve settings - wrong file structure\r\n" + ex.Message);
			}

			filestream.Close();
			
			inside = temp;
		}
	}
}
