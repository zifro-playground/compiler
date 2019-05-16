using Mellis.Core.Entities;
using Mellis.Lang.Python3.Tests.TestingOps;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Processor.WalkerStatus
{
    [TestClass]
    public sealed class WalkInstructionStatusTests : BaseWalkerStatusTester
    {
        protected override WalkStatus WalkProcessor(PyProcessor processor)
        {
            return processor.WalkInstruction();
        }

        [TestMethod]
        public void NullStatusWhenNothingPeculiarTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp()
            );

            // Act
            var status = WalkProcessor(processor);

            // Assert
            Assert.AreEqual(PyProcessor.NULL_WALK_STATUS, status);
        }

        public override void BreakStatusOnJumpLimitReachedTest()
        {
            // Disables the test
        }

        public override void EndedStatusOnJumpLimitNotReachedTest()
        {
            // Disables the test
        }

        public override void BreakStatusOnInstructionLimitReachedTest()
        {
            // Disables the test
        }

        public override void EndedStatusOnInstructionLimitNotReachedTest()
        {
            // Disables the test
        }
    }
}