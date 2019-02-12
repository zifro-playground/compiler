using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Entities;
using Zifro.Compiler.Lang.Python3.Instructions;
using Zifro.Compiler.Lang.Python3.Resources;
using Zifro.Compiler.Lang.Python3.Syntax.Literals;
using Zifro.Compiler.Lang.Python3.Tests.Processor.TestingOps;

namespace Zifro.Compiler.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class OpCodeTests
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
            Assert.IsNull(processor.LastError, "Last error <{0}>:{1}", processor.LastError?.GetType().Name, processor.LastError?.Message);
            Assert.AreEqual(ProcessState.Ended, processor.State);
        }

        [TestMethod]
        public void OpCodeThrewInterpreterErrorTest()
        {
            // Arrange
            var actualEx = new InternalException("","");
            var processor = new PyProcessor(
                new ThrowingOp(actualEx)
            );

            // Act
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(ProcessState.Error, processor.State);
            Assert.AreEqual(ProcessState.Error, processor.State);
            Assert.AreSame(actualEx, processor.LastError);
        }

        [TestMethod]
        public void OpCodeThrewSystemErrorTest()
        {
            // Arrange
            var actualEx = new Exception();
            var processor = new PyProcessor(
                new ThrowingOp(actualEx)
            );

            // Act
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(ProcessState.Error, processor.State);
            Assert.IsNotNull(processor.LastError);
            Assert.IsInstanceOfType(processor.LastError, typeof(InterpreterLocalizedException));
            Assert.AreSame(actualEx, processor.LastError.InnerException);
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
            Assert.IsNull(processor.LastError, "Last error <{0}>:{1}", processor.LastError?.GetType().Name, processor.LastError?.Message);
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_Process_Ended));
        }

        [TestMethod]
        public void ErrorWalkAfterErrorTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new ThrowingOp(new InternalException("",""))
            );

            // Act
            processor.WalkInstruction();
            var ex = Assert.ThrowsException<InternalException>((Action)processor.WalkInstruction);

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
            processor.WalkInstruction();
            processor.WalkInstruction();
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(3, processor.ValueStackCount);
            Assert.AreEqual(3, processor.PopValue<PyInteger>().Value);
            Assert.AreEqual(2, processor.PopValue<PyInteger>().Value);
            Assert.AreEqual(1, processor.PopValue<PyInteger>().Value);
            Assert.AreEqual(ProcessState.Ended, processor.State);
        }

        [TestMethod]
        public void WalkLineManySameLineTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp { Source = new SourceReference(1,1,0,0) },
                new NopOp { Source = new SourceReference(1,1,1,1) },
                new NopOp { Source = new SourceReference(1,1,2,2) },
                new NopOp { Source = new SourceReference(2,2,3,3) }
            );

            // Act
            processor.WalkLine();

            // Assert
            Assert.AreEqual(2, processor.ProgramCounter);
            Assert.AreEqual(ProcessState.Running, processor.State);
        }

        [TestMethod]
        public void WalkInstructionManySameLineTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp { Source = new SourceReference(1, 1, 0, 0) },
                new NopOp { Source = new SourceReference(1, 1, 1, 1) },
                new NopOp { Source = new SourceReference(1, 1, 2, 2) },
                new NopOp { Source = new SourceReference(2, 2, 3, 3) }
            );

            // Act
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(0, processor.ProgramCounter);
            Assert.AreEqual(ProcessState.Running, processor.State);
        }

        [TestMethod]
        public void WalkLineOneSameLineTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp { Source = new SourceReference(1, 1, 0, 0) },
                new NopOp { Source = new SourceReference(2, 2, 1, 1) },
                new NopOp { Source = new SourceReference(2, 2, 2, 2) },
                new NopOp { Source = new SourceReference(2, 2, 3, 3) }
            );

            // Act
            processor.WalkLine();

            // Assert
            Assert.AreEqual(0, processor.ProgramCounter);
            Assert.AreEqual(ProcessState.Running, processor.State);
        }

        [TestMethod]
        public void WalkLineOneFollowedByClrTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp { Source = new SourceReference(1, 1, 0, 0) },
                new NopOp { Source = SourceReference.ClrSource },
                new NopOp { Source = SourceReference.ClrSource },
                new NopOp { Source = new SourceReference(2, 2, 3, 3) }
            );

            // Act
            processor.WalkLine();

            // Assert
            Assert.AreEqual(2, processor.ProgramCounter);
            Assert.AreEqual(ProcessState.Running, processor.State);
        }

        [TestMethod]
        public void WalkLineOneFirstIsClrTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp { Source = SourceReference.ClrSource },
                new NopOp { Source = new SourceReference(1, 1, 1, 1) },
                new NopOp { Source = new SourceReference(1, 1, 2, 2) },
                new NopOp { Source = new SourceReference(2, 2, 3, 3) }
            );

            // Act
            processor.WalkLine();

            // Assert
            Assert.AreEqual(0, processor.ProgramCounter);
            Assert.AreEqual(ProcessState.Running, processor.State);
        }

        [TestMethod]
        public void WalkLineAllSameLineTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp { Source = new SourceReference(2, 1, 0, 0) },
                new NopOp { Source = new SourceReference(2, 2, 1, 1) },
                new NopOp { Source = new SourceReference(2, 2, 2, 2) },
                new NopOp { Source = new SourceReference(2, 2, 3, 3) }
            );

            // Act
            processor.WalkLine();

            // Assert
            Assert.AreEqual(3, processor.ProgramCounter);
            Assert.AreEqual(ProcessState.Ended, processor.State);
        }

    }
}