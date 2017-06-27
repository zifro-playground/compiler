using ErrorHandler;
using System.Collections;
using System.Collections.Generic;
using Runtime;

namespace Compiler {

	public class DefStatementParser{
		
		
		// Could be optimized so that we save left, right and operators for next comparision so we do not need to parse every loop instance!
		
		
		public static Logic parseDefStatement(Logic[] logicOrder, int lineNumber, Scope currentScope){
			FunctionCall theFuncCall = logicOrder [1] as FunctionCall;

			if(SpecialWordParser.isKeyWord(theFuncCall.name))
				ErrorMessage.sendErrorMessage (lineNumber,string.Format( "\"{0}\" är ett Python keyword, du kan därför inte döpa en funktion till det", theFuncCall.name));

			int paraAmount = (FunctionParser.getParameterAmount (theFuncCall.parameter, lineNumber, currentScope));
			UserFunction theFunc = new UserFunction (theFuncCall.name, (logicOrder [0] as ScopeStarter).getTargetScope(), paraAmount);
			currentScope.scopeFunctions.addFunction(theFunc);

			theFunc.inputParameterNames = FunctionParser.getParameterNames (theFuncCall.parameter, lineNumber, currentScope);
			
			return new DefStatement ();
		}
		
		
	}
	
}