using System;
using System.Collections.Generic;

namespace ErrorHandler
{

	public interface IfStatementErrors
	{
		string missingIndentOperator (string[] arg);
		string unknownFormat (string[] arg);
		string expressionNotCorrectType (string[] arg);
	}


	public class IfErrorsOrder
	{
		public static Dictionary<string, Func<string[], string>> getMessages(IfStatementErrors theLogicOrder){
			Dictionary<string, Func<string[], string>> messages = new Dictionary<string, Func<string[], string>> ();

			messages.Add (IfErrorType.missingIndentOperator.ToString(), theLogicOrder.missingIndentOperator);
			messages.Add (IfErrorType.unknownFormat.ToString(), theLogicOrder.unknownFormat);
			messages.Add (IfErrorType.expressionNotCorrectType.ToString(), theLogicOrder.expressionNotCorrectType);

			return messages;
		}
	}


}

