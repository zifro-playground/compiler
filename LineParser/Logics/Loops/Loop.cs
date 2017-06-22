using System.Collections;

namespace Compiler{
	
	public interface Loop{
		bool makeComparison (int lineNumber, bool changeValue = true);
		void addCounterVariableToScope (int lineNumber);
		void resetCounterVariable();
	}

}
