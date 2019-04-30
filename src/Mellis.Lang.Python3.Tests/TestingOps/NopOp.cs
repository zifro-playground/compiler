using Mellis.Core.Entities;
using Mellis.Lang.Python3.Interfaces;

namespace Mellis.Lang.Python3.Tests.TestingOps
{
    public class NopOp : IOpCode
    {
        public SourceReference Source { get; set; } = SourceReference.ClrSource;
        public void Execute(VM.PyProcessor processor)
        { }
    }
}