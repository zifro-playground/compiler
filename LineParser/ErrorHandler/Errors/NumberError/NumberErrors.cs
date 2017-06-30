using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface NumberErrors
	{
		//string speciallDeclerationNeedsDeclaredVariable (string[] arg);
	}


	public class NumberErrorsOrder
	{

		public static Dictionary<string, Func<string[], string>> getMessages(NumberErrors theLogicOrder){
			Dictionary<string, Func<string[], string>> messages = new Dictionary<string, Func<string[], string>> ();

			//messages.Add (NumberErrorType.speciallDeclerationNeedsDeclaredVariable.ToString(), theLogicOrder.speciallDeclerationNeedsDeclaredVariable);

			return messages;
		}
	}
}