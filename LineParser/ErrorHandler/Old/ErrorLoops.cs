using System;

namespace ErrorHandler
{
	public class ErrorLoops
	{
		[System.Obsolete("Method is deprecated, use new ErrorHandling system instead", true)]
		internal static string missingIndentOperator(string operatorType){
			return	string.Format("Det saknas ett \":\" i slutet på din {0} loop", operatorType);
		}

		[System.Obsolete("Method is deprecated, use new ErrorHandling system instead", true)]
		internal static string unknowsFormat(string operatorType){
			return	string.Format("Ett okänt format på din {0} loop", operatorType);
		}

		[System.Obsolete("Method is deprecated, use new ErrorHandling system instead", true)]
		internal static string forLoopArgumentType(){
			return "Inparametrarna i din for loop kan bara vara av typen nummer";
		}
	}
}

