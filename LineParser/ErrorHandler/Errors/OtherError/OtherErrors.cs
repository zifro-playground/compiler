using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface OtherErrors
	{
		//string corruptAndOrStatement (string[] arg);
	}


	public class OtherErrorsOrder
	{

		public static Dictionary<string, Func<string[], string>> getMessages(OtherErrors theLogicOrder){
			Dictionary<string, Func<string[], string>> messages = new Dictionary<string, Func<string[], string>> ();

			//messages.Add (OtherErrorType.corruptAndOrStatement.ToString(), theLogicOrder.corruptAndOrStatement);

			return messages;
		}
	}
}