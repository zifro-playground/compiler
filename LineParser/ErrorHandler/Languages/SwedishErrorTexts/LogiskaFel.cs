using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public class LogiskaFel : LogicErrors
	{
		#region LogicErrors implementation

		public string unknownLogic (string[] arg)
		{
			return "Okänd kombination av kod";
		}

		public string missingIndentOperator (string[] arg)
		{
			return "Det saknas ett \":\"";
		}

		#endregion



	}
}

