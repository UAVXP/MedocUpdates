using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

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

		public static void Init(string lang)
		{
			// TODO: Implement localization files and parsing of them
			/*
			List<LocalizePair> strs = new List<LocalizePair>();
		//	strs.Add(new LocalizePair("", ""));
			//strs.Add(new LocalizePair("frmMain_Done", "Done!"));
			//strs.Add(new LocalizePair("frmMain.trayIcon.Text", "Medoc Updates"));
			//strs.Add(new LocalizePair("frmMain.checkForUpdatesToolStripMenuItem.Text", "Check for updates"));
			//strs.Add(new LocalizePair("frmMain.delayNotificationsToolStripMenuItem.Text", "Delay notifications..."));
			//strs.Add(new LocalizePair("frmMain.exitToolStripMenuItem.Text", "Exit"));
			//strs.Add(new LocalizePair("frmMain.fileToolStripMenuItem.Text", "File"));
			//strs.Add(new LocalizePair("frmMain.labelVersion.Text", "VERSION HERE"));
			//strs.Add(new LocalizePair("frmMain.menuStrip1.Text", "menuStrip1"));
			//strs.Add(new LocalizePair("frmMain.editToolStripMenuItem.Text", "Edit"));
			//strs.Add(new LocalizePair("frmMain.settingsToolStripMenuItem.Text", "Settings"));
			//strs.Add(new LocalizePair("frmMain.labelLocalVersion.Text", "LOCAL VERSION HERE"));
			//strs.Add(new LocalizePair("frmMain.statusStrip1.Text", "statusStrip1"));
			//strs.Add(new LocalizePair("frmMain.toolStripStatusLabel1.Text", "toolStripStatusLabel1"));
			//strs.Add(new LocalizePair("frmMain.Text", "Medoc Updates"));
			langstrs.Add(new LocLanguage(strs, "en"));

			strs = new List<LocalizePair>();
			//strs.Add(new LocalizePair("frmMain_Done", "Готово!"));
			//strs.Add(new LocalizePair("frmMain.trayIcon.Text", "Обновления M.E.Doc"));
			//strs.Add(new LocalizePair("frmMain.checkForUpdatesToolStripMenuItem.Text", "Проверить обновления"));
			//strs.Add(new LocalizePair("frmMain.delayNotificationsToolStripMenuItem.Text", "Отложить уведомления..."));
			//strs.Add(new LocalizePair("frmMain.exitToolStripMenuItem.Text", "Выход"));
			//strs.Add(new LocalizePair("frmMain.fileToolStripMenuItem.Text", "Файл"));
			//strs.Add(new LocalizePair("frmMain.labelVersion.Text", "ВЕРСИЯ ЗДЕСЬ"));
			//strs.Add(new LocalizePair("frmMain.menuStrip1.Text", "menuStrip1"));
			//strs.Add(new LocalizePair("frmMain.editToolStripMenuItem.Text", "Правка"));
			//strs.Add(new LocalizePair("frmMain.settingsToolStripMenuItem.Text", "Настройки"));
			//strs.Add(new LocalizePair("frmMain.labelLocalVersion.Text", "ЛОКАЛЬНАЯ ВЕРСИЯ ЗДЕСЬ"));
			//strs.Add(new LocalizePair("frmMain.statusStrip1.Text", "statusStrip1"));
			//strs.Add(new LocalizePair("frmMain.toolStripStatusLabel1.Text", "toolStripStatusLabel1"));
			//strs.Add(new LocalizePair("frmMain.Text", "Обновления M.E.Doc"));

			string json = File.ReadAllText("ru.json");
			LocFile locfile = JsonConvert.DeserializeObject<LocFile>(json);
			strs.AddRange(locfile.tokens);

			//LocFile locfileWrite = new LocFile();
			//locfileWrite.language = "English";
			//locfileWrite.tokens = strs;
			//string jsonWrite = JsonConvert.SerializeObject(locfileWrite, Formatting.Indented);
			//File.WriteAllText("en.json", jsonWrite);

			langstrs.Add(new LocLanguage(strs, "ru"));
			*/

			List<LocalizePair> strs;
			string json;
			LocFile locfile;

			string[] langfiles = Directory.GetFiles("lang", "*.json");
			foreach(string langfile in langfiles)
			{
				string langname = Path.GetFileNameWithoutExtension(langfile);
				strs = new List<LocalizePair>();
				json = File.ReadAllText(langfile);
				locfile = JsonConvert.DeserializeObject<LocFile>(json);
				strs.AddRange(locfile.tokens);
				langstrs.Add(new LocLanguage(strs, langname));
			}

			Loc.lang = lang;
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
