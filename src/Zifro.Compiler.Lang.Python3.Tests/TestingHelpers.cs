using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Resources;
using Zifro.Compiler.Lang.Python3.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Resources;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Operators;

namespace Zifro.Compiler.Lang.Python3.Tests
{
    public static class TestingHelpers
    {
        public static void SetupForSourceReference<T>(this Mock<T> context,
            Mock<IToken> startTokenMock, Mock<IToken> stopTokenMock)
            where T : ParserRuleContext
        {
            context.SetupGet(o => o.Start).Returns(startTokenMock.Object)
                .Verifiable($"Did not get start token from {typeof(T).Name}");
            context.SetupGet(o => o.Stop).Returns(stopTokenMock.Object)
                .Verifiable($"Did not get stop token from {typeof(T).Name}");
        }

        public static void SetupChildren<T>(this Mock<T> contextMock,
            params IParseTree[] children)
            where T : ParserRuleContext
        {
            contextMock.Object.children = children;
            contextMock.SetupGet(o => o.ChildCount)
                .Returns(children.Length);

            for (var i = 0; i < children.Length; i++)
            {
                int iCopy = i;
                contextMock.Setup(o => o.GetChild(iCopy))
                    .Returns(children[i]);
            }
        }

        public static void IsStatementListWithCount(this Assert assert, int expected, SyntaxNode resultNode)
        {
            Assert.IsInstanceOfType(resultNode, typeof(StatementList));
            Assert.AreEqual(expected, ((StatementList)resultNode).Statements.Count);
        }

        public static void IsUnaryOperator<TOperator>(this Assert assert,
            ExpressionNode expectedInner,
            SyntaxNode resultNode)
            where TOperator : UnaryOperator
        {
            Assert.IsInstanceOfType(resultNode, typeof(TOperator));
            var op = (TOperator)resultNode;
            Assert.AreSame(expectedInner, op.Operand);
        }

        public static void IsBinaryOperator<TOperator>(this Assert assert, 
            ExpressionNode expectedLhs, ExpressionNode expectedRhs,
            SyntaxNode resultNode)
            where TOperator : BinaryOperator
        {
            Assert.IsInstanceOfType(resultNode, typeof(TOperator));
            var op = (TOperator)resultNode;
            Assert.AreSame(expectedLhs, op.LeftOperand);
            Assert.AreSame(expectedRhs, op.RightOperand);
        }
        
        public static ExpressionNode IsBinaryOperatorGetLhs<TOperator>(this Assert assert,
            ExpressionNode expectedRhs,
            SyntaxNode resultNode)
            where TOperator : BinaryOperator
        {
            Assert.IsInstanceOfType(resultNode, typeof(TOperator));
            var op = (TOperator)resultNode;
            Assert.AreSame(expectedRhs, op.RightOperand);
            return op.LeftOperand;
        }

        public static void ErrorFormatArgsEqual(this Assert assert,
            InterpreterLocalizedException exception, string expectedLocalizedKey,
            params object[] expectedArgs)
        {
            Assert.AreEqual(expectedLocalizedKey, exception.LocalizeKey);

            Assert.AreEqual(expectedArgs.Length, exception.FormatArgs.Length, $"relevant key: {expectedLocalizedKey}");
            for (var i = 0; i < expectedArgs.Length; i++)
            {
                Assert.AreEqual(expectedArgs[i], exception.FormatArgs[i], $"index: {i}");
            }
        }

        public static void ErrorSyntaxFormatArgsEqual(this Assert assert,
            SyntaxException exception, string expectedLocalizedKey,
            IToken startToken, IToken stopToken,
            params object[] expectedExcessArgs)
        {
            object[] expected = (new object[]
            {
                startToken.Line, startToken.Column,
                stopToken.Line, stopToken.Column
            }).Concat(expectedExcessArgs).ToArray();

            assert.ErrorFormatArgsEqual(exception, expectedLocalizedKey, expectedArgs: expected);
        }

