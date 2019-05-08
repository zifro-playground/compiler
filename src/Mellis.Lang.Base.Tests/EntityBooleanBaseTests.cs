using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Tests
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
            var a = GetBoolean(true);
            var b = GetBoolean(false);
            object[] expectedFormatArgs = {a.Value, LocalizedBool(a.Value), b.GetTypeName()};

            void Action() { a.ArithmeticAdd(b); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Boolean_AddInvalidOperation),
                expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanSubtractionTest()
        {
            // Arrange
            var a = GetBoolean(true);
            var b = GetBoolean(false);
            object[] expectedFormatArgs = {a.Value, LocalizedBool(a.Value), b.GetTypeName()};

            void Action() { a.ArithmeticSubtract(b); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Boolean_SubtractInvalidOperation),
                expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanMultiplicationTest()
        {
            // Arrange
            var a = GetBoolean(true);
            var b = GetBoolean(false);
            object[] expectedFormatArgs = {a.Value, LocalizedBool(a.Value), b.GetTypeName()};

            void Action() { a.ArithmeticMultiply(b); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Boolean_MultiplyInvalidOperation),
                expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanDivideTest()
        {
            // Arrange
            var a = GetBoolean(true);
            var b = GetBoolean(false);
            object[] expectedFormatArgs = {a.Value, LocalizedBool(a.Value), b.GetTypeName()};

            void Action() { a.ArithmeticDivide(b); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Boolean_DivideInvalidOperation), expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanDivideByZeroTest()
        {
            // Arrange
            var a = GetBoolean(true);
            var b = GetBoolean(false);
            object[] expectedFormatArgs = {a.Value, LocalizedBool(a.Value), b.GetTypeName()};

            void Action() { a.ArithmeticDivide(b); }

            // Act + Assert
            // Ensure not the "DIVIDE BY ZERO" error, just regular invalid-operation error
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Boolean_DivideInvalidOperation), expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanIndexGet()
        {
            // Arrange
            var a = GetBoolean(true);
            object[] expectedFormatArgs = {a.Value, LocalizedBool(a.Value)};

            void Action() { a.GetIndex(null); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Boolean_IndexGet), expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanIndexSet()
        {
            // Arrange
            var a = GetBoolean(true);
            object[] expectedFormatArgs = { a.Value, LocalizedBool(a.Value) };

            void Action() { a.SetIndex(null, null); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Boolean_IndexSet), expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanPropertyGet()
        {
            // Arrange
            var a = GetBoolean(true);
            const string property = "prop";
            object[] expectedFormatArgs = { a.Value, LocalizedBool(a.Value), property };

            void Action() { a.GetProperty(property); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Boolean_PropertyGet), expectedFormatArgs);
        }

        [TestMethod]
        public void BooleanPropertySet()
        {
            // Arrange
            var a = GetBoolean(true);
            const string property = "prop";
            object[] expectedFormatArgs = { a.Value, LocalizedBool(a.Value), property };

            void Action() { a.SetProperty(property, null); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Boolean_PropertySet), expectedFormatArgs);
        }

        [DataTestMethod]
        [DataRow(true, false, false)]
        [DataRow(false, true, false)]
        [DataRow(false, false, true)]
        [DataRow(true, true, true)]
        public void BooleanCompareEqualBoolean(bool aVal, bool bVal, bool expected)
        {
            // Arrange
            var a = GetBoolean(aVal);
            var b = GetBoolean(bVal);

            // Act
            var result = a.CompareEqual(b);

            // Assert
            AssertArithmeticResult<ScriptBoolean>(result, a, b, expected);
        }

        [DataTestMethod]
        [DataRow("foo")]
        [DataRow("True")]
        [DataRow(1)]
        [DataRow(0)]
        [DataRow(1.5)]
        public void BooleanCompareEqualOther(object value)
        {
            // Arrange
            var a = GetBoolean(true);
            var b = GetValue(value);
            const bool expected = false;

            // Act
            var result = a.CompareEqual(b);

            // Assert
            AssertArithmeticResult<ScriptBoolean>(result, a, b, expected);
        }

        [DataTestMethod]
        [DataRow(true, false, true)]
        [DataRow(false, true, true)]
        [DataRow(false, false, false)]
        [DataRow(true, true, false)]
        public void BooleanCompareNotEqualBoolean(bool aVal, bool bVal, bool expected)
        {
            // Arrange
            var a = GetBoolean(aVal);
            var b = GetBoolean(bVal);

            // Act
            var result = a.CompareNotEqual(b);

            // Assert
            AssertArithmeticResult<ScriptBoolean>(result, a, b, expected);
        }

        [DataTestMethod]
        [DataRow("foo")]
        [DataRow("True")]
        [DataRow(1)]
        [DataRow(0)]
        [DataRow(1.5)]
        public void BooleanCompareNotEqualOther(object value)
        {
            // Arrange
            var a = GetBoolean(true);
            var b = GetValue(value);
            const bool expected = true;

            // Act
            var result = a.CompareNotEqual(b);

            // Assert
            AssertArithmeticResult<ScriptBoolean>(result, a, b, expected);
        }
    }
}