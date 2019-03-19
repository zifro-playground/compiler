using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language.Flow;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Operators.Logicals;

// ReSharper disable ConvertToLocalFunction
// ReSharper disable SuggestVarOrType_Elsewhere
// ReSharper disable SuggestVarOrType_SimpleTypes

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public sealed class VisitOrTestTests
        : BaseBinaryOperatorTestClass<
            Python3Parser.Or_testContext,
            Python3Parser.And_testContext,
            LogicalOr
        >
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitOr_test(contextMock.Object);
        }

        public override ISetup<Grammar.SyntaxConstructor, SyntaxNode> RawSetupForInnerMock(Mock<Python3Parser.And_testContext> innerMock)
        {
            return ctorMock.Setup(o => o.VisitAnd_test(innerMock.Object));
        }

        public override ITerminalNode GetTerminalForThisClass()
        {
            return GetTerminal(Python3Parser.OR);
        }
    }
}