using System;
using System.Collections;
using System.Collections.Generic;
using Runtime;

namespace ErrorHandler
{

	public abstract class Language{


		public string notFoundStatement = "Unknown Error Statement!";

		private Dictionary<ErrorType, Dictionary<string, Func<string[], string>>> errorMessages = new Dictionary<ErrorType, Dictionary<string, Func<string[], string>>> ();


		/// Inits first part of the language errors. Stores error messages in dictionary.
		public void initLanguageErrors1(IfStatementErrors If, ElseStatementErrors Else, ForLoopErrors For, IndentationErrors Indent, TextErrors Text, VariableErrors Variable){
			errorMessages.Add (ErrorType.IfStatements, IfErrorsOrder.getMessages (If));
			errorMessages.Add (ErrorType.ElseStatements, ElseErrorsOrder.getMessages (Else));
			errorMessages.Add (ErrorType.ForLoop, ForErrorsOrder.getMessages (For));
			errorMessages.Add (ErrorType.Indentation, IndentationErrorsOrder.getMessages (Indent));
			errorMessages.Add (ErrorType.Text, TextErrorsOrder.getMessages (Text));
			errorMessages.Add (ErrorType.Variable, VariableErrorsOrder.getMessages (Variable));
		}

		/// Inits second part of the language errors. Stores error messages in dictionary.
		public void initLanguageErrors2(WhileLoopErrors While, NumberErrors Number, KeywordErrors Keyword, FunctionErrors Function, OtherErrors Other, SystemFailureErrors system) {
			errorMessages.Add (ErrorType.WhileLoop, WhileErrorsOrder.getMessages (While));
			errorMessages.Add (ErrorType.Number, NumberErrorsOrder.getMessages (Number));
			errorMessages.Add (ErrorType.Keyword, KeywordErrorsOrder.getMessages (Keyword));
			errorMessages.Add (ErrorType.Function, FunctionErrorsOrder.getMessages (Function));
			errorMessages.Add (ErrorType.Other, OtherErrorsOrder.getMessages (Other));
			errorMessages.Add (ErrorType.System, SystemFailureErrorsOrder.getMessages (system));
		}


		/// Returns a specific error message. Second parameter must be of specific error type (e.g. ForLoopErrorType) and converted to string.
		public string getErrorMessage(ErrorType theErrorType, string theSpecificError, string[] args){

			if (errorMessages.ContainsKey (theErrorType)){
				if (errorMessages [theErrorType].ContainsKey (theSpecificError))
					return errorMessages [theErrorType] [theSpecificError].Invoke (args);
			}

			return notFoundStatement;

		}

	}
}

