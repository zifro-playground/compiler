using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Entities;
using Zifro.Compiler.Resources;

namespace Zifro.Compiler.Tests
{
    [TestClass]
    public class EntityDoubleBaseTests : BaseTestClass
    {
        [TestMethod]
        public void DoubleAdditionWholeTest()
        {
            // Arrange
            DoubleBase a = GetDouble(5);
            DoubleBase b = GetDouble(10);

            // Act
            IScriptType result = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<IntegerBase>(result, a, b, 15);
        }

        [TestMethod]
        public void DoubleAdditionFractionTest()
        {
            // Arrange
            DoubleBase a = GetDouble(0.5);
            DoubleBase b = GetDouble(1);

            // Act
            IScriptType result = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<DoubleBase>(result, a, b, 1.5);
        }

        [TestMethod]
        public void DoubleAdditionIntegerTest()
        {
            // Arrange
            DoubleBase a = GetDouble(0.5);
            IntegerBase b = GetInteger(1);

            // Act
            IScriptType result = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<DoubleBase>(result, a, b, 1.5);
        }

        [TestMethod]
        public void DoubleAdditionInvalidTest()
        {
            // Arrange
            DoubleBase a = GetDouble(5);
            StringBase b = GetString("foo");
            object[] expectedFormatArgs = {5d, b.GetTypeName()};
            Action action = delegate { a.ArithmeticAdd(b); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Double_AddInvalidType), expectedFormatArgs);
        }

        [TestMethod]
        public void DoubleSubtractionWholeTest()
        {
            // Arrange
            DoubleBase a = GetDouble(5);
            DoubleBase b = GetDouble(10);

            // Act
            IScriptType result = a.ArithmeticSubtract(b);

            // Assert
            AssertArithmeticResult<IntegerBase>(result, a, b, -5);
        }

        [TestMethod]
        public void DoubleSubtractionFractionTest()
        {
            // Arrange
            DoubleBase a = GetDouble(.5);
            DoubleBase b = GetDouble(.1);

            // Act
            IScriptType result = a.ArithmeticSubtract(b);

            // Assert
            AssertArithmeticResult<DoubleBase>(result, a, b, .4);
        }

        [TestMethod]
        public void DoubleSubtractionIntegerTest()
        {
            // Arrange
            DoubleBase a = GetDouble(5);
            IntegerBase b = GetInteger(1);

            // Act
            IScriptType result = a.ArithmeticSubtract(b);

            // Assert
            AssertArithmeticResult<IntegerBase>(result, a, b, 4);
        }

