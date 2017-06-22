using System;
using System.Collections.Generic;
using Runtime;
using ErrorHandler;
using System.Linq;

namespace Compiler
{
	public class BoolExpressionParser
	{


		public static Logic[] compressAndOrStatements(Logic[] preLogicOrder, int lineNumber, Scope currentScope){
			bool foundAndOr = BoolCompressExpressions.hasAndOr(preLogicOrder, lineNumber);

			if (foundAndOr) {
				string boolString = convertIntoBoolAlgebra (preLogicOrder, lineNumber, currentScope);
				if (SyntaxCheck.globalParser.Evaluate (boolString) != 0)
					return new Logic[1]{ new BooleanValue (true) };
				
				return new Logic[1]{ new BooleanValue (false) };
			} 
			else
				return (preLogicOrder [0] as Package).logicOrder;
		}


		private static string convertIntoBoolAlgebra(Logic[] logicOrder, int lineNumber, Scope currentScope){
			string boolString = "";
			for (int i = 0; i < logicOrder.Length; i++) {
				if (logicOrder [i] is AndOrOperator) {
					if(logicOrder[i].currentType == WordTypes.andOperator)
						boolString += "*";
					else
						boolString += "+";

					continue;
				}
					
				if (logicOrder [i].currentType == WordTypes.package) {
					Variable tempSum = SumParser.parseIntoSum ((logicOrder [i] as Package).logicOrder, lineNumber, currentScope);	
					if (tempSum.getBool ())
						boolString += "1";
					else
						boolString += "0";
				}
				else
					ErrorMessage.sendErrorMessage (lineNumber, "Korrupt uttryck");
			}
				
			return boolString;
		}
			

	}
}

