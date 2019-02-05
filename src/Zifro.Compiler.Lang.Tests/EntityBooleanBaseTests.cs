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
    public class EntityBooleanBaseTests : BaseTestClass
    {
        private static string LocalizedBool(bool value)
        {
            return value
                ? Localized_Base_Entities.Type_Boolean_True
                : Localized_Base_Entities.Type_Boolean_False;
        }

        [TestMethod]
        public void BooleanAdditionTest()
        {
            // Arrange
            BooleanBase a = GetBoolean(true);
            BooleanBase b = GetBoolean(false);
            object[] expectedFormatArgs = {a.Value, LocalizedBool(a.Value), b.GetTypeName()};

            Action action = delegate { a.ArithmeticAdd(b); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Boolean_AddInvalidOperation),
                expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanSubtractionTest()
        {
            // Arrange
            BooleanBase a = GetBoolean(true);
            BooleanBase b = GetBoolean(false);
            object[] expectedFormatArgs = {a.Value, LocalizedBool(a.Value), b.GetTypeName()};

            Action action = delegate { a.ArithmeticSubtract(b); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Boolean_SubtractInvalidOperation),
                expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanMultiplicationTest()
        {
            // Arrange
            BooleanBase a = GetBoolean(true);
            BooleanBase b = GetBoolean(false);
            object[] expectedFormatArgs = {a.Value, LocalizedBool(a.Value), b.GetTypeName()};

            Action action = delegate { a.ArithmeticMultiply(b); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Boolean_MultiplyInvalidOperation),
                expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanDivideTest()
        {
            // Arrange
            BooleanBase a = GetBoolean(true);
            BooleanBase b = GetBoolean(false);
            object[] expectedFormatArgs = {a.Value, LocalizedBool(a.Value), b.GetTypeName()};

            Action action = delegate { a.ArithmeticDivide(b); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Boolean_DivideInvalidOperation), expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanDivideByZeroTest()
        {
            // Arrange
            BooleanBase a = GetBoolean(true);
            BooleanBase b = GetBoolean(false);
            object[] expectedFormatArgs = {a.Value, LocalizedBool(a.Value), b.GetTypeName()};

            Action action = delegate { a.ArithmeticDivide(b); };

            // Act + Assert
            // Ensure not the "DIVIDE BY ZERO" error, just regular invalid-operation error
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Boolean_DivideInvalidOperation), expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanIndexGet()
        {
            // Arrange
            BooleanBase a = GetBoolean(true);
            object[] expectedFormatArgs = {a.Value, LocalizedBool(a.Value)};

            Action action = delegate { a.GetIndex(null); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Boolean_IndexGet), expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanIndexSet()
        {
            // Arrange
            BooleanBase a = GetBoolean(true);
            object[] expectedFormatArgs = { a.Value, LocalizedBool(a.Value) };

            Action action = delegate { a.SetIndex(null, null); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Boolean_IndexSet), expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanPropertyGet()
        {
            // Arrange
            BooleanBase a = GetBoolean(true);
            const string property = "prop";
            object[] expectedFormatArgs = { a.Value, LocalizedBool(a.Value), property };

            Action action = delegate { a.GetProperty(property); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Boolean_PropertyGet), expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanPropertySet()
        {
            // Arrange
            BooleanBase a = GetBoolean(true);
            const string property = "prop";
            object[] expectedFormatArgs = { a.Value, LocalizedBool(a.Value), property };

            Action action = delegate { a.SetProperty(property, null); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Boolean_PropertySet), expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanInvoke()
        {
            // Arrange
            BooleanBase a = GetBoolean(true);
            object[] expectedFormatArgs = { a.Value, LocalizedBool(a.Value) };

            Action action = delegate { a.Invoke(null); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Boolean_Invoke), expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanTryConvertValid()
        {
            // Arrange
            BooleanBase a = GetBoolean(true);

            // Act
            bool success = a.TryConvert(typeof(bool), out object result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(true, result);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [DataTestMethod]
        [DataRow(true, typeof(uint))]
        [DataRow(true, typeof(int))]
        [DataRow(true, typeof(ulong))]
        [DataRow(true, typeof(long))]
        [DataRow(true, typeof(byte))]
        [DataRow(true, typeof(sbyte))]
        [DataRow(true, typeof(string))]
        [DataRow(true, typeof(char))]
        public void BooleanTryConvertInvalid(bool input, Type type)
        {
            // Arrange
            BooleanBase a = GetBoolean(input);

            // Act
            bool success = a.TryConvert(type, out object _);

            // Assert
            Assert.IsFalse(success);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void BooleanTryConvertGenericValid()
        {
            // Arrange
            BooleanBase a = GetBoolean(true);

            // Act
            bool success = a.TryConvert(out bool result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(true, result);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void BooleanTryConvertGenericInvalid()
        {
            // Arrange
            BooleanBase a = GetBoolean(true);

            // Act
            bool success = a.TryConvert(out char _);

            // Assert
            Assert.IsFalse(success);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }
    }
}