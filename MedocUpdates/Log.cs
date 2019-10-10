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

		public Log()
		{
			m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
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
				using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
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
				txtWriter.Write("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
				txtWriter.WriteLine(":\t\t{0}", logMessage);
			//	txtWriter.WriteLine("-------------------------------");
			}
			catch (Exception ex)
			{
			}
		}
	}
}
