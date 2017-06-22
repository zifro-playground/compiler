using System;
using ErrorHandler;
using Runtime;

namespace Compiler
{
	public class StatementParser{

		public static bool parseAndCheckStatement(Logic[] logicOrder, int lineNumber, Scope currentScope){
			Variable sum = SumParser.parseIntoSum (logicOrder, lineNumber, currentScope);

			if (sum.variableType != VariableTypes.boolean)
				ErrorMessage.sendErrorMessage (lineNumber, "Ett expression kan bara vara Sant eller Falskt");

			return sum.getBool ();
		}


		public static bool parseStatement(Logic[] logicOrder, int lineNumber, Scope currentScope){
			BoolAlgebraWord[] algebra = BoolAlgebraParser.parseIntoBoolAlgebra (logicOrder, lineNumber, currentScope);
			return BoolAlgebraParser.parseBoolAlgebraStatement (algebra, lineNumber);
		}

		public static bool parseExpression(Logic[] logicOrder, int lineNumber, Scope currentScope){
			#region findOperators
			int operatorLow = -1;
			int operatorHigh = -1;
			int operatorAmount = 0;
			for (int i = 0; i < logicOrder.Length; i++)
				if (isStatementOperator (logicOrder [i])) {
					if (operatorLow == -1)
						operatorLow = i;
					else
						operatorHigh = i;
					operatorAmount++;
				}

			#endregion
			if(operatorLow == 0)
				ErrorMessage.sendErrorMessage (lineNumber, "FparsEtt expressionn kan inte starta med en operator");
			if (operatorAmount > 2) 
				ErrorMessage.sendErrorMessage (lineNumber, "Operatorerna i ditt expressions går inte att tyda");
			if (operatorAmount == 0)
				return handleOnlyValueStatement (logicOrder, lineNumber, currentScope);


			if (operatorAmount == 2) {
				if (operatorHigh - operatorLow != 1) 
					ErrorMessage.sendErrorMessage (lineNumber, "Operatorerna måste komma direkt efter varandra");
			}
			else
				operatorHigh = operatorLow;




			Logic[] leftSide = new Logic[operatorLow];
			Logic[] rightSide = new Logic[logicOrder.Length - 1 - operatorHigh];
			Logic[] operators = new Logic[operatorAmount];
			setSidesOfStatement (logicOrder, leftSide, rightSide, operators, operatorLow, operatorHigh);


			Variable firstSum = SumParser.parseIntoSum (leftSide, lineNumber, currentScope);
			Variable secondSum = SumParser.parseIntoSum (rightSide, lineNumber, currentScope);
			ComparisonType operatorType = ComparisonOperatorParser.parseOperators (operators);

			if (operatorType == ComparisonType.unknown) 
				ErrorMessage.sendErrorMessage (lineNumber, "Operatorerna går inte att tyda");

			if (firstSum.variableType == VariableTypes.unknown || secondSum.variableType == VariableTypes.unknown) 
				ErrorMessage.sendErrorMessage (lineNumber, "Korrupt värde");

			if (firstSum.variableType != secondSum.variableType)
				ErrorMessage.sendErrorMessage (lineNumber, "Misslyckades med att jämföra " + firstSum.variableType + " med " + secondSum.variableType);

			if (ComparisonOperatorParser.checkSumsToOperator(firstSum.variableType, operatorType,lineNumber)) {
				bool resultOfComparison = ComparisonOperatorParser.makeComparison (firstSum, secondSum, operatorType, lineNumber);
				return resultOfComparison;
			} 

			ErrorMessage.sendErrorMessage (lineNumber, "Korrupt expression!");
			return false;
		}


		public static bool isStatementOperator(Logic currentLogic){
			if (currentLogic.currentType == WordTypes.equalSign)
				return true;

			if (currentLogic.currentType == WordTypes.lessThenSign || currentLogic.currentType == WordTypes.greaterThenSign)
				return true;

			if (currentLogic.currentType == WordTypes.xorOperator)
				return true;

			return false;
		}


		static void setSidesOfStatement(Logic[] logicOrder, Logic[] leftSide, Logic[] rightSide, Logic[] operators, int operatorLow, int operatorHigh){
			for (int i = 0; i < leftSide.Length; i++)
				leftSide [i] = logicOrder [i];

			for (int i = operatorHigh+1; i < logicOrder.Length; i++)
				rightSide [i - (operatorHigh+1)] = logicOrder [i];

			for (int i = operatorLow; i <= operatorHigh; i++)
				operators [i - operatorLow] = logicOrder [i];
		}

		static bool handleOnlyValueStatement(Logic[] logicOrder, int lineNumber, Scope currentScope){

			Variable sum = SumParser.parseIntoSum (logicOrder, lineNumber, currentScope);

			if (sum.variableType != VariableTypes.boolean)
				ErrorMessage.sendErrorMessage (lineNumber, "När du skriver in endast ett värde, måste det vara Sant eller Falskt");

			return sum.getBool ();
		}
	}

}

