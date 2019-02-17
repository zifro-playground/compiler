using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Operators;
using Mellis.Lang.Python3.Syntax.Operators.Arithmetics;
using Mellis.Lang.Python3.Syntax.Operators.Binaries;
using Mellis.Lang.Python3.Syntax.Operators.Comparisons;
using Mellis.Lang.Python3.Syntax.Operators.Logicals;
using Mellis.Lang.Python3.Tests.TestingOps;

namespace Mellis.Lang.Python3.Tests.Compiler
{
    [TestClass]
    public class CompileExpressionTests
    {
        [DataTestMethod]
        [DataRow(typeof(ArithmeticAdd), OperatorCode.AAdd, DisplayName = "comp op +")]
        [DataRow(typeof(ArithmeticSubtract), OperatorCode.ASub, DisplayName = "comp op -")]
        [DataRow(typeof(ArithmeticMultiply), OperatorCode.AMul, DisplayName = "comp op *")]
        [DataRow(typeof(ArithmeticDivide), OperatorCode.ADiv, DisplayName = "comp op /")]
        [DataRow(typeof(ArithmeticFloor), OperatorCode.AFlr, DisplayName = "comp op //")]
        [DataRow(typeof(ArithmeticModulus), OperatorCode.AMod, DisplayName = "comp op %")]
        [DataRow(typeof(ArithmeticPower), OperatorCode.APow, DisplayName = "comp op **")]
        [DataRow(typeof(BinaryAnd), OperatorCode.BAnd, DisplayName = "comp op a&b")]
        [DataRow(typeof(BinaryLeftShift), OperatorCode.BLsh, DisplayName = "comp op a<<b")]
        [DataRow(typeof(BinaryRightShift), OperatorCode.BRsh, DisplayName = "comp op a>>b")]
        [DataRow(typeof(BinaryOr), OperatorCode.BOr, DisplayName = "comp op a|b")]
        [DataRow(typeof(BinaryXor), OperatorCode.BXor, DisplayName = "comp op a^b")]
        [DataRow(typeof(CompareEquals), OperatorCode.CEq, DisplayName = "comp op a==b")]
        //[DataRow(typeof(CNEq), OperatorCode.CNEq, DisplayName = "comp op a!=b")]
        //[DataRow(typeof(CGt), OperatorCode.CGt, DisplayName = "comp op a>b")]
        //[DataRow(typeof(CGtEq), OperatorCode.CGtEq, DisplayName = "comp op a>=b")]
        //[DataRow(typeof(CLt), OperatorCode.CLt, DisplayName = "comp op a<b")]
        //[DataRow(typeof(CLtEq), OperatorCode.CLtEq, DisplayName = "comp op a<=b")]
        [DataRow(typeof(LogicalAnd), OperatorCode.LAnd, DisplayName = "comp op a&&b")]
        [DataRow(typeof(LogicalOr), OperatorCode.LOr, DisplayName = "comp op a||b")]
        public void CompileBinaryTests(Type operatorType, OperatorCode expectedCode)
        {
            // Arrange
            var compiler = new PyCompiler();
            compiler.CreateAndSetup(
                out Mock<ExpressionNode> exprLhsMock,
                out NopOp exprLhsOp);

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> exprRhsMock,
                out NopOp exprRhsOp);

            var opNode = (BinaryOperator) Activator.CreateInstance(operatorType,
                exprLhsMock.Object, exprRhsMock.Object);

            // Act
            opNode.Compile(compiler);

            // Assert
            Assert.That.IsBinaryOpCode(expectedCode, compiler, index: 2);
            Assert.AreEqual(3, compiler.Count);
            Assert.AreSame(exprLhsOp, compiler[0], "compiler[0] was not exprLhsOp");
            Assert.AreSame(exprRhsOp, compiler[1], "compiler[1] was not exprRhsOp");

            exprLhsMock.Verify(o => o.Compile(compiler), Times.Once);
            exprRhsMock.Verify(o => o.Compile(compiler), Times.Once);
        }

        [DataTestMethod]
        [DataRow(typeof(ArithmeticNegative), OperatorCode.ANeg, DisplayName = "comp op +b")]
        [DataRow(typeof(ArithmeticPositive), OperatorCode.APos, DisplayName = "comp op -b")]
        [DataRow(typeof(BinaryNot), OperatorCode.BNot, DisplayName = "comp op ~b")]
        [DataRow(typeof(LogicalNot), OperatorCode.LNot, DisplayName = "comp op !b")]
        public void CompileUnaryTests(Type operatorType, OperatorCode expectedCode)
        {
            // Arrange
            var compiler = new PyCompiler();

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> exprMock,
                out NopOp exprOp);

            var opNode = (UnaryOperator) Activator.CreateInstance(operatorType,
                SourceReference.ClrSource,
                exprMock.Object);

            // Act
            opNode.Compile(compiler);

            // Assert
            Assert.That.IsBinaryOpCode(expectedCode, compiler, index: 1);
            Assert.AreEqual(2, compiler.Count);
            Assert.AreSame(exprOp, compiler[0]);

            exprMock.Verify(o => o.Compile(compiler), Times.Once);
        }

        [TestMethod]
        public void IdentifierTest()
        {
            // Arrange
            var compiler = new PyCompiler();

            var identifier = new Identifier(SourceReference.ClrSource, "foo");

            // Act
            identifier.Compile(compiler);

            // Assert
            var varGet = Assert.That.IsOpCode<VarGet>(compiler, 0);
            Assert.AreEqual(1, compiler.Count);
            Assert.AreEqual("foo", varGet.Identifier);
        }
    }
}