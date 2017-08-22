using System;

namespace ErrorHandler
{
	public class ElseSatserFel : ElseStatementErrors
	{
		#region Errors from SyntaxCheck

		public string missingIndentOperator (string[] arg)
		{
			return "Det saknas ett \":\" i slutet av din Else sats";
		}

		/// Called if the else statement is not composed of only 2 parts.
		/// An else statement should only look like "else:"
		public string unknownFormat (string[] arg)
		{
			return "Okänt format i din Else sats. Kom ihåg att else ser ut såhär: \"else:\"";
		}


		/// Missing if before else so link can not be setup
		public string missingIfBeforeElse (string[] arg)
		{
			return "Else måste vara länkat till en If-sats";
		}

		/// Called if there is two else after each other
		public string elseCantLinkToElse (string[] arg)
		{
			return "Else måste komma direkt efter en If-sats. Kan inte kopplas till en annan else";
		}

		#endregion



		#region Errors from Runtime

		#endregion

	}
}

