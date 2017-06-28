using System;

namespace ErrorHandler
{
	public class ForLoopFel : ForLoopErrors
	{
		
		/// Missings the indent operator ":".
		public string missingIndentOperator (string[] arg)
		{
			return "Det saknas ett \":\" i slutet av din for-slinga";
		}

		/// Weird format on for loop. E.g. more than 5 or less than 4 words.
		public string unknownFormat (string[] arg)
		{
			return "Okänt format på for-slingan. Kom ihåg att den ska likna: \"for i in range(5):\"";
		}

		/// Expects a index variable (e.g. i) as word number 2.
		public string expectVariableAt2 (string[] arg)
		{
			return "I en for-slinga förväntas det andra ordet att vara en variabel (till exempel: i)";
		}

		/// Expects the keyword "in" as word number 3.
		public string expectInAt3 (string[] arg)
		{
			return "I en for-slinga förväntas det tredje ordet att vara \"in\"";
		}

		/// Expects the keyword "range" as word number 4.
		public string expectRangeAt4 (string[] arg)
		{
			return "I en for-slinga förväntas det fjärde ordet att vara \"range\"";
		}

		/// Expects atleast on number between parenthesis to range().
		public string rangeArgumentEmpty (string[] arg)
		{
			return "Det måste finnas minst en siffra mellan parenteserna till \"range()\"";
		}

		/// Expects the arguments to range() to be numbers.
		public string rangeArgumentNotNumber (string[] arg)
		{
			return "Det får bara vara siffor mellan parenteserna till \"range()\"";
		}

		/// Expects the keyword "range" to be followed by parenthesis.
		public string rangeMissingParenthesis (string[] arg)
		{
			return "Det saknas parenteser till \"range()\"";
		}

	}
}

