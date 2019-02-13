using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Resources;

namespace Zifro.Compiler.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class ScopeTests
    {
        [TestMethod]
        public void PushRunFullNoPopTest()
        {
            // Arrange
            var processor = new PyProcessor();

            // Act
            processor.PushScope();
            var ex = Assert.ThrowsException<InternalException>((Action) processor.WalkInstruction);

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
            var processor = new PyProcessor();

            // Act
            processor.PushScope();
            processor.PushScope();
            processor.PopScope();
            var ex = Assert.ThrowsException<InternalException>((Action)processor.WalkInstruction);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_Scope_LastScopeNotPopped));

            Assert.AreEqual(ProcessState.Error, processor.State);

            Assert.That.ErrorFormatArgsEqual(
                (InterpreterLocalizedException)processor.LastError,
                nameof(Localized_Python3_Interpreter.Ex_Scope_LastScopeNotPopped));
        }

        [TestMethod]
        public void Push1RunFull1PopTest()
        {
            // Arrange
            var processor = new PyProcessor();

            // Act
            processor.PushScope();
            processor.PopScope();
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(ProcessState.Ended, processor.State);
        }

        [TestMethod]
        public void Push2RunFull2PopTest()
        {
            // Arrange
            var processor = new PyProcessor();

            // Act
            processor.PushScope();
            processor.PushScope();
            processor.PopScope();
            processor.PopScope();
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(ProcessState.Ended, processor.State);
        }

        [TestMethod]
        public void PopGlobalScopeTest()
        {
            // Arrange
            var processor = new PyProcessor();

            // Act
            processor.PushScope();
            var ex = Assert.ThrowsException<InternalException>((Action)processor.WalkInstruction);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_Scope_PopGlobal));

            Assert.AreEqual(ProcessState.Error, processor.State);

            Assert.That.ErrorFormatArgsEqual(
                (InterpreterLocalizedException)processor.LastError,
                nameof(Localized_Python3_Interpreter.Ex_Scope_PopGlobal));
        }
    }
}