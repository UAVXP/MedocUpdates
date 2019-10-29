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
		public static string m_defaultLang = "en";
		private static string m_lang = SessionStorage.inside.SelectedLanguage;

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

				// Clearing up the "comments" (objects with empty tokens)
				strs = strs.Where(elem => !String.IsNullOrWhiteSpace(elem.token)).Distinct().ToList(); // FIXME: Distinct is very uneffective

				langstrs.Add(new LocLanguage(strs, langname));

				Loc.m_lang = forcelang;
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

					// Clearing up the "comments" (objects with empty tokens)
					strs = strs.Where(elem => !String.IsNullOrWhiteSpace(elem.token)).Distinct().ToList(); // FIXME: Distinct is very uneffective

					langstrs.Add(new LocLanguage(strs, langname));
				}
			}
			catch (Exception ex)
			{
				Log.Write(LogLevel.NORMAL, "Localization: Cannot process the localization\r\n" + ex.Message);
				return false;
			}


			//Loc.lang = forcelang; // Already defined at the beginning of the class
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

		private static bool IsDefaultLang(string lang)
		{
			return lang.Equals(m_defaultLang);
		}

		private static string GetDefaultLangToken(string token, string defaultValue)
		{
			LocLanguage loclang;
			LocalizePair locpair;

			loclang = langstrs.FirstOrDefault(elem => IsDefaultLang(elem.lang));
			if (loclang == null)
				return (!defaultValue.Trim().Equals("") ? defaultValue : token); // Give up and just return the token or defaultValue - we don't have any localizations for it

			locpair = loclang.str.FirstOrDefault(elem => elem.token.Equals(token));
			if (locpair != null)
				return locpair.value;

			return (!defaultValue.Trim().Equals("") ? defaultValue : token); // Give up and just return the token or defaultValue - we don't have any localizations for it
		}

		private static string GetLangToken(string token, string defaultValue, string lang)
		{
			LocLanguage loclang;
			LocalizePair locpair;

			loclang = langstrs.FirstOrDefault(elem => elem.lang.Equals(lang));
			if (loclang == null)
				return GetDefaultLangToken(token, defaultValue);

			locpair = loclang.str.FirstOrDefault(elem => elem.token.Equals(token));
			if (locpair != null)
				return locpair.value;

			return GetDefaultLangToken(token, defaultValue);
		}

		public static string Get(string token, string defaultValue = "", string forceLang = "")
		{
			// Checking forced lang first
			if (forceLang.Trim().Length > 0)
			{
				return GetLangToken(token, defaultValue, forceLang);
			}

			// Checking selected lang
			if (Loc.m_lang.Trim().Length > 0 && !IsDefaultLang(Loc.m_lang)) // We don't need to search for token for this lang, if it's already a default lang. Just pass to the end
			{
				return GetLangToken(token, defaultValue, Loc.m_lang);
			}

			// Checking default lang (en)
			return GetDefaultLangToken(token, defaultValue);
		}
	}
}
