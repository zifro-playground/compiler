using System.Collections;

namespace Compiler{
	
	public class IndentOperator : Logic{
		
		public IndentOperator(){
			base.currentType = WordTypes.indentOperator;
			base.word = ":";
		}
		
	}
	

}