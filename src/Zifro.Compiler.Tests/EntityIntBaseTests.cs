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
    public class EntityIntBaseTests : BaseTestClass
    {

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
            processorMock.VerifyGet(o => o.Factory, Times.Once);
            processorMock.VerifyNoOtherCalls();
            factoryMock.Verify(o => o.Create(15), Times.Once);
            factoryMock.VerifyNoOtherCalls();
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
            processorMock.VerifyGet(o => o.Factory, Times.Once);
            processorMock.VerifyNoOtherCalls();
            factoryMock.Verify(o => o.Create(-5), Times.Once);
            factoryMock.VerifyNoOtherCalls();
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
            processorMock.VerifyGet(o => o.Factory, Times.Once);
            processorMock.VerifyNoOtherCalls();
            factoryMock.Verify(o => o.Create(50), Times.Once);
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void IntDivideWholeTest()
        {
            // Arrange
            IntegerBase a = GetInteger(50);
            IntegerBase b = GetInteger(10);

            // Act
            IScriptType resultBase = a.ArithmeticDivide(b);
            var result = resultBase as IntegerBase;

            // Assert
            Assert.IsInstanceOfType(resultBase, typeof(IntegerBase));
            Assert.IsNotNull(result);
            Assert.AreNotSame(a, result);
            Assert.AreNotSame(b, result);
            Assert.AreEqual(5, result.Value);
            processorMock.VerifyGet(o => o.Factory, Times.Once);
            processorMock.VerifyNoOtherCalls();
            factoryMock.Verify(o => o.Create(5), Times.Once);
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void IntDivideFractionTest()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            IntegerBase b = GetInteger(10);

            // Act
            IScriptType resultBase = a.ArithmeticDivide(b);
            var result = resultBase as DoubleBase;

            // Assert
            Assert.IsInstanceOfType(resultBase, typeof(DoubleBase));
            Assert.IsNotNull(result);
            Assert.AreNotSame(a, result);
            Assert.AreNotSame(b, result);
            Assert.AreEqual(0.5, result.Value);
            processorMock.VerifyGet(o => o.Factory, Times.Once);
            processorMock.VerifyNoOtherCalls();
            factoryMock.Verify(o => o.Create(0.5d), Times.Once);
            factoryMock.VerifyNoOtherCalls();
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
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
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
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
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
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
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
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
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
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void IntInvoke()
        {
            // Arrange
            IntegerBase a = GetInteger(5);

            Action action = delegate { a.Invoke(null); };

            // Act + Assert
            var ex = Assert.ThrowsException<RuntimeException>(action);

            Assert.AreEqual(ex.LocalizeKey, nameof(Localized_Base_Entities.Ex_Int_Invoke));
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
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
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
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
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void IntTryConvertGenericValid()
        {
            // Arrange
            IntegerBase a = GetInteger(5);

            // Act
            bool success = a.TryConvert(out int result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(5, result);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void IntTryConvertGenericInvalid()
        {
            // Arrange
            IntegerBase a = GetInteger(5);

            // Act
            bool success = a.TryConvert(out string _);

            // Assert
            Assert.IsFalse(success);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }
    }
}