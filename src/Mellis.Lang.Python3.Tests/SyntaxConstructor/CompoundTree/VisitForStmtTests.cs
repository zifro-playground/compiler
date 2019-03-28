using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Statements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor.CompoundTree
{
    [TestClass]
    public class VisitForStmtTests : BaseVisitClass<Python3Parser.For_stmtContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitFor_stmt(contextMock.Object);
        }

        private void CreateAndSetupExprList(out Mock<Python3Parser.ExprlistContext> testMock, out ExpressionNode testExpr)
        {
            testMock = GetMockRule<Python3Parser.ExprlistContext>();
            var refCopy = testMock;
            testExpr = ctorMock.SetupExpressionMock(o => o.VisitExprlist(refCopy.Object));
        }

        private void CreateAndSetupTestList(out Mock<Python3Parser.TestlistContext> testMock, out ExpressionNode testExpr)
        {
            testMock = GetMockRule<Python3Parser.TestlistContext>();
            var refCopy = testMock;
            testExpr = ctorMock.SetupExpressionMock(o => o.VisitTestlist(refCopy.Object));
        }

        private void CreateAndSetupSuite(out Mock<Python3Parser.SuiteContext> suiteMock, out Statement suiteStmt)
        {
            suiteMock = GetMockRule<Python3Parser.SuiteContext>();
            var refCopy = suiteMock;
            suiteStmt = ctorMock.SetupStatementMock(o => o.VisitSuite(refCopy.Object));
        }

        [TestMethod]
        public void Visit_SingleIdentifier()
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            CreateAndSetupExprList(
                out var idRuleMock,
                out var idExpr);
            CreateAndSetupTestList(
                out var iterRuleMock,
                out var iter);
            CreateAndSetupSuite(
                out var suiteRuleMock,
                out var suiteStmt);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.FOR),
                idRuleMock.Object,
                GetTerminal(Python3Parser.IN),
                iterRuleMock.Object,
                GetTerminal(Python3Parser.COLON),
                suiteRuleMock.Object
            );

            // Act
            var result = VisitContext();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ForStatement));
            var forStmt = (ForStatement)result;
            Assert.AreSame(idExpr, forStmt.Operand);
            Assert.AreSame(iter, forStmt.Iterator);
            Assert.AreSame(suiteStmt, forStmt.Suite);
        }

        [TestMethod]
        public void Visit_WithElseStatement()
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            CreateAndSetupExprList(
                out var idRuleMock,
                out var idExpr);
            CreateAndSetupTestList(
                out var iterRuleMock,
                out var iter);
            CreateAndSetupSuite(
                out var suiteRuleMock,
                out var suiteStmt);
            CreateAndSetupSuite(
                out var elseRuleMock,
                out var elseSuiteStmt);

            var elseTerm = GetTerminal(Python3Parser.ELSE);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.FOR),
                idRuleMock.Object,
                GetTerminal(Python3Parser.IN),
                iterRuleMock.Object,
                GetTerminal(Python3Parser.COLON),
                suiteRuleMock.Object,
                elseTerm,
                GetTerminal(Python3Parser.COLON),
                elseRuleMock.Object
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex, elseTerm, "for..else");
        }

        [TestMethod]
        public void Visit_MissingSuite()
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            CreateAndSetupExprList(
                out var idRuleMock,
                out var idExpr);
            CreateAndSetupTestList(
                out var iterRuleMock,
                out var iter);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.FOR),
                idRuleMock.Object,
                GetTerminal(Python3Parser.IN),
                iterRuleMock.Object,
                GetTerminal(Python3Parser.COLON)
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);
        }
    }
}