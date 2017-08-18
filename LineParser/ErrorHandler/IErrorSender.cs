using System;

namespace ErrorHandler
{
	public interface IErrorSender {
		
		string getErrorMessage(ErrorType theErrorType, string theSpecificError, string[] args);
		void initLanguageErrors1(IfStatementErrors If, ElseStatementErrors Else, ForLoopErrors For, IndentationErrors Indent, TextErrors Text, VariableErrors Variable);
		void initLanguageErrors2(WhileLoopErrors While, NumberErrors Number, KeywordErrors Keyword, FunctionErrors Function, OtherErrors Other, SystemFailureErrors System);

		IfStatementErrors ifStatementErrors{ get; }
		ElseStatementErrors elseStatementErrors{ get; }
		ForLoopErrors forLoopErrors { get; }
		IndentationErrors indentErrors{ get; }
		TextErrors textErrors { get; }
		VariableErrors variableErrors { get; }

		WhileLoopErrors whileLoopErrors{ get; }
		NumberErrors numberErrors { get; }
		KeywordErrors keywordErrors { get; }
		FunctionErrors functionErrors { get; }
		OtherErrors otherErrors { get; }
		SystemFailureErrors systemErrors { get; }
	}
}

