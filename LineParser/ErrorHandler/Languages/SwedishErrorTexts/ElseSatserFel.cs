using System;

namespace ErrorHandler
{
	public class ElseSatserFel : ElseStatementErrors
	{
		#region ElseStatementErrors implementation

		public string missingIndentOperator (string[] arg)
		{
			return "Det saknas ett \":\" i slutet av din Else sats";
		}

		/// Called if the else statement is not composed of only 2 parts.
		/// And else statement should only look like "else:"
		public string unknownFormat (string[] arg)
		{
			return "Okänt format i din Else sats. Kom ihåg att else ser ut såhär: \"else:\"";
		}

		public string missingStatementLink (string[] arg)
		{
			return "Else måste vara länkat till en If sats";
		}

		#endregion

	}
}

