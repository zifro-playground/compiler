using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface FunctionErrors
	{
		//string speciallDeclerationNeedsDeclaredVariable (string[] arg);
	}


	public class FunctionErrorsOrder
	{

		public static Dictionary<string, Func<string[], string>> getMessages(FunctionErrors theLogicOrder){
			Dictionary<string, Func<string[], string>> messages = new Dictionary<string, Func<string[], string>> ();

			//messages.Add (FunctionErrorType.speciallDeclerationNeedsDeclaredVariable.ToString(), theLogicOrder.speciallDeclerationNeedsDeclaredVariable);

			return messages;
		}
	}
}