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
            AssertArithmeticResult<ScriptInteger>(resultBase, a, b, 15);
        }

        [TestMethod]
        public void IntAdditionInvalidTest()
        {
            // Arrange
            var a = GetInteger(5);
            var b = GetString("foo");

            // Act
            var result = a.ArithmeticAdd(b);

            // Assert
            Assert.IsNull(result);
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
            AssertArithmeticResult<ScriptInteger>(resultBase, a, b, -5);
        }

        [TestMethod]
        public void IntSubtractionInvalidTest()
        {
            // Arrange
            var a = GetInteger(5);
            var b = GetString("foo");

            // Act
            var result = a.ArithmeticSubtract(b);

            // Assert
            Assert.IsNull(result);
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
            AssertArithmeticResult<ScriptInteger>(resultBase, a, b, 50);
        }

        [TestMethod]
        public void IntMultiplicationInvalidTest()
        {
            // Arrange
            var a = GetInteger(5);
            var b = GetString("foo");

            // Act
            var result = a.ArithmeticMultiply(b);

            // Assert
            Assert.IsNull(result);
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
            AssertArithmeticResult<ScriptInteger>(resultBase, a, b, 10);
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
            AssertArithmeticResult<ScriptDouble>(resultBase, a, b, expected);
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
            AssertArithmeticResult<ScriptInteger>(resultBase, a, b, 5);
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
            AssertArithmeticResult<ScriptDouble>(resultBase, a, b, 0.5);
        }

        [TestMethod]
        public void IntDivideInvalidTest()
        {
            // Arrange
            var a = GetInteger(5);
            var b = GetString("foo");

            // Act
            var result = a.ArithmeticDivide(b);

            // Assert
            Assert.IsNull(result);
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
            AssertArithmeticResult<ScriptDouble>(resultBase, a, b, 2.5);
        }

        protected override IScriptType GetBasicOperand()
        {
            return GetInteger(5);
        }
    }
}