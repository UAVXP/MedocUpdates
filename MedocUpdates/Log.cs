using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Reflection;

namespace MedocUpdates
{
	public static class LogLevel
	{
		public const int BASIC = 0;
		public const int NORMAL = 1;
		public const int EXPERT = 2;
		public const int MAXLOGLEVELS = EXPERT + 1;

		// TODO: Refactor
		public static int Clamp(int input, int min, int max)
		{
			if (input.CompareTo(min) < 0)
				return min;
			else if (input.CompareTo(max) > 0)
				return max;

			return input;
		}

		public static string GetName(int level)
		{
			switch(level)
			{
			case LogLevel.BASIC:
				return "Basic";
			case LogLevel.NORMAL:
				return "Normal";
			case LogLevel.EXPERT:
				return "Expert";
			default:
				return "";
			}
		}
	}

	public static class Log
	{
		private static string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		private static string m_logPath = m_exePath + "\\mu_logs\\" + String.Format("log_{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"));
		private static bool m_Enabled = SessionStorage.inside.LogsEnabled;
		private static int m_logLevel = SessionStorage.inside.LoggingLevel;

		public static void Init()
		{
			// Make sure log directory does exist
			string logDirectory = Path.GetDirectoryName(m_logPath);
			if (!Directory.Exists(logDirectory))
				Directory.CreateDirectory(logDirectory);

			string logLevelStr = ParsedArgs.GetArgument("loglevel");
			if (logLevelStr.Length > 0)
			{
				int logLevel = m_logLevel;
				if (int.TryParse(logLevelStr, out logLevel))
				{
					m_logLevel = LogLevel.Clamp(logLevel, LogLevel.BASIC, LogLevel.MAXLOGLEVELS - 1);
					Log.Write("Log: Level was forcibly set to " + LogLevel.GetName(m_logLevel));
				}
			}
		}

		// FIXME: Why not property?
		public static void SetEnabled(bool enabled)
		{
			if(!enabled)
				Log.Write("Log: State changed to Disabled");

			m_Enabled = enabled;

			if (enabled)
				Log.Write("Log: State changed to Enabled");
		}

		// FIXME: Why not property?
		public static void SetLevel(int level) // LogLevel
		{
			Log.Write("Log: Level set to " + LogLevel.GetName(m_logLevel));
			m_logLevel = level;
		}

		public static void Write(string logMessage)
		{
			Write(LogLevel.BASIC, logMessage);
		}

		public static void Write(int logLevel, bool forceConsole, string logMessage)
		{
			if(forceConsole)
				Console.WriteLine(logMessage); // TODO: Make the same EOL stuff as in Write

			Write(LogLevel.BASIC, logMessage);
		}

		public static void Write(int logLevel, string logMessage)
		{
			if(!m_Enabled)
				return;

			if(logLevel > m_logLevel)
				return;

			try
			{
				using (StreamWriter w = File.AppendText(m_logPath))
				{
					string[] parsed = logMessage.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
					int i = 0;
					foreach (string rawline in parsed)
					{
						string line = rawline;
						if (i > 0)
							line = "\t" + line;

						LogInternal(line, w);

						i++;
					}
				}
			}
			catch (Exception ex)
			{
				LogFallbackInternal(ex.Message);
			}
		}

		public static void LogInternal(string logMessage, TextWriter txtWriter)
		{
			try
			{
				txtWriter.WriteLine("{0}\t\t\t{1}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.fff"), logMessage);
			}
			catch (Exception ex)
			{
				LogFallbackInternal(ex.Message);
			}
		}

		public static void LogFallbackInternal(string logMessage)
		{
			Console.Error.WriteLine(logMessage);
		}
	}
}
