using System;

namespace ErrorHandler
{
	public class ErrorStatements
	{
		internal static string missingIndentOperatorIFStatement = "Det saknas ett \":\" i slutet på din if sats";



		internal static string missingIndentOperator(string operatorType){
			return	string.Format("Det saknas ett \":\" i slutet på din {0} sats", operatorType);
		}

		internal static string unknowsFormat(string operatorType){
			return	string.Format("Ett okänt format på din {0} sats", operatorType);
		}
	}

}

