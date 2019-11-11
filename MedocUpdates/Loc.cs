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
		public string language = "";

		//[JsonProperty("tokens")]
		public List<LocalizePair> tokens = new List<LocalizePair>();
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
			if(forcelang.Trim().Length > 0)
			{
				Log.Write("Loc: Forcing application language set to " + forcelang);
				if(File.Exists(Path.Combine(m_languagePath, forcelang + ".json")))
				{
					string langfile = Path.Combine(m_languagePath, forcelang + ".json");
					string langname = forcelang;
					strs = new List<LocalizePair>();

					if (!File.Exists(langfile)) // File exist in file list, but not now?
					{
						Log.Write(LogLevel.NORMAL, "Loc: " + langfile + " doesn't exist");
						return false;
					}

					json = File.ReadAllText(langfile);
					if (json.Trim().Length <= 0) // Cannot read file
					{
						Log.Write(LogLevel.NORMAL, "Loc: Cannot read " + langfile);
						return false;
					}

					locfile = JsonConvert.DeserializeObject<LocFile>(json);
					if (locfile == null)
					{
						Log.Write(LogLevel.NORMAL, "Loc: Cannot load " + langfile);
						return false;
					}

					if (locfile.tokens == null || locfile.tokens.Count <= 0) // No tokens in the file
					{
						Log.Write(LogLevel.NORMAL, "Loc: " + langfile + " doesn't seem to be a proper language file");
						return false;
					}

					strs.AddRange(locfile.tokens);

					// Clearing up the "comments" (objects with empty tokens)
					strs = strs.Where(elem => !String.IsNullOrWhiteSpace(elem.token)).Distinct().ToList(); // FIXME: Distinct is very uneffective
					if (strs.Count <= 0) // Something went wrong with searching for empty tokens, or the whole file is filled with those ones
					{
						Log.Write(LogLevel.NORMAL, "Loc: " + langfile + " contains only empty tokens");
						return false;
					}

					langstrs.Add(new LocLanguage(strs, langname));

					Loc.m_lang = forcelang;
					return true;
				}
				else
				{
					Log.Write(LogLevel.NORMAL,
						String.Format("Loc: Cannot use -forcelanguage because language {0} doesn't exist! Proceeding to the saved language.", forcelang));
				}
			}


			string[] langfiles;
			if(!GetLanguageFiles(out langfiles))
			{
				Log.Write(LogLevel.NORMAL, "Loc: Cannot get any language files from \"lang\" folder");
				return false;
			}

			try
			{
				foreach (string langfile in langfiles)
				{
					string langname = Path.GetFileNameWithoutExtension(langfile);
					strs = new List<LocalizePair>();

					if(!File.Exists(langfile)) // File exist in file list, but not now?
					{
						Log.Write(LogLevel.NORMAL, "Loc: " + langfile + " doesn't exist");
						continue;
					}

					json = File.ReadAllText(langfile);
					if(json.Trim().Length <= 0) // Cannot read file
					{
						Log.Write(LogLevel.NORMAL, "Loc: Cannot read " + langfile);
						continue;
					}

					locfile = JsonConvert.DeserializeObject<LocFile>(json);
					if(locfile == null)
					{
						Log.Write(LogLevel.NORMAL, "Loc: Cannot load " + langfile);
						continue;
					}

					if(locfile.tokens == null || locfile.tokens.Count <= 0) // No tokens in the file
					{
						Log.Write(LogLevel.NORMAL, "Loc: " + langfile + " doesn't seem to be a proper language file");
						continue;
					}

					strs.AddRange(locfile.tokens);

					// Clearing up the "comments" (objects with empty tokens)
					strs = strs.Where(elem => !String.IsNullOrWhiteSpace(elem.token)).Distinct().ToList(); // FIXME: Distinct is very uneffective
					if(strs.Count <= 0) // Something went wrong with searching for empty tokens, or the whole file is filled with those ones
					{
						Log.Write(LogLevel.NORMAL, "Loc: " + langfile + " contains only empty tokens");
						continue;
					}

					langstrs.Add(new LocLanguage(strs, langname));
				}
			}
			catch (Exception ex)
			{
				Log.Write(LogLevel.NORMAL, "Loc: Cannot process the localization\r\n" + ex.Message);
				return false;
			}


			//Loc.lang = forcelang; // Already defined at the beginning of the class
			return true;
		}

		public static bool GetLanguageFiles(out string[] files)
		{
			files = new string[0];
			if (!Directory.Exists(m_languagePath))
			{
				Log.Write(LogLevel.NORMAL, "Loc: \"lang\" folder doesn't exist");
				return false;
			}

			files = Directory.GetFiles(m_languagePath, "*.json");
			if (files.Length <= 0)
			{
				Log.Write(LogLevel.NORMAL, "Loc: \"lang\" folder doesn't contain any language files in it");
				return false;
			}

			return true;
		}

		public static bool GetLocalizations(out string[] names, out string[] files)
		{
			names = new string[0];
			if(!GetLanguageFiles(out files))
			{
				Log.Write(LogLevel.NORMAL, "Loc: Cannot get any language files from \"lang\" folder");
				return false;
			}

			List<string> namesHelper = new List<string>();

			string json;
			LocFile locfile;

			try
			{
				foreach (string langfile in files)
				{
					string langname = Path.GetFileNameWithoutExtension(langfile);

					if (!File.Exists(langfile)) // File exist in file list, but not now?
					{
						Log.Write(LogLevel.NORMAL, "Loc: " + langfile + " doesn't exist");
						continue;
					}

					json = File.ReadAllText(langfile);
					if (json.Trim().Length <= 0) // Cannot read file
					{
						Log.Write(LogLevel.NORMAL, "Loc: Cannot read " + langfile);
						continue;
					}

					locfile = JsonConvert.DeserializeObject<LocFile>(json);
					if(locfile == null)
					{
						Log.Write(LogLevel.NORMAL, "Loc: Cannot load " + langfile);
						continue;
					}

					if(locfile.language == null || locfile.language.Trim().Length <= 0)
					{
						//namesHelper.Add(Loc.m_defaultLang);
						Log.Write(LogLevel.NORMAL, "Loc: " + langfile + " is corrupted or is not a valid language file");
						continue;
					}
					else
					{
						namesHelper.Add(locfile.language);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Write(LogLevel.NORMAL, "Loc: Cannot process the localization\r\n" + ex.Message);
				return false;
			}

			if(namesHelper.Count <= 0)
			{
				Log.Write(LogLevel.NORMAL, "Loc: None of the language files were added");
				return false;
			}

			names = namesHelper.ToArray();
			if(names.Length <= 0) // How's this would happen?
			{
				Log.Write(LogLevel.NORMAL, "Loc: Something went wrong with getting a localizations (cannot convert list to array)");
				return false;
			}

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
