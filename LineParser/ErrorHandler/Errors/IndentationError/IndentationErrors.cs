using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface IndentationErrors
	{
		string unknownIndentStarter  (string[] arg);
		string firstLineIndentError  (string[] arg);
		string indentationError (string[] arg);
		string expectingBodyAfterScopeStarter (string[] arg);

	}


	public class IndentationErrorsOrder
	{
		public static Dictionary<string, Func<string[], string>> getMessages(IndentationErrors theLogicOrder)
		{
			Dictionary<string, Func<string[], string>> messages = new Dictionary<string, Func<string[], string>> ();

			messages.Add (IndentationErrorType.unknownIndentStarter.ToString(), theLogicOrder.unknownIndentStarter);
			messages.Add (IndentationErrorType.firstLineIndentError.ToString(), theLogicOrder.firstLineIndentError);
			messages.Add (IndentationErrorType.indentationError.ToString(), theLogicOrder.indentationError);
			messages.Add (IndentationErrorType.expectingBodyAfterScopeStarter.ToString(), theLogicOrder.expectingBodyAfterScopeStarter);

			return messages;
		}
	}
}


