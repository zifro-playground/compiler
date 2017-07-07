using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface LogicErrors
	{
		string unknownLogic (string[] arg);
		string missingIndentOperator (string[] arg);
		string corruptAndOrStatement (string[] arg);

	}

	public class LogicErrorsOrder{

		[System.Obsolete("Use getMessages() instead", true)]
		public static Func<string[], string>[]  getStatements(LogicErrors theLogicOrder){
			List<Func<string[], string>> statements = new List<Func<string[], string>> ();

			statements.Add (theLogicOrder.unknownLogic);
			statements.Add (theLogicOrder.missingIndentOperator);

			return statements.ToArray ();
		}

		public static Dictionary<string, Func<string[], string>> getMessages(LogicErrors theLogicOrder){
			Dictionary<string, Func<string[], string>> messages = new Dictionary<string, Func<string[], string>> ();

			messages.Add (LogicErrorType.unknownLogic.ToString(), theLogicOrder.unknownLogic);
			messages.Add (LogicErrorType.missingIndentOperator.ToString(), theLogicOrder.missingIndentOperator);
			messages.Add (LogicErrorType.corruptAndOrStatement.ToString(), theLogicOrder.corruptAndOrStatement);

			return messages;
		}
	}
}

