using System;
using ErrorHandler;
using Runtime;

namespace Compiler
{
	public class TextSpeciallVariableDeclare
	{


		internal static Variable handleTextVariable(Logic[] cloneLogicOrder, int lineNumber, Variable afterEqualSign, Scope currentScope, string variableName){
			Variable returnVar = new Variable(variableName, "");
			string originalStringValue = (cloneLogicOrder [0] as Variable).getString ();

			if (afterEqualSign.variableType == VariableTypes.number)
				return handleNumberValue (cloneLogicOrder, returnVar, afterEqualSign, originalStringValue, lineNumber);
		
			else if (afterEqualSign.variableType == VariableTypes.textString)
				return handeTextValue (cloneLogicOrder, returnVar, afterEqualSign, originalStringValue, lineNumber);
			

		//	ErrorMessage.sendErrorMessage (lineNumber, "Ogiltig använding av extra operatorer");
			return returnVar;
		}


		private static Variable handeTextValue(Logic[] cloneLogicOrder, Variable returnVar, Variable afterEqualSign, string originalStringValue, int lineNumber){
			if (cloneLogicOrder [1].word != "+")
				ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, "När du använder extra operatorer på en text variabel med en annan text kan du endast använda \"+\"");

			returnVar.setValue (originalStringValue + afterEqualSign.getString());
			return returnVar;
		}


		private static Variable handleNumberValue(Logic[] cloneLogicOrder, Variable returnVar, Variable afterEqualSign, string originalStringValue, int lineNumber){
			if (cloneLogicOrder [1].word != "*")
				ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, "När du använder extra operatorer på en text variabel med en siffra kan du endast använda \"*\"");

			if (afterEqualSign.getNumber() % 1 != 0)
				ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, "Du kan endast multiplicera strängar med heltal");

			if (afterEqualSign.getNumber() <= 0)
				return returnVar;

			for (int i = 0; i < afterEqualSign.getNumber (); i++)
				returnVar.setValue (returnVar.getString () + originalStringValue);

			return returnVar;
		}
	}
}

