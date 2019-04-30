using Mellis.Core.Entities;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Processor.WalkerStatus
{
    [TestClass]
    public sealed class WalkInstructionStatusTests : BaseWalkerStatusTester
    {
        protected override WalkStatus? WalkProcessor(PyProcessor processor)
        {
            return processor.WalkInstruction();
        }
    }
}