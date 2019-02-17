using System;
using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language.Flow;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Operators;
using Mellis.Lang.Python3.Syntax.Operators.Arithmetics;
using Mellis.Lang.Python3.Syntax.Operators.Binaries;

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitTermTests : BaseBinaryMultiOperatorTestClass<
            Python3Parser.TermContext,
            Python3Parser.FactorContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitTerm(contextMock.Object);
        }

        public override ISetup<Grammar.SyntaxConstructor, SyntaxNode> RawSetupForInnerMock(
            Mock<Python3Parser.FactorContext> innerMock)
        {
            return ctorMock.Setup(o => o.VisitFactor(innerMock.Object));
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            OperatorsAndExpectedTypes = new[]
            {
                (Python3Parser.STAR, typeof(ArithmeticMultiply)),
                //(Python3Parser.AT, typeof(?)),
                (Python3Parser.DIV, typeof(ArithmeticDivide)),
                (Python3Parser.MOD, typeof(ArithmeticModulus)),
                (Python3Parser.IDIV, typeof(ArithmeticFloor)),
            };
        }

        [DataTestMethod]
        [DataRow(Python3Parser.AT, "@", DisplayName = "nyi @")]
        public virtual void Visit_InvalidTokenNotYetImplemented_Test(int token, string expectedKeyword)
        {
            // Arrange
            var innerRuleMock = GetInnerMockWithSetup(GetExpressionMock());

            ITerminalNode unexpectedToken = GetTerminal(token);

            contextMock.SetupChildren(
                innerRuleMock.Object,
                unexpectedToken,
                innerRuleMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, unexpectedToken, expectedKeyword);
            // ~~Suppose error directly on token~~
            // Allow any number of iterations through children
            //contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}