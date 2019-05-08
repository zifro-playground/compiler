using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Tests
{
    [TestClass]
    public class EntityDoubleBaseTests : BaseTestClass
    {
        [TestMethod]
        public void DoubleAdditionWholeTest()
        {
            // Arrange
            var a = GetDouble(5);
            var b = GetDouble(10);

            // Act
            var result = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<ScriptInteger>(result, a, b, 15);
        }

        [TestMethod]
        public void DoubleAdditionFractionTest()
        {
            // Arrange
            var a = GetDouble(0.5);
            var b = GetDouble(1);

            // Act
            var result = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(result, a, b, 1.5);
        }

        [TestMethod]
        public void DoubleAdditionIntegerTest()
        {
            // Arrange
            var a = GetDouble(0.5);
            var b = GetInteger(1);

            // Act
            var result = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(result, a, b, 1.5);
        }

        [TestMethod]
        public void DoubleAdditionInvalidTest()
        {
            // Arrange
            var a = GetDouble(5);
            var b = GetString("foo");

            // Act
            var result = a.ArithmeticAdd(b);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DoubleSubtractionWholeTest()
        {
            // Arrange
            var a = GetDouble(5);
            var b = GetDouble(10);

            // Act
            var result = a.ArithmeticSubtract(b);

            // Assert
            AssertArithmeticResult<ScriptInteger>(result, a, b, -5);
        }

        [TestMethod]
        public void DoubleSubtractionFractionTest()
        {
            // Arrange
            var a = GetDouble(.5);
            var b = GetDouble(.1);

            // Act
            var result = a.ArithmeticSubtract(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(result, a, b, .4);
        }

        [TestMethod]
        public void DoubleSubtractionIntegerTest()
        {
            // Arrange
            var a = GetDouble(5);
            var b = GetInteger(1);

            // Act
            var result = a.ArithmeticSubtract(b);

            // Assert
            AssertArithmeticResult<ScriptInteger>(result, a, b, 4);
        }

        [TestMethod]
        public void DoubleSubtractionInvalidTest()
        {
            // Arrange
            var a = GetDouble(5);
            var b = GetString("foo");

            // Act
            var result = a.ArithmeticSubtract(b);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DoubleMultiplicationTest()
        {
            // Arrange
            var a = GetDouble(5);
            var b = GetDouble(10);

            // Act
            var result = a.ArithmeticMultiply(b);

            // Assert
            AssertArithmeticResult<ScriptInteger>(result, a, b, 50);
        }

        [TestMethod]
        public void DoubleMultiplicationIntegerTest()
        {
            // Arrange
            var a = GetDouble(5);
            var b = GetInteger(10);

            // Act
            var result = a.ArithmeticMultiply(b);

            // Assert
            AssertArithmeticResult<ScriptInteger>(result, a, b, 50);
        }

        [TestMethod]
        public void DoubleMultiplicationInvalidTest()
        {
            // Arrange
            var a = GetDouble(5);
            var b = GetString("foo");

            // Act
            var result = a.ArithmeticMultiply(b);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DoubleDivideWholeTest()
        {
            // Arrange
            var a = GetDouble(50);
            var b = GetDouble(10);

            // Act
            var result = a.ArithmeticDivide(b);

            // Assert
            AssertArithmeticResult<ScriptInteger>(result, a, b, 5);
        }

        [TestMethod]
        public void DoubleDivideFractionTest()
        {
            // Arrange
            var a = GetDouble(5);
            var b = GetDouble(10);

            // Act
            var result = a.ArithmeticDivide(b);

            // Assert
            AssertArithmeticResult<ScriptDouble>(result, a, b, .5);
        }
        
        [TestMethod]
        public void DoubleDivideIntegerTest()
        {
            // Arrange
            var a = GetDouble(50);
            var b = GetInteger(10);

            // Act
            var result = a.ArithmeticDivide(b);

            // Assert
            AssertArithmeticResult<ScriptInteger>(result, a, b, 5);
        }

        [TestMethod]
        public void DoubleDivideInvalidTest()
        {
            // Arrange
            var a = GetDouble(5);
            var b = GetString("foo");
            
            // Act
            var result = a.ArithmeticDivide(b);

            // Act + Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DoubleDivideByZero()
        {
            // Arrange
            var a = GetDouble(5);
            var b = GetDouble(0);
            object[] expectedFormatArgs = { };
            void Action() { a.ArithmeticDivide(b); }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Math_DivideByZero), expectedFormatArgs);
        }

        [DataTestMethod]
        [DataRow(double.PositiveInfinity, nameof(Localized_Base_Entities.Type_Double_PosInfinity))]
        [DataRow(double.NegativeInfinity, nameof(Localized_Base_Entities.Type_Double_NegInfinity))]
        [DataRow(double.NaN, nameof(Localized_Base_Entities.Type_Double_NaN))]
        public void DoubleToStringSpecial(double value, string expectedKey)
        {
            // Arrange
            var scriptType = GetDouble(value);
            string expected = Localized_Base_Entities.ResourceManager.GetString(expectedKey);

            // Act
            string result = scriptType.ToString();

            // Assert
            Assert.AreEqual(expected, result);
        }

        protected override IScriptType GetBasicOperand()
        {
            return GetDouble(5d);
        }
    }
}