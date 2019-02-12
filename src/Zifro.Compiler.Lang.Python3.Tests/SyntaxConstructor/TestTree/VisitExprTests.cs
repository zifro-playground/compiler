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
    public class VisitExprTests : BaseBinaryOperatorTestClass<
            Python3Parser.ExprContext,
            Python3Parser.Xor_exprContext,
            BinaryOr
        >
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitExpr(contextMock.Object);
        }

        public override ITerminalNode GetTerminalForThisClass()
        {
            return GetTerminal(Python3Parser.OR_OP);
        }

        public override ISetup<Grammar.SyntaxConstructor, SyntaxNode> RawSetupForInnerMock(Mock<Python3Parser.Xor_exprContext> innerMock)
        {
            return ctorMock.Setup(o => o.VisitXor_expr(innerMock.Object));
        }
    }
}