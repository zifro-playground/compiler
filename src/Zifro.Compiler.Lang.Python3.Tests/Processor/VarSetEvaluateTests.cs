using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Instructions;
using Zifro.Compiler.Lang.Python3.Tests.TestingOps;

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

            var globalScope = (PyScope) processor.GlobalScope;

            // Act
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;

            // Assert
            globalScope.Variables.TryGetValue(identifier, out IScriptType globalVar);
            Assert.AreSame(rhsMock.Object, globalVar);

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

            var rhs = Mock.Of<IScriptType>();
            processor.PushValue(rhs);

            var before = Mock.Of<IScriptType>();
            var globalScope = (PyScope)processor.GlobalScope;
            globalScope.SetVariable(identifier, before);

            // Act
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;

            // Assert
            globalScope.Variables.TryGetValue(identifier, out IScriptType globalVar);
            Assert.AreSame(rhs, globalVar);
            Assert.AreNotSame(before, globalVar);

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

            var rhs = Mock.Of<IScriptType>();
            processor.PushValue(rhs);

            var globalScope = (PyScope)processor.GlobalScope;

            // Act
            processor.WalkInstruction();
            processor.WalkInstruction();
            IScopeContext localScope =  processor.CurrentScope;
            processor.WalkInstruction();
            int numOfValues = processor.ValueStackCount;

            // Assert
            globalScope.Variables.TryGetValue(identifier, out IScriptType globalVar);
            localScope.Variables.TryGetValue(identifier, out IScriptType localVar);
            Assert.IsNull(globalVar);
            Assert.AreSame(rhs, localVar);

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

            var rhs = Mock.Of<IScriptType>();
            processor.PushValue(rhs);

            processor.WalkInstruction();

            var before = Mock.Of<IScriptType>();
            var localScope = (PyScope)processor.CurrentScope;
            localScope.SetVariable(identifier, before);

            // Act
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;

            // Assert
            localScope.Variables.TryGetValue(identifier, out IScriptType localVar);
            Assert.AreSame(rhs, localVar);
            Assert.AreNotSame(rhs, before);

            Assert.AreEqual(0, numOfValues, "Did not absorb value.");
        }
    }
}