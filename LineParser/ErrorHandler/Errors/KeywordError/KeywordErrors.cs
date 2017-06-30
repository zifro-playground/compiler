using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface KeywordErrors
	{
		//string speciallDeclerationNeedsDeclaredVariable (string[] arg);
	}


	public class KeywordErrorsOrder
	{

		public static Dictionary<string, Func<string[], string>> getMessages(KeywordErrors theLogicOrder){
			Dictionary<string, Func<string[], string>> messages = new Dictionary<string, Func<string[], string>> ();

			//messages.Add (KeywordErrorType.speciallDeclerationNeedsDeclaredVariable.ToString(), theLogicOrder.speciallDeclerationNeedsDeclaredVariable);

			return messages;
		}
	}
}