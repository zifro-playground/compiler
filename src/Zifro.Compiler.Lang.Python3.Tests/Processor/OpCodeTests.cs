using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Entities;
using Zifro.Compiler.Lang.Python3.Instructions;
using Zifro.Compiler.Lang.Python3.Syntax.Literals;

namespace Zifro.Compiler.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class OpCodeTests
    {
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
        }
    }
}