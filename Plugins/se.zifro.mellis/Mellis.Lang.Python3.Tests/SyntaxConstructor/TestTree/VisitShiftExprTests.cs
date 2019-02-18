using System;
using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language.Flow;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Operators;
using Mellis.Lang.Python3.Syntax.Operators.Binaries;

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitShiftExprTests : BaseBinaryMultiOperatorTestClass<
            Python3Parser.Shift_exprContext,
            Python3Parser.Arith_exprContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitShift_expr(contextMock.Object);
        }

        public override ISetup<Grammar.SyntaxConstructor, SyntaxNode> RawSetupForInnerMock(
            Mock<Python3Parser.Arith_exprContext> innerMock)
        {
            return ctorMock.Setup(o => o.VisitArith_expr(innerMock.Object));
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            OperatorsAndExpectedTypes = new[]
            {
                (Python3Parser.LEFT_SHIFT, typeof(BinaryLeftShift)),
                (Python3Parser.RIGHT_SHIFT, typeof(BinaryRightShift))
            };
        }
    }
}