using System;

namespace ErrorHandler
{
	public class VariabelFel : VariableErrors
	{
		#region VariableErrors implementation

		public string speciallDeclerationNeedsDeclaredVariable (string[] arg)
		{
			return "För att använda speciall tilldelning, så måste variabeln först vara deklarerad";
		}

		#endregion

	}
}

