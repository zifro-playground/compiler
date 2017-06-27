using System;
using System.Collections.Generic;
using SyntaxCheck;

namespace Compiler
{
	public class ScopeParser
	{
		/// <summary>
		/// Parses all text into lines and lines into words. Parses the into scopes. Checks syntax for indentation, scopeStarters
		/// </summary>
		/// <returns>The into scopes.</returns>
		/// <param name="codeText">Code text.</param>
		public static Scope parseIntoScopes(string codeText){

			// Creates list with CodeLine objects with lineNumber, indentNumber and word list
			List<CodeLine> programLines = SyntaxCheck.parseLines(codeText);

			Scope mainScope = IndentParser.parseIntoScopes (programLines);
			ScopeLogicParser.parseScopeLineLogic (mainScope);
			ScopeStarterParser.checkScopeStarters (programLines, mainScope);
			//CodeSyntaxWalker.walkAllLines (programLines.ToArray (), mainScope);
	
			return mainScope;
		}


	}
}

