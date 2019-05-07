using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Tests
{
    [TestClass]
    public class EntityStringBaseTests : BaseTestClass
    {
        [TestMethod]
        public void StringAdditionTest()
        {
            // Arrange
            var a = GetString("foo");
            var b = GetString("bar");

            // Act
            var resultBase = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<StringBase>(resultBase, a, b, "foobar");
        }

        [TestMethod]
        public void StringAdditionEmptyTest()
        {
            // Arrange
            var a = GetString("");
            var b = GetString("");

            // Act
            var resultBase = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<StringBase>(resultBase, a, b, "");
        }

        [TestMethod]
        public void StringAdditionInvalidTest()
        {
            // Arrange
            var a = GetString("foo");
            var b = GetInteger(5);
            object[] expectedFormatArgs = { a.Value, a.Value.Length, b.GetTypeName() };

            void Action() { a.ArithmeticAdd(b); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_String_AddInvalidType), expectedFormatArgs);
        }

        [TestMethod]
        public void StringSubtractionTest()
        {
            // Arrange
            var a = GetString("foo");
            var b = GetString("bar");
            object[] expectedFormatArgs = { a.Value, a.Value.Length, b.GetTypeName() };

            void Action() { a.ArithmeticSubtract(b); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_String_SubtractInvalidOperation), expectedFormatArgs);
        }

        [TestMethod]
        public void StringMultiplicationTest()
        {
            // Arrange
            var a = GetString("foo");
            var b = GetString("bar");
            object[] expectedFormatArgs = { a.Value, a.Value.Length, b.GetTypeName() };

            void Action() { a.ArithmeticMultiply(b); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_String_MultiplyInvalidOperation), expectedFormatArgs);
        }

        [TestMethod]
        public void StringDivideWholeTest()
        {
            // Arrange
            var a = GetString("foo");
            var b = GetString("bar");
            object[] expectedFormatArgs = { a.Value, a.Value.Length, b.GetTypeName() };

            void Action() { a.ArithmeticDivide(b); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_String_DivideInvalidOperation), expectedFormatArgs);
        }

        [TestMethod]
        public void StringDivideByZeroTest()
        {
            // Arrange
            var a = GetString("foo");
            var b = GetInteger(0);
            object[] expectedFormatArgs = { a.Value, a.Value.Length, b.GetTypeName() };

            void Action() { a.ArithmeticDivide(b); }

            // Act + Assert
            // Ensure not the "DIVIDE BY ZERO" error, just regular invalid-operation error
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_String_DivideInvalidOperation), expectedFormatArgs);
        }

        [TestMethod]
        public void StringIndexGetInteger()
        {
            // Arrange
            var a = GetString("foo");
            var b = GetInteger(0);

            // Act
            var result = a.GetIndex(b);

            // Assert
            AssertArithmeticResult<StringBase>(result, a, b, "f");
        }

        [TestMethod]
        public void StringIndexGetIntegerOutOfRange()
        {
            // Arrange
            var a = GetString("foo");
            var b = GetInteger(10);
            object[] expectedFormatArgs = { a.Value, a.Value.Length, b.Value };

            void Action() { a.GetIndex(b); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_String_IndexGet_OutOfRange), expectedFormatArgs);
        }

        [TestMethod]
        public void StringIndexGetInvalid()
        {
            // Arrange
            var a = GetString("foo");
            var b = GetString("a");
            object[] expectedFormatArgs = { a.Value, a.Value.Length, b.GetTypeName() };

            void Action() { a.GetIndex(b); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_String_IndexGet_InvalidType), expectedFormatArgs);
        }

        [TestMethod]
        public void StringIndexSet()
        {
            // Arrange
            var a = GetString("foo");
            object[] expectedFormatArgs = { a.Value, a.Value.Length };

            void Action() { a.SetIndex(null, null); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_String_IndexSet), expectedFormatArgs);
        }

        [TestMethod]
        public void StringPropertyGet()
        {
            // Arrange
            var a = GetString("foo");
            const string property = "prop";
            object[] expectedFormatArgs = { a.Value, a.Value.Length, property };

            void Action() { a.GetProperty(property); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_String_PropertyGet), expectedFormatArgs);
        }

        [TestMethod]
        public void StringPropertySet()
        {
            // Arrange
            var a = GetString("foo");
            const string property = "prop";
            object[] expectedFormatArgs = { a.Value, a.Value.Length, property };

            void Action() { a.SetProperty(property, null); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_String_PropertySet), expectedFormatArgs);
        }

        [DataTestMethod]
        [DataRow("foo", "foo")]
        [DataRow("f", "f")]
        [DataRow("", "")]
        [DataRow("f", 'f')] // char
        [DataRow("foo", 'f')] // char
        public void StringTryConvertValid(string input, object expected)
        {
            // Arrange
            var a = GetString(input);
            var type = expected.GetType();

            // Act
            bool success = a.TryCoerce(type, out object result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(expected, result);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [DataTestMethod]
        [DataRow("foo", typeof(uint))]
        [DataRow("foo", typeof(int))]
        [DataRow("foo", typeof(ulong))]
        [DataRow("foo", typeof(long))]
        [DataRow("foo", typeof(float))]
        [DataRow("foo", typeof(double))]
        [DataRow("", typeof(char))] // because its empty
        public void StringTryConvertInvalid(string input, Type type)
        {
            // Arrange
            var a = GetString(input);

            // Act
            bool success = a.TryCoerce(type, out object _);

            // Assert
            Assert.IsFalse(success);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void StringTryConvertGenericValid()
        {
            // Arrange
            var a = GetString("foo");

            // Act
            bool success = a.TryCoerce(out string result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual("foo", result);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void StringTryConvertGenericInvalid()
        {
            // Arrange
            var a = GetString("");

            // Act
            bool success = a.TryCoerce(out char _);

            // Assert
            Assert.IsFalse(success);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }
    }
}