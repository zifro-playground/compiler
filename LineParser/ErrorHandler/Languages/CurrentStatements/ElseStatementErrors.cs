using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface ElseStatementError
	{
		string missingIndentOperator  (string[] arg);
		string unknownFormat  (string[] arg);
		string missingStatementLink   (string[] arg);
	}


	public class ElseErrorsOrder
	{
		public static Func<string[], string>[] getStatements(ElseStatementError theLogicOrder){
			List<Func<string[], string>> statements = new List<Func<string[], string>> ();

			statements.Add (theLogicOrder.missingIndentOperator);
			statements.Add (theLogicOrder.unknownFormat);
			statements.Add(theLogicOrder.missingStatementLink);

			return statements.ToArray ();
		}
	}
}

