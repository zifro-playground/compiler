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
			//string message = currentLanguage.getErrorMessage (theErrorType, index, args);
			string message = "Message sent via old system...";
			sendErrorMessage (lineNumber, message);
		}

	#region test
		internal static void sendErrorMessage(int lineNumber, ErrorType errorType, string specificError, string[] args){
			string message = currentLanguage.getErrorMessage (errorType, specificError, args);
			sendErrorMessage (lineNumber, message);
		}
	#endregion
			
		public static void setLanguage(){
			currentLanguage = new SwedishLanguage ();
			IErrorSender theSender = (currentLanguage as IErrorSender);

			IfStatementErrors ifError = theSender.ifStatementErrors;
			ElseStatementErrors elseError = theSender.elseStatementErrors;
			ForLoopErrors forError = theSender.forLoopErrors;
			IndentationErrors indentError = theSender.indentErrors;
			TextErrors txtError = theSender.textErrors;
			VariableErrors varError = theSender.variableErrors;

			LogicErrors logicError = theSender.logicErrors;
			WhileLoopErrors whileError = theSender.whileLoopErrors;
			NumberErrors numError = theSender.numberErrors;
			KeywordErrors keywordError = theSender.keywordErrors;
			FunctionErrors funcError = theSender.functionErrors;
			OtherErrors otherError = theSender.otherErrors;

			currentLanguage.initLanguageErrors1 (ifError, elseError, forError, indentError, txtError, varError);
			currentLanguage.initLanguageErrors2 (logicError, whileError, numError, keywordError, funcError, otherError);
		}

		public static void setErrorMethod(Action<int, string> errorMethod){
			errorMessageMethod = errorMethod;
		}

	}
}

