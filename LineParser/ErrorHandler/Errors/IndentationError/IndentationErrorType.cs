using System;

namespace ErrorHandler
{
	public enum IndentationErrorType
	{
		unknownIndentStarter,
		firstLineIndentError,
		indentationError,
		expectingBodyAfterScopeStarter
	}
}

