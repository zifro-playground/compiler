using ErrorHandler;
using System.Collections;
using Runtime;

namespace Compiler {

	public class ForLoopParser{
		
		public static Logic parseForLoop(Logic[] logicOrder, int lineNumber, Scope currentScope){
			syntaxCheckForLoop (logicOrder, lineNumber);


			#region get & check variables
			FunctionCall rangeCall = (logicOrder [3] as FunctionCall);
			Variable[] inputVariables = FunctionParser.getValueOfParameters (rangeCall.parameter, rangeCall.targetFunc, lineNumber, currentScope, rangeCall);
			

			foreach (Variable v in inputVariables)
				if(v.variableType != VariableTypes.number)
					ErrorMessage.sendErrorMessage (lineNumber, ErrorHandler.ErrorLoops.forLoopArgumentType());
			#endregion

			ForLoop returnLoop = new ForLoop ();
			returnLoop.setTargetScope((logicOrder [0] as ScopeStarter).getTargetScope());
			returnLoop.getTargetScope().theScoopLoop = returnLoop;

			string counterName = logicOrder[1].word;
			if (inputVariables.Length == 1) 
				returnLoop.setLoopVariables(counterName, 0,inputVariables [0].getNumber (), 1);
			
			if (inputVariables.Length == 2) 
				returnLoop.setLoopVariables(counterName, inputVariables [0].getNumber (), inputVariables [1].getNumber (), 1);
			
			if (inputVariables.Length == 3) 
				returnLoop.setLoopVariables(counterName, inputVariables [0].getNumber (), inputVariables [1].getNumber (), inputVariables [2].getNumber ());
			
			returnLoop.doEnterScope = returnLoop.makeComparison (lineNumber, false);
			returnLoop.addCounterVariableToScope(lineNumber);


			return returnLoop;
		}
		


		private static void syntaxCheckForLoop(Logic[] logicOrder, int lineNumber){
			if (logicOrder.Length != 5)
				ErrorMessage.sendErrorMessage (lineNumber, ErrorType.ForLoop, 1, null);

			if (logicOrder [1].currentType != WordTypes.variable)
				ErrorMessage.sendErrorMessage (lineNumber, ErrorType.ForLoop, 2, null);

			if (logicOrder[2].word != "in")
				ErrorMessage.sendErrorMessage (lineNumber,  ErrorType.ForLoop, 3, null);

			if (logicOrder [3].currentType != WordTypes.functionCall) 
				ErrorMessage.sendErrorMessage (lineNumber,  ErrorType.ForLoop, 4, null);

			if ((logicOrder [3] as FunctionCall).name != "range") 
				ErrorMessage.sendErrorMessage (lineNumber,  ErrorType.ForLoop, 4, null);

			if (logicOrder [4].currentType != WordTypes.indentOperator)
				ErrorMessage.sendErrorMessage (lineNumber,  ErrorType.ForLoop, 0, null);
		}
	}
	
}