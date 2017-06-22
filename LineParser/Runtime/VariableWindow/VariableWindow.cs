using System;
using System.Collections.Generic;
using Compiler;

namespace Runtime
{
	public class VariableWindow
	{

		private static Action<Variable> insertVariable;
		private static Action resetList;

		private static List<Variables> tempVariables = new List<Variables>();
	//	private static PreBuiltFunctionList

		public static void setVariableWindowFunctions(Action<Variable> insertVariableToList, Action resetTheList){
			insertVariable = insertVariableToList;
			resetList = resetTheList;
		}

		internal static void sendStackVariables(Scope currentScope){
			resetList.Invoke ();
			tempVariables.Clear ();
			tempVariables.Add (new Variables ());
			sendScopeVariables (currentScope);

			foreach (Variables Vars in tempVariables)
				foreach (Variable v in Vars.variableList)
					insertVariable (v);
		}

		private static void sendScopeVariables(Scope currentScope){
			Variables latestList = tempVariables [tempVariables.Count - 1];
			foreach (Variable v in currentScope.scopeVariables.variableList)
				if (latestList.containsVariable (v.name) < 0)
					latestList.addVariable (v, currentScope.scopeParser, 0); 



		/*
			if (tempVariables == null)
				tempVariables = new Variables ();
			
			foreach (Variable v in currentScope.scopeVariables.variableList)
				tempVariables.addVariable (v, Compiler.SyntaxCheck.globalParser, 0);


			if (currentScope.theScopeType == ScopeType.main || currentScope.theScopeType == ScopeType.function) {
				for (int i = tempVariables.variableList.Count - 1; i >= 0; i--)
					insertVariable (tempVariables.variableList [i]);
				tempVariables = null;
			}
		*/
			if (currentScope.parentScope != null) {
				if (currentScope.theScopeType == ScopeType.function)
					tempVariables.Add (new Variables ());
				sendScopeVariables (currentScope.parentScope);
			}
		}

	}
}

