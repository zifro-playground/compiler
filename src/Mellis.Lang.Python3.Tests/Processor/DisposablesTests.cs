using System;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Tests.TestingOps;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class DisposablesTests
    {
        private static Mock<IDisposable> CreateThrowingDisposable(Exception exception1)
        {
            var disposableMock = new Mock<IDisposable>();
            disposableMock.Setup(o => o.Dispose())
                .Throws(exception1)
                .Verifiable();
            return disposableMock;
        }

        [TestMethod]
        public void CallsDisposeOnError()
        {
            // Arrange
            var processor = new PyProcessor(
                new ThrowingTestOp(new Exception())
            );

            var disposableMock = new Mock<IDisposable>();
            disposableMock.Setup(o => o.Dispose())
                .Verifiable();

            processor.PushDisposable(disposableMock.Object);
            
            // Act
            processor.WalkInstruction(); // warmup
            try
            {
                processor.WalkInstruction();
            }
            catch
            {
                // ignored
            }

            // Assert
            disposableMock.Verify();
        }

        [TestMethod]
        public void RethrowDisposableInterpreterError()
        {
            // Arrange
            var processor = new PyProcessor(
                new ThrowingTestOp(new Exception())
            );

            var exception = new InterpreterException("foo bar");

            var disposableMock = CreateThrowingDisposable(exception);

            processor.PushDisposable(disposableMock.Object);

            // Act
            processor.WalkInstruction(); // warmup
            var ex = Assert.ThrowsException<InterpreterException>(delegate { processor.WalkInstruction(); });

            // Assert
            Assert.AreSame(exception, ex);
        }

        [TestMethod]
        public void RethrowDisposableInnerError()
        {
            // Arrange
            var processor = new PyProcessor(
                new ThrowingTestOp(new Exception())
            );

            var exception = new Exception("foo bar");

            var disposableMock = CreateThrowingDisposable(exception);

            processor.PushDisposable(disposableMock.Object);

            // Act
            processor.WalkInstruction(); // warmup
            var ex = Assert.ThrowsException<InterpreterLocalizedException>(delegate { processor.WalkInstruction(); });

            // Assert
            Assert.AreEqual(nameof(Localized_Python3_Interpreter.Ex_Unknown_Error), ex.LocalizeKey);
            Assert.AreEqual(exception.Message, ex.FormatArgs[0]);
            Assert.AreSame(exception, ex.InnerException);
        }

        [TestMethod]
        public void RethrowDisposableMultipleShowsFirstError()
        {
            // Arrange
            var processor = new PyProcessor(
                new ThrowingTestOp(new Exception())
            );

            var exception1 = new Exception("foo bar 1");
            var disposable1Mock = CreateThrowingDisposable(exception1);
            var exception2 = new Exception("foo bar 2");
            var disposable2Mock = CreateThrowingDisposable(exception2);
            var exception3 = new Exception("foo bar 3");
            var disposable3Mock = CreateThrowingDisposable(exception3);

            processor.PushDisposable(disposable1Mock.Object);
            processor.PushDisposable(disposable2Mock.Object);
            processor.PushDisposable(disposable3Mock.Object);

            // Act
            processor.WalkInstruction(); // warmup
            var ex = Assert.ThrowsException<InterpreterLocalizedException>(delegate { processor.WalkInstruction(); });

            // Assert
            Assert.AreEqual(nameof(Localized_Python3_Interpreter.Ex_Unknown_Error), ex.LocalizeKey);
            Assert.AreEqual(exception1.Message, ex.FormatArgs[0]);
            Assert.AreSame(exception1, ex.InnerException);
        }
    }
}