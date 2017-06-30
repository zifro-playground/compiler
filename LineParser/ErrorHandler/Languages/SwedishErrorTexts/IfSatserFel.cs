using System;
namespace ErrorHandler
{
	public class IfSatserFel : IfStatementErrors
	{
		#region IfStatementErrors implementation

		public string possibleComparissonStatements (string[] arg)
		{
			return "Okänt format av jämförelsen i IF satsen";
		}

		public string missingIndentOperator (string[] arg)
		{
			return "Det saknas ett \":\" i slutet av din IF sats";
		}

		public string unknownFormat (string[] arg)
		{
			return "Okänt format i din IF sats";
		}

		#endregion

	}
}

