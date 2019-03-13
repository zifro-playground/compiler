using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Exceptions
{
    public sealed class RuntimeTooFewArgumentsException : RuntimeException
    {
        public RuntimeTooFewArgumentsException(string functionName, int minimum, int actual)
            : base(nameof(Localized_Python3_Runtime.Ex_Invoke_TooFewArguments),
                Localized_Python3_Runtime.Ex_Invoke_TooFewArguments,
                functionName, minimum, actual)
        {
            Data["functionName"] = functionName;
            Data["minimum"] = minimum;
            Data["actual"] = actual;
        }
    }
}