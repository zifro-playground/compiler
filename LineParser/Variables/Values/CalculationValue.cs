using System;

namespace Compiler
{
	public class CalculationValue : Logic
	{
		int lineNumber;

		public CalculationValue(int lineNumber){
			this.lineNumber = lineNumber;
			base.currentType = WordTypes.calculationValue;
		}

	}
}

