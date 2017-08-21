using System;
using ErrorHandler;
using Runtime;

namespace Compiler
{
	public class ReturnStatementParser
	{
		public static Logic parseReturnStatement(Logic[] logicOrder, int lineNumber, Scope currentScope){
			if (logicOrder.Length <= 1) {
			}

			Variable returnSum;

			if (logicOrder.Length <= 1) {
				// Should return None if there is no sum to return
				returnSum = new Variable ();

				//This error should not exist
				ErrorMessage.sendErrorMessage (lineNumber, "I detta moment måste du returnera något ur funktionen");
			}
			else {
				Logic[] followOrder = InternalParseFunctions.getSubArray (logicOrder, 1, logicOrder.Length - 1, lineNumber);
				returnSum = SumParser.parseIntoSum (followOrder, lineNumber, currentScope);
			}
				
			ReturnStatement theReturn = (logicOrder [0] as ReturnStatement);
			theReturn.findFunctionParent (currentScope, lineNumber);

			CodeLine parentLine = theReturn.FunctionParent.parentScope.codeLines [theReturn.FunctionParent.parentScope.lastReadLine];
			ReturnMemoryControll.insertReturnValue (parentLine, lineNumber, returnSum);


			currentScope.isReturning = true;
			CodeWalker.setReturnTarget (theReturn.FunctionParent.parentScope);

			return (logicOrder [0] as ReturnStatement);
		}

	}





}

