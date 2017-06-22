using System;

namespace Compiler
{
	public class WhileLoop : ScopeLoop, Loop {

		public int indentLevel;
		public Logic[] theStatement;

		public WhileLoop(){
			base.currentType = WordTypes.whileLoop;
			base.word = "while";
		}

		#region Loop implementation
		public bool makeComparison (int lineNumber, bool changeValue = true)
		{
			return StatementParser.parseStatement (theStatement, lineNumber, getTargetScope());
		}

		public void addCounterVariableToScope (int lineNumber)
		{

		}
		public void resetCounterVariable ()
		{

		}
		#endregion
	}

}

