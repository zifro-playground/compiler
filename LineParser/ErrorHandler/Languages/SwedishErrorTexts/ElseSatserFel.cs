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

		public string unknownFormat (string[] arg)
		{
			return "Okänt format i din Else sats";
		}

		public string missingStatementLink (string[] arg)
		{
			return "Else måste vara länkat till en If sats";
		}

		#endregion

	}
}

