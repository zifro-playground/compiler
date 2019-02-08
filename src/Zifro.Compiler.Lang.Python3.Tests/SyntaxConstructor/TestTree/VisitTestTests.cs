using System;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
    public class VisitTestTests
        : BaseVisitTestClass<Python3Parser.TestContext, Python3Parser.Or_testContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitTest(contextMock.Object);
        }

        public override void SetupForInnerMock(Mock<Python3Parser.Or_testContext> innerMock, SyntaxNode returnValue)
        {
            ctorMock.Setup(o => o.VisitOr_test(innerMock.Object))
                .Returns(returnValue).Verifiable();
        }

        [TestMethod]
        public void Visit_SingleOr_Test()
        {
            // Arrange
            var innerRuleMock = GetInnerMock();
            var expected = GetExpressionMock();

            contextMock.SetupChildren(
                innerRuleMock.Object
            );

            SetupForInnerMock(innerRuleMock, expected);

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.AreSame(expected, result);
            contextMock.VerifyLoopedChildren(1);

            innerRuleMock.Verify();
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
            var innerRuleMock = GetInnerMock();
            var testMock = GetMockRule<Python3Parser.TestContext>();
            
            ITerminalNode ifNode = GetTerminal(Python3Parser.IF);

            contextMock.SetupChildren(
                innerRuleMock.Object,
                ifNode,
                innerRuleMock.Object,
                GetTerminal(Python3Parser.ELSE),
                testMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, ifNode, "if");
            // Suppose error directly on IF token
            contextMock.VerifyLoopedChildren(2);

            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidInlinedIfMissingTokens_Test()
        {
            // Arrange
            var innerRuleMock = GetInnerMock();

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                innerRuleMock.Object,
                GetTerminal(Python3Parser.IF)
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);
            // Suppose error directly on IF token and count not enough
            contextMock.VerifyLoopedChildren(2);

            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidInlinedIfInvalidTokens_Test()
        {
            // Arrange
            var innerRuleMock = GetInnerMock();
            var unexpectedMock = GetMockRule<Python3Parser.File_inputContext>();

            unexpectedMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                innerRuleMock.Object,
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

            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}