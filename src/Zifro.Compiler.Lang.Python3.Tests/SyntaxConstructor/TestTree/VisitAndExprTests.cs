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
    public class VisitAndExprTests : BaseBinaryOperatorTestClass<
            Python3Parser.And_exprContext,
            Python3Parser.Shift_exprContext,
            BinaryAnd
        >
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitAnd_expr(contextMock.Object);
        }

        public override ITerminalNode GetTerminalForThisClass()
        {
            return GetTerminal(Python3Parser.AND_OP);
        }

        public override ISetup<Grammar.SyntaxConstructor, SyntaxNode> RawSetupForInnerMock(
            Mock<Python3Parser.Shift_exprContext> innerMock)
        {
            return ctorMock.Setup(o => o.VisitShift_expr(innerMock.Object));
        }
    }
}