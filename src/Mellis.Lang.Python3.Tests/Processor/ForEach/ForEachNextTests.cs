using System;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Tests.TestingOps;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Processor.ForEach
{
    [TestClass]
    public class ForEachNextTests
    {
        [TestMethod]
        public void NextCallsMoveNext()
        {
            // Arrange
            var processor = new PyProcessor(
                new ForEachNext(SourceReference.ClrSource, 1),
                new NopOp()
            );

            var setup = new IteratorSetup();
            setup.SetupEnumeratorIsIScriptType();

            setup.EnumeratorMock.SetupGet(o => o.Current)
                .Returns(Mock.Of<IScriptType>());

            setup.EnumeratorMock.Setup(o => o.MoveNext())
                .Returns(true).Verifiable();

            processor.PushValue((IScriptType)setup.EnumeratorMock.Object);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            setup.EnumeratorMock.Verify(o => o.MoveNext());
        }

        [TestMethod]
        public void NextKeepsEnumeratorOnStack()
        {
            // Arrange
            var processor = new PyProcessor(
                new ForEachNext(SourceReference.ClrSource, 1),
                new NopOp()
            );

            var setup = new IteratorSetup();
            setup.SetupEnumeratorIsIScriptType();

            setup.EnumeratorMock.SetupGet(o => o.Current)
                .Returns(Mock.Of<IScriptType>());

            setup.EnumeratorMock.Setup(o => o.MoveNext())
                .Returns(false).Verifiable();

            processor.PushValue((IScriptType)setup.EnumeratorMock.Object);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(1, processor.ValueStackCount);
            var topValue = processor.PopValue();
            Assert.AreSame(setup.EnumeratorMock.Object, topValue);
        }

        [TestMethod]
        public void NextPushesCurrentValue()
        {
            // Arrange
            var processor = new PyProcessor(
                new ForEachNext(SourceReference.ClrSource, 1),
                new NopOp()
            );

            var setup = new IteratorSetup();
            setup.SetupEnumeratorIsIScriptType();

            var scriptType = Mock.Of<IScriptType>();
            setup.EnumeratorMock.SetupGet(o => o.Current)
                .Returns(scriptType)
                .Verifiable();

            setup.EnumeratorMock.Setup(o => o.MoveNext())
                .Returns(true);

            processor.PushValue((IScriptType)setup.EnumeratorMock.Object);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            var result = processor.PopValue();
            Assert.IsNotNull(result);
            Assert.AreSame(scriptType, result);
            setup.EnumeratorMock.VerifyGet(o => o.Current);
        }

        [TestMethod]
        public void NextPushesConvertedNullValue()
        {
            // Arrange
            var processor = new PyProcessor(
                new ForEachNext(SourceReference.ClrSource, 1),
                new NopOp()
            );

            var setup = new IteratorSetup();
            setup.SetupEnumeratorIsIScriptType();

            setup.EnumeratorMock.SetupGet(o => o.Current)
                .Returns((IScriptType) null)
                .Verifiable();

            setup.EnumeratorMock.Setup(o => o.MoveNext())
                .Returns(true);

            processor.PushValue((IScriptType)setup.EnumeratorMock.Object);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            var result = processor.PopValue();
            Assert.IsNotNull(result);
            Assert.AreSame(processor.Factory.Null, result);
            setup.EnumeratorMock.VerifyGet(o => o.Current);
        }

        [TestMethod]
        public void MoveNextTrueJumps()
        {
            // Arrange
            const bool moveNextReturnValue = true;
            const int expectedProgramCounter = 2;

            var processor = new PyProcessor(
                /* 0 */ new ForEachNext(SourceReference.ClrSource, 2),
                /* 1 */ new NopOp(),
                /* 2 */ new NopOp(),
                /* 3 */ new NopOp()
            );

            var setup = new IteratorSetup();
            setup.SetupEnumeratorIsIScriptType();

            var current = Mock.Of<IScriptType>();
            setup.EnumeratorMock.SetupGet(o => o.Current)
                .Returns(current);

            setup.EnumeratorMock.Setup(o => o.MoveNext())
                .Returns(moveNextReturnValue);

            processor.PushValue((IScriptType)setup.EnumeratorMock.Object);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(expectedProgramCounter, processor.ProgramCounter);

            // Should push `Current` value
            var topValue = processor.PopValue();
            Assert.AreSame(current, topValue);

            var resultEnum = processor.PopValue();
            Assert.AreSame(setup.EnumeratorMock.Object, resultEnum);
        }

        [TestMethod]
        public void MoveNextFalseDoesNothing()
        {
            // Arrange
            const bool moveNextReturnValue = false;
            const int expectedProgramCounter = 1;

            var processor = new PyProcessor(
                /* 0 */ new ForEachNext(SourceReference.ClrSource, 2),
                /* 1 */ new NopOp(),
                /* 2 */ new NopOp(),
                /* 3 */ new NopOp()
            );

            var setup = new IteratorSetup();
            setup.SetupEnumeratorIsIScriptType();

            setup.EnumeratorMock.Setup(o => o.MoveNext())
                .Returns(moveNextReturnValue);

            processor.PushValue((IScriptType)setup.EnumeratorMock.Object);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(expectedProgramCounter, processor.ProgramCounter);

            // Should not push
            var resultEnum = processor.PopValue();
            Assert.AreSame(setup.EnumeratorMock.Object, resultEnum);
        }

        [TestMethod]
        public void ThrowsOnNotEnumerator()
        {
            // Arrange
            var processor = new PyProcessor(
                new ForEachNext(SourceReference.ClrSource, 2)
            );

            processor.PushMockValue();

            // Act
            processor.WalkInstruction(); // warmup
            var ex = Assert.ThrowsException<InternalException>((Action)processor.WalkInstruction);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_ForEach_NextNotEnumerator)
            );
        }
    }
}