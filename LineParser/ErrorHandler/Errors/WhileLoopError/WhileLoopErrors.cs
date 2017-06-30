using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface WhileLoopErrors
	{
		string missingIndentOperator (string[] arg);
		string unknownFormat (string[] arg);

	}


	public class WhileErrorsOrder
	{
		[System.Obsolete("Use getMessages() instead", true)]
		public static Func<string[], string>[] getStatements(WhileLoopErrors theLogicOrder){
			List<Func<string[], string>> statements = new List<Func<string[], string>> ();

			statements.Add (theLogicOrder.missingIndentOperator);
			statements.Add (theLogicOrder.unknownFormat);


			return statements.ToArray ();
		}

		public static Dictionary<string, Func<string[], string>> getMessages(WhileLoopErrors theLogicOrder){
			Dictionary<string, Func<string[], string>> messages = new Dictionary<string, Func<string[], string>> ();

			messages.Add (WhileErrorType.missingIndentOperator.ToString(), theLogicOrder.missingIndentOperator);
			messages.Add (WhileErrorType.unknownFormat.ToString(), theLogicOrder.unknownFormat);

			return messages;
		}
	}
}

