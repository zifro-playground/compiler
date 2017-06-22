using System.Collections;

namespace Compiler{
	
	public class XorOperator : Logic, ComparisonOperator {
		
		public XorOperator(string word){
			base.currentType = WordTypes.xorOperator;
			
			base.word = word;
		}
		
	}
	

}