using System;
using Runtime;

namespace Compiler
{
	public class SumParser{
		

		public static Variable parseIntoSum(Logic[] logicOrderInput, int lineNumber, Scope currentScope){
			Logic[] logicOrder = (Logic[])logicOrderInput.Clone ();
			UnpackPackages (logicOrder, lineNumber, currentScope);

			logicOrder = BoolCompressExpressions.compressExpression (logicOrder, lineNumber, currentScope, true);
			logicOrder = BoolExpressionParser.compressAndOrStatements (logicOrder, lineNumber, currentScope);

			if (logicOrder.Length == 0)
				return new Variable("CalcVar");

			expectType theExpectedType = new expectType ();
			calcSumType (logicOrder, theExpectedType, lineNumber, currentScope);
			return getCalcSum (theExpectedType, logicOrder, lineNumber);
		}
			
		private static void UnpackPackages(Logic[] PrelogicOrder, int lineNumber, Scope currentScope){
			for (int i = 0; i < PrelogicOrder.Length; i++)
				if (PrelogicOrder [i].currentType == WordTypes.package) {
					PrelogicOrder[i] = parseIntoSum ((PrelogicOrder[i] as Package).getLatestOrder(), lineNumber, currentScope);
				}
		}


		private static void calcSumType(Logic[] logicOrder, expectType theExpectedType, int lineNumber, Scope currentScope){
			for(int i = 0; i < logicOrder.Length; i++){
				switch (logicOrder [i].currentType) {

				#region values
				case WordTypes.textString:
					setNewExpectVariable (theExpectedType, VariableTypes.textString, lineNumber);
					break;
				case WordTypes.number:
					setNewExpectVariable (theExpectedType, VariableTypes.number, lineNumber);
					break;
				case WordTypes.booleanValue:
					setNewExpectVariable (theExpectedType, VariableTypes.boolean, lineNumber);
					break;
				#endregion

				case WordTypes.booleanExpression: //Är det möjligt att få det till ett booleanExpression?
					Print.print("Om du har lyckats komma hit så vill jag se koden. Bool expressions.");
					logicOrder [i] = (logicOrder [i] as BooleanExpression).parseExpression ();
					setNewExpectVariable (theExpectedType, VariableTypes.boolean, lineNumber);
					break;
					
				case WordTypes.variable:
					handleVariable(logicOrder, theExpectedType, lineNumber, currentScope, i);
					break;

				case WordTypes.functionCall:
					handleFunctionCall(logicOrder, theExpectedType, lineNumber, currentScope, i);
					break;
				}
			}
		}

		private static void handleFunctionCall(Logic[] logicOrder, expectType theExpectedType, int lineNumber, Scope currentScope, int i){
			FunctionParser.linkFunctionCall ((logicOrder [i] as FunctionCall), lineNumber, currentScope);

			if ((logicOrder [i] as FunctionCall).targetFunc.isUserFunction) {
				currentScope.getCurrentLine ().insertReturnExpect (logicOrder [i]);
				(logicOrder [i] as FunctionCall).runFunction (currentScope);

				throw new FunctionCallException ();
			} else {
				if ((logicOrder [i] as FunctionCall).targetFunc.pauseWalker)
					CodeWalker.pauseWalker ();
				
				Variable returnVariable = (logicOrder [i] as FunctionCall).runFunction (currentScope);
				logicOrder [i] = returnVariable;

				if(returnVariable.variableType == VariableTypes.boolean)
					setNewExpectVariable (theExpectedType, VariableTypes.boolean, lineNumber);
				else if(returnVariable.variableType == VariableTypes.number)
					setNewExpectVariable (theExpectedType, VariableTypes.number, lineNumber);
				else if(returnVariable.variableType == VariableTypes.textString)
					setNewExpectVariable (theExpectedType, VariableTypes.textString, lineNumber);
			}
		}

		private static void handleVariable(Logic[] logicOrder, expectType theExpectedType, int lineNumber, Scope currentScope, int i){
			int varPos = currentScope.scopeVariables.containsVariable ((logicOrder [i] as Variable).name);
			if (varPos >= 0) {
				Variable foundVar = currentScope.scopeVariables.variableList [varPos];
				setNewExpectVariable (theExpectedType, foundVar.variableType, lineNumber);
				logicOrder [i] = foundVar;
			}
			else {
				if ((logicOrder [i] as Variable).isCalcVar) 
					setNewExpectVariable (theExpectedType, (logicOrder [i] as Variable).variableType, lineNumber);
				else{
					if((logicOrder [i] as Variable).name.Length > 10)
						ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, "Användning av icke deklarerad variabel!");

					LevenshteinDist.checkForClosesVariable (logicOrder [i].word, lineNumber, currentScope);
					ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, "Variabeln: " + (logicOrder [i] as Variable).name + " är inte deklarerad");
				}
			}
		}



		private static Variable getCalcSum(expectType theExpectedType, Logic[] logicOrder, int lineNumber){
			if (theExpectedType.currentType == VariableTypes.boolean) {
				Variable sumVar = BooleanSumParser.validBoolSum (logicOrder, lineNumber);
				if (sumVar.variableType == VariableTypes.boolean)
					return sumVar;
			}

			if (theExpectedType.currentType == VariableTypes.textString) {
				Variable sumVar = TextSumParser.validTextSum (logicOrder, lineNumber);
				if (sumVar.variableType == VariableTypes.textString)
					return sumVar;
			}

			if (theExpectedType.currentType == VariableTypes.number) {
				Variable sumVar = NumberSumParser.validNumberSum (logicOrder, lineNumber);
				if (sumVar.variableType == VariableTypes.number)
					return sumVar;
			}
				
			return new Variable ("CalcVar");
		}


		#region Expected type
		private class expectType
		{
			public VariableTypes currentType;
			public expectType(){
				currentType = VariableTypes.unsigned;
			}
		}
			
		private static void setNewExpectVariable(expectType expect, VariableTypes newType, int lineNumber){
			if (expect.currentType == VariableTypes.unsigned)
				expect.currentType = newType;
			else if (expect.currentType != newType) {
				ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, "Misslyckades att tolka " + expect.currentType + " med " + newType);
				expect.currentType  = VariableTypes.unknown;
			}
		}
		#endregion
	}

}

