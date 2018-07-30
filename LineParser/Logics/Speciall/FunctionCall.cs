using System.Collections;
using System.Collections.Generic;
using Runtime;

namespace Compiler{
	
	public class FunctionCall : Logic {
		
		public string fullWord;
		public string name;
		public string parameter;
		public bool hasReturnVariable;
		public Function targetFunc;
		public Variable[] inputVariables;

		public List<Logic[]> returnCalculations;

		public int lineNumber;
		
		public FunctionCall(string fullWord, string name, string parameter, Function targetFunc){
			setLogic (word, WordTypes.functionCall);
			this.fullWord = word;
			this.targetFunc = targetFunc;
			this.name = name;
			this.parameter = parameter;
			base.currentType = WordTypes.functionCall;
		}
		
		
		public Variable runFunction (Scope currentScope){
			if (CodeWalker.isWaitingForUserInput)
				return new Variable();

			return targetFunc.runFunction (currentScope, inputVariables, lineNumber);
		}
		
		public void setLogic(string word, WordTypes currentType){
			base.currentType = currentType;
			base.word = word;
		}


		#region Return Calculations
		public void setReturnCalculations(List<Logic[]> inputList){
			returnCalculations = new List<Logic[]> ();
			foreach (Logic[] l in inputList)
				returnCalculations.Add (l);
		}

		public void resetCalculations(){
			returnCalculations.Clear ();
			returnCalculations = null;
		}

		#endregion
	}
	

}