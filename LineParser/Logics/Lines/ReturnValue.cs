using System;

namespace Compiler
{
	public class ReturnValue : Logic
	{
		public ReturnValue(){
			base.currentType = WordTypes.returnValue;
			base.word = "returnValue";
		}
	}
}

