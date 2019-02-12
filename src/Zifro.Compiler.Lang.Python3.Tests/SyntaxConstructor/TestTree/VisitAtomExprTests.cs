﻿using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor.TestTree
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
            var innerMock = GetInnerMock();
            var trailerMock = GetMockRule<Python3Parser.TrailerContext>();
            trailerMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                innerMock.Object,
                trailerMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock);
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
            var innerMock = GetInnerMock();
            var trailerMock = GetMockRule<Python3Parser.TrailerContext>();
            trailerMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            var extraTrailerMock = GetMockRule<Python3Parser.TrailerContext>();

            contextMock.SetupChildren(
                innerMock.Object,
                trailerMock.Object,
                extraTrailerMock.Object,
                extraTrailerMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock);
            contextMock.VerifyLoopedChildren(2);

            innerMock.Verify();
            trailerMock.Verify();
            extraTrailerMock.Verify();
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