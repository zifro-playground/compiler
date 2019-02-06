using System;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Resources;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Resources;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Statements;

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor
{
    [TestClass]
    public class VisitTests
    {
        // ReSharper disable InconsistentNaming
        protected Mock<Grammar.SyntaxConstructor> ctorMock;
        protected Grammar.SyntaxConstructor ctor;
        protected Mock<IToken> tokenMock;
        // ReSharper restore InconsistentNaming

        protected static Mock<T> GetMockRule<T>() where T : ParserRuleContext
        {
            return new Mock<T>(ParserRuleContext.EmptyContext, 0);
        }

        protected Statement GetStatementMock()
        {
            return new Mock<Statement>(MockBehavior.Strict, SourceReference.ClrSource, string.Empty).Object;
        }

        protected Statement GetAssignmentMock()
        {
            return new Mock<StatementAssignment>(MockBehavior.Strict, SourceReference.ClrSource, string.Empty).Object;
        }

        protected StatementList GetStatementList(int count)
        {
            return new StatementList(SourceReference.ClrSource,
                new byte[count].Select(_ => GetStatementMock()).ToArray());
        }

        [TestInitialize]
        public void TestInitialize()
        {
            ctorMock = new Mock<Grammar.SyntaxConstructor>
            {
                CallBase = true
            };
            ctor = ctorMock.Object;

            tokenMock = new Mock<IToken>();
            tokenMock.SetupGet(o => o.Line).Returns(1);
            tokenMock.SetupGet(o => o.Column).Returns(1);
        }

        [TestMethod]
        public void FileStmt_Visit_FilledStatementList_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.File_inputContext>();
            contextMock.SetupForSourceReference(tokenMock);

            var stmtMock = GetMockRule<Python3Parser.StmtContext>();
            ctorMock.Setup(o => o.VisitStmt(stmtMock.Object))
                .Returns(GetStatementMock());

            contextMock.SetupChildren(
                stmtMock.Object, stmtMock.Object, stmtMock.Object
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
        public void FileStmt_Visit_EmptyStatementList_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.File_inputContext>();
            contextMock.SetupForSourceReference(tokenMock);
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
            contextMock.SetupForSourceReference(tokenMock);

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
            contextMock.SetupForSourceReference(tokenMock);
            contextMock.SetupChildren();

            Action action = delegate { ctor.VisitStmt(contextMock.Object); };

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(action);
            Assert.That.ErrorExpectedChildFormatArgs(ex, tokenMock, tokenMock, contextMock);

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
            contextMock.SetupForSourceReference(tokenMock);
            var innerContextMock = GetMockRule<Python3Parser.Del_stmtContext>();
            contextMock.SetupChildren(innerContextMock.Object);

            // TODO: Add this functionality
            Action action = delegate { ctor.VisitSmall_stmt(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(action);
            Assert.That.ErrorNotYetImplFormatArgs(ex, tokenMock, tokenMock, contextMock);
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SmallStmt_Visit_PassStmt_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();
            contextMock.SetupForSourceReference(tokenMock);
            var innerContextMock = GetMockRule<Python3Parser.Pass_stmtContext>();
            contextMock.SetupChildren(innerContextMock.Object);

            // TODO: Add this functionality
            Action action = delegate { ctor.VisitSmall_stmt(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(action);
            Assert.That.ErrorNotYetImplFormatArgs(ex, tokenMock, tokenMock, contextMock);
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SmallStmt_Visit_FlowStmt_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();
            contextMock.SetupForSourceReference(tokenMock);
            var innerContextMock = GetMockRule<Python3Parser.Flow_stmtContext>();
            contextMock.SetupChildren(innerContextMock.Object);

            // TODO: Add this functionality
            Action action = delegate { ctor.VisitSmall_stmt(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(action);
            Assert.That.ErrorNotYetImplFormatArgs(ex, tokenMock, tokenMock, contextMock);
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SmallStmt_Visit_ImportStmt_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();
            contextMock.SetupForSourceReference(tokenMock);
            var innerContextMock = GetMockRule<Python3Parser.Import_stmtContext>();
            contextMock.SetupChildren(innerContextMock.Object);

            // TODO: Add this functionality
            Action action = delegate { ctor.VisitSmall_stmt(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(action);
            Assert.That.ErrorNotYetImplFormatArgs(ex, tokenMock, tokenMock, contextMock);
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SmallStmt_Visit_GlobalStmt_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();
            contextMock.SetupForSourceReference(tokenMock);
            var innerContextMock = GetMockRule<Python3Parser.Global_stmtContext>();
            contextMock.SetupChildren(innerContextMock.Object);

            // TODO: Add this functionality
            Action action = delegate { ctor.VisitSmall_stmt(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(action);
            Assert.That.ErrorNotYetImplFormatArgs(ex, tokenMock, tokenMock, contextMock);
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SmallStmt_Visit_NonLocalStmt_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();
            contextMock.SetupForSourceReference(tokenMock);
            var innerContextMock = GetMockRule<Python3Parser.Nonlocal_stmtContext>();
            contextMock.SetupChildren(innerContextMock.Object);

            // TODO: Add this functionality
            Action action = delegate { ctor.VisitSmall_stmt(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(action);
            Assert.That.ErrorNotYetImplFormatArgs(ex, tokenMock, tokenMock, contextMock);
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SmallStmt_Visit_AssertStmt_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();
            contextMock.SetupForSourceReference(tokenMock);
            var innerContextMock = GetMockRule<Python3Parser.Assert_stmtContext>();
            contextMock.SetupChildren(innerContextMock.Object);

            // TODO: Add this functionality
            Action action = delegate { ctor.VisitSmall_stmt(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(action);
            Assert.That.ErrorNotYetImplFormatArgs(ex, tokenMock, tokenMock, contextMock);
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SmallStmt_Visit_NoChildren_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();
            contextMock.SetupForSourceReference(tokenMock);
            contextMock.SetupChildren();

            Action action = delegate { ctor.VisitSmall_stmt(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(action);
            Assert.That.ErrorExpectedChildFormatArgs(ex, tokenMock, tokenMock, contextMock);
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void SmallStmt_Visit_InvalidChild_Test()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Small_stmtContext>();
            contextMock.SetupForSourceReference(tokenMock);
            var innerContextMock = GetMockRule<Python3Parser.File_inputContext>();
            contextMock.SetupChildren(innerContextMock.Object);

            Action action = delegate { ctor.VisitSmall_stmt(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(action);
            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, tokenMock, tokenMock, contextMock, innerContextMock);

            contextMock.Verify();
            ctorMock.Verify();
        }

        #endregion
    }
}