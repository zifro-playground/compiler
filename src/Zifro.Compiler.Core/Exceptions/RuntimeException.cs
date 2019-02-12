using System;
using System.Collections.Generic;
using System.Text;

namespace Zifro.Compiler.Core.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Use this for exceptions that occur during runtime. From DivideByZero and TypeError to simple AssertionError.
    /// </summary>
    public class RuntimeException : InterpreterLocalizedException
    {
        public RuntimeException(string localizeKey, string localizedMessageFormat, params object[] values)
            : base(localizeKey, localizedMessageFormat, values)
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