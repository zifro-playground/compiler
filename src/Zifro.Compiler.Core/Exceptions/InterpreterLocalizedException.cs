using System;
using System.Collections.Generic;
using System.Text;

namespace Zifro.Compiler.Core.Exceptions
{
    public class InterpreterLocalizedException : InterpreterException
    {
        public string LocalizeKey { get; }
        public object[] FormatArgs { get; }

        private InterpreterLocalizedException(string localizeKey,
            string localizedMessage, object[] formatArgs, Exception innerException)
            : base(localizedMessage, innerException)
        {
            LocalizeKey = localizeKey;
            FormatArgs = formatArgs ?? new object[0];
        }

        public InterpreterLocalizedException(string localizeKey, string localizedMessageFormat,
            params object[] values)
            : this(localizeKey,
                string.Format(localizedMessageFormat, values),
                formatArgs: values, innerException: null)
        {
        }

        public InterpreterLocalizedException(string localizeKey,
            string localizedMessage, Exception innerException)
            : this(localizeKey, localizedMessage,
                formatArgs: null, innerException: innerException)
        {
        }

        public InterpreterLocalizedException(string localizeKey, string localizedMessage)
            : this(localizeKey, localizedMessage,
                formatArgs: null, innerException: null)
        {
        }
    }
}