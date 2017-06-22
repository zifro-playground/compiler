using System;
using ErrorHandler;
using Runtime;

namespace Compiler 
{
	public class ReturnStatement : Logic
	{
		public Logic[] followOrder;
		public Scope FunctionParent;


		public ReturnStatement ()
		{
			base.currentType = WordTypes.returnStatement;
			base.word = "return";
		}


		public void findFunctionParent(Scope currentScope, int lineNumber){
			if (currentScope.theScopeType == ScopeType.function) {
				return;
			}
			else if(currentScope.theScopeType == ScopeType.main)
				ErrorMessage.sendErrorMessage (lineNumber, "Du kan inte returnera från main");


			if(currentScope.parentScope == null)
				ErrorMessage.sendErrorMessage (lineNumber, "Scope saknar anropare \n" + currentScope.theScopeType);


			Scope parentScope = currentScope.parentScope;
			while (parentScope.theScopeType != ScopeType.function) {
				parentScope = parentScope.parentScope;
			}
			FunctionParent = parentScope;
		}


	}
}

