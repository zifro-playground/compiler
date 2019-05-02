using Mellis.Core.Entities;
using Mellis.Lang.Python3.Tests.TestingOps;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Processor.WalkerStatus
{
    [TestClass]
    public sealed class WalkLineStatusTests : BaseWalkerStatusTester
    {
        protected override WalkStatus WalkProcessor(PyProcessor processor)
        {
            return processor.WalkLine();
        }

        [TestMethod]
        public void EndedStatusWalkLineToEndOverClrTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp {Source = SourceReference.ClrSource},
                new NopOp {Source = SourceReference.ClrSource}
            );

            // Act
            processor.WalkInstruction(); // warmup
            var status = WalkProcessor(processor);

            // Assert
            Assert.AreEqual(WalkStatus.Ended, status);
        }

        [TestMethod]
        public void EndedStatusWalkLineToEndOverLineTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp {Source = new SourceReference(1, 1, 1, 1)},
                new NopOp {Source = new SourceReference(1, 1, 1, 1)}
            );

            // Act
            processor.WalkInstruction(); // warmup
            var status = WalkProcessor(processor);

            // Assert
            Assert.AreEqual(WalkStatus.Ended, status);
        }

        [TestMethod]
        public void NewLineStatusAfterClrSourcesTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp {Source = SourceReference.ClrSource},
                new NopOp {Source = SourceReference.ClrSource},
                new NopOp {Source = new SourceReference(1, 1, 1, 1)},
                new ThrowingTestOp(new AssertFailedException("Walked too far.")) {
                    Source = new SourceReference(2, 2, 1, 1)
                }
            );

            // Act
            processor.WalkInstruction(); // warmup
            var status = WalkProcessor(processor);

            // Assert
            Assert.AreEqual(WalkStatus.NewLine, status);
        }

        [TestMethod]
        public void NewLineStatusAfterSameLinesTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp {Source = new SourceReference(1, 1, 1, 1)},
                new NopOp {Source = new SourceReference(1, 1, 1, 1)},
                new NopOp {Source = new SourceReference(1, 1, 1, 1)},
                new ThrowingTestOp(new AssertFailedException("Walked too far.")) {
                    Source = new SourceReference(2, 2, 1, 1)
                }
            );

            // Act
            processor.WalkInstruction(); // warmup
            var status = WalkProcessor(processor);

            // Assert
            Assert.AreEqual(WalkStatus.NewLine, status);
        }

        [TestMethod]
        public void NewLineStatusOnWarmupTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp()
            );

            // Act
            var status = WalkProcessor(processor); // warmup

            // Assert
            Assert.AreEqual(WalkStatus.NewLine, status);
        }

        [TestMethod]
        public void BreakStatusOnJumpLimitReachedClrTest()
        {
            BreakStatusOnJumpLimitReachedBase(SourceReference.ClrSource);
        }

        [TestMethod]
        public void BreakStatusOnJumpLimitReachedNonClrTest()
        {
            BreakStatusOnJumpLimitReachedBase(new SourceReference(1, 1, 1, 1));
        }

        [TestMethod]
        public void EndedStatusOnJumpLimitNotReachedClrTest()
        {
            EndedStatusOnJumpLimitNotReachedBase(SourceReference.ClrSource);
        }

        [TestMethod]
        public void EndedStatusOnJumpLimitNotReachedNonClrTest()
        {
            EndedStatusOnJumpLimitNotReachedBase(new SourceReference(1, 1, 1, 1));
        }

        [TestMethod]
        public void BreakStatusOnInstructionLimitReachedClrTest()
        {
            BreakStatusOnInstructionLimitReachedBase(SourceReference.ClrSource);
        }

        [TestMethod]
        public void BreakStatusOnInstructionLimitReachedNonClrTest()
        {
            BreakStatusOnInstructionLimitReachedBase(new SourceReference(1, 1, 1, 1));
        }

        [TestMethod]
        public void EndedStatusOnInstructionLimitNotReachedClrTest()
        {
            EndedStatusOnInstructionLimitNotReachedBase(SourceReference.ClrSource);
        }

        [TestMethod]
        public void EndedStatusOnInstructionLimitNotReachedNonClrTest()
        {
            EndedStatusOnInstructionLimitNotReachedBase(new SourceReference(1, 1, 1, 1));
        }

        public override void BreakStatusOnInstructionLimitReachedTest()
        {
            // Disabled
        }

        public override void BreakStatusOnJumpLimitReachedTest()
        {
            // Disabled
        }

        public override void EndedStatusOnInstructionLimitNotReachedTest()
        {
            // Disabled
        }

        public override void EndedStatusOnJumpLimitNotReachedTest()
        {
            // Disabled
        }
    }
}