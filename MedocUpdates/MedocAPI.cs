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

		internal HtmlNodeCollection DownloadsItems()
		{
			if (doc == null)
				return null;

			if (doc.ParseErrors != null && doc.ParseErrors.Count() > 0)
				return null;

			if (doc.DocumentNode == null)
				return null;

			HtmlNode docNode = doc.DocumentNode;
			if (!docNode.HasChildNodes)
				return null;

			//	string selector = "//div[@class='download-dist-specification-item-box']/div[@class='col-sm-*']";
			//	string selector = "//div[contains(@class,'col-sm-')]";
			string selector = "//div[@class='download-items']";
			HtmlNode downloads = docNode.SelectSingleNode(selector);
			if (downloads == null)
				return null;

			if (!downloads.HasChildNodes)
				return null;

			HtmlNodeCollection downloadsItems = downloads.SelectNodes(".//div[@class='download-dist-specification-item']");
			return downloadsItems;
		}

		internal string GetVersion(HtmlNode node)
		{
			HtmlNode download = node.SelectSingleNode(".//span[@class='js-update-num']");
			if (download == null)
				return "";

			if (!download.HasChildNodes)
				return "";

			return download.InnerText;
		}

		internal bool GetItems(out MedocDownloadItem[] items)
		{
			items = null;

			HtmlNodeCollection downloadsItems = DownloadsItems();
			if (downloadsItems == null)
				return false;

			items = new MedocDownloadItem[downloadsItems.Count];

			int i = 0;
			foreach(HtmlNode node in downloadsItems)
			{
				MedocDownloadItem item = new MedocDownloadItem();
				item.version = GetVersion(node);
				item.link = GetDownload(node);

				items[i] = item;
				i++;
			}

			return (i == downloadsItems.Count);
		}

		internal string GetLatestVersion()
		{
			HtmlNodeCollection downloadsItems = DownloadsItems();
			if (downloadsItems == null)
				return "";

			string version = GetVersion( downloadsItems[0] );

			Console.WriteLine("{0}", version);

			return version;
		}


		internal string GetDownload(HtmlNode node)
		{
			HtmlNode download = node.SelectSingleNode(".//div[@class='download-dist-specification-item-box-btn']");
			if (download == null)
				return "";

			if (!download.HasChildNodes)
				return "";

			HtmlNode link = download.SelectSingleNode(".//a[@class='main-btn']");
			if (link == null)
				return "";

			return link.GetAttributeValue("href", "<no link>");
		}

		internal string GetLatestDownload()
		{
			HtmlNodeCollection downloadsItems = DownloadsItems();
			if (downloadsItems == null)
				return "";

			string downloadStr = GetDownload(downloadsItems[0]);

			Console.WriteLine("{0}", downloadStr);

			return downloadStr;
		}
	}
}
