using System;
using System.Collections.Generic;
using Runtime;

namespace Compiler
{
	public class PostLogicParsing
	{

		/// <summary>
		/// Split Packages and Statements. Combine Variables and Packages into FunctionCalls.
		/// </summary>
		public static Logic[] parsePostLogics(Logic[] logicOrder, int lineNumber, Scope currentScope){
			logicOrder = splitPackagesAndStatements(logicOrder, lineNumber, currentScope);
			return parseVariablesAndPackagesIntoFunctionCalls (logicOrder, lineNumber, currentScope);
		}


		private static Logic[] splitPackagesAndStatements(Logic[] logicOrder, int lineNumber, Scope currentScope){
			List<Logic> newOrder = new List<Logic> ();

			for (int i = 0; i < logicOrder.Length; i++) {
				if (logicOrder [i].currentType == WordTypes.functionCall && SpecialWordParser.isKeyWord ((logicOrder [i] as FunctionCall).name)) {

					Package temp = new Package ("(" + (logicOrder [i] as FunctionCall).parameter + ")", lineNumber);
					temp.logicOrder = WordsToLogicParser.determineLogicFromWords (new string[]{ temp.word}, lineNumber, currentScope);

					if ((logicOrder [i] as FunctionCall).name == "if") {
						newOrder.Add (new IfStatement ());
						newOrder.Add(temp);
					}
					else if ((logicOrder [i] as FunctionCall).name == "while") {
						newOrder.Add (new WhileLoop ());
						newOrder.Add(temp);
					}

				} else
					newOrder.Add (logicOrder [i]);
			}
			return newOrder.ToArray();
		}


		private static Logic[] parseVariablesAndPackagesIntoFunctionCalls(Logic[] logicOrder, int lineNumber, Scope currentScope){
			List<Logic> tempOrder = new List<Logic> ();
			
			for (int i = 0; i < logicOrder.Length; i++)
				if (logicOrder [i].currentType == WordTypes.variable && (i+1 < logicOrder.Length && logicOrder[i+1].currentType == WordTypes.package)) {
					Logic theFuncCall = FunctionParser.parseIntoFunctionCall (logicOrder [i].word + logicOrder [i + 1].word, lineNumber, currentScope);
					tempOrder.Add (theFuncCall);
					i += 1;
				} 
				else
					tempOrder.Add (logicOrder [i]);
			
			return tempOrder.ToArray ();
		}

	}
}

