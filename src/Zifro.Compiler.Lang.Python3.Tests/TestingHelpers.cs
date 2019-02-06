﻿using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Resources;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Resources;
using Zifro.Compiler.Lang.Python3.Syntax;

namespace Zifro.Compiler.Lang.Python3.Tests
{
    public static class TestingHelpers
    {
        public static void SetupForSourceReference<T>(this Mock<T> context, Mock<IToken> tokenMock)
            where T : ParserRuleContext
        {
            context.SetupGet(o => o.Start).Returns(tokenMock.Object)
                .Verifiable($"Did not get start token from {typeof(T).Name}");
            context.SetupGet(o => o.Stop).Returns(tokenMock.Object)
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

        public static void ErrorFormatArgsEqual(this Assert assert,
            InterpreterLocalizedException exception, string expectedLocalizedKey,
            params object[] expectedArgs)
        {
            Assert.AreEqual(expectedLocalizedKey, exception.LocalizeKey);
            CollectionAssert.AreEqual(expectedArgs, exception.FormatArgs);

            Assert.AreEqual(expectedArgs.Length, exception.FormatArgs.Length);
            CollectionAssert.AreEqual(expectedArgs, exception.FormatArgs);
        }

        public static void ErrorSyntaxFormatArgsEqual(this Assert assert,
            SyntaxException exception, string expectedLocalizedKey,
            IToken startToken, IToken stopToken,
            params object[] expectedExcessArgs)
        {
            Assert.AreEqual(expectedLocalizedKey, exception.LocalizeKey);

            object[] expected = (new object[]
            {
                startToken.Line, startToken.Column,
                stopToken.Line, stopToken.Column
            }).Concat(expectedExcessArgs).ToArray();

            assert.ErrorFormatArgsEqual(exception, expectedLocalizedKey, expectedArgs: expected);
        }

        public static void ErrorUnexpectedChildTypeFormatArgs<TContext, TChild>(this Assert assert,
            SyntaxException exception, Mock<IToken> startTokenMock, Mock<IToken> stopTokenMock,
            Mock<TContext> context, Mock<TChild> childRule)
            where TContext : ParserRuleContext
            where TChild : ParserRuleContext
        {
            assert.ErrorSyntaxFormatArgsEqual(exception,
                nameof(Localized_Python3_Parser.Ex_Syntax_UnexpectedChildType),
                startTokenMock.Object, stopTokenMock.Object,
                Python3Parser.ruleNames[context.Object.RuleIndex],
                Python3Parser.ruleNames[childRule.Object.RuleIndex]);
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

        public static void ErrorNotYetImplFormatArgs<TContext>(this Assert assert,
            SyntaxException exception, Mock<IToken> startTokenMock, Mock<IToken> stopTokenMock,
            Mock<TContext> context)
            where TContext : ParserRuleContext
        {
            assert.ErrorSyntaxFormatArgsEqual(exception,
                nameof(Localized_Exceptions.Ex_Syntax_NotYetImplemented),
                startTokenMock.Object, stopTokenMock.Object,
                Python3Parser.ruleNames[context.Object.RuleIndex]);
        }

        public static void VerifyLoopedChildren<T>(this Mock<T> contextMock, int count)
            where T : ParserRuleContext
        {
            if (count <= 1)
                contextMock.VerifyGet(o => o.ChildCount, Times.AtMost(count + 1));
            else
                contextMock.VerifyGet(o => o.ChildCount, Times.Exactly(count + 1));

            for (var i = 0; i < count; i++)
            {
                int index = i;
                contextMock.Verify(o => o.GetChild(index), Times.Once);
            }
        }
    }
}