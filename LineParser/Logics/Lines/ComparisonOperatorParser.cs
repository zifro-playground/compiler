using System;
using ErrorHandler;

namespace Compiler
{
	public class ComparisonOperatorParser{

		public static ComparisonType parseOperators(Logic[] logicOrder){

			if (logicOrder.Length > 2 || logicOrder.Length == 0) 
				return ComparisonType.unknown;


			if (logicOrder.Length == 1)
				return parseSingleOperator (logicOrder [0]);

			if (logicOrder.Length == 2)
				return parseSDoubleOperator (logicOrder);

			return ComparisonType.unknown;
		}

		static ComparisonType parseSDoubleOperator(Logic[] theOperators){
			if (theOperators[1].currentType != WordTypes.equalSign)
				return ComparisonType.unknown;


			if (theOperators[0].currentType == WordTypes.equalSign)
				return ComparisonType.equalsTo;

			if (theOperators[0].currentType == WordTypes.lessThenSign)
				return ComparisonType.lessThenOrEqualsTo;

			if (theOperators[0].currentType == WordTypes.greaterThenSign)
				return ComparisonType.greaterThenOrEqaulsTo;

			if (theOperators[0].currentType == WordTypes.xorOperator)
				return ComparisonType.notEqualsTo;


			return ComparisonType.unknown;
		}


		static ComparisonType parseSingleOperator(Logic theOperator){
			if (theOperator.currentType == WordTypes.lessThenSign)
				return ComparisonType.lessThen;

			if (theOperator.currentType == WordTypes.greaterThenSign)
				return ComparisonType.greaterThen;

			return ComparisonType.unknown;
		}




		public static bool checkSumsToOperator(VariableTypes theSum, ComparisonType theOperator, int lineNumber){
			if (theSum == VariableTypes.number)
				return true;

			if (theSum == VariableTypes.boolean) {
				if (theOperator == ComparisonType.equalsTo || theOperator == ComparisonType.notEqualsTo)
					return true;
			}

			if (theSum == VariableTypes.textString) {
				if (theOperator == ComparisonType.equalsTo || theOperator == ComparisonType.notEqualsTo)
					return true;
			}


			ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, "Okänd operator kombination av " + theSum);
			return false;
		}

		public static bool makeComparison(Variable var1, Variable var2, ComparisonType theOperator, int lineNumber){

			if (var1.variableType == VariableTypes.unknown || var1.variableType == VariableTypes.unsigned || var2.variableType == VariableTypes.unknown || var2.variableType == VariableTypes.unsigned)
				ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, "En eller flera av operatorerna går inte att tyda");


			if (var1.variableType != var2.variableType)
				ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, "Kan inte utföra jämförelse mellan " + var1.variableType + " och " + var2.variableType);


			if (theOperator == ComparisonType.equalsTo) {
				if (var1.variableType == VariableTypes.boolean) {
					if (var1.getBool () == var2.getBool ())
						return true;
					else
						return false;
				}

				if (var1.variableType == VariableTypes.number) {
					if (var1.getNumber () == var2.getNumber ())
						return true;
					else
						return false;
				}

				if (var1.variableType == VariableTypes.textString) {
					if (var1.getString () == var2.getString ())
						return true;
					else
						return false;
				}
			}

			if (theOperator == ComparisonType.notEqualsTo) {
				if (var1.variableType == VariableTypes.boolean) {
					if (var1.getBool () != var2.getBool ())
						return true;
					else
						return false;
				}

				if (var1.variableType == VariableTypes.number) {
					if (var1.getNumber () != var2.getNumber ())
						return true;
					else
						return false;
				}

				if (var1.variableType == VariableTypes.textString) {
					if (var1.getString () != var2.getString ())
						return true;
					else
						return false;
				}
			}

			if (theOperator == ComparisonType.greaterThenOrEqaulsTo && var1.variableType == VariableTypes.number){
				if (var1.getNumber () >= var2.getNumber ())
					return true;
				else
					return false;
			}

			if (theOperator == ComparisonType.lessThenOrEqualsTo && var1.variableType == VariableTypes.number){
				if (var1.getNumber () <= var2.getNumber ())
					return true;
				else
					return false;
			}
			if (theOperator == ComparisonType.lessThen && var1.variableType == VariableTypes.number){
				if (var1.getNumber () < var2.getNumber ())
					return true;
				else
					return false;
			}
			if (theOperator == ComparisonType.greaterThen && var1.variableType == VariableTypes.number){
				if (var1.getNumber () > var2.getNumber ())
					return true;
				else
					return false;
			}


			ErrorMessage.sendErrorMessage (lineNumber, ErrorType.Logic, LogicErrorType.unknownLogic.ToString(), null);
			return false;
		}
	}

}

