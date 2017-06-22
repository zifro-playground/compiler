using System;
using System.Collections.Generic;
using Runtime;

namespace Compiler
{
	public class UserFunction : Function {

		public Scope targetScope;

		public UserFunction(string name, Scope targetScope, int inputParaAmount){
			base.name = name;
			this.targetScope = targetScope;
			base.inputParameterAmount.Add (inputParaAmount);
			base.isUserFunction = true;
		}

		#region implemented abstract members of Function

		public override Variable runFunction (Scope currentScope, Variable[] inputParas, int lineNumber)
		{
			Scope targetClone = targetScope.createDeepCopy ();
			ScopeReturnParser.parseScopeReturns (targetClone, targetClone);
			targetClone.linkChildScopes (targetClone);

			for (int i = 0; i < base.inputParameterNames.Length && i < inputParas.Length; i++) {
				inputParas [i].name = base.inputParameterNames [i];
				Print.print ("Adding: " + inputParas [i].name + " : " + inputParas [i].getNumber ());
				targetClone.scopeVariables.addVariable (inputParas [i], targetClone.scopeParser, lineNumber);
			}

			foreach (Function f in currentScope.scopeFunctions.getShallowCopy())
				targetClone.scopeFunctions.addFunction (f);



			Runtime.CodeWalker.switchToUserFunc (currentScope, targetClone);
			return new Variable("Return Variable");
		}

		#endregion

	}

}

