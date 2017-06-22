using System;
using ErrorHandler;
using Runtime;

namespace Compiler
{
	public class ReturnStatementParser
	{
		public static Logic parseReturnStatement(Logic[] logicOrder, int lineNumber, Scope currentScope){
			if (logicOrder.Length <= 1)
				ErrorMessage.sendErrorMessage (lineNumber, "Du måste returnera något");

			Logic[] followOrder = InternalParseFunctions.getSubArray (logicOrder, 1, logicOrder.Length - 1, lineNumber);
			Variable returnSum = SumParser.parseIntoSum (followOrder, lineNumber, currentScope);


			if (logicOrder [0].currentType != WordTypes.returnStatement)
				ErrorMessage.sendErrorMessage (lineNumber, "Return måste vara i början av kodraden");
			ReturnStatement theReturn = (logicOrder [0] as ReturnStatement);

			CodeLine parentLine = theReturn.FunctionParent.parentScope.codeLines [theReturn.FunctionParent.parentScope.lastReadLine];
			ReturnMemoryControll.insertReturnValue (parentLine, lineNumber, returnSum);


			currentScope.isReturning = true;
			CodeWalker.setReturnTarget (theReturn.FunctionParent.parentScope);

			return (logicOrder [0] as ReturnStatement);
		}

	}





}

