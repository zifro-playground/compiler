using System;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Resources;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Statements;
// ReSharper disable ConvertToLocalFunction
// ReSharper disable SuggestVarOrType_Elsewhere

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor
{
    [TestClass]
    public class VisitStatementTests : BaseVisitClass
    {
        #region FileStmt

        [TestMethod]
        public void FileStmt_Visit_FilledStatementList_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.File_inputContext>();
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            var stmtMock = GetMockRule<Python3Parser.StmtContext>();
            ctorMock.Setup(o => o.VisitStmt(stmtMock.Object))
                .Returns(GetStatementMock());

            contextMock.SetupChildren(
                stmtMock.Object,
                stmtMock.Object,
                stmtMock.Object
            );

            // Act
            SyntaxNode result = ctor.VisitFile_input(contextMock.Object);

            // Assert
            ctorMock.Verify(o => o.VisitChildren(It.IsAny<IRuleNode>()), Times.Never);

            Assert.That.IsStatementListWithCount(3, result);
            contextMock.VerifyLoopedChildren(3);

            ctorMock.Verify(o => o.VisitStmt(stmtMock.Object), Times.Exactly(3));

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void FileStmt_Visit_FilledStatementListAndNewlines_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.File_inputContext>();
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            var stmtMock = GetMockRule<Python3Parser.StmtContext>();
            ctorMock.Setup(o => o.VisitStmt(stmtMock.Object))
                .Returns(GetStatementMock());

            contextMock.SetupChildren(
                stmtMock.Object, GetTerminal(Python3Parser.NEWLINE),
                stmtMock.Object, GetTerminal(Python3Parser.NEWLINE),
                stmtMock.Object
            );

            // Act
            SyntaxNode result = ctor.VisitFile_input(contextMock.Object);

            // Assert
            ctorMock.Verify(o => o.VisitChildren(It.IsAny<IRuleNode>()), Times.Never);

            Assert.That.IsStatementListWithCount(3, result);
            contextMock.VerifyLoopedChildren(5);

            ctorMock.Verify(o => o.VisitStmt(stmtMock.Object), Times.Exactly(3));

            contextMock.Verify();
            ctorMock.Verify();
        }


        [TestMethod]
        public void FileStmt_Visit_FilledNewlines_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.File_inputContext>();
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            var stmtMock = GetMockRule<Python3Parser.StmtContext>();
            ctorMock.Setup(o => o.VisitStmt(stmtMock.Object))
                .Returns(GetStatementMock());

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.NEWLINE),
                GetTerminal(Python3Parser.NEWLINE),
                GetTerminal(Python3Parser.NEWLINE)
            );

            // Act
            SyntaxNode result = ctor.VisitFile_input(contextMock.Object);

            // Assert
            ctorMock.Verify(o => o.VisitChildren(It.IsAny<IRuleNode>()), Times.Never);

            Assert.That.IsStatementListWithCount(0, result);
            contextMock.VerifyLoopedChildren(3);

            ctorMock.Verify(o => o.VisitStmt(stmtMock.Object), Times.Never);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void FileStmt_Visit_EmptyStatementList_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.File_inputContext>();
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren();

            // Act
            SyntaxNode result = ctor.VisitFile_input(contextMock.Object);

            // Assert
            ctorMock.Verify(o => o.VisitChildren(It.IsAny<IRuleNode>()), Times.Never);

            Assert.That.IsStatementListWithCount(0, result);
            contextMock.VerifyLoopedChildren(0);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void FileStmt_Visit_NestedStatementList_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.File_inputContext>();
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            var stmtMock = GetMockRule<Python3Parser.StmtContext>();
            ctorMock.Setup(o => o.VisitStmt(stmtMock.Object))
                .Returns(GetStatementMock());

            var stmtWithNestedMock = GetMockRule<Python3Parser.StmtContext>();
            ctorMock.Setup(o => o.VisitStmt(stmtWithNestedMock.Object))
                .Returns(GetStatementList(3));

            contextMock.SetupChildren(
                stmtMock.Object, stmtMock.Object, stmtWithNestedMock.Object
            );

            // Act
            SyntaxNode result = ctor.VisitFile_input(contextMock.Object);

            // Assert
            ctorMock.Verify(o => o.VisitChildren(It.IsAny<IRuleNode>()), Times.Never);

            Assert.That.IsStatementListWithCount(5, result);
            contextMock.VerifyLoopedChildren(3);

            ctorMock.Verify(o => o.VisitStmt(stmtMock.Object), Times.Exactly(2));
            ctorMock.Verify(o => o.VisitStmt(stmtWithNestedMock.Object), Times.Exactly(1));

            contextMock.Verify();
            ctorMock.Verify();
        }

        #endregion

        #region Stmt

        [TestMethod]
        public void Stmt_Visit_SimpleStmt_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.StmtContext>();
            var simpleStmtMock = GetMockRule<Python3Parser.Simple_stmtContext>();

            ctorMock.Setup(o => o.VisitSimple_stmt(simpleStmtMock.Object))
                .Returns(GetStatementList(2)).Verifiable();

            contextMock.SetupChildren(
                simpleStmtMock.Object
            );

            // Act
            SyntaxNode result = ctor.VisitStmt(contextMock.Object);

            // Assert
            ctorMock.Verify(o => o.VisitChildren(It.IsAny<IRuleNode>()), Times.Never);

            Assert.That.IsStatementListWithCount(2, result);
            contextMock.VerifyLoopedChildren(1);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Stmt_Visit_CompoundStmt_Test()
        {

            // Arrange
            var contextMock = GetMockRule<Python3Parser.StmtContext>();
            var compoundStmtMock = GetMockRule<Python3Parser.Compound_stmtContext>();
            SyntaxNode expectedResult = GetStatementMock();

            ctorMock.Setup(o => o.VisitCompound_stmt(compoundStmtMock.Object))
                .Returns(expectedResult).Verifiable();

            contextMock.SetupChildren(
                compoundStmtMock.Object
            );

            // Act
            SyntaxNode result = ctor.VisitStmt(contextMock.Object);

            // Assert
            ctorMock.Verify(o => o.VisitChildren(It.IsAny<IRuleNode>()), Times.Never);

            Assert.AreSame(expectedResult, result);
            contextMock.VerifyLoopedChildren(1);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Stmt_Visit_NoChildren_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.StmtContext>();
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren();

            void Action() { ctor.VisitStmt(contextMock.Object); }

            // Act
            var ex = Assert.ThrowsException<SyntaxException>((Action) Action);
            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Stmt_Visit_TooManyChildren_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.StmtContext>();
            var compoundStmtMock = GetMockRule<Python3Parser.Compound_stmtContext>();
            SyntaxNode expectedResult = GetStatementMock();

            ctorMock.Setup(o => o.VisitCompound_stmt(compoundStmtMock.Object))
                .Returns(expectedResult).Verifiable();

            // Contains three, but should just ignore the excess as it's not part of the syntax.
            contextMock.SetupChildren(
                compoundStmtMock.Object, compoundStmtMock.Object, compoundStmtMock.Object
            );

            // Act
            SyntaxNode result = ctor.VisitStmt(contextMock.Object);

            // Assert
            ctorMock.Verify(o => o.VisitChildren(It.IsAny<IRuleNode>()), Times.Never);

            Assert.AreSame(expectedResult, result);
            contextMock.VerifyLoopedChildren(1);

            contextMock.Verify();
            ctorMock.Verify();
        }

        #endregion

        #region SimpleStmt

        [TestMethod]
        public void SimpleStmt_Visit_SingleSmall_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Simple_stmtContext>();
            var smallMock = GetMockRule<Python3Parser.Small_stmtContext>();
            Statement statement = GetStatementMock();

            ctorMock.Setup(o => o.VisitSmall_stmt(smallMock.Object))
                .Returns(statement).Verifiable();

            contextMock.SetupChildren(
                smallMock.Object, GetTerminal(Python3Parser.NEWLINE)
            );

            // Act
            SyntaxNode result = ctor.VisitSimple_stmt(contextMock.Object);

            // Assert
            ctorMock.Verify(o => o.VisitChildren(It.IsAny<IRuleNode>()), Times.Never);

            Assert.AreSame(statement, result);
            contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SimpleStmt_Visit_MultipleSmall_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Simple_stmtContext>();
            var smallMock = GetMockRule<Python3Parser.Small_stmtContext>();
            Statement statement = GetStatementMock();

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            ctorMock.Setup(o => o.VisitSmall_stmt(smallMock.Object))
                .Returns(statement).Verifiable();

            contextMock.SetupChildren(
                smallMock.Object, GetTerminal(Python3Parser.SEMI_COLON),
                smallMock.Object, GetTerminal(Python3Parser.SEMI_COLON),
                smallMock.Object, GetTerminal(Python3Parser.NEWLINE)
            );

            // Act
            SyntaxNode result = ctor.VisitSimple_stmt(contextMock.Object);

            // Assert
            ctorMock.Verify(o => o.VisitChildren(It.IsAny<IRuleNode>()), Times.Never);

            Assert.That.IsStatementListWithCount(3, result);
            contextMock.VerifyLoopedChildren(6);

            contextMock.Verify();
            ctorMock.Verify();
        }


        [TestMethod]
        public void SimpleStmt_Visit_MultipleSmallNoTokens_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Simple_stmtContext>();
            var smallMock = GetMockRule<Python3Parser.Small_stmtContext>();
            Statement statement = GetStatementMock();

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            ctorMock.Setup(o => o.VisitSmall_stmt(smallMock.Object))
                .Returns(statement).Verifiable();

            // Invalid according to syntax tree, but syntax constructor should handle it
            contextMock.SetupChildren(
                smallMock.Object,
                smallMock.Object,
                smallMock.Object
            );

            // Act
            SyntaxNode result = ctor.VisitSimple_stmt(contextMock.Object);

            // Assert
            ctorMock.Verify(o => o.VisitChildren(It.IsAny<IRuleNode>()), Times.Never);

            Assert.That.IsStatementListWithCount(3, result);
            contextMock.VerifyLoopedChildren(3);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SimpleStmt_Visit_NoChildren_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Simple_stmtContext>();

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren();

            void Action() { ctor.VisitSimple_stmt(contextMock.Object); }

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>((Action) Action);
            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);
            
            contextMock.Verify();
            ctorMock.Verify();
        }

        #endregion

        #region SmallStmt

        [TestMethod]
        public void SmallStmt_Visit_ExprStmt_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();
            var exprMock = GetMockRule<Python3Parser.Expr_stmtContext>();
            SyntaxNode expectedResult = GetAssignmentMock();

            ctorMock.Setup(o => o.VisitExpr_stmt(exprMock.Object))
                .Returns(expectedResult);

            contextMock.SetupChildren(
                exprMock.Object
            );

            // Act
            SyntaxNode result = ctor.VisitSmall_stmt(contextMock.Object);

            // Assert
            ctorMock.Verify(o => o.VisitChildren(It.IsAny<IRuleNode>()), Times.Never);

            Assert.AreSame(expectedResult, result);
            contextMock.VerifyLoopedChildren(1);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SmallStmt_Visit_DelStmt_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();
            var innerContextMock = GetMockRule<Python3Parser.Del_stmtContext>();
            innerContextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren(innerContextMock.Object);

            // TODO: Add this functionality
            void Action() { ctor.VisitSmall_stmt(contextMock.Object); }

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>((Action) Action);
            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock, "del");
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SmallStmt_Visit_PassStmt_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();
            var innerContextMock = GetMockRule<Python3Parser.Pass_stmtContext>();
            innerContextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren(innerContextMock.Object);

            // TODO: Add this functionality
            void Action() { ctor.VisitSmall_stmt(contextMock.Object); }

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>((Action) Action);
            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock, "pass");
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SmallStmt_Visit_FlowStmt_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();
            var innerContextMock = GetMockRule<Python3Parser.Flow_stmtContext>();
            innerContextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren(innerContextMock.Object);

            // TODO: Add this functionality
            void Action() { ctor.VisitSmall_stmt(contextMock.Object); }

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>((Action) Action);
            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock);
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SmallStmt_Visit_ImportStmt_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();
            var innerContextMock = GetMockRule<Python3Parser.Import_stmtContext>();
            innerContextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren(innerContextMock.Object);

            // TODO: Add this functionality
            void Action() { ctor.VisitSmall_stmt(contextMock.Object); }

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>((Action) Action);
            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock, "import");
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SmallStmt_Visit_GlobalStmt_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();
            var innerContextMock = GetMockRule<Python3Parser.Global_stmtContext>();
            innerContextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren(innerContextMock.Object);

            // TODO: Add this functionality
            void Action() { ctor.VisitSmall_stmt(contextMock.Object); }

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>((Action) Action);
            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock, "global");
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SmallStmt_Visit_NonLocalStmt_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();
            var innerContextMock = GetMockRule<Python3Parser.Nonlocal_stmtContext>();
            innerContextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren(innerContextMock.Object);

            // TODO: Add this functionality
            void Action() { ctor.VisitSmall_stmt(contextMock.Object); }

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>((Action) Action);
            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock, "nonlocal");
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SmallStmt_Visit_AssertStmt_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();
            var innerContextMock = GetMockRule<Python3Parser.Assert_stmtContext>();
            innerContextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren(innerContextMock.Object);

            // TODO: Add this functionality
            void Action() { ctor.VisitSmall_stmt(contextMock.Object); }

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>((Action) Action);
            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock, "assert");
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SmallStmt_Visit_NoChildren_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren();

            void Action() { ctor.VisitSmall_stmt(contextMock.Object); }

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>((Action) Action);
            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SmallStmt_Visit_InvalidChild_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();

            var innerContextMock = GetMockRule<Python3Parser.File_inputContext>();
            innerContextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren(innerContextMock.Object);

            void Action() { ctor.VisitSmall_stmt(contextMock.Object); }

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>((Action) Action);
            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, innerContextMock.Object);

            contextMock.Verify();
            ctorMock.Verify();
        }

        #endregion
    }
}