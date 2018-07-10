using System;
using System.Linq;
using System.Collections;
using Compiler;


namespace Runtime
{
	public class VariableDeclareParser{


		public static Logic checkForVariableDecleration(Logic[] logicOrder, int lineNumber, Scope currentScope){
			//We use a shallow copy because we alter logicOrder in case of speciall operators
			Logic[] cloneLogicOrder = (Logic[])logicOrder.Clone ();

			if(couldBeVariableDec(cloneLogicOrder, lineNumber) == false)
				return new UnknownLogic(lineNumber);


			Logic[] afterEqualSign = getAfterEqualSign (cloneLogicOrder, lineNumber, currentScope);
			if(afterEqualSign == null || afterEqualSign.Length == 0)
				return new UnknownLogic(lineNumber);

			Variable afterEqualSignSum = SumParser.parseIntoSum (afterEqualSign, lineNumber, currentScope);

			//Determine whether to use speciallOperators or not
			if (cloneLogicOrder [1].currentType != WordTypes.equalSign)
				afterEqualSignSum = SpeciallVariableDeclareParser.speciallVariableDeclare (cloneLogicOrder, lineNumber, afterEqualSignSum, currentScope);
			else
				afterEqualSignSum.name = (cloneLogicOrder[0] as Variable).name;
				
			//Add the newly declared variable
			if (afterEqualSignSum.variableType != VariableTypes.unknown) {
				currentScope.scopeVariables.addVariable(afterEqualSignSum, currentScope.scopeParser, lineNumber);
				return afterEqualSignSum;
			}

			ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, "Något gick fel i variabeldeklareringen");
			return new UnknownLogic(lineNumber);
		}



		#region Basic Checks
		private static bool couldBeVariableDec(Logic[] cloneLogicOrder, int lineNumber){
			if (cloneLogicOrder.Length < 3)
				return false;

			if (cloneLogicOrder [0].currentType != WordTypes.variable)
				return false;

			return true;
		}
		#endregion




		#region getLogicAfterEqualSign
		private static Logic[] getAfterEqualSign(Logic[] cloneLogicOrder, int lineNumber, Scope currentScope){

			int equalPos = getEqualPos (cloneLogicOrder);
			if ((equalPos != 1 && equalPos != 2))
				return null;


			Logic[] afterEqualSign = new Logic[cloneLogicOrder.Length - (equalPos+1)];
			for (int i = equalPos+1; i < cloneLogicOrder.Length; i++)
				afterEqualSign [i-(equalPos+1)] = cloneLogicOrder [i];

			return afterEqualSign;
		}


		private static int getEqualPos(Logic[] logicOrder){
			int equalPos = 0;
			for (int i = 0; i < logicOrder.Length; i++, equalPos++)
				if (logicOrder [i].currentType == WordTypes.equalSign)
					break;
			
			return equalPos;
		}
		#endregion
	}
}

