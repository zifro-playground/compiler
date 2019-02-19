using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Syntax.Literals;
using Mellis.Lang.Python3.Tests.TestingOps;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class WalkerTests
    {
        [TestMethod]
        public void NoOpCodesNotStartedTest()
        {
            // Arrange
            var processor = new PyProcessor();

            // Assert
            Assert.AreEqual(ProcessState.NotStarted, processor.State);
            Assert.IsNull(processor.LastError);
        }

        [TestMethod]
        public void NoOpCodesStartedTest()
        {
            // Arrange
            var processor = new PyProcessor();

            // Act
            processor.WalkInstruction();

            // Assert
            Assert.IsNull(processor.LastError, "Last error <{0}>:{1}", processor.LastError?.GetType().Name,
                processor.LastError?.Message);
            Assert.AreEqual(ProcessState.Ended, processor.State);
        }

        [TestMethod]
        public void OpCodeThrewInterpreterErrorTest()
        {
            // Arrange
            var expectedEx = new InternalException("", "");
            var processor = new PyProcessor(
                new ThrowingOp(expectedEx)
            );

            // Act
            processor.WalkInstruction(); // to enter first op
            var thrownEx = Assert.ThrowsException<InternalException>((Action) processor.WalkInstruction);

            // Assert
            Assert.AreEqual(ProcessState.Error, processor.State);
            Assert.AreEqual(ProcessState.Error, processor.State);
            Assert.AreSame(expectedEx, processor.LastError);
            Assert.AreSame(expectedEx, thrownEx);
        }

        [TestMethod]
        public void OpCodeThrewSystemErrorTest()
        {
            // Arrange
            var expectedEx = new Exception();
            var processor = new PyProcessor(
                new ThrowingOp(expectedEx)
            );

            // Act
            processor.WalkInstruction(); // to enter first op
            var thrownEx = Assert.ThrowsException<InterpreterLocalizedException>((Action) processor.WalkInstruction);

            // Assert
            Assert.AreEqual(ProcessState.Error, processor.State);
            Assert.IsNotNull(processor.LastError);
            Assert.IsInstanceOfType(processor.LastError, typeof(InterpreterLocalizedException));
            Assert.AreSame(expectedEx, processor.LastError.InnerException);
            Assert.AreSame(expectedEx, thrownEx.InnerException);
        }

        [TestMethod]
        public void ErrorWalkAfterEndedTest()
        {
            // Arrange
            var processor = new PyProcessor();

            // Act
            processor.WalkInstruction();
            var ex = Assert.ThrowsException<InternalException>((Action) processor.WalkInstruction);

            // Assert
            Assert.IsNull(processor.LastError, "Last error <{0}>:{1}", processor.LastError?.GetType().Name,
                processor.LastError?.Message);
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_Process_Ended));
        }

        [TestMethod]
        public void ErrorWalkAfterErrorTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new ThrowingOp(new InternalException("", ""))
            );

            // Act
            processor.WalkInstruction(); // to enter first op
            Assert.ThrowsException<InternalException>((Action)processor.WalkInstruction);

            var ex = Assert.ThrowsException<InternalException>((Action) processor.WalkInstruction);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_Process_Ended));
            Assert.IsNotNull(processor.LastError);
        }

        [TestMethod]
        public void PushValuesTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new PushLiteral<int>(new LiteralInteger(SourceReference.ClrSource, 1)),
                new PushLiteral<int>(new LiteralInteger(SourceReference.ClrSource, 2)),
                new PushLiteral<int>(new LiteralInteger(SourceReference.ClrSource, 3))
            );

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction();
            processor.WalkInstruction();
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(3, processor.ValueStackCount);
            Assert.AreEqual(3, PopInteger().Value);
            Assert.AreEqual(2, PopInteger().Value);
            Assert.AreEqual(1, PopInteger().Value);
            Assert.AreEqual(ProcessState.Ended, processor.State);

            PyInteger PopInteger()
            {
                return (PyInteger) processor.PopValue();
            }
        }

        [TestMethod]
        public void WalkLineManySameLineTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp {Source = new SourceReference(1, 1, 0, 0)},
                new NopOp {Source = new SourceReference(1, 1, 1, 1)},
                new NopOp {Source = new SourceReference(1, 1, 2, 2)},
                new NopOp {Source = new SourceReference(2, 2, 3, 3)}
            );

            // Act
            processor.WalkLine(); // to enter first op
            processor.WalkLine();

            // Assert
            Assert.AreEqual(3, processor.ProgramCounter);
            Assert.AreEqual(ProcessState.Running, processor.State);
        }

        [TestMethod]
        public void WalkInstructionManySameLineTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp {Source = new SourceReference(1, 1, 0, 0)},
                new NopOp {Source = new SourceReference(1, 1, 1, 1)},
                new NopOp {Source = new SourceReference(1, 1, 2, 2)},
                new NopOp {Source = new SourceReference(2, 2, 3, 3)}
            );

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(1, processor.ProgramCounter);
            Assert.AreEqual(ProcessState.Running, processor.State);
        }

        [TestMethod]
        public void WalkLineOneSameLineTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp {Source = new SourceReference(1, 1, 0, 0)},
                new NopOp {Source = new SourceReference(2, 2, 1, 1)},
                new NopOp {Source = new SourceReference(2, 2, 2, 2)},
                new NopOp {Source = new SourceReference(2, 2, 3, 3)}
            );

            // Act
            processor.WalkLine(); // to enter first op
            processor.WalkLine();

            // Assert
            Assert.AreEqual(1, processor.ProgramCounter);
            Assert.AreEqual(ProcessState.Running, processor.State);
        }

        [TestMethod]
        public void WalkLineOneFollowedByClrTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp {Source = new SourceReference(1, 1, 0, 0)},
                new NopOp {Source = SourceReference.ClrSource},
                new NopOp {Source = SourceReference.ClrSource},
                new NopOp {Source = new SourceReference(2, 2, 3, 3)}
            );

            // Act
            processor.WalkLine(); // to enter first op
            processor.WalkLine();

            // Assert
            Assert.AreEqual(3, processor.ProgramCounter);
            Assert.AreEqual(ProcessState.Running, processor.State);
        }

        [TestMethod]
        public void WalkLineOneFirstIsClrTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp {Source = SourceReference.ClrSource},
                new NopOp {Source = new SourceReference(1, 1, 1, 1)},
                new NopOp {Source = new SourceReference(1, 1, 2, 2)},
                new NopOp {Source = new SourceReference(2, 2, 3, 3)}
            );

            // Act
            processor.WalkLine(); // to enter first op
            processor.WalkLine();

            // Assert
            Assert.AreEqual(1, processor.ProgramCounter);
            Assert.AreEqual(ProcessState.Running, processor.State);
        }

        [TestMethod]
        public void WalkLineAllSameLineTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp {Source = new SourceReference(2, 1, 0, 0)},
                new NopOp {Source = new SourceReference(2, 2, 1, 1)},
                new NopOp {Source = new SourceReference(2, 2, 2, 2)},
                new NopOp {Source = new SourceReference(2, 2, 3, 3)}
            );

            // Act
            processor.WalkLine(); // to enter first op
            processor.WalkLine();

            // Assert
            Assert.AreEqual(4, processor.ProgramCounter);
            Assert.AreEqual(ProcessState.Ended, processor.State);
        }
    }
}