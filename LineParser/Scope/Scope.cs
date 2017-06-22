using System;
using System.Collections.Generic;
using B83.ExpressionParser;
using Runtime;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Compiler
{
	
	public class Scope
	{
		public ScopeType theScopeType;
		public ScopeLoop theScoopLoop;

		public int indentLevel = 0;
		public int startLine, endLine;
		public bool isReturning = false;

		public List<CodeLine> codeLines;
		public List<Scope> childScopes;
		private Scope secretParent;
		public Scope parentScope{
			get{
				return secretParent;
			}
			set{
				secretParent = value;
			}
		}

		public CurrentFunctions scopeFunctions;
		public Variables scopeVariables;
		public ExpressionParser scopeParser;
		public int lastReadLine = 0;

		public Scope(ScopeType theScopeType, int startLine, int endLine, int indentLevel, List<CodeLine> codeLines, List<Scope> childScopes, bool isClone){
			scopeParser = new ExpressionParser ();

			this.theScopeType = theScopeType;
			this.startLine = startLine;
			this.endLine = endLine;
			this.indentLevel = indentLevel;

			this.codeLines = codeLines;
			this.childScopes = childScopes;

			scopeVariables = new Variables ();
			scopeFunctions = new CurrentFunctions ();
			scopeParser = new ExpressionParser ();

			if (theScopeType == ScopeType.main || isClone)
				theScopeType = ScopeType.main;
			else
				this.codeLines.RemoveAt (0);
		}

		public void giveInheritVariables(List<Variable> inheritVariables){
			foreach (Variable var in inheritVariables) {

				int varPos = scopeVariables.containsVariable (var.name);
				if (varPos >= 0) {
					if (scopeVariables.variableList [varPos].isForLoopVariable) {
						double value = scopeVariables.variableList [varPos].getNumber();
						scopeVariables.addVariable (var, scopeParser, -1);
						scopeVariables.variableList [varPos].setValue (value);
					}
					else
						scopeVariables.addVariable (var, scopeParser, codeLines[0].lineNumber);
				}
				else
					scopeVariables.addVariable (var, scopeParser, codeLines[0].lineNumber);

			}

		}

		public void upInheritVariable(Scope targetScope){
			foreach (Variable v in scopeVariables.variableList)
				targetScope.scopeVariables.addVariable (v, scopeParser, codeLines [0].lineNumber);
		}

		public CodeLine getCurrentLine(){
			return codeLines [lastReadLine];
		}
			


		public Scope createDeepCopy(){

			List<CodeLine> tempCodeLines = new List<CodeLine> ();
			foreach (CodeLine c in codeLines)
				tempCodeLines.Add (c.cloneLine ());

			List<Scope> tempChildScopes = new List<Scope> ();
			foreach (Scope s in childScopes)
				tempChildScopes.Add (s.createDeepCopy());

			Scope tempScope = new Scope (theScopeType, startLine, endLine, indentLevel, tempCodeLines, tempChildScopes, true);


			return tempScope;
		}


		public void linkChildScopes(Scope currentScope){
			int counter = 0;
			foreach (CodeLine c in currentScope.codeLines) {
			
				if (c.logicOrder [0] is ScopeStarter) {
					(c.logicOrder [0] as ScopeStarter).setTargetScope(currentScope.childScopes [counter]);
					counter++;
				}
			}

			foreach (Scope s in currentScope.childScopes)
				linkChildScopes (s);
		}
	}
}

