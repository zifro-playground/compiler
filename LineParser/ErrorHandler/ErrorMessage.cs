using System;
using Runtime;

namespace ErrorHandler
{

	public class ErrorMessage
	{
		public static Action<int, string> errorMessageMethod;
		internal static Language currentLanguage;

		internal static void sendErrorMessage(int lineNumber, string message){
			errorMessageMethod.Invoke (lineNumber, message);
		}


		internal static void sendErrorMessage(int lineNumber, ErrorType theErrorType, int index, string[] args){
			string message = currentLanguage.getErrorMessage (theErrorType, index, args);
			sendErrorMessage (lineNumber, message);
		}

		#region test
		internal static void sendErrorMessage(int lineNumber, ErrorType errorType, ForLoopErrorType specificError, string[] args){
			string message = currentLanguage.getErrorMessage (errorType, specificError, args);
			sendErrorMessage (lineNumber, message);
		}
		#endregion
			
		public static void setLanguage(){
			currentLanguage = new SwedishLanguage ();
			IErrorSender theSender = (currentLanguage as IErrorSender);

			LogicOrderError logicError = theSender.logicOrderErrors;
			IfStatementError ifError = theSender.ifStatementErrors;
			ElseStatementError elseError = theSender.elseStatementErrors;
			ForLoopErrors forError = theSender.forLoopErrors;
			WhileLoopErrors whileError = theSender.whileLoopErrors;
			currentLanguage.initLanguage (logicError, ifError, elseError, forError, whileError);

			IndentationErrors indentError = theSender.indentErrors;
			TextErrors txtError = theSender.textErrors;
			VariableErrors varError = theSender.variableErrors;
			currentLanguage.initLanguage2 (indentError, txtError, varError);

			currentLanguage.initLanguageErrors (logicError, ifError, elseError, forError, whileError, indentError, txtError, varError);
		}

		public static void setErrorMethod(Action<int, string> errorMethod){
			errorMessageMethod = errorMethod;
		}

	}
}

