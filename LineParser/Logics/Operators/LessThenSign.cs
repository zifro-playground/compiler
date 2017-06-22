using System.Collections;

namespace Compiler{

	public class LessThenSign : Logic, ComparisonOperator{
		
		public LessThenSign(){
			base.currentType = WordTypes.lessThenSign;
			base.word = "<";
		}
		
	}
	
}