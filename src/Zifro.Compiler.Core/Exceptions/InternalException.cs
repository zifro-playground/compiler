using System;

namespace Zifro.Compiler.Core.Exceptions
{
    public class InternalException : InterpreterLocalizedException
    {
        public InternalException(string localizeKey, string localizedMessageFormat, params object[] values)
            : base(localizeKey, localizedMessageFormat, values)
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