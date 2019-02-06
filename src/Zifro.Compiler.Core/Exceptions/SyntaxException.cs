using System;
using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Core.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Use this for compile-time exceptions based off the source code.
    /// </summary>
    public class SyntaxException : InterpreterLocalizedException
    {
        public SourceReference SourceReference { get; set; }
        
        public SyntaxException(SourceReference source, string localizeKey, 
            string localizedMessageFormat, params object[] values)
            : base(localizeKey, localizedMessageFormat, values)
        {
            SourceReference = source;
        }

        public SyntaxException(SourceReference source, string localizeKey,
            string localizedMessage, Exception innerException)
            : base(localizeKey, localizedMessage, innerException)
        {
            SourceReference = source;
        }

        public SyntaxException(SourceReference source, string localizeKey,
            string localizedMessage)
            : base(localizeKey, localizedMessage)
        {
            SourceReference = source;
        }
    }
}