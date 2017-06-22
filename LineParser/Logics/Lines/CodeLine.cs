using System;
using System.Collections.Generic;
using Runtime;

namespace Compiler
{
	
	public class CodeLine
	{
		public int lineNumber = 0;
		public int indentLevel = 0;
		public string[] words;
		public	Logic[] logicOrder;
		public Logic theCommandType;
		public Logic[] returnCalculations;
		public bool doParseLine = true;

		public CodeLine(int lineNumber, int indentNumber, string[] words){
			this.indentLevel = indentNumber;
			this.lineNumber = lineNumber;
			this.words = words;
		}


		public string getFullLine(){
			string temp = "";
			for (int i = 0; i < words.Length; i++)
				temp += words [i];

			return temp;
		}

		public CodeLine cloneLine(){
			CodeLine tempLine = new CodeLine (lineNumber, indentLevel, words);

			Logic[] tempOrder = (Logic[])logicOrder.Clone ();
			for (int i = 0; i < tempOrder.Length; i++) {
				if (tempOrder [i].currentType == WordTypes.returnStatement)
					tempOrder [i] = new ReturnStatement ();
				
			}
			tempLine.logicOrder = tempOrder;

			tempLine.resetCalculations ();
			return tempLine;
		}


		#region return calculations
		public Logic[] getLatestOrder(){
			if (returnCalculations == null) 
				return logicOrder;
			
			return returnCalculations;
		}
		
		public void resetCalculations(){
			returnCalculations = null;
		}
		
		public void insertReturnExpect(Logic l){
			Logic[] tempLogic = (Logic[])getLatestOrder ().Clone ();
			returnCalculations = tempLogic;

			ReturnMemoryControll.insertReturnExpectaton (this, lineNumber, l);
		}
		


		#endregion
	}
}

