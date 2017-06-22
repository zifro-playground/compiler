using System;
using Compiler;

namespace Runtime
{
	public class PureFunctionCallParser
	{
		public static Logic parsePureFunctionCall(Logic[] logicOrder, int lineNumber, Scope currentScope){
			if (logicOrder.Length != 1)
				return new UnknownLogic(lineNumber);

			if (logicOrder [0].currentType == WordTypes.functionCall) {
				FunctionCall tempCall = (logicOrder [0] as FunctionCall);
				FunctionParser.linkFunctionCall (tempCall, lineNumber, currentScope);


				if (tempCall.targetFunc.pauseWalker)
					CodeWalker.pauseWalker ();
					
				tempCall.runFunction (currentScope);

				if(tempCall.targetFunc.isUserFunction)
					throw new FunctionCallException ();
				
				return tempCall;
			}
			else
				return new UnknownLogic(lineNumber);
		}
	}
}

