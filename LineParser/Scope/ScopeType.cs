using System;

namespace Compiler{

	
	public enum ScopeType{
		function,
		forLoop,
		whileLoop,
		ifStatement,
		elseStatement,
		elifStatement,
		main,
		unknown
	}
}