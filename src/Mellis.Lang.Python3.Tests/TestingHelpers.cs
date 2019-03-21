using System;
using System.Linq;
using System.Linq.Expressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Resources;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Operators;
using Mellis.Lang.Python3.Tests.SyntaxConstructor;
using Mellis.Lang.Python3.Tests.TestingOps;

namespace Mellis.Lang.Python3.Tests
{
    public static class TestingHelpers
    {
        public static void CreateAndSetup<TSyntaxType>(this PyCompiler compiler,
            out Mock<TSyntaxType> nodeMock,
            out NopOp resultOp)
            where TSyntaxType : SyntaxNode
        {
            nodeMock = new Mock<TSyntaxType>(SourceReference.ClrSource);
            resultOp = new NopOp();
            NopOp exprOpRefCopy = resultOp;

            nodeMock.Setup(o => o.Compile(compiler))
                .Callback((PyCompiler c) => { c.Push(exprOpRefCopy); })
                .Verifiable();
        }

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

        public static ExpressionNode SetupExpressionMock(this Mock<Grammar.SyntaxConstructor> ctorMock,
            Expression<Func<Grammar.SyntaxConstructor, SyntaxNode>> setupExpression)
        {
            var expr = BaseVisitClass.GetExpressionMock();

            ctorMock.Setup(setupExpression).Returns(expr).Verifiable();

            return expr;
        }

        public static Identifier SetupIdentifier(this Mock<Grammar.SyntaxConstructor> ctorMock,
            Expression<Func<Grammar.SyntaxConstructor, SyntaxNode>> setupExpression,
            string identifierName)
        {
            var id = new Identifier(SourceReference.ClrSource, identifierName);

            ctorMock.Setup(setupExpression).Returns(id).Verifiable();

            return id;
        }

        public static Statement SetupStatementMock(this Mock<Grammar.SyntaxConstructor> ctorMock,
            Expression<Func<Grammar.SyntaxConstructor, SyntaxNode>> setupExpression)
        {
            var stmt = BaseVisitClass.GetStatementMock();

            ctorMock.Setup(setupExpression).Returns(stmt).Verifiable();

            return stmt;
        }

        public static void IsStatementListWithCount(this Assert assert, int expected, SyntaxNode resultNode)
        {
            Assert.IsInstanceOfType(resultNode, typeof(StatementList));
            Assert.AreEqual(expected, ((StatementList) resultNode).Statements.Count);
        }

        public static void IsStatementListContaining(this Assert assert, SyntaxNode resultNode,
            params Statement[] statements)
        {
            Assert.IsNotNull(resultNode, "Expected StatementList, got null.");
            Assert.IsInstanceOfType(resultNode, typeof(StatementList));
            var stmtList = (StatementList) resultNode;
            for (var i = 0; i < statements.Length; i++)
            {
                Assert.IsTrue(i < stmtList.Statements.Count,
                    $"Expected {statements.Length} statements, actual {stmtList.Statements.Count}.");
                Assert.AreSame(statements[i], stmtList.Statements[i],
                    $"Statements on index {i} did not match.\nExpected<{statements[i]?.GetType().FullName ?? "null"}>\nActual:<{stmtList.Statements[i]?.GetType().FullName ?? "null"}>");
            }

            Assert.AreEqual(statements.Length, ((StatementList) resultNode).Statements.Count,
                $"Expected {statements.Length} statements, actual {stmtList.Statements.Count}.");
        }

        public static void IsUnaryOperator(this Assert assert,
            Type expectedType,
            ExpressionNode expectedInner,
            SyntaxNode resultNode)
        {
            Assert.IsInstanceOfType(resultNode, expectedType);
            var op = (UnaryOperator) resultNode;
            Assert.AreSame(expectedInner, op.Operand);
        }

        public static void IsUnaryOperator<TOperator>(this Assert assert,
            ExpressionNode expectedInner,
            SyntaxNode resultNode)
            where TOperator : UnaryOperator
        {
            assert.IsUnaryOperator(typeof(TOperator), expectedInner, resultNode);
        }

        public static void IsBinaryOperator(this Assert assert,
            Type expectedType,
            ExpressionNode expectedLhs, ExpressionNode expectedRhs,
            SyntaxNode resultNode)
        {
            Assert.IsInstanceOfType(resultNode, expectedType);
            var op = (BinaryOperator) resultNode;
            Assert.AreSame(expectedLhs, op.LeftOperand);
            Assert.AreSame(expectedRhs, op.RightOperand);
        }

        public static void IsBinaryOperator<TOperator>(this Assert assert,
            ExpressionNode expectedLhs, ExpressionNode expectedRhs,
            SyntaxNode resultNode)
            where TOperator : BinaryOperator
        {
            assert.IsBinaryOperator(typeof(TOperator), expectedLhs, expectedRhs, resultNode);
        }

        public static ExpressionNode IsBinaryOperatorGetLhs(this Assert assert,
            Type expectedType,
            ExpressionNode expectedRhs,
            SyntaxNode resultNode)
        {
            Assert.IsInstanceOfType(resultNode, expectedType);
            var op = (BinaryOperator) resultNode;
            Assert.AreSame(expectedRhs, op.RightOperand);
            return op.LeftOperand;
        }

        public static ExpressionNode IsBinaryOperatorGetLhs<TOperator>(this Assert assert,
            ExpressionNode expectedRhs,
            SyntaxNode resultNode)
            where TOperator : BinaryOperator
        {
            return assert.IsBinaryOperatorGetLhs(typeof(TOperator), expectedRhs, resultNode);
        }

