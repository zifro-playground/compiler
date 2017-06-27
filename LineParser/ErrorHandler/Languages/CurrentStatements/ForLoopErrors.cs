using System;
using System.Collections.Generic;

namespace ErrorHandler
{
	public interface ForLoopErrors
	{
		string missingIndentOperator (string[] arg);
		string unknownFormat (string[] arg);
		string expectVariableAt2 (string[] arg);
		string expectInAt3 (string[] arg);
		string expectRangeAt4 (string[] arg);
		string rangeArgumentEmpty (string[] arg);
		string rangeArgumentNotNumber (string[] arg);
		string rangeMissingParenthesis (string[] arg);
	}



	public class ForErrorsOrder
	{
		[System.Obsolete("Use messages instead")]
		public static Func<string[], string>[] getStatements(ForLoopErrors theLogicOrder){
			List<Func<string[], string>> statements = new List<Func<string[], string>> ();

			statements.Add (theLogicOrder.missingIndentOperator);
			statements.Add (theLogicOrder.unknownFormat);
			statements.Add (theLogicOrder.expectVariableAt2);
			statements.Add (theLogicOrder.expectInAt3);
			statements.Add (theLogicOrder.expectRangeAt4);
			statements.Add (theLogicOrder.rangeArgumentEmpty);
			statements.Add (theLogicOrder.rangeArgumentNotNumber);
			statements.Add (theLogicOrder.rangeMissingParenthesis);

			return statements.ToArray ();
		}

		#region test

		public static Dictionary<ForLoopErrorType, Func<string[], string>> getMessages(ForLoopErrors theLogicOrder){
			Dictionary<ForLoopErrorType, Func<string[], string>> messages = new Dictionary<ForLoopErrorType, Func<string[], string>> ();

			messages.Add (ForLoopErrorType.missingIndentOperator, theLogicOrder.missingIndentOperator);
			messages.Add (ForLoopErrorType.unknownFormat, theLogicOrder.unknownFormat);
			messages.Add (ForLoopErrorType.expectVariableAt2, theLogicOrder.expectVariableAt2);
			messages.Add (ForLoopErrorType.expectInAt3, theLogicOrder.expectInAt3);
			messages.Add (ForLoopErrorType.expectRangeAt4, theLogicOrder.expectRangeAt4);
			messages.Add (ForLoopErrorType.rangeArgumentEmpty, theLogicOrder.rangeArgumentEmpty);
			messages.Add (ForLoopErrorType.rangeArgumentNotNumber, theLogicOrder.rangeArgumentNotNumber);
			messages.Add (ForLoopErrorType.rangeMissingParenthesis, theLogicOrder.rangeMissingParenthesis);

			return messages;
		}

		#endregion

	}
}

