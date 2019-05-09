using Mellis.Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Tests.Entities
{
    [TestClass]
    public class ScriptBooleanTests : ScriptTypeBaseTestClass
    {
        [DataTestMethod]
        [DataRow(true, false, false)]
        [DataRow(false, true, false)]
        [DataRow(false, false, true)]
        [DataRow(true, true, true)]
        public void BooleanCompareEqualBoolean(bool aVal, bool bVal, bool expected)
        {
            // Arrange
            var a = GetScriptBoolean(aVal);
            var b = GetScriptBoolean(bVal);

            // Act
            var result = a.CompareEqual(b);

            // Assert
            AssertArithmeticResult<IScriptBoolean>(result, a, b, expected);
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
            var a = GetScriptBoolean(true);
            var b = GetScriptValue(value);
            const bool expected = false;

            // Act
            var result = a.CompareEqual(b);

            // Assert
            AssertArithmeticResult<IScriptBoolean>(result, a, b, expected);
        }

        [DataTestMethod]
        [DataRow(true, false, true)]
        [DataRow(false, true, true)]
        [DataRow(false, false, false)]
        [DataRow(true, true, false)]
        public void BooleanCompareNotEqualBoolean(bool aVal, bool bVal, bool expected)
        {
            // Arrange
            var a = GetScriptBoolean(aVal);
            var b = GetScriptBoolean(bVal);

            // Act
            var result = a.CompareNotEqual(b);

            // Assert
            AssertArithmeticResult<IScriptBoolean>(result, a, b, expected);
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
            var a = GetScriptBoolean(true);
            var b = GetScriptValue(value);
            const bool expected = true;

            // Act
            var result = a.CompareNotEqual(b);

            // Assert
            AssertArithmeticResult<IScriptBoolean>(result, a, b, expected);
        }

        protected override IScriptType GetBasicOperand()
        {
            return GetScriptBoolean(true);
        }

        protected override IScriptType GetBasicOtherOperandInvalidType()
        {
            return GetScriptNull();
        }
    }
}