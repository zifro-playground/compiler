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
    public class ForEachExitTests
    {
        [TestMethod]
        public void ExitPops()
        {
            // Arrange
            var processor = new PyProcessor(
                new ForEachExit(SourceReference.ClrSource)
            );

            var setup = new IteratorSetup();
            setup.SetupEnumeratorIsIScriptType();

            processor.PushValue((IScriptType)setup.EnumeratorMock.Object);
            processor.PushDisposable(setup.EnumeratorMock.Object);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(0, processor.ValueStackCount);
        }

        [TestMethod]
        public void ThrowsOnNotIEnumerator()
        {
            // Arrange
            var processor = new PyProcessor(
                new ForEachExit(SourceReference.ClrSource)
            );

            processor.PushValue(Mock.Of<IScriptType>());

            // Act
            processor.WalkInstruction(); // warmup
            var ex = Assert.ThrowsException<InternalException>(delegate
            {
                processor.WalkInstruction();
            });

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_ForEach_ExitNotEnumerator));
        }
    }
}