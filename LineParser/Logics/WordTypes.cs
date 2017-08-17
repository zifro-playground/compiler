namespace Compiler{

	public enum WordTypes{
		variable,
		functionCall,
		mathOperator,
		number,
		textString,
		booleanValue,
		booleanExpression,
		ifOperator,
		elseOperator,
		elifOperator,
		forLoop,
		whileLoop,
		defStatement,
		breakStatement,
		continueStatement,
		returnStatement,
		returnValue,
		equalSign,
		xorOperator,
		lessThenSign,
		greaterThenSign,
		notOperator,
		andOperator,
		orOperator,
		speciallWord,
		inWord,
		indentOperator,
		commaSign,
		package, // A package is a compressed parameter example: (x + 23 - 3)
		nill,
		unknown,
	};
	
}