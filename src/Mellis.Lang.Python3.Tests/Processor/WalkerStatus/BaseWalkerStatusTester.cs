using Mellis.Core.Entities;
using Mellis.Lang.Python3.Tests.TestingOps;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Processor.WalkerStatus
{
    public abstract class BaseWalkerStatusTester
    {
        protected abstract WalkStatus WalkProcessor(PyProcessor processor);

        [TestMethod]
        public void EndedStatusWalkOneOpTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp()
            );

            // Act
            processor.WalkInstruction(); // warmup
            var status = WalkProcessor(processor);

            // Assert
            Assert.AreEqual(WalkStatus.Ended, status);
        }
    }
}