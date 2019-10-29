using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;

namespace MedocUpdates
{
	class LocalizePair
	{
		public string token;
		public string value;

		public LocalizePair(string token, string value)
		{
			this.token = token;
			this.value = value;
		}
	}

	class LocLanguage
	{
		public List<LocalizePair> str;
		public string lang;

		public LocLanguage(List<LocalizePair> str, string lang)
		{
			this.str = str;
			this.lang = lang;
		}
	}

	class LocFile
	{
		//[JsonProperty("language")]
		public string language;

		//[JsonProperty("tokens")]
		public List<LocalizePair> tokens;
	}

	public static class Loc
	{
		private static List<LocLanguage> langstrs = new List<LocLanguage>();
		private static string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		private static string m_languagePath = Path.Combine(m_exePath, "lang");
		private static string lang = SessionStorage.inside.SelectedLanguage; // "en" by default

		public static bool Init(string forcelang = "")
		{
			//LocFile locfileWrite = new LocFile();
			//locfileWrite.language = "English";
			//locfileWrite.tokens = strs;
			//string jsonWrite = JsonConvert.SerializeObject(locfileWrite, Formatting.Indented);
			//File.WriteAllText("en.json", jsonWrite);

			List<LocalizePair> strs;
			string json;
			LocFile locfile;


			// If the language was forcebly set in the parameters, then check it first
			if(forcelang.Trim().Length > 0 && File.Exists(Path.Combine(m_languagePath, forcelang + ".json")))
			{
				string langfile = Path.Combine(m_languagePath, forcelang + ".json");
				string langname = forcelang;

				strs = new List<LocalizePair>();
				json = File.ReadAllText(langfile);
				locfile = JsonConvert.DeserializeObject<LocFile>(json);
				strs.AddRange(locfile.tokens);
				langstrs.Add(new LocLanguage(strs, langname));

				Loc.lang = forcelang;
				return true;
			}


			string[] langfiles;
			GetLanguageFiles(out langfiles); // TODO: Check

			try
			{
				foreach (string langfile in langfiles)
				{
					string langname = Path.GetFileNameWithoutExtension(langfile);

					strs = new List<LocalizePair>();
					json = File.ReadAllText(langfile);
					locfile = JsonConvert.DeserializeObject<LocFile>(json);
					strs.AddRange(locfile.tokens);
					langstrs.Add(new LocLanguage(strs, langname));
				}
			}
			catch (Exception ex)
			{
				Log.Write(LogLevel.NORMAL, "Localization: Cannot process the localization\r\n" + ex.Message);
				return false;
			}


			//Loc.lang = forcelang;
			return true;
		}

		public static bool GetLanguageFiles(out string[] files)
		{
			files = new string[0];
			if (!Directory.Exists(m_languagePath))
				return false;

			files = Directory.GetFiles(m_languagePath, "*.json");
			if (files.Length <= 0)
				return false;

			return true;
		}

		public static bool GetLocalizations(out string[] names, out string[] files)
		{
			names = new string[0];
			GetLanguageFiles(out files); // TODO: Check

			List<string> namesHelper = new List<string>();

			string json;
			LocFile locfile;

			try
			{
				foreach (string langfile in files)
				{
					string langname = Path.GetFileNameWithoutExtension(langfile);

					json = File.ReadAllText(langfile);
					locfile = JsonConvert.DeserializeObject<LocFile>(json);
					namesHelper.Add(locfile.language);
				}
			}
			catch (Exception ex)
			{
				Log.Write(LogLevel.NORMAL, "Localization: Cannot process the localization\r\n" + ex.Message);
				return false;
			}

			names = namesHelper.ToArray();
			return true;
		}

		public static string Get(string token)
		{
			LocLanguage loclang;
			LocalizePair locpair;


			// Checking selected lang first
			if (Loc.lang.Trim().Length > 0)
			{
				loclang = langstrs.FirstOrDefault(elem => elem.lang.Equals(Loc.lang));
				if(loclang == null)
					return token;

				locpair = loclang.str.FirstOrDefault(elem => elem.token.Equals(token));
				if(locpair != null)
					return locpair.value;
			}


			// Check default lang first (en)
			loclang = langstrs.FirstOrDefault(elem => elem.lang.Equals("en"));
			if (loclang == null)
				return token;

			locpair = loclang.str.FirstOrDefault(elem => elem.token.Equals(token));
			if (locpair != null)
				return locpair.value;


			return token;
		}
	}
}
