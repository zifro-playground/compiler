using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language.Flow;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    public abstract class BaseVisitTestClass<TContext, TInnerContext> 
        : BaseVisitClass<TContext> 
        where TContext : ParserRuleContext
        where TInnerContext : ParserRuleContext
    {
        public virtual Mock<TInnerContext> GetInnerMock()
        {
            return GetMockRule<TInnerContext>();
        }

        public virtual Mock<TInnerContext> GetInnerMockWithSetup(SyntaxNode returnValue)
        {
            Mock<TInnerContext> mock = GetMockRule<TInnerContext>();
            SetupForInnerMock(mock, returnValue);
            return mock;
        }

        public abstract void SetupForInnerMock(
            Mock<TInnerContext> innerMock,
            SyntaxNode returnValue);

        [TestMethod]
        public void Visit_InvalidToken_Test()
        {
            // Arrange
            ITerminalNode unexpectedNode = GetTerminal(Python3Parser.ASYNC);
            contextMock.SetupChildren(
                unexpectedNode
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext,
                message: $"expected throw for context `{Python3Parser.ruleNames[contextMock.Object.RuleIndex]}`");

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
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext,
                message: $"expected throw for context `{Python3Parser.ruleNames[contextMock.Object.RuleIndex]}`");

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
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext,
                message: $"expected throw for context `{Python3Parser.ruleNames[contextMock.Object.RuleIndex]}`");

            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);

            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}