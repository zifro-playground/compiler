using System;

namespace Compiler
{
	public class BoolAlgebraWord
	{
		public string calcWord;

		public BoolAlgebraWord (string word)
		{
			if (word == "and" || word == "or")
				setOperatorSettings (word);
			else
				setValueSettings (word);
		}

		private void setOperatorSettings(string word){
			if (word == "and")
				calcWord = "*";
			else
				calcWord = "+";
		}

		private void setValueSettings(string word){
			calcWord = Convert.ToInt32 (Convert.ToBoolean (word)).ToString();
		}
	}
}

