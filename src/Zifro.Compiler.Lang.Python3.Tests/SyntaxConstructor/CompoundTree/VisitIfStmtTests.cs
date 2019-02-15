using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Resources;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Statements;

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor.CompoundTree
{
    [TestClass]
    public class VisitIfStmtTests : BaseVisitClass<Python3Parser.If_stmtContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitIf_stmt(contextMock.Object);
        }

        private void CreateAndSetupTest(out Mock<Python3Parser.TestContext> testMock, out ExpressionNode testExpr)
        {
            testMock = GetMockRule<Python3Parser.TestContext>();
            Mock<Python3Parser.TestContext> refCopy = testMock;
            testExpr = ctorMock.SetupExpressionMock(o => o.VisitTest(refCopy.Object));
        }

        private void CreateAndSetupSuite(out Mock<Python3Parser.SuiteContext> suiteMock, out Statement suiteStmt)
        {
            suiteMock = GetMockRule<Python3Parser.SuiteContext>();
            Mock<Python3Parser.SuiteContext> refCopy = suiteMock;
            suiteStmt = ctorMock.SetupStatementMock(o => o.VisitSuite(refCopy.Object));
        }

        [TestMethod]
        public void Visit_If_Test()
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            CreateAndSetupTest(
                out Mock<Python3Parser.TestContext> testMock,
                out ExpressionNode testExpr);
            CreateAndSetupSuite(
                out Mock<Python3Parser.SuiteContext> suiteMock, 
                out Statement suiteStmt);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.IF),
                testMock.Object,
                GetTerminal(Python3Parser.COLON),
                suiteMock.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            /* Expected tree:
             *
             * IF
             * : test: testExpr
             * : suite: suiteStmt
             * : else: null
             */
            Assert.IsInstanceOfType(result, typeof(IfStatement));
            var ifStmt = (IfStatement)result;
            Assert.AreSame(testExpr, ifStmt.Condition, "if condition did not match");
            Assert.AreSame(ifStmt.IfSuite, suiteStmt);
            Assert.IsNull(ifStmt.ElseSuite, "ifStmt.ElseSuite was not null");

            testMock.Verify();
            suiteMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_If_Else_Test()
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            CreateAndSetupTest(
                out Mock<Python3Parser.TestContext> testMock,
                out ExpressionNode testExpr);
            var suiteMock = GetMockRule<Python3Parser.SuiteContext>();
            var elseMock = GetMockRule<Python3Parser.SuiteContext>();

            var suiteStmt = ctorMock.SetupStatementMock(o => o.VisitSuite(suiteMock.Object));
            var elseStmt = ctorMock.SetupStatementMock(o => o.VisitSuite(elseMock.Object));

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.IF),
                testMock.Object,
                GetTerminal(Python3Parser.COLON),
                suiteMock.Object,
                GetTerminal(Python3Parser.ELSE),
                GetTerminal(Python3Parser.COLON),
                elseMock.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            /* Expected tree:
             *
             * IF
             * : test: testExpr
             * : suite: suiteStmt
             * : else: elseStmt
             */
            Assert.IsInstanceOfType(result, typeof(IfStatement));
            var ifStmt = (IfStatement)result;
            Assert.AreSame(testExpr, ifStmt.Condition, "if condition did not match");
            Assert.AreSame(ifStmt.IfSuite, suiteStmt);
            Assert.AreSame(ifStmt.ElseSuite, elseStmt);

            testMock.Verify();
            suiteMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_If_ElIf_Test()
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            CreateAndSetupTest(
                out Mock<Python3Parser.TestContext> testMock,
                out ExpressionNode testExpr);

            CreateAndSetupSuite(
                out Mock<Python3Parser.SuiteContext> suiteMock,
                out Statement suiteStmt);

            CreateAndSetupTest(
                out Mock<Python3Parser.TestContext> elifTestMock,
                out ExpressionNode elifTestExpr);

            CreateAndSetupSuite(
                out Mock<Python3Parser.SuiteContext> elifSuiteMock,
                out Statement elifStmt);

            elifSuiteMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.IF),
                testMock.Object,
                GetTerminal(Python3Parser.COLON),
                suiteMock.Object,
                GetTerminal(Python3Parser.ELIF),
                elifTestMock.Object,
                GetTerminal(Python3Parser.COLON),
                elifSuiteMock.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            /* Expected tree:
             *
             * IF
             * : test: testExpr
             * : suite: suiteStmt
             * : else:
             *      IF
             *      : test: elifTestExpr
             *      : suite: elifStmt
             *      : else: null
             */
            Assert.IsInstanceOfType(result, typeof(IfStatement));
            var ifStmt = (IfStatement)result;
            Assert.AreSame(testExpr, ifStmt.Condition, "if condition did not match");
            Assert.AreSame(ifStmt.IfSuite, suiteStmt);

            var innerIf = (IfStatement) ifStmt.ElseSuite;
            Assert.AreSame(elifTestExpr, innerIf.Condition);
            Assert.AreSame(innerIf.IfSuite, elifStmt);
            Assert.IsNull(innerIf.ElseSuite);

            testMock.Verify();
            suiteMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_If_ElIf_Else_Test()
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            CreateAndSetupTest(
                out Mock<Python3Parser.TestContext> testMock,
                out ExpressionNode testExpr);

            CreateAndSetupSuite(
                out Mock<Python3Parser.SuiteContext> suiteMock,
                out Statement suiteStmt);

            CreateAndSetupTest(
                out Mock<Python3Parser.TestContext> elifTestMock,
                out ExpressionNode elifTestExpr);

            CreateAndSetupSuite(
                out Mock<Python3Parser.SuiteContext> elifSuiteMock,
                out Statement elifStmt);

            elifSuiteMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            CreateAndSetupSuite(
                out Mock<Python3Parser.SuiteContext> elseSuiteMock,
                out Statement elseStmt);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.IF),
                testMock.Object,
                GetTerminal(Python3Parser.COLON),
                suiteMock.Object,

                GetTerminal(Python3Parser.ELIF),
                elifTestMock.Object,
                GetTerminal(Python3Parser.COLON),
                elifSuiteMock.Object,

                GetTerminal(Python3Parser.ELSE),
                GetTerminal(Python3Parser.COLON),
                elseSuiteMock.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            /* Expected tree:
             *
             * IF
             * : test: testExpr
             * : suite: suiteStmt
             * : else:
             *      IF
             *      : test: elifTestExpr
             *      : suite: elifStmt
             *      : else: elseStmt
             */
            Assert.IsInstanceOfType(result, typeof(IfStatement));
            var ifStmt = (IfStatement)result;
            Assert.AreSame(testExpr, ifStmt.Condition, "if condition did not match");
            Assert.AreSame(ifStmt.IfSuite, suiteStmt);

            var innerIf = (IfStatement)ifStmt.ElseSuite;
            Assert.AreSame(elifTestExpr, innerIf.Condition);
            Assert.AreSame(innerIf.IfSuite, elifStmt);

            Assert.AreSame(innerIf.ElseSuite, elseStmt);

            testMock.Verify();
            suiteMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_If_2ElIf_Else_Test()
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            CreateAndSetupTest(
                out Mock<Python3Parser.TestContext> testMock,
                out ExpressionNode testExpr);

            CreateAndSetupSuite(
                out Mock<Python3Parser.SuiteContext> suiteMock,
                out Statement suiteStmt);

            CreateAndSetupTest(
                out Mock<Python3Parser.TestContext> elif1TestMock,
                out ExpressionNode elif1TestExpr);

            CreateAndSetupSuite(
                out Mock<Python3Parser.SuiteContext> elif1SuiteMock,
                out Statement elif1Stmt);

            elif1SuiteMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            CreateAndSetupTest(
                out Mock<Python3Parser.TestContext> elif2TestMock,
                out ExpressionNode elif2TestExpr);

            CreateAndSetupSuite(
                out Mock<Python3Parser.SuiteContext> elif2SuiteMock,
                out Statement elif2Stmt);

            elif2SuiteMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            CreateAndSetupSuite(
                out Mock<Python3Parser.SuiteContext> elseSuiteMock,
                out Statement elseStmt);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.IF),
                testMock.Object,
                GetTerminal(Python3Parser.COLON),
                suiteMock.Object,

                GetTerminal(Python3Parser.ELIF),
                elif1TestMock.Object,
                GetTerminal(Python3Parser.COLON),
                elif1SuiteMock.Object,

                GetTerminal(Python3Parser.ELIF),
                elif2TestMock.Object,
                GetTerminal(Python3Parser.COLON),
                elif2SuiteMock.Object,

                GetTerminal(Python3Parser.ELSE),
                GetTerminal(Python3Parser.COLON),
                elseSuiteMock.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            /* Expected tree:
             *
             * IF
             * : test: testExpr
             * : suite: suiteStmt
             * : else:
             *      IF
             *      : test: elif1TestExpr
             *      : suite: elif1Stmt
             *      : else:
             *      IF
             *          : test: elif2TestExpr
             *          : suite: elif2Stmt
             *          : else: elseStmt
             */

            Assert.IsInstanceOfType(result, typeof(IfStatement));
            var ifStmt = (IfStatement)result;
            Assert.AreSame(testExpr, ifStmt.Condition, "if condition did not match");
            Assert.AreSame(ifStmt.IfSuite, suiteStmt);

            var innerIf1 = (IfStatement)ifStmt.ElseSuite;
            Assert.AreSame(elif1TestExpr, innerIf1.Condition);
            Assert.AreSame(innerIf1.IfSuite, elif1Stmt);

            var innerIf2 = (IfStatement)innerIf1.ElseSuite;
            Assert.AreSame(elif2TestExpr, innerIf2.Condition);
            Assert.AreSame(innerIf2.IfSuite, elif2Stmt);
            Assert.AreSame(innerIf2.ElseSuite, elseStmt);

            testMock.Verify();
            suiteMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Invalid_If_MissingColon_Test()
        {
            // Arrange
            var testMock = GetMockRule<Python3Parser.TestContext>();
            var suiteMock = GetMockRule<Python3Parser.SuiteContext>();
            ITerminalNode missingColon = GetMissingTerminal(Python3Parser.COLON);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.IF),
                testMock.Object,
                missingColon,
                suiteMock.Object
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorSyntaxFormatArgsEqual(ex,
                nameof(Localized_Python3_Parser.Ex_Syntax_If_MissingColon),
                missingColon);

            testMock.Verify();
            suiteMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Invalid_If_Else_MissingColon_Test()
        {
            // Arrange
            CreateAndSetupTest(
                out Mock<Python3Parser.TestContext> testMock,
                out _);
            CreateAndSetupSuite(
                out Mock<Python3Parser.SuiteContext> suiteMock,
                out Statement _);

            var elseSuiteMock = GetMockRule<Python3Parser.SuiteContext>();
            ITerminalNode missingColon = GetMissingTerminal(Python3Parser.COLON);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.IF),
                testMock.Object,
                GetTerminal(Python3Parser.COLON),
                suiteMock.Object,
                GetTerminal(Python3Parser.ELSE),
                missingColon,
                elseSuiteMock.Object
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorSyntaxFormatArgsEqual(ex,
                nameof(Localized_Python3_Parser.Ex_Syntax_If_Else_MissingColon),
                missingColon);

            testMock.Verify();
            suiteMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Invalid_If_Elif_MissingColon_Test()
        {
            // Arrange
            CreateAndSetupTest(
                out Mock<Python3Parser.TestContext> testMock,
                out _);
            CreateAndSetupSuite(
                out Mock<Python3Parser.SuiteContext> suiteMock,
                out Statement _);

            var elifTestMock = GetMockRule<Python3Parser.TestContext>();
            var elifSuiteMock = GetMockRule<Python3Parser.SuiteContext>();
            ITerminalNode missingColon = GetMissingTerminal(Python3Parser.COLON);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.IF),
                testMock.Object,
                GetTerminal(Python3Parser.COLON),
                suiteMock.Object,
                GetTerminal(Python3Parser.ELIF),
                elifTestMock.Object,
                missingColon,
                elifSuiteMock.Object
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorSyntaxFormatArgsEqual(ex,
                nameof(Localized_Python3_Parser.Ex_Syntax_If_Elif_MissingColon),
                missingColon);

            testMock.Verify();
            suiteMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}