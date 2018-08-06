using System;
using System.Linq;
using ErrorHandler;
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

				// booleanExpressions are for example a expression used in an if-statement
				case WordTypes.booleanExpression:
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

				if ((logicOrder[i] as FunctionCall).targetFunc.name == "input"){
					CodeWalker.linkSubmitInput.Invoke(SubmitInput, currentScope);

					Variable returnVariable = (logicOrder[i] as FunctionCall).runFunction(currentScope);
					logicOrder[i] = returnVariable;

					if (returnVariable.variableType == VariableTypes.boolean)
						setNewExpectVariable(theExpectedType, VariableTypes.boolean, lineNumber);
					else if (returnVariable.variableType == VariableTypes.number)
						setNewExpectVariable(theExpectedType, VariableTypes.number, lineNumber);
					else if (returnVariable.variableType == VariableTypes.textString)
						setNewExpectVariable(theExpectedType, VariableTypes.textString, lineNumber);

					CodeWalker.isWaitingForUserInput = true;
				}
				else {
					Variable returnVariable = (logicOrder[i] as FunctionCall).runFunction(currentScope);
					logicOrder[i] = returnVariable;

					if (returnVariable.variableType == VariableTypes.boolean)
						setNewExpectVariable(theExpectedType, VariableTypes.boolean, lineNumber);
					else if (returnVariable.variableType == VariableTypes.number)
						setNewExpectVariable(theExpectedType, VariableTypes.number, lineNumber);
					else if (returnVariable.variableType == VariableTypes.textString)
						setNewExpectVariable(theExpectedType, VariableTypes.textString, lineNumber);
				}
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
						ErrorMessage.sendErrorMessage (lineNumber, "Användning av icke deklarerad variabel!");

					LevenshteinDist.checkForClosesVariable (logicOrder [i].word, lineNumber, currentScope);
					ErrorMessage.sendErrorMessage (lineNumber, "Kunde inte hitta variabeln \"" + (logicOrder [i] as Variable).name + "\" i minnet.");
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

		public static void SubmitInput(string inputFromUser, Scope currentScope){
			if (!CodeWalker.isWaitingForUserInput)
				return;

			CodeWalker.isWaitingForUserInput = false;

			var oldLine = currentScope.getCurrentLine().getFullLine();
			var newLine = ReplaceInputWithValue(oldLine, inputFromUser);

			var lines = SyntaxCheck.parseLines(newLine);
			var words = lines.First().words;
			var logic = WordsToLogicParser.determineLogicFromWords(words, 1, currentScope);

			currentScope.getCurrentLine().words = words;
			currentScope.getCurrentLine().logicOrder = logic;

			CodeWalker.parseLine(currentScope.getCurrentLine());
		}

		private static string ReplaceInputWithValue(string currentLine, string value){
			var currentCharIndex = 0;
			var inputStartIndex = -1;
			var inputEndIndex = -1;
			var startedParanthesis = -1;

			while (currentCharIndex < currentLine.Length){

				if (currentLine[currentCharIndex] == 'i' && currentLine[currentCharIndex + 1] == 'n' && currentLine[currentCharIndex + 2] == 'p' &&
				    currentLine[currentCharIndex + 3] == 'u' && currentLine[currentCharIndex + 4] == 't' && currentLine[currentCharIndex + 5] == '('){

					inputStartIndex = currentCharIndex;
					startedParanthesis = 1;
					currentCharIndex += 6;
				}

				if (currentLine[currentCharIndex] == '(')
					startedParanthesis++;
				else if (currentLine[currentCharIndex] == ')')
					startedParanthesis--;

				if (startedParanthesis == 0)
					inputEndIndex = currentCharIndex + 1;

				currentCharIndex++;
			}

			var subStringLength = inputEndIndex - inputStartIndex;
			var subStringWithInput = currentLine.Substring(inputStartIndex, subStringLength);

			var firstPartSubString = currentLine.Substring(0, inputStartIndex);
			var secondPartSubString = currentLine.Substring(inputEndIndex);

			var newLine = firstPartSubString + "\"" + value + "\"" + secondPartSubString;
			
			return newLine;
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
				var firstSumType = TypeToString(expect.currentType);
				var secondSumType = TypeToString(newType);

				string errorMessage;

				if (firstSumType == "" || secondSumType == "")
					errorMessage = "Misslyckades med att para ihop " + expect.currentType + " med " + newType;
				else
					errorMessage = "Kan inte lägga ihop " + firstSumType + " med " + secondSumType + ".";

				ErrorMessage.sendErrorMessage (lineNumber, errorMessage);
				expect.currentType  = VariableTypes.unknown;
			}
		}
		#endregion

		public static string TypeToString(VariableTypes variableType)
		{
			if (variableType == VariableTypes.number)
				return "ett tal";
			if (variableType == VariableTypes.textString)
				return "en sträng";
			if (variableType == VariableTypes.boolean)
				return "ett boolskt värde";
			if (variableType == VariableTypes.None)
				return "None";
			if (variableType == VariableTypes.unknown)
				return "ett okänt värde";
			if (variableType == VariableTypes.unsigned)
				return "ett odefinierat värde";
			return "";
		}
	}

}

