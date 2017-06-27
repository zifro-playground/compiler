using System;
using System.Collections.Generic;

namespace Compiler
{
	public class WordParser {

		//Parses entered text into a string array of all the seperate words entered 

		private static char[] saveOperators = { ':', '=', '!', ',', '>', '<' };

		//Everything included into Parentheses will be returnd as ONE word.
		//The logic for this word will later be *Package*.

		internal static string[] parseWords(string trimedString){

			List<string> words = new List<string> ();

			int lastBreak = -1;
			int foundLetters = 0;
			int functionParantes = 0;
			int functionQoutes = 0;
			for (int i = 0; i < trimedString.Length; i++) {


				//Checks if current Char is a "BreakWordChar" meaning it stops the current word recording.
				if (isBreakWordChar (trimedString [i], functionParantes, functionQoutes)) {

					if (foundLetters > 0) {
						string temp = trimedString.Substring (lastBreak + 1, foundLetters);
						words.Add (temp);
					}

					foundLetters = 0;
					lastBreak = i;

					//If the current breakchar is worth saving!
					if (isSaveOperator (trimedString [i])) {
						string temp = "";
						temp += trimedString [i];
						words.Add (temp);
					}
				} else {
					functionQoutes = isInFunctionQuotes (trimedString [i], functionQoutes);
					functionParantes += isInFunctionParameter (trimedString [i], foundLetters, functionQoutes);
					foundLetters++;
				}


			}

			if (foundLetters > 0) {
				string temp = trimedString.Substring (lastBreak + 1, foundLetters);
				words.Add (temp);
			}


			return words.ToArray ();
		}




		#region char checks
		static bool isSaveOperator(char c){

			foreach (char Operator in saveOperators)
				if (c == Operator)
					return true;

			for(int i = 0; i < MathParser.mathOperators.Length; i++)
				if(c == MathParser.mathOperators[i])
					return true;

			return false;
		}


		static int isInFunctionQuotes(char c, int functionQoutes){

			if (c == '"') {
				if (functionQoutes > 0)
					return 0;
				else
					return 1;
			}
			return functionQoutes;

		}

		static int isInFunctionParameter(char c, int foundLetters, int functionQoutes){

			if (functionQoutes > 0)
				return 0;

			if (c == '(')
				return 1;

			if (c == ')')
				return -1;

			return 0;
		}


		static bool isBreakWordChar(char c, int functionParantes, int functionQoutes){
			if (c == '\n')
				return true;

			if (functionParantes > 0 || functionQoutes > 0) 
				return false;


			if (c == ' ' || c == '\t')
				return true;

			if (c == ':' || c == '=' || c == '!' || c == ',' || c == '>' || c == '<' ) 
				return true;


			for(int i = 0; i < MathParser.mathOperators.Length; i++)
				if(c == MathParser.mathOperators[i])
					return true;

			return false;
		}
		#endregion
	}

}

