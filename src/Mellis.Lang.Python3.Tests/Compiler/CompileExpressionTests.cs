using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mellis.Core.Entities;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Literals;
using Mellis.Lang.Python3.Syntax.Operators;
using Mellis.Lang.Python3.Syntax.Operators.Arithmetics;
using Mellis.Lang.Python3.Syntax.Operators.Binaries;
using Mellis.Lang.Python3.Syntax.Operators.Comparisons;
using Mellis.Lang.Python3.Syntax.Operators.Logicals;
using Mellis.Lang.Python3.Syntax.Statements;
using Mellis.Lang.Python3.Tests.TestingOps;

namespace Mellis.Lang.Python3.Tests.Compiler
{
    [TestClass]
    public class CompileExpressionTests
    {
        [DataTestMethod]
        [DataRow(typeof(ArithmeticAdd), BasicOperatorCode.AAdd, DisplayName = "comp op +")]
        [DataRow(typeof(ArithmeticSubtract), BasicOperatorCode.ASub, DisplayName = "comp op -")]
        [DataRow(typeof(ArithmeticMultiply), BasicOperatorCode.AMul, DisplayName = "comp op *")]
        [DataRow(typeof(ArithmeticDivide), BasicOperatorCode.ADiv, DisplayName = "comp op /")]
        [DataRow(typeof(ArithmeticFloor), BasicOperatorCode.AFlr, DisplayName = "comp op //")]
        [DataRow(typeof(ArithmeticModulus), BasicOperatorCode.AMod, DisplayName = "comp op %")]
        [DataRow(typeof(ArithmeticPower), BasicOperatorCode.APow, DisplayName = "comp op **")]
        [DataRow(typeof(BinaryAnd), BasicOperatorCode.BAnd, DisplayName = "comp op a&b")]
        [DataRow(typeof(BinaryLeftShift), BasicOperatorCode.BLsh, DisplayName = "comp op a<<b")]
        [DataRow(typeof(BinaryRightShift), BasicOperatorCode.BRsh, DisplayName = "comp op a>>b")]
        [DataRow(typeof(BinaryOr), BasicOperatorCode.BOr, DisplayName = "comp op a|b")]
        [DataRow(typeof(BinaryXor), BasicOperatorCode.BXor, DisplayName = "comp op a^b")]
        [DataRow(typeof(CompareEquals), BasicOperatorCode.CEq, DisplayName = "comp op a==b")]
        [DataRow(typeof(CompareNotEquals), BasicOperatorCode.CNEq, DisplayName = "comp op a!=b")]
        [DataRow(typeof(CompareGreaterThan), BasicOperatorCode.CGt, DisplayName = "comp op a>b")]
        [DataRow(typeof(CompareGreaterThanOrEqual), BasicOperatorCode.CGtEq, DisplayName = "comp op a>=b")]
        [DataRow(typeof(CompareLessThan), BasicOperatorCode.CLt, DisplayName = "comp op a<b")]
        [DataRow(typeof(CompareLessThanOrEqual), BasicOperatorCode.CLtEq, DisplayName = "comp op a<=b")]
        [DataRow(typeof(CompareIn), BasicOperatorCode.CIn, DisplayName = "comp op a in b")]
        [DataRow(typeof(CompareInNot), BasicOperatorCode.CNIn, DisplayName = "comp op a not in b")]
        [DataRow(typeof(CompareIs), BasicOperatorCode.CIs, DisplayName = "comp op a is b")]
        [DataRow(typeof(CompareIsNot), BasicOperatorCode.CIsN, DisplayName = "comp op a is not b")]
        public void CompileBasicBinaryTests(Type operatorType, BasicOperatorCode expectedCode)
        {
            // Arrange
            var compiler = new PyCompiler();
            compiler.CreateAndSetup(
                out Mock<ExpressionNode> exprLhsMock,
                out NopOp exprLhsOp);

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> exprRhsMock,
                out NopOp exprRhsOp);

