using System.Collections;
using Runtime;
using Compiler;

public class PossibleVariableDeclare{

	internal static bool checkForVariableDecleration(Logic[] logicOrder, int lineNumber){

		if (logicOrder.Length < 3)
			return false;

		if (logicOrder [0].currentType != WordTypes.variable)
			return false;

		int equalPos = 0;
		for (int i = 0; i < logicOrder.Length; i++, equalPos++)
			if (logicOrder [i].currentType == WordTypes.equalSign)
				break;

		if (equalPos != 1 && equalPos != 2)
			return false;

		if (equalPos == 2 && logicOrder [1].currentType != WordTypes.mathOperator)
			ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, "Du kan bara ha matte operatorer i special deklareringar!");

		Logic[] afterEqualSign = new Logic[logicOrder.Length - (equalPos+1)];
		for (int i = equalPos+1; i < logicOrder.Length; i++)
			afterEqualSign [i-(equalPos+1)] = logicOrder [i];


		ValidSumCheck.checkIfValidSum (afterEqualSign, lineNumber);

		return true;
	}
}