        public static ExpressionNode IsFunctionCall(this Assert assert,
            SyntaxNode result,
            ArgumentsList expectedArgumentsList)
        {
            Assert.IsInstanceOfType(result, typeof(FunctionCall));
            var resultFunc = (FunctionCall) result;
            Assert.AreSame(expectedArgumentsList, resultFunc.Arguments, "Function call arguments did not match.");
            return resultFunc.Operand;
        }

        /// <summary>
        /// Asserts the localized interpreter exception and uses the list of args
        /// when comparing formatting arguments.
        /// </summary>
        public static void ErrorFormatArgsEqual(this Assert assert,
            InterpreterLocalizedException exception, string expectedLocalizedKey,
            params object[] expectedArgs)
        {
            Assert.AreEqual(expectedLocalizedKey, exception.LocalizeKey, "\n\n" + exception);

            for (var i = 0; i < expectedArgs.Length; i++)
            {
                Assert.AreEqual(expectedArgs[i], exception.FormatArgs[i], $"index: {i}\n\n{exception}");
            }

            Assert.AreEqual(expectedArgs.Length, exception.FormatArgs.Length,
                $"Format args count differs. Relevant key: {expectedLocalizedKey}\n\n{exception}");
        }

        /// <summary>
        /// Asserts the syntax exception and uses the source reference from the start token
        /// <paramref name="startToken"/> and stop token <paramref name="stopToken"/>
        /// when comparing formatting arguments {0} to {3}, as well as optional additional args.
        /// </summary>
        public static void ErrorSyntaxFormatArgsEqual(this Assert assert,
            SyntaxException exception, string expectedLocalizedKey,
            IToken startToken, IToken stopToken,
            params object[] expectedExcessArgs)
        {
            object[] expected = new object[]
            {
                startToken.Line, startToken.Column,
                stopToken.Line, stopToken.Column
            }.Concat(expectedExcessArgs).ToArray();

            assert.ErrorFormatArgsEqual(exception, expectedLocalizedKey, expectedArgs: expected);
        }

        public static void ErrorSyntaxFormatArgsEqual(this Assert assert,
            SyntaxException exception, string expectedLocalizedKey,
            SourceReference source,
            params object[] expectedExcessArgs)
        {
            object[] expected = new object[]
            {
                source.FromRow, source.FromColumn,
                source.ToRow, source.ToColumn
            }.Concat(expectedExcessArgs).ToArray();

            assert.ErrorFormatArgsEqual(exception, expectedLocalizedKey, expectedArgs: expected);
        }

        /// <summary>
        /// Asserts the syntax exception and uses the source reference from the parameter <paramref name="token"/>
        /// when comparing formatting arguments {0} to {3}, as well as optional additional args.
        /// </summary>
        public static void ErrorSyntaxFormatArgsEqual(this Assert assert,
            SyntaxException exception, string expectedLocalizedKey,
            ITerminalNode token,
            params object[] expectedExcessArgs)
        {
            int stopOffset = token.Symbol.StartIndex == -1
                ? -1
                : token.Symbol.StopIndex - token.Symbol.StartIndex;

            object[] expected = new object[]
            {
                token.Symbol.Line, token.Symbol.Column,
                token.Symbol.Line, token.Symbol.Column + stopOffset
            }.Concat(expectedExcessArgs).ToArray();

            assert.ErrorFormatArgsEqual(exception, expectedLocalizedKey, expectedArgs: expected);
        }

        /// <summary>
        /// Asserts the syntax exception and uses the source reference from the start token
        /// <paramref name="startTokenMock"/> and stop token <paramref name="stopTokenMock"/>
        /// when comparing formatting arguments {0} to {3}, as well as rule names from
        /// the containing context <paramref name="context"/> on {4}
        /// and the rule name from the child context <paramref name="child"/> on {5}.
        /// </summary>
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
            SyntaxNotYetImplementedException exception, SourceReference source)
        {
            assert.ErrorFormatArgsEqual(exception,
                nameof(Localized_Exceptions.Ex_Syntax_NotYetImplemented),
                source.FromRow, source.FromColumn,
                source.ToRow, source.ToColumn);
        }

        public static void ErrorNotYetImplFormatArgs(this Assert assert,
            SyntaxNotYetImplementedException exception, Mock<IToken> startTokenMock, Mock<IToken> stopTokenMock)
        {
            assert.ErrorSyntaxFormatArgsEqual(exception,
                nameof(Localized_Exceptions.Ex_Syntax_NotYetImplemented),
                startTokenMock.Object, stopTokenMock.Object);
        }

        public static void ErrorNotYetImplFormatArgs(this Assert assert,
            SyntaxNotYetImplementedException exception, ITerminalNode node)
        {
            assert.ErrorSyntaxFormatArgsEqual(exception,
                nameof(Localized_Exceptions.Ex_Syntax_NotYetImplemented),
                node);
        }

        public static void ErrorNotYetImplFormatArgs(this Assert assert,
            SyntaxNotYetImplementedExceptionKeyword exception, SourceReference source, string keyword)
        {
            assert.ErrorFormatArgsEqual(exception,
                nameof(Localized_Python3_Parser.Ex_Syntax_NotYetImplemented_Keyword),
                source.FromRow, source.FromColumn,
                source.ToRow, source.ToColumn,
                keyword);
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