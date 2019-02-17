using System;

namespace Mellis.Core.Exceptions
{
    public class InternalException : InterpreterLocalizedException
    {
        public InternalException(string localizeKey, string localizedMessageFormat, params object[] formatArgs)
            : base(localizeKey, localizedMessageFormat, formatArgs)
        {
        }

        public InternalException(string localizeKey, string localizedMessage, Exception innerException)
            : base(localizeKey, localizedMessage, innerException)
        {
        }

        public InternalException(string localizeKey, string localizedMessage)
            : base(localizeKey, localizedMessage)
        {
        }
    }
}