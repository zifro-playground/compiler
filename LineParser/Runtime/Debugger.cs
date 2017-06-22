using System;
using Compiler;

namespace Runtime
{
	public class Debugger
	{

		public static void printLogicOrder(Logic[] logicOrder, string message){
			Print.print (message + "************");
			foreach (Logic l in logicOrder)
				Print.print (l.currentType.ToString ());
			Print.print ("*********************");
		}

		public static void printStackTrace(Scope currentScope){
			string stackString = "";

			Scope tempScope = currentScope;
			while (tempScope != null) {
				if (tempScope.theScopeType == ScopeType.main || tempScope.theScopeType == ScopeType.function)
					stackString += tempScope.theScopeType.ToString () + " - ";

				tempScope = tempScope.parentScope;
			}

			Print.print ("Stack trace: ******");
			Print.print (stackString);
			Print.print ("**********");
		}

	}
}

