using System;
using Runtime;
using ErrorHandler;

namespace Compiler
{
	public class ScopeReturnParser
	{
		public static void parseScopeReturns(Scope currentSuperParent, Scope currentScope){
			checkLinesInScope (currentSuperParent, currentScope);

			foreach (Scope s in currentScope.childScopes)
				parseScopeReturns (currentSuperParent, s);
		}

		private static void checkLinesInScope(Scope currentSuperParent, Scope currentScope){
			foreach (CodeLine c in currentScope.codeLines)
				if (c.logicOrder [0].currentType == WordTypes.returnStatement) {
					if (currentSuperParent.theScopeType != ScopeType.function)
						ErrorMessage.sendErrorMessage (c.lineNumber, "Du kan bara returnera värden ifrån en funktion " + currentSuperParent.theScopeType);

				
					(c.logicOrder [0] as ReturnStatement).FunctionParent = currentSuperParent;
				}
					
		}
	}
}

