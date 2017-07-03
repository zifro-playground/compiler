using System;
namespace ErrorHandler
{
	public class IfSatserFel : IfStatementErrors
	{
		#region IfStatementErrors implementation

		public string possibleComparissonStatements (string[] arg)
		{
			return "Okänt format av jämförelsen i  if-satsen";
		}

		public string missingIndentOperator (string[] arg)
		{
			return "Det saknas ett \":\" i slutet av din if-sats";
		}

		public string unknownFormat (string[] arg)
		{
			return "Okänt format i din if-sats. Kom ihåg att en if-sats ska likna: \"if jämförelse:\"";
		}

		#endregion

	}
}

