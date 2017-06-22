using System.Collections;

namespace Compiler{

	public class OrOperator : Logic, AndOrOperator {

		public OrOperator(string word){
			base.currentType = WordTypes.orOperator;
			base.word = word;
		}

	}


}