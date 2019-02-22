using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Base.Resources;
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
        [DataRow(typeof(CompareNotEquals), OperatorCode.CNEq, DisplayName = "comp op a!=b")]
        [DataRow(typeof(CompareGreaterThan), OperatorCode.CGt, DisplayName = "comp op a>b")]
        [DataRow(typeof(CompareGreaterThanOrEqual), OperatorCode.CGtEq, DisplayName = "comp op a>=b")]
        [DataRow(typeof(CompareLessThan), OperatorCode.CLt, DisplayName = "comp op a<b")]
        [DataRow(typeof(CompareLessThanOrEqual), OperatorCode.CLtEq, DisplayName = "comp op a<=b")]
        [DataRow(typeof(CompareIn), OperatorCode.CIn, DisplayName = "comp op a in b")]
        [DataRow(typeof(CompareInNot), OperatorCode.CNIn, DisplayName = "comp op a not in b")]
        [DataRow(typeof(CompareIs), OperatorCode.CIs, DisplayName = "comp op a is b")]
        [DataRow(typeof(CompareIsNot), OperatorCode.CIsN, DisplayName = "comp op a is not b")]
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
            var ex = Assert.ThrowsException<SyntaxUncompilableException>((Action) Action);

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

            Assert.That.IsOpCode<CallStackPop>(compiler, 2);
        }

        [TestMethod]
        public void FunctionCallSingleArgTest()
        {
            // Arrange
            var compiler = new PyCompiler();

            const int expectedLiteral = 5;
            const string expectedIdentifier = "foo";

            var args = new []
            {
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

            var numb = Assert.That.IsOpCode<PushLiteral<int>>(compiler, 1);
            Assert.AreEqual(expectedLiteral, numb.Literal.Value);

            var callOp = Assert.That.IsOpCode<Call>(compiler, 2);
            Assert.AreEqual(1, callOp.ArgumentCount);
            Assert.AreEqual(3, callOp.ReturnAddress);

            Assert.That.IsOpCode<CallStackPop>(compiler, 3);
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

            var args = new ExpressionNode[]
            {
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

            var lit1 = Assert.That.IsOpCode<PushLiteral<int>>(compiler, 1);
            Assert.AreEqual(expectedLiteral1, lit1.Literal.Value);

            var lit2 = Assert.That.IsOpCode<PushLiteral<string>>(compiler, 2);
            Assert.AreEqual(expectedLiteral2, lit2.Literal.Value);

            var lit3 = Assert.That.IsOpCode<PushLiteral<bool>>(compiler, 3);
            Assert.AreEqual(expectedLiteral3, lit3.Literal.Value);

            var callOp = Assert.That.IsOpCode<Call>(compiler, 4);
            Assert.AreEqual(3, callOp.ArgumentCount);
            Assert.AreEqual(5, callOp.ReturnAddress);

            Assert.That.IsOpCode<CallStackPop>(compiler, 5);
        }
    }
}