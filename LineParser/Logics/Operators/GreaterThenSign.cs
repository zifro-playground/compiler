using System.Collections;

namespace Compiler{
	
	public class GreaterThenSign : Logic, ComparisonOperator{
		
		public GreaterThenSign(){
			base.currentType = WordTypes.greaterThenSign;
			base.word = ">";
		}
		
	}
	

}