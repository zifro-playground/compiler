using System;

namespace Compiler
{
	public class NumberValue : Logic{

		public double value = 0;

		public NumberValue(string word){
			base.currentType = WordTypes.number;
			base.word = word;
			double.TryParse (word, out value);
		}

		public NumberValue(double value){
			base.currentType = WordTypes.number;
			base.word = value.ToString();
			this.value = value;
		}
	}
}

