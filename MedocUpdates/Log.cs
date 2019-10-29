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
		private static bool m_bInit = false;
		private static string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		private static string m_logPath = Path.Combine(m_exePath, "mu_logs", String.Format("log_{0}.txt", DateTime.Now.ToString("yyyy-MM-dd")));
		private static bool m_Enabled = SessionStorage.inside.LogsEnabled;
		private static int m_logLevel = SessionStorage.inside.LoggingLevel;

		public static void Init()
		{
			// If initialized already - don't do this again
			if (m_bInit)
				return;

			try
			{
				// Make sure log directory does exist
				string logDirectory = Path.GetDirectoryName(m_logPath);
				if (!Directory.Exists(logDirectory))
					Directory.CreateDirectory(logDirectory);
			}
			catch(Exception ex)
			{
				LogFallbackInternal(FormatLogMessage("Log: Cannot create log directory"), FormatLogMessage(ex.Message));
				return;
			}

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

			m_bInit = true;
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

		private static string FormatLogMessage(string logMessage, int logLevel = LogLevel.BASIC)
		{
			return String.Format("{0}\t{1}\t\t{2}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.fff"), logLevel, logMessage);
		}

		public static void Write(int logLevel, bool forceConsole, string logMessage)
		{
			if(forceConsole)
			{
				string[] parsed = logMessage.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
				int i = 0;
				foreach (string rawline in parsed)
				{
					string line = rawline;
					if (i > 0 && line.Length > 0)
						line = "\t" + line;

					Console.WriteLine(line);

					i++;
				}
			}

			Write(LogLevel.BASIC, logMessage);
		}

		public static void Write(int logLevel, string logMessage)
		{
			if(!m_bInit)
			{
				// If not initialized - something was probably wrong with creating a log directory.
				// Falling back to console method
				LogFallbackInternal(FormatLogMessage(logMessage, logLevel));
				return;
			}

			if(!m_Enabled)
				return;

			if(logLevel > m_logLevel)
				return;

			try
			{
				using (StreamWriter w = File.AppendText(m_logPath))
				{
					string[] parsed = logMessage.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
					int i = 0;
					foreach (string rawline in parsed)
					{
						string line = rawline;

						// Add an extra tab char if the message have more than one line
						if (i > 0 && line.Length > 0)
							line = "\t" + line;

						LogInternal(line, w, logLevel);

						i++;
					}
				}
			}
			catch (Exception ex)
			{
				LogFallbackInternal(FormatLogMessage(logMessage, logLevel), FormatLogMessage(ex.Message));
			}
		}

		public static void LogInternal(string logMessage, TextWriter txtWriter, int logLevel)
		{
			string endMessage = FormatLogMessage(logMessage, logLevel);
			try
			{
				txtWriter.WriteLine(endMessage);
			}
			catch (Exception ex)
			{
				LogFallbackInternal(endMessage, FormatLogMessage(ex.Message));
			}
		}

		private static void LogFallbackInternal(string logMessage, string errorMessage = "")
		{
			Console.Out.WriteLine(logMessage);

			if(errorMessage.Trim().Length > 0)
				Console.Error.WriteLine(errorMessage);
		}
	}
}
