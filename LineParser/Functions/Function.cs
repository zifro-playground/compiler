using System;
using System.Collections.Generic;

namespace Compiler
{
	public abstract class Function{

		public string name;
		public string buttonText;
		public bool hasReturnVariable;
		public bool pauseWalker;
		public List<int> inputParameterAmount = new List<int>();
		public string[] inputParameterNames = new string[10];
		public Scope functionScope;
		public bool isUserFunction = false;


		public abstract Variable runFunction (Scope currentScope, Variable[] inputParas, int lineNumber);
	}

}

