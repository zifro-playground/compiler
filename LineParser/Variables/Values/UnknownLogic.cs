using System;

namespace Compiler
{
	public class UnknownLogic : Logic {

		int lineNumber;

		public UnknownLogic(int lineNumber){
			this.lineNumber = lineNumber;
			base.currentType = WordTypes.unknown;
		}

	}

}

