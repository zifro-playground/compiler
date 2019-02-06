using Antlr4.Runtime;
using Moq;

namespace Zifro.Compiler.Lang.Python3.Tests
{
    public static class MockingHelpers
    {
        private static Mock<IToken> tokenMock;

        public static void SetupForSourceReference<T>(this Mock<T> context)
            where T : ParserRuleContext
        {
            if (tokenMock == null)
            {
                tokenMock = new Mock<IToken>();
                tokenMock.SetupGet(o => o.Line).Returns(1);
                tokenMock.SetupGet(o => o.Column).Returns(1);
            }

            context.SetupGet(o => o.Start).Returns(tokenMock.Object);
            context.SetupGet(o => o.Stop).Returns(tokenMock.Object);
        }
    }
}