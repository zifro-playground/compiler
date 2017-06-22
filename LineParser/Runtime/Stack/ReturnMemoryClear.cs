using System;

namespace Compiler
{
	public class ReturnMemoryClear
	{
		public static void clearLineMemory(CodeLine currentLine){
			searchLogicOrder(currentLine.getLatestOrder());
			currentLine.resetCalculations ();
		}

		private static void searchLogicOrder(Logic[] logicOrder){
			foreach (Logic l in logicOrder)
				if (l.currentType == WordTypes.package)
					clearPackageMemory (l as Package);
				else if(l.currentType == WordTypes.functionCall)
					clearFuncCallMemory (l as FunctionCall);
		}


		private static void clearPackageMemory(Package thePackage){
			searchLogicOrder(thePackage.getLatestOrder());
			thePackage.resetCalculations ();
		}


		private static void clearFuncCallMemory(FunctionCall theFuncCall){
			if (theFuncCall.returnCalculations == null)
				return;

			foreach(Logic[] l in theFuncCall.returnCalculations)
				searchLogicOrder(l);

			theFuncCall.resetCalculations ();
		}

	}
}

