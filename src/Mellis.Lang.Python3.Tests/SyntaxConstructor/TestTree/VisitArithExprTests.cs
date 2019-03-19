using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language.Flow;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Operators.Arithmetics;

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitArithExprTests : BaseBinaryMultiOperatorTestClass<
            Python3Parser.Arith_exprContext,
            Python3Parser.TermContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitArith_expr(contextMock.Object);
        }

        public override ISetup<Grammar.SyntaxConstructor, SyntaxNode> RawSetupForInnerMock(
            Mock<Python3Parser.TermContext> innerMock)
        {
            return ctorMock.Setup(o => o.VisitTerm(innerMock.Object));
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            OperatorsAndExpectedTypes = new[]
            {
                (Python3Parser.ADD, typeof(ArithmeticAdd)),
                (Python3Parser.MINUS, typeof(ArithmeticSubtract)),
            };
        }
    }
}