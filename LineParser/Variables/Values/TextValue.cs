using System;

namespace Compiler
{
	public class TextValue : Logic{

		public string value;

		public TextValue(string word){
			base.currentType = WordTypes.textString;
			base.word = word;

			if (word [0] == '"' && word [word.Length - 1] == '"')
				removeQoutes ();
			else
				value = word;
		}

		private void removeQoutes(){
			if (word.Length < 3)
				value = "";
			else 
				value = word.Substring (1, word.Length - 2);
		}

	}
}

