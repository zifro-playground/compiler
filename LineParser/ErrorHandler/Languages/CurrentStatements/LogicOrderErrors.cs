using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface LogicOrderError
	{
		string unknownLogic (string[] arg);
		string missingIndentOperator (string[] arg);

	}

	public class LogicErrorOrder{
		
		public static Func<string[], string>[]  getStatements(LogicOrderError theLogicOrder){
			List<Func<string[], string>> statements = new List<Func<string[], string>> ();

			statements.Add (theLogicOrder.unknownLogic);
			statements.Add (theLogicOrder.missingIndentOperator);

			return statements.ToArray ();
		}
	}
}

