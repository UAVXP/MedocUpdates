using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Reflection;

namespace MedocUpdates
{
	public static class TelegramChatsStorage
	{
		private static List<long> m_TelegramChatIDs = new List<long>();
		private static string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		private static string m_storageFilePath = Path.Combine(m_exePath, "mu_chatids.txt");

		public static List<long> TelegramChats
		{
			get
			{
				return m_TelegramChatIDs;
			}
		}

		public static bool Save()
		{
			if (!File.Exists(m_storageFilePath))
				File.CreateText(m_storageFilePath);

			// Checking again
			if (!File.Exists(m_storageFilePath))
			{
				Log.Write(LogLevel.NORMAL, "TelegramChatsStorage: Save(): Cannot create chatID storage file");
				return false;
			}

			string chatIDsFileStr = "";
			foreach(long chatID in m_TelegramChatIDs)
			{
				chatIDsFileStr += chatID + "\r\n";
			}

			File.WriteAllText(m_storageFilePath, chatIDsFileStr);
			if (!File.Exists(m_storageFilePath)) // TODO: Check for file size instead of availability
			{
				Log.Write(LogLevel.NORMAL, "TelegramChatsStorage: Save(): Storage file is gone in the middle of writing");
				return false; // And here we're going to fail
			}

			return true;
		}

		public static bool Restore()
		{
			if (!File.Exists(m_storageFilePath))
			{
				Log.Write(LogLevel.NORMAL, "TelegramChatsStorage: Restore(): Cannot restore - storage file doesn't exist");
				return false;
			}

			string chatIDsStr = File.ReadAllText(m_storageFilePath);
			if(chatIDsStr.Trim().Length <= 0)
			{
				Log.Write(LogLevel.NORMAL, "TelegramChatsStorage: Restore(): Cannot restore - storage file is empty. Using the empty list instead");
				return false;
			}

			string[] userIDsStr = chatIDsStr.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			if(userIDsStr.Length <= 0)
			{
				Log.Write(LogLevel.NORMAL, "TelegramChatsStorage: Restore(): Cannot restore - parsing error");
				return false;
			}

			long[] userIDs = Array.ConvertAll(userIDsStr, long.Parse);
			if(userIDs.Length <= 0)
			{
				Log.Write(LogLevel.NORMAL, "TelegramChatsStorage: Restore(): Cannot restore - convertation error");
				return false;
			}

			m_TelegramChatIDs.AddRange(userIDs);
			return true;
		}
	}
}
