using System;
using System.Collections;
using System.Collections.Generic;
using Runtime;

namespace ErrorHandler
{

	public abstract class Language{


		public string notFoundStatement = "Unknown Error Statement!";
		/*private Dictionary<ErrorType, Dictionary<int, Func<string[], string>>> theStatements = new Dictionary<ErrorType, Dictionary<int, Func<string[], string>>> ();


		public void initLanguage(LogicErrors logic, IfStatementErrors If, ElseStatementErrors Else, ForLoopErrors For, WhileLoopErrors While){
			//insertIntoDictionary (ErrorType.ForLoop, ForErrorsOrder.getStatements (For));
			//insertIntoDictionary (ErrorType.Logic, LogicErrorsOrder.getStatements(logic));
			//insertIntoDictionary (ErrorType.IfStatements, IfErrorsOrder.getStatements(If));
			//insertIntoDictionary (ErrorType.ElseStatements, ElseErrorsOrder.getStatements(Else));
			//insertIntoDictionary (ErrorType.WhileLoop, WhileErrorsOrder.getStatements (While));
		}

		public void initLanguage2(IndentationErrors indent, TextErrors text, VariableErrors varen){
			//insertIntoDictionary (ErrorType.Indentation, IndentationErrorsOrder.getStatements (indent));
			//insertIntoDictionary (ErrorType.Text, TextErrorsOrder.getStatements (text));
			//insertIntoDictionary (ErrorType.Variable, VariableErrorsOrder.getStatements (varen));
		}


		public string getErrorMessage(ErrorType theErrorType, int index, string[] args){
			if(theStatements.ContainsKey(theErrorType) == false)
				return notFoundStatement;

			Dictionary<int, Func<string[], string>> innerDict = theStatements[theErrorType];
			if(innerDict.ContainsKey(index) == false)
				return notFoundStatement;

			return innerDict [index].Invoke (args);

		private void insertIntoDictionary(ErrorType theErrorType, Func<string[], string>[] funcs){
			Dictionary<int, Func<string[], string>> funcDict = new Dictionary<int, Func<string[], string>>();
			
			for (int i = 0; i < funcs.Length; i++)
				funcDict.Add (i, funcs [i]);
			
			theStatements.Add (theErrorType, funcDict);
		}
		}*/

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
		public void initLanguageErrors2(LogicErrors Logic, WhileLoopErrors While, NumberErrors Number, KeywordErrors Keyword, FunctionErrors Function, OtherErrors Other) {
			errorMessages.Add (ErrorType.Logic, LogicErrorsOrder.getMessages (Logic));
			errorMessages.Add (ErrorType.WhileLoop, WhileErrorsOrder.getMessages (While));
			errorMessages.Add (ErrorType.Number, NumberErrorsOrder.getMessages (Number));
			errorMessages.Add (ErrorType.Keyword, KeywordErrorsOrder.getMessages (Keyword));
			errorMessages.Add (ErrorType.Function, FunctionErrorsOrder.getMessages (Function));
			errorMessages.Add (ErrorType.Other, OtherErrorsOrder.getMessages (Other));
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

