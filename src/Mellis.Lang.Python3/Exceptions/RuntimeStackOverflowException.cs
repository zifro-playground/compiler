using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Exceptions
{
    public class RuntimeStackOverflowException : RuntimeException
    {
        public RuntimeStackOverflowException(string functionName, int limit = PyProcessor.CALL_STACK_LIMIT)
            : base(
                nameof(Localized_Python3_Runtime.Ex_StackOverflow),
                Localized_Python3_Runtime.Ex_StackOverflow,
                functionName,
                limit
            )
        {
        }
    }
}