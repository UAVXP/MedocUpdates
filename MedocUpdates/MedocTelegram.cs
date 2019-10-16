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
	[Serializable]
	public class MedocTelegramUser // Almost the same as Telegram.Bot.Types.User, but serializable
	{
		public int Id { get; set; }
		public bool IsBot { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Username { get; set; }
		public string LanguageCode { get; set; }

		public override bool Equals(object obj)
		{
			MedocTelegramUser user = (MedocTelegramUser)obj;
			return Id == user.Id &&
				   IsBot == user.IsBot &&
				   FirstName == user.FirstName &&
				   LastName == user.LastName &&
				   Username == user.Username &&
				   LanguageCode == user.LanguageCode;
		}

		public override int GetHashCode()
		{
			var hashCode = 1624733837;
			hashCode = hashCode * -1521134295 + Id.GetHashCode();
			hashCode = hashCode * -1521134295 + IsBot.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstName);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LastName);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Username);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LanguageCode);
			return hashCode;
		}

		public static bool operator ==(MedocTelegramUser left, MedocTelegramUser right)
		{
			return EqualityComparer<MedocTelegramUser>.Default.Equals(left, right);
		}

		public static bool operator !=(MedocTelegramUser left, MedocTelegramUser right)
		{
			return !(left == right);
		}

		public static implicit operator MedocTelegramUser(User v)
		{
			MedocTelegramUser medocTlgUser = new MedocTelegramUser();
			medocTlgUser.Id = v.Id;
			medocTlgUser.IsBot = v.IsBot;
			medocTlgUser.FirstName = v.FirstName;
			medocTlgUser.LastName = v.LastName;
			medocTlgUser.Username = v.Username;
			medocTlgUser.LanguageCode = v.LanguageCode;
			return medocTlgUser;
		}
	}

	class MedocTelegram
	{
		private TelegramBotClient botClient;
		private string botToken;

		public MedocTelegram()
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
			botClient.OnCallbackQuery += OnCallbackQueryReceived;
			botClient.StartReceiving();
		}

		public bool IsSubscribed(MedocTelegramUser user)
		{
			return SessionStorage.inside.TelegramUsers.IndexOf(user) >= 0;
		}

		public bool Subscribe(MedocTelegramUser user)
		{
			if(IsSubscribed(user))
				return false;

			SessionStorage.inside.TelegramUsers.Add(user);

			// Double check
			if (!IsSubscribed(user))
				return false;

			return true;
		}

		public bool Unsubscribe(MedocTelegramUser user)
		{
			if (!IsSubscribed(user))
				return false;

			if(!SessionStorage.inside.TelegramUsers.Remove(user))
				return false;

			// Does this user still subscribed?
			if (IsSubscribed(user))
				return false;

			return true;
		}

		private async void OnMessageReceived(object sender, MessageEventArgs e)
		{
			Message message = e.Message;
			if (message == null)
				return;

			if(message.Type != MessageType.Text)
				return;

			Chat chat = message.Chat;
			if(chat == null)
				return;

			MedocTelegramUser user = message.From;
			if(user == null)
				return;

			string lastMessage = message.Text.Trim().Split(' ')[0];
			if(lastMessage == "/start")
			{
				InlineKeyboardMarkup inlineKeyboard;
				string replyMessage = "Your move:";

				if (!IsSubscribed(user))
				{
					inlineKeyboard = new InlineKeyboardMarkup(new[]
					{
						new [] // first row
						{
							InlineKeyboardButton.WithCallbackData("Subscribe")
						}
					});
					replyMessage = "You're not subscribed to server updates";
				}
				else
				{
					inlineKeyboard = new InlineKeyboardMarkup(new[]
					{
						new [] // first row
						{
							InlineKeyboardButton.WithCallbackData("Unsubscribe")
						}
					});
					replyMessage = "You've already subscribed to server updates";
				}

				await botClient.SendTextMessageAsync(
						chat.Id,
						replyMessage,
						replyMarkup: inlineKeyboard);
			}
			else if (lastMessage == "/sub")
			{
				bool success = Subscribe(user);
				if(!success)
				{
					SendMessage(chat.Id, "Cannot subscribe - you're probably subscribed already\r\nUse /start to make sure");
					return;
				}

				SendMessage(chat.Id, "You have been subscribed to M.E.Doc updates.");
			}
			else if(lastMessage == "/unsub")
			{
				bool success = Unsubscribe(user);
				if (!success)
				{
					SendMessage(chat.Id, "Cannot unsubscribe - you're probably unsubscribed already.\r\nUse /start to make sure");
					return;
				}

				SendMessage(chat.Id, "You have been unsubscribed to M.E.Doc updates.");
			}
		}

		private async void OnCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
		{
			CallbackQuery callbackQuery = e.CallbackQuery;
			string data = callbackQuery.Data;
			MedocTelegramUser user = callbackQuery.From;

			if(data == "Subscribe")
			{
				await botClient.AnswerCallbackQueryAsync(
					callbackQuery.Id,
					"Subscribed.");

				bool success = Subscribe(user);
				if (!success)
				{
					SendMessage(callbackQuery.Message.Chat.Id, "Something went wrong with subscribing you to the updates");
					return;
				}

				SendMessage(callbackQuery.Message.Chat.Id, "You have been subscribed to M.E.Doc updates.");
			}
			else if(data == "Unsubscribe")
			{
				await botClient.AnswerCallbackQueryAsync(
					callbackQuery.Id,
					"Unsubscribed.");

				bool success = Unsubscribe(user);
				if (!success)
				{
					SendMessage(callbackQuery.Message.Chat.Id, "Something went wrong with unsubscribing you from the updates");
					return;
				}

				SendMessage(callbackQuery.Message.Chat.Id, "You have been unsubscribed to M.E.Doc updates.");
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
			foreach (MedocTelegramUser savedUser in SessionStorage.inside.TelegramUsers)
			{
				SendMessage(savedUser.Id, textmessage);
			}
		}
	}
}
