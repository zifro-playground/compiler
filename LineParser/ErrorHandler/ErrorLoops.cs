using System;

namespace ErrorHandler
{
	public class ErrorLoops
	{
		internal static string missingIndentOperator(string operatorType){
			return	string.Format("Det saknas ett \":\" i slutet på din {0} loop", operatorType);
		}

		internal static string unknowsFormat(string operatorType){
			return	string.Format("Ett okänt format på din {0} loop", operatorType);
		}


		internal static string forLoopArgumentType(){
			return "Inparametrarna i din for loop kan bara vara av typen nummer";
		}
	}
}

