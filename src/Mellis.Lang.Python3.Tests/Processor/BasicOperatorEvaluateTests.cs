using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Extensions;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class BasicOperatorEvaluateTests
    {
        [TestMethod]
        public void EvaluateBinary_Add_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.AAdd,
                o => o.ArithmeticAdd(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_Sub_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.ASub,
                o => o.ArithmeticSubtract(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_Mul_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.AMul,
                o => o.ArithmeticMultiply(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_Div_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.ADiv,
                o => o.ArithmeticDivide(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_Flr_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.AFlr,
                o => o.ArithmeticFloorDivide(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_Mod_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.AMod,
                o => o.ArithmeticModulus(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_Pow_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.APow,
                o => o.ArithmeticExponent(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_BAnd_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.BAnd,
                o => o.BinaryAnd(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_BLsh_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.BLsh,
                o => o.BinaryLeftShift(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_BRsh_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.BRsh,
                o => o.BinaryRightShift(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_BOr_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.BOr,
                o => o.BinaryOr(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_BXor_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.BXor,
                o => o.BinaryXor(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_CEq_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.CEq,
                o => o.CompareEqual(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_CNEq_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.CNEq,
                o => o.CompareNotEqual(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_CGt_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.CGt,
                o => o.CompareGreaterThan(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_CGtEq_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.CGtEq,
                o => o.CompareGreaterThanOrEqual(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_CLt_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.CLt,
                o => o.CompareLessThan(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_CLtEq_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.CLtEq,
                o => o.CompareLessThanOrEqual(It.IsAny<IScriptType>()));
        }

        [DataTestMethod]
        // Binary operators (lhs op rhs)
        [DataRow(OperatorCode.LAnd, "and", DisplayName = "nyi and")]
        [DataRow(OperatorCode.LOr, "or", DisplayName = "nyi or")]
        [DataRow(OperatorCode.CIn, "in", DisplayName = "nyi in")]
        [DataRow(OperatorCode.CNIn, "not in", DisplayName = "nyi not in")]
        [DataRow(OperatorCode.CIs, "is", DisplayName = "nyi is")]
        [DataRow(OperatorCode.CIsN, "is not", DisplayName = "nyi is not")]
        // Unary operators (op rhs)
        [DataRow(OperatorCode.ANeg, "+", DisplayName = "nyi +")]
        [DataRow(OperatorCode.APos, "-", DisplayName = "nyi -")]
        [DataRow(OperatorCode.BNot, "~", DisplayName = "nyi ~")]
        [DataRow(OperatorCode.LNot, "not", DisplayName = "nyi not")]
        public void EvaluateBinary_NotYetImplemented_Tests(OperatorCode opCode, string expectedKeyword)
        {
            // Arrange
            var source = new SourceReference(1,2,3,4);
            var processor = new VM.PyProcessor(
                new BasicOperator(source, opCode)
            );

            var lhsMock = new Mock<IScriptType>(MockBehavior.Strict);
            var rhsMock = new Mock<IScriptType>(MockBehavior.Strict);

            processor.PushValue(lhsMock.Object);
            processor.PushValue(rhsMock.Object);

            // Act
            processor.WalkInstruction(); // to enter first op
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>((Action) processor.WalkLine);

            // Assert
            Assert.IsNotNull(processor.LastError);
            Assert.AreEqual(ProcessState.Error, processor.State);

            Assert.That.ErrorNotYetImplFormatArgs(ex, source, expectedKeyword);
        }

        protected void EvaluateBinaryTestTemplate(OperatorCode opCode, Expression<Func<IScriptType, IScriptType>> method)
        {
            // Arrange
            var processor = new VM.PyProcessor(
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
            Assert.IsNull(processor.LastError, "Last error <{0}>:{1}", processor.LastError?.GetType().Name, processor.LastError?.Message);

            Assert.AreEqual(1, numOfValues, "Did not absorb values.");
            Assert.AreSame(resultMock.Object, result, "Did not produce result.");
            lhsMock.Verify(method);
        }

        [DataTestMethod]
        [DataRow(OperatorCode.AAdd, DisplayName = "is bin op a+b")]
        [DataRow(OperatorCode.ASub, DisplayName = "is bin op a-b")]
        [DataRow(OperatorCode.AMul, DisplayName = "is bin op a*b")]
        [DataRow(OperatorCode.ADiv, DisplayName = "is bin op a/b")]
        [DataRow(OperatorCode.AFlr, DisplayName = "is bin op a//b")]
        [DataRow(OperatorCode.AMod, DisplayName = "is bin op a%b")]
        [DataRow(OperatorCode.APow, DisplayName = "is bin op a**b")]

        [DataRow(OperatorCode.BAnd, DisplayName = "is bin op a&b")]
        [DataRow(OperatorCode.BLsh, DisplayName = "is bin op a<<b")]
        [DataRow(OperatorCode.BRsh, DisplayName = "is bin op a>>b")]
        [DataRow(OperatorCode.BOr, DisplayName = "is bin op a|b")]
        [DataRow(OperatorCode.BXor, DisplayName = "is bin op a^b")]

        [DataRow(OperatorCode.CEq, DisplayName = "is bin op a==b")]
        [DataRow(OperatorCode.CNEq, DisplayName = "is bin op a!=b")]
        [DataRow(OperatorCode.CGt, DisplayName = "is bin op a>b")]
        [DataRow(OperatorCode.CGtEq, DisplayName = "is bin op a>=b")]
        [DataRow(OperatorCode.CLt, DisplayName = "is bin op a<b")]
        [DataRow(OperatorCode.CLtEq, DisplayName = "is bin op a<=b")]

        [DataRow(OperatorCode.CIn, DisplayName = "is bin op a in b")]
        [DataRow(OperatorCode.CNIn, DisplayName = "is bin op a not in b")]
        [DataRow(OperatorCode.CIs, DisplayName = "is bin op a is b")]
        [DataRow(OperatorCode.CIsN, DisplayName = "is bin op a is not b")]

        [DataRow(OperatorCode.LAnd, DisplayName = "is bin op a&&b")]
        [DataRow(OperatorCode.LOr, DisplayName = "is bin op a||b")]
        public void IsBinaryTests(OperatorCode code)
        {
            // Act
            bool isBinary = code.IsBinary();
            bool isUnary = code.IsUnary();

            // Assert
            Assert.IsTrue(isBinary, $"OperatorCode.{code}.IsBinary() was false");
            Assert.IsFalse(isUnary, $"OperatorCode.{code}.IsUnary() was true");
        }

        [DataTestMethod]
        [DataRow(OperatorCode.ANeg, DisplayName = "is un op +a")]
        [DataRow(OperatorCode.APos, DisplayName = "is un op -a")]
        [DataRow(OperatorCode.BNot, DisplayName = "is un op ~a")]
        [DataRow(OperatorCode.LNot, DisplayName = "is un op !a")]
        public void IsUnaryTests(OperatorCode code)
        {
            // Act
            bool isBinary = code.IsBinary();
            bool isUnary = code.IsUnary();

            // Assert
            Assert.IsFalse(isBinary, $"OperatorCode.{code}.IsBinary() was true");
            Assert.IsTrue(isUnary, $"OperatorCode.{code}.IsUnary() was false");
        }
    }
}