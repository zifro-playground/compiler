using System;

namespace ErrorHandler
{
	public enum ForLoopErrorType {
		missingIndentOperator,
		unknownFormat,
		expectVariableAt2,
		expectInAt3,
		expectRangeAt4,
		rangeArgumentEmpty,
		rangeArgumentNotNumber,
		rangeMissingParenthesis
	}
}

