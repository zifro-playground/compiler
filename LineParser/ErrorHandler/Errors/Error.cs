using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public class Error
	{
		private ForLoopErrorType forType;
		private TextErrorType textType;

		public Error (TextErrorType eType)
		{
			textType = eType;
		}

		public Error (ForLoopErrorType eType)
		{
			forType = eType;
		}

		/*public Error (TextErrorType eType)
		{
			errorType = eType;
		}*/
	}
}

