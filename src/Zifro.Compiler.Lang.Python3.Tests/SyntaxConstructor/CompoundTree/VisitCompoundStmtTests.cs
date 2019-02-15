using System;
using System.Linq.Expressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor.CompoundTree
{
    [TestClass]
    public class VisitCompoundStmtTests : BaseVisitClass<Python3Parser.Compound_stmtContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitCompound_stmt(contextMock.Object);
        }

        [TestMethod]
        public void Visit_TooMany_Test()
        {
            // Arrange
            var innerMock = GetMockRule<Python3Parser.If_stmtContext>();
            var unexpectedMock = GetMockRule<Python3Parser.If_stmtContext>();
            unexpectedMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                innerMock.Object,
                unexpectedMock.Object
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, unexpectedMock.Object);

            innerMock.Verify();
            unexpectedMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_IfStmt_Test()
        {
            PerformVisitStatement<Python3Parser.If_stmtContext>(
                o => o.VisitIf_stmt(It.IsAny<Python3Parser.If_stmtContext>()));
        }

        [TestMethod]
        public void Visit_WhileStmt_Test()
        {
            PerformVisitStatement<Python3Parser.While_stmtContext>(
                o => o.VisitWhile_stmt(It.IsAny<Python3Parser.While_stmtContext>()));
        }

        [TestMethod]
        public void Visit_ForStmt_Test()
        {
            PerformVisitStatement<Python3Parser.For_stmtContext>(
                o => o.VisitFor_stmt(It.IsAny<Python3Parser.For_stmtContext>()));
        }

        [TestMethod]
        public void Visit_TryStmt_Test()
        {
            PerformVisitStatement<Python3Parser.Try_stmtContext>(
                o => o.VisitTry_stmt(It.IsAny<Python3Parser.Try_stmtContext>()));
        }

        [TestMethod]
        public void Visit_WithStmt_Test()
        {
            PerformVisitStatement<Python3Parser.With_stmtContext>(
                o => o.VisitWith_stmt(It.IsAny<Python3Parser.With_stmtContext>()));
        }

        [TestMethod]
        public void Visit_FuncDefStmt_Test()
        {
            PerformVisitStatement<Python3Parser.FuncdefContext>(
                o => o.VisitFuncdef(It.IsAny<Python3Parser.FuncdefContext>()));
        }

        [TestMethod]
        public void Visit_ClassStmt_Test()
        {
            PerformVisitStatement<Python3Parser.ClassdefContext>(
                o => o.VisitClassdef(It.IsAny<Python3Parser.ClassdefContext>()));
        }

        [TestMethod]
        public void Visit_DecoratedStmt_Test()
        {
            PerformVisitStatement<Python3Parser.DecoratedContext>(
                o => o.VisitDecorated(It.IsAny<Python3Parser.DecoratedContext>()));
        }

        [TestMethod]
        public void Visit_AsyncStmt_Test()
        {
            PerformVisitStatement<Python3Parser.Async_stmtContext>(
                o => o.VisitAsync_stmt(It.IsAny<Python3Parser.Async_stmtContext>()));
        }

        protected void PerformVisitStatement<TInnerRule>(
            Expression<Func<Grammar.SyntaxConstructor, SyntaxNode>> setupExpression)
            where TInnerRule : ParserRuleContext
        {
            // Arrange
            var innerMock = GetMockRule<TInnerRule>();
            Statement statement = GetStatementMock();

            ctorMock.Setup(setupExpression)
                .Returns(statement).Verifiable();

            contextMock.SetupChildren(
                innerMock.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.AreSame(statement, result, "Visit did not return same statement.");
            contextMock.VerifyLoopedChildren(1);
            ctorMock.Verify(setupExpression);

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}