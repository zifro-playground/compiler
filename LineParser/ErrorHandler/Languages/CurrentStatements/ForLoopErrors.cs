using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface ForLoopErrors
	{
		string missingIndentOperator (string[] arg);
		string unknownFormat (string[] arg);

		string expectVariableAt2 (string[] arg);
		string expectInAt3 (string[] arg);
		string expectRangeAt4 (string[] arg);
	}


	public class ForErrorsOrder
	{
		public static Func<string[], string>[] getStatements(ForLoopErrors theLogicOrder){
			List<Func<string[], string>> statements = new List<Func<string[], string>> ();

			statements.Add (theLogicOrder.missingIndentOperator);
			statements.Add (theLogicOrder.unknownFormat);
			statements.Add (theLogicOrder.expectVariableAt2);
			statements.Add (theLogicOrder.expectInAt3);
			statements.Add (theLogicOrder.expectRangeAt4);


			return statements.ToArray ();
		}

	}
}

