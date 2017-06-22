using System;
using Runtime;

namespace Compiler
{
	public class BooleanExpression : Logic, BoolType
	{
		private Logic[] logicOrder;
		private int lineNumber;
		private Scope currentScope;

		public BooleanExpression (Logic[] logicOrder, int lineNumber, Scope currentScope)
		{
			base.currentType = WordTypes.booleanExpression;
			this.logicOrder = logicOrder;
			this.lineNumber = lineNumber;
			this.currentScope = currentScope;
		}

		public BooleanValue parseExpression(){
			return new BooleanValue(StatementParser.parseStatement(logicOrder, lineNumber, currentScope));
		}
	}
}

