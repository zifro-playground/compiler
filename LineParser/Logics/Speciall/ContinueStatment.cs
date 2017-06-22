using System;

namespace Compiler
{
	public class ContinueStatment : Logic
	{
		public ContinueStatment(){
			base.currentType = WordTypes.continueStatement;
			base.word = "continue";
		}

	}
}

