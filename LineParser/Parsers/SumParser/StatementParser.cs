using System;
using ErrorHandler;
using Runtime;

namespace Compiler
{
	public class StatementParser{

		public static bool parseAndCheckStatement(Logic[] logicOrder, int lineNumber, Scope currentScope){
			Variable sum = SumParser.parseIntoSum (logicOrder, lineNumber, currentScope);

			if (sum.variableType == VariableTypes.number){
				if (sum.getNumber() == 0)
					return false;
				else
					return true;
			}

			if (sum.variableType == VariableTypes.textString) {
				if (sum.getString () == "")
					return false;
				else
					return true;
			}

			if (sum.variableType == VariableTypes.None)
				return false;

			if (sum.variableType != VariableTypes.boolean)
				ErrorMessage.sendErrorMessage (lineNumber, ErrorType.IfStatements, IfErrorType.expressionNotCorrectType.ToString(), null);

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


			if (operatorAmount > 2 && operatorHigh - operatorLow != 1) {
				ErrorMessage.sendErrorMessage (lineNumber, "I detta moment är det inte tillåtet att gör flera jämförelser direkt efter varandra. Använd \"and\" och \"or\" istället.");
			}
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

			if (operatorType == ComparisonType.unknown) {
				if (operators[0].currentType == WordTypes.equalSign)
					ErrorMessage.sendErrorMessage(lineNumber, "Fel vid tilldelning. Kom ihåg att använda == om du vill jämföra om två värden är lika.");
				else
					ErrorMessage.sendErrorMessage(lineNumber, "Operatorerna går inte att tyda");
			} 

			if (firstSum.variableType == VariableTypes.unknown || secondSum.variableType == VariableTypes.unknown) 
				ErrorMessage.sendErrorMessage (lineNumber, "Korrupt värde");

			if (firstSum.variableType != secondSum.variableType)
			{
				var firstSumType = SumParser.TypeToString(firstSum.variableType);
				var secondSumType = SumParser.TypeToString(secondSum.variableType);

				string errorMessage;

				if (firstSumType == "" || secondSumType == "")
					errorMessage = "Misslyckades med att jämföra " + firstSum.variableType + " med " + secondSum.variableType;
				else
					errorMessage = "Kan inte jämföra " + firstSumType + " med " + secondSumType + ".";

				ErrorMessage.sendErrorMessage(lineNumber, errorMessage);
			}

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

