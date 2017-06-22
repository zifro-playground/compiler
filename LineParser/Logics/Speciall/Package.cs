using System;

namespace Compiler
{
	public class Package : Logic{

		public int lineNumber = 0;
		public Logic[] logicOrder;
		public Logic[] returnCalculations;


		public Package(string word, int lineNumber){
			this.lineNumber = lineNumber;
			base.word = word;
			base.currentType = WordTypes.package;
		}

		public Package(Logic[] logicOrder, int lineNumber){
			this.lineNumber = lineNumber;
			base.word = "Internal Package";
			base.currentType = WordTypes.package;
			this.logicOrder = logicOrder;
		}



		public void setLogicOrder(Logic[] logicOrder){
			this.logicOrder = logicOrder;
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

		public void insertAwaitReturn(Logic l){
			Logic[] tempLogic = (Logic[])getLatestOrder ().Clone ();

			for (int i = 0; i < tempLogic.Length; i++)
				if (tempLogic [i] == l) {
					tempLogic [i] = new ReturnValue ();
					returnCalculations = tempLogic;
					return;
				}
				else if (tempLogic [i].currentType == WordTypes.package)
					(tempLogic [i] as Package).insertAwaitReturn (l);
		}
		#endregion
	}
}

