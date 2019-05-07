using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language.Flow;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Operators.Arithmetics;

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitPowerTests : BaseBinaryOperatorTestClass<
        Python3Parser.PowerContext,
        Python3Parser.Atom_exprContext,
        ArithmeticPower
    >
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitPower(contextMock.Object);
        }

        public override ITerminalNode GetTerminalForThisClass()
        {
            return GetTerminal(Python3Parser.POWER);
        }

        public override ISetup<Grammar.SyntaxConstructor, SyntaxNode> RawSetupForInnerMock(
            Mock<Python3Parser.Atom_exprContext> innerMock)
        {
            return ctorMock.Setup(o => o.VisitAtom_expr(innerMock.Object));
        }

        [TestMethod]
        public override void Visit_MultipleOpsOrder_Test()
        {
            // Arrange
            var innerRuleMock = GetInnerMockWithSetup(GetExpressionMock());

            var unexpectedNode = GetTerminalForThisClass();

            contextMock.SetupChildren(
                innerRuleMock.Object,
                GetTerminalForThisClass(),
                GetMockRule<Python3Parser.FactorContext>().Object,
                unexpectedNode,
                innerRuleMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpectedNode);
            contextMock.VerifyLoopedChildren(4);

            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public override void Visit_MultipleRuleExcessNode_Test()
        {
            // Arrange
            var atomExprMock = GetInnerMockWithSetup(GetExpressionMock());
            var factorMock = GetMockRule<Python3Parser.FactorContext>();

            var unexpectedNode = GetTerminalForThisClass();

            contextMock.SetupChildren(
                atomExprMock.Object,
                GetTerminalForThisClass(),
                factorMock.Object,
                unexpectedNode,
                factorMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpectedNode);
            contextMock.VerifyLoopedChildren(4);

            atomExprMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public override void Visit_MultipleRuleSingleToken_Test()
        {
            // Arrange
            var expectedLhs = GetExpressionMock();
            var expectedRhs = GetExpressionMock();

            var innerRuleMock = GetInnerMockWithSetup(expectedLhs);
            var factorMock = GetMockRule<Python3Parser.FactorContext>();

            ctorMock.Setup(o => o.VisitFactor(factorMock.Object))
                .Returns(expectedRhs).Verifiable();

            contextMock.SetupChildren(
                innerRuleMock.Object,
                GetTerminalForThisClass(),
                factorMock.Object
            );

            // Act
            var result = VisitContext();

            // Assert
            Assert.That.IsBinaryOperator<ArithmeticPower>(expectedLhs, expectedRhs, result);

            contextMock.VerifyLoopedChildren(3);

            innerRuleMock.Verify();
            factorMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}