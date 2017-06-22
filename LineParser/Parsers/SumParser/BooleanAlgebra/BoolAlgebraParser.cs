using System;
using System.Collections.Generic;
using ErrorHandler;
using B83;
using Runtime;

namespace Compiler
{
	public class BoolAlgebraParser
	{
		public static bool parseBoolAlgebraStatement(BoolAlgebraWord[] logicOrder, int lineNumber){
			string calcString = "";
			foreach (BoolAlgebraWord w in logicOrder)
				calcString += w.calcWord;

			int sum = (int)SyntaxCheck.globalParser.Evaluate (calcString);

			return Convert.ToBoolean(sum);
		}




		public static BoolAlgebraWord[] parseIntoBoolAlgebra(Logic[] logicOrder, int lineNumber, Scope currentScope){
			List<BoolAlgebraWord> algebraList = new List<BoolAlgebraWord> ();

			int lastOperatorPos = -1;
			for (int i = 0; i < logicOrder.Length; i++)
				if (logicOrder [i] is AndOrOperator) {

					Logic[] partStatement = InternalParseFunctions.getSubArray (logicOrder, lastOperatorPos + 1, i-1, lineNumber);

					bool tempResult = StatementParser.parseExpression (partStatement, lineNumber, currentScope);
					algebraList.Add (new BoolAlgebraWord (tempResult.ToString ()));
					algebraList.Add (new BoolAlgebraWord (logicOrder [i].word));

					lastOperatorPos = i;
				}


			Logic[] finalStatement = InternalParseFunctions.getSubArray (logicOrder, lastOperatorPos + 1, logicOrder.Length-1, lineNumber);
			bool finalResult = StatementParser.parseExpression (finalStatement, lineNumber, currentScope);
			algebraList.Add (new BoolAlgebraWord (finalResult.ToString ()));

			return algebraList.ToArray ();
		}

	}

}

