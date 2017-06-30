using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface ElseStatementErrors
	{
		string missingIndentOperator  (string[] arg);
		string unknownFormat  (string[] arg);
		string missingStatementLink   (string[] arg);
	}


	public class ElseErrorsOrder
	{
		[System.Obsolete("Use getMessages() instead", true)]
		public static Func<string[], string>[] getStatements(ElseStatementErrors theLogicOrder){
			List<Func<string[], string>> statements = new List<Func<string[], string>> ();

			statements.Add (theLogicOrder.missingIndentOperator);
			statements.Add (theLogicOrder.unknownFormat);
			statements.Add(theLogicOrder.missingStatementLink);

			return statements.ToArray ();
		}

		public static Dictionary<string, Func<string[], string>> getMessages(ElseStatementErrors theLogicOrder){
			Dictionary<string, Func<string[], string>> messages = new Dictionary<string, Func<string[], string>> ();

			messages.Add (ElseErrorType.missingIndentOperator.ToString(), theLogicOrder.missingIndentOperator);
			messages.Add (ElseErrorType.unknownFormat.ToString(), theLogicOrder.unknownFormat);
			messages.Add (ElseErrorType.missingStatementLink.ToString(), theLogicOrder.missingStatementLink);

			return messages;
		}
	}
}

