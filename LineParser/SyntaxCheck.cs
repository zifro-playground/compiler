using System;
using System.Collections.Generic;
using B83;

namespace Compiler
{
	public class SyntaxCheck
	{

		public static B83.ExpressionParser.ExpressionParser globalParser;

		/// <summary>
		/// Declares globalParser and returns parsed lines in list of Codelines.
		/// </summary>
		public static List<CodeLine> parseLines(string currentText){
			globalParser = new B83.ExpressionParser.ExpressionParser ();

			List<ParsedLine> textLines = LineSplitter.splitTextIntoLines(currentText);
			return LineParser.parseLines (textLines);

		}
			
		/// Links fullText and actions (endWalker etc) to CodeWalker via setActions.
		public static void CompileCode(string fullText, Action endWalker, Action pauseWalker, Action activateFunctionColor, Action<int> setWalkerPos){
			Scope mainScope = ScopeParser.parseIntoScopes (fullText);
			Runtime.CodeWalker.setActions(endWalker, pauseWalker, activateFunctionColor, setWalkerPos, mainScope);	
		}
	}
}

