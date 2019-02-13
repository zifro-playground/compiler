using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Interfaces;

namespace Zifro.Compiler.Lang.Python3.Tests.TestingOps
{
    public class ScopePopper : IOpCode
    {
        public SourceReference Source => SourceReference.ClrSource;
        public void Execute(PyProcessor processor)
        {
            processor.PopScope();
        }
    }
}