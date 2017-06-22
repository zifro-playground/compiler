using System;
using Runtime;
using ErrorHandler;

namespace Compiler
{

	public class SpeciallWordParser{

		public static readonly string[] keywords = {"while", "for", "in", "if", "else", "elif", "def", "return", "break", "continue"};
		public static readonly string[] scopeStarters = {"while", "for", "if", "else", "elif", "def"};

		public static readonly string[] unSupportedKeywords = {"as", "assert", "class", "del", "except",
		"exec", "finally", "from", "global", "import", "is", "lambda", "pass", "raise", "try", "with", "yield"};


		public static Logic parseSpeciallLine(Logic[] logicOrder, int lineNumber, Scope currentScope){

			switch (logicOrder[0].currentType) {

			case WordTypes.elifOperator:
			case WordTypes.ifOperator:
				return IfAndElifStatementParser.parseStatement (logicOrder, lineNumber, currentScope);

			case WordTypes.elseOperator:
				return ElseStatementParser.parseElseStatement (logicOrder, lineNumber, currentScope);

			case WordTypes.forLoop:
				return ForLoopParser.parseForLoop(logicOrder, lineNumber, currentScope);

			case WordTypes.whileLoop:
				return WhileLoopParser.parseWhileLoop(logicOrder, lineNumber, currentScope);

			case WordTypes.defStatement:
				return DefStatementParser.parseDefStatement(logicOrder, lineNumber, currentScope);
				 
			case WordTypes.returnStatement:
				return ReturnStatementParser.parseReturnStatement (logicOrder, lineNumber, currentScope);

			case WordTypes.breakStatement:
				return BreakAndContinueStatementParser.parseBreakStatement (logicOrder, lineNumber, currentScope);

			case WordTypes.continueStatement:
				return BreakAndContinueStatementParser.parseContinueStatement (logicOrder, lineNumber, currentScope);


			default:
				return new UnknownLogic(lineNumber);
			}
		}


		public static bool isKeyWord(string word){
			foreach (string s in keywords)
				if (s == word)
					return true;

			foreach (string s in unSupportedKeywords)
				if (s == word)
					return true;

			return false;
		}

		public static void checkIfUnsupportedKeyword(string word,int lineNumber){
			foreach (string s in unSupportedKeywords)
				if (s == word)
					ErrorMessage.sendErrorMessage (lineNumber, s + " stöds inte i detta spel");
		}

		public static WordTypes getSpecialType(string word, int lineNumber){

			if (word == "while")
				return WordTypes.whileLoop;

			if (word == "for")
				return WordTypes.forLoop;

			if (word == "if")
				return WordTypes.ifOperator;

			if (word == "else")
				return WordTypes.elseOperator;

			if (word == "elif")
				return WordTypes.elifOperator;

			if (word == "in")
				return WordTypes.inWord;

			if (word == "def")
				return WordTypes.defStatement;

			if (word == "return")
				return WordTypes.returnStatement;

			if (word == "break")
				return WordTypes.breakStatement;

			if (word == "continue")
				return WordTypes.continueStatement;

			ErrorMessage.sendErrorMessage (lineNumber, word + " antogs vara ett keyword, men något gick fel");
			return WordTypes.unknown;
		}


		public static bool isValidScopeStarter(Logic[] logicOrder, int lineNumber){
			WordTypes t = logicOrder [0].currentType;

			if (logicOrder[0] is ScopeStarter){
				return true;
			}


			return false;
		}

	}

}

