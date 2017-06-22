using System.Collections;

namespace Compiler{
	
	public class ElseStatement : ScopeStarter, ComparisonScope{
		
		public int indentLevel;
		public ElseStatement(){
			base.currentType = WordTypes.elseOperator;
			base.word = "else";
		}
		
		#region ComparisonScope implementation
		public void linkNextStatement (ComparisonScope nextScope){}
		public void initNextstatement (){}

		public void setEnterScope (bool value)
		{
			base.doEnterScope = value;
		}

		public void setParseLine (bool value)
		{
			base.doParseLine = value;
		}
		#endregion
	}
	

}