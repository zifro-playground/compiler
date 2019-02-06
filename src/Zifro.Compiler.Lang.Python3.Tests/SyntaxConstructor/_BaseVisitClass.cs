using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Statements;

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor
{
    public class BaseVisitClass
    {
        // ReSharper disable InconsistentNaming
        protected Mock<Grammar.SyntaxConstructor> ctorMock;
        protected Grammar.SyntaxConstructor ctor;
        protected Mock<IToken> startTokenMock;

        protected Mock<IToken> stopTokenMock;
        // ReSharper restore InconsistentNaming

        protected static Mock<T> GetMockRule<T>() where T : ParserRuleContext
        {
            return new Mock<T>(ParserRuleContext.EmptyContext, 0) { CallBase = true };
        }

        protected static ITerminalNode GetTerminal(int symbol)
        {
            var mock = new Mock<ITerminalNode>();
            mock.Setup(o => o.Symbol).Returns(GetSymbol(symbol));
            return mock.Object;
        }

        protected static IToken GetSymbol(int symbol)
        {
            var mock = new Mock<IToken>(MockBehavior.Strict);
            mock.SetupGet(o => o.Type).Returns(symbol);
            mock.SetupGet(o => o.Text).Returns(Python3Parser.DefaultVocabulary
                .GetLiteralName(symbol)?.Trim('\''));
            return mock.Object;
        }

        protected Statement GetStatementMock()
        {
            return new Mock<Statement>(MockBehavior.Strict, SourceReference.ClrSource, string.Empty).Object;
        }

        protected Statement GetAssignmentMock()
        {
            return new Mock<Assignment>(MockBehavior.Strict, SourceReference.ClrSource, string.Empty).Object;
        }

        protected StatementList GetStatementList(int count)
        {
            return new StatementList(SourceReference.ClrSource,
                new byte[count].Select(_ => GetStatementMock()).ToArray());
        }

        protected ExpressionNode GetExpressionMock()
        {
            return new Mock<ExpressionNode>(MockBehavior.Strict, SourceReference.ClrSource, string.Empty).Object;
        }

        [TestInitialize]
        public void TestInitialize()
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