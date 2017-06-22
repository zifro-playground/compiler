using System;

namespace Compiler
{
	public class PackageUnWrapper{

		public static void unpackPackage(string word, int lineNumber, Scope currentScope, Package currentPackage){

			if (word.Length <= 2)
				return;

			string trimedString = removeSurrondingParanteser(word);

			string[] words = WordParser.parseWords (trimedString);
			Logic[] logicOrder = WordsToLogicParser.determineLogicFromWords (words, lineNumber, currentScope);

			currentPackage.logicOrder = logicOrder;
		}



		public static string removeSurrondingParanteser(string word){
			return  word.Substring (1, word.Length - 2);
		}

	}
}

