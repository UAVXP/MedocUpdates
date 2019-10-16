using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedocUpdates
{
	public static class ParsedArgs
	{
		public class ArgsPair
		{
			string token;
			string arg;
		}

		public static List<ArgsPair> args = new List<ArgsPair>();
	}
}
