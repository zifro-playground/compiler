using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface VariableErrors
	{
		string speciallDeclerationNeedsDeclaredVariable (string[] arg);
	}


	public class VariableErrorsOrder
	{
		public static Func<string[], string>[] getStatements(VariableErrors theLogicOrder){
			List<Func<string[], string>> statements = new List<Func<string[], string>> ();

			statements.Add (theLogicOrder.speciallDeclerationNeedsDeclaredVariable);


			return statements.ToArray ();
		}
	}
}