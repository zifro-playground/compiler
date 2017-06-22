using System;

namespace ErrorHandler
{
	public class ForLoopFel : ForLoopErrors
	{
		public string missingIndentOperator (string[] arg)
		{
			return "Det saknas ett \":\" i slutet av din For Loop";
		}
		public string unknownFormat (string[] arg)
		{
			return "Okänt format i din For loop";
		}
		public string expectVariableAt2 (string[] arg)
		{
			return "I en For loop, förväntas det andra ordet att vara en Variabel";
		}
		public string expectInAt3 (string[] arg)
		{
			return "I en For loop, förväntas det tredje ordet att vara \"in\"";
		}
		public string expectRangeAt4 (string[] arg)
		{
			return "I en For loop, förväntas det fjärde ordet att vara ett funktions anrop till \"range\"";
		}

	}
}

