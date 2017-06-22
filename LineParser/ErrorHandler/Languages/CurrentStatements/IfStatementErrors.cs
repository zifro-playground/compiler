using System;
using System.Collections.Generic;

namespace ErrorHandler
{

	public interface IfStatementError
	{
		string missingIndentOperator (string[] arg);
		string unknownFormat (string[] arg);
		string possibleComparissonStatements (string[] arg);
	}


	public class IfErrorsOrder
	{
		public static Func<string[], string>[]  getStatements(IfStatementError theLogicOrder){
			List<Func<string[], string>> statements = new List<Func<string[], string>> ();

			statements.Add (theLogicOrder.missingIndentOperator);
			statements.Add (theLogicOrder.unknownFormat);
			statements.Add (theLogicOrder.possibleComparissonStatements);

			return statements.ToArray ();
		}
	}
}

