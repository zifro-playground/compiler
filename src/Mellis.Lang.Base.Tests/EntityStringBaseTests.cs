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
            var result = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<ScriptString>(result, a, b, "foobar");
        }

        [TestMethod]
        public void StringAdditionEmptyTest()
        {
            // Arrange
            var a = GetString("");
            var b = GetString("");

            // Act
            var result = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<ScriptString>(result, a, b, "");
        }

        [TestMethod]
        public void StringAdditionInvalidTest()
        {
            // Arrange
            var a = GetString("foo");
            var b = GetInteger(5);

            // Act
            var result = a.ArithmeticAdd(b);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void StringSubtractionTest()
        {
            // Arrange
            var a = GetString("foo");
            var b = GetString("bar");

            // Act
            var result = a.ArithmeticSubtract(b);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void StringMultiplicationTest()
        {
            // Arrange
            var a = GetString("foo");
            var b = GetString("bar");

            // Act
            var result = a.ArithmeticMultiply(b);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void StringDivideWholeTest()
        {
            // Arrange
            var a = GetString("foo");
            var b = GetString("bar");

            // Act
            var result = a.ArithmeticDivide(b);
            
            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void StringDivideByZeroTest()
        {
            // Arrange
            var a = GetString("foo");
            var b = GetInteger(0);

            // Act
            var result = a.ArithmeticDivide(b);

            // Assert
            Assert.IsNull(result);
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
            AssertArithmeticResult<ScriptString>(result, a, b, "f");
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

        public override void IndexGetFails()
        {
            // disabled
        }

        protected override IScriptType GetBasicOperand()
        {
            return GetString("foo");
        }
    }
}