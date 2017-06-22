using System;
using System.Collections.Generic;
using ErrorHandler;
using Runtime;

namespace Compiler
{

	public class ScopeStarterParser{

		public static void checkScopeStarters(List<CodeLine> programLines, Scope mainScope){

			int expectedIndent = 0;
			bool expectedHigherIndent = true;
			bool isFirst = true;

			for (int i = 0; i < programLines.Count; i++) {
				if (expectedHigherIndent)
					if (isFirst) {
						if (programLines [i].indentLevel != 0)
						ErrorMessage.sendErrorMessage (programLines [i].lineNumber, ErrorType.Indentation, 1, null);
						isFirst = false;
					} 
					else {
						if (programLines [i].indentLevel != expectedIndent)
						ErrorMessage.sendErrorMessage (programLines [i].lineNumber, ErrorType.Indentation, 2, null);
					}


				if (SpeciallWordParser.isValidScopeStarter (programLines [i].logicOrder, programLines [i].lineNumber)) {
					expectedIndent = programLines [i].indentLevel + 1;
					expectedHigherIndent = true;

					if (i == programLines.Count - 1)
						ErrorMessage.sendErrorMessage (programLines [i].lineNumber, ErrorType.Indentation, 3, null);
				}
				else
					expectedHigherIndent = false;
			}

			setScopeStarterTargets (mainScope);
		}

		static void setScopeStarterTargets(Scope currentScope){
			int currentChildScope = 0;
			int linkCounter = 0;

			for(int i = 0; i < currentScope.codeLines.Count; i++){
				CodeLine line = currentScope.codeLines [i];

				if (SpeciallWordParser.isValidScopeStarter (line.logicOrder, line.lineNumber)) {
					if (currentChildScope > currentScope.childScopes.Count - 1)
						ErrorMessage.sendErrorMessage (line.lineNumber, " Något gick fel med Scope Parsingen");

					(line.logicOrder [0] as ScopeStarter).setTargetScope(currentScope.childScopes [currentChildScope]);
					currentChildScope++;


					//Links else/elif
					if (line.logicOrder [0] is ComparisonScope) {
						if (i == 0 && line.logicOrder [0] is IfStatement == false)
							ErrorMessage.sendErrorMessage (line.lineNumber, "Du kan inte börja med ett Else/Elif");

						if (line.logicOrder [0] is IfStatement == false)
							linkElseStatement (line.logicOrder [0], currentScope.codeLines [i - 1].logicOrder [0], currentScope, linkCounter, line.lineNumber);
					

						linkCounter++;
					} else
						linkCounter = 0;
				}


			}

			foreach (Scope tempScope in currentScope.childScopes)
				setScopeStarterTargets (tempScope);

		}

		static void linkElseStatement(Logic newStatement, Logic preLineLogic, Scope currentScope, int linkCounter, int lineNumber){
			if (linkCounter == 0)
				ErrorMessage.sendErrorMessage (lineNumber, "Finns inget if statment att länka med");
			if(preLineLogic is ElseStatement)
				ErrorMessage.sendErrorMessage (lineNumber, "Det går inte att länka med ett Else uttryck");

			(preLineLogic as ComparisonScope).linkNextStatement (newStatement as ComparisonScope);
		}


	}

}

