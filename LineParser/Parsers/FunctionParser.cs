using System;
using System.Collections.Generic;

namespace Compiler
{
	public class FunctionParser{

		public static Compiler.Logic parseIntoFunctionCall(string word, int lineNumber, Compiler.Scope currentScope){

			string funcName = getFunctionName (word, lineNumber);
			string funcPara = getFunctionInParameter (word, lineNumber);

			return new FunctionCall (word, funcName, funcPara, null);
		}

		public static void linkFunctionCall(FunctionCall theFunc, int lineNumber, Compiler.Scope currentScope){
			Compiler.Function searchedFunc = currentScope.scopeFunctions.getSavedFunction (theFunc.name, lineNumber);
			if (searchedFunc != null){

				Compiler.Variable[] inputVariables = validParameters (PackageUnWrapper.removeSurrondingParanteser (theFunc.parameter), searchedFunc, lineNumber, currentScope);
				theFunc.targetFunc = searchedFunc;
			}

		}


		public static string getFunctionName(string functionWord, int lineNumber){

			for (int i = 0; i < functionWord.Length; i++) 
				if (functionWord [i] == '(') 
					return functionWord.Substring (0, i);

			ErrorMessage.sendErrorMessage (lineNumber, "Compiler.Function Call is missing a (");
			return null;
		}


		public static string getFunctionInParameter(string functionWord, int lineNumber){

			for (int i = 0; i < functionWord.Length; i++)

				if (functionWord [i] == '(') 
					return functionWord.Substring(i, (functionWord.Length)-i);

			ErrorMessage.sendErrorMessage (lineNumber, "Compiler.Function Call is missing a (");
			return null;
		}


		public static bool hasReturnValue(string word, int lineNumber){
			Compiler.Function searchedFunc = Camera.main.GetComponent<HelloCompiler>().funcs.getSavedFunction (FunctionParser.getFunctionName (word, lineNumber), lineNumber);
			if (searchedFunc != null) 
				return searchedFunc.hasReturnVariable;


			return false;
		}



		static Compiler.Variable[] validParameters(string trimmedPara, Compiler.Function calledFunction, int lineNumber, Compiler.Scope currentScope){


			string[] words = Compiler.WordParser.parseWords (trimmedPara);

			if (words.Length != 0) {
				Compiler.Logic[] logicOrder = WordLogic.determineLogicFromWords (words, lineNumber, currentScope);

				List<List<Compiler.Logic>> packedLogics = convertIntoParameterLogic (words, logicOrder, lineNumber);

				if (packedLogics != null) {
					Compiler.Variable[] inputVariables = new Compiler.Variable[packedLogics.Count];

					for (int i = 0; i < packedLogics.Count; i++)
						inputVariables [i] = ValidSumCheck.checkIfValidSum (packedLogics [i].ToArray (), lineNumber);


					foreach (Compiler.Variable v in inputVariables)
						if (v.variableType == Compiler.VariableTypes.unkown)
							ErrorMessage.sendErrorMessage (lineNumber, "one or several of the input parameters to function: " + calledFunction.name + " are corrupt!");


					if (calledFunction.inputParameters.Contains (inputVariables.Length))
						return inputVariables;
				}

			} 

			if (calledFunction.inputParameters.Contains (0))
				return new Compiler.Variable[0];

			ErrorMessage.sendErrorMessage (lineNumber, "Amount of parameters does not match Expected!");
			return new Compiler.Variable[0];
		}


		public static Compiler.Variable[] getValueOfParameters(string trimmedPara, Compiler.Function calledFunction, int lineNumber,Compiler.Scope currentScope){


			string[] words = Compiler.WordParser.parseWords (trimmedPara);

			if (words.Length != 0) {
				Compiler.Logic[] logicOrder = WordLogic.determineLogicFromWords (words, lineNumber, currentScope);

				List<List<Compiler.Logic>> packedLogics = convertIntoParameterLogic (words, logicOrder, lineNumber);

				if (packedLogics != null) {
					Compiler.Variable[] inputVariables = new Compiler.Variable[packedLogics.Count];

					for (int i = 0; i < packedLogics.Count; i++)
						inputVariables [i] = SumParser.parseIntoSum (packedLogics [i].ToArray (), lineNumber, currentScope);


					foreach (Compiler.Variable v in inputVariables)
						if (v.variableType == Compiler.VariableTypes.unkown)
							ErrorMessage.sendErrorMessage (lineNumber, "one or several of the input parameters to function: " + calledFunction.name + " are corrupt!");


					if (calledFunction.inputParameters.Contains (inputVariables.Length))
						return inputVariables;
				}

			} 

			return new Compiler.Variable[0];
		}



		static List<List<Compiler.Logic>> convertIntoParameterLogic(string[] words, Compiler.Logic[] logicOrder, int lineNumber){

			List<List<Compiler.Logic>> packedLogic = new List<List<Compiler.Logic>> ();



			if (logicOrder [0].currentType == Compiler.WordTypes.commaSign || logicOrder [logicOrder.Length-1].currentType == Compiler.WordTypes.commaSign)
				ErrorMessage.sendErrorMessage (lineNumber, "Commas does not match in the parameter");


			int lastComma = -1;
			for (int i = 0; i < logicOrder.Length; i++) {
				if (logicOrder [i].currentType == Compiler.WordTypes.commaSign) {

					if (i == lastComma + 1) {
						ErrorMessage.sendErrorMessage (lineNumber, "Stacked commas in parameter");
						return null;
					} else {

						List<Compiler.Logic> tempList = new List<Compiler.Logic> ();
						for (int j = lastComma + 1; j < i; j++)
							tempList.Add (logicOrder [j]);					

						packedLogic.Add (tempList);
						lastComma = i;
					}

				}
			}

			// Fixes the last package should be written as a function for beautiful code later
			List<Compiler.Logic> tempList2 = new List<Compiler.Logic> ();
			for (int j = lastComma+1; j < logicOrder.Length; j++) 
				tempList2.Add (logicOrder [j]);			

			packedLogic.Add (tempList2);


			return packedLogic;
		}




	}

}

