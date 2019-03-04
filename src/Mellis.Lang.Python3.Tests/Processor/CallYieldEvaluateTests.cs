using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Syntax.Literals;
using Mellis.Lang.Python3.Tests.TestingOps;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class CallYieldEvaluateTests
    {
        [TestMethod]
        public void YieldCallInvokesEnterMethodTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Call(SourceReference.ClrSource, 0, 1),
                new NopOp()
            );

            var defMock = new Mock<IClrYieldingFunction>();
            defMock.Setup(o => o.InvokeEnter(It.IsAny<IScriptType[]>()));

            var function = new PyClrFunction(processor, defMock.Object);

            processor.PushValue(function);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            defMock.Verify(o => o.InvokeEnter(It.IsAny<IScriptType[]>()), Times.Once);
            defMock.Verify(o => o.Invoke(It.IsAny<IScriptType[]>()), Times.Never);
        }

        [TestMethod]
        public void YieldCallYieldsProcessorTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Call(SourceReference.ClrSource, 0, 1),
                new NopOp()
            );

            var defMock = new Mock<IClrYieldingFunction>();

            var function = new PyClrFunction(processor, defMock.Object);

            processor.PushValue(function);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(ProcessState.Yielded, processor.State);
        }
        
        [TestMethod]
        public void YieldCallResolveResumesProcessorTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Call(SourceReference.ClrSource, 0, 1),
                new NopOp(),
                new NopOp(),
                new NopOp()
            );

            var defMock = new Mock<IClrYieldingFunction>();

            var function = new PyClrFunction(processor, defMock.Object);

            processor.PushValue(function);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();
            processor.ResolveYield();

            // Assert
            Assert.AreEqual(ProcessState.Running, processor.State);
        }

        [TestMethod]
        public void YieldCallInvokesExitOnResolveTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Call(SourceReference.ClrSource, 0, 1),
                new NopOp()
            );

            var retValue = Mock.Of<IScriptType>();

            var defMock = new Mock<IClrYieldingFunction>();
            defMock.Setup(o => o.InvokeExit(It.IsAny<IScriptType[]>(), It.IsAny<IScriptType>()))
                .Returns<IScriptType[], IScriptType>((args, ret) => ret);

            var function = new PyClrFunction(processor, defMock.Object);

            processor.PushValue(function);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();
            processor.ResolveYield(retValue);

            // Assert
            defMock.Verify(o => o.InvokeExit(It.IsAny<IScriptType[]>(), retValue), Times.Once);
            defMock.Verify(o => o.Invoke(It.IsAny<IScriptType[]>()), Times.Never);
        }

        [TestMethod]
        public void YieldCallJumpsAfterResolveTest()
        {
            // Arrange
            const int returnAddress = 3;
            var processor = new PyProcessor(
                new Call(SourceReference.ClrSource, 0, returnAddress),
                new NopOp(),
                new NopOp(),
                new NopOp()
            );

            var defMock = new Mock<IClrYieldingFunction>();

            var function = new PyClrFunction(processor, defMock.Object);

            processor.PushValue(function);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();
            int callAddress = processor.ProgramCounter;
            processor.ResolveYield();

            // Assert
            Assert.AreEqual(returnAddress, processor.ProgramCounter);
            Assert.AreEqual(0, callAddress);
            Assert.AreEqual(ProcessState.Running, processor.State);
        }
        [TestMethod]
        public void YieldCallLoadsArgumentsTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Call(SourceReference.ClrSource, 3, 1)
            );

            var retVal = Mock.Of<IScriptType>();
            var defMock = new Mock<IClrYieldingFunction>();
            defMock.Setup(o => o.InvokeEnter(It.IsAny<IScriptType[]>()));
            defMock.Setup(o => o.InvokeExit(It.IsAny<IScriptType[]>(), retVal))
                .Returns(retVal);

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
            processor.ResolveYield(retVal);

            // Assert
            defMock.Verify(o => o.InvokeEnter(new IScriptType[] {lit1, lit2, lit3}));
            defMock.Verify(o => o.InvokeExit(new IScriptType[] {lit1, lit2, lit3}, retVal));

            Assert.AreEqual(1, processor.ValueStackCount);
        }

        [TestMethod]
        public void YieldCallPushesResultToValueStackTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Call(SourceReference.ClrSource, 0, 1)
            );

            var value = Mock.Of<IScriptType>();

            var defMock = new Mock<IClrYieldingFunction>();
            defMock.Setup(o => o.InvokeExit(It.IsAny<IScriptType[]>(), It.IsAny<IScriptType>()))
                .Returns(value).Verifiable();

            var function = new PyClrFunction(processor, defMock.Object);
            processor.PushValue(function);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkLine();
            processor.ResolveYield(value);

            int stackCount = processor.ValueStackCount;
            IScriptType actualValue = processor.PopValue();

            // Assert
            defMock.Verify();

            Assert.AreSame(value, actualValue);
            Assert.AreEqual(1, stackCount);
        }

        [TestMethod]
        public void YieldCallPushesCallScopeTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Call(SourceReference.ClrSource, 0, 1)
            );

            int inside = -1;
            var defMock = new Mock<IClrYieldingFunction>();
            defMock.Setup(o => o.InvokeExit(It.IsAny<IScriptType[]>(), It.IsAny<IScriptType>()))
                .Returns<IScriptType[], IScriptType>((args, ret) =>
                {
                    inside = processor.CallStackCount;
                    return ret;
                });

            var function = new PyClrFunction(processor, defMock.Object);
            processor.PushValue(function);

            // Act
            processor.WalkInstruction(); // warmup
            int before = processor.CallStackCount;
            processor.WalkInstruction();
            int during = processor.CallStackCount;
            processor.ResolveYield();
            int afterResolve = processor.CallStackCount;

            // Assert
            Assert.AreEqual(0, before, "Before call op.");
            Assert.AreEqual(1, during, "After yield invoke enter.");
            Assert.AreEqual(1, inside, "Inside invoke exit.");
            Assert.AreEqual(0, afterResolve, "After yield resolved.");
        }
    }
}