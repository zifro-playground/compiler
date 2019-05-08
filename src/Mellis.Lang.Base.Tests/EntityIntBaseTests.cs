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
        public void IntAddTest()
        {
            // Arrange
            var a = GetScriptInteger(5);
            var b = GetScriptInteger(10);

            // Act
            var resultBase = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<ScriptInteger>(resultBase, a, b, 15);
        }

        [TestMethod]
        public void IntSubtractTest()
        {
            // Arrange
            var a = GetScriptInteger(5);
            var b = GetScriptInteger(10);

            // Act
            var resultBase = a.ArithmeticSubtract(b);

            // Assert
            AssertArithmeticResult<ScriptInteger>(resultBase, a, b, -5);
        }

        [TestMethod]
        public void IntMultiplyTest()
        {
            // Arrange
            var a = GetScriptInteger(5);
            var b = GetScriptInteger(10);

            // Act
            var resultBase = a.ArithmeticMultiply(b);

            // Assert
            AssertArithmeticResult<ScriptInteger>(resultBase, a, b, 50);
        }

        [TestMethod]
        public void IntMultiplyDoubleWholeTest()
        {
            // Arrange
            var a = GetScriptInteger(5);
            var b = GetScriptDouble(2);

            // Act
            var resultBase = a.ArithmeticMultiply(b);

            // Assert
            AssertArithmeticResult<ScriptInteger>(resultBase, a, b, 10);
        }

        [TestMethod]
        public void IntMultiplyDoubleFractionTest()
        {
            // Arrange
            var a = GetScriptInteger(5);
            var b = GetScriptDouble(2.5);
            const double expected = 5 * 2.5;

            // Act
            var resultBase = a.ArithmeticMultiply(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(resultBase, a, b, expected);
        }

        [TestMethod]
        public void IntDivideWholeTest()
        {
            // Arrange
            var a = GetScriptInteger(50);
            var b = GetScriptInteger(10);

            // Act
            var resultBase = a.ArithmeticDivide(b);

            // Assert
            AssertArithmeticResult<ScriptInteger>(resultBase, a, b, 5);
        }

        [TestMethod]
        public void IntDivideFractionTest()
        {
            // Arrange
            var a = GetScriptInteger(5);
            var b = GetScriptInteger(10);

            // Act
            var resultBase = a.ArithmeticDivide(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(resultBase, a, b, 0.5);
        }

        [TestMethod]
        public void IntDivideByZero()
        {
            // Arrange
            var a = GetScriptInteger(5);
            var b = GetScriptInteger(0);
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

        [TestMethod]
        public void IntDivideByDouble()
        {
            // Arrange
            var a = GetScriptInteger(5);
            var b = GetScriptDouble(2);

            // Act
            var resultBase = a.ArithmeticDivide(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(resultBase, a, b, 2.5);
        }

        public override void ArithmeticUnaryPositive_NotImplemented()
        {
            // disable
        }

        [TestMethod]
        public void IntArithmeticUnaryPositive()
        {
            // Arrange
            var a = GetScriptInteger(5);
            const int expected = +5;

            // Act
            var result = a.ArithmeticUnaryPositive();

            // Assert
            AssertAreEqual(expected, result);
        }

        public override void ArithmeticUnaryNegative_NotImplemented()
        {
            // disable
        }

        [TestMethod]
        public void IntArithmeticUnaryNegative()
        {
            // Arrange
            var a = GetScriptInteger(5);
            const int expected = -5;

            // Act
            var result = a.ArithmeticUnaryNegative();

            // Assert
            AssertAreEqual(expected, result);
        }

        public override void BinaryNot_NotImplemented()
        {
            // disable
        }

        [TestMethod]
        public void IntBinaryNot()
        {
            // Arrange
            var a = GetScriptInteger(5);
            const int expected = ~5;

            // Act
            var result = a.BinaryNot();

            // Assert
            AssertAreEqual(expected, result);
        }

        protected override IScriptType GetBasicOperand()
        {
            return GetScriptInteger(5);
        }

        protected override IScriptType GetBasicOtherOperandInvalidType()
        {
            return GetScriptNull();
        }
    }
}