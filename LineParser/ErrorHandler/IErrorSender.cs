using System;

namespace ErrorHandler
{
	public interface IErrorSender {
		
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
}

