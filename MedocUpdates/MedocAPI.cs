using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;
//using System.Windows.Forms;

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

			Log.Write("MedocAPI: Initializing");
		}

		internal bool RefreshDoc()
		{
			doc = null;
			try
			{
				Log.Write(LogLevel.NORMAL, "MedocAPI: Trying to load a medoc.ua download webpage");
				doc = web.Load("https://www.medoc.ua/uk/download");
			}
			catch(Exception ex)
			{
				string errormsg = "Cannot retrieve downloads from the website!\n" + ex.Message;
				//	MessageBox.Show(errormsg, "Critical error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Log.Write("MedocAPI: " + errormsg);
			}
			return (doc != null);
		}

		internal HtmlNodeCollection DownloadsItems()
		{
			if (doc == null)
			{
				Log.Write(LogLevel.NORMAL, "MedocAPI: Web document wasn't initialized");
				return null;
			}

			if (doc.ParseErrors != null && doc.ParseErrors.Count() > 0)
			{
				Log.Write(LogLevel.EXPERT, "MedocAPI: Parsing errors occured");
				foreach(HtmlParseError error in doc.ParseErrors)
				{
					Log.Write(LogLevel.EXPERT, error.SourceText);
				}
				return null;
			}

			if (doc.DocumentNode == null)
			{
				Log.Write(LogLevel.EXPERT, "MedocAPI: Cannot find main document node");
				return null;
			}

			HtmlNode docNode = doc.DocumentNode;
			if (!docNode.HasChildNodes)
			{
				Log.Write(LogLevel.EXPERT, "MedocAPI: Document contains only it's body");
				return null;
			}

			//	string selector = "//div[@class='download-dist-specification-item-box']/div[@class='col-sm-*']";
			//	string selector = "//div[contains(@class,'col-sm-')]";
			string selector = "//div[@class='download-items']";
			HtmlNode downloads = docNode.SelectSingleNode(selector);
			if (downloads == null)
			{
				Log.Write(LogLevel.EXPERT, "MedocAPI: Cannot find download-items node. Is it the download page?");
				return null;
			}

			if (!downloads.HasChildNodes)
			{
				Log.Write(LogLevel.EXPERT, "MedocAPI: download-items node doesn't contain child nodes");
				return null;
			}

			HtmlNodeCollection downloadsItems = downloads.SelectNodes(".//div[@class='download-dist-specification-item']");
			return downloadsItems;
		}

		internal MedocVersion GetVersion(HtmlNode node)
		{
			HtmlNode download = node.SelectSingleNode(".//span[@class='js-update-num']");
			if (download == null)
			{
				Log.Write(LogLevel.EXPERT, "MedocAPI: Download info node doesn't exist");
				return "";
			}

			if (!download.HasChildNodes)
			{
				Log.Write(LogLevel.EXPERT, "MedocAPI: Download info node doesn't contain a update version");
				return "";
			}

			return (MedocVersion)download.InnerText; // Added explicit conversion for convenience
		}

		internal bool GetItems(out MedocDownloadItem[] items)
		{
			items = null;

			HtmlNodeCollection downloadsItems = DownloadsItems();
			if (downloadsItems == null)
			{
				Log.Write(LogLevel.EXPERT, "MedocAPI: Cannot get items from the download-items");
				return false;
			}

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

		internal MedocVersion GetLatestVersion()
		{
			HtmlNodeCollection downloadsItems = DownloadsItems();
			if (downloadsItems == null)
			{
				Log.Write(LogLevel.EXPERT, "MedocAPI: Cannot get items from the download-items");
				return "";
			}

			MedocVersion version = GetVersion( downloadsItems[0] );

			Console.WriteLine("{0}", version);

			return version;
		}


		internal string GetDownload(HtmlNode node)
		{
			HtmlNode download = node.SelectSingleNode(".//div[@class='download-dist-specification-item-box-btn']");
			if (download == null)
			{
				Log.Write(LogLevel.EXPERT, "MedocAPI: Cannot get a download link for " + node.GetAttributeValue("class", "<unknown class>"));
				return "";
			}

			if (!download.HasChildNodes)
			{
				Log.Write(LogLevel.EXPERT, "MedocAPI: Download link node doesn't contain any external links");
				return "";
			}

			HtmlNode link = download.SelectSingleNode(".//a[@class='main-btn']");
			if (link == null)
			{
				Log.Write(LogLevel.EXPERT, "MedocAPI: Download node doesn't contain a download button");
				return "";
			}

			return link.GetAttributeValue("href", "<no link>");
		}

		internal string GetLatestDownload()
		{
			HtmlNodeCollection downloadsItems = DownloadsItems();
			if (downloadsItems == null)
			{
				Log.Write(LogLevel.EXPERT, "MedocAPI: Cannot get the first download node");
				return "";
			}

			string downloadStr = GetDownload(downloadsItems[0]);

			Console.WriteLine("{0}", downloadStr);

			return downloadStr;
		}
	}
}
