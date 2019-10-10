using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Reflection;

namespace MedocUpdates
{
	public class Log
	{
		private string m_exePath = String.Empty;
		private string m_logPath = String.Empty;

		public Log()
		{
			m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			string filename = String.Format("log_{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"));
			m_logPath = m_exePath + "\\mu_logs\\" + filename;

			string logDirectory = Path.GetDirectoryName(m_logPath);
			if (!Directory.Exists(logDirectory))
				Directory.CreateDirectory(logDirectory);
		}

		public Log(string logMessage) : base()
		{
			Write(logMessage);
		}

		public void Write(string logMessage)
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
			}
		}

		public void LogInternal(string logMessage, TextWriter txtWriter)
		{
			try
			{
				txtWriter.WriteLine("{0}\t\t\t{1}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.fff"), logMessage);
			}
			catch (Exception ex)
			{
			}
		}
	}
}
