using System.Collections;
using Compiler;

namespace SyntaxCheck{

	internal class PossibleFunctionCall {
		
		public static bool possibleFunctionCall(Logic[] logicOrder, int lineNumber){
			if (logicOrder.Length == 1 && logicOrder [0].currentType == WordTypes.functionCall)
				return true;
			
			return false;
		}
	}
	
}