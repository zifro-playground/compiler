using System;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Base.Resources;
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
    public class ForStatementTests
    {
        [TestMethod]
        public void ForLoopAssignIdentifier()
        {
            // Arrange
            var compiler = new PyCompiler();

            var id = new Identifier(SourceReference.ClrSource, "foo");

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> iterMock,
                out var iterOp);

            compiler.CreateAndSetup(
                out Mock<Statement> suiteMock,
                out var suiteOp);

            var stmt = new ForStatement(SourceReference.ClrSource,
                id, iterMock.Object, suiteMock.Object
            );

            // Act
            stmt.Compile(compiler);

            // Assert

            /*
             * nop "iter"
             * iter->prep
             * iter->next
             * set->foo
             * nop "suite"
             * iter->end
             */
        }

        private static void _ThrowAssignLiteralIntegerTest<TLiteral>(
            SourceReference source,
            Mock<TLiteral> literalMock,
            string errorLocalizedKey,
            params object[] errorFormatArgs)
            where TLiteral : ExpressionNode
        {
            // Arrange
            var compiler = new PyCompiler();

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> iterMock,
                out var iterOp);

            compiler.CreateAndSetup(
                out Mock<Statement> suiteMock,
                out var suiteOp);

            var stmt = new ForStatement(SourceReference.ClrSource,
                literalMock.Object, iterMock.Object, suiteMock.Object
            );

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
            iterMock.Verify(o => o.Compile(compiler), Times.Never);
            suiteMock.Verify(o => o.Compile(compiler), Times.Never);
        }

        [TestMethod]
        public void ThrowAssignLiteralIntegerTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralInteger>(source, 5);

            _ThrowAssignLiteralIntegerTest(
                source,
                literalMock,
                errorLocalizedKey: nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Literal),
                literalMock.Object.GetTypeName());
        }

        [TestMethod]
        public void ThrowAssignLiteralDoubleTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralDouble>(source, 10d);

            _ThrowAssignLiteralIntegerTest(
                source,
                literalMock,
                errorLocalizedKey: nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Literal),
                literalMock.Object.GetTypeName());
        }

        [TestMethod]
        public void ThrowAssignLiteralStringTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralString>(source, "foo");

            _ThrowAssignLiteralIntegerTest(
                source,
                literalMock,
                errorLocalizedKey: nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Literal),
                literalMock.Object.GetTypeName());
        }

        [TestMethod]
        public void ThrowAssignTrueTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralBoolean>(source, true);
            _ThrowAssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Boolean),
                Localized_Base_Entities.Type_Boolean_True
            );
        }

        [TestMethod]
        public void ThrowAssignFalseTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralBoolean>(source, false);
            _ThrowAssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Boolean),
                Localized_Base_Entities.Type_Boolean_False
            );
        }

        [TestMethod]
        public void ThrowAssignExpression()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<ExpressionNode>(source);

            _ThrowAssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Expression)
            );
        }

        [TestMethod]
        public void ThrowAssignNoneTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralNone>(source);

            _ThrowAssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_None)
            );
        }

    }
}