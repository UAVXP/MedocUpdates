﻿using System;
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
	class MedocTelegram
	{
		private TelegramBotClient botClient;
		private string botToken;

		public MedocTelegram()
		{
			botToken = SessionStorage.inside.TelegramToken;

			string tokenStr = ParsedArgs.GetArgument("telegramtoken");
			if (tokenStr.Length > 0)
			{
				botToken = tokenStr;
				Log.Write("MedocTelegram: Token was forcibly set to " + tokenStr.Substring(0, 3) + "..."); // Print only first 3 symbols - just to make sure
			}

			try
			{
				botClient = new TelegramBotClient(botToken);
				Log.Write(LogLevel.NORMAL, "MedocTelegram: Created TelegramBotClient object");
			}
			catch(Exception ex)
			{
				Log.Write("MedocTelegram: Cannot create TelegramBotClient object\r\n" + ex.Message);
				return;
			}

			//Console.WriteLine(botClient.GetMeAsync().Result.Username);

			botClient.OnCallbackQuery += OnCallbackQueryReceived;
			botClient.OnInlineQuery += BotClient_OnInlineQuery;
			botClient.OnInlineResultChosen += BotClient_OnInlineResultChosen;
			botClient.OnMessage += OnMessageReceived;
			botClient.OnReceiveError += BotClient_OnReceiveError;
			botClient.OnReceiveGeneralError += BotClient_OnReceiveGeneralError;

			try
			{
				Log.Write(LogLevel.NORMAL, "MedocTelegram: Client begins to receive updates...");

			/* Not needed
				UpdateType[] updates = new UpdateType [4]{
					UpdateType.CallbackQuery,
					UpdateType.InlineQuery,
					UpdateType.ChosenInlineResult,
					UpdateType.Message
				};
			*/



				botClient.StartReceiving(/*updates*/); // Start loop
			}
			catch(Exception ex)
			{
				Log.Write(LogLevel.NORMAL, "MedocTelegram: Cannot start receiving updates\r\n" + ex.Message);
				return;
			}

			if (TelegramChatsStorage.TelegramChats == null) // May be rare
			{
				Log.Write(LogLevel.EXPERT, "MedocTelegram: Internal chat list is null");
				return;
			}
		}

		~MedocTelegram()
		{
			if(botClient != null)
			{
				try
				{
					Log.Write(LogLevel.NORMAL, "MedocTelegram: Trying to destroy client object");
					botClient.StopReceiving();
				}
				catch(Exception ex)
				{
					Log.Write(LogLevel.EXPERT, "MedocTelegram: Client destruction has been failed\r\n" + ex.Message);
				}
			}
		}

		private void BotClient_OnInlineQuery(object sender, InlineQueryEventArgs e)
		{
			//throw new NotImplementedException();
			Log.Write(LogLevel.NORMAL, "MedocTelegram: OnInlineQuery()\r\n" + e.InlineQuery.Query);
		}

		private void BotClient_OnInlineResultChosen(object sender, ChosenInlineResultEventArgs e)
		{
			//throw new NotImplementedException();
			Log.Write(LogLevel.NORMAL, "MedocTelegram: OnInlineResultChosen()\r\n" + e.ChosenInlineResult.Query);
		}

		private void BotClient_OnReceiveError(object sender, ReceiveErrorEventArgs e)
		{
			//throw new NotImplementedException();
			Log.Write(LogLevel.NORMAL, "MedocTelegram: OnReceiveError()\r\n" + e.ApiRequestException.Message);
		}

		private void BotClient_OnReceiveGeneralError(object sender, ReceiveGeneralErrorEventArgs e)
		{
			//throw new NotImplementedException();
			Log.Write(LogLevel.NORMAL, "MedocTelegram: OnReceiveGeneralError()\r\n" + e.Exception.Message + "\r\n" + e.Exception.StackTrace);
		}

		private bool IsSubscribed(long chatID)
		{
			return TelegramChatsStorage.TelegramChats.IndexOf(chatID) >= 0;
		}

		private bool SubscribeInternal(long chatID)
		{
			if(IsSubscribed(chatID))
			{
				Log.Write(LogLevel.NORMAL, "MedocTelegram: Cannot subscribe chat #" + chatID + " - already subscribed");
				return false;
			}

			TelegramChatsStorage.TelegramChats.Add(chatID);

			// Double check
			if (!IsSubscribed(chatID))
			{
				Log.Write(LogLevel.NORMAL, "MedocTelegram: Cannot subscribe chat #" + chatID + " - something wrong with internal chat list");
				return false;
			}

			TelegramChatsStorage.Save();

			return true;
		}

		private bool UnsubscribeInternal(long chatID)
		{
			if (!IsSubscribed(chatID))
			{
				Log.Write(LogLevel.NORMAL, "MedocTelegram: Cannot unsubscribe chat #" + chatID + " - already not subscribed");
				return false;
			}

			if(!TelegramChatsStorage.TelegramChats.Remove(chatID))
			{
				Log.Write(LogLevel.NORMAL, "MedocTelegram: Cannot unsubscribe chat #" + chatID + " - something wrong with internal chat list");
				return false;
			}

			// Does this user still subscribed?
			if (IsSubscribed(chatID))
			{
				Log.Write(LogLevel.NORMAL, "MedocTelegram: Cannot unsubscribe chat #" + chatID + " - chat list still has this chat in it");
				return false;
			}

			TelegramChatsStorage.Save();

			return true;
		}

		// Sending a message to a single user or channel/group
		public async void SendMessage(long chatID, string textmessage, ParseMode parsemode = ParseMode.Default)
		{
			//await botClient.SendChatActionAsync(chatID, ChatAction.FindLocation);
			//return;
			try
			{
				await botClient.SendTextMessageAsync(chatId: chatID,
					text: textmessage,
					parseMode: parsemode,
					disableNotification: false,
					replyMarkup: new ReplyKeyboardRemove()
				);
			}
			catch (Exception ex)
			{
				Log.Write(LogLevel.NORMAL, "MedocTelegram: Sending message to chat #" + chatID + " has been failed\r\n" + ex.Message);
			}
		}

		public void SendMessageAll(string textmessage, ParseMode parsemode = ParseMode.Default)
		{
			Log.Write(LogLevel.NORMAL, "MedocTelegram: Sending message to all chats\r\n" + textmessage);

			string logChatIDs = "";
			foreach (long chatID in TelegramChatsStorage.TelegramChats)
			{
				SendMessage(chatID, textmessage, parsemode);
				logChatIDs += chatID + ", ";
			}
			Log.Write(LogLevel.NORMAL, String.Format("MedocTelegram: Sending message to chats {0}\r\n{1}", logChatIDs, textmessage));
		}

		private async void Subscribe(long chatID, ChatMember member)
		{
			Chat chat = await botClient.GetChatAsync(chatID);

			if (chat.Type != ChatType.Private && member.Status > ChatMemberStatus.Administrator)
			{
				SendMessage(chatID, "You cannot subscribe since you're not an administrator");
				return;
			}

			bool success = SubscribeInternal(chatID);
			if (!success)
			{
				SendMessage(chatID, "Cannot subscribe - you're probably subscribed already\r\nUse /start to make sure");
				return;
			}

			SendMessage(chatID, member.User + " have subscribed this chat to M.E.Doc updates."); // TODO: Adaptive choice between member.User Username and FirstName
			Log.Write(LogLevel.NORMAL, "MedocTelegram: Chat " + (chat.Type != ChatType.Private ? chat.Title : chat.Username) + " was subscribed by " + member.User); // TODO: Adaptive choice between chat.Username and FirstName
		}

		private async void Unsubscribe(long chatID, ChatMember member)
		{
			//botClient.GetChatMembersCountAsync
			Chat chat = await botClient.GetChatAsync(chatID);

			if (chat.Type != ChatType.Private && member.Status > ChatMemberStatus.Administrator)
			{
				SendMessage(chatID, "You cannot unsubscribe since you're not an administrator");
				return;
			}

			bool success = UnsubscribeInternal(chatID);
			if (!success)
			{
				SendMessage(chatID, "Cannot unsubscribe - you're probably unsubscribed already.\r\nUse /start to make sure");
				return;
			}

			SendMessage(chatID, member.User + " have unsubscribed this chat from M.E.Doc updates."); // TODO: Adaptive choice between member.User Username and FirstName
			Log.Write(LogLevel.NORMAL, "MedocTelegram: Chat " + (chat.Type != ChatType.Private ? chat.Title : chat.Username) + " was unsubscribed by " + member.User); // TODO: Adaptive choice between chat.Username and FirstName
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

			long chatID = chat.Id;
			ChatMember member = new ChatMember();
			try
			{
				member = await botClient.GetChatMemberAsync(chatID, message.From.Id);
			}
			catch(Exception ex)
			{
				Log.Write(LogLevel.NORMAL, "MedocTelegram: OnMessageReceived(): Cannot get chat member - probably timeout\r\n" + ex.Message);
				return;
			}

			string lastMessage = message.Text.Trim().Split(new char[] { ' ', '@' }).First();
			if(!lastMessage.StartsWith("/"))
				return; // Regular chat message, not a command

			switch(lastMessage)
			{
			case "/start":
				{
					if (!IsSubscribed(chatID))
					{
						await botClient.SendTextMessageAsync(
							chatID,
							"This chat doesn't subscribed to server updates",
							replyMarkup: new InlineKeyboardMarkup(//new[] {
								//new [] {
									InlineKeyboardButton.WithCallbackData("Subscribe")
								//}
							//}
							)
						);
					}
					else
					{
						await botClient.SendTextMessageAsync(
							chatID,
							"This chat is already subscribed to server updates",
							replyMarkup: new InlineKeyboardMarkup(//new[] {
								//new [] {
									InlineKeyboardButton.WithCallbackData("Unsubscribe")
								//}
							//}
							)
						);
					}
					break;
				}
			case "/sub":
				{
					Subscribe(chatID, member);
					break;
				}
			case "/unsub":
				{
					Unsubscribe(chatID, member);
					break;
				}
			default:
				{
					Log.Write(LogLevel.NORMAL, "MedocTelegram: User requested usage");
					SendMessage(chatID, @"Usage:
/start - See subscribe/unsubscribe button
/sub - Subscribe chat to M.E.Doc updates notifications
/unsub - Unsubscribe chat from M.E.Doc updates notifications");
					break;
				}
			}
		}

		private async void OnCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
		{
			CallbackQuery callbackQuery = e.CallbackQuery;
			Message message = callbackQuery.Message;
			Chat chat = message.Chat;
			User user = callbackQuery.From;

			string data = callbackQuery.Data;
			long chatID = chat.Id;

			ChatMember member = new ChatMember();
			try
			{
				member = await botClient.GetChatMemberAsync(chatID, user.Id);
			}
			catch(Exception ex)
			{
				Log.Write(LogLevel.NORMAL, "MedocTelegram: OnCallbackQueryReceived(): Cannot get chat member - probably timeout\r\n" + ex.Message);
				return;
			}

			switch (data)
			{
			case "Subscribe":
				{
					Subscribe(chatID, member);

					// FIXME: Does this even needed?
					try
					{
						await botClient.AnswerCallbackQueryAsync(
							callbackQuery.Id,
							"Subscribed.");
					}
					catch (Exception ex)
					{
						Log.Write(LogLevel.NORMAL, "MedocTelegram: OnCallbackQueryReceived(): Exception error\r\n" + ex.Message);
					}

					break;
				}
			case "Unsubscribe":
				{
					Unsubscribe(chatID, member);

					// FIXME: Does this even needed?
					try
					{
						await botClient.AnswerCallbackQueryAsync(
							callbackQuery.Id,
							"Unsubscribed.");
					}
					catch(Exception ex)
					{
						Log.Write(LogLevel.NORMAL, "MedocTelegram: OnCallbackQueryReceived(): Exception error\r\n" + ex.Message);
					}

					break;
				}
			default:
				{
					Log.Write(LogLevel.NORMAL, "MedocTelegram: OnCallbackQueryReceived(): Chat #" + chatID + " has sent an unknown callback query (" + data + ", by @" + member.User + ")");

					break;
				}
			}
		}
	}
}