            var opNode = (BinaryOperator)Activator.CreateInstance(operatorType,
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
        [DataRow(typeof(ArithmeticNegative), BasicOperatorCode.ANeg, DisplayName = "comp op +b")]
        [DataRow(typeof(ArithmeticPositive), BasicOperatorCode.APos, DisplayName = "comp op -b")]
        [DataRow(typeof(BinaryNot), BasicOperatorCode.BNot, DisplayName = "comp op ~b")]
        [DataRow(typeof(LogicalNot), BasicOperatorCode.LNot, DisplayName = "comp op !b")]
        public void CompileBasicUnaryTests(Type operatorType, BasicOperatorCode expectedCode)
        {
            // Arrange
            var compiler = new PyCompiler();

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> exprMock,
                out NopOp exprOp);

            var opNode = (UnaryOperator)Activator.CreateInstance(operatorType,
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
        public void CompileShortCircuitAnd()
        {
            CompileShortCircuit(typeof(LogicalAnd), jumpOverRhsIfTrue: false);
        }

        [TestMethod]
        public void CompileShortCircuitOr()
        {
            CompileShortCircuit(typeof(LogicalOr), jumpOverRhsIfTrue: true);
        }

        private static void CompileShortCircuit(Type operatorType, bool jumpOverRhsIfTrue)
        {
            // Arrange
            var compiler = new PyCompiler();
            compiler.CreateAndSetup(
                out Mock<ExpressionNode> exprLhsMock,
                out NopOp exprLhsOp);

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> exprRhsMock,
                out NopOp exprRhsOp);

            var opNode = (BinaryOperator)Activator.CreateInstance(operatorType,
                exprLhsMock.Object, exprRhsMock.Object);

            // Act
            opNode.Compile(compiler);

            // Assert
            Assert.That.IsExpectedOpCode(compiler, 0, exprLhsOp);
            Assert.That.IsOpCode<VarPop>(compiler, 2);
            Assert.That.IsExpectedOpCode(compiler, 3, exprRhsOp);

            Assert.AreEqual(4, compiler.Count);

            if (jumpOverRhsIfTrue)
            {
                var jump = Assert.That.IsOpCode<JumpIfTrue>(compiler, 1);
                Assert.IsTrue(jump.Peek);
                Assert.AreEqual(4, jump.Target);
            }
            else
            {
                var jump = Assert.That.IsOpCode<JumpIfFalse>(compiler, 1);
                Assert.IsTrue(jump.Peek);
                Assert.AreEqual(4, jump.Target);
            }

            exprLhsMock.Verify(o => o.Compile(compiler), Times.Once);
            exprRhsMock.Verify(o => o.Compile(compiler), Times.Once);
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

        [TestMethod]
        public void ComparisonFactoryTest()
        {
            // Arrange
            var factory = new ComparisonFactory(ComparisonType.Equals);

            void Action()
            {
                factory.Compile(null);
            }

            // Act
            var ex = Assert.ThrowsException<SyntaxUncompilableException>((Action)Action);

            // Assert
            Assert.That.ErrorSyntaxFormatArgsEqual(ex,
                nameof(Localized_Python3_Syntax.Ex_SyntaxNode_Uncompilable),
                SourceReference.ClrSource,
                nameof(ComparisonFactory));

            Assert.AreEqual(typeof(ComparisonFactory), ex.UncompilableType);
        }

        [TestMethod]
        public void ArgumentListTest()
        {
            // Arrange
            var argumentsList = new ArgumentsList(SourceReference.ClrSource, new ExpressionNode[0]);

            void Action()
            {
                argumentsList.Compile(null);
            }

            // Act
            var ex = Assert.ThrowsException<SyntaxUncompilableException>((Action)Action);

            // Assert
            Assert.That.ErrorSyntaxFormatArgsEqual(ex,
                nameof(Localized_Python3_Syntax.Ex_SyntaxNode_Uncompilable),
                SourceReference.ClrSource,
                nameof(ArgumentsList));

            Assert.AreEqual(typeof(ArgumentsList), ex.UncompilableType);
        }

        [TestMethod]
        public void FunctionCallEmptyArgsTest()
        {
            // Arrange
            var compiler = new PyCompiler();

            var args = new ExpressionNode[0];

            var callNode = new FunctionCall(
                SourceReference.ClrSource,
                new Identifier(SourceReference.ClrSource, "foo"),
                new ArgumentsList(SourceReference.ClrSource, args)
            );

            // Act
            callNode.Compile(compiler);

            // Assert
            var foo = Assert.That.IsOpCode<VarGet>(compiler, 0);
            Assert.AreEqual("foo", foo.Identifier);

            var callOp = Assert.That.IsOpCode<Call>(compiler, 1);
            Assert.AreEqual(0, callOp.ArgumentCount);
            Assert.AreEqual(2, callOp.ReturnAddress);

            Assert.AreEqual(2, compiler.Count);
        }

        [TestMethod]
        public void FunctionCallSingleArgTest()
        {
            // Arrange
            var compiler = new PyCompiler();

            const int expectedLiteral = 5;
            const string expectedIdentifier = "foo";

            var args = new[] {
                new LiteralInteger(SourceReference.ClrSource, expectedLiteral),
            };

            var callNode = new FunctionCall(
                SourceReference.ClrSource,
                new Identifier(SourceReference.ClrSource, expectedIdentifier),
                new ArgumentsList(SourceReference.ClrSource, args)
            );

            // Act
            callNode.Compile(compiler);

            // Assert
            var foo = Assert.That.IsOpCode<VarGet>(compiler, 0);
            Assert.AreEqual(expectedIdentifier, foo.Identifier);

            Assert.That.IsPushLiteralOpCode(expectedLiteral, compiler, 1);

            var callOp = Assert.That.IsOpCode<Call>(compiler, 2);
            Assert.AreEqual(1, callOp.ArgumentCount);
            Assert.AreEqual(3, callOp.ReturnAddress);

            Assert.AreEqual(3, compiler.Count);
        }

        [TestMethod]
        public void FunctionCallMultipleArgsTest()
        {
            // Arrange
            var compiler = new PyCompiler();

            const int expectedLiteral1 = 5;
            const string expectedLiteral2 = "bar";
            const bool expectedLiteral3 = true;

            const string expectedIdentifier = "foo";

            var args = new ExpressionNode[] {
                new LiteralInteger(SourceReference.ClrSource, expectedLiteral1),
                new LiteralString(SourceReference.ClrSource, expectedLiteral2),
                new LiteralBoolean(SourceReference.ClrSource, expectedLiteral3),
            };

            var callNode = new FunctionCall(
                SourceReference.ClrSource,
                new Identifier(SourceReference.ClrSource, expectedIdentifier),
                new ArgumentsList(SourceReference.ClrSource, args)
            );

            // Act
            callNode.Compile(compiler);

            // Assert
            var foo = Assert.That.IsOpCode<VarGet>(compiler, 0);
            Assert.AreEqual(expectedIdentifier, foo.Identifier);

            Assert.That.IsPushLiteralOpCode(expectedLiteral1, compiler, 1);
            Assert.That.IsPushLiteralOpCode(expectedLiteral2, compiler, 2);
            Assert.That.IsPushLiteralOpCode(expectedLiteral3, compiler, 3);

            var callOp = Assert.That.IsOpCode<Call>(compiler, 4);
            Assert.AreEqual(3, callOp.ArgumentCount);
            Assert.AreEqual(5, callOp.ReturnAddress);

            Assert.AreEqual(5, compiler.Count);
        }

        [TestMethod]
        public void ExpressionStatementTest()
        {
            // Arrange
            var compiler = new PyCompiler();

            var nopOp = new NopOp();
            var exprMock = new Mock<ExpressionNode>(SourceReference.ClrSource);

            exprMock.Setup(o => o.Compile(compiler))
                .Callback(() => compiler.Push(nopOp))
                .Verifiable();

            var exprStmt = new ExpressionStatement(exprMock.Object);

            // Act
            exprStmt.Compile(compiler);

            // Assert
            Assert.That.IsExpectedOpCode(compiler, 0, nopOp);
            Assert.That.IsOpCode<VarPop>(compiler, 1);
            Assert.AreEqual(2, compiler.Count);
            exprMock.Verify();
        }
    }
}