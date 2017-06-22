using System;

namespace Compiler
{
	public class BreakStatement : Logic{

		public BreakStatement(){
			base.currentType = WordTypes.breakStatement;
			base.word = "break";
		}


	}
}

