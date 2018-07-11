using System;
using ErrorHandler;
using Runtime;

namespace Compiler
{
	public class ValidSumCheck{


		public static Variable checkIfValidSum(Logic[] PrelogicOrder, int lineNumber){
			Logic[] logicOrder = (Logic[])PrelogicOrder.Clone ();
			UnpackPackages (logicOrder, lineNumber);

			logicOrder = BoolCompressExpressions.compressExpression (logicOrder, lineNumber, null, false);
			logicOrder = BoolExpressionParser.compressAndOrStatements (logicOrder, lineNumber, null);

			if (logicOrder.Length == 0)
				return new Variable("No var");

			expectType theExpectedType = new expectType ();

			for(int i = 0; i < logicOrder.Length; i++){
				
				if(logicOrder[i].currentType == WordTypes.booleanExpression)
					setNewExpectVariable (theExpectedType, VariableTypes.boolean, lineNumber, logicOrder);
				
				else if (logicOrder [i].currentType == WordTypes.variable)
					logicOrder [i] = new Variable (logicOrder [i].word);
				else if (logicOrder [i].currentType == WordTypes.mathOperator) {

				} 
				else if (logicOrder [i].currentType == WordTypes.number)
					setNewExpectVariable (theExpectedType, VariableTypes.number, lineNumber, logicOrder);
				else if (logicOrder [i].currentType == WordTypes.textString)
					setNewExpectVariable (theExpectedType, VariableTypes.textString, lineNumber, logicOrder);
				else if (logicOrder [i].currentType == WordTypes.booleanValue)
					setNewExpectVariable (theExpectedType, VariableTypes.boolean, lineNumber, logicOrder);
			}

			if (theExpectedType.currentType == VariableTypes.unknown)
				return new Variable ();

			return theExpectedType.returnExpectedVariable();
		}

		private static void UnpackPackages(Logic[] PrelogicOrder, int lineNumber){
			for(int i = 0; i < PrelogicOrder.Length; i++)
				if (PrelogicOrder[i].currentType == WordTypes.package)
					PrelogicOrder[i] = checkIfValidSum ((PrelogicOrder[i] as Package).getLatestOrder(), lineNumber);
		}


		#region Expect type
		static void setNewExpectVariable(expectType expect, VariableTypes newType, int lineNumber, Logic[] logicOrder){
			if (expect.currentType == VariableTypes.unknown)
				return;

			if (expect.currentType == VariableTypes.unsigned)
				expect.currentType = newType;
			else if (expect.currentType != newType)
			{
				var firstSumType = SumParser.TypeToString(expect.currentType);
				var secondSumType = SumParser.TypeToString(newType);

				string errorMessage;

				if (firstSumType == "" || secondSumType == "")
					errorMessage = "Misslyckades med att para ihop " + expect.currentType + " med " + newType;
				else
					errorMessage = "Kan inte para ihop " + firstSumType + " med " + secondSumType + ".";

				ErrorMessage.sendErrorMessage (lineNumber, errorMessage);
			}

			if (expect.currentType == VariableTypes.boolean)
				BooleanSumParser.validBoolSum (logicOrder, lineNumber);
		}


		private class expectType
		{
			public VariableTypes currentType;
			public expectType(){
				currentType = VariableTypes.unsigned;
			}

			public Variable returnExpectedVariable(){
				if (currentType == VariableTypes.boolean)
					return new Variable ("Type Variable", false);
				if (currentType == VariableTypes.number)
					return new Variable ("Type Variable", 0);
				if (currentType == VariableTypes.textString)
					return new Variable ("Type Variable", "");

				return new Variable ("Type Variable");
			}
		}
		#endregion

	}

}

