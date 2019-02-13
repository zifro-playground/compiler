using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Exceptions;
using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class OpCodeEvaluateTests
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

        [DataTestMethod]
        // Binary operators (lhs op rhs)
        [DataRow(OperatorCode.BAnd, "&", DisplayName = "nyi &")]
        [DataRow(OperatorCode.BLsh, "<<", DisplayName = "nyi <<")]
        [DataRow(OperatorCode.BRsh, ">>", DisplayName = "nyi >>")]
        [DataRow(OperatorCode.BOr, "|", DisplayName = "nyi |")]
        [DataRow(OperatorCode.BXor, "^", DisplayName = "nyi ^")]

        [DataRow(OperatorCode.CEq, "==", DisplayName = "nyi ==")]
        [DataRow(OperatorCode.CNEq, "!=", DisplayName = "nyi !=")]
        [DataRow(OperatorCode.CGt, ">", DisplayName = "nyi >")]
        [DataRow(OperatorCode.CGtEq, ">=", DisplayName = "nyi >=")]
        [DataRow(OperatorCode.CLt, "<", DisplayName = "nyi <")]
        [DataRow(OperatorCode.CLtEq, "<=", DisplayName = "nyi <=")]

        [DataRow(OperatorCode.LAnd, "&&", DisplayName = "nyi &&")]
        [DataRow(OperatorCode.LOr, "||", DisplayName = "nyi ||")]

        // Unary operators (op rhs)
        [DataRow(OperatorCode.ANeg, "+", DisplayName = "nyi +")]
        [DataRow(OperatorCode.APos, "-", DisplayName = "nyi -")]
        [DataRow(OperatorCode.BNot, "~", DisplayName = "nyi ~")]
        [DataRow(OperatorCode.LNot, "!", DisplayName = "nyi !")]
        public void EvaluateBinary_NotYetImplemented_Tests(OperatorCode opCode, string expectedKeyword)
        {
            // Arrange
            var source = new SourceReference(1,2,3,4);
            var processor = new PyProcessor(
                new OpBinOpCode(source, opCode)
            );

            var lhsMock = new Mock<IScriptType>(MockBehavior.Strict);
            var rhsMock = new Mock<IScriptType>(MockBehavior.Strict);

            processor.PushValue(lhsMock.Object);
            processor.PushValue(rhsMock.Object);

            // Act
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>((Action) processor.WalkLine);

            // Assert
            Assert.IsNotNull(processor.LastError);
            Assert.AreEqual(ProcessState.Error, processor.State);

            Assert.That.ErrorNotYetImplFormatArgs(ex, source, expectedKeyword);
        }

        protected void EvaluateBinaryTestTemplate(OperatorCode opCode, Expression<Func<IScriptType, IScriptType>> method)
        {
            // Arrange
            var processor = new PyProcessor(
                new OpBinOpCode(SourceReference.ClrSource, opCode)
            );

            var lhsMock = new Mock<IScriptType>(MockBehavior.Strict);
            var rhsMock = new Mock<IScriptType>(MockBehavior.Strict);
            var resultMock = new Mock<IScriptType>();

            lhsMock.Setup(method).Returns(resultMock.Object);

            processor.PushValue(lhsMock.Object);
            processor.PushValue(rhsMock.Object);

            // Act
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;
            var result = processor.PopValue<IScriptType>();

            // Assert
            Assert.IsNull(processor.LastError, "Last error <{0}>:{1}", processor.LastError?.GetType().Name, processor.LastError?.Message);

            Assert.AreEqual(1, numOfValues, "Did not absorb values.");
            Assert.AreSame(resultMock.Object, result, "Did not produce result.");
            lhsMock.Verify(method);
        }
    }
}