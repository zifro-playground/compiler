using System.Collections;

namespace Compiler{

	public class CommaSign : Logic{

		public CommaSign(){
			base.currentType = WordTypes.commaSign;
			base.word = ",";
		}

	}
}