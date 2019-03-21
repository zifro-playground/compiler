using System;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Literals;
using Mellis.Lang.Python3.Syntax.Statements;
using Mellis.Lang.Python3.Tests.TestingOps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Compiler.Statements
{
    [TestClass]
    public class AssignmentTests
    {
        [TestMethod]
        public void AssignIdentifierTest()
        {
            // Arrange
            const string identifier = "foo";
            var compiler = new PyCompiler();

            var idLhsMock = new Mock<Identifier>(SourceReference.ClrSource, identifier);

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> exprRhsMock,
                out NopOp exprRhsOp);

            var stmt = new Assignment(SourceReference.ClrSource,
                leftOperand: idLhsMock.Object,
                rightOperand: exprRhsMock.Object);

            // Act
            stmt.Compile(compiler);

            // Assert
            var setOpCode = Assert.That.IsOpCode<VarSet>(compiler, index: 1);
            Assert.AreEqual(identifier, setOpCode.Identifier);
            Assert.AreEqual(2, compiler.Count);
            Assert.AreSame(exprRhsOp, compiler[0], "compiler[0] was not exprRhsOp");

            idLhsMock.Verify(o => o.Compile(compiler), Times.Never);
            exprRhsMock.Verify(o => o.Compile(compiler), Times.Once);
        }

        [TestMethod]
        public void AssignLiteralIntegerTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralInteger>(source, 5);

            _AssignLiteralIntegerTest<LiteralInteger, int>(source, literalMock);
        }

        [TestMethod]
        public void AssignLiteralDoubleTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralDouble>(source, 10d);

            _AssignLiteralIntegerTest<LiteralDouble, double>(source, literalMock);
        }

        [TestMethod]
        public void AssignLiteralStringTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralString>(source, "foo");

            _AssignLiteralIntegerTest<LiteralString, string>(source, literalMock);
        }

        private static void _AssignLiteralIntegerTest<TLiteral, TValue>(
            SourceReference source,
            Mock<TLiteral> literalMock)
            where TLiteral : Literal<TValue>
        {
            // Arrange
            var compiler = new PyCompiler();

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> exprRhsMock,
                out NopOp exprRhsOp);

            var stmt = new Assignment(SourceReference.ClrSource,
                leftOperand: literalMock.Object,
                rightOperand: exprRhsMock.Object);

            void Action()
            {
                stmt.Compile(compiler);
            }

            // Act
            var ex = Assert.ThrowsException<SyntaxException>((Action)Action);

            // Assert
            Assert.That.ErrorSyntaxFormatArgsEqual(ex,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Literal),
                source,
                literalMock.Object.GetTypeName()
            );

            literalMock.Verify(o => o.Compile(compiler), Times.Never);
            exprRhsMock.Verify(o => o.Compile(compiler), Times.Never);
        }

        [DataTestMethod]
        [DataRow(true, nameof(Localized_Base_Entities.Type_Boolean_True))]
        [DataRow(false, nameof(Localized_Base_Entities.Type_Boolean_False))]
        public void AssignBooleanTest(bool value, string nameKey)
        {
            // Arrange
            var compiler = new PyCompiler();

            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralBoolean>(source, value);

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> exprRhsMock,
                out NopOp exprRhsOp);

            var stmt = new Assignment(SourceReference.ClrSource,
                leftOperand: literalMock.Object,
                rightOperand: exprRhsMock.Object);

            void Action()
            {
                stmt.Compile(compiler);
            }

            // Act
            var ex = Assert.ThrowsException<SyntaxException>((Action)Action);

            // Assert
            Assert.That.ErrorSyntaxFormatArgsEqual(ex,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Boolean),
                source,
                Localized_Base_Entities.ResourceManager.GetString(nameKey)
            );

            literalMock.Verify(o => o.Compile(compiler), Times.Never);
            exprRhsMock.Verify(o => o.Compile(compiler), Times.Never);
        }

        [TestMethod]
        public void AssignExpression()
        {
            // Arrange
            var compiler = new PyCompiler();

            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<ExpressionNode>(source);

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> exprRhsMock,
                out NopOp exprRhsOp);

            var stmt = new Assignment(SourceReference.ClrSource,
                leftOperand: literalMock.Object,
                rightOperand: exprRhsMock.Object);

            void Action()
            {
                stmt.Compile(compiler);
            }

            // Act
            var ex = Assert.ThrowsException<SyntaxException>((Action)Action);

            // Assert
            Assert.That.ErrorSyntaxFormatArgsEqual(ex,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Expression),
                source
            );

            literalMock.Verify(o => o.Compile(compiler), Times.Never);
            exprRhsMock.Verify(o => o.Compile(compiler), Times.Never);
        }

        [TestMethod]
        public void AssignNoneTest()
        {
            // Arrange
            var compiler = new PyCompiler();

            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralNone>(source);

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> exprRhsMock,
                out NopOp exprRhsOp);

            var stmt = new Assignment(SourceReference.ClrSource,
                leftOperand: literalMock.Object,
                rightOperand: exprRhsMock.Object);

            void Action()
            {
                stmt.Compile(compiler);
            }

            // Act
            var ex = Assert.ThrowsException<SyntaxException>((Action)Action);

            // Assert
            Assert.That.ErrorSyntaxFormatArgsEqual(ex,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_None),
                source
            );

            literalMock.Verify(o => o.Compile(compiler), Times.Never);
            exprRhsMock.Verify(o => o.Compile(compiler), Times.Never);
        }

    }
}