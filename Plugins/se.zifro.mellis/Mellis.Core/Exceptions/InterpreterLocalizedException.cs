using System;
using System.Collections.Generic;
using System.Text;

namespace Mellis.Core.Exceptions
{
    public class InterpreterLocalizedException : InterpreterException
    {
        public string LocalizeKey { get; }
        public object[] FormatArgs { get; }

        public InterpreterLocalizedException(string localizeKey,
            string localizedMessage, Exception innerException)
            : base(
                localizedMessage,
                innerException: innerException)
        {
            LocalizeKey = localizeKey;
            FormatArgs = new object[0];
        }

        public InterpreterLocalizedException(string localizeKey, string localizedMessageFormat,
            Exception innerException, params object[] formatArgs)
            : this(localizeKey, 
                string.Format(localizedMessageFormat, formatArgs),
                innerException: innerException)
        {
            FormatArgs = formatArgs;
        }

        public InterpreterLocalizedException(string localizeKey, string localizedMessageFormat,
            params object[] formatArgs)
            : this(localizeKey, localizedMessageFormat,
                innerException: null,
                formatArgs: formatArgs)
        {
        }

        public InterpreterLocalizedException(string localizeKey, string localizedMessage)
            : this(localizeKey, localizedMessage,
                innerException: null)
        {
        }
    }
}