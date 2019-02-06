using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Statements;

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor
{
    [TestClass]
    public class VisitExpressionTests : BaseVisitClass
    {
        [TestMethod]
        public void Expr_Visit_TestListStarExpr_BothSides_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Expr_stmtContext>();

            ExpressionNode expr = GetExpressionMock();

            var lhsMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();
            var rhsMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

            ctorMock.Setup(o => o.VisitTestlist_star_expr(It.IsAny<Python3Parser.Testlist_star_exprContext>()))
                .Returns(expr);

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

            //ctorMock.Verify(o => o.visit(lhsMock.Object), Times.Exactly(3));

            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}