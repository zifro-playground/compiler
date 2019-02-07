using System;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Statements;

// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_Elsewhere

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor
{
    [TestClass]
    public class VisitExprStmtTests : BaseVisitClass
    {
        [TestInitialize]
        public void TestInitializeExprStmt()
        {
            ctorMock.Setup(o => o.VisitTestlist_star_expr(It.IsAny<Python3Parser.Testlist_star_exprContext>()))
                .Returns(GetExpressionMock());
        }

        [TestMethod]
        public void Visit_BasicAssignment_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Expr_stmtContext>();
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            var lhsMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();
            var rhsMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

            contextMock.SetupChildren(
                lhsMock.Object,
                GetTerminal(Python3Parser.ASSIGN),
                rhsMock.Object
            );

            // Act
            SyntaxNode result = ctor.VisitExpr_stmt(contextMock.Object);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Assignment));
            contextMock.VerifyLoopedChildren(3);

            ctorMock.Verify(o => o.VisitTestlist_star_expr(lhsMock.Object), Times.Once);
            ctorMock.Verify(o => o.VisitTestlist_star_expr(rhsMock.Object), Times.Once);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_MultiAssignment_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Expr_stmtContext>();

            var testListMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

            ITerminalNode secondEqual = GetTerminal(Python3Parser.ASSIGN);

            contextMock.SetupChildren(
                testListMock.Object,
                GetTerminal(Python3Parser.ASSIGN),
                testListMock.Object,
                secondEqual,
                testListMock.Object
            );

            Action action = delegate { ctor.VisitExpr_stmt(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(action);

            Assert.That.ErrorNotYetImplFormatArgs(ex, secondEqual, "=");
            // Should throw at 4th
            contextMock.VerifyLoopedChildren(4);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_Empty_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Expr_stmtContext>();
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren();

            Action action = delegate { ctor.VisitExpr_stmt(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(action);

            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);
            contextMock.VerifyLoopedChildren(0);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidOrdering_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Expr_stmtContext>();

            var testListMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();
            var secondTestListMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();
            secondTestListMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                testListMock.Object,
                secondTestListMock.Object,
                GetTerminal(Python3Parser.ASSIGN)
            );

            Action action = delegate { ctor.VisitExpr_stmt(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(action);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock,
                secondTestListMock.Object);
            // Should throw at 2nd
            contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_AugmentedAssignment_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Expr_stmtContext>();

            var testListStarMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();
            var augAssignMock = GetMockRule<Python3Parser.AugassignContext>();
            var testListMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

            var addAssign = GetTerminal(Python3Parser.ADD_ASSIGN);
            augAssignMock.SetupChildren(addAssign);
            augAssignMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                testListStarMock.Object,
                augAssignMock.Object,
                testListMock.Object
            );

            Action action = delegate { ctor.VisitExpr_stmt(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(action);

            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock, "+=");
            // Should throw at 2nd
            contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_AnnotatedAssignment_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Expr_stmtContext>();

            var testListStarMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();
            var annAssignMock = GetMockRule<Python3Parser.AnnassignContext>();
            annAssignMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                testListStarMock.Object,
                annAssignMock.Object
            );

            Action action = delegate { ctor.VisitExpr_stmt(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(action);

            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock, ":");
            contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_YieldExpression_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Expr_stmtContext>();

            var testListStarMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();
            var yieldExprMock = GetMockRule<Python3Parser.Yield_exprContext>();
            yieldExprMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                testListStarMock.Object,
                yieldExprMock.Object
            );

            Action action = delegate { ctor.VisitExpr_stmt(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(action);

            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock, "yield");
            contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}