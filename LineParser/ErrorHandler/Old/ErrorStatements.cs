using System;

namespace ErrorHandler
{
	public class ErrorStatements
	{
		[System.Obsolete("Method is deprecated, use new ErrorHandling system instead", true)]
		internal static string missingIndentOperatorIFStatement = "Det saknas ett \":\" i slutet på din if sats";


		[System.Obsolete("Method is deprecated, use new ErrorHandling system instead", true)]
		internal static string missingIndentOperator(string operatorType){
			return	string.Format("Det saknas ett \":\" i slutet på din {0} sats", operatorType);
		}

		[System.Obsolete("Method is deprecated, use new ErrorHandling system instead", true)]
		internal static string unknowsFormat(string operatorType){
			return	string.Format("Ett okänt format på din {0} sats", operatorType);
		}
	}

}

