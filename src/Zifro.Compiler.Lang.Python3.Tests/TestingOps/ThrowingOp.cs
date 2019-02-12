using System;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Interfaces;

namespace Zifro.Compiler.Lang.Python3.Tests.TestingOps
{
    public class ThrowingOp : IOpCode
    {
        public readonly Exception Exception;

        public ThrowingOp(Exception exception)
        {
            this.Exception = exception;
        }

        public SourceReference Source { get; } = SourceReference.ClrSource;

        public void Execute(PyProcessor processor)
        {
            throw Exception;
        }
    }
}