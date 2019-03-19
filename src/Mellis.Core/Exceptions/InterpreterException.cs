using System;

namespace Mellis.Core.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Base class for all exceptions regarding the Zifro Compiler and it's plugins.
    /// </summary>
    public class InterpreterException : Exception
    {
        public InterpreterException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public InterpreterException(string message)
            : this(message, null)
        { }
    }
}