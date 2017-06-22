using System.Collections;

namespace Compiler{
	
	public class IfStatement : ScopeStarter, ComparisonScope {
		
		public int indentLevel;
		public ComparisonScope nextStatement;
		
		public IfStatement(){
			base.currentType = WordTypes.ifOperator;
			base.word = "if";
		}


		public void initNextstatement(){
			if (nextStatement != null) {
				nextStatement.setParseLine (!doEnterScope);

				if (nextStatement is ElseStatement)
					nextStatement.setEnterScope (!doEnterScope);
			}
		}
			
		#region ComparisonScope implementation
		public void linkNextStatement (ComparisonScope nextScope){
			nextStatement = nextScope;
		}
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