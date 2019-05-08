using Antlr4.Runtime.Tree;
using Mellis.Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Statements;

// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_Elsewhere

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor
{
    [TestClass]
    public class VisitExprStmtTests : BaseVisitClass<Python3Parser.Expr_stmtContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitExpr_stmt(contextMock.Object);
        }

        [TestInitialize]
        public void TestInitializeExprStmt()
        {
            ctorMock.Setup(o => o.VisitTestlist_star_expr(It.IsAny<Python3Parser.Testlist_star_exprContext>()))
                .Returns(GetExpressionMock());
        }

        [TestMethod]
        public void Visit_SingleTestListStarExpr_Test()
        {
            // Arrange
            var expr = GetExpressionMock();
            var innerMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();
            ctorMock.Setup(o => o.VisitTestlist_star_expr(innerMock.Object))
                .Returns(expr).Verifiable();

            contextMock.SetupChildren(
                innerMock.Object
            );

            // Act
            var result = VisitContext();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ExpressionStatement));
            var resultExpr = (ExpressionStatement)result;
            Assert.AreSame(expr, resultExpr.Expression);

            contextMock.VerifyLoopedChildren(1);

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidAssignmentTerminal()
        {
            // Arrange
            var testListMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

            var unexpected = GetTerminal(Python3Parser.ADD);

            contextMock.SetupChildren(
                testListMock.Object,
                unexpected,
                testListMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpected);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_BasicAssignment_Test()
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            var lhsMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();
            var rhsMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

            contextMock.SetupChildren(
                lhsMock.Object,
                GetTerminal(Python3Parser.ASSIGN),
                rhsMock.Object
            );

            // Act
            var result = VisitContext();

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
            var testListMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

            var secondEqual = GetTerminal(Python3Parser.ASSIGN);

            contextMock.SetupChildren(
                testListMock.Object,
                GetTerminal(Python3Parser.ASSIGN),
                testListMock.Object,
                secondEqual,
                testListMock.Object
            );
            
            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, secondEqual, "=");
            // Should throw at 4th
            contextMock.VerifyLoopedChildren(4);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidOrdering_Test()
        {
            // Arrange
            var testListMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();
            var secondTestListMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();
            secondTestListMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                testListMock.Object,
                secondTestListMock.Object,
                GetTerminal(Python3Parser.ASSIGN)
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

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
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            var lhsMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();
            var rhsMock = GetMockRule<Python3Parser.TestlistContext>();
            var augAssignMock = GetMockRule<Python3Parser.AugassignContext>();
            
            ctorMock.Setup(o => o.VisitAugassign(augAssignMock.Object))
                .Returns(new AugmentedAssignment(SourceReference.ClrSource, BasicOperatorCode.AMul))
                .Verifiable();

            contextMock.SetupChildren(
                lhsMock.Object,
                augAssignMock.Object,
                rhsMock.Object
            );

            var lhsExpr = ctorMock.SetupExpressionMock(o => o.VisitTestlist_star_expr(lhsMock.Object));
            var rhsExpr = ctorMock.SetupExpressionMock(o => o.VisitTestlist(rhsMock.Object));

            // Act
            var result = VisitContext();

            // Assert
            Assert.IsInstanceOfType(result, typeof(AugmentedAssignment));
            var augAssignResult = (AugmentedAssignment)result;
            Assert.AreSame(lhsExpr, augAssignResult.LeftOperand);
            Assert.AreSame(rhsExpr, augAssignResult.RightOperand);
            Assert.AreEqual(BasicOperatorCode.AMul, augAssignResult.OpCode);

            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_AnnotatedAssignment_Test()
        {
            // Arrange
            var testListStarMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();
            var annAssignMock = GetMockRule<Python3Parser.AnnassignContext>();
            annAssignMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                testListStarMock.Object,
                annAssignMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock, ":");
            contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_YieldAssignExpression_Test()
        {
            // Arrange
            var testListStarMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();
            var yieldExprMock = GetMockRule<Python3Parser.Yield_exprContext>();
            yieldExprMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                testListStarMock.Object,
                GetTerminal(Python3Parser.ASSIGN),
                yieldExprMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock, "yield");
            contextMock.VerifyLoopedChildren(3);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_OnlyAssignmentToken_Test()
        {
            // Arrange
            var assignNode = GetTerminal(Python3Parser.ASSIGN);

            contextMock.SetupChildren(
                assignNode
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, assignNode);
            contextMock.VerifyLoopedChildren(1);

            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}