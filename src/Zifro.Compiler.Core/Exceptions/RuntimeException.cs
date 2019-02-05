using System;
using System.Collections.Generic;
using System.Text;

namespace Zifro.Compiler.Core.Exceptions
{
    public class RuntimeException : InterpreterException
    {
        public string LocalizeKey { get; }
        public object[] FormatArgs { get; }

        private RuntimeException(string localizeKey,
            string localizedMessage, object[] formatArgs, Exception innerException)
            : base(localizedMessage, innerException)
        {
            LocalizeKey = localizeKey;
            FormatArgs = formatArgs ?? new object[0];
        }

        public RuntimeException(string localizeKey,
            string localizedMessage, Exception innerException)
            : this(localizeKey, localizedMessage,
                formatArgs: null, innerException: innerException)
        {
        }

        public RuntimeException(string localizeKey, string localizedMessage)
            : this(localizeKey, localizedMessage,
                formatArgs: null, innerException: null)
        {
        }

        public RuntimeException(string localizeKey, string localizedMessageFormat,
            params object[] values)
            : this(localizeKey,
                string.Format(localizedMessageFormat, values),
                formatArgs: values, innerException: null)
        {
        }
    }
}