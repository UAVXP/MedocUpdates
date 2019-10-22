using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;
using Syroot.Windows.IO;

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
				//Log.Write(LogLevel.NORMAL, true, "SessionStorage: Converting SessionStorage file from v1 to v2"); // Not needed anymore

				Inside_v2 inside_v2 = new Inside_v2();
				inside_v2.NotificationDelay = v.NotificationDelay;
				return inside_v2;
			}
		}

		[Serializable]
		public class Inside_v3
		{
			public double					NotificationDelay = 1 * 60 * 60 * 1000; // Default notification delay - 1 hour
			public List<MedocTelegramUser>	TelegramUsers = new List<MedocTelegramUser>();
			public bool						LogsEnabled = true;
			public int						LoggingLevel = LogLevel.BASIC; // LogLevel
			public string					TelegramToken = "";
			public string					DownloadsFolderPath = new KnownFolder(KnownFolderType.Downloads).Path;
			public bool						RemoveUpdateFileAfterInstall = true;

			public static implicit operator Inside_v3(Inside_v2 v)
			{
				Log.Write(LogLevel.NORMAL, true, "SessionStorage: Converting SessionStorage file from v2 to v3");

				Inside_v3 inside_v3 = new Inside_v3();
				inside_v3.NotificationDelay = v.NotificationDelay;
				inside_v3.TelegramUsers = v.TelegramUsers;
				inside_v3.LogsEnabled = v.LogsEnabled;
				inside_v3.LoggingLevel = v.LoggingLevel;
				inside_v3.TelegramToken = v.TelegramToken;
				return inside_v3;
			}

			public static implicit operator Inside_v3(Inside v)
			{
				Log.Write(LogLevel.NORMAL, true, "SessionStorage: Converting SessionStorage file from v1 to v3");

				Inside_v3 inside_v3 = new Inside_v3();
				inside_v3.NotificationDelay = v.NotificationDelay;
				return inside_v3;
			}
		}

		private static string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		private static string m_settingsPath = m_exePath + "\\mu.dat";

		public static Inside_v3 inside = new Inside_v3();

		public static event EventHandler NotificationDelayChanged = delegate { };
		public static event EventHandler LogsEnabledChanged = delegate { };
		public static event EventHandler LoggingLevelChanged = delegate { };
		public static event EventHandler TelegramTokenChanged = delegate { };
		public static event EventHandler DownloadsFolderPathChanged = delegate { };
		public static event EventHandler RemoveUpdateFileAfterInstallChanged = delegate { };

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
		public static void DownloadsFolderPathChangedFunc(object sender)
		{
			DownloadsFolderPathChanged.Invoke(sender, new EventArgs());
		}
		public static void RemoveUpdateFileAfterInstallChangedFunc(object sender)
		{
			RemoveUpdateFileAfterInstallChanged.Invoke(sender, new EventArgs());
		}

		public static void Save()
		{
			IFormatter formatter = new BinaryFormatter();
			Stream filestream = new FileStream(m_settingsPath, FileMode.Create, FileAccess.Write);
			if (filestream == null)
			{
				Log.Write(LogLevel.NORMAL, true, "SessionStorage: Cannot save session storage. Check your permissions");
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
				Log.Write(LogLevel.NORMAL, true, "SessionStorage: Cannot load session storage file. Check your permissions");

				// Initializing and saving a default values
				Save();

				return;
			}

			if (filestream.Length <= 0)
			{
				Log.Write(LogLevel.NORMAL, true, "SessionStorage: Cannot load saved session");
				filestream.Close();

				// Initializing and saving a default values
				Save();

				return;
			}


			SessionStorage.Inside_v3 temp = new Inside_v3();

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
				case "Inside_v3":
					temp = (SessionStorage.Inside_v3)deserialized;
					break;
				}
			}
			catch (SerializationException ex)
			{
				Log.Write(LogLevel.NORMAL, true, "SessionStorage: Cannot retrieve settings - wrong file structure\r\n" + ex.Message);
			}

			filestream.Close();
			
			inside = temp;
		}
	}
}
