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
				if (expectedHigherIndent) {
					if (isFirst) {
						if (programLines [i].indentLevel != 0)
							ErrorMessage.sendErrorMessage (programLines [i].lineNumber, ErrorType.Indentation, IndentationErrorType.firstLineIndentError.ToString(), null);
						isFirst = false;
					} else {
						if (programLines [i].indentLevel != expectedIndent)
							ErrorMessage.sendErrorMessage (programLines [i].lineNumber, ErrorType.Indentation, IndentationErrorType.indentationError.ToString(), null);
					}
				}

				if (SpecialWordParser.isValidScopeStarter (programLines [i].logicOrder, programLines [i].lineNumber)) {
					expectedIndent = programLines [i].indentLevel + 1;
					expectedHigherIndent = true;

					if (i == programLines.Count - 1)
						ErrorMessage.sendErrorMessage (programLines [i].lineNumber, ErrorType.Indentation, IndentationErrorType.expectingBodyAfterScopeStarter.ToString(), null);
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

				if (SpecialWordParser.isValidScopeStarter (line.logicOrder, line.lineNumber)) {
					if (currentChildScope > currentScope.childScopes.Count - 1)
						ErrorMessage.sendErrorMessage (line.lineNumber, ErrorType.System, SystemFailureErrorType.scopeParsingMalfunction.ToString(), null);

					(line.logicOrder [0] as ScopeStarter).setTargetScope(currentScope.childScopes [currentChildScope]);
					currentChildScope++;


					// Links else/elif
					if (line.logicOrder [0] is ComparisonScope) {
						if (i == 0 && line.logicOrder [0] is IfStatement == false)
							ErrorMessage.sendErrorMessage (line.lineNumber , ErrorType.ElseStatements, ElseErrorType.missingIfBeforeElse.ToString(), null);

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
				ErrorMessage.sendErrorMessage (lineNumber , ErrorType.ElseStatements, ElseErrorType.missingIfBeforeElse.ToString(), null);
			if(preLineLogic is ElseStatement)
				ErrorMessage.sendErrorMessage (lineNumber, ErrorType.ElseStatements, ElseErrorType.elseCantLinkToElse.ToString(), null);

			(preLineLogic as ComparisonScope).linkNextStatement (newStatement as ComparisonScope);
		}


	}

}

