using System;
using System.Collections.Generic;

namespace Compiler
{
	public class LineParser
	{
		public static List<CodeLine> parseLines(List<ParsedLine> lineList){
			List<CodeLine> tempList = new List<CodeLine> ();

			for (int i = 0; i < lineList.Count; i++) {
				if (lineList [i].theString != "" && containsChar(lineList[i].theString) && !isComment(lineList[i].theString)) {
					int indentLevel = getIndentLevel (lineList [i].theString);
					string trimmedString = lineList [i].theString.Substring (indentLevel, lineList [i].theString.Length - indentLevel);
					string[] words = WordParser.parseWords (trimmedString);
					addLine (tempList, lineList[i].lineNumber, indentLevel, words);
				}
			}

			return tempList;
		}

		private static bool containsChar(string line){	
			if (string.IsNullOrEmpty(line.Trim()))
				return false;
			return true;
		}

		private static bool isComment(string line){
			for (int i = 0; i < line.Length; i++)
				if (char.IsWhiteSpace (line [i]) == false) {
					if (line [i] == '#')
						return true;
				
					return false;
				}

			return false;
		}

		private static int getIndentLevel(string line){
			int indentCounter = 0;
			for (int i = 0; i < line.Length; i++) {
				if (line [i] == '\t')
					indentCounter++;
				else
					break;
			}

			return indentCounter;
		}

		private static void addLine(List<CodeLine> programCodeLines ,int lineNumber, int indentLevel, string[] words){
			CodeLine temp = new CodeLine (lineNumber, indentLevel, words);
			programCodeLines.Add (temp);
		}
	}
}

