using System;
using System.Collections.Generic;

namespace Compiler
{
	public class WordsToLogicParser{


		public static Logic[] determineLogicFromWords(string[] words, int lineNumber, Scope currentScope){
			Logic[] logicOrder = new Logic[words.Length];

			for (int i = 0; i < words.Length; i++) {
				logicOrder [i] = getCurrentLogic (words [i], lineNumber, currentScope);
				if (logicOrder [i].currentType == WordTypes.package) {
					PackageUnWrapper.unpackPackage (words [i], lineNumber, currentScope, logicOrder [i] as Package);
				}
			}

			logicOrder = PostLogicParsing.parsePostLogics (logicOrder, lineNumber, currentScope);

			return logicOrder;
		}



		private static Logic getCurrentLogic(string word, int lineNumber, Scope currentScope){

			if (word == ",")
				return new CommaSign();

			if (word == ">")
				return new GreaterThenSign();

			if (word == "<")
				return new LessThenSign();

			if (word == ":")
				return new IndentOperator();

			if (word == "!")
				return new XorOperator (word);

			if (word == "not")
				return new NotOperator (word);

			if (word == "or")
				return new OrOperator (word);

			if (word == "and")
				return new AndOperator (word);
			

			if (isBooleanValue (word) == 1)
				return new BooleanValue (true);
			if (isBooleanValue (word) == 0)
				return new BooleanValue (false);


			if (isNumber (word))
				return new NumberValue(word);

			if (isString (word))
				return new TextValue (word);


			if (SpecialWordParser.isKeyWord (word)) {
				SpecialWordParser.checkIfUnsupportedKeyword (word, lineNumber);
				WordTypes specialType = SpecialWordParser.getSpecialType (word, lineNumber);

				if (specialType == WordTypes.forLoop)
					return new ForLoop ();

				if (specialType == WordTypes.whileLoop)
					return new WhileLoop ();

				if (specialType == WordTypes.ifOperator)
					return new IfStatement ();

				if (specialType == WordTypes.elifOperator)
					return new ElifStatement ();

				if (specialType == WordTypes.elseOperator)
					return new ElseStatement ();

				if (specialType == WordTypes.defStatement)
					return new DefStatement ();

				if (specialType == WordTypes.returnStatement)
					return new ReturnStatement ();
				
				if (specialType == WordTypes.breakStatement)
					return new BreakStatement ();

				if (specialType == WordTypes.continueStatement)
					return new ContinueStatment ();
			}



			if (startsWithDigitOrWeirdChar(word) == false) {

				if (endsWithParantes (word)) {
					Logic temp = FunctionParser.parseIntoFunctionCall (word, lineNumber, currentScope);
					if (temp != null)
						return temp;
				} 
				else {
					if(containsButNotStartWithDigitWeirdChar(word) == false)
						return checkVariable(word, currentScope.scopeVariables);
				}


			}

			if (word.Length == 1) {
				if (word [0] == '=')
					return new EqualSign ();
				if(isMathOperator(word[0]))
					return new MathOperator(word);
			}

			if (isPackage (word)) {

				return new Package(word, lineNumber);
			}

			return new UnknownLogic (lineNumber);
		}


		private static Logic checkVariable(string word, Variables scopeVariables){

			int variablePos = scopeVariables.containsVariable (word);
			if (variablePos >= 0) {
				return scopeVariables.variableList [variablePos];
			}

			return new Variable (word);
		}



		private static int isBooleanValue(string word){
			if (word == "True")
				return 1;
			if (word == "False")
				return 0;

			return -1;
		}


		private static bool isNumber(string word){
			double testNumber = 0;

			return double.TryParse (word, out testNumber);
		}

		private static bool isMathOperator(char c){
			for(int i = 0; i < MathParser.mathOperators.Length; i++)
				if(c == MathParser.mathOperators[i])
					return true;

			return false;
		}


		private static bool startsWithDigitOrWeirdChar(string word){
			if(char.IsLetter(word[0]))
				return false;

			if (word [0] == '_')
				return false;

			return true;
		}

		private static bool containsButNotStartWithDigitWeirdChar(string word){
			if (word.Length == 1)
				return false;

			for (int i = 1; i < word.Length; i++)
				if (!char.IsLetter (word [i]) && !char.IsDigit (word [i]) && word [i] != '_')
					return true;

			return false;
		}

		private static bool endsWithParantes(string word){
			if(word[word.Length-1] == ')')
				return true;

			return false;
		}


		private static bool isPackage(string word){
			if (word [0] == '(' && word [word.Length - 1] == ')')
				return true;

			return false;
		}

		private static bool isString(string word){
			if (word [0] == '"' && word [word.Length - 1] == '"')
				return true;

			return false;
		}

	}

}

