using System;

namespace Compiler
{
	public class ScopeLogicParser
	{

		public static void parseScopeLineLogic(Scope currentScope){
			if (currentScope.childScopes.Count == 0) {
				parseCodeLineLogic (currentScope);
				return;
			} 
			else 
				foreach (Scope tempScope in currentScope.childScopes)
					parseScopeLineLogic (tempScope);


			parseCodeLineLogic (currentScope);
		}

		static void parseCodeLineLogic(Scope currentScope){
			foreach (CodeLine tempLine in currentScope.codeLines) 
				tempLine.logicOrder = WordsToLogicParser.determineLogicFromWords (tempLine.words, tempLine.lineNumber, currentScope);
		}

	}
}

