using System;
using System.Collections;
using System.Collections.Generic;
using Runtime;

namespace ErrorHandler
{

	public abstract class Language{


		private Dictionary<ErrorType, Dictionary<int, Func<string[], string>>> theStatements = new Dictionary<ErrorType, Dictionary<int, Func<string[], string>>> ();
		public string notFoundStatement = "Unknown Error Statement!";


		public void initLanguage(LogicOrderError logic, IfStatementError If, ElseStatementError Else, ForLoopErrors For, WhileLoopErrors While){
			insertIntoDictionary (ErrorType.ForLoop, ForErrorsOrder.getStatements (For));
			insertIntoDictionary (ErrorType.LogicOrder, LogicErrorOrder.getStatements(logic));
			insertIntoDictionary (ErrorType.IfStatements, IfErrorsOrder.getStatements(If));
			insertIntoDictionary (ErrorType.ElseStatements, ElseErrorsOrder.getStatements(Else));
			insertIntoDictionary (ErrorType.WhileLoop, WhileErrorsOrder.getStatements (While));
		}

		public void initLanguage2(IndentationErrors indent, TextErrors text, VariableErrors varen){
			insertIntoDictionary (ErrorType.Indentation, IndentationErrorsOrder.getStatements (indent));
			insertIntoDictionary (ErrorType.Text, TextErrorsOrder.getStatements (text));
			insertIntoDictionary (ErrorType.Variable, VariableErrorsOrder.getStatements (varen));
		}


		public string getErrorMessage(ErrorType theErrorType, int index, string[] args){
			if(theStatements.ContainsKey(theErrorType) == false)
				return notFoundStatement;

			Dictionary<int, Func<string[], string>> innerDict = theStatements[theErrorType];
			if(innerDict.ContainsKey(index) == false)
				return notFoundStatement;

			return innerDict [index].Invoke (args);
		}

		private void insertIntoDictionary(ErrorType theErrorType, Func<string[], string>[] funcs){
			Dictionary<int, Func<string[], string>> funcDict = new Dictionary<int, Func<string[], string>>();

			for (int i = 0; i < funcs.Length; i++)
				funcDict.Add (i, funcs [i]);

			theStatements.Add (theErrorType, funcDict);
		}
	}





	public interface ErrorSender
	{
		string getErrorMessage(ErrorType theErrorType, int index, string[] args);
		void initLanguage(LogicOrderError logicOrder, IfStatementError If, ElseStatementError Else, ForLoopErrors For, WhileLoopErrors While);
		void initLanguage2 (IndentationErrors indent, TextErrors text, VariableErrors varen);

		LogicOrderError logicOrderErrors{ get; }
		IfStatementError ifStatementErrors{ get; }
		ElseStatementError elseStatementErrors{ get; }
		ForLoopErrors forLoopErrors { get; }
		WhileLoopErrors whileLoopErrors{ get; }
		IndentationErrors indentErrors{ get; }
		TextErrors textErrors { get; }
		VariableErrors variableErrors { get; }
	}



	public enum ErrorType
	{
		LogicOrder,
		ForLoop,
		WhileLoop,
		Variable,
		Function,
		IfStatements,
		ElseStatements,
		Expression,
		Indentation,
		Text
	}
}

