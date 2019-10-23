using System;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace MedocUpdates
{
	/// <summary>
	/// Deprecated.
	/// </summary>
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
}
