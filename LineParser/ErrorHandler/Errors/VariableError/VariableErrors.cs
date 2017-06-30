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
		[System.Obsolete("Use getMessages() instead", true)]
		public static Func<string[], string>[] getStatements(VariableErrors theLogicOrder){
			List<Func<string[], string>> statements = new List<Func<string[], string>> ();

			statements.Add (theLogicOrder.speciallDeclerationNeedsDeclaredVariable);


			return statements.ToArray ();
		}

		public static Dictionary<string, Func<string[], string>> getMessages(VariableErrors theLogicOrder){
			Dictionary<string, Func<string[], string>> messages = new Dictionary<string, Func<string[], string>> ();

			messages.Add (VariableErrorType.speciallDeclerationNeedsDeclaredVariable.ToString(), theLogicOrder.speciallDeclerationNeedsDeclaredVariable);

			return messages;
		}
	}
}