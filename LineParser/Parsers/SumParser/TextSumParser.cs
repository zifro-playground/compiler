using System;
using ErrorHandler;

namespace Compiler
{

	internal class TextSumParser{

		public static Variable validTextSum(Logic[] logicOrder, int lineNumber){

			if (logicOrder.Length < 1) 
				return new Variable("Unknown Variable");


			string returnValue = "";
			bool corrupt = false;

			for (int i = 0; i < logicOrder.Length; i++) {

				if (i % 2 == 1) {
					if (!isPlusSign (logicOrder [i], lineNumber)) {
						ErrorMessage.sendErrorMessage (lineNumber, ErrorType.Text, 0, null);	
						corrupt = true;
						break;
					}
				}
				else {
					if (isStringType (logicOrder [i], lineNumber))
						returnValue += getStringValue (logicOrder [i]);

					else {
						// Can never reach this?
						corrupt = true;
						ErrorMessage.sendErrorMessage (lineNumber, ErrorType.Text, 1, null);	
					}
				}
			}


			//Check so it ends with a String
			if(isStringType( logicOrder[logicOrder.Length -1], lineNumber) == false){
				corrupt = true;
				ErrorMessage.sendErrorMessage (lineNumber, ErrorType.Text, 2, null);
			}


			if (corrupt) {
				ErrorMessage.sendErrorMessage (lineNumber,  ErrorType.Text, 3, null);
				return new Variable ("Unknown Variable");
			} 
			else 
				return new Variable ("TextVar", returnValue, true);
		}



		static bool isPlusSign(Logic currentLogic, int lineNumber){
			if (currentLogic.currentType == WordTypes.mathOperator) 
			if ((currentLogic as MathOperator).word == "+")
				return true;

			return false;
		}

		static bool isStringType(Logic currentLogic, int lineNumber){
			if (currentLogic.currentType == WordTypes.textString)
				return true;

			if (currentLogic.currentType == WordTypes.variable) {

				if ((currentLogic as Variable).variableType == VariableTypes.textString)
					return true;
			}


			return false;
		}

		static string getStringValue(Logic currentLogic){
			if (currentLogic.currentType == WordTypes.textString)
				return (currentLogic as TextValue).value;

			if (currentLogic.currentType == WordTypes.variable) 
			if ((currentLogic as Variable).variableType == VariableTypes.textString)
				return (currentLogic as Variable).getString();

			return "";
		}

	}

}

