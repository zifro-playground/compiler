using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface FunctionErrors
	{
		string cantReturnFromMainScope (string[] arg);
	}


	public class FunctionErrorsOrder
	{

		public static Dictionary<string, Func<string[], string>> getMessages(FunctionErrors theLogicOrder){
			Dictionary<string, Func<string[], string>> messages = new Dictionary<string, Func<string[], string>> ();

			messages.Add (FunctionErrorType.cantReturnFromMainScope.ToString(), theLogicOrder.cantReturnFromMainScope);

			return messages;
		}
	}
}