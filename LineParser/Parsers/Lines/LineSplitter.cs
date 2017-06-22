using System;
using System.Collections.Generic;

namespace Compiler
{
	internal class LineSplitter
	{
		public static List<ParsedLine> splitTextIntoLines(string currentText){
			List<ParsedLine> tempList = new List<ParsedLine> ();

			string tempLine = "";
			int currentLineNumber = 1;
			int lastLinePos = -1;

			for (int i = 0; i < currentText.Length; i++) {
				if (currentText [i] == '\n') {
					tempLine = currentText.Substring (lastLinePos + 1, i - (lastLinePos + 1));
					tempList.Add (new ParsedLine(tempLine, currentLineNumber));
					lastLinePos = i;
					currentLineNumber++;
				}
			}

			if (lastLinePos != currentText.Length) {
				tempLine = currentText.Substring (lastLinePos + 1, currentText.Length - (lastLinePos + 1));
				tempList.Add (new ParsedLine(tempLine, currentLineNumber));
			}

			return tempList;
		}
	}
}

