using System;
using System.Collections.Generic;

namespace Compiler
{
	
	public class CurrentFunctions
	{
		public List<Function> savedFunctions = new List<Function>();

		public CurrentFunctions(){
			foreach (Function f in PreBuiltFunctionList.pythonFunctions)
				addFunction (f);
			foreach (Function f in GameFunctions.gameFunctions)
				addFunction (f);
		}


		public Function getSavedFunction(string searchedFunction, int lineNumber){
			for (int i = 0; i < savedFunctions.Count; i++)
				if (savedFunctions [i].name == searchedFunction) 
					return savedFunctions [i];

			ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, "Funktionen \"" + searchedFunction + "\" är inte definierad");
			return null;
		}

		public bool doesFunctionExist(string searchedFunction){
			foreach(Function f in savedFunctions)
				if (f.name == searchedFunction) 
					return true;

			return false;
		}


		public void addFunction(Function newFunc){
			for (int i = savedFunctions.Count-1; i >= 0; i--)
				if (savedFunctions [i].name == newFunc.name)
					savedFunctions.RemoveAt (i);

			savedFunctions.Add (newFunc);
		}


		public List<Function> getShallowCopy(){
			Function[] tempArray = savedFunctions.ToArray ();
			Function[] shallow = (Function[])tempArray.Clone ();
			List<Function> returnList = new List<Function> ();
			returnList.AddRange (shallow);
			return returnList;
		}
	}
}

