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

// TODO: Rewrite this mess!
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
			botToken = SessionStorage.inside.TelegramToken;

			string[] args = Environment.GetCommandLineArgs();
			if (args.Length <= 0)
				Log.Write("MedocTelegram: Something went wrong with the application arguments");
			if (args[0].Equals(""))
				Log.Write("MedocTelegram: Cannot find filename in application arguments");

			// TODO: Refactor this
			int tokenArg = Array.FindIndex(args, element => element.StartsWith("-token", StringComparison.Ordinal));
			if (tokenArg > 0) // Skip the first argument (a filename)
			{
				if((tokenArg + 1) >= args.Length)
				{
					Log.Write(LogLevel.NORMAL, "MedocTelegram: Unexpected end of the \"-token\" argument");
					return;
				}

				string tokenStr = args[tokenArg + 1];
				if(tokenStr.Length <= 0)
				{
					Log.Write(LogLevel.NORMAL, "MedocTelegram: Cannot set \"-token\" - wrong argument");
					return;
				}
				botToken = tokenStr;

				Log.Write("MedocTelegram: Token was forcibly set to " + tokenStr.Substring(0, 3) + "..."); // Print only first 3 symbols - just to make sure
			}

			try
			{
				botClient = new TelegramBotClient(botToken);
			}
			catch(Exception ex)
			{
				Log.Write("MedocTelegram: Cannot create TelegramBotClient object\r\n" + ex.Message);
				return;
			}

		//	Console.WriteLine(botClient.GetMeAsync().Result.Username);

			botClient.OnMessage += OnMessageReceived;
			botClient.OnCallbackQuery += OnCallbackQueryReceived;
			botClient.StartReceiving();

			if(SessionStorage.inside.TelegramUsers == null) // May be rare
			{
				Log.Write(LogLevel.EXPERT, "MedocTelegram: Internal user list is null");
				return;
			}
		}

		public bool IsSubscribed(MedocTelegramUser user)
		{
			return SessionStorage.inside.TelegramUsers.IndexOf(user) >= 0;
		}

		public bool Subscribe(MedocTelegramUser user)
		{
			if(IsSubscribed(user))
			{
				Log.Write(LogLevel.NORMAL, "MedocTelegram: Cannot subscribe @" + user.Username + " - already subscribed");
				return false;
			}

			SessionStorage.inside.TelegramUsers.Add(user);

			// Double check
			if (!IsSubscribed(user))
			{
				Log.Write(LogLevel.NORMAL, "MedocTelegram: Cannot subscribe @" + user.Username + " - something wrong with internal user list");
				return false;
			}

			// TODO: Should I call SessionStorage.Save() immediately here?

			return true;
		}

		public bool Unsubscribe(MedocTelegramUser user)
		{
			if (!IsSubscribed(user))
			{
				Log.Write(LogLevel.NORMAL, "MedocTelegram: Cannot unsubscribe @" + user.Username + " - already not subscribed");
				return false;
			}

			if(!SessionStorage.inside.TelegramUsers.Remove(user))
			{
				Log.Write(LogLevel.NORMAL, "MedocTelegram: Cannot unsubscribe " + user.Username + " - something wrong with internal user list");
				return false;
			}

			// Does this user still subscribed?
			if (IsSubscribed(user))
			{
				Log.Write(LogLevel.NORMAL, "MedocTelegram: Cannot unsubscribe " + user.Username + " - user list still has this user in it");
				return false;
			}

			// TODO: Should I call SessionStorage.Save() immediately here?

			return true;
		}

		public async void SendMessage(MedocTelegramUser user, string textmessage, ParseMode parsemode = ParseMode.Default)
		{
			try
			{
				await botClient.SendTextMessageAsync(chatId: user.Id,
					text: textmessage,
					parseMode: parsemode,
					disableNotification: false
				);
			}
			catch(Exception ex)
			{
				Log.Write(LogLevel.NORMAL, "MedocTelegram: Sending message to @" + user.Username + " has been failed\r\n" + ex.Message);
			}
		}

		public async void SendUpdateButton(MedocTelegramUser user)
		{
			//InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
			//{
			//	new [] // first row
			//	{
			//		InlineKeyboardButton.WithCallbackData("Update")
			//		// TODO: Make Cancel button
			//	}
			//});

			//await botClient.SendTextMessageAsync(
			//			user.Id,
			//			"Are you going to initiate an update sequence?",
			//			replyMarkup: inlineKeyboard);

			ReplyKeyboardMarkup ReplyKeyboard = new[]
					{
						new[] { "Update", "Cancel" },
						new[] { "/start", "Cancel" },
					};

			await botClient.SendTextMessageAsync(
				user.Id,
				"Choose",
				replyMarkup: ReplyKeyboard);
		}

		public void SendMessageAll(string textmessage)
		{
			Log.Write(LogLevel.NORMAL, "MedocTelegram: Sending message to all users\r\n" + textmessage);
			foreach (MedocTelegramUser savedUser in SessionStorage.inside.TelegramUsers)
			{
				SendMessage(savedUser, textmessage);
			}
		}

		public void SendUpdateButtonAll()
		{
			Log.Write(LogLevel.NORMAL, "MedocTelegram: Sending update button to all users");
			foreach (MedocTelegramUser savedUser in SessionStorage.inside.TelegramUsers)
			{
				SendUpdateButton(savedUser);
			}
		}


		private async void OnMessageReceived(object sender, MessageEventArgs e)
		{
			Message message = e.Message;
			if (message == null)
			{
				Log.Write(LogLevel.EXPERT, "MedocTelegram: OnMessageReceived(): Message is null");
				return;
			}

			if (message.Type != MessageType.Text)
			{
				Log.Write(LogLevel.EXPERT, "MedocTelegram: OnMessageReceived(): Message type is not a text (" + message.Type + ")");
				return;
			}

			Chat chat = message.Chat;
			if (chat == null)
			{
				Log.Write(LogLevel.EXPERT, "MedocTelegram: OnMessageReceived(): Chat object is null");
				return;
			}

			MedocTelegramUser user = message.From;
			if (user == null)
			{
				Log.Write(LogLevel.EXPERT, "MedocTelegram: OnMessageReceived(): User object is null");
				return;
			}

			string lastMessage = message.Text.Trim().Split(' ')[0];
			if (lastMessage == "/start")
			{
				InlineKeyboardMarkup inlineKeyboard;
				string replyMessage = "Your move:";

				if (!IsSubscribed(user))
				{
					// TODO: Make a function with creating a keyboard button/callback
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
				if (!success)
				{
					SendMessage(user, "Cannot subscribe - you're probably subscribed already\r\nUse /start to make sure"); // chat.Id?
					return;
				}

				SendMessage(user, "You have been subscribed to M.E.Doc updates."); // chat.Id?
				Log.Write(LogLevel.NORMAL, "MedocTelegram: OnMessageReceived(): User @" + user.Username + " is subscribed through the chat command");
			}
			else if (lastMessage == "/unsub")
			{
				bool success = Unsubscribe(user);
				if (!success)
				{
					SendMessage(user, "Cannot unsubscribe - you're probably unsubscribed already.\r\nUse /start to make sure"); // chat.Id?
					return;
				}

				SendMessage(user, "You have been unsubscribed to M.E.Doc updates."); // FIXME: chat.Id
				Log.Write(LogLevel.NORMAL, "MedocTelegram: OnMessageReceived(): User @" + user.Username + " is unsubscribed through the chat command");
			}
		}

		private async void OnCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
		{
			CallbackQuery callbackQuery = e.CallbackQuery;
			string data = callbackQuery.Data;
			MedocTelegramUser user = callbackQuery.From;

			// MedocTelegram.SendMessage used chat ID before, and not the user ID
			// I hope this will not frick up non-single user chats (channel/group ones)
			// But I think channel/group chats just can use inline commands?

			if (data == "Subscribe")
			{
				await botClient.AnswerCallbackQueryAsync(
					callbackQuery.Id,
					"Subscribed.");

				bool success = Subscribe(user);
				if (!success)
				{
					SendMessage(user, "Something went wrong with subscribing you to the updates");
					return;
				}

				SendMessage(user, "You have been subscribed to M.E.Doc updates.");
				Log.Write(LogLevel.NORMAL, "MedocTelegram: OnMessageReceived(): User @" + user.Username + " is subscribed through the keyboard button");
			}
			else if (data == "Unsubscribe")
			{
				await botClient.AnswerCallbackQueryAsync(
					callbackQuery.Id,
					"Unsubscribed.");

				bool success = Unsubscribe(user);
				if (!success)
				{
					SendMessage(user, "Something went wrong with unsubscribing you from the updates");
					return;
				}

				SendMessage(user, "You have been unsubscribed to M.E.Doc updates.");
				Log.Write(LogLevel.NORMAL, "MedocTelegram: OnMessageReceived(): User @" + user.Username + " is unsubscribed through the keyboard button");
			}
			else if (data == "Update")
			{
				Log.Write("MedocTelegram: User @" + user.Username + " tried to initiate an update sequence");
				// TODO: Here's an update sequence

				SendMessageAll("User @" + user.Username + " initiated an update sequence");
				//await botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "You have chosen " + e.CallbackQuery.Data, true); // Popup
				await botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
			}
		}
	}
}
