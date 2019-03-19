using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Exceptions
{
    public sealed class RuntimeTooManyArgumentsException : RuntimeException
    {
        public RuntimeTooManyArgumentsException(string functionName, int maximum, int actual)
            : base(nameof(Localized_Python3_Runtime.Ex_Invoke_TooManyArguments),
                Localized_Python3_Runtime.Ex_Invoke_TooManyArguments,
                functionName, maximum, actual)
        {
            Data["functionName"] = functionName;
            Data["maximum"] = maximum;
            Data["actual"] = actual;
        }
    }
}