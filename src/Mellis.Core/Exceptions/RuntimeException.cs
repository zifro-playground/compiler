using System;

namespace Mellis.Core.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Use this for exceptions that occur during runtime. From DivideByZero and TypeError to simple AssertionError.
    /// </summary>
    public class RuntimeException : InterpreterLocalizedException
    {
        public RuntimeException(string localizeKey, string localizedMessageFormat, params object[] formatArgs)
            : base(localizeKey, localizedMessageFormat, formatArgs)
        {
        }

        public RuntimeException(string localizeKey, string localizedMessage, Exception innerException)
            : base(localizeKey, localizedMessage, innerException)
        {
        }

        public RuntimeException(string localizeKey, string localizedMessage)
            : base(localizeKey, localizedMessage)
        {
        }
    }
}