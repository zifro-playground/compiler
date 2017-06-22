using ErrorHandler;
using System.Collections;
using System.Collections.Generic;

namespace Compiler {
	
	public class WhileLoopParser{
		
		
		// Could be optimized so that we save left, right and operators for next comparision so we do not need to parse every loop instance!
		
		
		public static Logic parseWhileLoop(Logic[] logicOrder, int lineNumber, Scope currentScope){
			if (logicOrder.Length < 3)
				ErrorMessage.sendErrorMessage (lineNumber,  ErrorType.WhileLoop, 1, null);
			
			if (logicOrder [logicOrder.Length -1].currentType != WordTypes.indentOperator)
				ErrorMessage.sendErrorMessage (lineNumber,  ErrorType.WhileLoop, 0, null);
			
			Logic[] statementLogic = new Logic[logicOrder.Length - 2];
			for (int i = 1; i < logicOrder.Length - 1; i++)
				statementLogic [i - 1] = logicOrder [i];
			
			
			WhileLoop returnLoop = new WhileLoop ();
			returnLoop.setTargetScope((logicOrder [0] as ScopeStarter).getTargetScope());
			returnLoop.getTargetScope().theScoopLoop = returnLoop;
			
			
			returnLoop.doEnterScope = StatementParser.parseAndCheckStatement (statementLogic, lineNumber, currentScope);
			returnLoop.theStatement = statementLogic;
			
			
			return returnLoop;
		}
		
		
	}

}