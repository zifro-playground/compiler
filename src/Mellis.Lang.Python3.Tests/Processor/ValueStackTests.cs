using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Resources;
using Moq;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    [DoNotParallelize]
    public class ValueStackTests
    {
        protected VM.PyProcessor processor;

        protected PyInteger GetInteger(int value)
        {
            return new PyInteger(processor, value);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            processor = new VM.PyProcessor();
        }

        [TestMethod]
        public void AddToStackTest()
        {
            // Arrange
            var integer = GetInteger(5);

            // Act
            int before = processor.ValueStackCount;
            processor.PushValue(integer);
            int after = processor.ValueStackCount;

            // Assert
            Assert.AreEqual(0, before);
            Assert.AreEqual(1, after);
        }

        [TestMethod]
        public void AddMultipleTest()
        {
            // Arrange
            var integer = GetInteger(5);

            // Act
            int before = processor.ValueStackCount;
            processor.PushValue(integer);
            processor.PushValue(integer);
            processor.PushValue(integer);
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
            var ex = Assert.ThrowsException<InternalException>((Action) Action);

            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_ValueStack_PushNull));
        }

        [TestMethod]
        public void PopOrderingTest()
        {
            // Arrange
            var valueMock1 = GetInteger(5);
            var valueMock2 = GetInteger(5);
            var valueMock3 = GetInteger(5);
            processor.PushValue(valueMock1);
            processor.PushValue(valueMock2);
            processor.PushValue(valueMock3);

            // Act
            int before = processor.ValueStackCount;
            var result1 = processor.PopValue();
            var result2 = processor.PopValue();
            var result3 = processor.PopValue();
            int after = processor.ValueStackCount;

            // Assert

            // Check first in -> last out
            Assert.AreSame(valueMock3, result1);
            Assert.AreSame(valueMock2, result2);
            Assert.AreSame(valueMock1, result3);

            Assert.AreEqual(3, before);
            Assert.AreEqual(0, after);
        }

        [TestMethod]
        public void PopEmptyTest()
        {
            // Arrange
            void Action()
            {
                processor.PopValue();
            }

            // Act
            var ex = Assert.ThrowsException<InternalException>((Action)Action);

            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_ValueStack_PopEmpty));
        }

        [TestMethod]
        public void PeekKeepsItemOnStack()
        {
            // Arrange
            var value = Mock.Of<IScriptType>();
            processor.PushValue(value);

            // Act
            int before = processor.ValueStackCount;
            var result = processor.PeekValue();
            int after = processor.ValueStackCount;

            // Assert
            Assert.AreEqual(1, before);
            Assert.AreEqual(1, after);
            Assert.AreSame(value, result);
        }
    }
}