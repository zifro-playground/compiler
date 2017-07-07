using System;
using System.Collections.Generic;

namespace ErrorHandler
{

	public interface IfStatementErrors
	{
		string missingIndentOperator (string[] arg);
		string unknownFormat (string[] arg);
		string possibleComparissonStatements (string[] arg);
		string expressionNotCorrectType (string[] arg);
	}


	public class IfErrorsOrder
	{
		[System.Obsolete("Use getMessages() instead", true)]
		public static Func<string[], string>[]  getStatements(IfStatementErrors theLogicOrder){
			List<Func<string[], string>> statements = new List<Func<string[], string>> ();

			statements.Add (theLogicOrder.missingIndentOperator);
			statements.Add (theLogicOrder.unknownFormat);
			statements.Add (theLogicOrder.possibleComparissonStatements);

			return statements.ToArray ();
		}

		public static Dictionary<string, Func<string[], string>> getMessages(IfStatementErrors theLogicOrder){
			Dictionary<string, Func<string[], string>> messages = new Dictionary<string, Func<string[], string>> ();

			messages.Add (IfErrorType.missingIndentOperator.ToString(), theLogicOrder.missingIndentOperator);
			messages.Add (IfErrorType.unknownFormat.ToString(), theLogicOrder.unknownFormat);
			messages.Add (IfErrorType.possibleComparissonStatements.ToString(), theLogicOrder.possibleComparissonStatements);
			messages.Add (IfErrorType.expressionNotCorrectType.ToString(), theLogicOrder.expressionNotCorrectType);

			return messages;
		}
	}


}

