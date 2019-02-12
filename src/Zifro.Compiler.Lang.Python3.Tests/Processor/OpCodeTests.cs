using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Entities;
using Zifro.Compiler.Lang.Python3.Instructions;
using Zifro.Compiler.Lang.Python3.Interfaces;
using Zifro.Compiler.Lang.Python3.Resources;
using Zifro.Compiler.Lang.Python3.Syntax.Literals;

namespace Zifro.Compiler.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class OpCodeTests
    {
        private class ThrowingOp : IOpCode
        {
            private const string LocalizeKey = "_testing_";

            public readonly InternalException exception = 
                new InternalException(LocalizeKey, "Error thrown by op-code.");
            
            public SourceReference Source { get; } = SourceReference.ClrSource;

            public void Execute(PyProcessor processor)
            {
                throw exception;
            }
        }

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
        public void OpCodeThrewErrorTest()
        {
            // Arrange
            var op = new ThrowingOp();
            var processor = new PyProcessor(
                op
            );

            // Act
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(ProcessState.Error, processor.State);
            Assert.AreEqual(ProcessState.Error, processor.State);
            Assert.AreSame(op.exception, processor.LastError);
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
                new ThrowingOp()
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
    }
}