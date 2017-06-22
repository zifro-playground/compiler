using System;
using ErrorHandler;
using Runtime;

namespace Compiler
{
	public class BreakAndContinueStatementParser
	{
		public static Logic parseContinueStatement(Logic[] logicOrder, int lineNumber, Scope currentScope){
			//We use a shallow copy because we alter logicOrder in case of speciall operators
			Logic[] cloneLogicOrder = (Logic[])logicOrder.Clone (); 
			if (cloneLogicOrder.Length != 1 || cloneLogicOrder[0].currentType != WordTypes.continueStatement)
				return new UnknownLogic (lineNumber);

			CodeWalker.continueLoop (findParentLoop (currentScope, lineNumber), lineNumber);
			return new Variable ();
		}

		public static Logic parseBreakStatement(Logic[] logicOrder, int lineNumber, Scope currentScope){
			//We use a shallow copy because we alter logicOrder in case of speciall operators
			Logic[] cloneLogicOrder = (Logic[])logicOrder.Clone (); 
			if (cloneLogicOrder.Length != 1 || cloneLogicOrder[0].currentType != WordTypes.breakStatement)
				return new UnknownLogic (lineNumber);

			CodeWalker.breakLoop (findParentLoop (currentScope, lineNumber), lineNumber);
			return new Variable ();
		}



		private static Scope findParentLoop(Scope currentScope, int lineNumber){
			if (currentScope.theScopeType == ScopeType.forLoop || currentScope.theScopeType == ScopeType.whileLoop)
				return currentScope;

			if (currentScope.parentScope == null || currentScope.parentScope.theScopeType == ScopeType.function)
				ErrorMessage.sendErrorMessage (lineNumber, "Du kan endast använda break i en loop");

			return findParentLoop (currentScope.parentScope, lineNumber);
		}
	}
}

