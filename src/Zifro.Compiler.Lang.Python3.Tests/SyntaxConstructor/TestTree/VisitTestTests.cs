using System;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;

// ReSharper disable ConvertToLocalFunction
// ReSharper disable SuggestVarOrType_Elsewhere
// ReSharper disable SuggestVarOrType_SimpleTypes

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitTestTests : BaseVisitClass<Python3Parser.TestContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitTest(contextMock.Object);
        }

        [TestMethod]
        public void Visit_SingleOr_Test()
        {
            // Arrange
            var orTestMock = GetMockRule<Python3Parser.Or_testContext>();
            var expected = GetExpressionMock();

            contextMock.SetupChildren(
                orTestMock.Object
            );

            ctorMock.Setup(o => o.VisitOr_test(orTestMock.Object))
                .Returns(expected).Verifiable();

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.AreSame(expected, result);
            contextMock.VerifyLoopedChildren(1);

            orTestMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_SingleLambda_Test()
        {
            // Arrange
            var lambdaMock = GetMockRule<Python3Parser.LambdefContext>();

            lambdaMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                lambdaMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock, "lambda");
            contextMock.VerifyLoopedChildren(1);

            lambdaMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_SingleOrWithInlineIf_Test()
        {
            // Arrange
            var orTestMock = GetMockRule<Python3Parser.Or_testContext>();
            var testMock = GetMockRule<Python3Parser.TestContext>();
            
            ITerminalNode ifNode = GetTerminal(Python3Parser.IF);

            contextMock.SetupChildren(
                orTestMock.Object,
                ifNode,
                orTestMock.Object,
                GetTerminal(Python3Parser.ELSE),
                testMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, ifNode, "if");
            // Suppose error directly on IF token
            contextMock.VerifyLoopedChildren(2);

            orTestMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidInlinedIfMissingTokens_Test()
        {
            // Arrange
            var orTestMock = GetMockRule<Python3Parser.Or_testContext>();

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                orTestMock.Object,
                GetTerminal(Python3Parser.IF)
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);
            // Suppose error directly on IF token and count not enough
            contextMock.VerifyLoopedChildren(2);

            orTestMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidInlinedIfInvalidTokens_Test()
        {
            // Arrange
            var orTestMock = GetMockRule<Python3Parser.Or_testContext>();
            var unexpectedMock = GetMockRule<Python3Parser.File_inputContext>();

            unexpectedMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                orTestMock.Object,
                GetTerminal(Python3Parser.IF),
                unexpectedMock.Object,
                GetTerminal(Python3Parser.ARROW),
                GetTerminal(Python3Parser.ASYNC)
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, unexpectedMock.Object);
            // Suppose error directly on IF token and count not enough
            contextMock.VerifyLoopedChildren(2);

            orTestMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidToken_Test()
        {
            // Arrange
            ITerminalNode unexpectedNode = GetTerminal(Python3Parser.ASYNC);
            contextMock.SetupChildren(
                unexpectedNode
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpectedNode);
            contextMock.VerifyLoopedChildren(1);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidRule_Test()
        {
            // Arrange
            var unexpectedRule = GetMockRule<Python3Parser.File_inputContext>();

            unexpectedRule.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                unexpectedRule.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, unexpectedRule.Object);
            contextMock.VerifyLoopedChildren(1);

            unexpectedRule.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_NoChildren_Test()
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren();

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);

            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}