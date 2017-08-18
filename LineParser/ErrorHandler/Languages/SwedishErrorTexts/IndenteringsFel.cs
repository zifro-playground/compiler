using System;

namespace ErrorHandler
{
	public class IndenteringsFel : IndentationErrors
	{
		#region Errors from SyntaxCheck

		/// Called if there is an indentation without a scope starter (eg if, while) before
		public string unknownIndentStarter (string[] arg)
		{
			return "Okänd start på indentering";
		}

		/// Called if first line is not at indentation level 0
		public string firstLineIndentError (string[] arg)
		{
			return "Första raden i programmet måste alltid ha indentering noll";
		}

		/// Called if there is a indentation error like missing to indent after if declaration or indenting too much
		public string indentationError (string[] arg)
		{
			return "Indenterings fel!";
		}

		/// Called if there is no more lines of code after a scope starter (eg if, while)
		public string expectingBodyAfterScopeStarter (string[] arg)
		{
			return "Förväntar sig indenterad kod efter den här raden";
		}

		#endregion



		#region Errors from Runtime

		#endregion
	}
}

