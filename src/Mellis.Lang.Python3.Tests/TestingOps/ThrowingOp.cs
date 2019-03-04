using System;
using Mellis.Core.Entities;
using Mellis.Lang.Python3.Interfaces;

namespace Mellis.Lang.Python3.Tests.TestingOps
{
    public class ThrowingOp : IOpCode
    {
        public readonly Exception Exception;

        public ThrowingOp(Exception exception)
        {
            Exception = exception;
        }

        public SourceReference Source { get; } = SourceReference.ClrSource;

        public void Execute(VM.PyProcessor processor)
        {
            throw Exception;
        }
    }
}