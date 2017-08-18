using ErrorHandler;
using System.Collections;
using Compiler;

namespace SyntaxCheck{

	internal class PossibleSpeciallWord{
		
		public static bool checkForSpeciallWord(Logic[] logicOrder, int lineNumber, Scope mainScope){
			switch (logicOrder[0].currentType) {
			
			case WordTypes.ifOperator:
				return ifCheck (logicOrder, lineNumber);
			case WordTypes.elseOperator:
				return elseCheck(logicOrder, lineNumber);
				
			case WordTypes.forLoop:
				return forCheck(logicOrder, lineNumber, mainScope);
				
			case WordTypes.whileLoop:
				return whileCheck (logicOrder, lineNumber);
				
			case WordTypes.defStatement:
				return defCheck (logicOrder, lineNumber);

			case WordTypes.returnStatement:
				return returnCheck (logicOrder, lineNumber);
				
			default:
				return false;
			}
			
		}

		private static bool returnCheck(Logic[] logicOrder, int lineNumber){
			if (logicOrder.Length < 2)
				ErrorMessage.sendErrorMessage (lineNumber, "Du måste returnera något");

			ValidSumCheck.checkIfValidSum ((InternalParseFunctions.getSubArray(logicOrder, 1, logicOrder.Length-1, lineNumber)), lineNumber);


			return true;
		}
		
		
		private static bool ifCheck(Logic[] logicOrder, int lineNumber){
			if (logicOrder [logicOrder.Length - 1].currentType != WordTypes.indentOperator) 
				ErrorMessage.sendErrorMessage (lineNumber, ErrorType.IfStatements, IfErrorType.missingIndentOperator.ToString(), null);
			
			if (logicOrder.Length < 3) 
				ErrorMessage.sendErrorMessage (lineNumber, ErrorType.IfStatements, IfErrorType.unknownFormat.ToString(), null);
			
			
			Logic[] statementLogic = new Logic[logicOrder.Length - 2];
			
			for (int i = 1; i < logicOrder.Length - 1; i++)
				statementLogic [i - 1] = logicOrder [i];
			
			if(!PossibleStatement.validStatement(statementLogic, lineNumber))
				ErrorMessage.sendErrorMessage (lineNumber, ErrorType.System, SystemFailureErrorType.possibleComparissonStatements.ToString(), null);
			
			return true;
		}
		
		
		private static bool elseCheck(Logic[] logicOrder, int lineNumber){
			if (logicOrder.Length != 2) 
				ErrorMessage.sendErrorMessage (lineNumber, "Okänkt format på din Else sats");
			
			if (logicOrder [logicOrder.Length - 1].currentType != WordTypes.indentOperator) 
				ErrorMessage.sendErrorMessage (lineNumber, "Det saknas ett \":\" i slutet av din Else");
			
			return true;
		}
		
		private static bool forCheck(Logic[] logicOrder, int lineNumber, Scope currentScope){
			if (logicOrder.Length != 5)
				ErrorMessage.sendErrorMessage (lineNumber, "Okänt format på din for loop");
			
			if (logicOrder [1].currentType != WordTypes.variable)
				ErrorMessage.sendErrorMessage (lineNumber, "Förväntade sig en variabel som 2:a ord");
			
			if (logicOrder[2].word != "in")
				ErrorMessage.sendErrorMessage (lineNumber, "Förväntade sig ordet \"in\" 3:e ord");
			
			if (logicOrder [3].currentType != WordTypes.functionCall) 
				ErrorMessage.sendErrorMessage (lineNumber, "Förväntade sig funktionsanrop till \"range\" som 4:e ord");
			
			FunctionParser.linkFunctionCall ((logicOrder [3] as FunctionCall), lineNumber, currentScope);
			if ((logicOrder [3] as FunctionCall).targetFunc.name != "range") 
				ErrorMessage.sendErrorMessage (lineNumber, "Förväntade sig funktionsanrop till \"range\" som 4:e ord");
			
			if (logicOrder [4].currentType != WordTypes.indentOperator)
				ErrorMessage.sendErrorMessage (lineNumber, "Saknas ett \":\" i slutet av din for loop");
			
			
			return true;
		}
		
		private static bool whileCheck(Logic[] logicOrder, int lineNumber){
			if (logicOrder.Length < 3)
				ErrorMessage.sendErrorMessage (lineNumber, "Okänt format i din While loop");
			
			if (logicOrder [logicOrder.Length -1].currentType != WordTypes.indentOperator)
				ErrorMessage.sendErrorMessage (lineNumber, "Saknas ett \":\" i slutet av din While loop");
			
			
			Logic[] statementLogic = new Logic[logicOrder.Length - 2];
			for (int i = 1; i < logicOrder.Length - 1; i++)
				statementLogic [i - 1] = logicOrder [i];
			
			
			if(!PossibleStatement.validStatement(statementLogic, lineNumber))
				ErrorMessage.sendErrorMessage (lineNumber, "Okänt format i din While loops expression");
			
			
			return true;
		}
		
		
		private static bool defCheck(Logic[] logicOrder, int lineNumber){
			if (logicOrder.Length != 3)
				ErrorMessage.sendErrorMessage (lineNumber, "Okänt format i din funktions definiering");
			
			if (logicOrder [logicOrder.Length -1].currentType != WordTypes.indentOperator)
				ErrorMessage.sendErrorMessage (lineNumber, "Saknas ett \":\" i slutet av din funktions definiering");
			
			if (logicOrder [1].currentType != WordTypes.functionCall)
				ErrorMessage.sendErrorMessage (lineNumber, "Funktionsnamnet går av någon anledning inte att använda");
			
			return true;
		}
	}

}