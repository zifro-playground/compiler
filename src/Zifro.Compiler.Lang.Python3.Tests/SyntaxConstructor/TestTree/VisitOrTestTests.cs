using System;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Operators;

// ReSharper disable ConvertToLocalFunction
// ReSharper disable SuggestVarOrType_Elsewhere
// ReSharper disable SuggestVarOrType_SimpleTypes

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitOrTestTests
        : BaseVisitTestClass<Python3Parser.Or_testContext, Python3Parser.And_testContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitOr_test(contextMock.Object);
        }

        public override void SetupForInnerMock(Mock<Python3Parser.And_testContext> innerMock, SyntaxNode returnValue)
        {
            ctorMock.Setup(o => o.VisitAnd_test(innerMock.Object))
                .Returns(returnValue).Verifiable();
        }

        [TestMethod]
        public void Visit_SingleAnd_Test()
        {
            // Arrange
            var expected = GetExpressionMock();
            var innerRuleMock = GetInnerMockWithSetup(expected);

            contextMock.SetupChildren(
                innerRuleMock.Object
            );

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
        public void Visit_MultipleRuleSingleToken_Test()
        {
            // Arrange
            var expected = GetExpressionMock();
            var innerRuleMock = GetInnerMockWithSetup(expected);

            contextMock.SetupChildren(
                innerRuleMock.Object,
                GetTerminal(Python3Parser.OR),
                innerRuleMock.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.That.IsBinaryOperator<OperatorAnd>(expected, expected, result);
            contextMock.VerifyLoopedChildren(3);

            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidMissingRhs_Test()
        {
            // Arrange
            var innerRuleMock = GetInnerMockWithSetup(GetExpressionMock());

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                innerRuleMock.Object,
                GetTerminal(Python3Parser.OR)
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);
            contextMock.VerifyLoopedChildren(2);

            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_MultipleRuleInvalidToken_Test()
        {
            // Arrange
            var innerRuleMock = GetInnerMockWithSetup(GetExpressionMock());

            ITerminalNode unexpectedNode = GetTerminal(Python3Parser.ASYNC);

            contextMock.SetupChildren(
                innerRuleMock.Object,
                unexpectedNode,
                innerRuleMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpectedNode);
            // Suppose error directly on token
            contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_MultipleRuleExcessNode_Test()
        {
            // Arrange
            var innerRuleMock = GetInnerMockWithSetup(GetExpressionMock());

            ITerminalNode unexpectedNode = GetTerminal(Python3Parser.OR);

            contextMock.SetupChildren(
                innerRuleMock.Object,
                GetTerminal(Python3Parser.OR),
                innerRuleMock.Object,
                unexpectedNode
            );

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);
            contextMock.VerifyLoopedChildren(4);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_MultipleOpsOrder_Test()
        {
            // Arrange
            var expected1 = GetExpressionMock();
            var expected2 = GetExpressionMock();
            var expected3 = GetExpressionMock();

            var innerRuleMock1 = GetInnerMockWithSetup(expected1);
            var innerRuleMock2 = GetInnerMockWithSetup(expected2);
            var innerRuleMock3 = GetInnerMockWithSetup(expected3);

            contextMock.SetupChildren(
                innerRuleMock1.Object,
                GetTerminal(Python3Parser.OR),
                innerRuleMock2.Object,
                GetTerminal(Python3Parser.OR),
                innerRuleMock3.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            // Expect order ((1 or 2) or 3)
            var lhs = Assert.That.IsBinaryOperatorGetLhs<OperatorAnd>(
                expectedRhs: expected3, result);

            Assert.That.IsBinaryOperator<OperatorAnd>(
                expectedLhs: expected1,
                expectedRhs: expected2, lhs);

            contextMock.VerifyLoopedChildren(3);

            innerRuleMock1.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}