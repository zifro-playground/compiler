using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface TextErrors
	{
		string expectedPlusSignBetweenStrings(string[] arg);
		string expectedATextValue(string[] arg);
		string expressionNeedsToEndWithAString(string[] arg);
		string textParsingMalfunction(string[] arg);

	}


	public class TextErrorsOrder
	{
		[System.Obsolete("Use getMessages() instead", true)]
		public static Func<string[], string>[] getStatements(TextErrors theLogicOrder){
			List<Func<string[], string>> statements = new List<Func<string[], string>> ();

			statements.Add (theLogicOrder.expectedPlusSignBetweenStrings);
			statements.Add (theLogicOrder.expectedATextValue);
			statements.Add (theLogicOrder.expressionNeedsToEndWithAString);
			statements.Add (theLogicOrder.textParsingMalfunction);


			return statements.ToArray ();
		}

		public static Dictionary<string, Func<string[], string>> getMessages(TextErrors theLogicOrder){
			Dictionary<string, Func<string[], string>> messages = new Dictionary<string, Func<string[], string>> ();

			messages.Add (TextErrorType.expectedPlusSignBetweenStrings.ToString(), theLogicOrder.expectedPlusSignBetweenStrings);
			messages.Add (TextErrorType.expectedATextValue.ToString(), theLogicOrder.expectedATextValue);
			messages.Add (TextErrorType.expressionNeedsToEndWithAString.ToString(), theLogicOrder.expressionNeedsToEndWithAString);
			messages.Add (TextErrorType.textParsingMalfunction.ToString(), theLogicOrder.textParsingMalfunction);

			return messages;
		}
	}
}


