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
    public class VisitExprTests : BaseBinaryOperatorTestClass<
            Python3Parser.ExprContext,
            Python3Parser.Xor_exprContext,
            BinaryXor
        >
    {
        public override SyntaxNode VisitContext()
        {
            throw new System.NotImplementedException();
        }

        public override ITerminalNode GetTerminalForThisClass()
        {
            throw new System.NotImplementedException();
        }

        public override ISetup<Grammar.SyntaxConstructor, SyntaxNode> RawSetupForInnerMock(Mock<Python3Parser.Xor_exprContext> innerMock)
        {
            throw new System.NotImplementedException();
        }
    }
}