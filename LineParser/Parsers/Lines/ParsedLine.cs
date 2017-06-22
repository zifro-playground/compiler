using System;

namespace Compiler
{
	public class ParsedLine{
		public string theString;
		public int lineNumber;

		public ParsedLine(string text, int lineNumber){
			this.theString = text;
			this.lineNumber = lineNumber;
		}

	}
}

