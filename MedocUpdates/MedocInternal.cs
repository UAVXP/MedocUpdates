using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using BlackBeltCoder;

//using DMF.Config;
//using ZvitClientInterface;
//using ZvitClientInterface.Client;

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

				// M.E.Doc direct integration test
				//DMFEnvironment env = new DMFEnvironment();
				//ObjectProvider obj = new ObjectProvider(env);
				//DMFEnvironment.CurrEnvironment.Provider = new ObjectProvider(env);
				//ObjectProvider obj = new ObjectProvider(DMFEnvironment.CurrEnvironment);
				//string test = Globals.PrgVersion;
				//return test;

				// TODO: Do a fallback way - through Software\\M.E.Doc\\M.E.Doc subkey name
				// TODO: Make a proper cache for this? Maybe by time or something else
				MedocVersion tempVersion = new MedocVersion();

				//GetDSTVersion(out tempVersion); // Old version detection
				GetLastUpdateVersion(out tempVersion); // New, more reliable version detection

				Log.Write(LogLevel.NORMAL, "MedocInternal: Retrieving local version");
				return tempVersion;
			}
		}

		public static bool GetInstallationPath(out string path)
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
					{
						Log.Write(LogLevel.EXPERT, "MedocInternal: Cannot find PATH in HKLM");
						return false;
					}

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
					{
						Log.Write(LogLevel.EXPERT, "MedocInternal: Cannot find PATH in HKCU");
						return false;
					}

					path = keyValue.ToString();
					return true;
				}
			}

			Log.Write(LogLevel.NORMAL, "MedocInternal: Cannot get the PATH path from registry");
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
					{
						Log.Write(LogLevel.EXPERT, "MedocInternal: Cannot find APPDATA in HKLM");
						return false;
					}

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
					{
						Log.Write(LogLevel.EXPERT, "MedocInternal: Cannot find APPDATA in HKCU");
						return false;
					}

					path = keyValue.ToString();
					return true;
				}
			}

			Log.Write(LogLevel.NORMAL, "MedocInternal: Cannot get the APPDATA path from registry");
			return false;
		}

		internal bool GetLatestLogs(out string[] allLogFiles)
		{
			allLogFiles = new string[0];

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

			if(logPath.Trim().Length <= 0)
			{
				Log.Write("MedocInternal: Cannot find M.E.Doc install path");
				return false;
			}

			DateTime lastdt = DateTime.MinValue;
			string[] files = Directory.GetFiles(logPath, "update_*.log", SearchOption.TopDirectoryOnly);
			if(files.Length <= 0)
			{
				// Probably never updated?
				Log.Write("MedocInternal: M.E.Doc should have been updated at least once (can't find update_*.log)");
				return false;
			}

			allLogFiles = files;

		/* NOTE: Not needed anymore
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
		*/

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

		internal string ReadLogFile(string filename)
		{
			string[] file = new string[] { };
			try
			{
				file = File.ReadAllLines(filename);
			}
			catch (Exception ex)
			{
				Log.Write(LogLevel.NORMAL, "Cannot read a latest log file\r\n" + ex.Message);
			}

			string value = "";
			for (int i = 0; i < file.Length; i++)
			{
				string line = file[i];

				bool foundDSTVersionString = GetLine(line, "DSTVERSION");
				if (!foundDSTVersionString)
					continue;

				// next line should be the version now
				if ((i + 1) >= file.Length)
				{
					Log.Write(LogLevel.EXPERT, "MedocInternal: " + filename + ": unexpected EOF");
					continue;
				}

				line = file[i + 1];

				bool foundDSTVersion = GetValue(line, ": ", out value);
				if (!foundDSTVersion)
					continue;
			}

			return value;
		}

		// TODO: This should be replaced with a new detection method!
		// (evidences line 2)
		internal bool GetDSTVersion(out MedocVersion version)
		{
			version = new MedocVersion();

			string[] allLogFiles = new string[0];
			if (!GetLatestLogs(out allLogFiles))
				return false;

			string value = "";
			for (int i = allLogFiles.Length - 1; i >= 0; i--)
			{
				string logFile = allLogFiles[i];
				if(logFile.Trim().Length <= 0)
				{
					Log.Write(LogLevel.EXPERT, "MedocInternal: Unexpected log list searching error");
					continue;
				}

				value = ReadLogFile(logFile);

				// Check if the latest log contains DSTVERSION at all
				// If not - then check all the previous
				if (value.Trim().Length > 0)
					break;

				Log.Write(LogLevel.NORMAL, "MedocInternal: Cannot find DSTVERSION key in " + logFile);
			}

			// Always take the latest value
			Regex regex = new Regex(@"\d{2}.\d{2}.\d{3}");
			if (!regex.IsMatch(value))
			{
				Log.Write("MedocInternal: Update version doesn't match the expected pattern (xx.xx.xxx)");
				return false;
			}

			version = (MedocVersion)value; // Added explicit conversion for convenience

			return true;
		}

		// http://www.blackbeltcoder.com/Articles/strings/a-sscanf-replacement-for-net
		internal bool GetLastUpdateVersion(out MedocVersion version)
		{
			version = new MedocVersion();

			ScanFormatted parser = new ScanFormatted();

			string[] allLogFiles = new string[0];
			if (!GetLatestLogs(out allLogFiles))
				return false;

			for (int i = allLogFiles.Length - 1; i >= 0; i--)
			{
				string logFile = allLogFiles[i];
				if (logFile.Trim().Length <= 0)
				{
					Log.Write(LogLevel.EXPERT, "MedocInternal: Unexpected log list searching error");
					continue;
				}

				// 
				string[] file = new string[] { };
				try
				{
					file = File.ReadAllLines(logFile);
				}
				catch (Exception ex)
				{
					Log.Write(LogLevel.NORMAL, "Cannot read a latest log file\r\n" + ex.Message);
				}

				// Search from the end to start of the file
				for (int j = file.Length - 1; j >= 0; j--)
				{
					string line = file[j];

					//
					bool foundNewVerString = GetLine(line, "newVer");
					if (!foundNewVerString)
						continue;

					int newVerIdx = line.IndexOf("newVer");
					if(newVerIdx + "newVer".Length >= line.Length)
						continue;

					line = line.Substring(newVerIdx);

					// ScanFormatted fricks stuff up after parsing %s for some reason (doesn't care it's Ukrainian word of English (i.e. "is1C = False"))
					//string lineformat = "%d.%d.%d %d:%d:%d.%d %s INFO    %s: is1C = %s, newVer = %d, newRel = %d, newBld = %d";

					string lineformat = "newVer = %d, newRel = %d, newBld = %d";
					int count = parser.Parse(line, lineformat);
					//if(count > 9 /*&& count <= 13*/)
					if(count == 3)
					{
						version = new MedocVersion((int)parser.Results[0], (int)parser.Results[1], (int)parser.Results[2]);
						Console.WriteLine("Found version: {0}", version.Version);
						return true;
					}
				}
			}

			return false;
		}
	}
}
