using System.Collections;
using System.Collections.Generic;
using Runtime;
using ErrorHandler;

namespace Compiler{

	public class ForLoop : ScopeLoop, Loop{

		public int indentLevel;
		public double startValue = 0;
		public double endValue;
		public double incrementValue = 1;
		public Variable counterVariable;

		public ForLoop(){
			base.currentType = WordTypes.forLoop;
			base.word = "for";
			base.theComparisonType = ComparisonType.lessThen;
		}
			
		public void setLoopVariables(string counterName, double startValue, double endValue, double incrementValue){
			this.startValue = startValue;
			this.endValue = endValue;

			if (incrementValue < 0) {
				base.theComparisonType = ComparisonType.greaterThen;
				Print.print ("Changing type");
			}

			Variable leftVariable = new Variable (counterName, startValue);
			Variable checkVariable = new Variable ("ForLoopCheckValue", endValue);

			this.counterVariable = leftVariable;

			counterVariable.setValue (startValue);
			counterVariable.isForLoopVariable = true;

			base.leftValue = counterVariable;
			base.rightValue = checkVariable;
			this.incrementValue = incrementValue;
		}


		#region Loop implementation
		public void addCounterVariableToScope(int lineNumber){
			getTargetScope().scopeVariables.addVariable (counterVariable, getTargetScope().scopeParser, lineNumber);
		}

		public bool makeComparison (int lineNumber, bool doChangeValue = true)
		{
			if (counterVariable.variableType != VariableTypes.number)
				ErrorMessage.sendErrorMessage (lineNumber, ErrorType.ForLoop, ForLoopErrorType.counterVariableIsNotNumber.ToString(), new string[1] {counterVariable.name});

			if(doChangeValue)
				counterVariable.setValue (counterVariable.getNumber () + incrementValue);
			bool doLoop = ComparisonOperatorParser.makeComparison (counterVariable, base.rightValue, base.theComparisonType, lineNumber);


			getTargetScope().scopeVariables.addVariable (counterVariable, getTargetScope().scopeParser, lineNumber);
			if (doLoop == false && doChangeValue)
				counterVariable.setValue (endValue - incrementValue);

			return doLoop;
		}


		public void resetCounterVariable ()
		{
			counterVariable.setValue (startValue);
		}
		#endregion

	}

}