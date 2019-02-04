using System;
using System.Collections.Generic;
using System.Text;

namespace Zifro.Compiler.Core.Exceptions
{
    public class RuntimeException : InterpreterException
    {
        public string LocalizeKey { get; }

        public RuntimeException(string localizeKey,
            string localizedMessage, Exception innerException)
            : base(localizedMessage, innerException)
        {
            LocalizeKey = localizeKey;
        }

        public RuntimeException(string localizeKey, string localizedMessage)
            : this(localizeKey, localizedMessage, innerException: null)
        {
        }

        public RuntimeException(string localizeKey, string localizedMessageFormat,
            params object[] values)
            : this(localizeKey, string.Format(localizedMessageFormat, values), innerException: null)
        {
        }
    }
}