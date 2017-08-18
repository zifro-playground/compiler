using System;
namespace ErrorHandler
{
	public class IfSatserFel : IfStatementErrors
	{
		
		#region Errors from SyntaxCheck

		public string missingIndentOperator (string[] arg)
		{
			return "Det saknas ett \":\" i slutet av din if-sats";
		}

		public string unknownFormat (string[] arg)
		{
			return "Okänt format i din if-sats. Kom ihåg att en if-sats ska likna: \"if jämförelse:\"";
		}


		#endregion



		#region Errors from Runtime
		/// Called when expression type is not bool, number, string nor None.
		/// The expression in if-statement needs to evaluate to True or False.
		public string expressionNotCorrectType (string[] arg)
		{
			return "Uttrycket i en if-sats måste vara True (Sant) eller False (Falskt)";
		}

		#endregion
	}
}

