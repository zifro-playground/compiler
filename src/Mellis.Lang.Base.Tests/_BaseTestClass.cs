using System;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Base.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Base.Tests
{
    public abstract class BaseTestClass
    {
        protected Mock<IScriptTypeFactory> factoryMock;
        protected Mock<IProcessor> processorMock;

        [TestInitialize]
        public void TestInitialize()
        {
            processorMock = new Mock<IProcessor>();
            factoryMock = new Mock<IScriptTypeFactory>();

            processorMock.SetupGet(o => o.Factory).Returns(factoryMock.Object);
            factoryMock.Setup(o => o.Create(It.IsAny<int>()))
                .Returns<int>(GetScriptInteger);
            factoryMock.Setup(o => o.Create(It.IsAny<double>()))
                .Returns<double>(GetScriptDouble);
            factoryMock.Setup(o => o.Create(It.IsAny<string>()))
                .Returns<string>(GetScriptString);
            factoryMock.Setup(o => o.Create(It.IsAny<char>()))
                .Returns<char>(c => GetScriptString(c.ToString()));
            factoryMock.Setup(o => o.Create(It.IsAny<bool>()))
                .Returns<bool>(b => b ? factoryMock.Object.True : factoryMock.Object.False);

            factoryMock.SetupGet(o => o.True)
                .Returns(GetScriptBoolean(true));
            factoryMock.SetupGet(o => o.False)
                .Returns(GetScriptBoolean(false));
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void ArithmeticAdd_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.ArithmeticAdd(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void ArithmeticAddReverse_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.ArithmeticAddReverse(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void ArithmeticSubtract_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.ArithmeticSubtract(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void ArithmeticSubtractReverse_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.ArithmeticSubtractReverse(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void ArithmeticMultiply_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.ArithmeticMultiply(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void ArithmeticMultiplyReverse_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.ArithmeticMultiplyReverse(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void ArithmeticExponent_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.ArithmeticExponent(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void ArithmeticExponentReverse_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.ArithmeticExponentReverse(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void ArithmeticDivide_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.ArithmeticDivide(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void ArithmeticDivideReverse_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.ArithmeticDivideReverse(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void ArithmeticFloorDivide_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.ArithmeticFloorDivide(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void ArithmeticFloorDivideReverse_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.ArithmeticFloorDivideReverse(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void ArithmeticModulus_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.ArithmeticModulus(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void ArithmeticModulusReverse_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.ArithmeticModulusReverse(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void ArithmeticUnaryNegative_NotImplemented()
        {
            // Arrange
            var a = GetBasicOperand();

            // Act
            var result = a.ArithmeticUnaryNegative();

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void ArithmeticUnaryPositive_NotImplemented()
        {
            // Arrange
            var a = GetBasicOperand();

            // Act
            var result = a.ArithmeticUnaryPositive();

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void BinaryAnd_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.BinaryAnd(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void BinaryAndReverse_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.BinaryAndReverse(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void BinaryOr_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.BinaryOr(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void BinaryOrReverse_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.BinaryOrReverse(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void BinaryXor_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.BinaryXor(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void BinaryXorReverse_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.BinaryXorReverse(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void BinaryLeftShift_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.BinaryLeftShift(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void BinaryLeftShiftReverse_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.BinaryLeftShiftReverse(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void BinaryRightShift_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.BinaryRightShift(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void BinaryRightShiftReverse_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.BinaryRightShiftReverse(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void BinaryNot_NotImplemented()
        {
            // Arrange
            var a = GetBasicOperand();

            // Act
            var result = a.BinaryNot();

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void CompareEqual_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.CompareEqual(b);

            // Assert
            // Should default to not equal
            AssertAreEqual(false, result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void CompareNotEqual_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.CompareNotEqual(b);

            // Assert
            // Should default to not equal
            AssertAreEqual(true, result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void CompareGreaterThan_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.CompareGreaterThan(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void CompareGreaterThanOrEqual_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.CompareGreaterThanOrEqual(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void CompareLessThan_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.CompareLessThan(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void CompareLessThanOrEqual_InvalidType()
        {
            // Arrange
            var a = GetBasicOperand();
            var b = GetBasicOtherOperandInvalidType();

            // Act
            var result = a.CompareLessThanOrEqual(b);

            // Assert
            Assert.IsNull(result, "Unexpected result: {0}", result);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void IndexGet_NotImplemented()
        {
            // Arrange
            var a = GetBasicOperand();
            object[] expectedFormatArgs = {a.GetTypeName()};

            void Action()
            {
                a.GetIndex(null);
            }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Base_IndexGet), expectedFormatArgs);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void IndexSet_NotImplemented()
        {
            // Arrange
            var a = GetBasicOperand();
            object[] expectedFormatArgs = {a.GetTypeName()};

            void Action()
            {
                a.SetIndex(null, null);
            }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Base_IndexSet), expectedFormatArgs);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void PropertyGet_NotImplemented()
        {
            // Arrange
            var a = GetBasicOperand();
            const string property = "prop";
            object[] expectedFormatArgs = {a.GetTypeName(), property};

            void Action()
            {
                a.GetProperty(property);
            }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Base_PropertyGet), expectedFormatArgs);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void PropertySet_NotImplemented()
        {
            // Arrange
            var a = GetBasicOperand();
            const string property = "prop";
            object[] expectedFormatArgs = { a.GetTypeName(), property };

            void Action()
            {
                a.SetProperty(property, null);
            }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Base_PropertySet), expectedFormatArgs);
        }

        [TestMethod, TestCategory("BaseTestClass")]
        public virtual void MemberIn_NotImplemented()
        {
            // Arrange
            var a = GetBasicOperand();
            object[] expectedFormatArgs = { a.GetTypeName() };

            void Action()
            {
                a.MemberIn(null);
            }

            // Act + Assert
            AssertThrow(Action, nameof(Localized_Base_Entities.Ex_Base_MemberIn), expectedFormatArgs);
        }

        protected abstract IScriptType GetBasicOperand();
        protected abstract IScriptType GetBasicOtherOperandInvalidType();

        protected ScriptInteger GetScriptInteger(int value)
        {
            return GetValue<ScriptInteger, int>(value);
        }

        protected ScriptDouble GetScriptDouble(double value)
        {
            return GetValue<ScriptDouble, double>(value);
        }

        protected ScriptString GetScriptString(string value)
        {
            return GetValue<ScriptString, string>(value);
        }

        protected ScriptBoolean GetScriptBoolean(bool value)
        {
            return GetValue<ScriptBoolean, bool>(value);
        }

        protected ScriptNull GetScriptNull()
        {
            return new Mock<ScriptNull>(processorMock.Object) {CallBase = true}.Object;
        }

        protected IScriptType GetScriptValue(object value)
        {
            switch (value)
            {
            case int i:
                return GetScriptInteger(i);
            case double d:
                return GetScriptDouble(d);
            case string s:
                return GetScriptString(s);
            case bool b:
                return GetScriptBoolean(b);
            default:
                throw new NotSupportedException();
            }
        }

        protected void AssertArithmeticResult<T>(
            IScriptType resultBase,
            IScriptType lhs,
            IScriptType rhs,
            object expected)
            where T : IScriptType
        {
            Assert.IsNotNull(resultBase);
            Assert.IsInstanceOfType(resultBase, typeof(T));
            var result = (T)resultBase;
            Assert.IsNotNull(result);
            Assert.AreNotSame(lhs, result);
            Assert.AreNotSame(rhs, result);

            AssertAreEqual(expected, result);

            processorMock.VerifyGet(o => o.Factory, Times.Once);
            processorMock.VerifyNoOtherCalls();

            switch (expected)
            {
            case int i:
                factoryMock.Verify(o => o.Create(i),
                    Times.Once);
                break;

            case double d:
                factoryMock.Verify(o => o.Create(It.Is<double>(v => Math.Abs(v - d) < 1e-10)),
                    Times.Once);
                break;

            case string s:
                factoryMock.Verify(o => o.Create(It.Is<string>(v => v == s)),
                    Times.Exactly(s.Length == 1 ? 0 : 1));
                factoryMock.Verify(o => o.Create(It.Is<char>(v => v.ToString() == s)),
                    Times.Exactly(s.Length == 1 ? 1 : 0));
                break;

            case bool b when b:
                factoryMock.VerifyGet(o => o.True, Times.Once);
                break;

            case bool b when !b:
                factoryMock.VerifyGet(o => o.False, Times.Once);
                break;
            }

            factoryMock.VerifyNoOtherCalls();
        }

        protected void AssertThrow(Action action, string localizationKey, object[] expectedFormatArgs)
        {
            var ex = Assert.ThrowsException<RuntimeException>(action);
            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.LocalizeKey, localizationKey);
            CollectionAssert.AreEqual(ex.FormatArgs, expectedFormatArgs);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        protected void AssertAreEqual(object expected, IScriptType actual)
        {
            switch (actual)
            {
            case ScriptDouble d when expected is double e:
                Assert.AreEqual(e, d.Value, 1e-10);
                break;
            case ScriptInteger i:
                Assert.AreEqual(expected, i.Value);
                break;
            case ScriptString s:
                Assert.AreEqual(expected, s.Value);
                break;
            case ScriptBoolean s:
                Assert.AreEqual(expected, s.Value);
                break;
            default:
                Assert.Fail("Did not match. Expected: ({0}) {1}. Actual: ({2}) {3}",
                    expected?.GetType().Name ?? "null",
                    expected,
                    actual?.GetType().Name ?? "null",
                    actual);
                break;
            }
        }

        private TScriptType GetValue<TScriptType, TValue>(TValue value)
            where TScriptType : class, IScriptType
        {
            return new Mock<TScriptType>(processorMock.Object, value) {CallBase = true}.Object;
        }
    }
}