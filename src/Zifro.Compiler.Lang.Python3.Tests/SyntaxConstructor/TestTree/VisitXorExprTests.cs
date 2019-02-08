using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language.Flow;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Operators;
using Zifro.Compiler.Lang.Python3.Syntax.Operators.Binaries;

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitXorExprTests : BaseBinaryOperatorTestClass<
            Python3Parser.Xor_exprContext,
            Python3Parser.And_exprContext,
            BinaryXor
        >
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitXor_expr(contextMock.Object);
        }

        public override ITerminalNode GetTerminalForThisClass()
        {
            return GetTerminal(Python3Parser.XOR);
        }

        public override ISetup<Grammar.SyntaxConstructor, SyntaxNode> RawSetupForInnerMock(
            Mock<Python3Parser.And_exprContext> innerMock)
        {
            return ctorMock.Setup(o => o.VisitAnd_expr(innerMock.Object));
        }
    }
}