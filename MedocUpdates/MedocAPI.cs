using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;
using System.Windows.Forms;

namespace MedocUpdates
{
	class MedocAPI
	{
		HtmlWeb web;
		HtmlAgilityPack.HtmlDocument doc;

		internal MedocAPI()
		{
			web = new HtmlWeb();
			web.UsingCache = false;

		//	RefreshDoc();
		}

		internal bool RefreshDoc()
		{
			try
			{
				doc = web.Load("https://www.medoc.ua/uk/download");
			}
			catch(Exception ex)
			{
				MessageBox.Show("Cannot retrieve downloads from the website!\n"+ex.Message, "Critical error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return (doc != null);
		}

		internal HtmlNodeCollection DownloadItems()
		{
			return null;
		}
		internal string GetLatestVersion()
		{
			if (doc == null)
				return "";

			if (doc.ParseErrors != null && doc.ParseErrors.Count() > 0)
				return "";

			if (doc.DocumentNode == null)
				return "";

			HtmlNode docNode = doc.DocumentNode;
			if (!docNode.HasChildNodes)
				return "";

			//	string selector = "//div[@class='download-dist-specification-item-box']/div[@class='col-sm-*']";
			//	string selector = "//div[contains(@class,'col-sm-')]";
			string selector = "//div[@class='download-items']";
			HtmlNode downloads = docNode.SelectSingleNode(selector);
			if (downloads == null)
				return "";

			if (!downloads.HasChildNodes)
				return "";

			HtmlNodeCollection downloadsItems = downloads.SelectNodes(".//div[@class='download-dist-specification-item']");
			if (downloadsItems == null)
				return "";

			HtmlNode latestDownload = downloadsItems[0].SelectSingleNode(".//span[@class='js-update-num']");
			if (latestDownload == null)
				return "";

			if (!latestDownload.HasChildNodes)
				return "";

			Console.WriteLine("{0}", latestDownload.InnerText);

			return latestDownload.InnerText;
		}
	}
}
