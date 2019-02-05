using System;

namespace Zifro.Compiler.Core.Exceptions
{
    public class InternalException : InterpreterException
    {
        public InternalException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InternalException(string message) : base(message)
        {
        }
    }
}