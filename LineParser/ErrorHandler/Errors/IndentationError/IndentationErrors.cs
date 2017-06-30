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
		[System.Obsolete("Use getMessages() instead", true)]
		public static Func<string[], string>[] getStatements(IndentationErrors theLogicOrder){
			List<Func<string[], string>> statements = new List<Func<string[], string>> ();

			statements.Add (theLogicOrder.unknownIndentStarter);
			statements.Add (theLogicOrder.firstLineIndentError);
			statements.Add (theLogicOrder.indentationError);
			statements.Add (theLogicOrder.indentExpectingBody);


			return statements.ToArray ();
		}

		public static Dictionary<string, Func<string[], string>> getMessages(IndentationErrors theLogicOrder){
			Dictionary<string, Func<string[], string>> messages = new Dictionary<string, Func<string[], string>> ();

			messages.Add (IndentationErrorType.unknownIndentStarter.ToString(), theLogicOrder.unknownIndentStarter);
			messages.Add (IndentationErrorType.firstLineIndentError.ToString(), theLogicOrder.firstLineIndentError);
			messages.Add (IndentationErrorType.indentationError.ToString(), theLogicOrder.indentationError);
			messages.Add (IndentationErrorType.indentExpectingBody.ToString(), theLogicOrder.indentExpectingBody);

			return messages;
		}
	}
}


