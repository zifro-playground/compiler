using ErrorHandler;
using System.Collections.Generic;
using System;
using Compiler;

public class PossibleStatement{

	internal static bool validStatement(Logic[] logicOrder, int lineNumber){
		List<Logic[]> statementParts = new List<Logic[]> ();

		int lastOperatorPos = -1;
		for (int i = 0; i < logicOrder.Length; i++)
			if (logicOrder [i].currentType == WordTypes.andOperator || logicOrder [i].currentType == WordTypes.orOperator) {
				statementParts.Add(InternalParseFunctions.getSubArray(logicOrder,lastOperatorPos+1, i-1, lineNumber));
				lastOperatorPos = i;
			}
				
		statementParts.Add (InternalParseFunctions.getSubArray (logicOrder, lastOperatorPos + 1, logicOrder.Length-1, lineNumber));


		List<bool> partStatementValues = new List<bool> ();
		foreach (Logic[] l in statementParts)
			if (validPartStatement (l, lineNumber) != true)
				return false;
		
	

		return true;
	}



	internal static bool validPartStatement(Logic[] logicOrder, int lineNumber){
		#region findOperators
		int operatorLow = 0;
		int operatorHigh = 0;
		int operatorAmount = 0;
		for (int i = 0; i < logicOrder.Length; i++)
			if (isStatementOperator (logicOrder [i])) {
				if (operatorLow == 0)
					operatorLow = i;
				else
					operatorHigh = i;
				operatorAmount++;
			}

		#endregion

		if (operatorAmount > 2) 
			ErrorMessage.sendErrorMessage (lineNumber, "Operatorerna i ditt expression går inte att tyda");
		if (operatorAmount == 0)
			return true;

		if (operatorAmount == 2) {
			if (operatorHigh - operatorLow != 1) 
				ErrorMessage.sendErrorMessage (lineNumber, "Operatorerna måste komma efter varandra");

		}
		else
			operatorHigh = operatorLow;




		Logic[] leftSide = new Logic[operatorLow];
		Logic[] rightSide = new Logic[logicOrder.Length - 1 - operatorHigh];
		Logic[] operators = new Logic[operatorAmount];

		setSidesOfStatement (logicOrder, leftSide, rightSide, operators, operatorLow, operatorHigh);

		ComparisonType operatorType = ComparisonOperatorParser.parseOperators (operators);

		if (operatorType == ComparisonType.unknown) 
			ErrorMessage.sendErrorMessage (lineNumber, "Operatorerna går inte att tyda");

		ValidSumCheck.checkIfValidSum (leftSide, lineNumber);
		ValidSumCheck.checkIfValidSum (rightSide, lineNumber);

		return true;
	}

	static void setSidesOfStatement(Logic[] logicOrder, Logic[] leftSide, Logic[] rightSide, Logic[] operators, int operatorLow, int operatorHigh){

		for (int i = 0; i < leftSide.Length; i++)
			leftSide [i] = logicOrder [i];

		for (int i = operatorHigh+1; i < logicOrder.Length; i++)
			rightSide [i - (operatorHigh+1)] = logicOrder [i];

		for (int i = operatorLow; i <= operatorHigh; i++)
			operators [i - operatorLow] = logicOrder [i];
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


}
