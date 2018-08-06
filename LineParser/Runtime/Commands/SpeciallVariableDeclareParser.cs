using System;
using ErrorHandler;
using Compiler;

namespace Runtime
{
	public class SpeciallVariableDeclareParser
	{

		internal static Variable speciallVariableDeclare(Logic[] cloneLogicOrder, int lineNuber, Variable afterEqualSign, Scope currentScope){
			string variableName = cloneLogicOrder [0].word;

			if (cloneLogicOrder [1].currentType != WordTypes.mathOperator)
				ErrorHandler.ErrorMessage.sendErrorMessage (lineNuber, "Extra operator måste vara en matteoperator");

			int varPos = currentScope.scopeVariables.containsVariable (cloneLogicOrder [0].word);
			if(varPos < 0)
				ErrorHandler.ErrorMessage.sendErrorMessage (lineNuber, "För att kunna använda extra operatorer måste variabeln med namn \"" + cloneLogicOrder[0].word + "\" redan vara deklarerad ");
			cloneLogicOrder [0] = currentScope.scopeVariables.variableList [varPos];

			if ((cloneLogicOrder [0] as Variable).variableType == VariableTypes.number)
				return handleNumberVariable (cloneLogicOrder, lineNuber, afterEqualSign, currentScope);

			if ((cloneLogicOrder [0] as Variable).variableType == VariableTypes.textString)
				return TextSpeciallVariableDeclare.handleTextVariable (cloneLogicOrder, lineNuber, afterEqualSign, currentScope, variableName);


			ErrorHandler.ErrorMessage.sendErrorMessage (lineNuber, "Endast variabler med Nummer eller Text kan använda extra operatorer");

			return new Variable ("Temp");
		}


		private static Variable handleNumberVariable(Logic[] cloneLogicOrder, int lineNumber, Variable tempSum, Scope currentScope){
			Logic[] calcSum = new Logic[3];
			calcSum [0] = cloneLogicOrder [0];
			calcSum [1] = cloneLogicOrder [1];
			calcSum [2] = new NumberValue (tempSum.getNumber().ToString());
			Variable tempVar = SumParser.parseIntoSum (calcSum, lineNumber, currentScope);

			tempVar.name = (calcSum [0] as Variable).name;

			return tempVar;
		}

		[System.Obsolete("This function should not be in use", true)]
		private static Variable getAfterEqualSignSum(Logic[] cloneLogicOrder, int lineNuber, Scope currentScope){
			Logic[] afterEqualSign = InternalParseFunctions.getSubArray(cloneLogicOrder, 3, cloneLogicOrder.Length-1, lineNuber);

			//Debugger.printLogicOrder (afterEqualSign, "Checking how dis turns out");
			return new Variable ();
		}

	}
}

