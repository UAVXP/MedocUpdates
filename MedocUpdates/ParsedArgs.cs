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
			public string Token { get; set; }
			public string Argument { get; set; }
		}

		private static List<ArgsPair> args = new List<ArgsPair>();

		private static void AddArgument(string[] inputargs, int token, ref ArgsPair pair)
		{
			if ((token + 1) >= inputargs.Length)
			{
				//Log.LogFallbackInternal(String.Format("ParsedArgs: Token \"{0}\" is the latest in arguments", inputargs[token]));
				return;
			}

			string tokenArgStr = inputargs[token + 1];
			if (tokenArgStr.Length <= 0)
			{
				//Log.LogFallbackInternal(String.Format("ParsedArgs: \"{0}\" token is simple (doesn't contain any arguments)", inputargs[token]));
				return;
			}

			if(tokenArgStr.StartsWith("-")) // That's another token, move on
			{
				return;
			}

			pair.Argument = tokenArgStr;
		}

		public static void SetArgs( string[] inputargs )
		{
			if (inputargs.Length <= 0)
				Log.LogFallbackInternal("ParsedArgs: Something went wrong with the application arguments");
			if (inputargs[0].Equals(""))
				Log.LogFallbackInternal("ParsedArgs: Cannot find filename in application arguments");

			//int tokenArg = Array.FindIndex(inputargs, element => element.StartsWith("-", StringComparison.Ordinal));
			//int[] tokenIdxs = inputargs.FindAllIndexOf("-");
			int[] tokenIdxs = Enumerable.Range(0, inputargs.Length).Where(i => inputargs[i].StartsWith("-")).ToArray();
			foreach (int token in tokenIdxs)
			{
				ArgsPair pair = new ArgsPair();

				string tokenStr = inputargs[token];
				if(tokenStr.Length <= 1)
					continue;

				pair.Token = tokenStr.Substring(1); // Remove "-"

				AddArgument(inputargs, token, ref pair);

				ParsedArgs.args.Add(pair);
			}
		}

		public static string GetArgument(string token)
		{
			if(args.Count <= 0)
				return "";

			ArgsPair pair = args.FirstOrDefault(element => element.Token.Equals(token));
			if(pair == null)
				return "";

			return pair.Argument;
		}

		public static void PrintArgs()
		{
			foreach(ArgsPair pair in args)
			{
				Console.WriteLine("{0} = {1}", pair.Token, pair.Argument);
			}
			Console.WriteLine();

			
			Console.WriteLine(GetArgument("loglevel"));
		}
	}
}
