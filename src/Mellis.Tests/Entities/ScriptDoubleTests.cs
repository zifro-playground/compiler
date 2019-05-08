using Mellis.Core.Interfaces;
using Mellis.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Tests.Entities
{
    [TestClass]
    public class ScriptDoubleTests : ScriptTypeBaseTestClass
    {
        [TestMethod]
        public void DoubleAddWholeTest()
        {
            // Arrange
            var a = GetScriptDouble(5);
            var b = GetScriptDouble(10);

            // Act
            var result = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(result, a, b, 15d);
        }

        [TestMethod]
        public void DoubleAddFractionTest()
        {
            // Arrange
            var a = GetScriptDouble(0.5);
            var b = GetScriptDouble(1);

            // Act
            var result = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(result, a, b, 1.5d);
        }

        [TestMethod]
        public void DoubleAddIntegerTest()
        {
            // Arrange
            var a = GetScriptDouble(0.5);
            var b = GetScriptInteger(1);

            // Act
            var result = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(result, a, b, 1.5d);
        }

        [TestMethod]
        public void DoubleSubtractWholeTest()
        {
            // Arrange
            var a = GetScriptDouble(5);
            var b = GetScriptDouble(10);

            // Act
            var result = a.ArithmeticSubtract(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(result, a, b, -5d);
        }

        [TestMethod]
        public void DoubleSubtractFractionTest()
        {
            // Arrange
            var a = GetScriptDouble(.5);
            var b = GetScriptDouble(.1);

            // Act
            var result = a.ArithmeticSubtract(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(result, a, b, .4d);
        }

        [TestMethod]
        public void DoubleSubtractIntegerTest()
        {
            // Arrange
            var a = GetScriptDouble(5);
            var b = GetScriptInteger(1);

            // Act
            var result = a.ArithmeticSubtract(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(result, a, b, 4d);
        }

        [TestMethod]
        public void DoubleMultiplyTest()
        {
            // Arrange
            var a = GetScriptDouble(5);
            var b = GetScriptDouble(10);

            // Act
            var result = a.ArithmeticMultiply(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(result, a, b, 50d);
        }

        [TestMethod]
        public void DoubleMultiplyIntegerTest()
        {
            // Arrange
            var a = GetScriptDouble(5);
            var b = GetScriptInteger(10);

            // Act
            var result = a.ArithmeticMultiply(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(result, a, b, 50d);
        }

        [TestMethod]
        public void DoubleDivideWholeTest()
        {
            // Arrange
            var a = GetScriptDouble(50);
            var b = GetScriptDouble(10);

            // Act
            var result = a.ArithmeticDivide(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(result, a, b, 5d);
        }

        [TestMethod]
        public void DoubleDivideFractionTest()
        {
            // Arrange
            var a = GetScriptDouble(5);
            var b = GetScriptDouble(10);

            // Act
            var result = a.ArithmeticDivide(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(result, a, b, .5);
        }

        [TestMethod]
        public void DoubleDivideIntegerTest()
        {
            // Arrange
            var a = GetScriptDouble(50);
            var b = GetScriptInteger(10);

            // Act
            var result = a.ArithmeticDivide(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(result, a, b, 5d);
        }

        [TestMethod]
        public void DoubleDivideByZero()
        {
            // Arrange
            var a = GetScriptDouble(5);
            var b = GetScriptDouble(0);
            object[] expectedFormatArgs = { };

            void Action()
            {
                a.ArithmeticDivide(b);
            }

            // Act + Assert
            AssertThrow(Action,
                nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                expectedFormatArgs);
        }

        public override void ArithmeticUnaryPositive_NotImplemented()
        {
            // disable
        }

        public override void ArithmeticUnaryNegative_NotImplemented()
        {
            // disable
        }

        [TestMethod]
        public void IntArithmeticUnaryPositive()
        {
            // Arrange
            var a = GetScriptDouble(5);
            const double expected = +5;

            // Act
            var result = a.ArithmeticUnaryPositive();

            // Assert
            AssertAreEqual(expected, result);
        }

        [TestMethod]
        public void IntArithmeticUnaryNegative()
        {
            // Arrange
            var a = GetScriptDouble(5);
            const double expected = -5;

            // Act
            var result = a.ArithmeticUnaryNegative();

            // Assert
            AssertAreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(double.PositiveInfinity, nameof(Localized_Base_Entities.Type_Double_PosInfinity))]
        [DataRow(double.NegativeInfinity, nameof(Localized_Base_Entities.Type_Double_NegInfinity))]
        [DataRow(double.NaN, nameof(Localized_Base_Entities.Type_Double_NaN))]
        public void DoubleToStringSpecial(double value, string expectedKey)
        {
            // Arrange
            var scriptType = GetScriptDouble(value);
            string expected = Localized_Base_Entities.ResourceManager.GetString(expectedKey);

            // Act
            string result = scriptType.ToString();

            // Assert
            Assert.AreEqual(expected, result);
        }

        protected override IScriptType GetBasicOperand()
        {
            return GetScriptDouble(5d);
        }

        protected override IScriptType GetBasicOtherOperandInvalidType()
        {
            return GetScriptNull();
        }
    }
}