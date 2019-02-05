using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Entities;
using Zifro.Compiler.Lang.Resources;

namespace Zifro.Compiler.Lang.Tests
{
    [TestClass]
    public class EntityStringBaseTests : BaseTestClass
    {
        [TestMethod]
        public void StringAdditionTest()
        {
            // Arrange
            StringBase a = GetString("foo");
            StringBase b = GetString("bar");

            // Act
            IScriptType resultBase = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<StringBase>(resultBase, a, b, "foobar");
        }

        [TestMethod]
        public void StringAdditionEmptyTest()
        {
            // Arrange
            StringBase a = GetString("");
            StringBase b = GetString("");

            // Act
            IScriptType resultBase = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<StringBase>(resultBase, a, b, "");
        }

        [TestMethod]
        public void StringAdditionInvalidTest()
        {
            // Arrange
            StringBase a = GetString("foo");
            IntegerBase b = GetInteger(5);
            object[] expectedFormatArgs = { a.Value, a.Value.Length, b.GetTypeName() };

            Action action = delegate { a.ArithmeticAdd(b); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_String_AddInvalidType), expectedFormatArgs);
        }

        [TestMethod]
        public void StringSubtractionTest()
        {
            // Arrange
            StringBase a = GetString("foo");
            StringBase b = GetString("bar");
            object[] expectedFormatArgs = { a.Value, a.Value.Length, b.GetTypeName() };

            Action action = delegate { a.ArithmeticSubtract(b); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_String_SubtractInvalidOperation), expectedFormatArgs);
        }

        [TestMethod]
        public void StringMultiplicationTest()
        {
            // Arrange
            StringBase a = GetString("foo");
            StringBase b = GetString("bar");
            object[] expectedFormatArgs = { a.Value, a.Value.Length, b.GetTypeName() };

            Action action = delegate { a.ArithmeticMultiply(b); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_String_MultiplyInvalidOperation), expectedFormatArgs);
        }

        [TestMethod]
        public void StringDivideWholeTest()
        {
            // Arrange
            StringBase a = GetString("foo");
            StringBase b = GetString("bar");
            object[] expectedFormatArgs = { a.Value, a.Value.Length, b.GetTypeName() };

            Action action = delegate { a.ArithmeticDivide(b); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_String_DivideInvalidOperation), expectedFormatArgs);
        }

        [TestMethod]
        public void StringDivideByZeroTest()
        {
            // Arrange
            StringBase a = GetString("foo");
            IntegerBase b = GetInteger(0);
            object[] expectedFormatArgs = { a.Value, a.Value.Length, b.GetTypeName() };

            Action action = delegate { a.ArithmeticDivide(b); };

            // Act + Assert
            // Ensure not the "DIVIDE BY ZERO" error, just regular invalid-operation error
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_String_DivideInvalidOperation), expectedFormatArgs);
        }

        [TestMethod]
        public void StringIndexGetInteger()
        {
            // Arrange
            StringBase a = GetString("foo");
            IntegerBase b = GetInteger(0);

            // Act
            IScriptType result = a.GetIndex(b);

            // Assert
            AssertArithmeticResult<StringBase>(result, a, b, "f");
        }

        [TestMethod]
        public void StringIndexGetIntegerOutOfRange()
        {
            // Arrange
            StringBase a = GetString("foo");
            IntegerBase b = GetInteger(10);
            object[] expectedFormatArgs = { a.Value, a.Value.Length, b.Value };

            Action action = delegate { a.GetIndex(b); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_String_IndexGet_OutOfRange), expectedFormatArgs);
        }

        [TestMethod]
        public void StringIndexGetInvalid()
        {
            // Arrange
            StringBase a = GetString("foo");
            StringBase b = GetString("a");
            object[] expectedFormatArgs = { a.Value, a.Value.Length, b.GetTypeName() };

            Action action = delegate { a.GetIndex(b); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_String_IndexGet_InvalidType), expectedFormatArgs);
        }

        [TestMethod]
        public void StringIndexSet()
        {
            // Arrange
            StringBase a = GetString("foo");
            object[] expectedFormatArgs = { a.Value, a.Value.Length };

            Action action = delegate { a.SetIndex(null, null); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_String_IndexSet), expectedFormatArgs);
        }

        [TestMethod]
        public void StringPropertyGet()
        {
            // Arrange
            StringBase a = GetString("foo");
            const string property = "prop";
            object[] expectedFormatArgs = { a.Value, a.Value.Length, property };

            Action action = delegate { a.GetProperty(property); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_String_PropertyGet), expectedFormatArgs);
        }

        [TestMethod]
        public void StringPropertySet()
        {
            // Arrange
            StringBase a = GetString("foo");
            const string property = "prop";
            object[] expectedFormatArgs = { a.Value, a.Value.Length, property };

            Action action = delegate { a.SetProperty(property, null); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_String_PropertySet), expectedFormatArgs);
        }

        [TestMethod]
        public void StringInvoke()
        {
            // Arrange
            StringBase a = GetString("foo");
            object[] expectedFormatArgs = { a.Value, a.Value.Length };

            Action action = delegate { a.Invoke(null); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_String_Invoke), expectedFormatArgs);
        }

        [DataTestMethod]
        [DataRow(5)]
        [DataRow(5L)]
        [DataRow(5D)]
        [DataRow(5F)]
        public void StringTryConvertValid(object expected)
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
        public void StringTryConvertDecimal()
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
        public void StringTryConvertInvalid(Type type)
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
        public void StringTryConvertGenericValid()
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
        public void StringTryConvertGenericInvalid()
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