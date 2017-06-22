using System;

namespace Compiler
{
	public interface ComparisonScope
	{
		void setEnterScope(bool value);
		void setParseLine(bool value);
		void initNextstatement();
		void linkNextStatement(ComparisonScope nextScope);
	}
}

