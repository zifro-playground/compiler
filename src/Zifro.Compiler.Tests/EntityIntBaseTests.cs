using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Entities;
using Zifro.Compiler.Resources;

namespace Zifro.Compiler.Tests
{
    [TestClass]
    public class EntityIntBaseTests
    {
        private Mock<IProcessor> processorMock;
        private Mock<IScriptTypeFactory> factoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            processorMock = new Mock<IProcessor>();
            factoryMock = new Mock<IScriptTypeFactory>();

            processorMock.SetupGet(o => o.Factory).Returns(factoryMock.Object);
            factoryMock.Setup(o => o.Create(It.IsAny<int>()))
                .Returns<int>(GetInteger);
        }

        private IntegerBase GetInteger(int value)
        {
            var mock = new Mock<IntegerBase>();
            mock.SetupGet(o => o.Processor).Returns(processorMock.Object);
            mock.CallBase = true;
            IntegerBase integerBase = mock.Object;
            integerBase.Value = value;
            return integerBase;
        }

        [TestMethod]
        public void IntAdditionTest()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            IntegerBase b = GetInteger(10);

            // Act
            IScriptType resultBase = a.ArithmeticAdd(b);
            var result = resultBase as IntegerBase;

            // Assert
            Assert.IsInstanceOfType(resultBase, typeof(IntegerBase));
            Assert.IsNotNull(result);
            Assert.AreNotSame(a, result);
            Assert.AreNotSame(b, result);
            Assert.AreEqual(15, result.Value);
        }

        [TestMethod]
        public void IntSubtractionTest()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            IntegerBase b = GetInteger(10);

            // Act
            IScriptType resultBase = a.ArithmeticSubtract(b);
            var result = resultBase as IntegerBase;

            // Assert
            Assert.IsInstanceOfType(resultBase, typeof(IntegerBase));
            Assert.IsNotNull(result);
            Assert.AreNotSame(a, result);
            Assert.AreNotSame(b, result);
            Assert.AreEqual(-5, result.Value);
        }

        [TestMethod]
        public void IntMultiplicationTest()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            IntegerBase b = GetInteger(10);

            // Act
            IScriptType resultBase = a.ArithmeticMultiply(b);
            var result = resultBase as IntegerBase;

            // Assert
            Assert.IsInstanceOfType(resultBase, typeof(IntegerBase));
            Assert.IsNotNull(result);
            Assert.AreNotSame(a, result);
            Assert.AreNotSame(b, result);
            Assert.AreEqual(50, result.Value);
        }

        [TestMethod]
        public void IntDivideWholeTest()
        {
            // Arrange
            IntegerBase a = GetInteger(50);
            IntegerBase b = GetInteger(10);

            // Act
            IScriptType resultBase = a.ArithmeticMultiply(b);
            var result = resultBase as IntegerBase;

            // Assert
            Assert.IsInstanceOfType(resultBase, typeof(IntegerBase));
            Assert.IsNotNull(result);
            Assert.AreNotSame(a, result);
            Assert.AreNotSame(b, result);
            Assert.AreEqual(5, result.Value);
        }

        [TestMethod]
        public void IntDivideFractionTest()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            IntegerBase b = GetInteger(10);

            // Act
            IScriptType resultBase = a.ArithmeticMultiply(b);
            var result = resultBase as DoubleBase;

            // Assert
            Assert.IsInstanceOfType(resultBase, typeof(DoubleBase));
            Assert.IsNotNull(result);
            Assert.AreNotSame(a, result);
            Assert.AreNotSame(b, result);
            Assert.AreEqual(0.5, result.Value);
        }

        [TestMethod]
        public void IntDivideByZero()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            IntegerBase b = GetInteger(0);
            Action action = delegate { a.ArithmeticDivide(b); };

            // Act + Assert
            var ex = Assert.ThrowsException<RuntimeException>(action);
            Assert.AreEqual(ex.LocalizeKey, nameof(Localized_Base_Entities.Ex_Math_DivideByZero));
        }

        [TestMethod]
        public void IntIndexGet()
        {
            // Arrange
            IntegerBase a = GetInteger(5);

            Action action = delegate { a.GetIndex(null); };

            // Act + Assert
            var ex = Assert.ThrowsException<RuntimeException>(action);

            Assert.AreEqual(ex.LocalizeKey, nameof(Localized_Base_Entities.Ex_Int_IndexGet));
        }

        [TestMethod]
        public void IntIndexSet()
        {
            // Arrange
            IntegerBase a = GetInteger(5);

            Action action = delegate { a.SetIndex(null, null); };

            // Act + Assert
            var ex = Assert.ThrowsException<RuntimeException>(action);

            Assert.AreEqual(ex.LocalizeKey, nameof(Localized_Base_Entities.Ex_Int_IndexSet));
        }

        [TestMethod]
        public void IntPropertyGet()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            const string property = "prop";

            Action action = delegate { a.GetProperty(property); };

            // Act + Assert
            var ex = Assert.ThrowsException<RuntimeException>(action);

            Assert.AreEqual(ex.LocalizeKey, nameof(Localized_Base_Entities.Ex_Int_PropertyGet));
        }

        [TestMethod]
        public void IntPropertySet()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            const string property = "prop";

            Action action = delegate { a.SetProperty(property, null); };

            // Act + Assert
            var ex = Assert.ThrowsException<RuntimeException>(action);

            Assert.AreEqual(ex.LocalizeKey, nameof(Localized_Base_Entities.Ex_Int_PropertySet));
        }

        [DataTestMethod]
        [DataRow(5)]
        [DataRow(5L)]
        [DataRow(5D)]
        [DataRow(5F)]
        public void IntTryConvertValid(object expected)
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            Type type = expected.GetType();

            // Act
            bool success = a.TryConvert(type, out object result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(typeof(uint))]
        [DataRow(typeof(ulong))]
        [DataRow(typeof(bool))]
        [DataRow(typeof(byte))]
        [DataRow(typeof(short))]
        [DataRow(typeof(char))]
        [DataRow(typeof(string))]
        [DataRow(typeof(DateTime))]
        public void IntTryConvertInvalid(Type type)
        {
            // Arrange
            IntegerBase a = GetInteger(5);

            // Act
            bool success = a.TryConvert(type, out object _);

            // Assert
            Assert.IsFalse(success);
        }
    }
}