using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class ScopeTests
    {
        protected static IScriptType GetSetupCopy()
        {
            var mock = new Mock<IScriptType>(MockBehavior.Strict);
            mock.Setup(o => o.Copy(It.IsAny<string>()))
                .Returns(mock.Object).Verifiable();
            return mock.Object;
        }

        [TestMethod]
        public void PushRunFullNoPopTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new ScopePush(SourceReference.ClrSource)
            );

            // Act
            processor.WalkInstruction(); // to enter first op
            var ex = Assert.ThrowsException<InternalException>((Action) processor.WalkLine);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_Scope_LastScopeNotPopped));

            Assert.AreEqual(ProcessState.Error, processor.State);

            Assert.That.ErrorFormatArgsEqual(
                (InterpreterLocalizedException) processor.LastError,
                nameof(Localized_Python3_Interpreter.Ex_Scope_LastScopeNotPopped));
        }

        [TestMethod]
        public void Push2RunFull1PopTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new ScopePush(SourceReference.ClrSource),
                new ScopePush(SourceReference.ClrSource),
                new ScopePop(SourceReference.ClrSource)
            );

            // Act
            processor.WalkInstruction(); // to enter first op
            var ex = Assert.ThrowsException<InternalException>((Action)processor.WalkLine);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_Scope_LastScopeNotPopped));

            Assert.AreEqual(ProcessState.Error, processor.State);

            Assert.That.ErrorFormatArgsEqual(
                (InternalException)processor.LastError,
                nameof(Localized_Python3_Interpreter.Ex_Scope_LastScopeNotPopped));
        }

        [TestMethod]
        public void Push1RunFull1PopTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new ScopePush(SourceReference.ClrSource),
                new ScopePop(SourceReference.ClrSource)
            );
            var global = processor.GlobalScope;

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction();
            var scope = processor.CurrentScope;
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(ProcessState.Ended, processor.State);
            Assert.AreNotSame(global, scope);
            Assert.AreSame(global, processor.CurrentScope);
        }

        [TestMethod]
        public void Push2RunFull2PopTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new ScopePush(SourceReference.ClrSource),
                new ScopePush(SourceReference.ClrSource),
                new ScopePop(SourceReference.ClrSource),
                new ScopePop(SourceReference.ClrSource)
            );
            var global = processor.GlobalScope;

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction(); // push->$scope
            var scope1 = processor.CurrentScope;
            processor.WalkInstruction(); // push->$scope
            var scope2 = processor.CurrentScope;
            processor.WalkInstruction(); // pop->$scope
            var scope3 = processor.CurrentScope;
            processor.WalkInstruction(); // pop->$scope

            // Assert
            Assert.AreEqual(ProcessState.Ended, processor.State);
            Assert.AreNotSame(global, scope1);
            Assert.AreNotSame(global, scope2);
            Assert.AreNotSame(global, scope3);
            Assert.AreSame(scope1, scope3);
            Assert.AreSame(global, processor.CurrentScope);
        }

        [TestMethod]
        public void Push1RunFull2PopTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new ScopePush(SourceReference.ClrSource),
                new ScopePop(SourceReference.ClrSource),
                new ScopePop(SourceReference.ClrSource)
            );

            // Act
            processor.WalkInstruction(); // to enter first op
            var ex = Assert.ThrowsException<InternalException>((Action)processor.WalkLine);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_Scope_PopGlobal));

            Assert.AreEqual(ProcessState.Error, processor.State);

            Assert.That.ErrorFormatArgsEqual(
                (InternalException)processor.LastError,
                nameof(Localized_Python3_Interpreter.Ex_Scope_PopGlobal));
        }

        [TestMethod]
        public void PopGlobalScopeTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new ScopePop(SourceReference.ClrSource)
            );

            // Act
            processor.WalkInstruction(); // to enter first op
            var ex = Assert.ThrowsException<InternalException>((Action)processor.WalkLine);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_Scope_PopGlobal));

            Assert.AreEqual(ProcessState.Error, processor.State);

            Assert.That.ErrorFormatArgsEqual(
                (InterpreterLocalizedException)processor.LastError,
                nameof(Localized_Python3_Interpreter.Ex_Scope_PopGlobal));
        }

        [TestMethod]
        public void SetGlobalValueInGlobalScopeTest()
        {
            // Arrange
            var value = GetSetupCopy();
            var processor = new PyProcessor();
            var globalScope = (PyScope)processor.GlobalScope;

            // Act
            processor.SetGlobalVariable("foo", value);
            var actual = globalScope.GetVariable("foo");

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreSame(value, actual);
        }

        [TestMethod]
        public void SetGlobalValueInLocalScopeTest()
        {
            // Arrange
            var value = GetSetupCopy();
            var processor = new PyProcessor();
            var globalScope = (PyScope)processor.GlobalScope;
            processor.PushScope();
            var localScope = (PyScope)processor.CurrentScope;
            processor.PushScope();

            // Act
            processor.SetGlobalVariable("foo", value);

            var fromGlobal = globalScope.GetVariable("foo");
            var fromLocal = localScope.GetVariable("foo");

            // Assert
            Assert.IsNotNull(fromGlobal);
            Assert.IsNull(fromLocal);
            Assert.AreSame(value, fromGlobal);
        }

        [TestMethod]
        public void SetLocalValueInGlobalScopeTest()
        {
            // Arrange
            var value = GetSetupCopy();
            var processor = new PyProcessor();
            var globalScope = (PyScope)processor.GlobalScope;

            // Act
            processor.SetVariable("foo", value);

            var actual = globalScope.GetVariable("foo");

            // Assert
            Assert.IsNotNull(actual, "global value was null");
            Assert.AreSame(value, actual, "value was not same as mock");
        }

        [TestMethod]
        public void SetLocalValueInLocalScopeTest()
        {
            // Arrange
            var value = GetSetupCopy();
            var processor = new PyProcessor();
            var globalScope = (PyScope)processor.GlobalScope;
            processor.PushScope();
            var localScope = (PyScope) processor.CurrentScope;

            // Act
            processor.SetVariable("foo", value);

            var fromGlobal = globalScope.GetVariable("foo");
            var fromLocal = localScope.GetVariable("foo");

            // Assert
            Assert.IsNull(fromGlobal, "fromGlobal was not null");
            Assert.IsNotNull(fromLocal, "fromLocal was null");
            Assert.AreSame(value, fromLocal, "fromLocal was not same as mock");
        }

        [TestMethod]
        public void OverwriteValueTest()
        {
            // Arrange
            var valueBefore = GetSetupCopy();
            var valueAfter = GetSetupCopy();
            var processor = new PyProcessor();
            var globalScope = (PyScope)processor.GlobalScope;
            globalScope.SetVariable("foo", valueBefore);

            // Act
            processor.SetVariable("foo", valueAfter);

            var actual = globalScope.GetVariable("foo");

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreSame(valueAfter, actual, "Value did not get overwritten");
            Assert.AreNotSame(valueBefore, actual);
        }
        
        [TestMethod]
        public void GetGlobalValueWithinLocalScopeTest()
        {
            // Arrange
            var value = GetSetupCopy();
            var processor = new PyProcessor();
            var globalScope = (PyScope)processor.GlobalScope;
            processor.PushScope();
            globalScope.SetVariable("foo", value);

            // Act
            IScriptType result = processor.GetVariable("foo");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(value, result);
        }

        [TestMethod]
        public void GetValueInGlobalScopeTest()
        {
            // Arrange
            var value = GetSetupCopy();
            var processor = new PyProcessor();
            var globalScope = (PyScope)processor.GlobalScope;
            globalScope.SetVariable("foo", value);

            // Act
            IScriptType result = processor.GetVariable("foo");

            // Assert
            Assert.IsNotNull(result, "global value was null");
            Assert.AreSame(value, result, "value was not same as mock");
        }

        [TestMethod]
        public void GetValueFromLocalScopeTest()
        {
            // Arrange
            var value = GetSetupCopy();
            var processor = new PyProcessor();
            processor.PushScope();
            var localScope = (PyScope)processor.CurrentScope;
            localScope.SetVariable("foo", value);

            // Act
            IScriptType result = processor.GetVariable("foo");

            // Assert
            Assert.IsNotNull(result, "fromLocal was not null");
            Assert.AreSame(value, result, "fromLocal was not same as mock");
        }

        [TestMethod]
        public void GetNonExistingTest()
        {
            // Arrange
            var processor = new PyProcessor();

            processor.PushScope();

            void Action()
            {
                processor.GetVariable("foo");
            }

            // Act
            var ex = Assert.ThrowsException<RuntimeVariableNotDefinedException>((Action)Action);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Runtime.Ex_Variable_NotDefined),
                "foo");
        }

        [TestMethod]
        public void GetValueFromOldLocalTest()
        {
            // Arrange
            var value = Mock.Of<IScriptType>();
            var processor = new PyProcessor();

            processor.PushScope();
            processor.SetVariable("foo", value);
            processor.PopScope();

            void Action()
            {
                processor.GetVariable("foo");
            }

            // Act
            var ex = Assert.ThrowsException<RuntimeVariableNotDefinedException>((Action)Action);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Runtime.Ex_Variable_NotDefined),
                "foo");
        }
    }
}