        public static void ErrorSyntaxFormatArgsEqual(this Assert assert,
            SyntaxException exception, string expectedLocalizedKey,
            ITerminalNode token,
            params object[] expectedExcessArgs)
        {
            object[] expected = (new object[]
            {
                token.Symbol.Line, token.Symbol.Column,
                token.Symbol.Line, token.Symbol.Column + token.Symbol.Text.Length - 1
            }).Concat(expectedExcessArgs).ToArray();

            assert.ErrorFormatArgsEqual(exception, expectedLocalizedKey, expectedArgs: expected);
        }

        public static void ErrorUnexpectedChildTypeFormatArgs<TContext, TChild>(this Assert assert,
            SyntaxException exception, Mock<IToken> startTokenMock, Mock<IToken> stopTokenMock,
            Mock<TContext> context, TChild child)
            where TContext : ParserRuleContext
            where TChild : ParserRuleContext
        {
            assert.ErrorSyntaxFormatArgsEqual(exception,
                nameof(Localized_Python3_Parser.Ex_Syntax_UnexpectedChildType),
                startTokenMock.Object, stopTokenMock.Object,
                Python3Parser.ruleNames[context.Object.RuleIndex],
                Python3Parser.ruleNames[child.RuleIndex]);
        }

        public static void ErrorUnexpectedChildTypeFormatArgs<TContext>(this Assert assert,
            SyntaxException exception, Mock<TContext> context, ITerminalNode child)
            where TContext : ParserRuleContext
        {
            assert.ErrorSyntaxFormatArgsEqual(exception,
                nameof(Localized_Python3_Parser.Ex_Syntax_UnexpectedChildType),
                child,
                Python3Parser.ruleNames[context.Object.RuleIndex],
                child.Symbol.Text);
        }

        public static void ErrorExpectedChildFormatArgs<TContext>(this Assert assert,
            SyntaxException exception, Mock<IToken> startTokenMock, Mock<IToken> stopTokenMock,
            Mock<TContext> context)
            where TContext : ParserRuleContext
        {
            assert.ErrorSyntaxFormatArgsEqual(exception,
                nameof(Localized_Python3_Parser.Ex_Syntax_ExpectedChild),
                startTokenMock.Object, stopTokenMock.Object,
                Python3Parser.ruleNames[context.Object.RuleIndex]);
        }

        public static void ErrorNotYetImplFormatArgs(this Assert assert,
            SyntaxNotYetImplementedException exception, Mock<IToken> startTokenMock, Mock<IToken> stopTokenMock)
        {
            assert.ErrorSyntaxFormatArgsEqual(exception,
                nameof(Localized_Exceptions.Ex_Syntax_NotYetImplemented),
                startTokenMock.Object, stopTokenMock.Object);
        }

        public static void ErrorNotYetImplFormatArgs(this Assert assert,
            SyntaxNotYetImplementedException exception, ITerminalNode terminal)
        {
            assert.ErrorSyntaxFormatArgsEqual(exception,
                nameof(Localized_Exceptions.Ex_Syntax_NotYetImplemented),
                terminal);
        }

        public static void ErrorNotYetImplFormatArgs(this Assert assert,
            SyntaxNotYetImplementedExceptionKeyword exception, Mock<IToken> startTokenMock, Mock<IToken> stopTokenMock,
            string expectedKeyword)
        {
            assert.ErrorSyntaxFormatArgsEqual(exception,
                nameof(Localized_Python3_Parser.Ex_Syntax_NotYetImplemented_Keyword),
                startTokenMock.Object, stopTokenMock.Object,
                expectedKeyword);
        }

        public static void ErrorNotYetImplFormatArgs(this Assert assert,
            SyntaxNotYetImplementedExceptionKeyword exception, ITerminalNode terminal, string keyword)
        {
            assert.ErrorSyntaxFormatArgsEqual(exception,
                nameof(Localized_Python3_Parser.Ex_Syntax_NotYetImplemented_Keyword),
                terminal, keyword);
        }

        public static void VerifyLoopedChildren<T>(this Mock<T> contextMock, int count)
            where T : ParserRuleContext
        {
            for (var i = 0; i < count; i++)
            {
                int index = i;
                contextMock.Verify(o => o.GetChild(index), Times.Once);
            }

            contextMock.Verify(o => o.GetChild(It.Is<int>(i => i < 0 || i >= count)), Times.Never);
        }
    }
}