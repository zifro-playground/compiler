using System;
using System.Collections.Generic;
using ErrorHandler;
using Runtime;


namespace Compiler
{
	public class BoolCompressExpressions
	{

		public static Logic[] compressExpression(Logic[] preLogicOrder, int lineNumber, Scope currentScope, bool solveExpressions){
			Logic[] logicOrder = splitAtAndOr ((Logic[])preLogicOrder.Clone (), lineNumber);

			foreach (Logic l in logicOrder)
				if (l is Package && hasValidComparison ((l as Package).logicOrder, lineNumber))
						(l as Package).logicOrder = new Logic[1]{ new BooleanExpression ((l as Package).logicOrder, lineNumber, currentScope) };


			return logicOrder;
		}


		private static bool hasValidComparison(Logic[] logicOrder, int lineNumber){
			int foundCounter = 0;
			bool foundComparison = false;

			for (int i = 0; i < logicOrder.Length; i++) {
				if (logicOrder [i] is ComparisonOperator)
					foundCounter++;
				else
					foundCounter = 0;

				if (foundCounter > 0) {
					foundComparison = true;
					if (foundCounter > 2)
						ErrorMessage.sendErrorMessage (lineNumber, "Kombinationen: " + logicOrder [i - 2].word + logicOrder [i - 1].word + logicOrder [i].word + " är inte tillåten");
				}
			}
			if (logicOrder [0] is ComparisonOperator)
				ErrorMessage.sendErrorMessage (lineNumber, "Ett boolskt utryck kan inte börja med en jämförelseoperator");
			if (logicOrder [logicOrder.Length-1] is ComparisonOperator)
				ErrorMessage.sendErrorMessage (lineNumber, "Ett boolskt utryck kan inte sluta med en jämförelseoperator");

			return foundComparison;
		}


		public static bool hasAndOr(Logic[] logicOrder, int lineNumber){
			foreach (Logic l in logicOrder)
				if (l is AndOrOperator)
					return true;
			return false;
		}

		private static Logic[] splitAtAndOr(Logic[] logicOrder, int lineNumber){
			if(hasAndOr(logicOrder, lineNumber) == false)
				return new Logic[1]{ new Package(logicOrder, lineNumber)};

			List<Logic> compressedOrder = new List<Logic> ();
			int lastFound = 0;

			for(int i = 0; i < logicOrder.Length; i++){
				if (logicOrder [i] is AndOrOperator) {
					if (i == 0 || i == logicOrder.Length - 1)
						ErrorMessage.sendErrorMessage (lineNumber, "Kan inte börja eller sluta med And/Or");

					Logic[] subArray = InternalParseFunctions.getSubArray (logicOrder, lastFound, i - 1, lineNumber);
					compressedOrder.Add (new Package (subArray, lineNumber));

					if (logicOrder [i + 1] is AndOrOperator)
						ErrorMessage.sendErrorMessage (lineNumber, "Kan inte ha And/Or direkt efter varandra");

					lastFound = i + 1;
					compressedOrder.Add (logicOrder [i]);
				} 
			}
			Logic[] endArray = InternalParseFunctions.getSubArray (logicOrder, lastFound, logicOrder.Length-1, lineNumber);
			compressedOrder.Add (new Package (endArray, lineNumber));


			return compressedOrder.ToArray ();
		}

	}
}

