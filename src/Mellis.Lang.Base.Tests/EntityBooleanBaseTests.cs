using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;

namespace Mellis.Lang.Base.Tests
{
    [TestClass]
    public class EntityBooleanBaseTests : BaseTestClass
    {
        [TestMethod]
        public void BooleanAdditionTest()
        {
            // Arrange
            var a = GetBoolean(true);
            var b = GetBoolean(false);

            // Act
            var result = a.ArithmeticAdd(b);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void BooleanSubtractionTest()
        {
            // Arrange
            var a = GetBoolean(true);
            var b = GetBoolean(false);

            // Act
            var result = a.ArithmeticSubtract(b);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void BooleanMultiplicationTest()
        {
            // Arrange
            var a = GetBoolean(true);
            var b = GetBoolean(false);

            // Act
            var result = a.ArithmeticMultiply(b);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void BooleanDivideTest()
        {
            // Arrange
            var a = GetBoolean(true);
            var b = GetBoolean(false);

            // Act
            var result = a.ArithmeticDivide(b);

            // Assert
            Assert.IsNull(result);
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

        protected override IScriptType GetBasicOperand()
        {
            return GetBoolean(true);
        }
    }
}