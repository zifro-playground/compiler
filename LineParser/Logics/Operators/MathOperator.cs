using System.Collections;

namespace Compiler{
	
	public class MathOperator : Logic {
		
		public MathOperator(string word){
			base.currentType = WordTypes.mathOperator;
			
			base.word = word;
		}
		
	}
	

}