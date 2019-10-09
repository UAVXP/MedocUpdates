using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;

namespace MedocUpdates
{
	class MedocInternal
	{
		public string LocalVersion
		{
			get
			{
				/*
					RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\M.E.Doc\\M.E.Doc");
					if (key == null)
						return "";

					return key.GetSubKeyNames()[0]; // FIXME: Might be wrong maybe? What if there would be many subkeys before the version one?
				*/

				string version = "";
				GetDSTVersion(out version);
				return version;
			}
		}

		internal bool GetInstallationPath(out string path)
		{
			path = "";

			// Getting an installation path
			string[] regPaths = new string[2] {
				"SOFTWARE\\IntellectService\\BusinessDoc1",
				"SOFTWARE\\Wow6432Node\\IntellectService\\BusinessDoc1",
			};

			RegistryKey key;
			key = Registry.LocalMachine.OpenSubKey(regPaths[0]);
			if (key != null)
			{
				path = key.GetValue("PATH").ToString();
				return true;
			}

			key = Registry.LocalMachine.OpenSubKey(regPaths[1]);
			if (key != null)
			{
				path = key.GetValue("PATH").ToString();
				return true;
			}

			key = Registry.CurrentUser.OpenSubKey(regPaths[0]);
			if (key != null)
			{
				path = key.GetValue("PATH").ToString();
				return true;
			}

			key = Registry.CurrentUser.OpenSubKey(regPaths[1]);
			if (key != null)
			{
				path = key.GetValue("PATH").ToString();
				return true;
			}

			return false;
		}

		internal bool GetLatestLog(out string filename)
		{
			filename = "";

			string installPath = "";
			if (!GetInstallationPath(out installPath))
				return false;

			DateTime lastdt = DateTime.MinValue;
			string[] files = Directory.GetFiles(installPath, "update_*.log", SearchOption.AllDirectories);
			foreach (string file in files)
			{
				DateTime dt = lastdt;
				string filedate = "";

				filedate = Path.GetFileNameWithoutExtension(file);
				filedate = filedate.Remove(0, "update_".Length);

				if (DateTime.TryParse(filedate, out dt))
				{
					// Try to find the latest log
					// Usually the last "update_" file in directory, but still, better check
					if (dt > lastdt)
					{
						Console.WriteLine("Found log dated {0}", dt.ToString("yyyy-MM-dd"));
						lastdt = dt;

						filename = file;
					}
				}
			}

			return true;
		}

		private bool GetLine(string line, string forSearch)
		{
			try
			{
				line = line.Trim();
				int forSearchLength = forSearch.Length;
				int findMaterial = line.IndexOf(forSearch);
				if (findMaterial < 0) // If there's no forSearch in line
					return false;

				return true;
			}
			catch { }

			return false;
		}

		private bool GetValue(string line, string forSearch, out string value)
		{
			value = "";

			try
			{
				line = line.Trim();
				int forSearchLength = forSearch.Length;
				int findMaterial = line.IndexOf(forSearch);
				if (findMaterial < 0) // If there's no forSearch in line
					return false;

				value = line.Substring(findMaterial + forSearchLength).Replace('/', '\\').Trim(trimChars: '"');

				return true;
			}
			catch { }

			return false;
		}

		internal bool GetDSTVersion(out string version)
		{
			version = "";

			string logfile = "";
			if (!GetLatestLog(out logfile))
				return false;

			string[] file = new string[] { };
			try
			{
				file = File.ReadAllLines(logfile);
			}
			catch { }

			string value = "";
			for (int i = 0; i < file.Length; i++ )
			{
				string line = file[i];

				bool foundDSTVersionString = GetLine(line, "DSTVERSION");
				if (!foundDSTVersionString)
					continue;

				// next line should be the version now
				if ((i+1) >= file.Length)
					continue;
				line = file[i+1];

				bool foundDSTVersion = GetValue(line, ": ", out value);
				if (!foundDSTVersion)
					continue;
			}

			version = value; // Always take the latest value

			Regex regex = new Regex(@"\d{2}.\d{2}.\d{3}");
			if (!regex.IsMatch(version))
			{
				version = "";
				return false;
			}


			return true;
		}
	}
}
