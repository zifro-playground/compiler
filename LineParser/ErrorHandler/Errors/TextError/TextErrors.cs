using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface TextErrors
	{
		string expectedPlusSignBetweenStrings(string[] arg);
		string expectedATextValue(string[] arg);
		string expressionNeedsToEndWithAString(string[] arg);
	}


	public class TextErrorsOrder
	{
		public static Dictionary<string, Func<string[], string>> getMessages(TextErrors theLogicOrder){
			Dictionary<string, Func<string[], string>> messages = new Dictionary<string, Func<string[], string>> ();

			messages.Add (TextErrorType.expectedPlusSignBetweenStrings.ToString(), theLogicOrder.expectedPlusSignBetweenStrings);
			messages.Add (TextErrorType.expectedATextValue.ToString(), theLogicOrder.expectedATextValue);
			messages.Add (TextErrorType.expressionNeedsToEndWithAString.ToString(), theLogicOrder.expressionNeedsToEndWithAString);

			return messages;
		}
	}
}


