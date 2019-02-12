using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Entities;
using Zifro.Compiler.Lang.Python3.Resources;

namespace Zifro.Compiler.Lang.Python3.Tests.Processor
{
    [TestClass]
    [DoNotParallelize]
    public class ValueStackTests
    {
        protected PyProcessor processor;

        protected static Mock<IScriptType> GetScriptTypeMock()
        {
            return new Mock<IScriptType>(MockBehavior.Strict);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            processor = new PyProcessor();
        }

        [TestMethod]
        public void AddToStackTest()
        {
            // Arrange
            var valueMock = GetScriptTypeMock();

            // Act
            int before = processor.ValueStackCount;
            processor.PushValue(valueMock.Object);
            int after = processor.ValueStackCount;

            // Assert
            Assert.AreEqual(0, before);
            Assert.AreEqual(1, after);
        }

        [TestMethod]
        public void AddMultipleTest()
        {
            // Arrange
            var valueMock = GetScriptTypeMock();

            // Act
            int before = processor.ValueStackCount;
            processor.PushValue(valueMock.Object);
            processor.PushValue(valueMock.Object);
            processor.PushValue(valueMock.Object);
            int after = processor.ValueStackCount;

            // Assert
            Assert.AreEqual(0, before);
            Assert.AreEqual(3, after);
        }

        [TestMethod]
        public void AddNullTest()
        {
            // Arrange
            void Action()
            {
                processor.PushValue(null);
            }

            // Act
            var ex = Assert.ThrowsException<InterpreterLocalizedException>((Action) Action);

            Assert.That.ErrorFormatArgsEqual(ex,
                "the key");
        }

        [TestMethod]
        public void PopOrderingTest()
        {
            // Arrange
            var valueMock1 = GetScriptTypeMock();
            var valueMock2 = GetScriptTypeMock();
            var valueMock3 = GetScriptTypeMock();
            processor.PushValue(valueMock1.Object);
            processor.PushValue(valueMock2.Object);
            processor.PushValue(valueMock3.Object);

            // Act
            int before = processor.ValueStackCount;
            var result1 = processor.PopValue<IScriptType>();
            var result2 = processor.PopValue<IScriptType>();
            var result3 = processor.PopValue<IScriptType>();
            int after = processor.ValueStackCount;

            // Assert

            // Check first in -> last out
            Assert.AreSame(valueMock3.Object, result1);
            Assert.AreSame(valueMock2.Object, result2);
            Assert.AreSame(valueMock1.Object, result3);

            Assert.AreEqual(3, before);
            Assert.AreEqual(0, after);
        }

        [TestMethod]
        public void PopEmptyTest()
        {
            // Arrange
            void Action()
            {
                processor.PopValue<PyInteger>();
            }

            // Act
            var ex = Assert.ThrowsException<InterpreterLocalizedException>((Action)Action);

            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_ValueStack_PopEmpty),
                nameof(PyInteger));
        }

        [TestMethod]
        public void PopInvalidExpectedTypeTest()
        {
            // Arrange
            processor.PushValue(new PyBoolean(processor, true));

            void Action()
            {
                processor.PopValue<PyInteger>();
            }

            // Act
            var ex = Assert.ThrowsException<InterpreterLocalizedException>((Action)Action);

            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_ValueStack_PopInvalidType),
                nameof(PyInteger), nameof(PyBoolean));
        }
    }
}