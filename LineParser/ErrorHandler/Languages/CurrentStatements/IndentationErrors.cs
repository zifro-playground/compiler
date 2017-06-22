using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface IndentationErrors
	{
		string unknownIndentStarter  (string[] arg);
		string firstLineIndentError  (string[] arg);
		string indentationError (string[] arg);
		string indentExpectingBody (string[] arg);

	}


	public class IndentationErrorsOrder
	{
		public static Func<string[], string>[] getStatements(IndentationErrors theLogicOrder){
			List<Func<string[], string>> statements = new List<Func<string[], string>> ();

			statements.Add (theLogicOrder.unknownIndentStarter);
			statements.Add (theLogicOrder.firstLineIndentError);
			statements.Add (theLogicOrder.indentationError);
			statements.Add (theLogicOrder.indentExpectingBody);


			return statements.ToArray ();
		}
	}
}


