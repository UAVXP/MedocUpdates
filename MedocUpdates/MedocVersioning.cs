#define PARSE_VERSION_USING_REGEX

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace MedocUpdates
{
	// FIXME: Implement comparison with null, and nullable object overall
	public class MedocVersion : IEquatable<MedocVersion>
	{
		// Used for calculation and comparison
		int rawfirst;
		int rawsecond;
		int rawthird;

		string first;
		string second;
		string third;

		internal string Version
		{
			get
			{
				return String.Format("{0}.{1}.{2}", this.first, this.second, this.third);
			}

			set
			{
				if (value.Length <= 0 || value.Length > "xx.xx.xxx".Length)
					throw new Exception("Wrong version passed");

#if PARSE_VERSION_USING_REGEX
				//Regex regex = new Regex(@"\d{2}.\d{2}.\d{3}");
				Regex regex = new Regex(@"\D+");
				string[] values = regex.Split(value);
#else
				string[] values = value.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
#endif
				if (values.Length <= 0 || values.Length > 3)
					throw new Exception("Cannot parse version");

				if (!int.TryParse(values[0], out this.rawfirst) ||
					!int.TryParse(values[1], out this.rawsecond) ||
					!int.TryParse(values[2], out this.rawthird))
					throw new Exception("Wrong numbers parsed");

				this.first = values[0];
				this.second = values[1];
				this.third = values[2];
			}
		}

		public MedocVersion()
		{
			this.rawfirst = 0;
			this.rawsecond = 0;
			this.rawthird = 0;

			this.first = "";
			this.second = "";
			this.third = "";
		}

		public MedocVersion(string version) : base()
		{
			this.Version = version;
		}

		public MedocVersion(int first, int second, int third) : base()
		{
			this.Version = String.Format("{0:00}.{1:00}.{2:000}", first, second, third);
		}

		public static int Comparison(MedocVersion ver1, MedocVersion ver2)
		{
			if (ver1.rawfirst < ver2.rawfirst || ver1.rawsecond < ver2.rawsecond || ver1.rawthird < ver2.rawthird)
				return -1;
			else if (ver1.rawfirst == ver2.rawfirst && ver1.rawsecond == ver2.rawsecond && ver1.rawthird == ver2.rawthird)
				return 0;
			else if (ver1.rawfirst > ver2.rawfirst || ver1.rawsecond > ver2.rawsecond || ver1.rawthird > ver2.rawthird)
				return 1;

			return 0;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as MedocVersion);
		}

		public bool Equals(MedocVersion other)
		{
			return other != null &&
				   rawfirst == other.rawfirst &&
				   rawsecond == other.rawsecond &&
				   rawthird == other.rawthird &&
				   first == other.first &&
				   second == other.second &&
				   third == other.third;
		}

		public override int GetHashCode()
		{
			var hashCode = -320580756;
			hashCode = hashCode * -1521134295 + rawfirst.GetHashCode();
			hashCode = hashCode * -1521134295 + rawsecond.GetHashCode();
			hashCode = hashCode * -1521134295 + rawthird.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(first);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(second);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(third);
			return hashCode;
		}

		public override string ToString()
		{
			return this.Version;
		}

		public static bool operator <(MedocVersion ver1, MedocVersion ver2)
		{
			return Comparison(ver1, ver2) < 0;
		}

		public static bool operator >(MedocVersion ver1, MedocVersion ver2)
		{
			return Comparison(ver1, ver2) > 0;
		}

		public static bool operator <=(MedocVersion ver1, MedocVersion ver2)
		{
			return Comparison(ver1, ver2) <= 0;
		}

		public static bool operator >=(MedocVersion ver1,  MedocVersion ver2)
		{
			return Comparison(ver1, ver2) >= 0;
		}

		public static bool operator ==(MedocVersion ver1, MedocVersion ver2)
		{
			return Comparison(ver1, ver2) == 0;
		}

		public static bool operator !=(MedocVersion ver1, MedocVersion ver2)
		{
			return Comparison(ver1, ver2) != 0;
		}

		public static implicit operator MedocVersion(string v)
		{
			MedocVersion version = new MedocVersion(v);
			return version;
		}

		public static implicit operator string(MedocVersion v)
		{
			return v.Version;
		}

		public bool IsEmpty()
		{
			return	(this.rawfirst == 0 &&
					this.rawsecond == 0 &&
					this.rawthird == 0) ||

					(this.first == "" &&
					this.second == "" &&
					this.third == "");
		}

		public static bool IsVersion(string value, out MedocVersion version)
		{
			version = new MedocVersion(value);
			return !version.IsEmpty();
		}
	}
}
