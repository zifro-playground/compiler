using System;
using ErrorHandler;
using System.Collections.Generic;
using Runtime;

namespace Compiler
{
	public class FunctionParser{

		public static Logic parseIntoFunctionCall(string word, int lineNumber, Scope currentScope){
			string funcName = getFunctionName (word, lineNumber);
			string funcPara = getFunctionInParameter (word, lineNumber);

			return new FunctionCall (word, funcName, PackageUnWrapper.removeSurrondingParanteser(funcPara), null);
		}


		public static void linkFunctionCall(FunctionCall theFuncCall, int lineNumber, Scope currentScope){

			Function searchedFunc = currentScope.scopeFunctions.getSavedFunction (theFuncCall.name, lineNumber);
			if (searchedFunc != null){
				validParameters (theFuncCall.parameter, searchedFunc, lineNumber, currentScope);

				if (theFuncCall.returnCalculations != null)
					theFuncCall.inputVariables = parseInputVariables (theFuncCall.returnCalculations, lineNumber, theFuncCall.targetFunc, currentScope);
				else
					theFuncCall.inputVariables = getValueOfParameters (theFuncCall.parameter, theFuncCall.targetFunc, lineNumber, currentScope, theFuncCall);

				for (int i = 0; i < theFuncCall.inputVariables.Length; i++) {
					Print.print (theFuncCall.inputVariables [i].name + "  : " +  theFuncCall.inputVariables [i].getNumber ().ToString ());
				}


				theFuncCall.lineNumber = lineNumber;
				theFuncCall.targetFunc = searchedFunc;
				theFuncCall.name = searchedFunc.name;
			}

		}


		public static string getFunctionName(string functionWord, int lineNumber){
			for (int i = 0; i < functionWord.Length; i++) 
				if (functionWord [i] == '(') 
					return functionWord.Substring (0, i);

			ErrorMessage.sendErrorMessage (lineNumber, "Funktions anrop saknar en \"(\"");
			return null;
		}


		public static string getFunctionInParameter(string functionWord, int lineNumber){
			for (int i = 0; i < functionWord.Length; i++)

				if (functionWord [i] == '(') 
					return functionWord.Substring(i, (functionWord.Length)-i);

			ErrorMessage.sendErrorMessage (lineNumber, "Funktions anrop saknar en \"(\"");
			return null;
		}


		public static bool validParameters(string trimmedPara, Function calledFunction, int lineNumber, Scope currentScope){
			int paraAmount = getParameterAmount (trimmedPara, lineNumber, currentScope);
			if (calledFunction.inputParameterAmount.Contains (paraAmount) == false)
				ErrorMessage.sendErrorMessage (lineNumber, "Antal parametrar matchar inte funktions definietionen");

			return true;
		}


		public static Variable[] getValueOfParameters(string trimmedPara, Function calledFunction, int lineNumber, Scope currentScope, FunctionCall theFuncCall){
			string[] words = WordParser.parseWords (trimmedPara);

			if (words.Length != 0) {
				Logic[] logicOrder = WordsToLogicParser.determineLogicFromWords (words, lineNumber, currentScope);
				List<Logic[]> packedLogics = convertIntoParameterLogic (words, logicOrder, lineNumber);
				theFuncCall.setReturnCalculations (packedLogics);

				if (packedLogics != null)
					return parseInputVariables(packedLogics, lineNumber, calledFunction, currentScope);
			} 

			return new Variable[0];
		}


		private static Variable[] parseInputVariables(List<Logic[]> packedLogics, int lineNumber, Function calledFunction, Scope currentScope){
			Variable[] inputVariables = new Variable[packedLogics.Count];

			for (int i = 0; i < packedLogics.Count; i++)
				inputVariables [i] = SumParser.parseIntoSum (packedLogics [i], lineNumber, currentScope);
			
			foreach (Variable v in inputVariables)
				if (v.variableType == VariableTypes.unknown)
					ErrorMessage.sendErrorMessage (lineNumber, "En eller flera av inparametrarna till: " + calledFunction.name + " är korrupta");

			return inputVariables;
		}


		public static int getParameterAmount(string trimmedPara, int lineNumber, Scope currentScope){
			string[] words = WordParser.parseWords (trimmedPara);
			if (words.Length == 0)
				return 0;	

			Logic[] logicOrder = WordsToLogicParser.determineLogicFromWords (words, lineNumber, currentScope);
			List<Logic[]> packedLogics = convertIntoParameterLogic (words, logicOrder, lineNumber);

			return packedLogics.Count;
		}

		public static string[] getParameterNames(string trimmedPara, int lineNumber, Scope currentScope){
			string[] words = WordParser.parseWords (trimmedPara);
			if (words.Length == 0)
				return new string[0];	

			Logic[] logicOrder = WordsToLogicParser.determineLogicFromWords (words, lineNumber, currentScope);
			List<Logic[]> packedLogics = convertIntoParameterLogic (words, logicOrder, lineNumber);
					

			List<string> returnWords = new List<string>();
			foreach (Logic[] l in packedLogics) {
				if (l.Length != 1)
					ErrorMessage.sendErrorMessage (lineNumber, "Du tycks ha glömt ett \",\"");

				returnWords.Add (l [0].word);
			}
					

			return returnWords.ToArray();
		}


		static List<Logic[]> convertIntoParameterLogic(string[] words, Logic[] logicOrder, int lineNumber){
			List<Logic[]> packedLogic = new List<Logic[]> ();

			if (logicOrder [0].currentType == WordTypes.commaSign || logicOrder [logicOrder.Length-1].currentType == WordTypes.commaSign)
				ErrorMessage.sendErrorMessage (lineNumber, "Dinna komma tecke matchar inte");


			int lastComma = -1;
			for (int i = 0; i < logicOrder.Length; i++) {
				if (logicOrder [i].currentType == WordTypes.commaSign) {

					if (i == lastComma + 1) {
						ErrorMessage.sendErrorMessage (lineNumber, "Två komma tecken kan inte komma direkt efter varandra");
						return null;
					} else {

						List<Logic> tempList = new List<Logic> ();
						for (int j = lastComma + 1; j < i; j++)
							tempList.Add (logicOrder [j]);					

						packedLogic.Add (tempList.ToArray());
						lastComma = i;
					}

				}
			}

			// Fix the last package should be written as a function for beautiful code later
			List<Logic> tempList2 = new List<Logic> ();
			for (int j = lastComma+1; j < logicOrder.Length; j++) 
				tempList2.Add (logicOrder [j]);			

			packedLogic.Add (tempList2.ToArray());


			return packedLogic;
		}
	}

}
	