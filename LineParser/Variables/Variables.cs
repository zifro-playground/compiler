using System;
using System.Collections.Generic;
using B83.ExpressionParser;
using Runtime;

namespace Compiler
{
	
	public class Variables
	{
		public List<Variable> variableList = new List<Variable>();

		//Later fix!
		//Should be possible to avoid all the else if statements.
		public void addVariable(Variable newVar, ExpressionParser scopePareser, int lineNumber){
			//First we check if the variable we are trying to add already exists, if it does we change the value of it.
			//Otherwise create a new variable
			int tempCont = containsVariable (newVar.name);
			if (tempCont >= 0) {
				variableList [tempCont] = newVar;

				if (newVar.variableType == VariableTypes.boolean)
					changeExistingVariable (tempCont, newVar.getBool (), scopePareser);
				else if (newVar.variableType == VariableTypes.number)
					changeExistingVariable (tempCont, newVar.getNumber (), scopePareser);
				else if (newVar.variableType == VariableTypes.textString)
					changeExistingVariable (tempCont, newVar.getString (), scopePareser);
				else if(newVar.variableType == VariableTypes.None)
					changeExistingVariable (tempCont, newVar.getString (), scopePareser);
				//else
				//	ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, "Tried to Change " + newVar.name + " to unsupported Value");

			} 
			else {
				if (newVar.variableType != VariableTypes.unknown && newVar.variableType != VariableTypes.unsigned) {
					variableList.Add (newVar);
					if (newVar.variableType == VariableTypes.number)
						scopePareser.AddConst (newVar.name, () => newVar.getNumber ());
				}
			//	else
				//	ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, "Tried to Add " + newVar.name + " with unsupported Value");

			}

		}


		#region Alteration methods
		public void changeExistingVariable(int index, double newValue, ExpressionParser scopeParser){
			removeFromParser (variableList [index].name, scopeParser);

			variableList [index].setValue (newValue);
			scopeParser.AddConst(variableList [index].name, () => variableList [index].getNumber());
		}

		public void changeExistingVariable(int index, string newValue, ExpressionParser scopeParser){
			removeFromParser (variableList [index].name, scopeParser);
			variableList [index].setValue (newValue);
		}

		public void changeExistingVariable(int index, bool newValue, ExpressionParser scopeParser){
			removeFromParser (variableList [index].name, scopeParser);
			variableList [index].setValue (newValue);
		}

		public void changeExistingVariable(int index, ExpressionParser scopeParser){
			removeFromParser (variableList [index].name, scopeParser);
			variableList [index].setValue ();
		}

		void removeFromParser(string name, ExpressionParser scopeParser){
			if (scopeParser.containgsConst (name))
				scopeParser.RemoveConst (name);
		}
		#endregion


		#region searchFunctions
		public int containsVariable(string name){
			for (int i = 0; i < variableList.Count; i++) 
				if (variableList [i].name == name)
					return i;


			return -1;
		}

		public int containsVariableOfType(string name, VariableTypes checkType){
			for (int i = 0; i < variableList.Count; i++)
				if (variableList [i].name == name)
				if (variableList [i].variableType == checkType)
					return i;
				else
					return -1;

			return -1;
		}
		#endregion


		public void printVariableList(){
			foreach (Variable v in variableList)
				Print.print (v.name + "      Num: " + v.getNumber () + " Bool: " + v.getBool () + " String: " + v.getString ());
			
		}
	}



}

