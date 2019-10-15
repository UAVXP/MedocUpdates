using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MedocUpdates
{
	class Telegram
	{
		private TelegramBotClient botClient;
		private string botToken;

		public Telegram()
		{
			string[] args = Environment.GetCommandLineArgs();
			if (args.Length <= 0)
				Log.LogFallbackInternal("Log: Something went wrong with the application arguments");
			if (args[0].Equals(""))
				Log.LogFallbackInternal("Log: Cannot find filename in application arguments");

			// TODO: Refactor this
			int tokenArg = Array.FindIndex(args, element => element.StartsWith("-token", StringComparison.Ordinal));
			if (tokenArg > 0) // Skip the first argument (a filename)
			{
				string tokenStr = args[tokenArg + 1];
				botToken = tokenStr;
			}

			botClient = new TelegramBotClient(botToken);
			Console.WriteLine(botClient.GetMeAsync().Result.Username);

			botClient.OnMessage += OnMessageReceived;
			botClient.StartReceiving();
		}

		private async void OnMessageReceived(object sender, MessageEventArgs e)
		{
			Message message = e.Message;
			if (message == null)
				return;

			if(message.Type != MessageType.Text)
				return;

			Chat chat = e.Message.Chat;
			if(chat == null)
				return;

			string lastMessage = message.Text.Trim().Split(' ')[0];
			if(lastMessage == "/start")
			{
				//InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
				//{
				//	new [] // first row
    //                {
				//		InlineKeyboardButton.WithCallbackData("Subscribe")
				//	}
				//});

				//await botClient.SendTextMessageAsync(
				//		chat.Id,
				//		"Choose",
				//		replyMarkup: inlineKeyboard);
			}
			else if (lastMessage == "/hello")
			{
				if(SessionStorage.inside.TelegramUserIDs.IndexOf(chat.Id) >= 0)
				{
					SendMessage(chat.Id, "You have already been subscribed.");
					return;
				}

				SessionStorage.inside.TelegramUserIDs.Add(chat.Id);
				SendMessage(chat.Id, "You have been subscribed to M.E.Doc updates.");
			}
			else if(lastMessage == "/bye")
			{
				if (SessionStorage.inside.TelegramUserIDs.IndexOf(chat.Id) < 0)
				{
					SendMessage(chat.Id, "You are not subscribed.");
					return;
				}

				SessionStorage.inside.TelegramUserIDs.Remove(chat.Id);
				SendMessage(chat.Id, "Bye.");
			}
		}

		public async void SendMessage(long userID, string textmessage)
		{
			try
			{
				Message message = await botClient.SendTextMessageAsync(chatId: userID,
					text: textmessage,
					parseMode: ParseMode.Default,
					disableNotification: false
				);
			}
			catch(ApiRequestException ex)
			{
				Log.Write(String.Format("Telegram: Sending message to {0} has been failed\r\n{1}", userID, ex.Message));
			}
}

		public void SendMessageAll(string textmessage)
		{
			foreach (long userid in SessionStorage.inside.TelegramUserIDs)
			{
				SendMessage(userid, textmessage);
			}
		}
	}
}
