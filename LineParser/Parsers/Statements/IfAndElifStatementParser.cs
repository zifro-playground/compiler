using ErrorHandler;
using System.Collections;
using Runtime;

namespace Compiler {
	public class IfAndElifStatementParser{
		
		public static Logic parseStatement(Logic[] logicOrder, int lineNumber, Scope currentScope){
			if (logicOrder [logicOrder.Length - 1].currentType != WordTypes.indentOperator) 
				ErrorMessage.sendErrorMessage (lineNumber, "Det saknas ett \":\" i slutet");
			
			if (logicOrder.Length < 3) 
				ErrorMessage.sendErrorMessage (lineNumber, "Okänt format i ditt uttryck");


			Logic[] statementLogic = InternalParseFunctions.getSubArray (logicOrder, 1, logicOrder.Length - 2, lineNumber);
			(logicOrder [0] as ScopeStarter).doEnterScope = StatementParser.parseAndCheckStatement (statementLogic, lineNumber, currentScope);
			(logicOrder [0] as ComparisonScope).initNextstatement ();

			if(logicOrder[0] is IfStatement)
				return (logicOrder[0] as IfStatement);
			
			return (logicOrder[0] as ElifStatement);
		}
		
	}

}