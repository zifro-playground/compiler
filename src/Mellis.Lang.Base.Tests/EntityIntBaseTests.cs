using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Tests
{
    [TestClass]
    public class EntityIntBaseTests : BaseTestClass
    {
        [TestMethod]
        public void IntAdditionTest()
        {
            // Arrange
            var a = GetInteger(5);
            var b = GetInteger(10);

            // Act
            var resultBase = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<IntegerBase>(resultBase, a, b, 15);
        }

        [TestMethod]
        public void IntAdditionInvalidTest()
        {
            // Arrange
            var a = GetInteger(5);
            var b = GetString("foo");
            object[] expectedFormatArgs = {5, b.GetTypeName()};
            void Action() { a.ArithmeticAdd(b); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Int_AddInvalidType), expectedFormatArgs);
        }

        [TestMethod]
        public void IntSubtractionTest()
        {
            // Arrange
            var a = GetInteger(5);
            var b = GetInteger(10);

            // Act
            var resultBase = a.ArithmeticSubtract(b);

            // Assert
            AssertArithmeticResult<IntegerBase>(resultBase, a, b, -5);
        }

        [TestMethod]
        public void IntSubtractionInvalidTest()
        {
            // Arrange
            var a = GetInteger(5);
            var b = GetString("foo");
            object[] expectedFormatArgs = {5, b.GetTypeName()};
            void Action() { a.ArithmeticSubtract(b); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Int_SubtractInvalidType), expectedFormatArgs);
        }

        [TestMethod]
        public void IntMultiplicationTest()
        {
            // Arrange
            var a = GetInteger(5);
            var b = GetInteger(10);

            // Act
            var resultBase = a.ArithmeticMultiply(b);

            // Assert
            AssertArithmeticResult<IntegerBase>(resultBase, a, b, 50);
        }

        [TestMethod]
        public void IntMultiplicationInvalidTest()
        {
            // Arrange
            var a = GetInteger(5);
            var b = GetString("foo");
            object[] expectedFormatArgs = {5, b.GetTypeName()};
            void Action() { a.ArithmeticMultiply(b); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Int_MultiplyInvalidType), expectedFormatArgs);
        }

        [TestMethod]
        public void IntMultiplicationDoubleWholeTest()
        {
            // Arrange
            var a = GetInteger(5);
            var b = GetDouble(2);

            // Act
            var resultBase = a.ArithmeticMultiply(b);

            // Assert
            AssertArithmeticResult<IntegerBase>(resultBase, a, b, 10);
        }

        [TestMethod]
        public void IntMultiplicationDoubleFractionTest()
        {
            // Arrange
            var a = GetInteger(5);
            var b = GetDouble(2.5);
            const double expected = 5 * 2.5;

            // Act
            var resultBase = a.ArithmeticMultiply(b);

            // Assert
            AssertArithmeticResult<DoubleBase>(resultBase, a, b, expected);
        }

        [TestMethod]
        public void IntDivideWholeTest()
        {
            // Arrange
            var a = GetInteger(50);
            var b = GetInteger(10);

            // Act
            var resultBase = a.ArithmeticDivide(b);

            // Assert
            AssertArithmeticResult<IntegerBase>(resultBase, a, b, 5);
        }

        [TestMethod]
        public void IntDivideFractionTest()
        {
            // Arrange
            var a = GetInteger(5);
            var b = GetInteger(10);

            // Act
            var resultBase = a.ArithmeticDivide(b);

            // Assert
            AssertArithmeticResult<DoubleBase>(resultBase, a, b, 0.5);
        }

        [TestMethod]
        public void IntDivideInvalidTest()
        {
            // Arrange
            var a = GetInteger(5);
            var b = GetString("foo");
            object[] expectedFormatArgs = {5, b.GetTypeName()};
            void Action() { a.ArithmeticDivide(b); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Int_DivideInvalidType), expectedFormatArgs);
        }

        [TestMethod]
        public void IntDivideByZero()
        {
            // Arrange
            var a = GetInteger(5);
            var b = GetInteger(0);
            object[] expectedFormatArgs = { };
            void Action() { a.ArithmeticDivide(b); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Math_DivideByZero), expectedFormatArgs);
        }

        [TestMethod]
        public void IntDivideByDouble()
        {
            // Arrange
            var a = GetInteger(5);
            var b = GetDouble(2);

            // Act
            var resultBase = a.ArithmeticDivide(b);

            // Assert
            AssertArithmeticResult<DoubleBase>(resultBase, a, b, 2.5);
        }

        [TestMethod]
        public void IntIndexGet()
        {
            // Arrange
            var a = GetInteger(5);
            object[] expectedFormatArgs = {5};

            void Action() { a.GetIndex(null); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Int_IndexGet), expectedFormatArgs);
        }

        [TestMethod]
        public void IntIndexSet()
        {
            // Arrange
            var a = GetInteger(5);
            object[] expectedFormatArgs = {5};

            void Action() { a.SetIndex(null, null); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Int_IndexSet), expectedFormatArgs);
        }

        [TestMethod]
        public void IntPropertyGet()
        {
            // Arrange
            var a = GetInteger(5);
            const string property = "prop";
            object[] expectedFormatArgs = {5, property};

            void Action() { a.GetProperty(property); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Int_PropertyGet), expectedFormatArgs);
        }

        [TestMethod]
        public void IntPropertySet()
        {
            // Arrange
            var a = GetInteger(5);
            const string property = "prop";
            object[] expectedFormatArgs = {5, property};

            void Action() { a.SetProperty(property, null); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Int_PropertySet), expectedFormatArgs);
        }

        [DataTestMethod]
        [DataRow(5)]
        [DataRow(5L)]
        [DataRow(5D)]
        [DataRow(5F)]
        public void IntTryConvertValid(object expected)
        {
            // Arrange
            var a = GetInteger(5);
            var type = expected.GetType();

            // Act
            bool success = a.TryCoerce(type, out object result);

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
            var a = GetInteger(5);

            // Act
            bool success = a.TryCoerce(typeof(decimal), out object result);

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
            var a = GetInteger(5);

            // Act
            bool success = a.TryCoerce(type, out object _);

            // Assert
            Assert.IsFalse(success);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void IntTryConvertGenericValid()
        {
            // Arrange
            var a = GetInteger(5);

            // Act
            bool success = a.TryCoerce(out int result);

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
            var a = GetInteger(5);

            // Act
            bool success = a.TryCoerce(out string _);

            // Assert
            Assert.IsFalse(success);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }
    }
}