using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface SystemFailureErrors
	{
		string corruptAndOrStatement (string[] arg);
		string textParsingMalfunction (string[] arg);
		string possibleComparissonStatements (string[] arg);
		string unknownLogic (string[] arg);
		string scopeParsingMalfunction (string[] arg);
	}

	public class SystemFailureErrorsOrder
	{
		public static Dictionary<string, Func<string[], string>> getMessages(SystemFailureErrors theLogicOrder){
			Dictionary<string, Func<string[], string>> messages = new Dictionary<string, Func<string[], string>> ();

			messages.Add (SystemFailureErrorType.corruptAndOrStatement.ToString(), theLogicOrder.corruptAndOrStatement);
			messages.Add (SystemFailureErrorType.textParsingMalfunction.ToString(), theLogicOrder.textParsingMalfunction);
			messages.Add (SystemFailureErrorType.possibleComparissonStatements.ToString(), theLogicOrder.possibleComparissonStatements);
			messages.Add (SystemFailureErrorType.unknownLogic.ToString(), theLogicOrder.unknownLogic);
			messages.Add (SystemFailureErrorType.scopeParsingMalfunction.ToString(), theLogicOrder.scopeParsingMalfunction);

			return messages;
		}
	}
}

