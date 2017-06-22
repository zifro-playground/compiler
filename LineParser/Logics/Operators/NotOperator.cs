using System.Collections;

namespace Compiler{
	
	public class NotOperator : Logic {
		
		public NotOperator(string word){
			base.currentType = WordTypes.notOperator;
			
			base.word = word;
		}
		
	}
	

}