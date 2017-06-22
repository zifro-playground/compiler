using System.Collections;

namespace Compiler{
	
	public class EqualSign : Logic, ComparisonOperator{
		
		public EqualSign(){
			base.currentType = WordTypes.equalSign;
			base.word = "=";
		}
		
	}

}