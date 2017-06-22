using System;
using System.Collections.Generic;

namespace Compiler
{
	public class IndentParser
	{
		public static Scope parseIntoScopes(List<CodeLine> programLines){

			Scope mainScope = parseCurrentScope (0, 0, programLines, true);

			return mainScope;
		}


		static Scope parseCurrentScope(int indentLevel, int startNumber, List<CodeLine> programLines, bool isMain){
			List<CodeLine> scopeLines = new List<CodeLine> ();
			List<Scope> childScopes = new List<Scope> ();
			ScopeType currentScopeType;

			//Set current Scope Type
			if (!isMain) {
				if (startNumber > 0) {
					scopeLines.Add (programLines [startNumber - 1]);
					currentScopeType = getNonMainScopeType (programLines [startNumber - 1].words, programLines [startNumber - 1].lineNumber);
				} else
					currentScopeType = ScopeType.unknown;
			}
			else
				currentScopeType = ScopeType.main;
			//


			int lastLine = startNumber;
			for (int i = startNumber; i < programLines.Count; i++) {
				int currentIndent = programLines [i].indentLevel;
				if (currentIndent == indentLevel) {
					lastLine = i;
					scopeLines.Add(programLines[i]);
				}

				if (currentIndent < indentLevel) 
					return new Scope (currentScopeType, startNumber, lastLine, indentLevel, scopeLines, childScopes, false);


				if (currentIndent > indentLevel) {
					Scope childScope = parseCurrentScope (currentIndent, i, programLines, false);
					i = childScope.endLine;
					childScopes.Add (childScope);

					if (i != programLines.Count - 1) 
					if (programLines [i + 1].indentLevel < indentLevel)
						lastLine = i;
				}

				if (i == programLines.Count - 1) {

					if (childScopes.Count > 0) {
						int childLastValue = childScopes [childScopes.Count - 1].endLine;
						if (childLastValue > lastLine)
							lastLine = childLastValue;
					}
					return new Scope (currentScopeType, startNumber, lastLine, indentLevel, scopeLines, childScopes, false);
				}

			}

			return new Scope (currentScopeType, startNumber, lastLine, indentLevel, scopeLines, childScopes, false);
		}




		static ScopeType getNonMainScopeType(string[] words, int lineNumber){

			ScopeType tempType = ScopeType.unknown;
			if (words [0].StartsWith("for"))
				tempType = ScopeType.forLoop;
			else if (words [0].StartsWith("while"))
				tempType = ScopeType.whileLoop;
			else if (words [0].StartsWith("def"))
				tempType = ScopeType.function;
			else if (words [0].StartsWith("if"))
				tempType = ScopeType.ifStatement;
			else if (words [0].StartsWith("else"))
				tempType = ScopeType.elseStatement;
			else if (words [0].StartsWith("elif"))
				tempType = ScopeType.elifStatement;

			if (tempType == ScopeType.unknown) {
				ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, ErrorHandler.ErrorType.Indentation, 0, null);
				return tempType;
			}

			return tempType;
		}



		static bool scopeCheck(Scope currentScope){

			foreach (Scope tempScope in currentScope.childScopes)
				if (!scopeCheck (tempScope))
					return false;

			if (currentScope.theScopeType == ScopeType.unknown)
				return false;


			return true;
		}

	}
}

