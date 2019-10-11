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
		string cachedVersion;
		int dstVersionCount;

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

				if (cachedVersion.Equals(""))
				{
					Log.Write("MedocInternal: Retrieving local version");
					GetDSTVersion(out cachedVersion); // TODO: Do a fallback way - through Software\\M.E.Doc\\M.E.Doc subkey name
				}
				return cachedVersion;
			}
		}

		public MedocInternal()
		{
			this.cachedVersion = "";
			this.dstVersionCount = 0;
		}

		private void InvalidateCache()
		{
			this.cachedVersion = "";
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

			foreach(string regPath in regPaths)
			{
				key = Registry.LocalMachine.OpenSubKey(regPath);
				if (key != null)
				{
					object keyValue = key.GetValue("PATH");
					if(keyValue == null)
						return false;

					path = keyValue.ToString();
					return true;
				}
			}

			foreach (string regPath in regPaths)
			{
				key = Registry.CurrentUser.OpenSubKey(regPath);
				if (key != null)
				{
					object keyValue = key.GetValue("PATH");
					if (keyValue == null)
						return false;

					path = keyValue.ToString();
					return true;
				}
			}

			return false;
		}

		internal bool GetAppdataPath(out string path)
		{
			path = "";

			// Getting an installation path
			string[] regPaths = new string[2] {
				"SOFTWARE\\IntellectService\\BusinessDoc1",
				"SOFTWARE\\Wow6432Node\\IntellectService\\BusinessDoc1",
			};

			RegistryKey key;

			foreach (string regPath in regPaths)
			{
				key = Registry.LocalMachine.OpenSubKey(regPath);
				if (key != null)
				{
					object keyValue = key.GetValue("APPDATA");
					if (keyValue == null)
						return false;

					path = keyValue.ToString();
					return true;
				}
			}

			foreach (string regPath in regPaths)
			{
				key = Registry.CurrentUser.OpenSubKey(regPath);
				if (key != null)
				{
					object keyValue = key.GetValue("APPDATA");
					if (keyValue == null)
						return false;

					path = keyValue.ToString();
					return true;
				}
			}

			return false;
		}

		internal bool GetLatestLog(out string filename)
		{
			filename = "";

			string logPath = "";

			// Usually server/HKLM
			string installPath = "";
			bool installPathFound = GetInstallationPath(out installPath);

			// Usually client/HKCU
			string appdataPath = "";
			bool appdataPathFound = GetAppdataPath(out appdataPath);

			if(installPathFound && Directory.Exists(installPath + "\\LOG"))
				logPath = installPath + "\\LOG";

			if (appdataPathFound && Directory.Exists(appdataPath + "\\LOG"))
				logPath = appdataPath + "\\LOG";

			DateTime lastdt = DateTime.MinValue;
			string[] files = Directory.GetFiles(logPath, "update_*.log", SearchOption.TopDirectoryOnly);
			if(files.Length <= 0)
			{
				// Probably never updated?
				return false;
			}

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
			int newDSTVersionCount = 0;
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

				newDSTVersionCount++;
			}

			// FIXME: This doesn't work! GetDSTVersion is not called if cachedVersion is present
			if (newDSTVersionCount != this.dstVersionCount) // Invalidate cache then
			{
				InvalidateCache();
				this.dstVersionCount = newDSTVersionCount;
			}

			// Always take the latest value
			Regex regex = new Regex(@"\d{2}.\d{2}.\d{3}");
			if (!regex.IsMatch(value))
				return false;

			version = value;

			return true;
		}
	}
}
