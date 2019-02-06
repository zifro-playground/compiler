using Antlr4.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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

        public static void IsStatementListWithCount(this Assert assert, int expected, SyntaxNode resultNode)
        {
            Assert.IsInstanceOfType(resultNode, typeof(StatementList));
            Assert.AreEqual(expected, ((StatementList)resultNode).Statements.Count);
        }

        public static void VerifyLoopedChildren<T>(this Mock<T> contextMock, int count)
            where T : ParserRuleContext
        {
            contextMock.VerifyGet(o => o.ChildCount, Times.Exactly(count + 1));

            for (var i = 0; i < count; i++)
            {
                int index = i;
                contextMock.Verify(o => o.GetChild(index), Times.Once);
            }
        }
    }
}