using System;

namespace ErrorHandler
{
	public class FunktionFel : FunctionErrors
	{
		#region Errors from SyntaxCheck


		#endregion



		#region Errors from Runtime

		/// Called if there is a return statement in the main scope
		public string cantReturnFromMainScope (string[] arg)
		{
			return "Du kan inte använda return utanför en funktion";
		}

		#endregion
	}
}

