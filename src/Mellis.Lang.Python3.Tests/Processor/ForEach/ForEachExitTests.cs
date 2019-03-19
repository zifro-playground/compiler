using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Processor.ForEach
{
    [TestClass]
    public class ForEachExitTests
    {
        [TestMethod]
        public void ExitCallsDispose()
        {
            // Arrange
            var processor = new PyProcessor(
                new ForEachExit(SourceReference.ClrSource)
            );

            var setup = new IteratorSetup();
            setup.SetupEnumeratorIsIScriptType();

            processor.PushValue((IScriptType)setup.EnumeratorMock.Object);

            setup.EnumeratorMock.Setup(o => o.Dispose())
                .Verifiable();

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            setup.EnumeratorMock.Verify();
        }

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

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(0, processor.ValueStackCount);
        }

        [TestMethod]
        public void NotIEnumeratorThrows()
        {
            // Arrange
            var processor = new PyProcessor(
                new ForEachExit(SourceReference.ClrSource)
            );

            var setup = new IteratorSetup();
            setup.SetupEnumeratorIsIScriptType();

            processor.PushValue((IScriptType)setup.EnumeratorMock.Object);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(0, processor.ValueStackCount);
        }
    }
}