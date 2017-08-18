using System;

namespace ErrorHandler
{
	public class VariabelFel : VariableErrors
	{
		#region Errors from SyntaxCheck

		#endregion



		#region Errors from Runtime
		public string speciallDeclerationNeedsDeclaredVariable (string[] arg)
		{
			return "För att använda specialtilldelning, så måste variabeln först vara deklarerad";
		}

		#endregion
	}
}

