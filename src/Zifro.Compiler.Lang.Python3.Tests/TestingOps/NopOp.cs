using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Interfaces;

namespace Zifro.Compiler.Lang.Python3.Tests.TestingOps
{
    public class NopOp : IOpCode
    {
        public SourceReference Source { get; set; }
        public void Execute(PyProcessor processor)
        { }
    }
}