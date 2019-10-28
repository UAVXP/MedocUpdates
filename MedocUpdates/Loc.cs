using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

	public static class Loc
	{
		private static string lang = "en"; // SessionStorage
		private static List<LocLanguage> langstrs = new List<LocLanguage>();

		public static void Init(string lang)
		{
			List<LocalizePair> enstrs = new List<LocalizePair>();
			enstrs.Add(new LocalizePair("frmMain_Done", "Done!"));
			enstrs.Add(new LocalizePair("frmMain_Edit", "Edit"));
			langstrs.Add(new LocLanguage(enstrs, "en"));

			List<LocalizePair> rustrs = new List<LocalizePair>();
			rustrs.Add(new LocalizePair("frmMain_Done", "Готово!"));
			rustrs.Add(new LocalizePair("frmMain_Edit", "Правка"));
			langstrs.Add(new LocLanguage(rustrs, "ru"));

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
