using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Tests.TestingOps;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Processor.WalkerStatus
{
    public abstract class BaseWalkerStatusTester
    {
        protected abstract WalkStatus WalkProcessor(PyProcessor processor);

        [TestMethod]
        public void EndedOnEmptyProcessor()
        {
            // Arrange
            var processor = new PyProcessor();

            // Act
            var result = WalkProcessor(processor);

            // Assert
            Assert.AreEqual(WalkStatus.Ended, result);
            Assert.AreEqual(ProcessState.Ended, processor.State);
        }

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

        [TestMethod]
        public void YieldedStatusWhenYieldedTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new YieldingTestOp()
            );

            // Act
            processor.WalkInstruction(); // warmup
            var status = WalkProcessor(processor);

            // Assert
            Assert.AreEqual(WalkStatus.Yielded, status);
        }

        [TestMethod]
        public void BreakStatusWhenBreakpointOpTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Breakpoint(BreakCause.LoopEnter)
            );

            // Act
            processor.WalkInstruction(); // warmup
            var status = WalkProcessor(processor);

            // Assert
            Assert.AreEqual(WalkStatus.Break, status);
            Assert.AreEqual(BreakCause.LoopEnter, processor.LastBreakCause);
        }

        [TestMethod]
        public virtual void BreakStatusOnJumpLimitReachedTest()
        {
            BreakStatusOnJumpLimitReachedBase(SourceReference.ClrSource);
        }

        protected void BreakStatusOnJumpLimitReachedBase(SourceReference jumpSource)
        {
            // Arrange
            var processor = new PyProcessor(
                new CompilerSettings {
                    BreakOn = BreakCause.JumpLimitReached,
                    JumpLimit = 2
                },
                new Jump(jumpSource, 1),
                new Jump(jumpSource, 2),
                new Jump(jumpSource, 3)
            );

            // Act
            processor.WalkInstruction(); // warmup
            var status = WalkProcessor(processor);

            // Assert
            Assert.AreEqual(WalkStatus.Break, status);
            Assert.AreEqual(BreakCause.JumpLimitReached, processor.LastBreakCause);
        }

        [TestMethod]
        public virtual void EndedStatusOnJumpLimitNotReachedTest()
        {
            EndedStatusOnJumpLimitNotReachedBase(SourceReference.ClrSource);
        }

        protected void EndedStatusOnJumpLimitNotReachedBase(SourceReference jumpSource)
        {

            // Arrange
            var processor = new PyProcessor(
                new CompilerSettings {
                    BreakOn = BreakCause.JumpLimitReached,
                    JumpLimit = 4
                },
                new Jump(jumpSource, 1),
                new Jump(jumpSource, 2),
                new Jump(jumpSource, 3)
            );

            // Act
            processor.WalkInstruction(); // warmup
            var status = WalkProcessor(processor);

            // Assert
            Assert.AreEqual(WalkStatus.Ended, status);
        }

        [TestMethod]
        public virtual void BreakStatusOnInstructionLimitReachedTest()
        {
            BreakStatusOnInstructionLimitReachedBase(SourceReference.ClrSource);
        }

        protected void BreakStatusOnInstructionLimitReachedBase(SourceReference nopSource)
        {
            // Arrange
            var processor = new PyProcessor(
                new CompilerSettings {
                    BreakOn = BreakCause.InstructionLimitReached,
                    InstructionLimit = 2
                },
                new NopOp {Source = nopSource},
                new NopOp {Source = nopSource},
                new NopOp {Source = nopSource}
            );

            // Act
            processor.WalkInstruction(); // warmup
            var status = WalkProcessor(processor);

            // Assert
            Assert.AreEqual(WalkStatus.Break, status);
            Assert.AreEqual(BreakCause.InstructionLimitReached, processor.LastBreakCause);
        }

        [TestMethod]
        public virtual void EndedStatusOnInstructionLimitNotReachedTest()
        {
            EndedStatusOnInstructionLimitNotReachedBase(SourceReference.ClrSource);
        }

        protected void EndedStatusOnInstructionLimitNotReachedBase(SourceReference nopSource)
        {
            // Arrange
            var processor = new PyProcessor(
                new CompilerSettings {
                    BreakOn = BreakCause.InstructionLimitReached,
                    InstructionLimit = 4
                },
                new NopOp {Source = nopSource},
                new NopOp {Source = nopSource},
                new NopOp {Source = nopSource}
            );

            // Act
            processor.WalkInstruction(); // warmup
            var status = WalkProcessor(processor);

            // Assert
            Assert.AreEqual(WalkStatus.Ended, status);
        }
    }
}