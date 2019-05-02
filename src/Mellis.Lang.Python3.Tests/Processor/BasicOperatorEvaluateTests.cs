using System;
using System.Linq.Expressions;
using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Extensions;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class BasicOperatorEvaluateTests
    {
        [TestMethod]
        public void EvaluateBinary_Add_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.AAdd,
                o => o.ArithmeticAdd(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_Sub_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.ASub,
                o => o.ArithmeticSubtract(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_Mul_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.AMul,
                o => o.ArithmeticMultiply(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_Div_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.ADiv,
                o => o.ArithmeticDivide(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_Flr_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.AFlr,
                o => o.ArithmeticFloorDivide(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_Mod_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.AMod,
                o => o.ArithmeticModulus(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_Pow_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.APow,
                o => o.ArithmeticExponent(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_BAnd_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.BAnd,
                o => o.BinaryAnd(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_BLsh_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.BLsh,
                o => o.BinaryLeftShift(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_BRsh_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.BRsh,
                o => o.BinaryRightShift(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_BOr_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.BOr,
                o => o.BinaryOr(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_BXor_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.BXor,
                o => o.BinaryXor(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_CEq_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.CEq,
                o => o.CompareEqual(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_CNEq_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.CNEq,
                o => o.CompareNotEqual(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_CGt_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.CGt,
                o => o.CompareGreaterThan(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_CGtEq_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.CGtEq,
                o => o.CompareGreaterThanOrEqual(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_CLt_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.CLt,
                o => o.CompareLessThan(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_CLtEq_Test()
        {
            EvaluateBinaryTestTemplate(BasicOperatorCode.CLtEq,
                o => o.CompareLessThanOrEqual(It.IsAny<IScriptType>()));
        }

        [DataTestMethod]
        // Binary operators (lhs op rhs)
        [DataRow(BasicOperatorCode.CIn, "in", DisplayName = "nyi in")]
        [DataRow(BasicOperatorCode.CNIn, "not in", DisplayName = "nyi not in")]
        [DataRow(BasicOperatorCode.CIs, "is", DisplayName = "nyi is")]
        [DataRow(BasicOperatorCode.CIsN, "is not", DisplayName = "nyi is not")]
        public void EvaluateBinary_NotYetImplemented_Tests(BasicOperatorCode opCode, string expectedKeyword)
        {
            // Arrange
            var source = new SourceReference(1, 2, 3, 4);
            var processor = new PyProcessor(
                new BasicOperator(source, opCode)
            );

            var lhsMock = new Mock<IScriptType>(MockBehavior.Strict);
            var rhsMock = new Mock<IScriptType>(MockBehavior.Strict);

            processor.PushValue(lhsMock.Object);
            processor.PushValue(rhsMock.Object);

            // Act
            processor.WalkInstruction(); // to enter first op
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(delegate
            {
                processor.WalkLine();
            });

            // Assert
            Assert.IsNotNull(processor.LastError);
            Assert.AreEqual(ProcessState.Error, processor.State);

            Assert.That.ErrorNotYetImplFormatArgs(ex, source, expectedKeyword);
        }

        private static void EvaluateBinaryTestTemplate(
            BasicOperatorCode opCode,
            Expression<Func<IScriptType, IScriptType>> method)
        {
            // Arrange
            var processor = new PyProcessor(
                new BasicOperator(SourceReference.ClrSource, opCode)
            );

            var lhsMock = new Mock<IScriptType>(MockBehavior.Strict);
            var rhsMock = new Mock<IScriptType>(MockBehavior.Strict);
            var resultMock = new Mock<IScriptType>();

            lhsMock.Setup(method).Returns(resultMock.Object);

            processor.PushValue(lhsMock.Object);
            processor.PushValue(rhsMock.Object);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;
            var result = processor.PopValue();

            // Assert
            Assert.IsNull(processor.LastError, "Last error <{0}>:{1}", processor.LastError?.GetType().Name,
                processor.LastError?.Message);

            Assert.AreEqual(1, numOfValues, "Did not absorb values.");
            Assert.AreSame(resultMock.Object, result, "Did not produce result.");
            lhsMock.Verify(method);
        }

        [TestMethod]
        public void EvaluateShortCircuit_NotBasicOperator()
        {
            // Arrange

            // Act

            // Assert
        }

        [TestMethod]
        public void EvaluateUnary_ANeg_Test()
        {
            EvaluateUnaryTestTemplate(BasicOperatorCode.ANeg,
                o => o.ArithmeticUnaryNegative());
        }

        [TestMethod]
        public void EvaluateUnary_APos_Test()
        {
            EvaluateUnaryTestTemplate(BasicOperatorCode.APos,
                o => o.ArithmeticUnaryPositive());
        }

        [TestMethod]
        public void EvaluateUnary_BNot_Test()
        {
            EvaluateUnaryTestTemplate(BasicOperatorCode.BNot,
                o => o.BinaryNot());
        }

        private static void EvaluateUnaryTestTemplate(
            BasicOperatorCode opCode,
            Expression<Func<IScriptType, IScriptType>> method)
        {
            // Arrange
            var processor = new PyProcessor(
                new BasicOperator(SourceReference.ClrSource, opCode)
            );

            var valueMock = new Mock<IScriptType>(MockBehavior.Strict);
            var resultMock = new Mock<IScriptType>();

            valueMock.Setup(method).Returns(resultMock.Object);

            processor.PushValue(valueMock.Object);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;
            var result = processor.PopValue();

            // Assert
            Assert.IsNull(processor.LastError, "Last error <{0}>:{1}", processor.LastError?.GetType().Name,
                processor.LastError?.Message);

            Assert.AreEqual(1, numOfValues, "Did not absorb value.");
            Assert.AreSame(resultMock.Object, result, "Did not produce result.");
            valueMock.Verify(method);
        }

        [DataTestMethod]
        [DataRow(true, DisplayName = "Truthy gives false")]
        [DataRow(false, DisplayName = "Falsy gives true")]
        public void EvaluateUnary_LNot_Test(bool truthy)
        {
            // Arrange
            bool expectedResult = !truthy;

            var processor = new PyProcessor(
                new BasicOperator(SourceReference.ClrSource, BasicOperatorCode.LNot)
            );

            var valueMock = new Mock<IScriptType>(MockBehavior.Strict);

            valueMock.Setup(o => o.IsTruthy()).Returns(truthy);

            processor.PushValue(valueMock.Object);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;
            var result = processor.PopValue();

            // Assert
            Assert.IsNull(processor.LastError, "Last error <{0}>:{1}", processor.LastError?.GetType().Name,
                processor.LastError?.Message);

            Assert.AreEqual(1, numOfValues, "Did not absorb value.");
            Assert.That.ScriptTypeEqual(expectedResult, result);
            valueMock.Verify(o => o.IsTruthy(), Times.Once);
        }

        [DataTestMethod]
        [DataRow(BasicOperatorCode.AAdd, DisplayName = "is bin op a+b")]
        [DataRow(BasicOperatorCode.ASub, DisplayName = "is bin op a-b")]
        [DataRow(BasicOperatorCode.AMul, DisplayName = "is bin op a*b")]
        [DataRow(BasicOperatorCode.ADiv, DisplayName = "is bin op a/b")]
        [DataRow(BasicOperatorCode.AFlr, DisplayName = "is bin op a//b")]
        [DataRow(BasicOperatorCode.AMod, DisplayName = "is bin op a%b")]
        [DataRow(BasicOperatorCode.APow, DisplayName = "is bin op a**b")]
        [DataRow(BasicOperatorCode.BAnd, DisplayName = "is bin op a&b")]
        [DataRow(BasicOperatorCode.BLsh, DisplayName = "is bin op a<<b")]
        [DataRow(BasicOperatorCode.BRsh, DisplayName = "is bin op a>>b")]
        [DataRow(BasicOperatorCode.BOr, DisplayName = "is bin op a|b")]
        [DataRow(BasicOperatorCode.BXor, DisplayName = "is bin op a^b")]
        [DataRow(BasicOperatorCode.CEq, DisplayName = "is bin op a==b")]
        [DataRow(BasicOperatorCode.CNEq, DisplayName = "is bin op a!=b")]
        [DataRow(BasicOperatorCode.CGt, DisplayName = "is bin op a>b")]
        [DataRow(BasicOperatorCode.CGtEq, DisplayName = "is bin op a>=b")]
        [DataRow(BasicOperatorCode.CLt, DisplayName = "is bin op a<b")]
        [DataRow(BasicOperatorCode.CLtEq, DisplayName = "is bin op a<=b")]
        [DataRow(BasicOperatorCode.CIn, DisplayName = "is bin op a in b")]
        [DataRow(BasicOperatorCode.CNIn, DisplayName = "is bin op a not in b")]
        [DataRow(BasicOperatorCode.CIs, DisplayName = "is bin op a is b")]
        [DataRow(BasicOperatorCode.CIsN, DisplayName = "is bin op a is not b")]
        public void IsBinaryTests(BasicOperatorCode code)
        {
            // Act
            bool isBinary = code.IsBinary();
            bool isUnary = code.IsUnary();

            // Assert
            Assert.IsTrue(isBinary, $"{nameof(BasicOperatorCode)}.{code}.IsBinary() was false");
            Assert.IsFalse(isUnary, $"{nameof(BasicOperatorCode)}.{code}.IsUnary() was true");
        }

        [DataTestMethod]
        [DataRow(BasicOperatorCode.ANeg, DisplayName = "is un op +a")]
        [DataRow(BasicOperatorCode.APos, DisplayName = "is un op -a")]
        [DataRow(BasicOperatorCode.BNot, DisplayName = "is un op ~a")]
        [DataRow(BasicOperatorCode.LNot, DisplayName = "is un op !a")]
        public void IsUnaryTests(BasicOperatorCode code)
        {
            // Act
            bool isBinary = code.IsBinary();
            bool isUnary = code.IsUnary();

            // Assert
            Assert.IsFalse(isBinary, $"{nameof(BasicOperatorCode)}.{code}.IsBinary() was true");
            Assert.IsTrue(isUnary, $"{nameof(BasicOperatorCode)}.{code}.IsUnary() was false");
        }
    }
}