using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class VarSetEvaluateTests
    {
        [TestMethod]
        public void EvaluateVarSetTest()
        {
            // Arrange
            const string identifier = "foo";
            var processor = new PyProcessor(
                new VarSet(SourceReference.ClrSource, identifier)
            );

            var rhsMock = new Mock<IScriptType>(MockBehavior.Strict);

            processor.PushValue(rhsMock.Object);

            // Act
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;

            // Assert
            // TODO test processor global scope (should have value)

            Assert.AreEqual(0, numOfValues, "Did not absorb value.");
        }

        [TestMethod]
        public void EvaluateVarSetOverrideTest()
        {
            // Arrange
            const string identifier = "foo";
            var processor = new PyProcessor(
                new VarSet(SourceReference.ClrSource, identifier)
            );

            var rhsMock = new Mock<IScriptType>(MockBehavior.Strict);

            processor.PushValue(rhsMock.Object);
            // TODO set initial value

            // Act
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;

            // Assert
            // TODO test processor global scope (should have new value)

            Assert.AreEqual(0, numOfValues, "Did not absorb value.");
        }

        [TestMethod]
        public void EvaluateVarSetLocalScopeTest()
        {
            // Arrange
            const string identifier = "foo";
            var processor = new PyProcessor(
                // TODO Add increment scope op,
                new VarSet(SourceReference.ClrSource, identifier)
            );

            var rhsMock = new Mock<IScriptType>(MockBehavior.Strict);

            processor.PushValue(rhsMock.Object);

            // Act
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;

            // Assert
            // TODO test processor global scope (should not have value)
            // TODO test processor current scope (should have value)

            Assert.AreEqual(0, numOfValues, "Did not absorb value.");
        }
        
        [TestMethod]
        public void EvaluateVarSetOverrideLocalScopeTest()
        {
            // Arrange
            const string identifier = "foo";
            var processor = new PyProcessor(
                // TODO Add increment scope op,
                new VarSet(SourceReference.ClrSource, identifier)
            );

            var rhsMock = new Mock<IScriptType>(MockBehavior.Strict);

            processor.PushValue(rhsMock.Object);
            // TODO set initial value to global scope

            // Act
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;

            // Assert
            // TODO test processor global scope (should have new value)
            // TODO test processor current scope (should not have value)

            Assert.AreEqual(0, numOfValues, "Did not absorb value.");
        }
    }
}