        [TestMethod]
        public void DoubleSubtractionInvalidTest()
        {
            // Arrange
            DoubleBase a = GetDouble(5);
            StringBase b = GetString("foo");
            object[] expectedFormatArgs = {5d, b.GetTypeName()};
            Action action = delegate { a.ArithmeticSubtract(b); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Double_SubtractInvalidType), expectedFormatArgs);
        }

        [TestMethod]
        public void DoubleMultiplicationTest()
        {
            // Arrange
            DoubleBase a = GetDouble(5);
            DoubleBase b = GetDouble(10);

            // Act
            IScriptType result = a.ArithmeticMultiply(b);

            // Assert
            AssertArithmeticResult<IntegerBase>(result, a, b, 50);
        }

        [TestMethod]
        public void DoubleMultiplicationIntegerTest()
        {
            // Arrange
            DoubleBase a = GetDouble(5);
            IntegerBase b = GetInteger(10);

            // Act
            IScriptType result = a.ArithmeticMultiply(b);

            // Assert
            AssertArithmeticResult<IntegerBase>(result, a, b, 50);
        }

        [TestMethod]
        public void DoubleMultiplicationInvalidTest()
        {
            // Arrange
            DoubleBase a = GetDouble(5);
            StringBase b = GetString("foo");
            object[] expectedFormatArgs = {5d, b.GetTypeName()};
            Action action = delegate { a.ArithmeticMultiply(b); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Double_MultiplyInvalidType), expectedFormatArgs);
        }

        [TestMethod]
        public void DoubleDivideWholeTest()
        {
            // Arrange
            DoubleBase a = GetDouble(50);
            DoubleBase b = GetDouble(10);

            // Act
            IScriptType result = a.ArithmeticDivide(b);

            // Assert
            AssertArithmeticResult<IntegerBase>(result, a, b, 5);
        }

        [TestMethod]
        public void DoubleDivideFractionTest()
        {
            // Arrange
            DoubleBase a = GetDouble(5);
            DoubleBase b = GetDouble(10);

            // Act
            IScriptType result = a.ArithmeticDivide(b);

            // Assert
            AssertArithmeticResult<DoubleBase>(result, a, b, .5);
        }
        
        [TestMethod]
        public void DoubleDivideIntegerTest()
        {
            // Arrange
            DoubleBase a = GetDouble(50);
            IntegerBase b = GetInteger(10);

            // Act
            IScriptType result = a.ArithmeticDivide(b);

            // Assert
            AssertArithmeticResult<IntegerBase>(result, a, b, 5);
        }

        [TestMethod]
        public void DoubleDivideInvalidTest()
        {
            // Arrange
            DoubleBase a = GetDouble(5);
            StringBase b = GetString("foo");
            object[] expectedFormatArgs = {5d, b.GetTypeName()};
            Action action = delegate { a.ArithmeticDivide(b); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Double_DivideInvalidType), expectedFormatArgs);
        }

        [TestMethod]
        public void DoubleDivideByZero()
        {
            // Arrange
            DoubleBase a = GetDouble(5);
            DoubleBase b = GetDouble(0);
            object[] expectedFormatArgs = { };
            Action action = delegate { a.ArithmeticDivide(b); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Math_DivideByZero), expectedFormatArgs);
        }

        [TestMethod]
        public void DoubleIndexGet()
        {
            // Arrange
            DoubleBase a = GetDouble(5);
            object[] expectedFormatArgs = {5d};

            Action action = delegate { a.GetIndex(null); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Double_IndexGet), expectedFormatArgs);
        }

        [TestMethod]
        public void DoubleIndexSet()
        {
            // Arrange
            DoubleBase a = GetDouble(5);
            object[] expectedFormatArgs = {5d};

            Action action = delegate { a.SetIndex(null, null); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Double_IndexSet), expectedFormatArgs);
        }

        [TestMethod]
        public void DoublePropertyGet()
        {
            // Arrange
            DoubleBase a = GetDouble(5);
            const string property = "prop";
            object[] expectedFormatArgs = {5d, property};

            Action action = delegate { a.GetProperty(property); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Double_PropertyGet), expectedFormatArgs);
        }

        [TestMethod]
        public void DoublePropertySet()
        {
            // Arrange
            DoubleBase a = GetDouble(5);
            const string property = "prop";
            object[] expectedFormatArgs = {5d, property};

            Action action = delegate { a.SetProperty(property, null); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Double_PropertySet), expectedFormatArgs);
        }

        [TestMethod]
        public void DoubleInvoke()
        {
            // Arrange
            DoubleBase a = GetDouble(5);
            object[] expectedFormatArgs = {5d};

            Action action = delegate { a.Invoke(null); };

            // Act + Assert
            AssertThrow(action, nameof(Localized_Base_Entities.Ex_Double_Invoke), expectedFormatArgs);
        }

        [DataTestMethod]
        [DataRow(5D)]
        public void DoubleTryConvertValid(object expected)
        {
            // Arrange
            DoubleBase a = GetDouble(5);
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
        public void DoubleTryConvertDecimal()
        {
            // Arrange
            DoubleBase a = GetDouble(5);

            // Act
            bool success = a.TryConvert(typeof(decimal), out object result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(5m, result);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [DataTestMethod]
        [DataRow(typeof(float))]
        [DataRow(typeof(int))]
        [DataRow(typeof(uint))]
        [DataRow(typeof(ulong))]
        [DataRow(typeof(bool))]
        [DataRow(typeof(byte))]
        [DataRow(typeof(short))]
        [DataRow(typeof(char))]
        [DataRow(typeof(string))]
        [DataRow(typeof(DateTime))]
        public void DoubleTryConvertInvalid(Type type)
        {
            // Arrange
            DoubleBase a = GetDouble(5);

            // Act
            bool success = a.TryConvert(type, out object _);

            // Assert
            Assert.IsFalse(success);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void DoubleTryConvertGenericValid()
        {
            // Arrange
            DoubleBase a = GetDouble(5);

            // Act
            bool success = a.TryConvert(out double result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(5, result);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void DoubleTryConvertGenericInvalid()
        {
            // Arrange
            DoubleBase a = GetDouble(5);

            // Act
            bool success = a.TryConvert(out string _);

            // Assert
            Assert.IsFalse(success);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }
    }
}