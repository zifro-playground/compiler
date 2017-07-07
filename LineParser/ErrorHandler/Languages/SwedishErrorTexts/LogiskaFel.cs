using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public class LogiskaFel : LogicErrors
	{

		#region common errors
		public string unknownLogic (string[] arg)
		{
			return "Okänd kombination av kod";
		}

		public string missingIndentOperator (string[] arg)
		{
			return "Det saknas ett \":\"";
		}
		#endregion
			

		#region errors that should not reach user
		/// Called if (startIndex < logicOrder.Length && endIndex < logicOrder.Length) == true
		/// Error could be caused by corrupt And/Or statement
		public string corruptAndOrStatement (string[] arg)
		{
			return "Något är fel vid And/Or";
		}

		#endregion


	}
}

