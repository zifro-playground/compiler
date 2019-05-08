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
        protected static IScriptType GetMockedValue()
        {
            var mock = new Mock<IScriptType>(MockBehavior.Strict);
            return mock.Object;
        }

        [TestMethod]
        public void EvaluateVarSetTest()
        {
            // Arrange
            const string identifier = "foo";
            var processor = new PyProcessor(
                new VarSet(SourceReference.ClrSource, identifier)
            );

            var rhsMock = GetMockedValue();

            processor.PushValue(rhsMock);

            var globalScope = (PyScope) processor.GlobalScope;

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;

            // Assert
            globalScope.Variables.TryGetValue(identifier, out var globalVar);
            Assert.AreSame(rhsMock, globalVar);

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

            var rhsMock = GetMockedValue();
            processor.PushValue(rhsMock);

            var beforeMock = GetMockedValue();
            var globalScope = (PyScope)processor.GlobalScope;
            globalScope.SetVariable(identifier, beforeMock);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;

            // Assert
            globalScope.Variables.TryGetValue(identifier, out var globalVar);
            Assert.AreSame(rhsMock, globalVar);
            Assert.AreNotSame(beforeMock, globalVar);

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

            var rhsMock = GetMockedValue();
            processor.PushValue(rhsMock);

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
            Assert.AreSame(rhsMock, localVar);

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

            var rhsMock = GetMockedValue();
            processor.PushValue(rhsMock);

            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction();

            var beforeMock = GetMockedValue();
            var localScope = (PyScope)processor.CurrentScope;
            localScope.SetVariable(identifier, beforeMock);

            // Act
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;

            // Assert
            localScope.Variables.TryGetValue(identifier, out var localVar);
            Assert.AreSame(rhsMock, localVar);
            Assert.AreNotSame(rhsMock, beforeMock);

            Assert.AreEqual(0, numOfValues, "Did not absorb value.");
        }
    }
}