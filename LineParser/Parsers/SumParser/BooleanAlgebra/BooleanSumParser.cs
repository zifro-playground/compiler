using System.Collections;
using Runtime;

namespace Compiler{
	
	internal class BooleanSumParser{
		
		public static Variable validBoolSum(Logic[] logicOrder, int lineNumber){
			#region parse out the not operators
			int notNotIndex = 0;
			int notCounter = 0;
			for (int i = 0; i < logicOrder.Length; i++)
				if (logicOrder [i].currentType == WordTypes.notOperator)
					notCounter++;
				else {
					notNotIndex = i;
					break;
				}
			
			if (logicOrder.Length - notNotIndex != 1)
				ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, "Korrupt Sant eller Falskt värde");
			#endregion


			if (logicOrder [notNotIndex].currentType == WordTypes.variable) {
				if (notCounter % 2 == 0) 
					return new Variable("BoolVar", (logicOrder [notNotIndex] as Variable).getBool(), true);
				else 
					return new Variable("BoolVar", !(logicOrder [notNotIndex] as Variable).getBool(), true);
			}
			
			if (logicOrder [notNotIndex].currentType == WordTypes.booleanValue) {
				if (notCounter % 2 == 0) 
					return new Variable("BoolVar", (logicOrder [notNotIndex] as BooleanValue).value, true);
				else 
					return new Variable("BoolVar", !(logicOrder [notNotIndex] as BooleanValue).value, true);
			}

			if (logicOrder [notNotIndex].currentType == WordTypes.booleanExpression)
				return new Variable ("Possible Bool Sum", false, true);
			
			ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, "Något gick fel med Sant eller Falsk parsing");
			return new Variable ();
		}
		
	}
	

}