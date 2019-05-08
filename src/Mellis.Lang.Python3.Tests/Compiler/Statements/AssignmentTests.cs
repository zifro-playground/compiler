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
                out var exprRhsOp);

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

        private static void _AssignLiteralIntegerTest<TLiteral>(
            SourceReference source,
            Mock<TLiteral> literalMock,
            string errorLocalizedKey,
            params object[] errorFormatArgs)
            where TLiteral : ExpressionNode
        {
            // Arrange
            var compiler = new PyCompiler();

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> exprRhsMock,
                out var exprRhsOp);

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
                errorLocalizedKey,
                source,
                errorFormatArgs
            );

            literalMock.Verify(o => o.Compile(compiler), Times.Never);
            exprRhsMock.Verify(o => o.Compile(compiler), Times.Never);
        }

        [TestMethod]
        public void AssignLiteralIntegerTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralInteger>(source, 5);

            _AssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Literal),
                literalMock.Object.GetTypeName()
            );
        }

        [TestMethod]
        public void AssignLiteralDoubleTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralDouble>(source, 10d);

            _AssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Literal),
                literalMock.Object.GetTypeName()
            );
        }

        [TestMethod]
        public void AssignLiteralStringTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralString>(source, "foo");

            _AssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Literal),
                literalMock.Object.GetTypeName()
            );
        }

        [TestMethod]
        public void AssignTrueTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralBoolean>(source, true);

            _AssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Boolean),
                Localized_Base_Entities.Type_Boolean_True
            );
        }

        [TestMethod]
        public void AssignFalseTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralBoolean>(source, false);

            _AssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Boolean),
                Localized_Base_Entities.Type_Boolean_False
            );
        }

        [TestMethod]
        public void AssignExpression()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<ExpressionNode>(source);

            _AssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Expression)
            );
        }

        [TestMethod]
        public void AssignNoneTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralNone>(source);

            _AssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_None)
            );
        }

    }
}