using System;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Tests.TestingOps;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class YieldingTests
    {
        private static PyProcessor DummyProcessor()
        {
            var dummyProcessor = new PyProcessor(
                new NopOp(),
                new NopOp(),
                new NopOp()
            );
            dummyProcessor.WalkInstruction(); // warmup
            return dummyProcessor;
        }

        [TestMethod]
        public void InvalidResolveWhenNotYieldedTest()
        {
            // Arrange
            PyProcessor processor = DummyProcessor();

            // Act
            var ex = Assert.ThrowsException<InternalException>((Action)processor.ResolveYield);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_Yield_ResolveWhenNotYielded));
        }

        [TestMethod]
        public void InvalidResolveWhenNoCallStackTest()
        {
            // Arrange
            PyProcessor processor = DummyProcessor();
            processor.Yield(new YieldData(null, null));

            // Act
            var ex = Assert.ThrowsException<InternalException>((Action)processor.ResolveYield);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_CallStack_PopEmpty));
        }

        [TestMethod]
        public void InvalidResolveNoYieldDataTest()
        {
            // Arrange
            PyProcessor processor = DummyProcessor();
            processor.Yield(null);

            // Act
            var ex = Assert.ThrowsException<InternalException>((Action)processor.ResolveYield);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_Yield_ResolveNoYieldData));
        }

        [TestMethod]
        public void ResolvePopsCallStackTest()
        {
            // Arrange
            PyProcessor processor = DummyProcessor();
            processor.PushCallStack(new CallStack(
                SourceReference.ClrSource, "foo", 1
            ));

            var def = Mock.Of<IClrYieldingFunction>();
            processor.Yield(new YieldData(new IScriptType[0], def));

            // Act
            int before = processor.CallStackCount;
            processor.ResolveYield();
            int after = processor.CallStackCount;

            // Assert
            Assert.AreEqual(1, before);
            Assert.AreEqual(0, after);
        }

        [TestMethod]
        public void ResolveCallsExitTest()
        {
            // Arrange
            PyProcessor processor = DummyProcessor();
            processor.PushCallStack(new CallStack(
                SourceReference.ClrSource, "foo", 1
            ));

            var defMock = new Mock<IClrYieldingFunction>();
            var args = new IScriptType[0];
            var ret = Mock.Of<IScriptType>();

            processor.Yield(new YieldData(args, defMock.Object));

            // Act
            processor.ResolveYield(ret);

            // Assert
            defMock.Verify(o => o.InvokeExit(args, ret), Times.Once);
        }

        [TestMethod]
        public void ResolveJumpsTest()
        {
            // Arrange
            PyProcessor processor = DummyProcessor();
            processor.PushCallStack(new CallStack(
                SourceReference.ClrSource, "foo", 1
            ));

            var defMock = new Mock<IClrYieldingFunction>();

            processor.Yield(new YieldData(new IScriptType[0], defMock.Object));

            // Act
            int before = processor.ProgramCounter;
            processor.ResolveYield();

            // Assert
            Assert.AreNotEqual(1, before);
            Assert.AreEqual(1, processor.ProgramCounter);
        }

        [TestMethod]
        public void ResolveChangesStateTest()
        {
            // Arrange
            PyProcessor processor = DummyProcessor();
            processor.PushCallStack(new CallStack(
                SourceReference.ClrSource, "foo", 1
            ));

            var defMock = new Mock<IClrYieldingFunction>();

            processor.Yield(new YieldData(new IScriptType[0], defMock.Object));

            // Act
            processor.ResolveYield();

            // Assert
            Assert.AreEqual(ProcessState.Running, processor.State);
        }
        
        [TestMethod]
        public void ResolveEmptyUsesNullTest()
        {
            // Arrange
            PyProcessor processor = DummyProcessor();
            processor.PushCallStack(new CallStack(
                SourceReference.ClrSource, "foo", 1
            ));

            var defMock = new Mock<IClrYieldingFunction>();

            var arguments = new IScriptType[0];
            processor.Yield(new YieldData(arguments, defMock.Object));

            // Act
            processor.ResolveYield();

            // Assert
            Assert.AreEqual(ProcessState.Running, processor.State);

            defMock.Verify(o => o.InvokeExit(arguments, processor.Factory.Null), Times.Once);
        }

        [TestMethod]
        public void ResolveNullConvertsToNullTest()
        {
            // Arrange
            PyProcessor processor = DummyProcessor();
            processor.PushCallStack(new CallStack(
                SourceReference.ClrSource, "foo", 1
            ));

            var defMock = new Mock<IClrYieldingFunction>();

            var arguments = new IScriptType[0];
            processor.Yield(new YieldData(arguments, defMock.Object));

            // Act
            processor.ResolveYield(null);

            // Assert
            Assert.AreEqual(ProcessState.Running, processor.State);

            defMock.Verify(o => o.InvokeExit(arguments, processor.Factory.Null), Times.Once);
        }

        [TestMethod]
        public void ResolveArgumentIsPassedTest()
        {
            // Arrange
            PyProcessor processor = DummyProcessor();
            processor.PushCallStack(new CallStack(
                SourceReference.ClrSource, "foo", 1
            ));

            var defMock = new Mock<IClrYieldingFunction>();

            var arguments = new IScriptType[0];
            processor.Yield(new YieldData(arguments, defMock.Object));

            var ret = Mock.Of<IScriptType>();

            // Act
            processor.ResolveYield(ret);

            // Assert
            Assert.AreEqual(ProcessState.Running, processor.State);

            defMock.Verify(o => o.InvokeExit(arguments, ret), Times.Once);
        }

        [TestMethod]
        public void ResolvePushesAlteredValueTest()
        {
            // Arrange
            PyProcessor processor = DummyProcessor();
            processor.PushCallStack(new CallStack(
                SourceReference.ClrSource, "foo", 1
            ));

            var defMock = new Mock<IClrYieldingFunction>();

            var ret1 = Mock.Of<IScriptType>();
            var ret2 = Mock.Of<IScriptType>();
            var arguments = new IScriptType[0];
            processor.Yield(new YieldData(arguments, defMock.Object));

            defMock.Setup(o => o.InvokeExit(arguments, ret1))
                .Returns(ret2);

            // Act
            processor.ResolveYield(ret1);

            // Assert
            var actual = processor.PopValue();
            Assert.AreSame(ret2, actual);
            Assert.AreNotSame(ret1, actual);
            Assert.AreNotSame(ret1, ret2);
        }

        [TestMethod]
        public void YieldChangesStateTest()
        {
            // Arrange
            PyProcessor processor = DummyProcessor();
            processor.PushCallStack(new CallStack(
                SourceReference.ClrSource, "foo", 1
            ));

            var defMock = new Mock<IClrYieldingFunction>();

            // Act
            var before = processor.State;
            processor.Yield(new YieldData(new IScriptType[0], defMock.Object));

            // Assert
            Assert.AreEqual(ProcessState.Running, before);
            Assert.AreEqual(ProcessState.Yielded, processor.State);
        }
    }
}