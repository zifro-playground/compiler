using System;
using System.Collections.Generic;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Processor.ForEach
{
    [TestClass]
    public class ForEachEnterTests
    {
        private class IteratorSetup
        {
            public readonly Mock<IScriptType> ValueMock;
            public readonly Mock<IEnumerable<IScriptType>> ValueEnumerableMock;
            public readonly Mock<IEnumerator<IScriptType>> EnumeratorMock;

            public IteratorSetup()
            {
                EnumeratorMock = new Mock<IEnumerator<IScriptType>>();

                ValueMock = new Mock<IScriptType>();

                ValueEnumerableMock = ValueMock.As<IEnumerable<IScriptType>>();
            }

            public void SetupGetEnumerable()
            {
                ValueEnumerableMock.Setup(o => o.GetEnumerator())
                    .Returns(EnumeratorMock.Object).Verifiable();
            }

            public void SetupEnumeratorIsIScriptType()
            {
                EnumeratorMock.As<IScriptType>();
            }

            public void Verify()
            {
                ValueMock.Verify();
                EnumeratorMock.Verify();
                ValueEnumerableMock.Verify();
            }
        }

        [TestMethod]
        public void EnterPushesIScriptTypeIEnumerator()
        {
            // Arrange
            var processor = new PyProcessor(
                new ForEachEnter(SourceReference.ClrSource)
            );

            var setup = new IteratorSetup();
            setup.SetupEnumeratorIsIScriptType();
            setup.SetupGetEnumerable();

            processor.PushValue(setup.ValueMock.Object);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            var result = processor.PopValue();
            Assert.AreSame(setup.EnumeratorMock.Object, result);

            setup.Verify();
        }

        [TestMethod]
        public void EnterWrapsIEnumeratorInIScriptType()
        {
            // Arrange
            var processor = new PyProcessor(
                new ForEachEnter(SourceReference.ClrSource)
            );

            var setup = new IteratorSetup();
            //setup.SetupEnumeratorIsIScriptType(); // intentionally left out
            setup.SetupGetEnumerable();

            processor.PushValue(setup.ValueMock.Object);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            var result = processor.PopValue();
            Assert.IsInstanceOfType(result, typeof(PyEnumeratorWrapper));
            var resultWrapper = (PyEnumeratorWrapper)result;
            Assert.AreSame(setup.ValueMock.Object, resultWrapper.SourceType);
            Assert.AreSame(setup.EnumeratorMock.Object, resultWrapper.Enumerator);

            setup.Verify();
        }

        [TestMethod]
        public void NotIEnumerableThrows()
        {
            // Arrange
            var processor = new PyProcessor(
                new ForEachEnter(SourceReference.ClrSource)
            );

            var setup = new IteratorSetup();
            //setup.SetupGetEnumerable(); // intentionally left out
            //setup.SetupEnumeratorIsIScriptType(); // intentionally left out

            processor.PushValue(setup.ValueMock.Object);

            const string typeName = "foo";
            setup.ValueMock.Setup(o => o.GetTypeName())
                .Returns(typeName).Verifiable();

            // Act
            processor.WalkInstruction(); // warmup
            var ex = Assert.ThrowsException<RuntimeException>((Action) processor.WalkInstruction);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Runtime.Ex_ForEach_NotIterable),
                typeName);

            setup.Verify();
        }
    }
}