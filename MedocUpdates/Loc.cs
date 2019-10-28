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
		private static string lang = "en"; // SessionStorage
		private static List<LocLanguage> langstrs = new List<LocLanguage>();

		public static bool Init(string lang)
		{
			//LocFile locfileWrite = new LocFile();
			//locfileWrite.language = "English";
			//locfileWrite.tokens = strs;
			//string jsonWrite = JsonConvert.SerializeObject(locfileWrite, Formatting.Indented);
			//File.WriteAllText("en.json", jsonWrite);

			List<LocalizePair> strs;
			string json;
			LocFile locfile;

			string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string languagePath = Path.Combine(exePath, "lang");

			if (!Directory.Exists(languagePath))
				return false;

			string[] langfiles = Directory.GetFiles(languagePath, "*.json");
			if(langfiles.Length <= 0)
				return false;

			try
			{
				foreach(string langfile in langfiles)
				{
					string langname = Path.GetFileNameWithoutExtension(langfile);

					strs = new List<LocalizePair>();
					json = File.ReadAllText(langfile);
					locfile = JsonConvert.DeserializeObject<LocFile>(json);
					strs.AddRange(locfile.tokens);
					langstrs.Add(new LocLanguage(strs, langname));
				}
			}
			catch(Exception ex)
			{
				Log.Write(LogLevel.NORMAL, "");
				return false;
			}

			Loc.lang = lang;
			return true;
		}

		public static string Get(string token)
		{
			LocLanguage loclang;

			loclang = langstrs.FirstOrDefault(elem => elem.lang.Equals(Loc.lang)); // Checking selected lang first
			LocalizePair locpair = loclang.str.FirstOrDefault(elem => elem.token.Equals(token));
			if(locpair != null)
				return locpair.value;

			loclang = langstrs.FirstOrDefault(elem => elem.lang.Equals("en")); // Check default lang first (en)
			locpair = loclang.str.FirstOrDefault(elem => elem.token.Equals(token));
			if (locpair != null)
				return locpair.value;

			return token;
		}
	}
}
