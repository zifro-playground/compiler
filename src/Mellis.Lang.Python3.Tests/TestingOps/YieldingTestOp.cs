using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.VM;
using Moq;

namespace Mellis.Lang.Python3.Tests.TestingOps
{
    public class YieldingTestOp : IOpCode
    {
        public SourceReference Source { get; } = SourceReference.ClrSource;

        public void Execute(PyProcessor processor)
        {
            processor.Yield(new YieldData(new IScriptType[0], Mock.Of<IClrYieldingFunction>()));
        }
    }
}