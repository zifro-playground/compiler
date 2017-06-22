using System;

namespace Compiler
{
	public class ElifStatement : ScopeStarter, ComparisonScope{

		public int indentLevel;
		public ComparisonScope nextStatement;

		public ElifStatement(){
			base.currentType = WordTypes.elifOperator;
			base.word = "elif";
			base.doEnterScope = false;
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
			if (value == false && nextStatement != null)
				nextStatement.setParseLine (false);
			base.doParseLine = value;
		}
		#endregion
	}
}

