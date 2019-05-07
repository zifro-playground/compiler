using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.VM;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class VarSetEvaluateTests
    {
        protected static Mock<IScriptType> GetSetupScriptType(out IScriptType copy)
        {
            var mock = new Mock<IScriptType>(MockBehavior.Strict);
            copy = new Mock<IScriptType>(MockBehavior.Strict).Object;
            mock.Setup(o => o.Copy(It.IsAny<string>()))
                .Returns(copy).Verifiable();
            return mock;
        }

        [TestMethod]
        public void EvaluateVarSetTest()
        {
            // Arrange
            const string identifier = "foo";
            var processor = new PyProcessor(
                new VarSet(SourceReference.ClrSource, identifier)
            );

            var rhsMock = GetSetupScriptType(
                out var rhsCopy);

            processor.PushValue(rhsMock.Object);

            var globalScope = (PyScope) processor.GlobalScope;

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;

            // Assert
            globalScope.Variables.TryGetValue(identifier, out var globalVar);
            Assert.AreSame(rhsCopy, globalVar);
            Assert.AreNotSame(rhsMock.Object, globalVar);
            rhsMock.Verify();

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

            var rhsMock = GetSetupScriptType(
                out var rhsCopy);
            processor.PushValue(rhsMock.Object);

            var beforeMock = GetSetupScriptType(
                out var beforeCopy);
            var globalScope = (PyScope)processor.GlobalScope;
            globalScope.SetVariable(identifier, beforeMock.Object);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;

            // Assert
            globalScope.Variables.TryGetValue(identifier, out var globalVar);
            Assert.AreSame(rhsCopy, globalVar);
            Assert.AreNotSame(rhsMock.Object, globalVar);
            Assert.AreNotSame(beforeCopy, globalVar);
            Assert.AreNotSame(beforeMock.Object, globalVar);
            rhsMock.Verify();
            beforeMock.Verify();

            Assert.AreEqual(0, numOfValues, "Did not absorb value.");
        }

        [TestMethod]
        public void EvaluateVarSetLocalScopeTest()
        {
            // Arrange
            const string identifier = "foo";
            var processor = new PyProcessor(
                new ScopePush(SourceReference.ClrSource),
                new VarSet(SourceReference.ClrSource, identifier),
                new ScopePop(SourceReference.ClrSource)
            );

            var rhsMock = GetSetupScriptType(
                out var rhsCopy);
            processor.PushValue(rhsMock.Object);

            var globalScope = (PyScope)processor.GlobalScope;

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction(); // push->$scope
            processor.WalkInstruction(); // pop->foo
            var localScope =  processor.CurrentScope;
            processor.WalkInstruction(); // pop->$scope
            int numOfValues = processor.ValueStackCount;

            // Assert
            globalScope.Variables.TryGetValue(identifier, out var globalVar);
            localScope.Variables.TryGetValue(identifier, out var localVar);
            Assert.IsNull(globalVar);
            Assert.AreSame(rhsCopy, localVar);
            Assert.AreNotSame(rhsMock.Object, localVar);
            rhsMock.Verify();

            Assert.AreEqual(0, numOfValues, "Did not absorb value.");
        }
        
        [TestMethod]
        public void EvaluateVarSetOverrideLocalScopeTest()
        {
            // Arrange
            const string identifier = "foo";
            var processor = new PyProcessor(
                new ScopePush(SourceReference.ClrSource),
                new VarSet(SourceReference.ClrSource, identifier),
                new ScopePop(SourceReference.ClrSource)
            );

            var rhsMock = GetSetupScriptType(
                out var rhsCopy);
            processor.PushValue(rhsMock.Object);

            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction();

            var beforeMock = GetSetupScriptType(
                out var beforeCopy);
            var localScope = (PyScope)processor.CurrentScope;
            localScope.SetVariable(identifier, beforeMock.Object);

            // Act
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;

            // Assert
            localScope.Variables.TryGetValue(identifier, out var localVar);
            Assert.AreSame(rhsCopy, localVar);
            Assert.AreNotSame(rhsMock.Object, localVar);
            Assert.AreNotSame(rhsMock.Object, beforeMock.Object);
            Assert.AreNotSame(rhsCopy, beforeMock.Object);
            Assert.AreNotSame(rhsCopy, beforeCopy);

            Assert.AreEqual(0, numOfValues, "Did not absorb value.");
        }
    }
}