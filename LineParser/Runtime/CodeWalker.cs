using Compiler;
using ErrorHandler;
using System;

namespace Runtime
{
	public class CodeWalker
	{
		private static Scope currentScope; 
		private static int currentLineIndex = 0;
		private static Scope returnTarget;
		private static bool isReturning = false;

		public static bool switchedToUserFunc = false;

		internal static Action doEndWalker, pauseWalker, activateFunctionColor;
		static Action<int> showCurrentActiveCodeLine;

		#region initWalker
		/// <summary>
		/// Setup for CodeWalker.
		/// </summary>
		public static void setActions(Action endWalker, Action walkerPause, Action funcColor, Action<int> showCurrentLine, Scope main){
			doEndWalker = endWalker;
			pauseWalker = walkerPause;
			activateFunctionColor = funcColor;
			showCurrentActiveCodeLine = showCurrentLine;
			currentScope = main;
			currentLineIndex = 0;
		}
		#endregion


		#region runtime
		public static void parseLine(){
			if (handleReturn ())
				return;

			if (currentLineIndex >= currentScope.codeLines.Count)
				if (currentScope.parentScope == null) {
					doEndWalker.Invoke ();
					return;
				}
				else {
					SetFinalScopeCommands (false, currentScope.codeLines[currentScope.codeLines.Count-1].lineNumber);
					return;
				}
				
			currentScope.lastReadLine = currentLineIndex;
			CodeLine currentLine = currentScope.getCurrentLine ();

			if (currentLine.logicOrder [0] is ScopeStarter) {

				//Print.print (currentLine.logicOrder [0].currentType.ToString() + ":  " + (currentLine.logicOrder [0] as ScopeStarter).doParseLine);
				if ((currentLine.logicOrder [0] as ScopeStarter).doParseLine == false) {
					currentLineIndex++;
					parseLine();
					return;
				}
			}

			showCurrentActiveCodeLine.Invoke (currentLine.lineNumber);

		

			try{
				currentLine.theCommandType = parseCommandType (currentLine.lineNumber, currentScope);
			}
			catch(Exception e){
				if (e is FunctionCallException)
					return;	
				throw e;
			}
			ReturnMemoryClear.clearLineMemory (currentLine);

			//Enter New Scope
			if ((currentLine.logicOrder[0] is ScopeStarter) && NTScopeStarter (currentLine))
				return;

			VariableWindow.sendStackVariables (currentScope);
			currentLineIndex++;
		}
		#endregion


		#region internal public methods
		internal static void switchToUserFunc(Scope current, Scope target){
			switchedToUserFunc = true;
			target.parentScope = current;
			setScopeToParse (target, current);
		}
		
		internal static void setScopeToParse(Scope newScope, Scope parentScope){
			newScope.parentScope = parentScope;
			currentScope = newScope;
			currentLineIndex = 0;
		}

		internal static void breakLoop(Scope targetLoop, int lineNumber){
			currentScope.upInheritVariable (targetLoop);
			currentScope = targetLoop;
			SetFinalScopeCommands (true, lineNumber);
			throw new FunctionCallException();
		}

		internal static void continueLoop(Scope targetLoop, int lineNumber){
			currentScope.upInheritVariable (targetLoop);
			currentScope = targetLoop;
			SetFinalScopeCommands (false, lineNumber);
			throw new FunctionCallException();
		}
		#endregion



		#region runtime backend logic

		#region finalCommands
		private static void SetFinalScopeCommands(bool forceQuit, int lineNumber){

			if ((currentScope.theScopeType == ScopeType.forLoop || currentScope.theScopeType == ScopeType.whileLoop)) {
				bool doLoopAgain = forceQuit ? false : (currentScope.theScoopLoop as Loop).makeComparison (currentScope.codeLines [0].lineNumber);
				if (doLoopAgain) {
					currentLineIndex = 0;
					return;
				}
			} 
				
			if (currentScope.theScopeType != ScopeType.main) {
				currentScope = findTargetParentScope (currentScope, lineNumber);
				currentLineIndex = currentScope.lastReadLine + 1;
				parseLine ();
			} 
			else
				doEndWalker.Invoke ();
		}


		private static Scope findTargetParentScope(Scope currentScope, int lineNumber){
			if (currentScope.parentScope == null)
				ErrorMessage.sendErrorMessage (lineNumber, "Parent scope is missing");

			if (currentScope.theScopeType != ScopeType.function) 
				currentScope.upInheritVariable (currentScope.parentScope);

			if (currentScope.parentScope.lastReadLine != currentScope.parentScope.codeLines.Count - 1 || currentScope.parentScope.theScopeType == ScopeType.main)
				return currentScope.parentScope;

			if (currentScope.parentScope.theScoopLoop != null && (currentScope.parentScope.theScoopLoop as Loop).makeComparison (lineNumber)) {
				currentScope.parentScope.lastReadLine = -1;
				return currentScope.parentScope;
			}

			return findTargetParentScope (currentScope.parentScope, lineNumber);
		}
		#endregion


		private static Logic parseCommandType(int lineNumber, Scope currentScope){
			CodeLine currentLine = currentScope.getCurrentLine ();
			Logic[] logicOrder = currentLine.getLatestOrder();
			checkForunknown (logicOrder, lineNumber);

			Logic result;
			result = VariableDeclareParser.checkForVariableDecleration (logicOrder, lineNumber, currentScope);
			if(result.currentType != WordTypes.unknown)
				return result;

			result = SpecialWordParser.parseSpeciallLine (logicOrder, lineNumber, currentScope);
			if(result.currentType != WordTypes.unknown)
				return result;

			result = PureFunctionCallParser.parsePureFunctionCall (logicOrder, lineNumber, currentScope);
			if(result.currentType != WordTypes.unknown)
				return result;

			ErrorMessage.sendErrorMessage (lineNumber, ErrorType.LogicOrder, 0, null);
			return new UnknownLogic(lineNumber);
		}

		private static void checkForunknown(Logic[] logicOrder, int lineNumber){
			foreach (Logic L in logicOrder)
				if (L.currentType == WordTypes.unknown)
					ErrorMessage.sendErrorMessage (lineNumber, ErrorType.LogicOrder, 0, null);
		}

		private static bool NTScopeStarter(CodeLine tempLine){
			Logic currentScopeStarter = tempLine.theCommandType;


			if((currentScopeStarter as ScopeStarter).doEnterScope){
				Scope targetScope = (currentScopeStarter as ScopeStarter).getTargetScope();
				targetScope.scopeFunctions.savedFunctions = currentScope.scopeFunctions.getShallowCopy();
				targetScope.giveInheritVariables (currentScope.scopeVariables.variableList);
				setScopeToParse (targetScope, currentScope);
				return true;
			}
			return false;
		}


		private static bool handleReturn(){
			if (isReturning == false)
				return false;

			currentLineIndex = returnTarget.lastReadLine;
			currentScope = returnTarget;

			isReturning = false;
			return true;
		}

		public static void setReturnTarget(Scope newReturnTarget){
			isReturning = true;
			returnTarget = newReturnTarget;
		}

		#endregion


	}
}

