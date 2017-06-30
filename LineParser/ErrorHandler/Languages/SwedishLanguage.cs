using System;

namespace ErrorHandler
{
	public class SwedishLanguage : Language, IErrorSender
	{
		
		public IfStatementErrors ifStatementErrors {
			get {return new IfSatserFel ();}
		}

		public ElseStatementErrors elseStatementErrors {
			get {return new ElseSatserFel ();}
		}

		public ForLoopErrors forLoopErrors {
			get {return new ForLoopFel ();	}
		}

		public IndentationErrors indentErrors {
			get {return new IndenteringsFel ();}
		}

		public TextErrors textErrors {
			get {return new TextFel();}
		}

		public VariableErrors variableErrors {
			get {return new VariabelFel ();}
		}



		public LogicErrors logicErrors {
			get {return new LogiskaFel();}
		}

		public WhileLoopErrors whileLoopErrors {
			get {return new WhileLoopFel ();}
		}

		public NumberErrors numberErrors {
			get {return new NummerFel ();}
		}

		public KeywordErrors keywordErrors {
			get {return new KeywordFel ();}
		}

		public FunctionErrors functionErrors {
			get {return new FunktionFel ();}
		}

		public OtherErrors otherErrors {
			get {return new AndraFel ();}
		}
	}
}

