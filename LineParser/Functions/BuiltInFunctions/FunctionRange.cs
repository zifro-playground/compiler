using System;
using System.Collections.Generic;
using Compiler;

namespace Compiler
{
	internal class FunctionRange : Function  {

		public FunctionRange(){
			base.name = "range";
			base.hasReturnVariable = false;
			base.inputParameterAmount.Add (1);
			base.inputParameterAmount.Add (2);
			base.inputParameterAmount.Add (3);
			base.pauseWalker = false;
		}




		#region implemented abstract members of Function
		public override Variable runFunction (Scope currentScope, Variable[] inputParas, int lineNumber)
		{
			throw new System.NotImplementedException ();
		}
		#endregion
	}

}

