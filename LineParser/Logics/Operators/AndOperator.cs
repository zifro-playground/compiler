using System.Collections;

namespace Compiler{

	public class AndOperator : Logic, AndOrOperator {

		public AndOperator(string word){
			base.currentType = WordTypes.andOperator;
			base.word = word;
		}

	}


}