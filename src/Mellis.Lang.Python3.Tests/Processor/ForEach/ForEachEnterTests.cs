using System;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Tests.TestingOps;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Processor.ForEach
{
    [TestClass]
    public class ForEachEnterTests
    {

        [TestMethod]
        public void EnterPushesIScriptTypeIEnumerator()
        {
            // Arrange
            var processor = new PyProcessor(
                new ForEachEnter(SourceReference.ClrSource)
            );

            var setup = new IteratorSetup();
            setup.SetupEnumeratorIsIScriptType();
            setup.SetupGetEnumerator();

            processor.PushValue(setup.ValueMock.Object);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            var result = processor.PopValue();
            Assert.AreSame(setup.EnumeratorMock.Object, result);

            setup.VerifyAll();
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
            setup.SetupGetEnumerator();

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

            setup.VerifyAll();
        }

        [TestMethod]
        public void ThrowsOnNotIEnumerable()
        {
            // Arrange
            var processor = new PyProcessor(
                new ForEachEnter(SourceReference.ClrSource)
            );

            var valueMock = new Mock<IScriptType>();

            processor.PushValue(valueMock.Object);

            const string typeName = "foo";
            valueMock.Setup(o => o.GetTypeName())
                .Returns(typeName).Verifiable();

            // Act
            processor.WalkInstruction(); // warmup
            var ex = Assert.ThrowsException<RuntimeException>((Action) processor.WalkInstruction);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Runtime.Ex_ForEach_NotIterable),
                typeName);

            valueMock.Verify();
        }
    }
}