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
    public class VisitWhileStmtTests : BaseVisitClass<Python3Parser.While_stmtContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitWhile_stmt(contextMock.Object);
        }

        private void CreateAndSetupTest(out Mock<Python3Parser.TestContext> testMock, out ExpressionNode testExpr)
        {
            testMock = GetMockRule<Python3Parser.TestContext>();
            var refCopy = testMock;
            testExpr = ctorMock.SetupExpressionMock(o => o.VisitTest(refCopy.Object));
        }

        private void CreateAndSetupSuite(out Mock<Python3Parser.SuiteContext> suiteMock, out Statement suiteStmt)
        {
            suiteMock = GetMockRule<Python3Parser.SuiteContext>();
            var refCopy = suiteMock;
            suiteStmt = ctorMock.SetupStatementMock(o => o.VisitSuite(refCopy.Object));
        }

        [TestMethod]
        public void ValidWhile()
        {
            // Arrange
            CreateAndSetupTest(
                out var testMock,
                out var testExpr
            );

            CreateAndSetupSuite(
                out var suiteMock,
                out var suiteStmt
            );

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.WHILE),
                testMock.Object,
                GetTerminal(Python3Parser.COLON),
                suiteMock.Object
            );

            // Act
            var result = VisitContext();

            // Assert
            Assert.IsInstanceOfType(result, typeof(WhileStatement));
            var resultWhile = (WhileStatement)result;

            Assert.AreSame(testExpr, resultWhile.Condition);
            Assert.AreSame(suiteStmt, resultWhile.Suite);
        }

        [TestMethod]
        public void ThrowOnElseSuiteNotYetImplemented()
        {
            // Arrange
            CreateAndSetupTest(
                out var testMock,
                out _
            );

            CreateAndSetupSuite(
                out var suiteMock,
                out _
            );

            CreateAndSetupSuite(
                out var elseMock,
                out _
            );

            var unexpected = GetTerminal(Python3Parser.ELSE);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.WHILE),
                testMock.Object,
                GetTerminal(Python3Parser.COLON),
                suiteMock.Object,
                unexpected,
                GetTerminal(Python3Parser.COLON),
                elseMock.Object
            );

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex, unexpected, "while..else");
        }

        [TestMethod]
        public void ThrowOnMissingTest()
        {
            // Arrange
            CreateAndSetupSuite(
                out var suiteMock,
                out _
            );

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.WHILE),
                //testMock.Object,
                GetTerminal(Python3Parser.COLON),
                suiteMock.Object
            );

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorExpectedChildFormatArgs(ex,
                startTokenMock, stopTokenMock, contextMock
            );
        }

        [TestMethod]
        public void ThrowOnMissingSuite()
        {
            // Arrange
            CreateAndSetupTest(
                out var testMock,
                out _
            );

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.WHILE),
                testMock.Object,
                GetTerminal(Python3Parser.COLON)
                //suiteMock.Object
            );

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorExpectedChildFormatArgs(ex,
                startTokenMock, stopTokenMock, contextMock
            );
        }

        [TestMethod]
        public void ThrowOnMissingWhileTerminal()
        {
            // Arrange
            CreateAndSetupTest(
                out var testMock,
                out _
            );

            CreateAndSetupSuite(
                out var suiteMock,
                out _
            );

            contextMock.SetupChildren(
                //GetTerminal(Python3Parser.WHILE),
                testMock.Object,
                GetTerminal(Python3Parser.COLON),
                suiteMock.Object
            );

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorExpectedChildFormatArgs(ex,
                startTokenMock, stopTokenMock, contextMock
            );
        }

        [TestMethod]
        public void ThrowOnMissingColonTerminal()
        {
            // Arrange
            CreateAndSetupTest(
                out var testMock,
                out _
            );

            CreateAndSetupSuite(
                out var suiteMock,
                out _
            );

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.WHILE),
                testMock.Object,
                //GetTerminal(Python3Parser.COLON),
                suiteMock.Object
            );

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorExpectedChildFormatArgs(ex,
                startTokenMock, stopTokenMock, contextMock
            );
        }

        [TestMethod]
        public void ThrowOnMissingElseTerminal()
        {
            // Arrange
            CreateAndSetupTest(
                out var testMock,
                out _
            );

            CreateAndSetupSuite(
                out var suiteMock,
                out _
            );

            CreateAndSetupSuite(
                out var elseMock,
                out _
            );

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.WHILE),
                testMock.Object,
                GetTerminal(Python3Parser.COLON),
                suiteMock.Object,
                //GetTerminal(Python3Parser.ELSE),
                GetTerminal(Python3Parser.COLON),
                elseMock.Object
            );

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorExpectedChildFormatArgs(ex,
                startTokenMock, stopTokenMock, contextMock
            );
        }

        [TestMethod]
        public void ThrowOnMissingElseColonTerminal()
        {
            // Arrange
            CreateAndSetupTest(
                out var testMock,
                out _
            );

            CreateAndSetupSuite(
                out var suiteMock,
                out _
            );

            CreateAndSetupSuite(
                out var elseMock,
                out _
            );

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.WHILE),
                testMock.Object,
                GetTerminal(Python3Parser.COLON),
                suiteMock.Object,
                GetTerminal(Python3Parser.ELSE),
                //GetTerminal(Python3Parser.COLON),
                elseMock.Object
            );

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorExpectedChildFormatArgs(ex,
                startTokenMock, stopTokenMock, contextMock
            );
        }

        [TestMethod]
        public void ThrowOnMissingElseSuite()
        {
            // Arrange
            CreateAndSetupTest(
                out var testMock,
                out _
            );

            CreateAndSetupSuite(
                out var suiteMock,
                out _
            );

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.WHILE),
                testMock.Object,
                GetTerminal(Python3Parser.COLON),
                suiteMock.Object,
                GetTerminal(Python3Parser.ELSE),
                GetTerminal(Python3Parser.COLON)
                //elseMock.Object
            );

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorExpectedChildFormatArgs(ex,
                startTokenMock, stopTokenMock, contextMock
            );
        }
    }
}