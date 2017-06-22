using System;
using ErrorHandler;
using Runtime;

namespace Compiler
{
	public class ReturnMemoryControll
	{
		public static void insertReturnExpectaton(CodeLine currentLine, int lineNumber, Logic targetLogic){
			if (currentLine.returnCalculations == null)
				currentLine.returnCalculations = (Logic[])currentLine.logicOrder.Clone ();

			Logic[] tempOrder = (Logic[])currentLine.getLatestOrder();

		//	Debugger.printLogicOrder (tempOrder, "Return insert Order");
			searchInLogicOrderExpect (tempOrder, lineNumber, targetLogic);
		}


		private static bool searchInLogicOrderExpect(Logic[] logicOrder, int lineNumber, Logic targetLogic){
			for (int i = 0; i < logicOrder.Length; i++)
				if (logicOrder [i] == targetLogic) {
					logicOrder [i] = new ReturnValue();
				//	Print.print ("FOOOUUND IT! and replaced it");
					return true;
				}
				else if (logicOrder [i].currentType == WordTypes.package) {
					if (insertReturnValueIntoPackageExpect (logicOrder[i] as Package, lineNumber, targetLogic))
						return true;
				}
				else if (logicOrder [i].currentType == WordTypes.functionCall) {
					if (insertReturnValueIntoFuncCallExpect (logicOrder[i] as FunctionCall, lineNumber, targetLogic))
						return true;
				}

			return false;
		}



		private static bool insertReturnValueIntoPackageExpect(Package thePackage, int lineNumber, Logic targetLogic){
			if (thePackage.returnCalculations == null)
				thePackage.returnCalculations = (Logic[])thePackage.logicOrder.Clone ();

			Logic[] tempOrder = thePackage.getLatestOrder();
			return searchInLogicOrderExpect (tempOrder, lineNumber, targetLogic);
		}




		private static bool insertReturnValueIntoFuncCallExpect(FunctionCall theFuncCall, int lineNumber, Logic targetLogic){
			if (theFuncCall.returnCalculations == null)
				return false;


			foreach (Logic[] l in theFuncCall.returnCalculations) {
				if (searchInLogicOrderExpect (l, lineNumber, targetLogic))
					return true;
			}

			return false;
		}





		//**********************************

		public static void insertReturnValue(CodeLine currentLine, int lineNumber, Variable returnVar){
			Logic[] tempOrder = currentLine.getLatestOrder();

		//	Debugger.printLogicOrder (tempOrder, "Return insert Order");

			searchInLogicOrder (tempOrder, lineNumber, returnVar);
		}


		private static bool searchInLogicOrder(Logic[] logicOrder, int lineNumber, Variable returnVar){

			for (int i = 0; i < logicOrder.Length; i++)
				if (logicOrder [i].currentType == WordTypes.returnValue) {
			//		Debugger.printLogicOrder (logicOrder, "Pre Order");
					logicOrder[i] = insertTheValue (returnVar);
				//	Debugger.printLogicOrder (logicOrder, "Post Order");
					return true;
				}
				else if (logicOrder [i].currentType == WordTypes.package) {
					if (insertReturnValueIntoPackage (logicOrder[i] as Package, lineNumber, returnVar))
						return true;
				}
				else if (logicOrder [i].currentType == WordTypes.functionCall) {
					if (insertReturnValueIntoFuncCall (logicOrder[i] as FunctionCall, lineNumber, returnVar))
						return true;
				}

			return false;
		}



		private static bool insertReturnValueIntoPackage(Package thePackage, int lineNumber, Variable returnVar){
			Logic[] tempOrder = thePackage.getLatestOrder();
			return searchInLogicOrder (tempOrder, lineNumber, returnVar);
		}

		private static bool insertReturnValueIntoFuncCall(FunctionCall theFuncCall, int lineNumber, Variable returnVar){
			if (theFuncCall.returnCalculations == null)
				return false;

			foreach (Logic[] l in theFuncCall.returnCalculations)
				if (searchInLogicOrder (l, lineNumber, returnVar)) {
					return true;
				}

			return false;
		}


		private static Logic insertTheValue(Variable returnVar){
			Print.print ("Var type: " + returnVar.variableType);

			if (returnVar.variableType == VariableTypes.boolean)
				return new BooleanValue (returnVar.getBool ());
			else if (returnVar.variableType == VariableTypes.number)
				return new NumberValue (returnVar.getNumber ());
			else if (returnVar.variableType == VariableTypes.textString)
				return new TextValue (returnVar.getString ());

			return new Variable ("Nill");
		}

	}
}

