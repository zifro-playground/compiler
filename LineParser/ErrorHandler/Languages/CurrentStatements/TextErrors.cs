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
		public static Func<string[], string>[] getStatements(TextErrors theLogicOrder){
			List<Func<string[], string>> statements = new List<Func<string[], string>> ();

			statements.Add (theLogicOrder.expectedPlusSignBetweenStrings);
			statements.Add (theLogicOrder.expectedATextValue);
			statements.Add (theLogicOrder.expressionNeedsToEndWithAString);
			statements.Add (theLogicOrder.textParsingMalfunction);


			return statements.ToArray ();
		}
	}
}


