using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace MedocUpdates
{
	class MedocVersion
	{
		// Used for calculation and comparison
		int rawfirst;
		int rawsecond;
		int rawthird;

		// TODO: Rename those fields
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
				// TODO: Do this with Regex
				//	Regex regex = new Regex(@"\d{2}.\d{2}.\d{3}");
				//	string[] values = regex.Split(value);

				string[] values = value.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

				int.TryParse(values[0], out this.rawfirst);
				int.TryParse(values[1], out this.rawsecond);
				int.TryParse(values[2], out this.rawthird);

				this.first = values[0];
				this.second = values[1];
				this.third = values[2];
			}
		}

		MedocVersion()
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
	}
	public class MedocVersioning
	{
		string rawversion;
		public MedocVersioning(string version)
		{
			this.rawversion = version;
			// Convert
		}
	}
}
