using System;

namespace Compiler
{
	public class BooleanValue : Logic, BoolType{

		public bool value;

		public BooleanValue(bool inputValue){
			base.currentType = WordTypes.booleanValue;
			base.word = "Bool Value";
			value = inputValue;
		}

	}
}

