using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Reflection;

namespace MedocUpdates
{
	public static class Log
	{
		private static string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		private static string m_logPath = m_exePath + "\\mu_logs\\" + String.Format("log_{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"));

		public static void Init()
		{
			string logDirectory = Path.GetDirectoryName(m_logPath);
			if (!Directory.Exists(logDirectory))
				Directory.CreateDirectory(logDirectory);
		}

		public static void Write(string logMessage)
		{
			// TODO: Implement different log levels (every message, standard, a little bit)
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
