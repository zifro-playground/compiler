using System;

namespace ErrorHandler
{
	public class IndenteringsFel : IndentationErrors
	{
		#region IndentationErrors implementation

		public string unknownIndentStarter (string[] arg)
		{
			return "Okänd start på indentering";
		}

		public string firstLineIndentError (string[] arg)
		{
			return "Första raden i programmet måste alltid ha indentering noll";
		}

		public string indentationError (string[] arg)
		{
			return "Indenterings fel!";
		}

		public string indentExpectingBody (string[] arg)
		{
			return "Förväntar sig indenterad kod efter den här raden";
		}

		#endregion

	}
}

