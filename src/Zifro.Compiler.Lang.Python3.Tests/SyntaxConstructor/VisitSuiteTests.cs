using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor
{
    [TestClass]
    public class VisitSuiteTests : BaseVisitClass<Python3Parser.SuiteContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitSuite(contextMock.Object);
        }

        [TestMethod]
        public void Visit_SimpleStmt_Test()
        {
            // Arrange
            var innerMock = GetMockRule<Python3Parser.Simple_stmtContext>();

            Statement suitStmt = ctorMock.SetupStatementMock(o
                => o.VisitSimple_stmt(innerMock.Object));

            contextMock.SetupChildren(
                innerMock.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.AreSame(suitStmt, result);

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Invalid_SimpleStmtExtraGunk_Test()
        {
            // Arrange
            var innerMock = GetMockRule<Python3Parser.Simple_stmtContext>();
            ITerminalNode unexpected = GetTerminal(Python3Parser.IF);

            contextMock.SetupChildren(
                unexpected,
                innerMock.Object
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex,
                contextMock, unexpected);

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_StmtSingle_Test()
        {
            // Arrange
            var innerMock = GetMockRule<Python3Parser.StmtContext>();

            Statement stmt = ctorMock.SetupStatementMock(o
                => o.VisitStmt(innerMock.Object));

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.NEWLINE),
                GetTerminal(Python3Parser.INDENT),
                innerMock.Object,
                GetTerminal(Python3Parser.DEDENT)
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.AreSame(stmt, result);

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_StmtMultiple_Test()
        {
            // Arrange
            var innerMock = GetMockRule<Python3Parser.StmtContext>();
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            Statement stmt = ctorMock.SetupStatementMock(o
                => o.VisitStmt(innerMock.Object));

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.NEWLINE),
                GetTerminal(Python3Parser.INDENT),
                innerMock.Object,
                innerMock.Object,
                innerMock.Object,
                GetTerminal(Python3Parser.DEDENT)
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.That.IsStatementListContaining(result,
                stmt,
                stmt,
                stmt);

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_StmtMultipleNested_Test()
        {
            // Arrange
            var innerMock = GetMockRule<Python3Parser.StmtContext>();

            var innerWithNestedMock = GetMockRule<Python3Parser.StmtContext>();

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            Statement stmt = ctorMock.SetupStatementMock(o
                => o.VisitStmt(innerMock.Object));

            Statement nestedStmt = GetStatementMock();
            
            var stmtList = new StatementList(SourceReference.ClrSource, new[]
                {
                    nestedStmt, nestedStmt
                }
            );

            ctorMock.Setup(o => o.VisitStmt(innerWithNestedMock.Object))
                .Returns(stmtList).Verifiable();

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.NEWLINE),
                GetTerminal(Python3Parser.INDENT),
                innerMock.Object,
                innerWithNestedMock.Object,
                innerMock.Object,
                GetTerminal(Python3Parser.DEDENT)
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.That.IsStatementListContaining(result,
                stmt,
                nestedStmt,
                nestedStmt,
                stmt);

            innerMock.Verify();
            innerWithNestedMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Invalid_StmtWithoutStartTokens_Test()
        {
            // Arrange
            var innerMock = GetMockRule<Python3Parser.StmtContext>();
            innerMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                innerMock.Object,
                GetTerminal(Python3Parser.DEDENT)
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex,
                startTokenMock, stopTokenMock, contextMock, innerMock.Object);

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Invalid_StmtWithWrongTokens_Test()
        {
            // Arrange
            var innerMock = GetMockRule<Python3Parser.StmtContext>();

            ITerminalNode unexpected = GetTerminal(Python3Parser.NOT);
            contextMock.SetupChildren(
                unexpected,
                GetTerminal(Python3Parser.AS),
                innerMock.Object,
                GetTerminal(Python3Parser.DEDENT)
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex,
                contextMock, unexpected);

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Invalid_StmtWithoutDedentTokens_Test()
        {
            // Arrange
            var innerMock = GetMockRule<Python3Parser.StmtContext>();
            innerMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.NEWLINE),
                GetTerminal(Python3Parser.INDENT),
                innerMock.Object
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex,
                startTokenMock, stopTokenMock, contextMock, innerMock.Object);

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}