using System;
using Runtime;

namespace Compiler
{
	public abstract class ScopeStarter : Logic {

		private Scope targetScope;
		public bool doEnterScope;
		public bool doParseLine = true;


		public void setTargetScope(Scope targetScope){
			this.targetScope = targetScope;
		}

		public Scope getTargetScope(){
			return targetScope;
		}
	}
}

