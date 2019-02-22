using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Instructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class VarPopEvaluateTests
    {
        [TestMethod]
        public void EvaluateVarPopTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new VarPop(SourceReference.ClrSource)    
            );

            var value = Mock.Of<IScriptType>();

            processor.PushValue(value);

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(0, processor.ValueStackCount);
            Assert.AreEqual(ProcessState.Ended, processor.State);
        }
    }
}