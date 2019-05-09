using System;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Literals;
using Mellis.Lang.Python3.Syntax.Statements;
using Mellis.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Compiler.Statements
{
    [TestClass]
    public class AssignmentTests
    {
        private static void _AssignLiteralTemplateTest<TLiteral>(
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
                out _);

            var stmt = new Assignment(SourceReference.ClrSource,
                literalMock.Object,
                exprRhsMock.Object);

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
                idLhsMock.Object,
                exprRhsMock.Object);

            // Act
            stmt.Compile(compiler);

            // Assert
            var setOpCode = Assert.That.IsOpCode<VarSet>(compiler, 1);
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

            _AssignLiteralTemplateTest(
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

            _AssignLiteralTemplateTest(
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

            _AssignLiteralTemplateTest(
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

            _AssignLiteralTemplateTest(
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

            _AssignLiteralTemplateTest(
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

            _AssignLiteralTemplateTest(
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

            _AssignLiteralTemplateTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_None)
            );
        }
    }
}