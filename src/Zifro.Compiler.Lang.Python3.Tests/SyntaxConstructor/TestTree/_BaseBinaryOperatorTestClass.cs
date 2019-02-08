using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language.Flow;
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
    public abstract class BaseBinaryOperatorTestClass<TContext, TInnerContext, TOperator>
        : BaseVisitTestClass<TContext, TInnerContext>
        where TContext : ParserRuleContext
        where TInnerContext : ParserRuleContext
        where TOperator : BinaryOperator
    {
        public override void SetupForInnerMock(Mock<TInnerContext> innerMock, SyntaxNode returnValue)
        {
            RawSetupForInnerMock(innerMock)
                .Returns(returnValue).Verifiable();
        }

        public abstract ISetup<Grammar.SyntaxConstructor, SyntaxNode> RawSetupForInnerMock(
            Mock<TInnerContext> innerMock);

        [TestMethod]
        public virtual void Visit_SingleAnd_Test()
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
        public virtual void Visit_MultipleRuleSingleToken_Test()
        {
            // Arrange
            var expected = GetExpressionMock();
            var innerRuleMock = GetInnerMockWithSetup(expected);

            contextMock.SetupChildren(
                innerRuleMock.Object,
                GetTerminalForThisClass(),
                innerRuleMock.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.That.IsBinaryOperator<TOperator>(expected, expected, result);
            contextMock.VerifyLoopedChildren(3);

            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public virtual void Visit_InvalidMissingRhs_Test()
        {
            // Arrange
            var innerRuleMock = GetInnerMockWithSetup(GetExpressionMock());

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                innerRuleMock.Object,
                GetTerminalForThisClass()
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
        public virtual void Visit_MultipleRuleInvalidToken_Test()
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
        public virtual void Visit_MultipleRuleExcessNode_Test()
        {
            // Arrange
            var innerRuleMock = GetInnerMockWithSetup(GetExpressionMock());

            ITerminalNode unexpectedNode = GetTerminalForThisClass();

            contextMock.SetupChildren(
                innerRuleMock.Object,
                GetTerminalForThisClass(),
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
        public virtual void Visit_MultipleOpsOrder_Test()
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
                GetTerminalForThisClass(),
                innerRuleMock2.Object,
                GetTerminalForThisClass(),
                innerRuleMock3.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            // Expect order ((1 op 2) op 3)
            // so it compiles left-to-right
            var lhs = Assert.That.IsBinaryOperatorGetLhs<TOperator>(
                expectedRhs: expected3, result);

            Assert.That.IsBinaryOperator<TOperator>(
                expectedLhs: expected1,
                expectedRhs: expected2, lhs);

            contextMock.VerifyLoopedChildren(5);

            innerRuleMock1.Verify();
            innerRuleMock2.Verify();
            innerRuleMock3.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        #region Inherited tests

        // This is here due to some testing tools only look
        // 1 in depth in hierarchy for tests

        [TestMethod]
        public override void Visit_InvalidRule_Test()
        {
            base.Visit_InvalidRule_Test();
        }

        [TestMethod]
        public override void Visit_InvalidToken_Test()
        {
            base.Visit_InvalidToken_Test();
        }

        [TestMethod]
        public override void Visit_NoChildren_Test()
        {
            base.Visit_NoChildren_Test();
        }

        #endregion
    }
}