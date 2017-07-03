using ErrorHandler;
using System.Collections;
using Compiler;
using Runtime;

namespace SyntaxCheck{
	
	internal class CodeSyntaxWalker{
		
		
		public static void walkAllLines(CodeLine[] allColdeLines, Scope mainScope){
			/*
			foreach (CodeLine c in allColdeLines)
				findCommandType (c, mainScope);
			*/
		}
		
		private static void findCommandType(CodeLine c, Scope mainScope){
			checkForUnknown (c.logicOrder, c.lineNumber);
			
			if (PossibleVariableDeclare.checkForVariableDecleration (c.logicOrder, c.lineNumber))
				return;
			
			if (PossibleSpeciallWord.checkForSpeciallWord (c.logicOrder, c.lineNumber, mainScope))
				return;
			
			if (PossibleFunctionCall.possibleFunctionCall (c.logicOrder, c.lineNumber))
				return;
			
			ErrorMessage.sendErrorMessage (c.lineNumber, ErrorType.LogicOrder, LogicErrorType.unknownLogic.ToString(), null);
		}
		
		
		static void checkForUnknown(Logic[] logicOrder, int lineNumber){
			foreach (Logic L in logicOrder)
				if (L.currentType == WordTypes.unknown)
					ErrorMessage.sendErrorMessage (lineNumber, "unknown logic!");
		}
	}
	

}