using System;
using Mellis.Core.Entities;
using Mellis.Lang.Python3.Interfaces;

namespace Mellis.Lang.Python3.Tests.TestingOps
{
    public class ThrowingTestOp : IOpCode
    {
        public readonly Exception Exception;

        public ThrowingTestOp(Exception exception)
        {
            Exception = exception;
        }

        public SourceReference Source { get; set; } = SourceReference.ClrSource;

        public void Execute(VM.PyProcessor processor)
        {
            throw Exception;
        }
    }
}