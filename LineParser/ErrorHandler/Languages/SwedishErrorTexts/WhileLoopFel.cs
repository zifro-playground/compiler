using System;

namespace ErrorHandler
{
	public class WhileLoopFel : WhileLoopErrors
	{
		#region Errors from SyntaxCheck

		public string missingIndentOperator (string[] arg)
		{
			return "Det saknas ett \":\" i slutet av din While loop";
		}

		public string unknownFormat (string[] arg)
		{
			return "Okänt format i din While loop";
		}

		#endregion



		#region Errors from Runtime

		#endregion
	}
}

