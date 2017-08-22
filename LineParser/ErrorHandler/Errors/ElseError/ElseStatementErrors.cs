using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface ElseStatementErrors
	{
		string missingIndentOperator (string[] arg);
		string unknownFormat (string[] arg);
		string missingIfBeforeElse (string[] arg);
		string elseCantLinkToElse (string[] arg);
	}


	public class ElseErrorsOrder
	{
		public static Dictionary<string, Func<string[], string>> getMessages(ElseStatementErrors theLogicOrder){
			Dictionary<string, Func<string[], string>> messages = new Dictionary<string, Func<string[], string>> ();

			messages.Add (ElseErrorType.missingIndentOperator.ToString(), theLogicOrder.missingIndentOperator);
			messages.Add (ElseErrorType.unknownFormat.ToString(), theLogicOrder.unknownFormat);
			messages.Add (ElseErrorType.missingIfBeforeElse.ToString(), theLogicOrder.missingIfBeforeElse);
			messages.Add (ElseErrorType.elseCantLinkToElse.ToString(), theLogicOrder.elseCantLinkToElse);

			return messages;
		}
	}
}

