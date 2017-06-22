using System;
using System.Collections.Generic;

namespace Compiler
{
	public class GameFunctions
	{
		internal static List<Function> gameFunctions;

		public static void setGameFunctions(List<Function> inputList){
			gameFunctions = inputList;
		}
	}
}

