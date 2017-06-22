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
		public static Func<string[], string>[] getStatements(WhileLoopErrors theLogicOrder){
			List<Func<string[], string>> statements = new List<Func<string[], string>> ();

			statements.Add (theLogicOrder.missingIndentOperator);
			statements.Add (theLogicOrder.unknownFormat);


			return statements.ToArray ();
		}
	}
}

