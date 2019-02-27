using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Syntax;

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitAtomExprTests : BaseVisitTestClass<
        Python3Parser.Atom_exprContext,
        Python3Parser.AtomContext
    >
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitAtom_expr(contextMock.Object);
        }

        public override void SetupForInnerMock(Mock<Python3Parser.AtomContext> innerMock, SyntaxNode returnValue)
        {
            ctorMock.Setup(o => o.VisitAtom(innerMock.Object))
                .Returns(returnValue).Verifiable();
        }

        public override ITerminalNode GetTerminalForThisClass()
        {
            return GetTerminal(Python3Parser.AWAIT);
        }

        protected void SetupForTrailerMock(
            Mock<Python3Parser.TrailerContext> trailerMock,
            SyntaxNode returnValue)
        {
            ctorMock.Setup(o => o.VisitTrailer(trailerMock.Object))
                .Returns(returnValue).Verifiable();
        }

        protected Mock<Python3Parser.TrailerContext> GetTrailerMockWithSetup(
            SyntaxNode returnValue)
        {
            var trailerMock = GetMockRule<Python3Parser.TrailerContext>();
            SetupForTrailerMock(trailerMock, returnValue);
            return trailerMock;
        }

        [TestMethod]
        public void Visit_AwaitAtom_Test()
        {
            // Arrange
            var awaitRule = GetTerminalForThisClass();
            var innerMock = GetInnerMock();

            contextMock.SetupChildren(
                awaitRule,
                innerMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, awaitRule, "await");
            // Should throw on 1st
            contextMock.VerifyLoopedChildren(1);

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_Atom_Test()
        {
            // Arrange
            ExpressionNode expr = GetExpressionMock();
            var innerMock = GetInnerMockWithSetup(expr);
            innerMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                innerMock.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.AreSame(expr, result);
            contextMock.VerifyLoopedChildren(1);

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_AtomSingleTrailer_Test()
        {
            // Arrange
            var exprNode = GetExpressionMock();
            var innerMock = GetInnerMockWithSetup(exprNode);
            innerMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            var argsNode = GetArgumentsListMock();
            var trailerMock = GetTrailerMockWithSetup(argsNode);
            trailerMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                innerMock.Object,
                trailerMock.Object
            );

            // Act
            SyntaxNode result = VisitContext();
            
            // Assert
            ExpressionNode result2 = Assert.That.IsFunctionCall(result, argsNode);
            Assert.AreSame(exprNode, result2);

            contextMock.VerifyLoopedChildren(2);

            innerMock.Verify();
            trailerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_AtomMultipleTrailer_Test()
        {
            // Arrange
            var exprNode = GetExpressionMock();
            var innerMock = GetInnerMockWithSetup(exprNode);
            innerMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            var argsNode1 = GetArgumentsListMock();
            var trailerMock1 = GetTrailerMockWithSetup(argsNode1);
            trailerMock1.SetupForSourceReference(startTokenMock, stopTokenMock);

            var argsNode2 = GetArgumentsListMock();
            var trailerMock2 = GetTrailerMockWithSetup(argsNode2);
            trailerMock2.SetupForSourceReference(startTokenMock, stopTokenMock);

            var argsNode3 = GetArgumentsListMock();
            var trailerMock3 = GetTrailerMockWithSetup(argsNode3);
            trailerMock3.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                innerMock.Object,
                trailerMock1.Object,
                trailerMock2.Object,
                trailerMock3.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            // Expect last-in, first-out
            ExpressionNode result2 = Assert.That.IsFunctionCall(result, argsNode3);
            ExpressionNode result3 = Assert.That.IsFunctionCall(result2, argsNode2);
            ExpressionNode result4 = Assert.That.IsFunctionCall(result3, argsNode1);
            Assert.AreSame(exprNode, result4);

            contextMock.VerifyLoopedChildren(4);

            innerMock.Verify();
            trailerMock1.Verify();
            contextMock.Verify();
            ctorMock.Verify();

        }

        [TestMethod]
        public void Visit_AwaitAtomSingleTrailer_Test()
        {
            // Arrange
            var awaitRule = GetTerminalForThisClass();
            var innerMock = GetInnerMock();
            var trailerMock = GetMockRule<Python3Parser.TrailerContext>();

            contextMock.SetupChildren(
                awaitRule,
                innerMock.Object,
                trailerMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, awaitRule, "await");
            contextMock.VerifyLoopedChildren(1);

            innerMock.Verify();
            trailerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_OnlyTrailer_Test()
        {
            // Arrange
            var trailerMock = GetMockRule<Python3Parser.TrailerContext>();
            trailerMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                trailerMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, trailerMock.Object);
            contextMock.VerifyLoopedChildren(1);

            trailerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_OnlyAwait_Test()
        {
            // Arrange
            var awaitRule = GetTerminalForThisClass();

            contextMock.SetupChildren(
                awaitRule
            );

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);
            contextMock.VerifyLoopedChildren(1);

            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}