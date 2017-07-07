using System;
using ErrorHandler;

namespace Compiler
{
	public class InternalParseFunctions
	{

		public static Logic[] getSubArray(Logic[] logicOrder, int startIndex, int endIndex, int lineNumber){
			if (startIndex < logicOrder.Length && endIndex < logicOrder.Length) {
				Logic[] subArray = new Logic[endIndex - startIndex + 1];
				for (int i = startIndex; i <= endIndex; i++)
					subArray [i - startIndex] = logicOrder [i];

				return subArray;
			} else
				ErrorMessage.sendErrorMessage (lineNumber, ErrorType.Logic, LogicErrorType.corruptAndOrStatement.ToString(), null);

			return null;
		}

	}
}

