using System;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Statements;

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor
{
    public abstract class BaseVisitClass<TContextType> : BaseVisitClass where TContextType : ParserRuleContext
    {
        // ReSharper disable InconsistentNaming
        protected Mock<TContextType> contextMock;
        // ReSharper restore InconsistentNaming

        [TestInitialize]
        public override void TestInitialize()
        {
            contextMock = GetMockRule<TContextType>();

            base.TestInitialize();
        }

        public abstract SyntaxNode VisitContext();


        [TestMethod]
        public virtual void Visit_InvalidToken_Test()
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
        public virtual void Visit_InvalidRule_Test()
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
        public virtual void Visit_NoChildren_Test()
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

    public class BaseVisitClass
    {
        // ReSharper disable InconsistentNaming
        protected Mock<Grammar.SyntaxConstructor> ctorMock;
        protected Grammar.SyntaxConstructor ctor;
        protected Mock<IToken> startTokenMock;

        protected Mock<IToken> stopTokenMock;
        // ReSharper restore InconsistentNaming

        public static Mock<T> GetMockRule<T>() where T : ParserRuleContext
        {
            return new Mock<T>(ParserRuleContext.EmptyContext, 0) { CallBase = true };
        }

        public static ITerminalNode GetTerminal(int symbol)
        {
            var mock = new Mock<ITerminalNode>();
            mock.Setup(o => o.Symbol).Returns(GetSymbol(symbol));
            return mock.Object;
        }

        public static ITerminalNode GetTerminal(int symbol, string text)
        {
            var mock = new Mock<ITerminalNode>();
            mock.Setup(o => o.Symbol).Returns(GetSymbol(symbol, text));
            return mock.Object;
        }

        public static IToken GetSymbol(int symbol)
        {
            return GetSymbol(symbol, Python3Parser.DefaultVocabulary
                .GetLiteralName(symbol)?.Trim('\''));
        }

        public static IToken GetSymbol(int symbol, string text)
        {
            var mock = new Mock<IToken>(MockBehavior.Strict);
            mock.SetupGet(o => o.Type).Returns(symbol);
            mock.SetupGet(o => o.Text).Returns(text);
            mock.SetupGet(o => o.Line).Returns(5);
            mock.SetupGet(o => o.Column).Returns(6);
            return mock.Object;
        }

        public static Statement GetStatementMock()
        {
            return new Mock<Statement>(MockBehavior.Strict, SourceReference.ClrSource).Object;
        }

        public static Statement GetAssignmentMock()
        {
            return new Mock<Assignment>(MockBehavior.Strict, SourceReference.ClrSource,
                GetExpressionMock(), GetExpressionMock()).Object;
        }

        public static StatementList GetStatementList(int count)
        {
            return new StatementList(SourceReference.ClrSource,
                new byte[count].Select(_ => GetStatementMock()).ToArray());
        }

        public static ExpressionNode GetExpressionMock(SourceReference source)
        {
            return new Mock<ExpressionNode>(MockBehavior.Strict, source).Object;
        }

        public static ExpressionNode GetExpressionMock()
        {
            return GetExpressionMock(SourceReference.ClrSource);
        }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            ctorMock = new Mock<Grammar.SyntaxConstructor>
            {
                CallBase = true
            };
            ctor = ctorMock.Object;

            startTokenMock = new Mock<IToken>();
            startTokenMock.SetupGet(o => o.Line).Returns(1);
            startTokenMock.SetupGet(o => o.Column).Returns(2);
            stopTokenMock = new Mock<IToken>();
            stopTokenMock.SetupGet(o => o.Line).Returns(3);
            stopTokenMock.SetupGet(o => o.Column).Returns(4);
        }

    }
}