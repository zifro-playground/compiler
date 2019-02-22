using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Syntax.Literals;
using Mellis.Lang.Python3.Tests.TestingOps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class CallEvaluateTests
    {
        [TestMethod]
        public void ClrCallJumpsAfterInvokeTest()
        {
            // Arrange
            const int returnAddress = 3;
            var processor = new PyProcessor(
                new Call(SourceReference.ClrSource, 0, returnAddress),
                new NopOp(),
                new NopOp(),
                new NopOp()
            );

            var defMock = new Mock<IClrFunction>();
            var function = new PyClrFunction(processor, defMock.Object);

            processor.PushValue(function);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(returnAddress, processor.ProgramCounter);
            Assert.AreEqual(ProcessState.Running, processor.State);
        }

        [TestMethod]
        public void ClrCallLoadsArgumentsTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Call(SourceReference.ClrSource, 3, 1),
                new CallStackPop(SourceReference.ClrSource)
            );

            var value = Mock.Of<IScriptType>();
            var defMock = new Mock<IClrFunction>();
            defMock.Setup(o => o.Invoke(It.IsAny<IScriptType[]>()))
                .Returns(value).Verifiable();

            var function = new PyClrFunction(processor, defMock.Object);
            processor.PushValue(function);

            var lit1 = new PyInteger(processor, 5);
            var lit2 = new PyString(processor, "moo");
            var lit3 = new PyBoolean(processor, false);

            processor.PushValue(lit1);
            processor.PushValue(lit2);
            processor.PushValue(lit3);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkLine();

            // Assert
            defMock.Verify(o => o.Invoke(new IScriptType[] {lit1, lit2, lit3}));
            defMock.Verify();

            Assert.AreEqual(1, processor.ValueStackCount);

            IScriptType actualValue = processor.PopValue();
            Assert.AreSame(value, actualValue);
        }

        [TestMethod]
        public void ClrCallPushesResultToValueStackTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Call(SourceReference.ClrSource, 0, 1),
                new CallStackPop(SourceReference.ClrSource)
            );

            var value = Mock.Of<IScriptType>();

            var defMock = new Mock<IClrFunction>();
            defMock.Setup(o => o.Invoke(It.IsAny<IScriptType[]>()))
                .Returns(value).Verifiable();

            var function = new PyClrFunction(processor, defMock.Object);
            processor.PushValue(function);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkLine();

            // Assert
            defMock.Verify();

            Assert.AreEqual(1, processor.ValueStackCount);

            IScriptType actualValue = processor.PopValue();
            Assert.AreSame(value, actualValue);
        }

        [TestMethod]
        public void ClrCallConvertsNullTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Call(SourceReference.ClrSource, 0, 1),
                new CallStackPop(SourceReference.ClrSource)
            );

            var defMock = new Mock<IClrFunction>();
            defMock.Setup(o => o.Invoke(It.IsAny<IScriptType[]>()))
                .Returns((IScriptType) null).Verifiable();

            var function = new PyClrFunction(processor, defMock.Object);
            processor.PushValue(function);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkLine();

            // Assert
            defMock.Verify();

            Assert.AreEqual(1, processor.ValueStackCount);

            IScriptType actualValue = processor.PopValue();
            Assert.AreSame(processor.Factory.Null, actualValue);
        }

        [TestMethod]
        public void ClrCallPushesCallScopeTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Call(SourceReference.ClrSource, 0, 1),
                new CallStackPop(SourceReference.ClrSource)
            );

            var defMock = new Mock<IClrFunction>();

            var function = new PyClrFunction(processor, defMock.Object);
            processor.PushValue(function);

            // Act
            processor.WalkInstruction(); // warmup
            int before = processor.CallStackCount;
            processor.WalkLine();

            // Assert
            Assert.AreEqual(0, before);
            Assert.AreEqual(1, processor.CallStackCount);
        }

    }
}