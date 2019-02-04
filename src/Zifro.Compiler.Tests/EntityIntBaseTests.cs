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
        public void IntAdditionInvalidTest()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            StringBase b = GetString("foo");
            object[] expectedFormatArgs = {5, b.GetTypeName()};
            Action action = delegate { a.ArithmeticAdd(b); };

            // Act + Assert
            var ex = Assert.ThrowsException<RuntimeException>(action);
            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.LocalizeKey, nameof(Localized_Base_Entities.Ex_Int_AddInvalidType));
            CollectionAssert.AreEqual(ex.FormatArgs, expectedFormatArgs);
            processorMock.VerifyNoOtherCalls();
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
        public void IntSubtractionInvalidTest()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            StringBase b = GetString("foo");
            object[] expectedFormatArgs = {5, b.GetTypeName()};
            Action action = delegate { a.ArithmeticSubtract(b); };

            // Act + Assert
            var ex = Assert.ThrowsException<RuntimeException>(action);
            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.LocalizeKey, nameof(Localized_Base_Entities.Ex_Int_SubtractInvalidType));
            CollectionAssert.AreEqual(ex.FormatArgs, expectedFormatArgs);
            processorMock.VerifyNoOtherCalls();
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
        public void IntMultiplicationInvalidTest()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            StringBase b = GetString("foo");
            object[] expectedFormatArgs = {5, b.GetTypeName()};
            Action action = delegate { a.ArithmeticMultiply(b); };

            // Act + Assert
            var ex = Assert.ThrowsException<RuntimeException>(action);
            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.LocalizeKey, nameof(Localized_Base_Entities.Ex_Int_MultiplyInvalidType));
            CollectionAssert.AreEqual(ex.FormatArgs, expectedFormatArgs);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void IntMultiplicationDoubleWholeTest()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            DoubleBase b = GetDouble(2);
            const int expected = 10;

            // Act
            IScriptType resultBase = a.ArithmeticMultiply(b);
            var result = resultBase as IntegerBase;

            // Assert
            Assert.IsInstanceOfType(resultBase, typeof(IntegerBase));
            Assert.IsNotNull(result);
            Assert.AreNotSame(a, result);
            Assert.AreNotSame(b, result);
            Assert.AreEqual(expected, result.Value);
            processorMock.VerifyGet(o => o.Factory, Times.Once);
            processorMock.VerifyNoOtherCalls();
            factoryMock.Verify(o => o.Create(expected), Times.Once);
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void IntMultiplicationDoubleFractionTest()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            DoubleBase b = GetDouble(2.5);
            const double expected = 5 * 2.5;

            // Act
            IScriptType resultBase = a.ArithmeticMultiply(b);
            var result = resultBase as DoubleBase;

            // Assert
            Assert.IsInstanceOfType(resultBase, typeof(DoubleBase));
            Assert.IsNotNull(result);
            Assert.AreNotSame(a, result);
            Assert.AreNotSame(b, result);
            Assert.AreEqual(expected, result.Value);
            processorMock.VerifyGet(o => o.Factory, Times.Once);
            processorMock.VerifyNoOtherCalls();
            factoryMock.Verify(o => o.Create(expected), Times.Once);
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
        public void IntDivideInvalidTest()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            StringBase b = GetString("foo");
            object[] expectedFormatArgs = {5, b.GetTypeName()};
            Action action = delegate { a.ArithmeticDivide(b); };

            // Act + Assert
            var ex = Assert.ThrowsException<RuntimeException>(action);
            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.LocalizeKey, nameof(Localized_Base_Entities.Ex_Int_DivideInvalidType));
            CollectionAssert.AreEqual(ex.FormatArgs, expectedFormatArgs);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void IntDivideByZero()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            IntegerBase b = GetInteger(0);
            object[] expectedFormatArgs = { };
            Action action = delegate { a.ArithmeticDivide(b); };

            // Act + Assert
            var ex = Assert.ThrowsException<RuntimeException>(action);
            Assert.AreEqual(ex.LocalizeKey, nameof(Localized_Base_Entities.Ex_Math_DivideByZero));
            CollectionAssert.AreEqual(ex.FormatArgs, expectedFormatArgs);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void IntDivideByDouble()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            DoubleBase b = GetDouble(2);

            // Act
            IScriptType resultBase = a.ArithmeticDivide(b);
            var result = resultBase as DoubleBase;

            // Assert
            Assert.IsInstanceOfType(resultBase, typeof(DoubleBase));
            Assert.IsNotNull(result);
            Assert.AreNotSame(a, result);
            Assert.AreNotSame(b, result);
            Assert.AreEqual(2.5d, result.Value, double.Epsilon);
            processorMock.VerifyGet(o => o.Factory, Times.Once);
            processorMock.VerifyNoOtherCalls();
            factoryMock.Verify(o => o.Create(It.IsInRange(2.4999, 2.5001, Range.Exclusive)), Times.Once);
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void IntIndexGet()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            object[] expectedFormatArgs = {5};

            Action action = delegate { a.GetIndex(null); };

            // Act + Assert
            var ex = Assert.ThrowsException<RuntimeException>(action);

            Assert.AreEqual(ex.LocalizeKey, nameof(Localized_Base_Entities.Ex_Int_IndexGet));
            CollectionAssert.AreEqual(ex.FormatArgs, expectedFormatArgs);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void IntIndexSet()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            object[] expectedFormatArgs = {5};

            Action action = delegate { a.SetIndex(null, null); };

            // Act + Assert
            var ex = Assert.ThrowsException<RuntimeException>(action);

            Assert.AreEqual(ex.LocalizeKey, nameof(Localized_Base_Entities.Ex_Int_IndexSet));
            CollectionAssert.AreEqual(ex.FormatArgs, expectedFormatArgs);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void IntPropertyGet()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            const string property = "prop";
            object[] expectedFormatArgs = {5, property};

            Action action = delegate { a.GetProperty(property); };

            // Act + Assert
            var ex = Assert.ThrowsException<RuntimeException>(action);

            Assert.AreEqual(ex.LocalizeKey, nameof(Localized_Base_Entities.Ex_Int_PropertyGet));
            CollectionAssert.AreEqual(ex.FormatArgs, expectedFormatArgs);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void IntPropertySet()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            const string property = "prop";
            object[] expectedFormatArgs = {5, property};

            Action action = delegate { a.SetProperty(property, null); };

            // Act + Assert
            var ex = Assert.ThrowsException<RuntimeException>(action);

            Assert.AreEqual(ex.LocalizeKey, nameof(Localized_Base_Entities.Ex_Int_PropertySet));
            CollectionAssert.AreEqual(ex.FormatArgs, expectedFormatArgs);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void IntInvoke()
        {
            // Arrange
            IntegerBase a = GetInteger(5);
            object[] expectedFormatArgs = {5};

            Action action = delegate { a.Invoke(null); };

            // Act + Assert
            var ex = Assert.ThrowsException<RuntimeException>(action);

            Assert.AreEqual(ex.LocalizeKey, nameof(Localized_Base_Entities.Ex_Int_Invoke));
            CollectionAssert.AreEqual(ex.FormatArgs, expectedFormatArgs);
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

        [TestMethod]
        public void IntTryConvertDecimal()
        {
            // Arrange
            IntegerBase a = GetInteger(5);

            // Act
            bool success = a.TryConvert(typeof(decimal), out object result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(5m, result);
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