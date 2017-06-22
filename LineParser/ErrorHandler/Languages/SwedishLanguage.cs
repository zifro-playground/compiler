using System;

namespace ErrorHandler
{
	public class SwedishLanguage : Language, ErrorSender
	{
		
		public VariableErrors variableErrors {
			get {return new VariabelFel ();}
		}
		public TextErrors textErrors {
			get {return new TextFel();}
		}
		public IndentationErrors indentErrors {
			get {return new IndenteringsFel ();}
		}
		public WhileLoopErrors whileLoopErrors {
			get {return new WhileLoopFel ();}
		}
		public ForLoopErrors forLoopErrors {
			get {return new ForLoopFel ();	}
		}
		public ElseStatementError elseStatementErrors {
			get {return new ElseSatserFel ();}
		}
		public LogicOrderError logicOrderErrors {
			get {return new LogiskaFel();}
		}
		public IfStatementError ifStatementErrors {
			get {return new IfSatserFel ();}
		}

	}
}

