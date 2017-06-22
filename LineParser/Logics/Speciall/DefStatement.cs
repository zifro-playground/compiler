using System.Collections;

namespace Compiler{
	
	public class DefStatement : ScopeStarter {
		
		
		public int indentLevel;
		public ElseStatement linkedElse;
		
		public DefStatement(){
			base.currentType = WordTypes.defStatement;
			base.word = "def";
		}
		
		
	}
	

}