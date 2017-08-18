using ErrorHandler;
using System.Collections;
using Compiler;
using Runtime;

namespace SyntaxCheck{
	
	internal class CodeSyntaxWalker{
		
		private static void findCommandType(CodeLine c, Scope mainScope){
			checkForUnknown (c.logicOrder, c.lineNumber);
			
			if (PossibleVariableDeclare.checkForVariableDecleration (c.logicOrder, c.lineNumber))
				return;
			
			if (PossibleSpeciallWord.checkForSpeciallWord (c.logicOrder, c.lineNumber, mainScope))
				return;
			
			if (PossibleFunctionCall.possibleFunctionCall (c.logicOrder, c.lineNumber))
				return;
			
			ErrorMessage.sendErrorMessage (c.lineNumber, ErrorType.System, SystemFailureErrorType.unknownLogic.ToString(), new string[]{"0"});
		}
		
		static void checkForUnknown(Logic[] logicOrder, int lineNumber){
			foreach (Logic L in logicOrder)
				if (L.currentType == WordTypes.unknown)
					ErrorMessage.sendErrorMessage (lineNumber, ErrorType.System, SystemFailureErrorType.unknownLogic.ToString(), new string[]{"1"});
		}
	}
	

}